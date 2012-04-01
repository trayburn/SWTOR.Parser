using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWTOR.Web.Controllers;
using Castle.Windsor;
using SWTOR.Web.Tests;
using SWTOR.Parser;
using Rhino.Mocks;
using SWTOR.Parser.Domain;

namespace SWTOR.Web.Controllers.Tests
{
    [TestClass]
    public class APIControllerTest : BaseControllerTest<APIController>
    {
        private IStringParser parser;

        protected override void RegisterComponents(IWindsorContainer container)
        {
            base.RegisterComponents(container);
            parser = RegisterMock<IStringParser>();
        }

        [TestMethod]
        public void Parse()
        {
            // Arrange
            string log = @"[03/17/2012 19:30:34] [@Argorash] [@Argorash] " + 
                @"[Personal Vehicle {2176247043981312}] " + 
                @"[RemoveEffect {836045448945478}: Personal Vehicle {2176247043981312}] ()";
            parser.Expect(m => m.ParseString(log)).Return(new List<LogEntry>());

            // Act
            ContentResult result = controller.ParseText(log) as ContentResult;

            // Assert
            Assert.AreEqual("[]", result.Content.ToString());
        }
    }
}
