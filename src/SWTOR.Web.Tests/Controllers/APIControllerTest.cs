using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWTOR.Web.Controllers;

namespace SWTOR.Web.Controllers.Tests
{
    [TestClass]
    public class APIControllerTest
    {
        [TestMethod]
        public void Parse()
        {
            // Arrange
            var controller = new APIController();

            // Act
            ContentResult result = controller.ParseText(@"[03/17/2012 19:30:34] [@Argorash] [@Argorash] [Personal Vehicle {2176247043981312}] [RemoveEffect {836045448945478}: Personal Vehicle {2176247043981312}] ()") as ContentResult;

            // Assert
            Assert.AreEqual("[{\"timestamp\":\"\\/Date(1332037834000)\\/\",\"source\":{\"isPlayer\":true,\"name\":\"Argorash\",\"number\":0},\"target\":{\"isPlayer\":true,\"name\":\"Argorash\",\"number\":0},\"ability\":{\"name\":\"Personal Vehicle\",\"number\":2176247043981312},\"event\":{\"name\":\"RemoveEffect\",\"number\":836045448945478},\"effect\":{\"subtype\":\"\",\"name\":\"Personal Vehicle\",\"number\":2176247043981312},\"result\":{\"amount\":0,\"isCritical\":false,\"name\":\"\",\"number\":0},\"defense\":{\"name\":\"\",\"number\":0},\"mitigation\":{\"amount\":0,\"isCritical\":false,\"name\":\"\",\"number\":0},\"threat\":0}]", result.Content.ToString());
        }
    }
}
