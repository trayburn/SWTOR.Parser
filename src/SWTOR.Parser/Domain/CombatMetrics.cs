using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class CombatMetrics : ICombatMetrics
    {
        public CombatMetrics()
        {
            Log = new List<LogEntry>();
            AbilityMetrics = new List<AbilityMetrics>();
        }

        public List<LogEntry> Log { get; private set; }

        public int TotalDamage { get; set; }
        public int TotalHealing { get; set; }
        public int TotalThreat { get; set; }
        public int CountOfParry { get; set; }
        public int CountOfDeflect { get; set; }
        public List<AbilityMetrics> AbilityMetrics { get; set; }

        public int Interval { get; set; }
        public double AverageDamagePerSecond { get; set; }
        public double AverageHealingPerSecond { get; set; }
        public double AverageThreatPerSecond { get; set; }
    }
}
