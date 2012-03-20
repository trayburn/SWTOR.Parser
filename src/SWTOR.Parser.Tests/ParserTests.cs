using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public class PTRXXLargeLog_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "PTRXXLargeLog.txt"; }
        }
        public override int NumberOfRecords
        {
            get { return 10713; }
        }
    }

    [TestClass]
    public class PTRXLargeLog_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "PTRXLargeLog.txt"; }
        }
        public override int NumberOfRecords
        {
            get { return 9963; }
        }
    }

    [TestClass]
    public class PTRLargeLog_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "PTRLargeLog.txt"; }
        }

        public override int NumberOfRecords
        {
            get { return 5534; }
        }
    }

    [TestClass]
    public class PTRMediumLog_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "PTRMediumLog.txt"; }
        }

        public override int NumberOfRecords
        {
            get { return 3364; }
        }
    }

    [TestClass]
    public class PTRSmallLog_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "PTRSmallLog.txt"; }
        }

        public override int NumberOfRecords
        {
            get { return 1113; }
        }
    }

    [TestClass]
    public class LogFileThree_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "LogFileThree.txt"; }
        }

        public override int NumberOfRecords
        {
            get { return 3249; }
        }
    }

    [TestClass]
    public class LogFileTwo_ParserTests : BaseFileParserTest
    {

        public override string FileName
        {
            get { return "LogFileTwo.txt"; }
        }

        public override int NumberOfRecords
        {
            get { return 1429; }
        }

    }

    [TestClass]
    public class LogFileOne_ParserTests : BaseFileParserTest
    {
        public override string FileName
        {
            get { return "LogFileOne.txt"; }
        }

        public override int NumberOfRecords
        {
            get { return 2416; }
        }
    }

    [TestClass]
    public class OneRowWithZeroDamageButHangingDash_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:47:11] [@Twos] [@Khantni] " + 
                "[Affliction (Force) {808192586023173}] " + 
                "[ApplyEffect {836045448945477}: Damage {836045448945501}] (0 -) <209>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 47, 11, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Twos", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Khantni", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Affliction (Force)", entry.ability.name);
            Assert.AreEqual(808192586023173, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(836045448945501, entry.@event.effect.number);
            Assert.AreEqual(0, entry.@event.result.amount);
            Assert.AreEqual(false, entry.@event.result.isCritical);
            Assert.AreEqual("", entry.@event.result.type);
            Assert.AreEqual("", entry.@event.result.mitigation.name);
            Assert.AreEqual(209, entry.@event.threat);
        }
    }

    [TestClass]
    public class OneRowWithCritHeal_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:41:33] [@Twos] [@Twos] " + 
                "[Resurgence {808699392163840}] " + 
                "[ApplyEffect {836045448945477}: Heal {836045448945500}] (1485*)");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 41, 33, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Twos", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Twos", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Resurgence", entry.ability.name);
            Assert.AreEqual(808699392163840, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Heal", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(836045448945500, entry.@event.effect.number);
            Assert.AreEqual(1485, entry.@event.result.amount);
            Assert.AreEqual(true, entry.@event.result.isCritical);
            Assert.AreEqual("", entry.@event.result.type);
            Assert.AreEqual("", entry.@event.result.mitigation.name);
            Assert.AreEqual(0, entry.@event.threat);
        }
    }

    [TestClass]
    public class OneRowWithNoTarget_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:25:55] [@Idrurrez] [] " + 
                "[Seethe {808226945761280}] " + 
                "[Event {836045448945472}: AbilityInterrupt {836045448945482}] ()");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 25, 55, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Idrurrez", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("", entry.target.name);
            Assert.AreEqual("Seethe", entry.ability.name);
            Assert.AreEqual(808226945761280, entry.ability.number);
            Assert.AreEqual("Event", entry.@event.name);
            Assert.AreEqual(836045448945472, entry.@event.number);
            Assert.AreEqual("AbilityInterrupt", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(836045448945482, entry.@event.effect.number);
            Assert.AreEqual(0, entry.@event.result.amount);
            Assert.AreEqual(false, entry.@event.result.isCritical);
            Assert.AreEqual("", entry.@event.result.type);
            Assert.AreEqual("", entry.@event.result.mitigation.name);
            Assert.AreEqual(0, entry.@event.threat);
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 25, 53, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Idrurrez", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Khantni", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Saber Throw", entry.ability.name);
            Assert.AreEqual(812165430771712, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(836045448945501, entry.@event.effect.number);
            Assert.AreEqual(1903, entry.@event.result.amount);
            Assert.AreEqual(true, entry.@event.result.isCritical);
            Assert.AreEqual("energy", entry.@event.result.type);
            Assert.AreEqual(836045448940874, entry.@event.result.number);
            Assert.AreEqual(1903, entry.@event.result.mitigation.amount);
            Assert.AreEqual("absorbed", entry.@event.result.mitigation.type);
            Assert.AreEqual(836045448945511, entry.@event.result.mitigation.number);
            Assert.AreEqual(1903, entry.@event.threat);
        }
    }

    [TestClass]
    public class OneRowAmountTypeAndNumberAndMore_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:23:49] [@Idrurrez] [Elite Tastybobble {846623953387520}] " + 
                "[Saber Throw {812165430771712}]" + 
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 23, 49, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Idrurrez", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Elite Tastybobble", entry.target.name);
            Assert.AreEqual(846623953387520, entry.target.number);
            Assert.AreEqual(false, entry.target.isPlayer);
            Assert.AreEqual("Saber Throw", entry.ability.name);
            Assert.AreEqual(812165430771712, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(836045448945501, entry.@event.effect.number);
            Assert.AreEqual(1002, entry.@event.result.amount);
            Assert.AreEqual("energy", entry.@event.result.type);
            Assert.AreEqual(836045448940874, entry.@event.result.number);
            Assert.AreEqual(1002, entry.@event.threat);
        }
    }

    [TestClass]
    public class OneRowNullAbility_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:23:49] [@Idrurrez] [@Idrurrez] [] " + 
                "[Restore {836045448945476}: focus point {836045448938496}] (3)");
        }


        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 23, 49, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Idrurrez", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Idrurrez", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("", entry.ability.name);
            Assert.AreEqual("Restore", entry.@event.name);
            Assert.AreEqual(836045448945476, entry.@event.number);
            Assert.AreEqual("focus point", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(836045448938496, entry.@event.effect.number);
            Assert.AreEqual(3, entry.@event.result.amount);
        }
    }

    [TestClass]
    public class OneRowNoSubtype_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[03/01/2012 14:19:14] [@Idrurrez] [@Idrurrez] " + 
                "[Force Might {1781496599805952}] " + 
                "[ApplyEffect {836045448945477}: Lucky Shots {1781496599806223}] ()");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 19, 14, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Idrurrez", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Idrurrez", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Force Might", entry.ability.name);
            Assert.AreEqual(1781496599805952, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Lucky Shots", entry.@event.effect.name);
            Assert.AreEqual("", entry.@event.effect.subtype);
            Assert.AreEqual(1781496599806223, entry.@event.effect.number);
            Assert.AreEqual(0, entry.@event.result.amount);
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
            Assert.AreEqual(entry.@event.result.amount, 0);
        }

        [TestMethod]
        public void Parse_Should_Return_ForceClap_Ability()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.ability.name, "Force Clap");
            Assert.AreEqual(entry.ability.number, 2848585519464448);
        }

        [TestMethod]
        public void Parse_Should_Return_ApplyEffect_Event()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.@event.name, "ApplyEffect");
            Assert.AreEqual(entry.@event.number, 836045448945477);
            Assert.AreEqual(entry.@event.effect.name, "Stunned");
            Assert.AreEqual(entry.@event.effect.subtype, "Physical");
            Assert.AreEqual(entry.@event.effect.number, 2848585519464704);
        }

        [TestMethod]
        public void Parse_Should_Return_Player_Khantni_As_Target()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual("Khantni", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
        }

        [TestMethod]
        public void Parse_Should_Return_Player_Idrurrez_As_Source()
        {
            // Arrange

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual("Idrurrez", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
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
            Assert.AreEqual(expected, entry.timestamp);
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

