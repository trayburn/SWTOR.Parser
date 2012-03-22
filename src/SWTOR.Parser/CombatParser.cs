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

            var allCombatLogs = cLog.Combats.SelectMany(m => m.Log);
            Analyzer(cLog, allCombatLogs);

            foreach (var combat in cLog.Combats)
            {
                Analyzer(combat, combat.Log);
            }

            return cLog;
        }

        private void Analyzer(ICombatMetrics data, IEnumerable<LogEntry> log)
        {
            data.TotalDamage = log.DamageEffects().Sum(m => m.@event.result.amount);
            data.TotalHealing = log.HealingEffects().Sum(m => m.@event.result.amount);
            data.CountOfParry = log.ParryEffects().Count();
            data.CountOfDeflect = log.DeflectEffects().Count();
        }
    }

    public static class LogQueryExtensions
    {
        public static IEnumerable<LogEntry> DamageEffects(this IEnumerable<LogEntry> log)
        {
            return log.Where(m => m.@event.name == "ApplyEffect" && m.@event.effect.name == "Damage");
        }

        public static IEnumerable<LogEntry> HealingEffects(this IEnumerable<LogEntry> log)
        {
            return log.Where(m => m.@event.name == "ApplyEffect" && m.@event.effect.name == "Heal");
        }

        public static IEnumerable<LogEntry> ParryEffects(this IEnumerable<LogEntry> log)
        {
            return log.DamageEffects().Where(m => m.@event.result.type == "-parry");
        }

        public static IEnumerable<LogEntry> DeflectEffects(this IEnumerable<LogEntry> log)
        {
            return log.DamageEffects().Where(m => m.@event.result.type == "-deflect");
        }
    }

    public class CombatLog : ICombatMetrics
    {
        public CombatLog()
        {
            Combats = new List<CombatData>();
        }

        public List<CombatData> Combats { get; private set; }

        public int TotalDamage { get; set; }
        public int TotalHealing { get; set; }
        public int CountOfParry { get; set; }
        public int CountOfDeflect { get; set; }
    }

    public class CombatData : ICombatMetrics
    {
        public CombatData()
        {
            Log = new List<LogEntry>();
        }

        public List<LogEntry> Log { get; private set; }

        public int TotalDamage { get; set; }
        public int TotalHealing { get; set; }
        public int CountOfParry { get; set; }
        public int CountOfDeflect { get; set; }
    }

    public interface ICombatMetrics
    {
        int TotalDamage { get; set; }
        int TotalHealing { get; set; }
        int CountOfParry { get; set; }
        int CountOfDeflect { get; set; }
    }
}
