using System;
using System.Linq;
using System.Collections.Generic;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser
{
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
}
