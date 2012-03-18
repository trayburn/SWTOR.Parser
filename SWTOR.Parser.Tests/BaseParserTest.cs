using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public abstract class BaseParserTest
    {
        protected StringBuilder bldr;
        protected Parser target;
        protected StringReader rdr;

        [TestInitialize]
        public void SetUp()
        {
            target = new Parser();
            bldr = new StringBuilder();
            BuildTestString(bldr);
            rdr = new StringReader(bldr.ToString());
        }

        public abstract void BuildTestString(StringBuilder bldr);
    }
}
