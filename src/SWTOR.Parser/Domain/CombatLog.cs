using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class CombatLog : ILogMetrics
    {
        public CombatLog()
        {
            Combats = new List<CombatData>();
            AbilityCounts = new List<AbilityCount>();
        }

        public List<CombatData> Combats { get; private set; }

        public int TotalDamage { get; set; }
        public int TotalHealing { get; set; }
        public int TotalThreat { get; set; }
        public int CountOfParry { get; set; }
        public int CountOfDeflect { get; set; }
        public List<AbilityCount> AbilityCounts { get; set; }
    }
}
