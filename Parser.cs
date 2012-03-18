using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Parser
    {
        public IList<LogEntry> Parse(System.IO.TextReader rdr)
        {
            if (rdr == null) throw new ArgumentNullException("rdr");

            string line = null;
            var list = new List<LogEntry>();

            while ((line = rdr.ReadLine()) != null)
            {
                var entry = new LogEntry();
                string rest = null;

                var btwn = Between('[', ']', line);
                entry.Timestamp = Convert.ToDateTime(btwn.FoundValue);

                rest = ParseSourceAndTarget(entry, btwn.Rest);

                rest = ParseAbility(entry, rest);

                rest = ParseEvent(entry, rest);

                rest = ParseEventAmount(entry, rest);

                list.Add(entry);
            }

            return list;
        }

        private string ParseEventAmount(LogEntry entry, string line)
        {
            //  ()
            var btwn = Between('(', ')', line);
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
                entry.Event.Amount = Convert.ToInt32(btwn.FoundValue);
            else entry.Event.Amount = 0;
            return btwn.Rest;
        }

        private string ParseEvent(LogEntry entry, string line)
        {
            // [ApplyEffect {836045448945477}: Stunned (Physical) {2848585519464704}]
            var btwn = Between('[', ']', line);
            var splitFound = btwn.FoundValue.Split(':');
            var rest = btwn.Rest;

            btwn = Between('{', '}', splitFound[0]);
            entry.Event.Name = btwn.BeforeFound.Trim();
            entry.Event.Number = Convert.ToInt64(btwn.FoundValue);

            btwn = Between('{', '}', splitFound[1]);
            entry.Event.EffectNumber = Convert.ToInt64(btwn.FoundValue);
            btwn = Between('(', ')', btwn.BeforeFound);
            entry.Event.EffectName = btwn.BeforeFound.Trim();
            entry.Event.EffectSubtype = btwn.FoundValue.Trim();

            return rest;
        }
        private string ParseAbility(LogEntry entry, string line)
        {
            // [Force Clap {2848585519464448}]
            var btwn = Between('[', ']', line);
            var rest = btwn.Rest;
            btwn = Between('{', '}', btwn.FoundValue);
            entry.Ability.Name = btwn.BeforeFound.Trim();
            entry.Ability.Number = Convert.ToInt64(btwn.FoundValue);
            return rest;
        }

        private string ParseSourceAndTarget(LogEntry entry, string line)
        {
            var btwn = Between('[', ']', line);
            entry.Source.IsPlayer = btwn.FoundValue.StartsWith("@");
            entry.Source.Name = btwn.FoundValue.Replace("@", "");

            btwn = Between('[', ']', btwn.Rest);
            entry.Target.IsPlayer = btwn.FoundValue.StartsWith("@");
            entry.Target.Name = btwn.FoundValue.Replace("@", "");

            return btwn.Rest;
        }

        private BetweenResult Between(char startChar, char endChar, string line)
        {
            var result = new BetweenResult();

            result.StartOfFound = line.IndexOf(startChar) + 1;
            result.EndOfFound = line.IndexOf(endChar, result.StartOfFound);

            result.FoundValue = line.Substring(result.StartOfFound, result.EndOfFound - result.StartOfFound);
            result.Rest = line.Substring(result.EndOfFound);
            result.BeforeFound = line.Substring(0, result.StartOfFound - 1);
            result.Original = line;

            return result;
        }

        class BetweenResult
        {
            public string FoundValue { get; set; }
            public string Rest { get; set; }
            public string Original { get; set; }
            public string BeforeFound { get; set; }
            public int StartOfFound { get; set; }
            public int EndOfFound { get; set; }
        }
    }

    public class LogEntry
    {
        public LogEntry()
        {
            Source = new Actor();
            Target = new Actor();
            Event = new Event();
            Ability = new Ability();
        }

        public DateTime Timestamp { get; set; }
        public Actor Source { get; set; }
        public Actor Target { get; set; }
        public Event Event { get; set; }
        public Ability Ability { get; set; }
    }

    public class Ability
    {
        public string Name { get; set; }
        public Int64 Number { get; set; }
    }

    public class Actor
    {
        public string Name { get; set; }
        public bool IsPlayer { get; set; }
    }

    public class Event
    {
        public string Name { get; set; }
        public Int64 Number { get; set; }
        public string EffectName { get; set; }
        public string EffectSubtype { get; set; }
        public Int64 EffectNumber { get; set; }
        public int Amount { get; set; }
        public int Threat { get; set; }
    }
}

