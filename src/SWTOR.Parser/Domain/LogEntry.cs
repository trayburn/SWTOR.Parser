using System;
using System.Collections.Generic;

namespace SWTOR.Parser.Domain
{
    public class LogEntry
    {
        public LogEntry()
        {
            source = new Actor();
            target = new Actor();
            @event = new GameObject();
            ability = new GameObject();
            effect = new Effect();
            result = new Result();
            mitigation = new Result();
            defense = new GameObject();
        }

        public DateTime timestamp { get; set; }
        public Actor source { get; set; }
        public Actor target { get; set; }
        public GameObject ability { get; set; }
        public GameObject @event { get; set; }
        public Effect effect { get; set; }
        public Result result { get; set; }
        public GameObject defense { get; set; }
        public Result mitigation { get; set; }
        public int threat { get; set; }
    }
}
