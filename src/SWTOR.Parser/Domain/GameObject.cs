using System;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class GameObject
    {
        public GameObject()
        {
            name = "";
        }
        public string name { get; set; }
        public Int64 number { get; set; }
    }
}
