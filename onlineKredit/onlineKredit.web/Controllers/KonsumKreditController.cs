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
            Debug.WriteLine("GET - KonsumKreditController - KreditRahmen");
            Debug.Unindent();
            return View();
        }

        [HttpPost]
        public ActionResult KreditRahmen(KreditRahmenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KreditRahmen");
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
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - FinanzielleSituation");
            Debug.Unindent();

            FinanzielleSituationModel model = new FinanzielleSituationModel()
            {
                KundenID = int.Parse(TempData["idKunde"].ToString())
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinanzielleSituation(FinanzielleSituationModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - FinanzielleSituation");
            Debug.Unindent();


            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.FinanzielleSituationSpeichern(model.NettoEinkommen,
                                                                        model.Wohnkosten, 
                                                                        model.EinkuenfteAlimenteUnterhalt, 
                                                                        model.UnterhaltsZahlungen, 
                                                                        model.RatenVerpflichtungen,
                                                                        model.KundenID))
                {
                    TempData["idKunde"] = model.KundenID;
                    return RedirectToAction("PersoenlicheDaten");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult PersoenlicheDaten()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersoenlicheDaten(PersoenlicheDatenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - PersoenlicheDaten");
            Debug.Unindent();

            return View();
        }

        [HttpGet]
        public ActionResult Arbeitgeber()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - Arbeitgeber");
            Debug.Unindent();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - Arbeitgeber");
            Debug.Unindent();

            return View();
        }

        [HttpGet]
        public ActionResult KontaktDaten()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontaktDaten");
            Debug.Unindent();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontaktDaten(KontaktDatenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontaktDaten");
            Debug.Unindent();

            return View();
        }

        [HttpGet]
        public ActionResult KontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontoInformation(KontoInformationenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            return View();
        }

        [HttpGet]
        public ActionResult ZusammenFassung()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - ZusammenFassung");
            Debug.Unindent();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ZusammenFassung(ZusammenFassungModel  model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - ZusammenFassung");
            Debug.Unindent();

            return View();
        }
    }
}