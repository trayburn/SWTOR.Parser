using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public interface ILogMetrics
    {
        List<AbilityMetrics> AbilityCounts { get; set; }
        int TotalDamage { get; set; }
        int TotalHealing { get; set; }
        int TotalThreat { get; set; }
        int CountOfParry { get; set; }
        int CountOfDeflect { get; set; }
    }
}
