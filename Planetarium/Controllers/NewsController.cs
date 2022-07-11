using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Planetarium.Handlers;
using Planetarium.Models;
using System.IO;
using System.Xml;
using Rotativa;

namespace Planetarium.Controllers
{
    public class NewsController : Controller
    {

        public NewsHandler DataAccess { get; set; }
        public ContentParser ContentParser { get; set; }

        public NewsController()
        {
            DataAccess = new NewsHandler();
            ContentParser = new ContentParser();
        }
        public ActionResult ViewSucceed()
        {
            ViewBag.Prueba = "Test";
            return View("View Succeed");
        }

        public ActionResult ListNews()
        {
            NewsHandler dataAccess = new NewsHandler();
            //ViewBag.News = dataAccess.GetAllNews();
            //ViewBag.News.Clear();
            RssFeedHandler rssHandler = new RssFeedHandler();


            List<string> sources = new List<string>();
            sources.Add("https://www.nasa.gov/rss/dyn/educationnews.rss");
            sources.Add("https://www.nasa.gov/rss/dyn/hurricaneupdate.rss");
            sources.Add("https://www.nasa.gov/rss/dyn/solar_system.rss");
            List<string> newsFromInternetHeaders = new List<string>();
            newsFromInternetHeaders.Add("Noticias Educativas");
            newsFromInternetHeaders.Add("Noticias del Mundo");
            newsFromInternetHeaders.Add("Noticias del Espacio");

            List<List<EventModel>> feeds = new List<List<EventModel>>();
            feeds.Add(rssHandler.GetRssFeed(sources[0]));
            feeds.Add(rssHandler.GetRssFeed(sources[1]));
            feeds.Add(rssHandler.GetRssFeed(sources[2]));

            ViewBag.NewsFromInternet = feeds;
            ViewBag.NewsFromInternetHeaders = newsFromInternetHeaders;
            return View();
        }

        public ActionResult PrintPDF() {
            RssFeedHandler rssHandler = new RssFeedHandler();
            List<EventModel> data = rssHandler.GetRssFeed("https://www.nasa.gov/rss/dyn/educationnews.rss");
            return new PartialViewAsPdf("_JobPrint", data)
            {
                FileName = "Test.pdf"
            };

        }


        public ActionResult PrintNewsId(string title = "") {

            ActionResult view;
            try
            {
                NewsHandler dataAccess = new NewsHandler();
                RssFeedHandler dataHandler = new RssFeedHandler();
                List<string> sources = new List<string>();
                sources.Add("https://www.nasa.gov/rss/dyn/educationnews.rss");
                sources.Add("https://www.nasa.gov/rss/dyn/hurricaneupdate.rss");
                sources.Add("https://www.nasa.gov/rss/dyn/solar_system.rss");
                List<string> newsFromInternetHeaders = new List<string>();
                newsFromInternetHeaders.Add("Noticias Educativas");
                newsFromInternetHeaders.Add("Noticias del Mundo");
                newsFromInternetHeaders.Add("Noticias del Espacio");

                List<EventModel> feeds = new List<EventModel>();

                for (int i = 0; i < sources.Count; i++) {

                    feeds.AddRange(dataHandler.GetRssFeed(sources[i]));

                }

                EventModel news = feeds.Find(smodel => String.Equals(smodel.Title, title));
                if (news == null)
                {
                    view = RedirectToAction("ListNews");
                }
                else
                {
                    ViewBag.SelectedNews = news;
                    view = View(ViewBag);

                }
            }
            catch
            {
                view = RedirectToAction("ListNews");
            }
            return view;

        }

        public ActionResult PrintAction(string title)
        {
            NewsHandler dataAccess = new NewsHandler();
            RssFeedHandler dataHandler = new RssFeedHandler();
            List<string> sources = new List<string>();
            sources.Add("https://www.nasa.gov/rss/dyn/educationnews.rss");
            sources.Add("https://www.nasa.gov/rss/dyn/hurricaneupdate.rss");
            sources.Add("https://www.nasa.gov/rss/dyn/solar_system.rss");
            List<string> newsFromInternetHeaders = new List<string>();
            newsFromInternetHeaders.Add("Noticias Educativas");
            newsFromInternetHeaders.Add("Noticias del Mundo");
            newsFromInternetHeaders.Add("Noticias del Espacio");

            List<EventModel> feeds = new List<EventModel>();

            for (int i = 0; i < sources.Count; i++)
            {

                feeds.AddRange(dataHandler.GetRssFeed(sources[i]));

            }

            EventModel news = feeds.Find(smodel => String.Equals(smodel.Title, title));

            ViewBag.SelectedNews = news;
            return new Rotativa.ViewAsPdf("PrintNewsId");


        }


        [HttpGet]
        public ActionResult News(string title)
        {
            ActionResult view;
            try
            {
                NewsHandler dataAccess = new NewsHandler();
                NewsModel news = dataAccess.GetAllNews().Find(smodel => String.Equals(smodel.Title, title));
                if (news == null)
                {
                    view = RedirectToAction("ListNews");
                }
                else
                {
                    ViewBag.News = news;
                    view = View(news);
                }
            }
            catch
            {
                view = RedirectToAction("ListNews");
            }
            return view;
        }

        public JsonResult GetTopicsList(string category)
        {
            List<SelectListItem> topicsList = new List<SelectListItem>();
            List<string> topicsFromCategory = DataAccess.GetTopicsByCategory(category);

            foreach (string topic in topicsFromCategory)
            {
                topicsList.Add(new SelectListItem { Text = topic, Value = topic });
            }
            return Json(new SelectList(topicsList, "Value", "Text"));
        }

        private List<SelectListItem> LoadCategories()
        {
            List<string> categories = DataAccess.GetAllCategories();
            List<SelectListItem> dropdownCategories = new List<SelectListItem>();
            foreach (string category in categories)
            {
                dropdownCategories.Add(new SelectListItem { Text = category, Value = category });
            }
            return dropdownCategories;
        }

        public ActionResult SubmitNewsForm()
        {
            ViewData["category"] = LoadCategories();
            return View();
        }

        [HttpPost]
        public ActionResult PostNews(NewsModel news)
        {
            ActionResult view = RedirectToAction("Success", "Home");
            LoadNewsWithForm(news);
            ViewBag.SuccessOnCreation = false;
            try
            {
                ViewBag.SuccessOnCreation = this.DataAccess.PublishNews(news);
                if (ViewBag.SuccessOnCreation)
                {
                    ModelState.Clear();
                    view = RedirectToAction("Success", "Home");
                }
            }
            catch
            {
                TempData["Error"] = true;
                TempData["WarningMessage"] = "Algo salió mal";
                view = RedirectToAction("SubmitNewsForm", "News");
            }
            return view;
        }

        private void LoadNewsWithForm(NewsModel news)
        {
            news.Category = Request.Form["Category"].Replace(" ", "_");
            news.Topics = ContentParser.GetListFromString(Request.Form["inputTopicString"]);
            news.Title = Request.Form["title"];
            news.Description = Request.Form["description"];
            news.Content = Request.Form["content"];
            news.ImagesRef = ContentParser.GetListFromString(Request.Form["imagesString"]);
        }

        [HttpPost]
        public ActionResult UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                string filePath = file.FileName.Replace("_", "-").Replace(" ", "-");
                file.SaveAs(Path.Combine(Server.MapPath("~/images/news"), filePath));
            }
            return Json("Files uploaded successfully");
        }




    }
}