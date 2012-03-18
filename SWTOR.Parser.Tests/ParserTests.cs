using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public class OneRow_ParserTests
    {
        private StringBuilder oneRow;
        private Parser target;
        private StringReader rdr;

        [TestInitialize]
        public void SetUp()
        {
            target = new Parser();
            oneRow = new StringBuilder();
            oneRow.Append("[03/01/2012 14:35:20] [@Idrurrez] [@Khantni] " +
                          "[Force Clap {2848585519464448}] " +
                          "[ApplyEffect {836045448945477}: Stunned (Physical) {2848585519464704}] ()");
            rdr = new StringReader(oneRow.ToString());
        }

        [TestMethod]
        public void Parse_Should_Return_Zero_Event_Amount()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.Event.Amount, 0);
        }

        [TestMethod]
        public void Parse_Should_Return_ForceClap_Ability()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.Ability.Name, "Force Clap");
            Assert.AreEqual(entry.Ability.Number, 2848585519464448);
        }

        [TestMethod]
        public void Parse_Should_Return_ApplyEffect_Event()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.Event.Name, "ApplyEffect");
            Assert.AreEqual(entry.Event.Number, 836045448945477);
            Assert.AreEqual(entry.Event.EffectName, "Stunned");
            Assert.AreEqual(entry.Event.EffectSubtype, "Physical");
            Assert.AreEqual(entry.Event.EffectNumber, 2848585519464704);
        }

        [TestMethod]
        public void Parse_Should_Return_Player_Khantni_As_Target()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual("Khantni", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
        }

        [TestMethod]
        public void Parse_Should_Return_Player_Idrurrez_As_Source()
        {
            // Arrange

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
        }

        [TestMethod]
        public void Parse_Should_Return_One_Entry()
        {
            // Arrange

            // Act
            var list = target.Parse(rdr);

            // Assert
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void Parse_Should_Return_Correct_Timestamp()
        {
            // Arrange

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            var expected = new DateTime(2012, 3, 1, 14, 35, 20, DateTimeKind.Unspecified);
            Assert.AreEqual(expected, entry.Timestamp);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Parse_Should_Throw_When_TextReader_Is_Null()
        {
            // Arrange

            // Act
            var list = target.Parse(null);

            // Assert
            Assert.IsNull(list);
            Assert.Fail();
        }
    }
}

