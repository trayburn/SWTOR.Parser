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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 47, 11, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Twos", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Khantni", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual("Affliction (Force)", entry.Ability.Name);
            Assert.AreEqual(808192586023173, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Damage", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(836045448945501, entry.Event.Effect.Number);
            Assert.AreEqual(0, entry.Event.Result.Amount);
            Assert.AreEqual(false, entry.Event.Result.IsCritical);
            Assert.AreEqual("", entry.Event.Result.Type);
            Assert.AreEqual("", entry.Event.Result.Mitigation.Name);
            Assert.AreEqual(209, entry.Event.Threat);
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 41, 33, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Twos", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Twos", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual("Resurgence", entry.Ability.Name);
            Assert.AreEqual(808699392163840, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Heal", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(836045448945500, entry.Event.Effect.Number);
            Assert.AreEqual(1485, entry.Event.Result.Amount);
            Assert.AreEqual(true, entry.Event.Result.IsCritical);
            Assert.AreEqual("", entry.Event.Result.Type);
            Assert.AreEqual("", entry.Event.Result.Mitigation.Name);
            Assert.AreEqual(0, entry.Event.Threat);
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 25, 55, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("", entry.Target.Name);
            Assert.AreEqual("Seethe", entry.Ability.Name);
            Assert.AreEqual(808226945761280, entry.Ability.Number);
            Assert.AreEqual("Event", entry.Event.Name);
            Assert.AreEqual(836045448945472, entry.Event.Number);
            Assert.AreEqual("AbilityInterrupt", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(836045448945482, entry.Event.Effect.Number);
            Assert.AreEqual(0, entry.Event.Result.Amount);
            Assert.AreEqual(false, entry.Event.Result.IsCritical);
            Assert.AreEqual("", entry.Event.Result.Type);
            Assert.AreEqual("", entry.Event.Result.Mitigation.Name);
            Assert.AreEqual(0, entry.Event.Threat);
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 25, 53, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Khantni", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual("Saber Throw", entry.Ability.Name);
            Assert.AreEqual(812165430771712, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Damage", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(836045448945501, entry.Event.Effect.Number);
            Assert.AreEqual(1903, entry.Event.Result.Amount);
            Assert.AreEqual(true, entry.Event.Result.IsCritical);
            Assert.AreEqual("energy", entry.Event.Result.Type);
            Assert.AreEqual(836045448940874, entry.Event.Result.Number);
            Assert.AreEqual(1903, entry.Event.Result.Mitigation.Amount);
            Assert.AreEqual("absorbed", entry.Event.Result.Mitigation.Type);
            Assert.AreEqual(836045448945511, entry.Event.Result.Mitigation.Number);
            Assert.AreEqual(1903, entry.Event.Threat);
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
            Assert.AreEqual("Damage", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(836045448945501, entry.Event.Effect.Number);
            Assert.AreEqual(1002, entry.Event.Result.Amount);
            Assert.AreEqual("energy", entry.Event.Result.Type);
            Assert.AreEqual(836045448940874, entry.Event.Result.Number);
            Assert.AreEqual(1002, entry.Event.Threat);
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 23, 49, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Idrurrez", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual("", entry.Ability.Name);
            Assert.AreEqual("Restore", entry.Event.Name);
            Assert.AreEqual(836045448945476, entry.Event.Number);
            Assert.AreEqual("focus point", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(836045448938496, entry.Event.Effect.Number);
            Assert.AreEqual(3, entry.Event.Result.Amount);
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
            Assert.AreEqual(new DateTime(2012, 3, 1, 14, 19, 14, DateTimeKind.Unspecified), entry.Timestamp);
            Assert.AreEqual("Idrurrez", entry.Source.Name);
            Assert.AreEqual(true, entry.Source.IsPlayer);
            Assert.AreEqual("Idrurrez", entry.Target.Name);
            Assert.AreEqual(true, entry.Target.IsPlayer);
            Assert.AreEqual("Force Might", entry.Ability.Name);
            Assert.AreEqual(1781496599805952, entry.Ability.Number);
            Assert.AreEqual("ApplyEffect", entry.Event.Name);
            Assert.AreEqual(836045448945477, entry.Event.Number);
            Assert.AreEqual("Lucky Shots", entry.Event.Effect.Name);
            Assert.AreEqual("", entry.Event.Effect.Subtype);
            Assert.AreEqual(1781496599806223, entry.Event.Effect.Number);
            Assert.AreEqual(0, entry.Event.Result.Amount);
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
            Assert.AreEqual(entry.Event.Result.Amount, 0);
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
            Assert.AreEqual(entry.Event.Effect.Name, "Stunned");
            Assert.AreEqual(entry.Event.Effect.Subtype, "Physical");
            Assert.AreEqual(entry.Event.Effect.Number, 2848585519464704);
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

