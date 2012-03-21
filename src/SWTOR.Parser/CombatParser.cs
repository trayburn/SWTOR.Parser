using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class CombatParser
    {
        public CombatLog Parse(List<LogEntry> log)
        {
            var cLog = new CombatLog();
            CombatData currentCombat = null;

            foreach (LogEntry logEntry in log)
            {
                if (logEntry.@event.effect.name == "EnterCombat")
                    currentCombat = new CombatData();

                if (currentCombat != null)
                    currentCombat.Log.Add(logEntry);

                if (logEntry.@event.effect.name == "ExitCombat")
                {
                    cLog.Combats.Add(currentCombat);
                    currentCombat = null;
                }
            }

            Analyzer(cLog);

            return cLog;
        }

        private void Analyzer(CombatLog log)
        {
            var allCombatLogs = log.Combats.SelectMany(m => m.Log);
            log.TotalDamage = allCombatLogs.DamageEffects().Sum(m => m.@event.result.amount);
        }
    }

    public static class LogQueryExtensions
    {
        public static IEnumerable<LogEntry> DamageEffects(this IEnumerable<LogEntry> log)
        {
            return log.Where(m => m.@event.name == "ApplyEffect" && m.@event.effect.name == "Damage");
        }
    }

    public class CombatLog
    {
        public CombatLog()
        {
            Combats = new List<CombatData>();
        }

        public int TotalDamage { get; set; }
        public List<CombatData> Combats { get; private set; }
    }

    public class CombatData
    {
        public CombatData()
        {
            Log = new List<LogEntry>();
        }

        public List<LogEntry> Log { get; private set; }
    }
}
