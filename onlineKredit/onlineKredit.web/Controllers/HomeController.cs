using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Controllers
{
    public class HomeController : Controller
    {
        public static bool alleDatenAngegeben = false;

        public ActionResult Index()
        {
            alleDatenAngegeben = false;

            return View();
        }
    }
}