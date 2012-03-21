using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class DpsPerCharacter
    {
        public int interval { get; set; }
        public string character { get; set; }
        public int damage { get; set; }
        public double average { get; set; }
    }
}
