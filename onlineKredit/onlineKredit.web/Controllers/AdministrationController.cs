using onlineKredit.logic;
using onlineKredit.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace onlineKredit.web.Controllers
{
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult Anmelden()
        {
            Debug.Indent();
            Debug.WriteLine("GET - AdministrationController - Anmelden");
            Debug.Unindent();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anmelden(LogInModel model)
        {
            Debug.WriteLine("POST - AdministrationController - Anmelden");

            if (ModelState.IsValid)
            {
                if (model.Benutzername == "admin"
                    && model.Passwort == "admin")
                {
                    //FormsAuthentication.SetAuthCookie("admin", true);
                    return RedirectToAction("KreditAntraege");
                }
                else
                {
                    ModelState.AddModelError("Benutzername", "Ungültiger Benutzername/Passwort!");
                }
            }

            return View(model);
        }

        [HttpGet]
        ////[Authorize]
        public ActionResult KreditAntraege()
        {
            Debug.WriteLine("GET - Administration - KreditAnträge");

            /// lade aus der DB die letzten 10 Kreditanträge
            /// 

            List<Kunde> alleKunden = KonsumKreditVerwaltung.KundenListeLaden();
            List<ZusammenFassungModel> alleKundenModel = new List<ZusammenFassungModel>();

            foreach (var aktKunde in alleKunden)
            {
                ZusammenFassungModel model = new ZusammenFassungModel();

                /// KreditRahmen
                model.GewuenschterBetrag = (int)aktKunde.Kredit.GewuenschterKredit;
                model.Laufzeit = aktKunde.Kredit.GewuenschteLaufzeit;

                /// FinanuielleSituation
                model.NettoEinkommen = (double)aktKunde.FinanzielleSituation.MonatsEinkommenNetto;
                model.Wohnkosten = (double)aktKunde.FinanzielleSituation.Wohnkosten;
                model.EinkuenfteAlimenteUnterhalt = (double)aktKunde.FinanzielleSituation.UnterhaltsZahlungen;
                model.RatenVerpflichtungen = (double)aktKunde.FinanzielleSituation.RatenZahlungen;

                /// PersoenlicheDaten
                model.Geschlecht = aktKunde.Geschlecht == "m" ? "Herr" : "Frau";
                model.Titel = aktKunde.Titel?.Bezeichnung;
                model.Vorname = aktKunde.Vorname;
                model.Nachname = aktKunde.Nachname;
                model.GeburtsDatum = aktKunde.Geburtsdatum;
                model.Staatsbuergerschaft = aktKunde.Land.Bezeichnung;
                model.AnzahlKinder = aktKunde.AnzahlKinder.HasValue ? aktKunde.AnzahlKinder.Value : 0;
                model.Familienstand = aktKunde.Familienstand.Bezeichnung;
                model.Wohnart = aktKunde.Wohnart.Bezeichnung;
                model.SchulAbschluss = aktKunde.Schulabschluss.Bezeichnung;
                model.IdentifikationsArt = aktKunde.IdentifikationsArt.Bezeichnung;
                model.IdentifikationsNummer = aktKunde.IdentifikationsNummer;

                /// KontaktDaten
                model.Strasse = aktKunde.KontaktDaten.Strasse;
                model.Hausnummer = aktKunde.KontaktDaten.Hausnummer;
                model.Stiege = aktKunde.KontaktDaten?.Stiege;
                model.Tuer = aktKunde.KontaktDaten?.Tür;
                model.Ort = aktKunde.KontaktDaten.Ort.PLZ + " " + aktKunde.KontaktDaten.Ort.Bezeichnung;
                model.EMail = aktKunde.KontaktDaten?.EMail;
                model.TelefonNummer = aktKunde.KontaktDaten?.Telefonnummer;

                /// Arbeitgeber
                model.Firma = aktKunde.Arbeitgeber.Firma;
                model.BeschaeftigungsArt = aktKunde.Arbeitgeber.Beschaeftigungsart.Bezeichnung;
                model.Branche = aktKunde.Arbeitgeber.Branche.Bezeichnung;
                model.BeschaeftigtSeit = (DateTime)aktKunde.Arbeitgeber.BeschaeftigtSeit;

                /// KontoInformation
                if (aktKunde.KontoDaten != null)
                {
                    model.BIC = aktKunde.KontoDaten.BIC;
                    model.IBAN = aktKunde.KontoDaten.IBAN;
                    model.BankInstitut = aktKunde.KontoDaten.Bank;
                }

                if (aktKunde.KreditKarte != null)
                {
                    model.KreditKartenInhaber = aktKunde.KreditKarte.Inhaber;
                    model.KreditKartenNummer = aktKunde.KreditKarte.Nummer;
                    model.KreditKartenGueltigBis = aktKunde.KreditKarte.GueltigBis;
                }
                alleKundenModel.Add(model);
            }
            return View(alleKundenModel);
        }
    }
}