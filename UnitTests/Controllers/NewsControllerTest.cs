using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planetarium.Controllers;
using Planetarium.Models;
using System;
using System.Web.Mvc;

namespace UnitTests.Controllers
{
    [TestClass]
    public class NewsControllerTest
    {
        [TestMethod]
        public void TestNewsViewIsNotNull()
        {
            NewsController newsController = new NewsController();
            ActionResult view = newsController.ViewSucceed();
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void TestNewsView()
        {
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

        [TestMethod]
        public void NewsFromRSSFeedTest()
        {
            int newsCount = 3;
            NewsController newsController = new NewsController();

            ViewResult result = newsController.ListNews() as ViewResult;

            Assert.AreEqual(newsCount, result.ViewBag.NewsFromInternet.Count);
        }

        [TestMethod]
        public void NewsHeadersFromRSSFeedTest()
        {
            int newsCount = 3;
            NewsController newsController = new NewsController();
            ViewResult view = newsController.ListNews() as ViewResult;
            Assert.AreEqual(newsCount, view.ViewBag.NewsFromInternetHeaders.Count);
        }

        [TestMethod]
        public void EducativeNewsHeadersFromRSSFeedTest()
        {
            string newsHeader = "Noticias Educativas";
            int headerPosition = 0;
            NewsController newsController = new NewsController();
            ViewResult view = newsController.ListNews() as ViewResult;
            Assert.AreEqual(newsHeader, view.ViewBag.NewsFromInternetHeaders[headerPosition]);
        }

        [TestMethod]
        public void WorldNewsHeadersFromRSSFeedTest()
        {
            string newsHeader = "Noticias del Mundo";
            int headerPosition = 1;
            NewsController newsController = new NewsController();
            ViewResult view = newsController.ListNews() as ViewResult;
            Assert.AreEqual(newsHeader, view.ViewBag.NewsFromInternetHeaders[headerPosition]);
        }

        [TestMethod]
        public void SpaceNewsHeadersFromRSSFeedTest()
        {
            string newsHeader = "Noticias del Espacio";
            int headerPosition = 2;
            NewsController newsController = new NewsController();
            ViewResult view = newsController.ListNews() as ViewResult;
            Assert.AreEqual(newsHeader, view.ViewBag.NewsFromInternetHeaders[headerPosition]);
        }
    }
}
