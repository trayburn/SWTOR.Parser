using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class CombatData : CombatMetrics
    {
        public CombatData()
        {
            Characters = new Dictionary<string, CharacterData>();
        }

        public Dictionary<string, CharacterData> Characters { get; private set; }
    }
}
