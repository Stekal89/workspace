using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Controllers
{
    public class FreigabeController : Controller
    {
        [HttpGet]
        public ActionResult BewilligungsAusgabe(bool bewilligt)
        {
            HomeController.alleDatenAngegeben = false;

            return View(bewilligt);
        }
    }
}