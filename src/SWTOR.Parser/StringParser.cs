using System;
using System.Collections.Generic;
using System.IO;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser
{
    public class StringParser : IStringParser
    {
        private IParser parser;

        public StringParser(IParser parser)
        {
            this.parser = parser;
        }

        public List<LogEntry> ParseString(string val)
        {
            var parser = new SWTOR.Parser.Parser();
            return parser.Parse(new StringReader(val));
        }
    }
}
