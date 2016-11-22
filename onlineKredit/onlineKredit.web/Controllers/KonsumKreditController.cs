using onlineKredit.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Controllers
{
    public class KonsumKreditController : Controller
    {
        [HttpGet]
        public ActionResult KreditRahmen()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKredit - KreditRahmen");
            Debug.Unindent();
            return View();
        }

        [HttpPost]
        public ActionResult KreditRahmen(KreditRahmenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKredit - KreditRahmen");

            /// Speichere Daten in die BusinessLogic
            
            /// gehe zum nächsten Schritt
            
            
            Debug.Unindent();

            return RedirectToAction("FinanhzielleSituation");
        }

        [HttpGet]
        public ActionResult FinanzielleSituation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FinanzielleSituation(FinanzielleSituationModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult PersoenlicheDaten()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PersoenlicheDaten(PersoenlicheDatenModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Arbeitgeber()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult KontaktDaten()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KontaktDaten(KontaktDatenModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult KontoInformation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KontoInformation(KontoInformationenModel model)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ZusammenFassung()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ZusammenFassung(ZusammenFassungModel  model)
        {
            return View();
        }
    }
}