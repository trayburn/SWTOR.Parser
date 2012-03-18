using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public class LogFileOne_ParserTests
    {
        private Stream logFileOne;
        private Parser target;
        private StreamReader rdr;

        [TestInitialize]
        public void SetUp()
        {
            target = new Parser();
            logFileOne = this.GetType().Assembly.GetManifestResourceStream("SWTOR.Parser.Tests.Samples.LogFileOne.txt");
            rdr = new StreamReader(logFileOne);
        }

        [TestMethod]
        public void Parse_Should_Return_2416_Entries()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            Assert.AreEqual(2416, list.Count);
        }
    }

    [TestClass]
    public class OneRowWithAbsorption_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:25:53] [@Idrurrez] [@Khantni] " +
                "[Saber Throw {812165430771712}] [ApplyEffect {836045448945477}: Damage {836045448945501}] " +
                "(1903* energy {836045448940874} (1903 absorbed {836045448945511})) <1903>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.Fail("Have not corrected this yet, or implemented correctly");
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 23, 49, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Elite Tastybobble", entry.Target.Name);
            Assert.AreEqual(846623953387520, entry.Target.Number);
            Assert.AreEqual(false, entry.Target.IsPlayer);
            Assert.AreEqual("Saber Throw", entry.Ability.Name);
            Assert.AreEqual(812165430771712, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Damage", entry.Event.EffectName);
            Assert.AreEqual("", entry.Event.EffectSubtype);
            Assert.AreEqual(836045448945501, entry.Event.EffectNumber);
            Assert.AreEqual(1002, entry.Event.Amount);
            Assert.AreEqual("energy", entry.Event.AmountType);
            Assert.AreEqual(836045448940874, entry.Event.AmountTypeNumber);
            Assert.AreEqual(1002, entry.Event.Threat);
        }
    }

    [TestClass]
    public class OneRowAmountTypeAndNumberAndMore_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:23:49] [@Idrurrez] [Elite Tastybobble {846623953387520}] [Saber Throw {812165430771712}]" + 
                " [ApplyEffect {836045448945477}: Damage {836045448945501}] (1002 energy {836045448940874}) <1002>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 23, 49, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Elite Tastybobble", entry.Target.Name);
            Assert.AreEqual(846623953387520, entry.Target.Number);
            Assert.AreEqual(false, entry.Target.IsPlayer);
            Assert.AreEqual("Saber Throw", entry.Ability.Name);
            Assert.AreEqual(812165430771712, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Damage", entry.Event.EffectName);
            Assert.AreEqual("", entry.Event.EffectSubtype);
            Assert.AreEqual(836045448945501, entry.Event.EffectNumber);
            Assert.AreEqual(1002, entry.Event.Amount);
            Assert.AreEqual("energy", entry.Event.AmountType);
            Assert.AreEqual(836045448940874, entry.Event.AmountTypeNumber);
            Assert.AreEqual(1002, entry.Event.Threat);
        }
    }

    [TestClass]
    public class OneRowNullAbility_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:23:49] [@Idrurrez] [@Idrurrez] [] [Restore {836045448945476}: focus point {836045448938496}] (3)");
        }


        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 23, 49, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Idrurrez", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual(null, entry.Ability);
            Assert.AreEqual("Restore", entry.Event.Name);
            Assert.AreEqual(836045448945476, entry.Event.Number);
            Assert.AreEqual("focus point", entry.Event.EffectName);
            Assert.AreEqual("", entry.Event.EffectSubtype);
            Assert.AreEqual(836045448938496, entry.Event.EffectNumber);
            Assert.AreEqual(3, entry.Event.Amount);
        }
    }

    [TestClass]
    public class OneRowNoSubtype_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:19:14] [@Idrurrez] [@Idrurrez] [Force Might {1781496599805952}] [ApplyEffect {836045448945477}: Lucky Shots {1781496599806223}] ()");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 19, 14, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Idrurrez", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual("Force Might", entry.Ability.Name);
            Assert.AreEqual(1781496599805952, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Lucky Shots", entry.Event.EffectName);
            Assert.AreEqual("", entry.Event.EffectSubtype);
            Assert.AreEqual(1781496599806223, entry.Event.EffectNumber);
            Assert.AreEqual(0, entry.Event.Amount);
        }
    }

    [TestClass]
    public class OneRow_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.Append("[03/01/2012 14:35:20] [@Idrurrez] [@Khantni] " +
                         "[Force Clap {2848585519464448}] " +
                         "[ApplyEffect {836045448945477}: Stunned (Physical) {2848585519464704}] ()");
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

