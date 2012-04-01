using System;
using System.Collections.Generic;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser
{
    public interface IStringParser
    {
        List<LogEntry> ParseString(string val);
    }
}
