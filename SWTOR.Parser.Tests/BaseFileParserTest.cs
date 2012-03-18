using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public abstract class BaseFileParserTest
    {
        protected Stream fileStream;
        protected Parser target;
        protected StreamReader rdr;

        [TestInitialize]
        public void SetUp()
        {
            target = new Parser();
            fileStream = this.GetType().Assembly.GetManifestResourceStream("SWTOR.Parser.Tests.Samples." + FileName);
            rdr = new StreamReader(fileStream);
        }

        public abstract string FileName { get; }
        public abstract int NumberOfRecords { get; }

        [TestMethod]
        public void Parse_Should_Return_All_Entries()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            Assert.AreEqual(NumberOfRecords, list.Count);
        }
    }
}
