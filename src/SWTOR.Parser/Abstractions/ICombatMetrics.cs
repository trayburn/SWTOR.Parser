using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public interface ICombatMetrics : ILogMetrics
    {
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        TimeSpan Interval { get; }
        double AverageDamagePerSecond { get; set; }
        double AverageHealingPerSecond { get; set; }
        double AverageThreatPerSecond { get; set; }
    }
}
