using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Dieser bool dient der Überprüfung, ob alle Daten schon mal Valide ausgefüllt wurden, damit man von der Zusammenfassung 
        /// aus ändern kann und wieder retour und die Änderungen wirksam werden
        /// </summary>
        public static bool alleDatenAngegeben = false;

        public ActionResult Index()
        {
            alleDatenAngegeben = false;


            return View();
        }
    }
}