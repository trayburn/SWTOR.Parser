using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Analyzer
    {
        public IList<DpsPerCharacter> AnalyzeDpsPerCharacter(IList<LogEntry> log)
        {
            var list = new List<DpsPerCharacter>();
            var startTimestamp = log.First().timestamp;
            var query = from entry in log
                        where entry.@event.effect.name == "Damage"
                        group entry by (entry.timestamp - startTimestamp).TotalSeconds into second
                        select new { interval = Convert.ToInt32(second.Key), entries = second.ToList() };

            foreach (var i in query)
            {
                var query2 = from entry in i.entries
                             group entry by entry.source.name into character
                             select new DpsPerCharacter { interval = i.interval, character = character.Key, damage = character.Sum(x => x.@event.result.amount) };
                list.AddRange(query2);
            }

            return list.OrderBy(x => x.interval).ThenBy(x => x.character).ToList();
        }
    }
}