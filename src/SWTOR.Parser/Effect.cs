using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Effect : GameObject
    {
        public Effect()
        {
            Subtype = "";
        }
        public string Subtype { get; set; }
    }
}
