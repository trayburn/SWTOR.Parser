using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class CombatParser
    {
        public CombatLog Parse(List<LogEntry> log)
        {
            CombatLog cLog = ParseCombatLog(log);

            var allCombatLogs = cLog.Combats.SelectMany(m => m.Log);
            LogAnalyzer(cLog, allCombatLogs);

            foreach (var combat in cLog.Combats)
            {
                CharacterAnalyzer(combat, combat.Log);
                CombatAnalyzer(combat, combat.Log);
            }

            return cLog;
        }

        private void CharacterAnalyzer(CombatData combat, List<LogEntry> log)
        {
            var allSources = log.DistinctSources();
            var allTargets = log.DistinctTargets();
            var allCharacters = allSources.Union(allTargets).Distinct();

            foreach (var character in allCharacters)
            {
                var metrics = new CharacterData();
                combat.Characters.Add(character, metrics);

                CombatAnalyzer(metrics.AsSource, log.WithSource(character));
                CombatAnalyzer(metrics.AsTarget, log.WithTarget(character));
            }
        }

        private void CombatAnalyzer(ICombatMetrics data, IEnumerable<LogEntry> log)
        {
            if (log.Count() == 0) return;

            LogAnalyzer(data, log);
            data.Interval = Convert.ToInt32((log.EndTime() - log.StartTime()).TotalSeconds);
            data.AverageDamagePerSecond = (double)data.TotalDamage / data.Interval;
            data.AverageHealingPerSecond = (double)data.TotalHealing / data.Interval;
        }

        private void LogAnalyzer(ILogMetrics data, IEnumerable<LogEntry> log)
        {
            data.TotalDamage = log.DamageEffects().Sum(m => m.result.amount);
            data.TotalHealing = log.HealingEffects().Sum(m => m.result.amount);
            data.CountOfParry = log.ParryEffects().Count();
            data.CountOfDeflect = log.DeflectEffects().Count();
        }

        private CombatLog ParseCombatLog(List<LogEntry> log)
        {
            var cLog = new CombatLog();
            CombatData currentCombat = null;

            foreach (LogEntry logEntry in log)
            {
                if (logEntry.effect.name == "EnterCombat")
                    currentCombat = new CombatData();

                if (currentCombat != null)
                    currentCombat.Log.Add(logEntry);

                if (logEntry.effect.name == "ExitCombat")
                {
                    cLog.Combats.Add(currentCombat);
                    currentCombat = null;
                }
            }

            if (currentCombat != null) // we have a partial combat
                cLog.Combats.Add(currentCombat);
            return cLog;
        }
    }

    public static class LogQueryExtensions
    {
        public static IEnumerable<string> DistinctSources(this IEnumerable<LogEntry> log)
        {
            return log.Select(m => m.source.name).Distinct();
        }

        public static IEnumerable<string> DistinctTargets(this IEnumerable<LogEntry> log)
        {
            return log.Select(m => m.target.name).Distinct();
        }

        public static IEnumerable<LogEntry> WithSource(this IEnumerable<LogEntry> log, string source)
        {
            return log.Where(m => m.source.name == source);
        }

        public static IEnumerable<LogEntry> WithTarget(this IEnumerable<LogEntry> log, string target)
        {
            return log.Where(m => m.target.name == target);
        }

        public static IEnumerable<LogEntry> DamageEffects(this IEnumerable<LogEntry> log)
        {
            return log.Where(m => m.@event.name == "ApplyEffect" && m.effect.name == "Damage");
        }

        public static IEnumerable<LogEntry> HealingEffects(this IEnumerable<LogEntry> log)
        {
            return log.Where(m => m.@event.name == "ApplyEffect" && m.effect.name == "Heal");
        }

        public static IEnumerable<LogEntry> ParryEffects(this IEnumerable<LogEntry> log)
        {
            return log.DamageEffects().Where(m => m.result.name == "-parry");
        }

        public static IEnumerable<LogEntry> DeflectEffects(this IEnumerable<LogEntry> log)
        {
            return log.DamageEffects().Where(m => m.result.name == "-deflect");
        }

        public static DateTime StartTime(this IEnumerable<LogEntry> log)
        {
            return log.Min(m => m.timestamp);
        }

        public static DateTime EndTime(this IEnumerable<LogEntry> log)
        {
            return log.Max(m => m.timestamp);
        }
    }

    public class CombatLog : ILogMetrics
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

    public class CombatData : CombatMetrics
    {
        public CombatData()
        {
            Characters = new Dictionary<string, CharacterData>();
        }

        public Dictionary<string, CharacterData> Characters { get; private set; }
    }

    public class CombatMetrics : ICombatMetrics
    {
        public CombatMetrics()
        {
            Log = new List<LogEntry>();
        }

        public List<LogEntry> Log { get; private set; }

        public int TotalDamage { get; set; }
        public int TotalHealing { get; set; }
        public int CountOfParry { get; set; }
        public int CountOfDeflect { get; set; }

        public int Interval { get; set; }
        public double AverageDamagePerSecond { get; set; }
        public double AverageHealingPerSecond { get; set; }
    }

    public class CharacterData
    {
        public CharacterData()
        {
            AsSource = new CombatMetrics();
            AsTarget = new CombatMetrics();
        }

        public CombatMetrics AsSource { get; set; }
        public CombatMetrics AsTarget { get; set; }
    }

    public interface ILogMetrics
    {
        int TotalDamage { get; set; }
        int TotalHealing { get; set; }
        int CountOfParry { get; set; }
        int CountOfDeflect { get; set; }
    }

    public interface ICombatMetrics : ILogMetrics
    {
        int Interval { get; set; }
        double AverageDamagePerSecond { get; set; }
        double AverageHealingPerSecond { get; set; }
    }
}
