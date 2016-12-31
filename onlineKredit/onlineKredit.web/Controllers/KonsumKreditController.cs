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

            if (HomeController.alleDatenAngegeben)
            {
                ZusammenFassungModel zusammenFassungsmodel = new ZusammenFassungModel();
                zusammenFassungsmodel.KundenID = int.Parse(Request.Cookies["KundenID"].Value);

                Kunde aktKunde = KonsumKreditVerwaltung.KundenDatenLaden(zusammenFassungsmodel.KundenID);

                /// KreditRahmen
                zusammenFassungsmodel.GewuenschterBetrag = (int)aktKunde.Kredit.GewuenschterKredit;
                zusammenFassungsmodel.Laufzeit = aktKunde.Kredit.GewuenschteLaufzeit;

                KreditRahmenModel model = new KreditRahmenModel()
                {
                    GewuenschterBetrag = (int)zusammenFassungsmodel.GewuenschterBetrag,
                    Laufzeit = zusammenFassungsmodel.Laufzeit,
                    //KundenID = zusammenFassungsmodel.KundenID
                };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public ActionResult KreditRahmen(KreditRahmenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KreditRahmen");
            Debug.Unindent();

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                /// speichere Daten über BusinessLogic
                Kunde neuerKunde = KonsumKreditVerwaltung.ErzeugeKunde();

                if (neuerKunde != null && KonsumKreditVerwaltung.KreditRahmenSpeichern(model.GewuenschterBetrag, model.Laufzeit, neuerKunde.ID, false))
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
                    return RedirectToAction("FinanzielleSituation");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                int kundenID = int.Parse(Request.Cookies["kundenID"].Value);

                if (ModelState.IsValid && KonsumKreditVerwaltung.KreditRahmenSpeichern(model.GewuenschterBetrag, model.Laufzeit, kundenID, true))
                {
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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinanzielleSituation(FinanzielleSituationModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - FinanzielleSituation");
            Debug.Unindent();


            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.FinanzielleSituationSpeichern(model.NettoEinkommen,
                                                                        model.Wohnkosten,
                                                                        model.EinkuenfteAlimenteUnterhalt,
                                                                        model.UnterhaltsZahlungen,
                                                                        model.RatenVerpflichtungen,
                                                                        model.KundenID,
                                                                        false))
                {
                    //TempData["idKunde"] = model.KundenID;
                    return RedirectToAction("PersoenlicheDaten");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.FinanzielleSituationSpeichern(model.NettoEinkommen,
                                                                       model.Wohnkosten,
                                                                       model.EinkuenfteAlimenteUnterhalt,
                                                                       model.UnterhaltsZahlungen,
                                                                       model.RatenVerpflichtungen,
                                                                       model.KundenID,
                                                                       true))
                {
                    return RedirectToAction("ZusammenFassung");
                }
            }

            return View(model);
        }

        #endregion

        #region PersoenlicheDaten

        
        [HttpGet]
        public ActionResult PersoenlicheDaten()
        {
            #region LookupTabellenLaden


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

            #endregion

            /// erzeugt das Model für die persönlichen Daten und 
            /// fügt dem die Daten für die Lookup-Tabellen hinzu.
            /// Id des Kunden wird auch mit übergeben
            PersoenlicheDatenModel model = new PersoenlicheDatenModel()
            {
                AlleTitelAngabenWeb = alleTitelAngabenWeb,
                AlleLaenderAngabenWeb = alleLaenderAngabenWeb,
                AlleFamilienstandsAngabenWeb = alleFamilienStaendeAngabenWeb,
                AlleSchulabschlussAngabenWeb = alleSchulabschluesseAngabenWeb,
                AlleIdentifikationsArtAngabenWeb = alleIdentifikationsArtenAngabenWeb,
                AlleWohnartsAngabenWeb = alleWohnartenAngabenWeb,
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersoenlicheDaten(PersoenlicheDatenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - PersoenlicheDaten");
            Debug.Unindent();

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
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
                    return RedirectToAction("KontaktDaten");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
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
                    return RedirectToAction("ZusammenFassung");
                }
            }
            

            return View(model);
        }

        #endregion

        #region KontaktDaten

        [HttpGet]
        public ActionResult KontaktDaten()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - KontaktDaten");
            Debug.Unindent();

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

            KontaktDatenModel model = new KontaktDatenModel()
            {
                AlleOrtsAngabenWeb = alleOrtsAngabenWeb,
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontaktDaten(KontaktDatenModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - KontaktDaten");
            Debug.Unindent();

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontaktDatenSpeichern(model.Strasse,
                                                                    model.Hausnummer,
                                                                    model.Stiege,
                                                                    model.Tuer,
                                                                    model.FK_Ort,
                                                                    model.EMail,
                                                                    model.TelefonNummer,
                                                                    model.KundenID,
                                                                    false
                    ))
                {
                    return RedirectToAction("Arbeitgeber");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontaktDatenSpeichern(model.Strasse,
                                                                  model.Hausnummer,
                                                                  model.Stiege,
                                                                  model.Tuer,
                                                                  model.FK_Ort,
                                                                  model.EMail,
                                                                  model.TelefonNummer,
                                                                  model.KundenID,
                                                                  true
                  ))
                {
                    return RedirectToAction("ZusammenFassung");
                }
            }

            return View(model);
        }

        #endregion

        #region Arbeitgeber
        
        [HttpGet]
        public ActionResult Arbeitgeber()
        {
            Debug.Indent();
            Debug.WriteLine("GET - KonsumKreditController - Arbeitgeber");
            Debug.Unindent();

            #region LookupTabellenLaden

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

            #endregion

            ArbeitgeberModel model = new ArbeitgeberModel()
            {
                AlleBeschaeftigungsArtAngabenWeb = alleBeschaeftugungsArtenAngabenWeb,
                AlleBranchenAngabenWeb = alleBranchenAngabenWeb,
                KundenID = int.Parse(Request.Cookies["kundenID"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - Arbeitgeber");
            Debug.Unindent();

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.ArbeitgeberSpeichern(
                                                                model.Firma,
                                                                model.ID_BeschaeftigungsArt,
                                                                model.ID_Branche,
                                                                model.BeschaeftigtSeit,
                                                                model.KundenID,
                                                                false
                    ))
                {
                    return RedirectToAction("KontoVerfuegbar");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.ArbeitgeberSpeichern(
                                                               model.Firma,
                                                               model.ID_BeschaeftigungsArt,
                                                               model.ID_Branche,
                                                               model.BeschaeftigtSeit,
                                                               model.KundenID,
                                                               true
                   ))
                {
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

            if (ModelState.IsValid && !HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern( model.BIC, 
                                                                        model.IBAN, 
                                                                        "Deutsche Bank AG", 
                                                                        true, 
                                                                        model.KundenID,
                                                                        false
                                                                        ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID,
                                                                        true
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
                                                                        model.KundenID,
                                                                        false
                                                                        ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID,
                                                                        true
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
                                                                        model.KundenID,
                                                                        false
                                                                        ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }
            else if (ModelState.IsValid && HomeController.alleDatenAngegeben)
            {
                if (KonsumKreditVerwaltung.KontoInformationenSpeichern(model.BIC,
                                                                        model.IBAN,
                                                                        "Deutsche Bank AG",
                                                                        true,
                                                                        model.KundenID,
                                                                        true
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
            model.EinkuenfteAlimenteUnterhalt = (double)aktKunde.FinanzielleSituation.Unterhalt;
            model.RatenVerpflichtungen = (double)aktKunde.FinanzielleSituation.Raten;

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
        public ActionResult ZusammenFassung(ZusammenFassungModel  model)
        {
            Debug.Indent();
            Debug.WriteLine("POST - KonsumKreditController - ZusammenFassung");
            Debug.Unindent();

            return View();
        }

        #endregion
    }
}