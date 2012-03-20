using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class Result : GameObject
    {
        public Result()
        {
            Type = "";
        }
        public int Amount { get; set; }
        public string Type { get; set; }
        public bool IsCritical { get; set; }
        public Result Mitigation { get; set; }
    }
}
