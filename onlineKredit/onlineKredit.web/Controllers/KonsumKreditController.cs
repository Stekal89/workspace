using onlineKredit.logic;
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
            Debug.Unindent();

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                Kunde neuerKunde = KonsumKreditVerwaltung.ErzeugeKunde();

                if (neuerKunde != null && KonsumKreditVerwaltung.KreditRahmenSpeichern(model.GewuenschterBetrag, model.Laufzeit, neuerKunde.ID))
                {
                    /// ich benötige für alle weiteren Schritte die ID
                    /// des angelegten Kunden. Damit ich diese bei der nächsten Action
                    /// habe, speichere ich sie für diesen Zweck in die TempData Variable
                    /// (ähnlich wie Session)
                    TempData["idKunde"] = neuerKunde.ID;

                    /// gehe zum nächsten Schritt
                    return RedirectToAction("FinanzielleSituation");
                }
            }

            /// falls der ModelState NICHT valid ist, bleibe hier und
            /// gib die Daten (falls vorhanden) wieder auf das UI
            return View(model);
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