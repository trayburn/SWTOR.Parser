using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWTOR.Web.Controllers;
using SWTOR.Web.Tests;

namespace SWTOR.Web.Controllers.Tests
{
    [TestClass]
    public class HomeControllerTest : BaseControllerTest<HomeController>
    {
        [TestMethod]
        public void Index()
        {
            // Arrange

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result as ViewResult);
            
        }
    }
}
