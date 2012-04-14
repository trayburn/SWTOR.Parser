using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace SWTOR.Parser.Tests.ParserTests
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
    public class OneLineAndParry_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[16:17:36.125] [@Rivder] [Krayt Street Thrasher {670156632096768}] " + 
                "[Ravage {1261367470325760}] " + 
                "[ApplyEffect {836045448945477}: Damage {836045448945501}] (0 -parry {836045448945503}) <1>");
        }


        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 17, 36, 125, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivder", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Krayt Street Thrasher", entry.target.name);
            Assert.AreEqual(670156632096768, entry.target.number);
            Assert.AreEqual(false, entry.target.isPlayer);
            Assert.AreEqual("Ravage", entry.ability.name);
            Assert.AreEqual(1261367470325760, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945501, entry.effect.number);
            Assert.AreEqual(0, entry.result.amount);
            Assert.AreEqual(false, entry.result.isCritical);
            Assert.AreEqual("-parry", entry.result.name);
            Assert.AreEqual("", entry.mitigation.name);
            Assert.AreEqual(1, entry.threat);
        }
    }

    [TestClass]
    public class OneLineAndColonInEffect_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[16:37:43.177] [@Rivder] [@Rivder] " +
                "[Heroic Moment: Call on the Force {1412666283261952}] " + 
                "[ApplyEffect {836045448945477}: Heroic Moment: Call on the Force {1412666283261952}] ()");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 37, 43, 177, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivder", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Rivder", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Heroic Moment: Call on the Force", entry.ability.name);
            Assert.AreEqual(1412666283261952, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Heroic Moment: Call on the Force", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(1412666283261952, entry.effect.number);
            Assert.AreEqual(0, entry.result.amount);
            Assert.AreEqual(false, entry.result.isCritical);
            Assert.AreEqual("", entry.result.name);
            Assert.AreEqual("", entry.mitigation.name);
            Assert.AreEqual(0, entry.threat);
        }
    }
    [TestClass]
    public class OneRowWithZeroDamageButHangingDash_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[01:39:39.776] [@Rivshot] [@Darklink] " +
                "[Fragmentation Grenade {1624614329384960}] " +
                "[ApplyEffect {836045448945477}: Damage {836045448945501}] (0 -) <1>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 1, 39, 39, 776, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivshot", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Darklink", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Fragmentation Grenade", entry.ability.name);
            Assert.AreEqual(1624614329384960, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945501, entry.effect.number);
            Assert.AreEqual(0, entry.result.amount);
            Assert.AreEqual(false, entry.result.isCritical);
            Assert.AreEqual("", entry.result.name);
            Assert.AreEqual("", entry.mitigation.name);
            Assert.AreEqual(1, entry.threat);
        }
    }

    [TestClass]
    public class OneRowWithCritHeal_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[01:39:40.383] [@Zartok] [@Rivshot] " +
                "[Revivification {808703687131136}] " +
                "[ApplyEffect {836045448945477}: Heal {836045448945500}] (347*) <147>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 1, 39, 40, 383, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Zartok", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Rivshot", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Revivification", entry.ability.name);
            Assert.AreEqual(808703687131136, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Heal", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945500, entry.effect.number);
            Assert.AreEqual(347, entry.result.amount);
            Assert.AreEqual(true, entry.result.isCritical);
            Assert.AreEqual("", entry.result.name);
            Assert.AreEqual("", entry.mitigation.name);
            Assert.AreEqual(147, entry.threat);
        }
    }

    [TestClass]
    public class OneRowWithNoTarget_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[01:41:08.649] [@Rivshot] [] " +
                "[Deliverance {880116108361728}] " +
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
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 1, 41, 8, 649, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivshot", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("", entry.target.name);
            Assert.AreEqual("Deliverance", entry.ability.name);
            Assert.AreEqual(880116108361728, entry.ability.number);
            Assert.AreEqual("Event", entry.@event.name);
            Assert.AreEqual(836045448945472, entry.@event.number);
            Assert.AreEqual("AbilityInterrupt", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945482, entry.effect.number);
            Assert.AreEqual(0, entry.result.amount);
            Assert.AreEqual(false, entry.result.isCritical);
            Assert.AreEqual("", entry.result.name);
            Assert.AreEqual("", entry.mitigation.name);
            Assert.AreEqual(0, entry.threat);
        }

    }

    [TestClass]
    public class OneRowWithAbsorption_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[01:42:38.840] [@Rivshot] [@Sandals] " +
                "[Fragmentation Grenade {1624614329384960}] [ApplyEffect {836045448945477}: Damage {836045448945501}] " +
                "(745 kinetic {836045448940873} (223 absorbed {836045448945511})) <745>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 1, 42, 38, 840, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivshot", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Sandals", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Fragmentation Grenade", entry.ability.name);
            Assert.AreEqual(1624614329384960, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945501, entry.effect.number);
            Assert.AreEqual(745, entry.result.amount);
            Assert.AreEqual(false, entry.result.isCritical);
            Assert.AreEqual("kinetic", entry.result.name);
            Assert.AreEqual(836045448940873, entry.result.number);
            Assert.AreEqual(223, entry.mitigation.amount);
            Assert.AreEqual("absorbed", entry.mitigation.name);
            Assert.AreEqual(836045448945511, entry.mitigation.number);
            Assert.AreEqual(745, entry.threat);
        }
    }

    [TestClass]
    public class OneRowWithResultDefenseAndMitigation_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[09:16:35.166] [@Psyfe] [@Argorash] " + 
                "[Series of Shots {2299572734918656}] " + 
                "[ApplyEffect {836045448945477}: Damage {836045448945501}] " + 
                "(234 energy {836045448940874} -glance {836045448945509} (234 absorbed {836045448945511})) " + 
                "<234>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 9, 16, 35, 166, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Psyfe", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Argorash", entry.target.name);
            Assert.AreEqual(0, entry.target.number);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Series of Shots", entry.ability.name);
            Assert.AreEqual(2299572734918656, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945501, entry.effect.number);
            Assert.AreEqual(234, entry.result.amount);
            Assert.AreEqual("energy", entry.result.name);
            Assert.AreEqual(836045448940874, entry.result.number);
            Assert.AreEqual("glance", entry.defense.name);
            Assert.AreEqual(836045448945509, entry.defense.number);
            Assert.AreEqual(234, entry.mitigation.amount);
            Assert.AreEqual("absorbed", entry.mitigation.name);
            Assert.AreEqual(836045448945511, entry.mitigation.number);
            Assert.AreEqual(234, entry.threat);
        }
    }

    [TestClass]
    public class OneRowWithCompanionData_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[16:24:12.706] [@Rivder:Malavai Quinn {493307058716672}] [@Rivder] " +
                "[Imperial Medical Probe {2466118681755907}] " +
                "[ApplyEffect {836045448945477}: Heal {836045448945500}] (61) <23>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 24, 12, 706, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivder:Malavai Quinn", entry.source.name);
            Assert.AreEqual(493307058716672, entry.source.number);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Rivder", entry.target.name);
            Assert.AreEqual(0, entry.target.number);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Imperial Medical Probe", entry.ability.name);
            Assert.AreEqual(2466118681755907, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Heal", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945500, entry.effect.number);
            Assert.AreEqual(61, entry.result.amount);
            Assert.AreEqual("", entry.result.name);
            Assert.AreEqual(0, entry.result.number);
            Assert.AreEqual(23, entry.threat);
        }
    }

    [TestClass]
    public class OneRowAmountTypeAndNumberAndMore_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[16:24:12.606] [Krayt Street Thrasher {670156632096768}] [@Rivder] " +
                "[Melee Attack {813445331025920}] " +
                "[ApplyEffect {836045448945477}: Damage {836045448945501}] (48 kinetic {836045448940873}) <48>");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 24, 12, 606, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Krayt Street Thrasher", entry.source.name);
            Assert.AreEqual(670156632096768, entry.source.number);
            Assert.AreEqual(false, entry.source.isPlayer);
            Assert.AreEqual("Rivder", entry.target.name);
            Assert.AreEqual(0, entry.target.number);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Melee Attack", entry.ability.name);
            Assert.AreEqual(813445331025920, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Damage", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448945501, entry.effect.number);
            Assert.AreEqual(48, entry.result.amount);
            Assert.AreEqual("kinetic", entry.result.name);
            Assert.AreEqual(836045448940873, entry.result.number);
            Assert.AreEqual(48, entry.threat);
        }
    }

    [TestClass]
    public class OneRowNullAbility_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[16:24:13.762] [@Rivder] [@Rivder] [] " +
                "[Restore {836045448945476}: rage point {836045448938497}] (1)");
        }


        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange


            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 24, 13, 762, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivder", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Rivder", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("", entry.ability.name);
            Assert.AreEqual("Restore", entry.@event.name);
            Assert.AreEqual(836045448945476, entry.@event.number);
            Assert.AreEqual("rage point", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(836045448938497, entry.effect.number);
            Assert.AreEqual(1, entry.result.amount);
        }
    }

    [TestClass]
    public class OneRowNoSubtype_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.AppendLine("[16:16:52.674] [@Rivder] [@Rivder] " +
                "[Ravage {1261367470325760}] " +
                "[ApplyEffect {836045448945477}: Unshakable {1261367470326274}] ()");
        }

        [TestMethod]
        public void Parse_Should_Return_Entry_Correctly()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 16, 52, 674, DateTimeKind.Unspecified), entry.timestamp);
            Assert.AreEqual("Rivder", entry.source.name);
            Assert.AreEqual(true, entry.source.isPlayer);
            Assert.AreEqual("Rivder", entry.target.name);
            Assert.AreEqual(true, entry.target.isPlayer);
            Assert.AreEqual("Ravage", entry.ability.name);
            Assert.AreEqual(1261367470325760, entry.ability.number);
            Assert.AreEqual("ApplyEffect", entry.@event.name);
            Assert.AreEqual(836045448945477, entry.@event.number);
            Assert.AreEqual("Unshakable", entry.effect.name);
            Assert.AreEqual("", entry.effect.subtype);
            Assert.AreEqual(1261367470326274, entry.effect.number);
            Assert.AreEqual(0, entry.result.amount);
            Assert.AreEqual(0, entry.threat);
        }
    }

    [TestClass]
    public class OneRow_ParserTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.Append("[16:17:39.833] [@Rivder] [Krayt Slicer {670139452227584}] " +
                         "[Smash {807801743998976}] " +
                         "[ApplyEffect {836045448945477}: Stunned (Physical) {807801743999258}] ()");
        }

        [TestMethod]
        public void Parse_Should_Return_Zero_Event_Amount()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.result.amount, 0);
        }

        [TestMethod]
        public void Parse_Should_Return_ForceClap_Ability()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual(entry.ability.name, "Smash");
            Assert.AreEqual(entry.ability.number, 807801743998976);
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
            Assert.AreEqual(entry.effect.name, "Stunned");
            Assert.AreEqual(entry.effect.subtype, "Physical");
            Assert.AreEqual(entry.effect.number, 807801743999258);
        }

        [TestMethod]
        public void Parse_Should_Return_Krayt_Slicer_As_Target()
        {
            // Arrange
            

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual("Krayt Slicer", entry.target.name);
            Assert.AreEqual(false, entry.target.isPlayer);
            Assert.AreEqual(670139452227584, entry.target.number);
        }

        [TestMethod]
        public void Parse_Should_Return_Player_Rivder_As_Source()
        {
            // Arrange

            // Act
            var list = target.Parse(rdr);

            // Assert
            var entry = list.First();
            Assert.AreEqual("Rivder", entry.source.name);
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
            var expected = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 16, 17, 39, 833, DateTimeKind.Unspecified);

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

    [TestClass]
    public class OneRow_BadInputTests : BaseParserTest
    {
        public override void BuildTestString(StringBuilder bldr)
        {
            bldr.Append("abcd");
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Parse_Should_Throw_When_Bad_Input_Is_Received()
        {
            // Arrange

            // Act
            var list = target.Parse(rdr);

            // Assert
            Assert.IsNull(list);
            Assert.Fail();
        }
    }
}

