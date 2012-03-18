using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class LogEntry
    {
        public LogEntry()
        {
            Source = new Actor();
            Target = new Actor();
            Event = new Event();
            Ability = new Ability();
        }

        public DateTime Timestamp { get; set; }
        public Actor Source { get; set; }
        public Actor Target { get; set; }
        public Event Event { get; set; }
        public Ability Ability { get; set; }
    }
}
