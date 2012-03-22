using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Parser
    {
        public List<LogEntry> Parse(System.IO.TextReader rdr)
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
                    entry.timestamp = Convert.ToDateTime(btwn.FoundValue);

                    rest = ParseSourceAndTarget(entry, btwn.Rest);
                    rest = ParseAbility(entry, rest);
                    rest = ParseEventAndEffect(entry, rest);
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
                entry.threat = Convert.ToInt32(btwn.FoundValue);
            else entry.threat = 0;
            return btwn.Rest;
        }

        private string ParseResult(LogEntry entry, string line)
        {
            // ()
            // (3)
            // (1002 energy {836045448940874})
            // (131* elemental {836045448940875}) 
            // (1903* energy {836045448940874} (1903 absorbed {836045448945511}))
            // (234 energy {836045448940874} -glance {836045448945509} (234 absorbed {836045448945511}))
            var btwn = Between('(', ')', line);
            var rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
                if (btwn.FoundValue.Trim().Contains(" ") == false ||
                    btwn.FoundValue.EndsWith("-"))
                {
                    // Handle the (3) case
                    entry.result.isCritical = btwn.FoundValue.Contains("*");
                    entry.result.amount = Convert.ToInt32(btwn.FoundValue.Replace("*","").Replace("-",""));
                }
                else
                {
                    string mitigation = null;
                    string defense = null;
                    string result = null;

                    var splitMitigation = btwn.FoundValue.Split('(');

                    if (splitMitigation.GetUpperBound(0) == 1)
                    {
                        mitigation = splitMitigation[1];
                        var splitDefense = splitMitigation[0].Split('-');
                        if (splitDefense.GetUpperBound(0) == 1)
                        {
                            defense = splitDefense[1];
                            result = splitDefense[0];
                        }
                        else result = splitDefense[0];
                    }
                    else result = splitMitigation[0];

                    ParseResultPart(entry.result, result);
                    ParseResultPart(entry.mitigation, mitigation);
                    ParseGameObject(entry.defense, defense);
                }
            }

            return rest;
        }

        private void ParseResultPart(Result entry, string line)
        {
            if (line == null) return;

            //    1002 energy {836045448940874}
            // or 131* elemental {836045448940875}
            var btwn = Between('{', '}', line);
            entry.number = Convert.ToInt64(btwn.FoundValue);
            var splitBefore = btwn.BeforeFound.Split(new[] { ' ' }, 2);
            entry.isCritical = splitBefore[0].Contains("*");
            entry.amount = Convert.ToInt32(splitBefore[0].Replace("*", ""));
            entry.name = splitBefore[1].Trim();
        }

        private void ParseGameObject(GameObject entry, string line)
        {
            if (line == null) return;

            var btwn = Between('{', '}', line);
            entry.name = btwn.BeforeFound.Trim();
            entry.number = Convert.ToInt64(btwn.FoundValue);
        }

        private string ParseEventAndEffect(LogEntry entry, string line)
        {
            // [ApplyEffect {836045448945477}: Stunned (Physical) {2848585519464704}]
            // [ApplyEffect {836045448945477}: Lucky Shots {1781496599806223}]
            var btwn = Between('[', ']', line);
            var splitFound = btwn.FoundValue.Split(new[] { ':' }, 2);
            var rest = btwn.Rest;

            btwn = Between('{', '}', splitFound[0]);
            entry.@event.name = btwn.BeforeFound.Trim();
            entry.@event.number = Convert.ToInt64(btwn.FoundValue);

            btwn = Between('{', '}', splitFound[1]);
            entry.effect.number = Convert.ToInt64(btwn.FoundValue);
            btwn = Between('(', ')', btwn.BeforeFound);
            if (btwn.FoundValue != null)
            {
                // Handle subtype if present
                entry.effect.name = btwn.BeforeFound.Trim();
                entry.effect.subtype = btwn.FoundValue.Trim();
            }
            else
            {
                entry.effect.name = btwn.Original.Trim();
                entry.effect.subtype = string.Empty;
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
                entry.ability.name = btwn.BeforeFound.Trim();
                entry.ability.number = Convert.ToInt64(btwn.FoundValue);
            }
            return rest;
        }

        private string ParseSourceAndTarget(LogEntry entry, string line)
        {
            var btwn = Between('[', ']', line);
            var rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
                btwn = Between('{', '}', btwn.FoundValue);
                entry.source.isPlayer = btwn.BeforeFound.StartsWith("@");
                entry.source.name = btwn.BeforeFound.Replace("@", "").Trim();
                entry.source.number = Convert.ToInt64(btwn.FoundValue);
            }

            btwn = Between('[', ']', rest);
            rest = btwn.Rest;
            if (string.IsNullOrWhiteSpace(btwn.FoundValue) == false)
            {
                btwn = Between('{', '}', btwn.FoundValue);
                entry.target.isPlayer = btwn.BeforeFound.StartsWith("@");
                entry.target.name = btwn.BeforeFound.Replace("@", "").Trim();
                entry.target.number = Convert.ToInt64(btwn.FoundValue);
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
                result.BeforeFound = line;
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