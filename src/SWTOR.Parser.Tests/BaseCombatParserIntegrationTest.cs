using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public abstract class BaseCombatParserIntegrationTest
    {
        protected CombatParser target;
        protected List<LogEntry> log;
        protected Parser parser;
        protected Stream stream;
        protected StreamReader rdr;

        [TestInitialize]
        public void SetUp()
        {
            target = new CombatParser();
            parser = new Parser();
            stream = this.GetType().Assembly.GetManifestResourceStream("SWTOR.Parser.Tests.Samples." + FileName);
            rdr = new StreamReader(stream);
            log = parser.Parse(rdr);
        }

        public abstract string FileName { get; }
    }
}
