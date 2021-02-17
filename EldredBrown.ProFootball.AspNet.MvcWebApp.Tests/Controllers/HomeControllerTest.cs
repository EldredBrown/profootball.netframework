using System.Web.Mvc;
using EldredBrown.ProFootball.AspNet.MvcWebApp.Controllers;
using NUnit.Framework;

namespace EldredBrown.ProFootball.AspNet.MvcWebApp.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [TestCase]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [TestCase]
        public void About()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.About();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual("Your application description page.", (result as ViewResult).ViewBag.Message);
        }

        [TestCase]
        public void Contact()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Contact();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
