using Models;
using NLog;
using Repository;
using Service;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class PhoneBookController : Controller
    {
        private readonly PhoneBookService _service;

        public PhoneBookController()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["PhoneBookDb"].ConnectionString;
            var logger = LogManager.GetCurrentClassLogger();
            var repository = new PhoneBookRepository(connectionString, logger);
            _service = new PhoneBookService(repository, logger);
        }

        public ActionResult GetContacts()
        {
            List<Contact> contacts = _service.GetContacts();
            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetContactById(int id)
        {
            Contact contact = _service.GetContactById(id);
            return Json(contact, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(Contact model)
        {
            bool result = _service.SaveContact(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteById(int id)
        {
            bool result = _service.DeleteContact(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
