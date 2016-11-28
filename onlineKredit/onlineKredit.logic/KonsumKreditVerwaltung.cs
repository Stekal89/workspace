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
    }
}
