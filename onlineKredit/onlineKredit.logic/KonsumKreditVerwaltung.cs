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
        #region FunktionierendeRegion

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

        /// <summary>
        /// Speichert zu einer übergebenene ID_Kunde den Wunsch Kredit und dessen Laufzeit ab
        /// </summary>
        /// <param name="kreditBetrag">die Höhe des gewünschten Kredits</param>
        /// <param name="laufzeit">die Laufzeit des gewünschten Kredits</param>
        /// <param name="idKunde">die ID des Kunden zu dem die Angaben gespeichert werden sollen</param>
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
                        Kredit neuerKreditWunsch = new Kredit()
                        {
                            GewuenschterKredit = (decimal)kreditBetrag,
                            GewuenschteLaufzeit = laufzeit,
                            ID = idKunde
                        };

                        context.AlleKredite.Add(neuerKreditWunsch);
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
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
        /// Speichert zu einer übergebenene ID_Kunde seine finanziellen Daten/Situation ab.
        /// </summary>
        /// <param name="nettoEinkommen"></param>
        /// <param name="wohnkosten"></param>
        /// <param name="einkuenfteAusAlimenten"></param>
        /// <param name="unterhaltsZahlungen"></param>
        /// <param name="ratenVerpflichtungen"></param>
        /// <param name="idKunde"></param>
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
                        FinanzielleSituation neueFinanzSituation = new FinanzielleSituation()
                        {
                            ID = idKunde,
                            MonatsEinkommenNetto = (decimal)nettoEinkommen,
                            Wohnkosten = (decimal)wohnkosten,
                            SonstigeEinkommen = (decimal)einkuenfteAusAlimenten,
                            Unterhalt = (decimal)unterhaltsZahlungen,
                            Raten = (decimal)ratenVerpflichtungen
                        };

                        context.AlleFinanzielleSituationen.Add(neueFinanzSituation);
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
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


        #endregion

        /// Hier werden die Daten für die Lookup-Tabellen aus der Datenbank gelesen
        /// und anschliessend in jeder Funktion zuruckgegeben.
        #region LookuptabellenDatenInOberflaecheGeben

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

            List <Land> alleLaenderBL = null;

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

        #region KontaktDaten

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

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
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
        /// Speicher zu einer übergegebene ID_Kunde (kundenID) seinen Arbeitgeber ab.
        /// </summary>
        /// <param name="firma"></param>
        /// <param name="idBeschaeftigungsArt"></param>
        /// <param name="idBranche"></param>
        /// <param name="beschaeftigtSeit"></param>
        /// <param name="kundenID"></param>
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
                        Arbeitgeber neuerArbeitgeber = new Arbeitgeber()
                        {
                            ID = kundenID,
                            Firma = firma,
                            FKBeschaeftigungsArt = idBeschaeftigungsArt,
                            FKBranche = idBranche,
                            BeschaeftigtSeit = beschaeftigtSeit
                        };
                        aktKunde.Arbeitgeber = neuerArbeitgeber;
                    }
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
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
                        KontaktDaten neueKontaktDaten = new KontaktDaten()
                        {
                            ID = kundenID,
                            Strasse = strasse,
                            Hausnummer = hausnummer, 
                            Stiege = stiege, 
                            Tür = tuer, 
                            FKOrt = fkOrt,
                            EMail = eMail,
                            Telefonnummer = telefonnummer
                        };
                        aktKunde.KontaktDaten = neueKontaktDaten;
                    }

                    int anzahlZeilenBetroffen = context.SaveChanges();
                    erfolgreich = anzahlZeilenBetroffen >= 1;
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


    }
}
