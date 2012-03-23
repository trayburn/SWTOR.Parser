using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class AbilityMetrics
    {
        public string Name { get; set; }
        public long Number { get; set; }
        public int Count { get; set; }
        public double AverageDamage { get; set; }
        public int MaximumDamage { get; set; }
        public int MinimumDamage { get; set; }
        public int CountOfCriticals { get; set; }
    }
}
