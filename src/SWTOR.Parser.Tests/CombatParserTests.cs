using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SWTOR.Parser.Tests
{
    [TestClass]
    public class CombatParserTests
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

        public LogHelper Damage(Actor source, Actor target, string abilityName, int amount, string type)
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
                    ability = new Ability { name = abilityName, number = RandomInt64() },
                    @event = new Event
                    {
                        name = "ApplyEffect",
                        number = 836045448945477,
                        effect = new Effect
                        {
                            name = "Damage",
                            number = 836045448945501
                        },
                        result = new Result
                        {
                            number = 836045448940874,
                            amount = amount,
                            type = type
                        },
                        threat = amount
                    }
                });

            return this;
        }

        public LogHelper ExitCombat(Actor actor)
        {
            // [03/17/2012 19:49:31] [@Argorash] [@Argorash] 
            // [] [Event {836045448945472}: ExitCombat {836045448945490}] ()  
            var exitCmbt = CreateSimpleEvent("ExitCombat", 836045448945490);
            log.Add(new LogEntry
                {
                    timestamp = now,
                    source = actor,
                    target = actor,
                    @event = exitCmbt
                });
            return this;
        }

        public LogHelper EnterCombat(Actor actor)
        {
            // [03/17/2012 19:48:51] [@Argorash] [@Argorash] 
            // [] [Event {836045448945472}: EnterCombat {836045448945489}] ()
            var entercmbt = CreateSimpleEvent("EnterCombat", 836045448945489);
            log.Add(new LogEntry
            {
                timestamp = now,
                source = actor,
                target = actor,
                @event = entercmbt
            });

            return this;
        }

        private Event CreateSimpleEvent(string effectName, Int64 effectNumber)
        {
            return new Event
            {
                name = "Event",
                number = 836045448945472,
                effect = new Effect
                {
                    name = effectName,
                    number = effectNumber
                }
            };
        }

        private long RandomInt64()
        {
            return DateTime.Now.Ticks;
        }
    }
}
