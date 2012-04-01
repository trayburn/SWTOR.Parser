using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client;
using Rhino.Mocks;
using SWTOR.Parser;
using SWTOR.Web.Controllers;
using SWTOR.Web.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using SWTOR.Parser.Domain;
using System.Web.Mvc;
using SWTOR.Web.Data;

namespace SWTOR.Web.Controllers.Tests
{
    [TestClass]
    public class CombatControllerTest : BaseControllerTest<CombatController>
    {
        private IRepository<CombatLog> repo;
        private IHashCreator hasher;
        private IStringParser parser;
        private ICombatParser combatParser;
        private string log;

        protected override void RegisterComponents(IWindsorContainer container)
        {
            base.RegisterComponents(container);
            repo = RegisterMock<IRepository<CombatLog>>();
            hasher = RegisterMock<IHashCreator>();
            parser = RegisterMock<IStringParser>();
            combatParser = RegisterMock<ICombatParser>();
            log = @"[03/17/2012 19:30:34] [@Argorash] [@Argorash] " +
                @"[Personal Vehicle {2176247043981312}] " +
                @"[RemoveEffect {836045448945478}: Personal Vehicle {2176247043981312}] ()";
        }

        [TestMethod]
        public void Parse_Should_Query_For_Hash_And_If_Found_Never_Parse()
        {
            // Arrange
            var id = "abc123";
            hasher.Stub(m => m.CreateHash(null))
                .IgnoreArguments().Return(id);
            repo.Stub(m => m.Query()).Return(
                new List<CombatLog>
                {
                    new CombatLog { Id = "xyz789" },
                    new CombatLog { Id = id }
                }.AsQueryable()
                );


            // Act
            var res = controller.Parse(log);

            // Assert
            repo.AssertWasCalled(m => m.Query());
            parser.AssertWasNotCalled(m => m.ParseString(log), mi => mi.IgnoreArguments());
        }

        [TestMethod]
        public void Parse_Should_Redirect_To_Log()
        {
            // Arrange
            combatParser.Stub(m => m.Parse(null))
                .IgnoreArguments().Return(new CombatLog());
            repo.Stub(m => m.Query()).Return(
                new List<CombatLog>
                {
                    new CombatLog { Id = "xyz789" }
                }.AsQueryable()
                );

            // Act
            var res = controller.Parse(log);

            // Assert
            Assert.IsNotNull(res as RedirectToRouteResult);
        }

        [TestMethod]
        public void Parse_Should_Save_With_Hash_Id()
        {
            // Arrange
            var cLog = new CombatLog();
            var hash = "abc123";
            hasher.Stub(m => m.CreateHash(null))
                .IgnoreArguments().Return(hash);
            combatParser.Stub(m => m.Parse(null))
                .IgnoreArguments().Return(cLog);
            repo.Stub(m => m.Query()).Return(
                new List<CombatLog>
                {
                    new CombatLog { Id = "xyz789" }
                }.AsQueryable()
                );

            // Act
            var res = controller.Parse(log);

            // Assert
            Assert.AreEqual(hash, cLog.Id);
            repo.AssertWasCalled(m => m.Store(cLog));
            repo.AssertWasCalled(m => m.SaveChanges());
        }

        [TestMethod]
        public void Parse_Should_Clean_Model()
        {
            // Arrange
            var cLog = new CombatLog();
            combatParser.Stub(m => m.Parse(null))
                .IgnoreArguments().Return(cLog);
            repo.Stub(m => m.Query()).Return(
                new List<CombatLog>
                {
                    new CombatLog { Id = "xyz789" }
                }.AsQueryable()
                );

            // Act
            var res = controller.Parse(log);

            // Assert
            combatParser.AssertWasCalled(m => m.Clean(cLog));
        }

        [TestMethod]
        public void Log_Should_Query_For_Id_And_Return_Home_If_Not_Found()
        {
            // Arrange
            var id = "abc123";
            repo.Stub(m => m.Query()).Return(
                new List<CombatLog>
                {
                    new CombatLog { Id = "xyz789" }
                }.AsQueryable()
                );

            // Act
            var res = controller.Log(id);

            // Assert
            Assert.IsNotNull(res as RedirectToRouteResult);
        }

        [TestMethod]
        public void Log_Should_Query_For_Id_And_Return_View_If_Found()
        {
            // Arrange
            var id = "abc123";
            repo.Stub(m => m.Query()).Return(
                new List<CombatLog>
                {
                    new CombatLog { Id = "xyz789" },
                    new CombatLog { Id = id }
                }.AsQueryable()
                );

            // Act
            var res = controller.Log(id);

            // Assert
            Assert.IsNotNull(res as ViewResult);
        }
    }
}
