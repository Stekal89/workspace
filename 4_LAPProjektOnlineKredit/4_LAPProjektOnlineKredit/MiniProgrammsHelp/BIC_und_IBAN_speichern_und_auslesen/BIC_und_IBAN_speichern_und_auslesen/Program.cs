using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIC_und_IBAN_speichern_und_auslesen
{
    class Program
    {
        #region Farben

        public static void Gruen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public static void Rot()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public static void Grau()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        #endregion
        
        static Random zufall = new Random();

        static void Main(string[] args)
        {
            /// Da für dieses Beispiel, wir immer nur von der selben Bank ausgehen,
            /// bleibt die Banknummer (IBAN) immer derselbe.
            const string bic = "BARCDEHAXXX";

            /// In dieser 2 dimensionalen Liste, werden die BIC´s und die IBAN´s gespeichert.
            List<List<string>> bicUndIban = new List<List<string>>();

            /// Selbstgenerierende Konten ganz nach dem "Random()" Zufallsprinzip
            #region SelbstgenerierendeKonten

            bicUndIban.Add(BankKontoErzeugen(bic));
            bicUndIban.Add(BankKontoErzeugen(bic));
            bicUndIban.Add(BankKontoErzeugen(bic));
            bicUndIban.Add(BankKontoErzeugen(bic));
            bicUndIban.Add(BankKontoErzeugen(bic));
            bicUndIban.Add(BankKontoErzeugen(bic));

            bool geht = AlleBICUndIBAANAuslesen(bicUndIban);
            RichtigFalsch(geht);
            Console.WriteLine("\nAlle Bankkonten: " + geht);
            Grau();

            geht = BICUndIBAANAuslesen(bicUndIban, 5);
            RichtigFalsch(geht);
            Console.WriteLine("\nBankkonto Nr. 5: " + geht);
            Grau();

            Console.WriteLine("\n\n\nWeiter mit beliebiger Taste...");
            Console.ReadKey();
            Console.Clear();

            #endregion

            Console.WriteLine("\n\nBitte Ihren BIC eingeben.");
            string eingabeIBAN = Console.ReadLine();

            eingabeIBAN = FilterAufVorhandeneLeerzeichen(eingabeIBAN);

            /// Fügt an jeder 4ten Stelle ein Leerzeichen ein.
            eingabeIBAN = LeerzeichenEinfuegen(eingabeIBAN);

            Console.WriteLine("Der eingegebene Code lautet: " + eingabeIBAN);

            Console.ReadKey();
        }

        public static void RichtigFalsch(bool erfolgreich)
        {
            if (erfolgreich)
            {
                Gruen();
            }
            else
            {
                Rot();
            }
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

            for (int i = 0; i < eingabeIBAN.Length; i++)
            {
                if (eingabeIBAN[i] == ' ')
                    zwischenListe += "";
                else
                    zwischenListe += eingabeIBAN[i];
            }

            eingabeIBAN = zwischenListe;

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
            return eingabeIBAN;
        }

        /// <summary>
        /// Erzeugt ein neues Bankkonto mittels Liste und gibt diese Liste zurück
        /// An der Stelle "0" BIC
        /// An der Stelle "1" IBAN
        /// </summary>
        /// <returns>Ein neues Bankkonto</returns>
        public static List<string> BankKontoErzeugen(string bic)
        {
            Debug.Indent();
            Debug.WriteLine("BankKontoErzeugen");

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

            string iban = "AT78 0202 16217";

            int leerzeichen = 0;

            for (int i = 0; i <= 12; i++)
            {
                Debug.Indent();
                Debug.WriteLine(leerzeichen + " % 5 =" + leerzeichen % 4);

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

            return bicUndIban;
        }

        /// <summary>
        /// Mittels dieser Funktion werden alle Bankkonten aus und gibt diese im 
        /// Debugger und auf der Konsole aus.
        /// </summary>
        /// <param name="liste">Liste der Bankkonten</param>
        /// <returns>Bei Erfolg "true" / anderfalls "false"</returns>
        public static bool AlleBICUndIBAANAuslesen(List<List<string>> liste)
        {
            Debug.Indent();
            Debug.WriteLine("AlleBICUndIBANAuslesen");
            bool erfolgreich = false;

            try
            {
                if (liste != null)
                {
                    for (int i = 0; i < liste.Count; i++)
                    {
                        Debug.Indent();
                        Debug.WriteLine("BIC: " + liste[i][0]);
                        Debug.WriteLine("IBAN: " + liste[i][1]);
                        Debug.Unindent();

                        Console.WriteLine("\n\n" + (i + 1) + ". Bankkonto");
                        Console.WriteLine("BIC: " + liste[i][0]);
                        Console.WriteLine("IBAN: " + liste[i][1]);
                    }

                    erfolgreich = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Indent();
                Debug.WriteLine("Fehler in AlleBICUndIBANAuslesen");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debug.Unindent();
            }

            Debug.Unindent();

            return erfolgreich;
        }

        /// <summary>
        /// Mittels dieser Funktion werden ein Bankkonto aus und gibt diese im 
        /// Debugger und auf der Konsole aus.
        /// </summary>
        /// <param name="liste">Liste der Bankkonten</param>
        /// <param name="stelle">begonnen bei 1</param>
        /// <returns>Bei Erfolg "true" / anderfalls "false"</returns>
        public static bool BICUndIBAANAuslesen(List<List<string>> liste, int stelle)
        {
            Debug.Indent();
            Debug.WriteLine("BICUndIBANAuslesen");
            bool erfolgreich = false;

            stelle--;

            try
            {
                if (liste != null && liste.Count >= stelle)
                {
                    
                        Debug.Indent();
                        Debug.WriteLine("BIC: " + liste[stelle][0]);
                        Debug.WriteLine("IBAN: " + liste[stelle][1]);
                        Debug.Unindent();

                        Console.WriteLine("\n\n" + (stelle + 1) + ". Bankkonto");
                        Console.WriteLine("BIC: " + liste[stelle][0]);
                        Console.WriteLine("IBAN: " + liste[stelle][1]);
                    
                    erfolgreich = true;
                }
            }
            catch (Exception ex)
            {
                Debug.Indent();
                Debug.WriteLine("Fehler in BICUndIBANAuslesen");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debug.Unindent();
            }

            Debug.Unindent();

            return erfolgreich;
        }
    }
}
