using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineKredit.freigabe
{
    public class KreditFreigabe
    {
        /// <summary>
        /// Gibt zurück ob für die angagebenen Daten eine Kreditfreigabe erteilt wird
        /// </summary>
        /// <param name="geschlecht">der Vorname des Antragstellers</param>
        /// <param name="vorname">der Nachname des Antragstellers</param>
        /// <param name="nachname">das monatliche Netto-Einkommen des Antragstellers</param>
        /// <param name="famileinStand">der Familienstand des Antragstellers</param>
        /// <param name="nettoEinkommen">das monatliche Nettoeinkommen des Antragstellers</param>
        /// <param name="wohnKosten">die monatlichen Wohnkosten des Antragstellers</param>
        /// <param name="einkuenfteAliUnt">die monatlichen Einkünfte aus Alimente, Unterhalt des Antragstellers</param>
        /// <param name="ausgabenAlimente">die monatlichen Ausgaben für Alimente, Unterhalt des Antragstellers</param>
        /// <param name="ratenZahlungen">die monatlichen Ratenzahlungen des Antragstellers</param>
        /// <returns>Bei gültiger</returns>
        public static bool FreigabePruefen(string geschlecht, string vorname, string nachname, string famileinStand, double nettoEinkommen, double wohnKosten, double einkuenfteAliUnt, double ausgabenAlimente, double ratenZahlungen)
        {
            Debug.Indent();
            Debug.WriteLine("KreditFreigabe - FreigabePruefen");
            Debug.Indent();

            /// Standardmäßig wird die Freigabe auf "FALSE" gesetzt
            bool freigabe = false;


            if (string.IsNullOrEmpty(vorname)) /// Überprüfe, ob die Eingabe Null oder leer ist
                throw new ArgumentException(nameof(vorname));
            if (string.IsNullOrEmpty(nachname))
                throw new ArgumentException(nameof(nachname));
            if (nettoEinkommen <= 0 || nettoEinkommen > 50000) /// Überprüfe, ob sich das "Nettoeinkommen" in einem realistischen Rahmen befindet
                throw new ArgumentException($"Ungültiger Wert für {nameof(nettoEinkommen)}");
            if (wohnKosten < 0 || wohnKosten > 10000)  /// Überprüfe, ob sich die "Wohnkosten" in einem realistischen Rahmen befindet
                throw new ArgumentException($"Ungültiger Wert für {nameof(wohnKosten)}");
            if (einkuenfteAliUnt < 0 || einkuenfteAliUnt > 10000)  /// Überprüfe, ob sich die "Einkünfte aus Alimenten" in einem realistischen Rahmen befindet
                throw new ArgumentException($"Ungültiger Wert für {nameof(einkuenfteAliUnt)}");
            if (ausgabenAlimente < 0 || ausgabenAlimente > 10000)  /// Überprüfe, ob sich die "Ausgaben für Alimente" in einem realistischen Rahmen befindet
                throw new ArgumentException($"Ungültiger Wert für {nameof(ausgabenAlimente)}");
            if (ratenZahlungen < 0 || ratenZahlungen > 10000)
                throw new ArgumentException($"Ungültiger Wert für {nameof(ratenZahlungen)}");

            /// diese Variable setzt sich aus allen verfügbaren Beträgen 
            /// inklussieve der notwendigen Abzüge (für Fixe Zahlungen) zusammen 
            double gesamtErhalt = nettoEinkommen + einkuenfteAliUnt - wohnKosten - ausgabenAlimente - ratenZahlungen;
            double verhaeltnisWohnkostenGesamtErhalt = wohnKosten / gesamtErhalt;

            switch (famileinStand)
            {
                /// Wenn der Familienstand gleich:
                case "ledig":
                case "verwitwet":
                case "geschieden":  /// ist dann werden folgende operationen durchgeführt:
                    switch (geschlecht)
                    {
                        case "m":
                            freigabe = gesamtErhalt > wohnKosten * 2;
                            break;
                        case "w":
                            freigabe = gesamtErhalt > wohnKosten * 1.8;
                            break;
                        default:
                            throw new ArgumentException($"Ungültiger Wert für {nameof(geschlecht)}!\n\nNur 'm' oder 'w' erlaubt.");
                            //break; wäre unerreichbar, da ich mit der "throw" Methode sowieso aussteige
                    }

                    break;
                /// Wenn der Familienstand gleich:
                case "in Partnerschaft":
                case "verheiratet": /// ist dann werden folgende operationen durchgeführt:
                    if (verhaeltnisWohnkostenGesamtErhalt < 0.5)
                    {
                        freigabe = gesamtErhalt > wohnKosten * 2.5;
                    }
                    else
                    {
                        freigabe = gesamtErhalt > wohnKosten * 3.5;
                    }
                    break;
                default:
                    throw new ArgumentException($"Ungültiger Wert für {nameof(famileinStand)}!\n\nNur 'ledig', 'verwitwet', 'in Partnerschaft', 'verheiratet' erlaubt.");
                    //break; wäre unerreichbar, da ich mit der "throw" Methode sowieso aussteige
            }
                    return freigabe;

        }
    }
}

