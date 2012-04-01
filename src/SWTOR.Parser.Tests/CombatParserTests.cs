using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using SWTOR.Parser.Domain;

namespace SWTOR.Parser.Tests.CombatParserTests
{
    [TestClass]
    public class OneCombatIntegration_CombatParserTests : BaseCombatParserIntegrationTest
    {
        public override string FileName
        {
            get { return "oneCombat.txt"; }
        }

        [TestMethod]
        public void When_Clean_Is_Called_Excess_Log_Data_Should_Be_Gone()
        {
            // Arrange
            var res = target.Parse(log);
            // Check there is data
            Assert.AreNotEqual(0, res.Combats[0].Log.Count, "There should be log data here.");

            // Act
            target.Clean(res);

            // Assert
            foreach (var combat in res.Combats)
            {
                Assert.AreEqual(0, combat.Log.Count);
                foreach (CharacterData value in combat.Characters.Values)
                {
                    Assert.AreEqual(0, value.AsTarget.Log.Count);
                    Assert.AreEqual(0, value.AsSource.Log.Count);
                }
            }
        }

        [TestMethod]
        public void For_Combat_One_Ensure_AverageDPS()
        {
            // Arrange
            

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1040.5, res.Combats[0].AverageDamagePerSecond);
        }


        [TestMethod]
        public void For_Combat_One_Ensure_AverageHPS()
        {
            // Arrange


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(528.35, res.Combats[0].AverageHealingPerSecond);
        }

        [TestMethod]
        public void For_Combat_One_Ensure_Interval()
        {
            // Arrange
            

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(40, res.Combats[0].Interval);
        }

        [TestMethod]
        public void Ensure_Damage_Total()
        {
            // Arrange
            

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(41620, res.TotalDamage);
        }

        [TestMethod]
        public void Ensure_Healing_Total()
        {
            // Arrange
            

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(21134, res.TotalHealing);
        }


        [TestMethod]
        public void Ensure_Parry_Count()
        {
            // Arrange


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(10, res.CountOfParry);
        }

        [TestMethod]
        public void Ensure_Deflect_Count()
        {
            // Arrange


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1, res.CountOfDeflect);
        }
    }
    [TestClass]
    public class OneAndTwoCombats_CombatParserTests
    {
        private CombatParser target;
        private List<LogEntry> log;
        private LogHelper h;
        private Actor player;
        private Actor mob;

        [TestInitialize]
        public void SetUp()
        {
            target = new CombatParser();
            log = new List<LogEntry>();
            h = new LogHelper(log);
            player = new Actor { name = "Gisben", isPlayer = true };
            mob = new Actor { name = "Soa", number = 836045448947788 };
        }

        [TestMethod]
        public void Given_One_Combat_When_Parse_Then_AbilityCounts_Populated()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(mob, player, "Headbutt", 150, "energy").Tick()
                .Damage(player, mob, "Junkpunch", 1000, "physical")
                .Damage(mob, player, "Headbutt", 450, "energy", true).Tick()
                .ExitCombat(player);


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(2, res.AbilityCounts[0].Count);
            Assert.AreEqual("Headbutt", res.AbilityCounts[0].Name);
            Assert.AreEqual(300.0, res.AbilityCounts[0].AverageDamage);
            Assert.AreEqual(150, res.AbilityCounts[0].MinimumDamage);
            Assert.AreEqual(450, res.AbilityCounts[0].MaximumDamage);
            Assert.AreEqual(1, res.AbilityCounts[0].CountOfCriticals);

            Assert.AreEqual(1, res.AbilityCounts[1].Count);
            Assert.AreEqual("Junkpunch", res.AbilityCounts[1].Name);
            Assert.AreEqual(1000.00, res.AbilityCounts[1].AverageDamage);
            Assert.AreEqual(1000, res.AbilityCounts[1].MinimumDamage);
            Assert.AreEqual(1000, res.AbilityCounts[1].MaximumDamage);
            Assert.AreEqual(0, res.AbilityCounts[1].CountOfCriticals);
        }

        [TestMethod]
        public void Given_One_Combat_When_Parse_Then_Threat_Should_Be_Totalled()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(mob, player, "Headbutt", 250, "energy").Tick()
                .Damage(mob, player, "Headbutt", 250, "energy").Tick()
                .ExitCombat(player);


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(500, res.TotalThreat);
            Assert.AreEqual(500, res.Combats[0].TotalThreat);
            Assert.AreEqual((double)500 / 3, res.Combats[0].AverageThreatPerSecond);
        }

        [TestMethod]
        public void Given_One_Combat_When_Parse_Then_Characters_Player_AsTarget_Should_Be_Populated()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(mob, player, "Headbutt", 250, "energy").Tick()
                .Damage(mob, player, "Headbutt", 250, "energy").Tick()
                .ExitCombat(player);


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1, res.Combats.Count);
            Assert.AreEqual(2, res.Combats[0].Characters.Count);
            var playerMetrics = res.Combats[0].Characters[player.name];
            Assert.AreEqual(3, playerMetrics.AsTarget.Interval);
            Assert.AreEqual(500, playerMetrics.AsTarget.TotalDamage);
            Assert.AreEqual(0, playerMetrics.AsTarget.TotalHealing);
            Assert.AreEqual((double)500 / 3, playerMetrics.AsTarget.AverageDamagePerSecond);
            Assert.AreEqual(0, playerMetrics.AsTarget.AverageHealingPerSecond);
        }

        [TestMethod]
        public void Given_One_Combat_When_Parse_Then_Characters_Player_AsSource_Should_Be_Populated()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .ExitCombat(player);


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1, res.Combats.Count);
            Assert.AreEqual(2, res.Combats[0].Characters.Count);
            var playerMetrics = res.Combats[0].Characters[player.name];
            Assert.AreEqual(3, playerMetrics.AsSource.Interval);
            Assert.AreEqual(500, playerMetrics.AsSource.TotalDamage);
            Assert.AreEqual(0, playerMetrics.AsSource.TotalHealing);
            Assert.AreEqual((double)500 / 3, playerMetrics.AsSource.AverageDamagePerSecond);
            Assert.AreEqual(0, playerMetrics.AsSource.AverageHealingPerSecond);
        }

        [TestMethod]
        public void Given_One_Combat_When_Parse_Then_Combats_Characters_Should_Be_2()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .ExitCombat(player);


            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1, res.Combats.Count);
            Assert.AreEqual(2, res.Combats[0].Characters.Count);
            Assert.IsTrue(res.Combats[0].Characters.Keys.Contains(player.name));
            Assert.IsTrue(res.Combats[0].Characters.Keys.Contains(mob.name));
        }

        [TestMethod]
        public void Given_One_Combat_Without_An_Exit_When_Parse_Then_Combats_Should_Be_1()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick();
 

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1, res.Combats.Count);
            Assert.AreEqual(3, res.Combats[0].Log.Count);
        }

        [TestMethod]
        public void Given_Two_Combats_With_Damage_When_Parse_Then_Sum_Damage_To_Each_Combat_TotalDamage()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .Damage(player, mob, "Headbutt", 250, "energy").Tick()
                .ExitCombat(player).Tick();
            h.EnterCombat(player).Tick()
                .Damage(mob, player, "Junk Punch", 999, "physical").Tick()
                .Damage(mob, player, "Junk Punch", 1, "physical").Tick()
                .ExitCombat(player);

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(500, res.Combats[0].TotalDamage);
            Assert.AreEqual(1000, res.Combats[1].TotalDamage);
        }

        [TestMethod]
        public void Given_Two_Combats_With_Healing_When_Parse_Then_Sum_Healing_To_Each_Combat_TotalHealing()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Heal(player, player, "Rejuvination", 1024).Tick()
                .Heal(player, player, "Rejuvination", 1024).Tick()
                .ExitCombat(player).Tick();
            h.EnterCombat(player).Tick()
                .Heal(player, player, "Rejuvination", 2048).Tick()
                .Heal(player, player, "Rejuvination", 2048).Tick()
                .ExitCombat(player).Tick();

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(2048, res.Combats[0].TotalHealing);
            Assert.AreEqual(4096, res.Combats[1].TotalHealing);
        }

        [TestMethod]
        public void Given_Two_Combats_With_Healing_When_Parse_Then_Sum_Healing_To_TotalHealing()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Heal(player, player, "Rejuvination", 1024).Tick()
                .ExitCombat(player).Tick();
            h.EnterCombat(player).Tick()
                .Heal(player, player, "Rejuvination", 2048).Tick()
                .ExitCombat(player).Tick();

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(3072, res.TotalHealing);
        }

        [TestMethod]
        public void Given_Two_Combats_With_Damage_When_Parse_Then_Sum_Damage_To_TotalDamage()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(player,mob,"Headbutt",250,"energy").Tick()
                .ExitCombat(player).Tick();
            h.EnterCombat(player).Tick()
                .Damage(mob, player, "Junk Punch", 999, "physical").Tick()
                .ExitCombat(player);

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1249, res.TotalDamage);
        }

        [TestMethod]
        public void Given_Combat_With_Damage_When_Parse_Then_Sum_Damage_To_TotalDamage()
        {
            // Arrange
            h.EnterCombat(player).Tick()
                .Damage(player,mob,"Headbutt",250,"energy").Tick()
                .ExitCombat(player);

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(250, res.TotalDamage);
        }

        [TestMethod]
        public void Given_One_Combat_When_Parse_Then_One_CombatLog_Entry()
        {
            // Arrange
            h.EnterCombat(player).Tick().ExitCombat(player);

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(1, res.Combats.Count);
            Assert.AreEqual(2, res.Combats.First().Log.Count);
        }

        [TestMethod]
        public void Given_Two_Combats_When_Parse_Then_Two_Combat_Entry()
        {
            // Arrange
            h.EnterCombat(player).Tick().ExitCombat(player)
                .EnterCombat(player).Tick().ExitCombat(player);

            // Act
            var res = target.Parse(log);

            // Assert
            Assert.AreEqual(2, res.Combats.Count);
            Assert.AreEqual(2, res.Combats[0].Log.Count);
            Assert.AreEqual(2, res.Combats[1].Log.Count);
        }
    }

    public class LogHelper
    {
        
        private List<LogEntry> log;
        private DateTime now;

        public LogHelper(List<LogEntry> log)
            : this(log, DateTime.Now)
        {
            this.log = log;
        }

        public LogHelper(List<LogEntry> log, DateTime now)
        {
            this.log = log;
            this.now = now;
        }

        public LogHelper Tick()
        {
            now = now.AddSeconds(1);
            return this;
        }

        public LogHelper Heal(Actor source, Actor target, string abilityName, int amount)
        {
            // [03/17/2012 19:45:04] [@Argorash] [@Argorash] 
            // [Heroic Moment: Call on the Force {1412666283261952}] 
            // [ApplyEffect {836045448945477}: Heal {836045448945500}] (434)

            log.Add(new LogEntry
            {
                timestamp = now,
                source = source,
                target = target,
                ability = new GameObject { name = abilityName, number = RandomInt64() },
                @event = new GameObject
                {
                    name = "ApplyEffect",
                    number = 836045448945477,
                },
                effect = new Effect
                {
                    name = "Heal",
                    number = 836045448945501
                },
                result = new Result
                {
                    number = 836045448940874,
                    amount = amount
                }
            });

            return this;
        }

        public LogHelper Damage(Actor source, Actor target, string abilityName, int amount, string type, bool isCritical = false)
        {
            // [03/17/2012 19:49:20] [@Psyfe] [@Argorash] 
            // [Series of Shots {2299572734918656}] 
            // [ApplyEffect {836045448945477}: Damage {836045448945501}] 
            // (234 energy {836045448940874} -glance {836045448945509} (234 absorbed {836045448945511})) <234>

            log.Add(new LogEntry
                {
                    timestamp = now,
                    source = source,
                    target = target,
                    ability = new GameObject { name = abilityName, number = RandomInt64() },
                    @event = new GameObject
                    {
                        name = "ApplyEffect",
                        number = 836045448945477
                    },
                    effect = new Effect
                    {
                        name = "Damage",
                        number = 836045448945501
                    },
                    result = new Result
                    {
                        number = 836045448940874,
                        amount = amount,
                        name = type, 
                        isCritical = isCritical
                    },
                    threat = amount
                });

            return this;
        }

        public LogHelper ExitCombat(Actor actor)
        {
            // [03/17/2012 19:49:31] [@Argorash] [@Argorash] 
            // [] [Event {836045448945472}: ExitCombat {836045448945490}] ()  
            log.Add(new LogEntry
                {
                    timestamp = now,
                    source = actor,
                    target = actor,
                    @event = CreateSimpleEvent(),
                    effect = CreateEffect("ExitCombat", 836045448945490)
                });
            return this;
        }

        public LogHelper EnterCombat(Actor actor)
        {
            // [03/17/2012 19:48:51] [@Argorash] [@Argorash] 
            // [] [Event {836045448945472}: EnterCombat {836045448945489}] ()
            log.Add(new LogEntry
            {
                timestamp = now,
                source = actor,
                target = actor,
                @event = CreateSimpleEvent(),
                effect = CreateEffect("EnterCombat", 836045448945489)
            });

            return this;
        }

        private GameObject CreateSimpleEvent()
        {
            return new GameObject
            {
                name = "Event",
                number = 836045448945472,
            };
        }

        private Effect CreateEffect(string effectName, Int64 effectNumber)
        {
            return new Effect
            {
                name = effectName,
                number = effectNumber
            };
        }

        private long RandomInt64()
        {
            return DateTime.Now.Ticks;
        }
    }
}
