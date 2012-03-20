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
                    rest = ParseResult(entry, rest);
                    rest = ParseThreat(entry, rest);
                }
                catch
                {
                    Console.WriteLine("Error on line {0}", lineNumber);
                    Console.WriteLine(line);
                    throw;
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

        private string ParseResult(LogEntry entry, string line)
        {
            // ()
            // (3)
            // (1002 energy {836045448940874})
            // (131* elemental {836045448940875}) 
            // (1903* energy {836045448940874} (1903 absorbed {836045448945511}))
            var btwn = Between('(', ')', line);
            var rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
                if (btwn.FoundValue.Trim().Contains(" ") == false ||
                    btwn.FoundValue.EndsWith("-"))
                {
                    // Handle the (3) case
                    entry.Event.Result.IsCritical = btwn.FoundValue.Contains("*");
                    entry.Event.Result.Amount = Convert.ToInt32(btwn.FoundValue.Replace("*","").Replace("-",""));
                }
                else
                {
                    var splitMitigation = btwn.FoundValue.Split('(');
                    if (splitMitigation.GetUpperBound(0) > 0)
                    {
                        // handle (1903* energy {836045448940874} (1903 absorbed {836045448945511}))
                        // splits to "1903* energy {836045448940874} " 
                        //       and "1903 absorbed {836045448945511}"
                        ParseResultPart(entry.Event.Result, splitMitigation[0]);
                        entry.Event.Result.Mitigation = new Result();
                        ParseResultPart(entry.Event.Result.Mitigation, splitMitigation[1]);
                    }
                    else
                    {
                        // Handle the (1002 energy {836045448940874}) case
                        // also (131* elemental {836045448940875}) case, * is a critical
                        ParseResultPart(entry.Event.Result, btwn.FoundValue);
                    }
                }
            }

            return rest;
        }

        private void ParseResultPart(Result entry, string line)
        {
            //    1002 energy {836045448940874}
            // or 131* elemental {836045448940875}
            var btwn = Between('{', '}', line);
            entry.Number = Convert.ToInt64(btwn.FoundValue);
            var splitBefore = btwn.BeforeFound.Split(new[] { ' ' }, 2);
            entry.IsCritical = splitBefore[0].Contains("*");
            entry.Amount = Convert.ToInt32(splitBefore[0].Replace("*", ""));
            entry.Type = splitBefore[1].Trim();
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
            entry.Event.Effect.Number = Convert.ToInt64(btwn.FoundValue);
            btwn = Between('(', ')', btwn.BeforeFound);
            if (btwn.FoundValue != null)
            {
                // Handle subtype if present
                entry.Event.Effect.Name = btwn.BeforeFound.Trim();
                entry.Event.Effect.Subtype = btwn.FoundValue.Trim();
            }
            else
            {
                entry.Event.Effect.Name = btwn.Original.Trim();
                entry.Event.Effect.Subtype = string.Empty;
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
            else entry.Ability = new Ability();
            return rest;
        }

        private string ParseSourceAndTarget(LogEntry entry, string line)
        {
            var btwn = Between('[', ']', line);
            var rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
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
            }
            else entry.Source = new Actor();

            btwn = Between('[', ']', rest);
            rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
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
            }
            else entry.Target = new Actor();

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
}