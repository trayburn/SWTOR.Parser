using System;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class Result : GameObject
    {
        public int amount { get; set; }
        public bool isCritical { get; set; }
    }
}
