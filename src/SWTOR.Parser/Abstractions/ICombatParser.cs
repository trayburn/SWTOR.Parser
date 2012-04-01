using System;
using System.Linq;
using System.Collections.Generic;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser
{
    public interface ICombatParser
    {
        CombatLog Parse(List<LogEntry> log);
        void Clean(CombatLog log);
    }
}
