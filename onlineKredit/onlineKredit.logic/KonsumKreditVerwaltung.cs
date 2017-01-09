using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineKredit.logic
{
    public class KonsumKreditVerwaltung
    {
        /// <summary>
        /// Erzeugt einen "leeren" dummy Kunden
        /// zu dem in Folge alle Konsumkredit Daten
        /// verknüpft werden können.
        /// </summary>
        /// <returns>einen leeren Kunden wenn erfolgreich, ansonsten null</returns>
        public static Kunde ErzeugeKunde()
        {

            Debug.Indent();
            Debug.WriteLine("\nKonsumKreditVerwaltung - ErzeugeKunde");
            Debug.Indent();

            Kunde neuerKunde = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    neuerKunde = new Kunde()
                    {
                        Vorname = "Anonym",
                        Nachname = "Anonym",
                        Geschlecht = "m",
                        Geburtsdatum = new DateTime(1989, 02, 12)
                    };

                    context.AlleKunden.Add(neuerKunde);

                    int anzahlZeilenBearbeitet = context.SaveChanges();
                    Debug.WriteLine($"{anzahlZeilenBearbeitet} Kunden angelegt!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nFehler bei " + "KonsumkreditVerwaltung - ErzeugeKunde".ToUpper());
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            return neuerKunde;
        }

        #region KreditRahmen

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde den Wunsch Kredit und dessen Laufzeit ab
        /// </summary>
        /// <param name="kreditBetrag">die Höhe des gewünschten Kredits</param>
        /// <param name="laufzeit">die Laufzeit des gewünschten Kredits</param>
        /// <param name="idKunde">die ID des Kunden zu dem die Angaben gespeichert werden sollen</param>
        /// <param name="datenSatzVorhanden">bei TRUE wird ein vorhandener Datensatz bearbeitet/ bei FALSE ein neuer generiert</param>
        /// <returns>true wenn Eintragung gespeichert werden konnte und der Kunde existiert, ansonsten false</returns>
        public static bool KreditRahmenSpeichern(double kreditBetrag, short laufzeit, int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KreditRahmenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.Kredit == null)
                            aktKunde.Kredit = new Kredit();
                        
                        /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                        aktKunde.Kredit.GewuenschterKredit = (decimal)kreditBetrag;
                        aktKunde.Kredit.GewuenschteLaufzeit = laufzeit;
                        aktKunde.Kredit.ID = idKunde;
                    }

                    /// Speichere Kreditwunsch in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} KreditRahmen gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nFehler bei " + "KonsumkreditVerwaltung - KreditRahmenSpeichern".ToUpper());
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }

        /// <summary>
        /// Liefert den aktuellen, KreditRahmen des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kredit bei Fehler NULL</returns>
        public static Kredit KreditRahmenLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KreditRahmenLaden");
            Debug.Indent();

            Kredit aktKreditRahmen = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktKreditRahmen = context.AlleKredite.Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung KreditRahmenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }
            return aktKreditRahmen;
        }

        #endregion

        #region FinanzielleSituation

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde seine finanziellen Daten/Situation ab.
        /// </summary>
        /// <param name="nettoEinkommen"></param>
        /// <param name="wohnkosten"></param>
        /// <param name="einkuenfteAusAlimenten"></param>
        /// <param name="unterhaltsZahlungen"></param>
        /// <param name="ratenVerpflichtungen"></param>
        /// <param name="idKunde"></param>
        /// <param name="datenSatzVorhanden">bei TRUE wird ein vorhandener Datensatz bearbeitet/ bei FALSE ein neuer generiert</param>
        /// <returns>true wenn Eintragung gespeichert werden konnte und der Kunde existiert, ansonsten false</returns>
        public static bool FinanzielleSituationSpeichern(double nettoEinkommen, double wohnkosten, double einkuenfteAusAlimenten, double unterhaltsZahlungen, double ratenVerpflichtungen, int idKunde)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - FinanzielleSituationSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.FinanzielleSituation == null)
                            aktKunde.FinanzielleSituation = new FinanzielleSituation();

                        /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                        aktKunde.FinanzielleSituation.ID = idKunde;
                        aktKunde.FinanzielleSituation.MonatsEinkommenNetto = (decimal)nettoEinkommen;
                        aktKunde.FinanzielleSituation.Wohnkosten = (decimal)wohnkosten;
                        aktKunde.FinanzielleSituation.SonstigeEinkommen = (decimal)einkuenfteAusAlimenten;
                        aktKunde.FinanzielleSituation.UnterhaltsZahlungen = (decimal)unterhaltsZahlungen;
                        aktKunde.FinanzielleSituation.RatenZahlungen = (decimal)ratenVerpflichtungen;
                    }
               
                    /// Speichere Finanzielle Situation (Änderungen) in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} FinanzielleSituation gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nFehler bei " + "KonsumkreditVerwaltung - FinanzielleSituationSpeichern".ToUpper());
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }


            Debug.Unindent();
            return erfolgreich;
        }

        /// <summary>
        /// Liefert die aktuelle, Finanzielle Situation des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>FinanzielleSituation bei Fehler NULL</returns>
        public static FinanzielleSituation FinanzielleSituationLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - FinanzielleSituationLaden");
            Debug.Indent();

            FinanzielleSituation aktFinanzielleSituation = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktFinanzielleSituation = context.AlleFinanzielleSituationen.Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung FinanzielleSituationLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
                return aktFinanzielleSituation;
            }

            return aktFinanzielleSituation;
        }

        #endregion

        #region PersoenlicheDaten

        /// Hier werden die Daten für die Lookup-Tabellen aus der Datenbank gelesen
        /// und anschliessend in jeder Funktion zuruckgegeben.
        #region PersoenlicheDatenLookup

        /* 
            Für das PersoenlicheDatenModel werden 6 Lookuptabellen benötigt
         
            public List<TitelModel> AlleTitelAngaben { get; set; }
            public List<StaatsbuergerschaftsModel> AlleStaatsbuergerschaftsAngaben { get; set; }
            public List<FamilienStandsModel> AlleFamilienstandsAngaben { get; set; }
            public List<SchulabschlussModel> AlleSchulabschlussAngaben { get; set; }
            public List<IdentifikationsArtModel> AlleIdentifikationsArtAngaben { get; set; }
            public List<WohnartModel> AlleWohnartsAngaben { get; set; }
        */

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Titel und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Die Liste aller Titel</returns>
        public static List<Titel> TitelLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - TitelLaden");
            Debug.Indent();

            // Liste in der BusinessLogic
            List<Titel> alleTitelBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleTitelBL = context.AlleTitel.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung TitelLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleTitelBL;
        }

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Land und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Bei Erfolg eine Liste der Länder / ansonsten "null"</returns>
        public static List<Land> LaenderLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - StaatsbuergerschaftenLaden");
            Debug.Indent();

            List<Land> alleLaenderBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleLaenderBL = context.AlleLaender.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung - StaatsbuergerschftenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleLaenderBL;
        }

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Famililenstand und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Bei Erfolg eine Liste der Familienstände / ansonsten "null"</returns>
        public static List<Familienstand> FamilienstaendeLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - FamilienstaendeLaden");
            Debug.Indent();

            List<Familienstand> alleFamilienstaendeBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleFamilienstaendeBL = context.AlleFamilienstaende.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung - FamilienstaendeLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleFamilienstaendeBL;
        }

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Schulabschluss und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Bei Erfolg eine Liste der Schulabschlüsse / ansonsten "null"</returns>
        public static List<Schulabschluss> SchulabschluesseLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - SchulabschluesseLaden");
            Debug.Indent();

            List<Schulabschluss> alleSchulabschluesseBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleSchulabschluesseBL = context.AlleSchulabschluesse.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung - SchulabschluesseLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleSchulabschluesseBL;
        }

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: IdentifkationsArt und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Bei Erfolg eine Liste der Identifikationsarten / ansonsten "null"</returns>
        public static List<IdentifikationsArt> IdentifikationsArtenLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - IdentifikationsArtenLaden");
            Debug.Indent();

            List<IdentifikationsArt> alleIdentifikationsArtenBl = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleIdentifikationsArtenBl = context.AlleIdentifikationsArten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung - IdentifikationsArtenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleIdentifikationsArtenBl;
        }

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Wohnart und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Bei Erfolg eine Liste der Wohnarten / ansonsten "null"</returns>
        public static List<Wohnart> WohnartenLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - TitelLaden");
            Debug.Indent();

            List<Wohnart> alleWohnartenBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleWohnartenBL = context.AlleWohnarten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in xxxxxxxx");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleWohnartenBL;
        }

        #endregion

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde seine persönlichen Daten ab.
        /// </summary>
        /// <param name="geschlecht"></param>
        /// <param name="titel"></param>
        /// <param name="vorname"></param>
        /// <param name="nachname"></param>
        /// <param name="geburtsDatum"></param>
        /// <param name="idStaatsbuergerschaft"></param>
        /// <param name="anzahlKinder"></param>
        /// <param name="idFamilienstand"></param>
        /// <param name="idWohnart"></param>
        /// <param name="idSchulAbschluss"></param>
        /// <param name="idIdentifikationsArt"></param>
        /// <param name="identifikationsNummer"></param>
        /// <param name="kundenID"></param>
        /// <returns>true wenn Eintragung gespeichert werden konnte und der Kunde existiert, ansonsten false</returns>
        public static bool PersoenlicheDatenSpeichern(string geschlecht, int? titel, string vorname, string nachname, DateTime geburtsDatum, string idStaatsbuergerschaft, int anzahlKinder, int idFamilienstand, int idWohnart, int idSchulAbschluss, int idIdentifikationsArt, string identifikationsNummer, int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - PersoenlicheDatenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == kundenID).FirstOrDefault();

                    /// Da wir am Anfang einen Dummy-Kunden anlegen (Anonym), ist der Kunde schon Vorhanden
                    /// und muss daher nur mehr geändert werden, somit benötige ich die Extra-Abfrage zum ändern bzw
                    /// zum Speichern nicht!!!
                    if (aktKunde != null)
                    {
                        aktKunde.Geschlecht = geschlecht;
                        aktKunde.FKTitel = titel;
                        aktKunde.Vorname = vorname;
                        aktKunde.Nachname = nachname;
                        aktKunde.Geburtsdatum = geburtsDatum;
                        aktKunde.FKStaatsangehoerigkeit = idStaatsbuergerschaft;
                        aktKunde.AnzahlKinder = anzahlKinder;
                        aktKunde.FKFamilienstand = idFamilienstand;
                        aktKunde.FKWohnart = idWohnart;
                        aktKunde.FKSchulabschluss = idSchulAbschluss;
                        aktKunde.FKIdentifikationsArt = idIdentifikationsArt;
                        aktKunde.IdentifikationsNummer = identifikationsNummer;
                    }

                    /// Speichere Änderungen des Kunden in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} persönliche Daten gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung - PersoenlicheDatenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return erfolgreich;
        }

        /// <summary>
        /// Liefert die aktuellen persönlichen Daten des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kundendaten bei Fehler NULL</returns>
        public static Kunde PersoenlicheDatenLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - PersoenlicheDatenLaden");
            Debug.Indent();

            Kunde aktKunde = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktKunde = context.AlleKunden.Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung PersoenlicheDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }
            return aktKunde;
        }

        #endregion

        #region Arbeitgeber

        /// Hier werden die Daten für die Lookup-Tabellen aus der Datenbank gelesen
        /// und anschliessend in jeder Funktion zuruckgegeben.
        #region ArbeitgeberLookup

        /* 
           Für das Arbeitgeber Model werden 2 Lookuptabellen benötigt.

           public List<BeschaeftigungsArtModel> AlleBeschaeftigungsArtenAngabenWeb { get; set; }
           public List<BranchenModel> AlleBranchenArtenAngabenWeb { get; set; }   
       */

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: BeschaeftigungsArt und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Die Liste aller Beschäftigungsarten</returns>
        public static List<Beschaeftigungsart> BeschaeftigungsArtenLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - BeschaeftigungsArtenLaden");
            Debug.Indent();

            List<Beschaeftigungsart> alleBeschaeftigungsArtenBL = null;

            try
            {
                using (var contex = new dbOnlineKredit())
                {
                    alleBeschaeftigungsArtenBL = contex.AlleBeschaeftigungsarten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in BeschaeftigungsArtenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();
            return alleBeschaeftigungsArtenBL;
        }

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Branche und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Die Liste aller Branchen</returns>
        public static List<Branche> BranchenLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditverwaltung - BranchenLaden");
            Debug.Indent();

            List<Branche> alleBranchenBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleBranchenBL = context.AlleBranchen.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in BranchenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();

            return alleBranchenBL;
        }

        #endregion

        /// <summary>
        /// Speicher zu einer übergegebene ID_Kunde (kundenID) seinen Arbeitgeber ab.
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="idBeschaeftigungsArt"></param>
        /// <param name="idBranche"></param>
        /// <param name="beschaeftigtSeit"></param>
        /// <param name="kundenID"></param>
        /// <param name="datenSatzVorhanden">bei TRUE wird ein vorhandener Datensatz bearbeitet/ bei FALSE ein neuer generiert</param>
        /// <returns>true wenn Eintragung gespeichert werden konnte und der Kunde existiert, ansonsten false</returns>
        public static bool ArbeitgeberSpeichern(string firma, int idBeschaeftigungsArt, int idBranche, DateTime beschaeftigtSeit, int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonusmKreditVerwaltung - ArbeitgeberSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == kundenID).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.Arbeitgeber == null)
                            aktKunde.Arbeitgeber = new Arbeitgeber();

                        /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                        aktKunde.Arbeitgeber.ID = kundenID;
                        aktKunde.Arbeitgeber.Firma = firma;
                        aktKunde.Arbeitgeber.FKBeschaeftigungsArt = idBeschaeftigungsArt;
                        aktKunde.Arbeitgeber.FKBranche = idBranche;
                        aktKunde.Arbeitgeber.BeschaeftigtSeit = beschaeftigtSeit;
                    }

                    /// Speichere den Arbeitgeber in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Arbeitgeber gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in ArbeitgeberSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return erfolgreich;
        }

        /// <summary>
        /// Liefert den aktuellen, Arbeitgeber des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Arbeitgeber bei Fehler NULL</returns>
        public static Arbeitgeber ArbeitgeberLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - ArbeitgeberLaden");
            Debug.Indent();

            Arbeitgeber aktArbeitgeber = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktArbeitgeber = context.AlleArbeitgeber.Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung ArbeitgeberLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }

            return aktArbeitgeber;
        }

        #endregion

        #region KontaktDaten

        /// Hier werden die Daten für die Lookup-Tabellen aus der Datenbank gelesen
        /// und anschliessend in jeder Funktion zuruckgegeben.
        #region KontaktDatenLookup

        /*
            Für das Kontaktdaten Model wird 1 Lookuptabellen benötigt.

            public List<OrtModel> AlleOrtsAngabenWeb { get; set; }  
       */

        /// <summary>
        /// Nimmt alle Einträge aus der Datenbank/Tabelle: Ort und fügt sie in eine Liste ein. 
        /// </summary>
        /// <returns>Die Liste aller Orte</returns>
        public static List<Ort> OrteLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - OrteLaden");
            Debug.Indent();

            List<Ort> alleOrteBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleOrteBL = context.AlleOrte.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in OrteLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();

            return alleOrteBL;
        }

        #endregion

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde seine Kontaktdaten 
        /// (Wohnadresse, E-Mail, Telefonnummer)ab.
        /// </summary>
        /// <param name="strasse"></param>
        /// <param name="hausnummer"></param>
        /// <param name="stiege"></param>
        /// <param name="tuer"></param>
        /// <param name="fkOrt"></param>
        /// <param name="eMail"></param>
        /// <param name="telefonnummer"></param>
        /// <param name="kundenID"></param>
        /// <param name="datenSatzVorhanden">bei TRUE wird ein vorhandener Datensatz bearbeitet/ bei FALSE ein neuer generiert</param>
        /// <returns>true wenn Eintragung gespeichert werden konnte und der Kunde existiert, ansonsten false</returns>
        public static bool KontaktDatenSpeichern(string strasse, string hausnummer, string stiege, string tuer, int fkOrt, string eMail, string telefonnummer, int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontaktDatenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == kundenID).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.KontaktDaten == null)
                            aktKunde.KontaktDaten = new KontaktDaten();

                        /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                        aktKunde.KontaktDaten.ID = kundenID;
                        aktKunde.KontaktDaten.Strasse = strasse;
                        aktKunde.KontaktDaten.Hausnummer = hausnummer;
                        aktKunde.KontaktDaten.Stiege = stiege;
                        aktKunde.KontaktDaten.Tür = tuer;
                        aktKunde.KontaktDaten.FKOrt = fkOrt;
                        aktKunde.KontaktDaten.EMail = eMail;
                        aktKunde.KontaktDaten.Telefonnummer = telefonnummer;
                    }

                    /// Speichere Kontaktdaten(Änderungen) in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kontakt Daten gespeichert!");
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontaktDatenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();

            return erfolgreich;
        }

        /// <summary>
        /// Liefert die aktuellen, Kontaktdaten des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kontaktdaten bei Fehler NULL</returns>
        public static KontaktDaten KontaktDatenLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontaktDatenLaden");
            Debug.Indent();

            KontaktDaten aktKontaktDaten = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktKontaktDaten = context.AlleKontaktDaten.Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung KontaktDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }

            return aktKontaktDaten;
        }

        #endregion

        #region KontoInformation

        /// <summary>
        /// Hier wird das statisch hinzugefügte Model "KontoAbfrageMoeglichkeit"
        /// Als Basis genommen. Davon wird eine Liste erzeugt, die statisch 3 Einträge
        /// beinhaltet. Da es nicht notwendig ist diese ändern zu können, wird sie eben 
        /// statisch produziert.
        /// </summary>
        /// <returns>Eine Liste aus Abfragemöglichkeiten</returns>
        public static List<KontoAbfrageMoeglichkeit> KontoAbfrageMoeglichkeitenLaden()
        {
            List<KontoAbfrageMoeglichkeit> alleKontoAbfrageMoeglichkeitenAngabenBL = null;

            try
            {
                List<KontoAbfrageMoeglichkeit> zwischenListe = new List<KontoAbfrageMoeglichkeit>()
                {
                    new KontoAbfrageMoeglichkeit() { ID = 1, Bezeichnung = "Vorhandenes Deutsche Bank AG Konto." },
                    new KontoAbfrageMoeglichkeit() { ID = 2, Bezeichnung = "Neues Konto bei Deutsche Bank AG anlegen." },
                    new KontoAbfrageMoeglichkeit() { ID = 3, Bezeichnung = "Anderes Konto verwenden." }
                };

                alleKontoAbfrageMoeglichkeitenAngabenBL = zwischenListe;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoAbfrageMoeglichkeitenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return alleKontoAbfrageMoeglichkeitenAngabenBL;
        }

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde seine Kontoinformationen ab.
        /// </summary>
        /// <param name="bic"></param>
        /// <param name="iban"></param>
        /// <param name="bankInstitut"></param>
        /// <param name="istDeutscheBankKunde"></param>
        /// <param name="kundenID"></param>
        /// <param name="datenSatzVorhanden">bei TRUE wird ein vorhandener Datensatz bearbeitet/ bei FALSE ein neuer generiert</param>
        /// <returns></returns>
        public static bool KontoInformationenSpeichern(string bic, string iban, string bankInstitut, bool istDeutscheBankKunde, int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontoInformationenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == kundenID).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        if (aktKunde.KontoDaten == null)
                            aktKunde.KontoDaten = new KontoDaten();

                        /// Weise Daten den aktuellen Kunden zu (Somit kann man vorhandene Daten auch ändern)
                        aktKunde.KontoDaten.ID = kundenID;
                        aktKunde.KontoDaten.BIC = bic;
                        aktKunde.KontoDaten.IBAN = iban;
                        aktKunde.KontoDaten.Bank = bankInstitut;
                        aktKunde.KontoDaten.HatKonto = istDeutscheBankKunde;
                    }

                    /// Speichere KontoDaten (Änderungen) in die Datenbank
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 0;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kontoinformationen gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoInformationenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return erfolgreich;
        }
        
        /// <summary>
        /// Liefert ALLE Kontodaten aus der Datenbank
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kontodaten bei Fehler NULL</returns>
        public static List<KontoDaten> KontoDatenLaden()
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontoDatenLaden");
            Debug.Indent();

            List<KontoDaten> alleKontoDateneBL = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    alleKontoDateneBL = context.AlleKontoDaten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();

            return alleKontoDateneBL;
        }

        /// <summary>
        /// Dieses Objekt wird zum erzeugen einer Zufallszahl benötigt.
        /// Da dies mit einem Algorithmus bezogen auf die Uhrzeit basiert,
        /// muss ich den "Random" als Membervariable initialisieren.
        /// </summary>
        static Random zufall = new Random();

        /// <summary>
        /// Erzeugt ein neues Bankkonto mittels Liste und gibt diese Liste zurück
        /// An der Stelle "0" BIC
        /// An der Stelle "1" IBAN
        /// </summary>
        /// <returns>Ein neues Bankkonto</returns>
        public static List<string> NeuesBankKontoErzeugen()
        {
            Debug.Indent();
            Debug.WriteLine("NeuesBankKontoErzeugen");

            /// Da für dieses Beispiel, wir immer nur von der selben Bank ausgehen,
            /// bleibt die Banknummer (IBAN) immer derselbe.
            const string bic = "BARCDEHAXXX";

            /// In dieser 2 dimensionalen Liste, werden die BIC´s und die IBAN´s gespeichert.
            List<string> bicUndIban = new List<string>();
            
            /* 
              __________________________________________________________________________________________________________
              Bestandteile des  |  Kurz-         |  Formatierung und Vergaben                          |  Beispiel
              IBAN-Standards    |  bezeichnung   |                                                     |
              __________________________________________________________________________________________________________
              Ländercode        |  LL            |   Konstant "DE"                                     |    DE
              ----------------------------------------------------------------------------------------------------------
              Prüfziffer        |  PZ            |   2-stellig, Modulus 97-10 (ISO 7064)	             |    21
              ----------------------------------------------------------------------------------------------------------
              Bankleitzahl      |  BLZ           |   Konstant 8-stellig, Bankidentifikation            |    30120400
                                |                |   entsprechend deutschem                            |
                                |                |   Bankleitzahlenverzeichnis                         |
              ----------------------------------------------------------------------------------------------------------
              Kontonummer       |  KTO           |   Konstant 10-stellig (ggf. mit vorangestellten     |    15228
                                                     Nullen) Kunden-Kontonummer
          */

            /// Dieser Teil des IBAN`s bleibt immmer gleich
            /// es werden lediglich die 10 Kundenstellen erzeugt.
            string iban = "AT78 0202 16217";

            try
            {
                int leerzeichen = 0;

                for (int i = 0; i <= 12; i++)
                {
                    Debug.Indent();
                    Debug.WriteLine(leerzeichen + " % 5 =" + leerzeichen % 5);

                    if (leerzeichen % 5 == 0)
                    {
                        iban += " ";
                    }
                    else
                    {
                        iban += zufall.Next(0, 10).ToString();
                    }

                    leerzeichen++;
                    Debug.Unindent();
                }
                
                bicUndIban.Add(bic);
                bicUndIban.Add(iban);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in NeuesBankKontoErzeugen");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();

            return bicUndIban;
        }

        /// <summary>
        /// Überprüft ob das erzeugte Bankkonto schon in der 
        /// Datenbank vorhanden ist, ist dies der Fall
        /// wird ein neues Bankkonto erzeugt
        /// </summary>
        /// <returns>Bei Erfolg das neue Bankkonto / andernfalls null</returns>
        public static List<string> BankKontoErzeugen()
        {
            Debug.Indent();
            Debug.WriteLine("BankKontoErzeugen");

            List<string> neuesBankKonto = null;
            bool erfolgreich = false;

            try
            {
                do
                {
                    neuesBankKonto = NeuesBankKontoErzeugen();

                    int zaehler = 0;

                    if (KontoDatenLaden().Count != 0)
                    {
                        foreach (var item in KontoDatenLaden())
                        {
                            if (item.IBAN == neuesBankKonto[1] && zaehler != KontoDatenLaden().Count)
                            {
                                NeuesBankKontoErzeugen();
                                zaehler++;
                            }
                            else
                            {
                                erfolgreich = true;
                            }
                        }
                    }
                    else
                    {
                        erfolgreich = true;
                    }
                } while (!erfolgreich);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in NeuesBankKontoErzeugen");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            
            
            return neuesBankKonto;
        }

        /// <summary>
        /// Erstüberprüfung der eingegebenen Textkette, die überprüft, ob
        /// etwaige Leerzeichen vorhanden sind (die könnten an jeder Stelle stehen)
        /// und fügt stattdesen einen Leerstring "" => null ein. Was bewirkt, dass das 
        /// Leerzeichen gelöscht wird und das danachkommende Zeichen (Buchstabe/Zahl)
        /// in der Zeichenkette, was auch als Liste von Caracter bekannt ist, automatisch auf.
        /// </summary>
        /// <param name="eingabeIBAN"></param>
        /// <returns>Textkette ohne Leerzzeichen</returns>
        public static string FilterAufVorhandeneLeerzeichen(string eingabeIBAN)
        {
            string zwischenListe = "";
            if (eingabeIBAN != null)
            {
                for (int i = 0; i < eingabeIBAN.Length; i++)
                {
                    if (eingabeIBAN[i] == ' ')
                        zwischenListe += "";
                    else
                        zwischenListe += eingabeIBAN[i];
                }

                eingabeIBAN = zwischenListe;

            }

            return eingabeIBAN;
        }

        /// <summary>
        /// Filtert eine Texteingabe und fügt an jeder 4ten Stelle ein Leerzeichen " " ein. 
        /// </summary>
        /// <param name="eingabeIBAN">Die Texteingabe die gefiltert werden soll.</param>
        /// <returns>überarbeitete Texteingabe</returns>
        public static string LeerzeichenEinfuegen(string eingabeIBAN)
        {
            string zwischenListe = "";
            int leerzeichen = 1;

            if (eingabeIBAN != null)
            {
                for (int j = 0; j < eingabeIBAN.Length; j++)
                {
                    if (leerzeichen % 5 == 0)
                    {
                        for (int i = 0; i < (eingabeIBAN.Length); i++)
                        {
                            if (i == (leerzeichen - 1))
                            {
                                zwischenListe += " ";
                                zwischenListe += eingabeIBAN[i];
                            }
                            else
                            {
                                zwischenListe += eingabeIBAN[i];
                            }
                        }
                        eingabeIBAN = zwischenListe;
                        zwischenListe = "";
                    }
                    leerzeichen++;
                }
            }
         
            return eingabeIBAN;
        }

        /// <summary>
        /// Liefert die aktuellen, KontoDaten des Kunden
        /// </summary>
        /// <param name="kundenID">ID des vorhandenen Kunden</param>
        /// <returns>Kontodaten bei Fehler NULL</returns>
        public static KontoDaten KontoDatenLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KontaktDatenLaden");
            Debug.Indent();

            KontoDaten aktKontoDaten = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktKontoDaten = context.AlleKontoDaten.Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KonsumKreditVerwaltung KontaktDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
                Debug.Unindent();
            }

            return aktKontoDaten;
        }

        #endregion

        #region Zusammenfassung

        public static Kunde KundenDatenLaden(int kundenID)
        {
            Debug.Indent();
            Debug.WriteLine("KonsumKreditVerwaltung - KundenDatenLaden");
            Debug.Indent();

            Kunde aktKunde = null;

            try
            {
                using (var context = new dbOnlineKredit())
                {
                    aktKunde = context.AlleKunden
                       .Include("IdentifikationsArt")
                       .Include("Familienstand")
                       .Include("Schulabschluss")
                       .Include("Titel")
                       .Include("Wohnart")
                       .Include("Kredit")
                       .Include("KontoDaten")
                       .Include("KontaktDaten")
                       .Include("KontaktDaten.Ort")
                       .Include("Land")
                       .Include("FinanzielleSituation")
                       .Include("Arbeitgeber")
                       .Include("Arbeitgeber.Branche")
                       .Include("Arbeitgeber.Beschaeftigungsart")
                       .Where(x => x.ID == kundenID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KundenDatenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            Debug.Unindent();

            return aktKunde;
        }

        #endregion
    }

    /// <summary>
    /// Da ich für diese Aktion keine Einträge in der Datenbank
    /// benötige, habe ich diese Klasse Statisch in die Business-Logic 
    /// eingebaut. Sie besteht, wie die Lookup-Tabellen einfach aus 
    /// ID und Bezeichnung.
    /// </summary>
    public class KontoAbfrageMoeglichkeit
    {
        public int ID { get; set; }

        public string Bezeichnung { get; set; }
    }

}
