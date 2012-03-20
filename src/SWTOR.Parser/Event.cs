using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Event : GameObject
    {
        public Event()
        {
            Effect = new Effect();
            Result = new Result();
        }

        public Effect Effect { get; set; }
        public Result Result { get; set; }
        public int Threat { get; set; }
    }
}
