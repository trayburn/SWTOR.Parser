using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Result : GameObject
    {
        public Result()
        {
            type = "";
        }
        public int amount { get; set; }
        public string type { get; set; }
        public bool isCritical { get; set; }
        public Result mitigation { get; set; }
    }
}
