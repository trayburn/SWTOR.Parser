using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Effect : GameObject
    {
        public Effect()
        {
            subtype = "";
        }
        public string subtype { get; set; }
    }
}
