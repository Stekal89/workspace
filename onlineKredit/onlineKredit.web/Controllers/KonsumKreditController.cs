using onlineKredit.logic;
using onlineKredit.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc;
using onlineKredit.freigabe;

namespace onlineKredit.web.Controllers
{
    public class KonsumKreditController : Controller
    {
        #region KreditRahmen

        [HttpGet]
        public ActionResult KreditRahmen()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KreditRahmen");
            Debug.Unindent();
            
            KreditRahmenModel model = new KreditRahmenModel()
            {
                GewuenschterBetrag = 5000,  // default Werte
                Laufzeit = 36   // default Werte
            };

            int id = -1;

            if (Request.Cookies["KundenID"] != null && int.TryParse(Request.Cookies["KundenID"].Value, out id))
            {
                // Lade Daten aus der Datenbank
                Kredit wunsch = KonsumKreditVerwaltung.KreditRahmenLaden(id);
                model.GewuenschterBetrag = (int)wunsch.GewuenschterKredit;
                model.Laufzeit = wunsch.GewuenschteLaufzeit;
            }

            return View(model);
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
                    //TempData["idKunde"] = neuerKunde.ID;

                    /// ich benötige für alle weiteren Schritte die ID
                    /// des angelegten Kunden. Damit ich diese bei der nächsten Action
                    /// habe, speichere ich sie für diesen Zweck in ein Cookie
                    Response.Cookies.Add(new HttpCookie("kundenID", neuerKunde.ID.ToString()));

                    /// gehe zum nächsten Schritt
                    if (!HomeController.alleDatenAngegeben)
                        return RedirectToAction("FinanzielleSituation");
                    else
                        return RedirectToAction("ZusammenFassung");
                }
            }
            
            /// falls der ModelState NICHT valid ist, bleibe hier und
            /// gib die Daten (falls vorhanden) wieder auf das UI
            return View(model);
        }

        #endregion

        #region FinanzielleSituation

        [HttpGet]
        public ActionResult FinanzielleSituation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - FinanzielleSituation");
            Debug.Unindent();
            
            FinanzielleSituationModel model = new FinanzielleSituationModel()
            {
                //KundenID = int.Parse(TempData["idKunde"].ToString())
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            /// Rufe Verwaltung mit Kunden ID auf
            /// Wenn es zu dieser KundenID Persönlichen Daten gibt
            /// gib diese zurück
            FinanzielleSituation finanzielleSituation = KonsumKreditVerwaltung.FinanzielleSituationLaden(model.KundenID);
            
            if (finanzielleSituation != null)
            {
                model.NettoEinkommen = (double)finanzielleSituation.MonatsEinkommenNetto;
                model.Wohnkosten = (double)finanzielleSituation.Wohnkosten.Value;
                model.EinkuenfteAlimenteUnterhalt = (double)finanzielleSituation.SonstigeEinkommen;
                model.UnterhaltsZahlungen = (double)finanzielleSituation.UnterhaltsZahlungen;
                model.RatenVerpflichtungen = (double)finanzielleSituation.RatenZahlungen;
            }

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
                    //TempData["idKunde"] = model.KundenID;

                    if (!HomeController.alleDatenAngegeben)
                        return RedirectToAction("PersoenlicheDaten");
                    else
                        return RedirectToAction("ZusammenFassung");
                }
            }   
            return View(model);
        }

        #endregion

        #region PersoenlicheDaten

        /// <summary>
        /// Fügt dem mitgegebenen PersoenlicheDaten Model die Daten von den Lookup-Tabellen
        /// hinzu.
        /// Seit dem ich im "PersoenlichenDatenModel" die eigene Validierung für das Datum geschrieben habe,
        /// werden die Lookup-Daten vom "model" auf "NULL" gesetzt, somit muss ich im [HttpPOST], noch mal 
        /// die Lookup-Daten für die Dropdownlisten neu zuweisen.
        /// </summary>
        /// <param name="model">Ein Modell für die persönlichen Daten eines Users</param>
        /// <returns>Lookuptabellendaten</returns>
        private PersoenlicheDatenModel PersoenlicheDatenLookup(PersoenlicheDatenModel model)
        {
            List<TitelModel> alleTitelAngabenWeb = new List<TitelModel>();

            /// In der BL habe ich Methoden/Funktionen geschrieben, die es mir erlauben die Daten 
            /// aus den Lookup-Tabellen in meiner Datenbank zu laden.
            /// die Schnittstelle von Datenbank zu Projekt.
            foreach (var titelAngabeWeb in KonsumKreditVerwaltung.TitelLaden())
            {
                alleTitelAngabenWeb.Add(new TitelModel()
                {
                    ID = titelAngabeWeb.ID.ToString(),
                    Bezeichnung = titelAngabeWeb.Bezeichnung
                });
            }

            List<LaenderModel> alleLaenderAngabenWeb = new List<LaenderModel>();

            foreach (var landAngabeWeb in KonsumKreditVerwaltung.LaenderLaden())
            {
                alleLaenderAngabenWeb.Add(new LaenderModel()
                {
                    ID = landAngabeWeb.ID.ToString(),
                    Bezeichnung = landAngabeWeb.Bezeichnung
                });
            }

            List<FamilienStandsModel> alleFamilienStaendeAngabenWeb = new List<FamilienStandsModel>();

            foreach (var familienStandAngabeWeb in KonsumKreditVerwaltung.FamilienstaendeLaden())
            {
                alleFamilienStaendeAngabenWeb.Add(new FamilienStandsModel()
                {
                    ID = familienStandAngabeWeb.ID.ToString(),
                    Bezeichnung = familienStandAngabeWeb.Bezeichnung
                });
            }

            List<SchulabschlussModel> alleSchulabschluesseAngabenWeb = new List<SchulabschlussModel>();

            foreach (var schulAbschlussAngabeWeb in KonsumKreditVerwaltung.SchulabschluesseLaden())
            {
                alleSchulabschluesseAngabenWeb.Add(new SchulabschlussModel()
                {
                    ID = schulAbschlussAngabeWeb.ID.ToString(),
                    Bezeichnung = schulAbschlussAngabeWeb.Bezeichnung
                });
            }

            List<IdentifikationsArtModel> alleIdentifikationsArtenAngabenWeb = new List<IdentifikationsArtModel>();

            foreach (var identifikationsArtWeb in KonsumKreditVerwaltung.IdentifikationsArtenLaden())
            {
                alleIdentifikationsArtenAngabenWeb.Add(new IdentifikationsArtModel()
                {
                    ID = identifikationsArtWeb.ID.ToString(),
                    Bezeichnung = identifikationsArtWeb.Bezeichnung
                });
            }

            List<WohnartModel> alleWohnartenAngabenWeb = new List<WohnartModel>();

            foreach (var wohnartsAngabeWeb in KonsumKreditVerwaltung.WohnartenLaden())
            {
                alleWohnartenAngabenWeb.Add(new WohnartModel()
                {
                    ID = wohnartsAngabeWeb.ID.ToString(),
                    Bezeichnung = wohnartsAngabeWeb.Bezeichnung
                });
            }

            model.AlleTitelAngabenWeb = alleTitelAngabenWeb;
            model.AlleLaenderAngabenWeb = alleLaenderAngabenWeb;
            model.AlleFamilienstandsAngabenWeb = alleFamilienStaendeAngabenWeb;
            model.AlleSchulabschlussAngabenWeb = alleSchulabschluesseAngabenWeb;
            model.AlleIdentifikationsArtAngabenWeb = alleIdentifikationsArtenAngabenWeb;
            model.AlleWohnartsAngabenWeb = alleWohnartenAngabenWeb;
            model.KundenID = int.Parse(Request.Cookies["kundenID"].Value);

            return model;
        }

        [HttpGet]
        public ActionResult PersoenlicheDaten()
        {
            /// erzeugt das Model für die persönlichen Daten und 
            /// fügt dem die Daten für die Lookup-Tabellen hinzu.
            /// Id des Kunden wird auch mit übergeben
            PersoenlicheDatenModel model = new PersoenlicheDatenModel();
            model = PersoenlicheDatenLookup(model); /// Lade LookupDaten für die Dropdownlist-Elemente

            /// Rufe Verwaltung mit Kunden ID auf
            /// Wenn es zu dieser KundenID Persönlichen Daten gibt
            /// gib diese zurück
            Kunde persoenlicheDaten = KonsumKreditVerwaltung.PersoenlicheDatenLaden(model.KundenID);

            if (persoenlicheDaten != null)
            {
                /// Überprüfe welches Geschlecht gespeichert ist
                if (persoenlicheDaten.Geschlecht == "w")
                    model.Geschlecht = Geschlecht.Weiblich;
                else
                    model.Geschlecht = Geschlecht.Männlch;

                model.ID_Titel = persoenlicheDaten.FKTitel.HasValue ? persoenlicheDaten.FKTitel.Value : 0;
                model.Vorname = persoenlicheDaten.Vorname;
                model.Nachname = persoenlicheDaten.Nachname;
                model.GeburtsDatum = persoenlicheDaten.Geburtsdatum;
                model.ID_Staatsbuergerschaft = persoenlicheDaten.FKStaatsangehoerigkeit;
                model.AnzahlKinder = persoenlicheDaten.AnzahlKinder.HasValue ? (int)persoenlicheDaten.AnzahlKinder : 0;
                model.ID_Familienstand = persoenlicheDaten.FKFamilienstand.HasValue ? (int)persoenlicheDaten.FKFamilienstand : 0;
                model.ID_Wohnart = persoenlicheDaten.FKWohnart.HasValue ? (int)persoenlicheDaten.FKWohnart : 0;
                model.ID_SchulAbschluss = persoenlicheDaten.FKSchulabschluss.HasValue ? (int)persoenlicheDaten.FKSchulabschluss : 0;
                model.ID_IdentifikationsArt = persoenlicheDaten.FKIdentifikationsArt.HasValue ? (int)persoenlicheDaten.FKIdentifikationsArt : 0;
                model.IdentifikationsNummer = persoenlicheDaten.IdentifikationsNummer;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersoenlicheDaten(PersoenlicheDatenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - PersoenlicheDaten");
            Debug.Unindent();
            
            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.PersoenlicheDatenSpeichern(
                    model.Geschlecht == Geschlecht.Männlch ? "m" : "w",
                    model.ID_Titel,
                    model.Vorname,
                    model.Nachname,
                    model.GeburtsDatum,
                    model.ID_Staatsbuergerschaft,
                    model.AnzahlKinder,
                    model.ID_Familienstand,
                    model.ID_Wohnart,
                    model.ID_SchulAbschluss,
                    model.ID_IdentifikationsArt,
                    model.IdentifikationsNummer,
                    model.KundenID
                    ))
                {
                    if (!HomeController.alleDatenAngegeben)
                        return RedirectToAction("KontaktDaten");
                    else
                        return RedirectToAction("ZusammenFassung");
                }
            }

            List<TitelModel> alleTitelAngabenWeb = new List<TitelModel>();

            foreach (var titelAngabeWeb in KonsumKreditVerwaltung.TitelLaden())
            {
                alleTitelAngabenWeb.Add(new TitelModel()
                {
                    ID = titelAngabeWeb.ID.ToString(),
                    Bezeichnung = titelAngabeWeb.Bezeichnung
                });
            }

            model = PersoenlicheDatenLookup(model);/// Daten für die Dropdownlisten

            return View(model);
        }

        #endregion

        #region KontaktDaten

        /// <summary>
        /// Ladet Alle Lookuptabellendaten von der BusinessLogic und gibt diese zurück
        /// </summary>
        /// <param name="model">KontaktDatenModel</param>
        /// <returns>LookupTabellenDaten</returns>
        private KontaktDatenModel KontaktDatenLookup(KontaktDatenModel model)
        {
            List<OrtModel> alleOrtsAngabenWeb = new List<OrtModel>();

            foreach (var ortsAngabenWeb in KonsumKreditVerwaltung.OrteLaden())
            {
                alleOrtsAngabenWeb.Add(new OrtModel()
                {
                    ID = ortsAngabenWeb.ID.ToString(),
                    Bezeichnung = ortsAngabenWeb.Bezeichnung,
                    FK_Land = ortsAngabenWeb.FKLand,
                    PostleitZahl = ortsAngabenWeb.PLZ,
                    /// in diesem Feld werden alle Postleitzahlen und Orte verkettet in 
                    /// einer Zeichenkette gespeichert, damit ich diese auf der 
                    /// Oberfläche anzeigen lassen kann.
                    PLZUndOrt = ortsAngabenWeb.PLZ + " " + ortsAngabenWeb.Bezeichnung
                });
            }
            model.AlleOrtsAngabenWeb = alleOrtsAngabenWeb;
            model.KundenID = int.Parse(Request.Cookies["kundenID"].Value);

            return model;
        }

        [HttpGet]
        public ActionResult KontaktDaten()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontaktDaten");
            Debug.Unindent();
            
            KontaktDatenModel model = new KontaktDatenModel();

            /// Daten für die Dropdownliste
            model = KontaktDatenLookup(model);

            KontaktDaten kontaktDaten = KonsumKreditVerwaltung.KontaktDatenLaden(int.Parse(Request.Cookies["KundenID"].Value));

            if (kontaktDaten != null)
            {
                /// Weise dem Model die aktuellen Kundendaten zu, die der User beliebig ändern kann
                model.Strasse = kontaktDaten.Strasse;
                model.Hausnummer = kontaktDaten.Hausnummer;
                model.Stiege = kontaktDaten.Stiege;
                model.Tuer = kontaktDaten.Tür;
                model.FK_Ort = (int)kontaktDaten.FKOrt;
                model.EMail = kontaktDaten.EMail;
                model.TelefonNummer = kontaktDaten.Telefonnummer;
                model.KundenID = kontaktDaten.ID;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontaktDaten(KontaktDatenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontaktDaten");
            Debug.Unindent();

            /// Daten für die Dropdownliste
            model = KontaktDatenLookup(model);

            /// Weist dem Model den Fremdschlüssel für die Eigenschaft FK_Ort zu
            /// "ComboBox"
            foreach (var item in model.AlleOrtsAngabenWeb)
            {
                if (item.PLZUndOrt == model.PLZUndOrtInText)
                {
                    int fk;
                    int.TryParse(item.ID, out fk);
                    model.FK_Ort = fk;
                    break;
                }
            }

            Kunde aktKunde = null;

                /// Dient zur überprüfung ob eine Änderung vorgenommen wurde
                aktKunde = KonsumKreditVerwaltung.KundenDatenLaden(int.Parse(Request.Cookies["KundenID"].Value));


            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.KontaktDatenSpeichern(model.Strasse,
                                                                    model.Hausnummer,
                                                                    model.Stiege,
                                                                    model.Tuer,
                                                                    model.FK_Ort,
                                                                    model.EMail,
                                                                    model.TelefonNummer,
                                                                    model.KundenID
                    ))
                {
                    if (!HomeController.alleDatenAngegeben)
                        return RedirectToAction("Arbeitgeber");
                    else
                        return RedirectToAction("ZusammenFassung");
                }
            }
                
            return View(model);
        }

        #endregion

        #region Arbeitgeber

        /// <summary>
        /// Fügt dem mitgegebenen Arbeitgeber-Model die Daten von den Lookup-Tabellen
        /// hinzu. Ladet die Daten aus der Datenbank
        /// </summary>
        /// <param name="model">Ein Modell für die Arbeitgeberdaten eines Users</param>
        /// <returns>Lookuptabellendaten</returns>
        private ArbeitgeberModel ArbeitgeberLookup(ArbeitgeberModel model)
        {
            /// In der BL habe ich Methoden/Funktionen geschrieben, die es mir erlauben die Daten 
            /// aus den Lookup-Tabellen in meiner Datenbank zu laden.
            /// die Schnittstelle von Datenbank zu Projekt.
            List<BeshaeftigungsArtModel> alleBeschaeftugungsArtenAngabenWeb = new List<BeshaeftigungsArtModel>();

            foreach (var beschaeftigungsArtenAngabenWeb in KonsumKreditVerwaltung.BeschaeftigungsArtenLaden())
            {
                alleBeschaeftugungsArtenAngabenWeb.Add(new BeshaeftigungsArtModel()
                {
                    ID = beschaeftigungsArtenAngabenWeb.ID.ToString(),
                    Bezeichnung = beschaeftigungsArtenAngabenWeb.Bezeichnung
                });
            }

            List<BrancheModel> alleBranchenAngabenWeb = new List<BrancheModel>();

            foreach (var branchnenAngabenWeb in KonsumKreditVerwaltung.BranchenLaden())
            {
                alleBranchenAngabenWeb.Add(new BrancheModel()
                {
                    ID = branchnenAngabenWeb.ID.ToString(),
                    Bezeichnung = branchnenAngabenWeb.Bezeichnung
                });
            }

            model.AlleBeschaeftigungsArtAngabenWeb = alleBeschaeftugungsArtenAngabenWeb;
            model.AlleBranchenAngabenWeb = alleBranchenAngabenWeb;
            model.KundenID = int.Parse(Request.Cookies["kundenID"].Value);

            return model;
        }

        [HttpGet]
        public ActionResult Arbeitgeber()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - Arbeitgeber");
            Debug.Unindent();

            ArbeitgeberModel model = new ArbeitgeberModel();
                
            /// Daten für die DropdownListen/Lookuptabellen
            model = ArbeitgeberLookup(model);

            Arbeitgeber arbeitgeber = KonsumKreditVerwaltung.ArbeitgeberLaden(int.Parse(Request.Cookies["KundenID"].Value));
                
            if (arbeitgeber != null)
            {
                model.Firma = arbeitgeber.Firma;
                model.ID_BeschaeftigungsArt = arbeitgeber.FKBeschaeftigungsArt;
                model.ID_Branche = (int)arbeitgeber.FKBranche;
                model.BeschaeftigtSeit = (DateTime)arbeitgeber.BeschaeftigtSeit;
                model.KundenID = int.Parse(Request.Cookies["KundenID"].Value);
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - Arbeitgeber");
            Debug.Unindent();

            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.ArbeitgeberSpeichern(
                                                                model.Firma,
                                                                model.ID_BeschaeftigungsArt,
                                                                model.ID_Branche,
                                                                model.BeschaeftigtSeit,
                                                                model.KundenID
                    ))
                {
                    if (!HomeController.alleDatenAngegeben)
                        return RedirectToAction("KontoVerfuegbar");
                    else
                        return RedirectToAction("ZusammenFassung");
                }
            }

            return View(model);
        }

        #endregion

        #region KontoIformation

        /// <summary>
        /// Hier wird mittels Dropdown abgefragt, ob der Kunde:
        ///     - ein vorhandenes Deutsche Ban AG - Konto verwenden möchte
        ///     - ein neues Konto bei der Deutschen Bank AG anlegen möchte
        ///       und dieses dann für den Konsum-Kredit vewenden möchte
        ///     - oder ein anderes Konto einer anderen Bank verwenden möchte
        /// </summary>
        /// <returns>Je nach Entscheidung zum richtigen Controller / anderfalls das "model" in der akt. View</returns>
        [HttpGet]
        public ActionResult KontoVerfuegbar()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            List<KontoAbfrageMoeglichkeitModel> alleKontoAbfrageMoeglichkeitenWeb = new List<KontoAbfrageMoeglichkeitModel>();

            /// Lade alle Kontoabfrage-Möglichkeiten aus der Businesslogic in 
            /// die Liste "alleKontoAbfrageMoeglichkeitenWeb".
            foreach (var kontoAbfrageMoeglichkeitWeb in KonsumKreditVerwaltung.KontoAbfrageMoeglichkeitenLaden())
            {
                alleKontoAbfrageMoeglichkeitenWeb.Add(new KontoAbfrageMoeglichkeitModel()
                {
                    ID = kontoAbfrageMoeglichkeitWeb.ID.ToString(),
                    Bezeichnung = kontoAbfrageMoeglichkeitWeb.Bezeichnung
                });
            }

            /// Erzeuge das Model und weise der Liste alle wichtigen Details zu (ID´s, Bezeichnungen)
            KontoAbfrageModel model = new KontoAbfrageModel()
            {
                AlleKontoAbfrageMoeglichkeitenAngaben = alleKontoAbfrageMoeglichkeitenWeb,
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            Debug.Unindent();

            return View(model);
        }

        [HttpPost]
        public ActionResult KontoVerfuegbar(KontoAbfrageModel model)
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Indent();

            int auswahl = model.ID_KontoAbfrage;

            if (ModelState.IsValid)
            {
                if (auswahl == 1)
                {
                    Debug.WriteLine("Auswahl des Kontos:");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.WriteLine("Auswahl: \"Vorhandenes Konto der Deutschen Bank AG\"");
                    Debug.Unindent();

                    return RedirectToAction("HatDeutscheBankKontoInformation");
                }
                else if (auswahl == 2)
                {
                    Debug.WriteLine("Auswahl des Kontos:");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.WriteLine("Auswahl: \"Neues Konto bei Deutsche Bank AG anlegen.\"");
                    Debug.Unindent();

                    return RedirectToAction("NeuesDeutscheBankKontoInformation");
                }
                else if (auswahl == 3)
                {
                    Debug.WriteLine("Auswahl des Kontos:");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.WriteLine("Auswahl: \"Anderes Konto verwenden.\"");
                    Debug.Unindent();

                    return RedirectToAction("AndereKontoInformation");
                }
                else
                {
                    Debug.WriteLine("Irgendetwas ist Schief gegangen....");
                    Debug.Indent();
                    Debug.WriteLine("ID: " + model.ID_KontoAbfrage);
                    Debug.Unindent();
                    Debugger.Break();
                }
            }


            Debug.Unindent();

            return View(model);
        }

        /// <summary>
        /// Hier darf der User die Daten seines Vorhandenen Deutsche Bank AG - Kontos
        /// angeben, Das Eingabefeld Bank wird sichtbar für den User da gestellt, aber
        /// nicht veränderbar sein (Label)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HatDeutscheBankKontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - HatDeutscheBankKontoInformation");
            Debug.Unindent();

            DeutscheBankKontoInformationModel model = new DeutscheBankKontoInformationModel()
            {
                KundenID = int.Parse(Request.Cookies["kundenID"].Value),
                BankInstitut = "Deutsche Bank AG",
                IstDeutscheBankKunde = true
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HatDeutscheBankKontoInformation(DeutscheBankKontoInformationModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - HatDeutscheBankKontoInformation");
            Debug.Unindent();

            model.IBAN = KonsumKreditVerwaltung.FilterAufVorhandeneLeerzeichen(model.IBAN);
            model.IBAN = KonsumKreditVerwaltung.LeerzeichenEinfuegen(model.IBAN);

            if (ModelState.IsValid)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID
                                                                        ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Hier bekommt der User einen Vorschlag für sein neues 
        /// Deutsche Bank AG - Kontos und kann dieses mit "erstellen" 
        /// bestätigen.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NeuesDeutscheBankKontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            List<string> bicUndIban = new List<string>();

            bicUndIban = KonsumKreditVerwaltung.BankKontoErzeugen();
            
            DeutscheBankKontoInformationModel model = new DeutscheBankKontoInformationModel()
            {
                BIC = bicUndIban[0],
                IBAN = bicUndIban[1],
                IstDeutscheBankKunde = true,
                BankInstitut = "Deutsche Bank AG",
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NeuesDeutscheBankKontoInformation(DeutscheBankKontoInformationModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID
                                                                        ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }

            return View(model);
        }

        /// <summary>
        /// Hier kann der User die ganzen benötigten Konto-Informationene selbst eingeben,
        /// muss allerdings seine Bank angeben.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AndereKontoInformation()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            KontoInformationenModel model = new KontoInformationenModel()
            {
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AndereKontoInformation(KontoInformationenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontoInformation");
            Debug.Unindent();

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID
                                                                        ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }

            return View(model);
        }
        
        #endregion

        #region ZusammenFassung

        [HttpGet]
        public ActionResult ZusammenFassung()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - ZusammenFassung");
            Debug.Unindent();

            HomeController.alleDatenAngegeben = true;

            ZusammenFassungModel model = new ZusammenFassungModel();
            model.KundenID = int.Parse(Request.Cookies["KundenID"].Value);

            Kunde aktKunde = KonsumKreditVerwaltung.KundenDatenLaden(model.KundenID);

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
            model.AnzahlKinder = (int)aktKunde.AnzahlKinder;
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
            model.BIC = aktKunde.KontoDaten.BIC;
            model.IBAN = aktKunde.KontoDaten.IBAN;
            model.BankInstitut = aktKunde.KontoDaten.Bank;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AngabeBestaetigung(int id, bool? bestaetigt)
        {
            if (bestaetigt.HasValue && bestaetigt.Value)
            {
                Debug.Indent();
                Debug.WriteLine("POST - KonsumKreditController - ZusammenFassung");


                //int idKunde = int.Parse(Request.Cookies["KundenID"].Value);
                Kunde aktKunde = KonsumKreditVerwaltung.KundenDatenLaden(id);
                Response.Cookies.Remove("KundenID");

                bool kreditBewilligt = KreditFreigabe.FreigabePruefen(
                                                          aktKunde.Geschlecht,
                                                            aktKunde.Vorname,
                                                            aktKunde.Nachname,
                                                            aktKunde.Familienstand.Bezeichnung,
                                                            (double)aktKunde.FinanzielleSituation.MonatsEinkommenNetto,
                                                            (double)aktKunde.FinanzielleSituation.Wohnkosten,
                                                            (double)aktKunde.FinanzielleSituation.SonstigeEinkommen,
                                                            (double)aktKunde.FinanzielleSituation.UnterhaltsZahlungen,
                                                            (double)aktKunde.FinanzielleSituation.RatenZahlungen);

                /// Rüfe Service/DLL auf und prüfe auf Kreditfreigabe
                Debug.WriteLine($"Kreditfreigabe {(kreditBewilligt ? "" : "nicht")}erteilt!");

                Debug.Unindent();
                return RedirectToAction("BewilligungsAusgabe", "Freigabe", new { bewilligt = kreditBewilligt });

            }
            else
            {
                Debug.Indent();
                Debug.WriteLine("POST - KonsumKreditController - ZusammenFassung");
                Debug.Indent();
                Debug.WriteLine("Der bool \"bestaetigt\" hat keinen gültigen Wert, oder ist auf \"FALSE\" gesetzt!!!");
                Debug.Unindent();
                Debug.Unindent();
                return RedirectToAction("Zusammenfassung");
            }
        }

        #endregion
    }
}
