using System.Configuration;
using Models;
using NLog;
using Repository;
using Service;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhoneBookService _service;

        public HomeController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PhoneBookDb"].ConnectionString;
            var logger = LogManager.GetCurrentClassLogger();
            var repository = new PhoneBookRepository(connectionString, logger);
            _service = new PhoneBookService(repository, logger);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetContacts()
        {
            List<Contact> contacts = _service.GetContacts();
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
