using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public interface ICombatMetrics : ILogMetrics
    {
        int Interval { get; set; }
        double AverageDamagePerSecond { get; set; }
        double AverageHealingPerSecond { get; set; }
        double AverageThreatPerSecond { get; set; }
    }
}
