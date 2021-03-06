using System;
using System.Linq;
using System.Collections.Generic;
using SWTOR.Parser.Domain;
using System.Security.Cryptography;
using System.Text;

namespace SWTOR.Parser
{
    public class CombatParser : ICombatParser
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

        public void Clean(CombatLog log)
        {
            // Clear log properties so they won't get stores/used later.
            foreach (CombatData combat in log.Combats)
            {
                combat.Log.Clear();
                foreach (CharacterData value in combat.Characters.Values)
                {
                    value.AsSource.Log.Clear();
                    value.AsTarget.Log.Clear();
                }
            }
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
            data.AverageThreatPerSecond = (double)data.TotalThreat / data.Interval;
        }

        private void LogAnalyzer(ILogMetrics data, IEnumerable<LogEntry> log)
        {
            data.TotalDamage = log.DamageEffects().Sum(m => m.result.amount);
            data.TotalHealing = log.HealingEffects().Sum(m => m.result.amount);
            data.TotalThreat = log.ThreatEffects().Sum(m => m.threat);
            data.CountOfParry = log.ParryEffects().Count();
            data.CountOfDeflect = log.DeflectEffects().Count();

            AbilityAnalyzer(data, log);
        }

        private static void AbilityAnalyzer(ILogMetrics data, IEnumerable<LogEntry> log)
        {
            var abilityNames = log.Where(m => m.ability.name != "").Select(m => m.ability.name).Distinct();
            foreach (var name in abilityNames)
            {
                var metrics = new AbilityMetrics();
                var abilityLog = log.Where(m => m.ability.name == name);

                var damageLog = abilityLog.DamageEffects();
                var healingLog = abilityLog.HealingEffects();
                var threatLog = abilityLog.ThreatEffects();

                data.AbilityMetrics.Add(metrics);

                metrics.Name = name;
                metrics.Number = abilityLog.First().ability.number;
                metrics.Count = abilityLog.Count();
                
                metrics.MaximumDamage = damageLog.IfEmpty(0, l => l.Max(m => m.result.amount));
                metrics.MinimumDamage = damageLog.IfEmpty(0, l => l.Min(m => m.result.amount));
                metrics.AverageDamage = damageLog.IfEmpty(0, l => l.Average(m => m.result.amount));

                metrics.MaximumHealing = healingLog.IfEmpty(0, l => l.Max(m => m.result.amount));
                metrics.MinimumHealing = healingLog.IfEmpty(0, l => l.Min(m => m.result.amount));
                metrics.AverageHealing = healingLog.IfEmpty(0, l => l.Average(m => m.result.amount));

                metrics.MaximumThreat = threatLog.IfEmpty(0, l => l.Max(m => m.result.amount));
                metrics.MinimumThreat = threatLog.IfEmpty(0, l => l.Min(m => m.result.amount));
                metrics.AverageThreat = threatLog.IfEmpty(0, l => l.Average(m => m.result.amount));

                metrics.CountOfCriticals = abilityLog.Where(m => m.result.isCritical).Count();
            }
            data.AbilityMetrics = data.AbilityMetrics.OrderByDescending(m => m.Count).ThenBy(m => m.Name).ToList();
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

                if (logEntry.effect.name == "ExitCombat" && currentCombat != null)
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
}