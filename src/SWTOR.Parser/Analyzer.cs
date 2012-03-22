using System;
using System.Linq;
using System.Collections.Generic;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser
{
    public class Analyzer
    {
        public IList<DpsPerCharacter> AnalyzeDpsPerCharacter(IList<LogEntry> log)
        {
            var list = new List<DpsPerCharacter>();
            var startTimestamp = log.First().timestamp;
            var query = from entry in log
                        where entry.effect.name == "Damage"
                        group entry by (entry.timestamp - startTimestamp).TotalSeconds into second
                        select new { interval = Convert.ToInt32(second.Key), entries = second.ToList() };

            foreach (var i in query)
            {
                var query2 = from entry in i.entries
                             group entry by entry.source.name into character
                             select new DpsPerCharacter { interval = i.interval, character = character.Key, damage = character.Sum(x => x.result.amount) };
                list.AddRange(query2);
            }

            list = FillEmptyDataPoints(list);
            list = CalculateRollingAverage(list);

            return list.OrderBy(x => x.interval).ThenBy(x => x.character).ToList();
        }

        private List<DpsPerCharacter> CalculateRollingAverage(List<DpsPerCharacter> list)
        {
            return list;
        }

        private List<DpsPerCharacter> FillEmptyDataPoints(List<DpsPerCharacter> list)
        {
            var allCharacters = list.Select(m => m.character).Distinct().OrderBy(m => m).ToList();
            var allIntervals = list.Select(m => m.interval).Distinct().OrderBy(m => m).ToList();

            // Fill in missing intervals
            var lastInterval = allIntervals.First();
            foreach (var interval in allIntervals.OrderBy(m => m))
            {
                int diff = interval - lastInterval;
                if (diff > 1)
                {
                    for (int i = 1; i < diff; i++)
                    {
                        foreach (var character in allCharacters)
                        {
                            list.Add(new DpsPerCharacter { character = character, damage = 0, interval = lastInterval + i });
                        }
                    }
                }

                foreach (var character in allCharacters)
                {
                    if (list.Any(m => m.character == character && m.interval == interval) == false)
                    {
                        list.Add(new DpsPerCharacter { character = character, interval = interval, damage = 0 });
                    }
                }

                lastInterval = interval;
            }

            return list;
        }
    }
}