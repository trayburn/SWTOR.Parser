using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
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
}
