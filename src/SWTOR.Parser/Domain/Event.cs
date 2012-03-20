using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Event : GameObject
    {
        public Event()
        {
            effect = new Effect();
            result = new Result();
            result.mitigation = new Result();
        }

        public Effect effect { get; set; }
        public Result result { get; set; }
        public int threat { get; set; }
    }
}
