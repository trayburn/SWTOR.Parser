using System;
using System.Collections.Generic;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser
{
    public interface IParser
    {
        List<LogEntry> Parse(System.IO.TextReader rdr);
    }
}
