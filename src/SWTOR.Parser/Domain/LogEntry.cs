using System;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public class LogEntry
    {
        public LogEntry()
        {
            source = new Actor();
            target = new Actor();
            @event = new Event();
            ability = new Ability();
        }

        public DateTime timestamp { get; set; }
        public Actor source { get; set; }
        public Actor target { get; set; }
        public Event @event { get; set; }
        public Ability ability { get; set; }
    }
}
