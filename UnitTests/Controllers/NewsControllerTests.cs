using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using Planetarium.Models;
using System;
using System.Web.Mvc;
namespace UnitTests.Controllers
{
    [TestClass]
    public class NewsControllerTests
    {
        [TestMethod]
        public void TestNewsViewIsNotNull()
        {
            NewsController newsController = new NewsController();
            ActionResult view = newsController.ViewSucceed();
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void TestNewsView() {
            NewsController newsController = new NewsController();
            var result = newsController.ViewSucceed() as ViewResult;
            Assert.AreEqual("View Succeed", result.ViewName);
        }

        [TestMethod]
        public void TestNews()
        {
            NewsController newsController = new NewsController();
            ActionResult view = newsController.News("Noticia");
            Assert.IsNotNull(view);
        }
    }
}
