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
            int lineNumber = 0;
            var list = new List<LogEntry>();

            while ((line = rdr.ReadLine()) != null)
            {
                lineNumber += 1;
                var entry = new LogEntry();
                string rest = null;

                try
                {
                    var btwn = Between('[', ']', line);
                    entry.Timestamp = Convert.ToDateTime(btwn.FoundValue);

                    rest = ParseSourceAndTarget(entry, btwn.Rest);

                    rest = ParseAbility(entry, rest);

                    rest = ParseEvent(entry, rest);

                    rest = ParseEventAmount(entry, rest);

                    rest = ParseThreat(entry, rest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error on line {0}", lineNumber);
                    Console.WriteLine(line);
                }

                list.Add(entry);
            }

            return list;
        }

        private string ParseThreat(LogEntry entry, string line)
        {
            // <1002>
            var btwn = Between('<', '>', line);
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
                entry.Event.Threat = Convert.ToInt32(btwn.FoundValue);
            else entry.Event.Threat = 0;
            return btwn.Rest;
        }

        private string ParseEventAmount(LogEntry entry, string line)
        {
            // ()
            // (3)
            // (1002 energy {836045448940874})
            // (131* elemental {836045448940875}) 
            var btwn = Between('(', ')', line);
            var rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
                if (btwn.FoundValue.Trim().Contains(" ") == false)
                {
                    // Handle the (3) case
                    entry.Event.Amount = Convert.ToInt32(btwn.FoundValue);
                }
                else
                {
                    // Handle the (1002 energy {836045448940874}) case
                    // also (131* elemental {836045448940875}) case, ignoring * for now
                    btwn = Between('{', '}', btwn.FoundValue);
                    entry.Event.AmountTypeNumber = Convert.ToInt64(btwn.FoundValue);
                    var splitBefore = btwn.BeforeFound.Split(new[] { ' ' }, 2);
                    entry.Event.Amount = Convert.ToInt32(splitBefore[0]); //.Replace("*","")
                    entry.Event.AmountType = splitBefore[1].Trim();
                }
            }
            else
            {
                // Handle the () case
                entry.Event.Amount = 0;
                entry.Event.AmountType = null;
                entry.Event.AmountTypeNumber = 0;
            }
            return rest;
        }

        private string ParseEvent(LogEntry entry, string line)
        {
            // [ApplyEffect {836045448945477}: Stunned (Physical) {2848585519464704}]
            // [ApplyEffect {836045448945477}: Lucky Shots {1781496599806223}]
            var btwn = Between('[', ']', line);
            var splitFound = btwn.FoundValue.Split(':');
            var rest = btwn.Rest;

            btwn = Between('{', '}', splitFound[0]);
            entry.Event.Name = btwn.BeforeFound.Trim();
            entry.Event.Number = Convert.ToInt64(btwn.FoundValue);

            btwn = Between('{', '}', splitFound[1]);
            entry.Event.EffectNumber = Convert.ToInt64(btwn.FoundValue);
            btwn = Between('(', ')', btwn.BeforeFound);
            if (btwn.FoundValue != null)
            {
                // Handle subtype if present
                entry.Event.EffectName = btwn.BeforeFound.Trim();
                entry.Event.EffectSubtype = btwn.FoundValue.Trim();
            }
            else
            {
                entry.Event.EffectName = btwn.Original.Trim();
                entry.Event.EffectSubtype = string.Empty;
            }

            return rest;
        }
        private string ParseAbility(LogEntry entry, string line)
        {
            // [Force Clap {2848585519464448}]
            var btwn = Between('[', ']', line);
            var rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
                btwn = Between('{', '}', btwn.FoundValue);
                entry.Ability.Name = btwn.BeforeFound.Trim();
                entry.Ability.Number = Convert.ToInt64(btwn.FoundValue);
            }
            else entry.Ability = null;
            return rest;
        }

        private string ParseSourceAndTarget(LogEntry entry, string line)
        {
            var btwn = Between('[', ']', line);
            var rest = btwn.Rest;
            if (btwn.FoundValue.StartsWith("@"))
            {
                entry.Source.IsPlayer = true;
                entry.Source.Name = btwn.FoundValue.Replace("@", "");
            }
            else
            {
                btwn = Between('{', '}', btwn.FoundValue);
                entry.Source.Name = btwn.BeforeFound.Trim();
                entry.Source.Number = Convert.ToInt64(btwn.FoundValue);
                entry.Source.IsPlayer = false;
            }

            btwn = Between('[', ']', rest);
            rest = btwn.Rest;
            if (btwn.FoundValue.StartsWith("@"))
            {
                entry.Target.IsPlayer = true;
                entry.Target.Name = btwn.FoundValue.Replace("@", "");
            }
            else
            {
                btwn = Between('{', '}', btwn.FoundValue);
                entry.Target.Name = btwn.BeforeFound.Trim();
                entry.Target.Number = Convert.ToInt64(btwn.FoundValue);
                entry.Target.IsPlayer = false;
            }

            return rest;
        }

        private BetweenResult Between(char startChar, char endChar, string line)
        {
            var result = new BetweenResult();

            result.Original = line;
            result.StartOfFound = line.IndexOf(startChar) + 1;
            result.EndOfFound = line.IndexOf(endChar, result.StartOfFound);

            if (result.EndOfFound == -1)
            {
                result.Rest = line;
                result.FoundValue = null;
                result.BeforeFound = null;
            }
            else
            {
                result.FoundValue = line.Substring(result.StartOfFound, result.EndOfFound - result.StartOfFound);
                result.Rest = line.Substring(result.EndOfFound);
                result.BeforeFound = line.Substring(0, result.StartOfFound - 1);
            }

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
        public Int64 Number { get; set; }
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
        public string AmountType { get; set; }
        public Int64 AmountTypeNumber { get; set; }
        public int Threat { get; set; }
    }
}

