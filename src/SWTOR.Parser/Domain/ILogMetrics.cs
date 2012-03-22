using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public interface ILogMetrics
    {
        int TotalDamage { get; set; }
        int TotalHealing { get; set; }
        int CountOfParry { get; set; }
        int CountOfDeflect { get; set; }
    }
}
