using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class ZusammenFassungModel
    {
        #region Allgemein

        [Required]
        [HiddenInput(DisplayValue = false)]
        public int KundenID { get; set; }

        #endregion

        #region Kreditrahmen
        
        [Display(Name = "Gewünschter Betrag")]
        public int GewuenschterBetrag { get; set; }
        
        [Display(Name = "Laufzeit in Monaten")]
        public short Laufzeit { get; set; }

        #endregion

        #region FinanzielleSituation
        
        [Display(Name = "Netto-Einkommen (14x jährlich)")]
        public double NettoEinkommen { get; set; }

        [Display(Name = "Wohnkosten (Miete, Heizung, Strom)")]
        public double Wohnkosten { get; set; }

        [Display(Name = "Einkünfte aus Alimenten, Unterhalte usw.")]
        public double EinkuenfteAlimenteUnterhalt { get; set; }

        [Display(Name = "Zahlungen für Unterhalt, Alimente usw.")]
        public double UnterhaltsZahlungen { get; set; }

        [Display(Name = "Raten-Verpflichtungen")]
        public double RatenVerpflichtungen { get; set; }

        #endregion

        #region PersoenlicheDaten

        [Display(Name = "Geschlecht")]
        public string Geschlecht { get; set; }

        [Display(Name = "Titel")]
        public string Titel { get; set; }

        [Display(Name = "Vorname")]
        public string Vorname { get; set; }

        [Display(Name = "Nachname")]
        public string Nachname { get; set; }

        [Display(Name = "Geburtsdatum")]
        public DateTime GeburtsDatum { get; set; }

        [Display(Name = "Staatsbürgerschaft")]
        public string Staatsbuergerschaft { get; set; }

        [Display(Name = "Anzahl unterhaltspflichtiger Kinder")]
        public int AnzahlKinder { get; set; }

        [Display(Name = "aktueller Familienstand")]
        public string Familienstand { get; set; }

        [Display(Name = "aktuelle Wohnsituation")]
        public string Wohnart { get; set; }

        [Display(Name = "höchst abgeschlossene Ausbildung")]
        public string SchulAbschluss { get; set; }

        [Display(Name = "Identifikationstyp")]
        public string IdentifikationsArt { get; set; }

        [Display(Name = "Identifikations-Nummer")]
        public string IdentifikationsNummer { get; set; }

        #endregion

        #region KontaktDaten
        
        [Display(Name = "Strasse")]
        public string Strasse { get; set; }

        [Display(Name = "Hausnummer")]
        public string Hausnummer { get; set; }

        [Display(Name = "Stiege")]
        public string Stiege { get; set; }

        [Display(Name = "Tür")]
        public string Tuer { get; set; }

        [Display(Name = "PLZ & Ort")]
        public string Ort { get; set; }

        [Display(Name = "E-Mail")]
        public string EMail { get; set; }

        [Display(Name = "Telefonnummer")]
        public string TelefonNummer { get; set; }

        #endregion

        #region Arbeitgeber
        
        [Display(Name = "Firma")]
        public string Firma { get; set; }

        [Display(Name = "Beschäftigungsart")]
        public string BeschaeftigungsArt { get; set; }

        [Display(Name = "Branche")]
        public string Branche { get; set; }

        [Display(Name = "Beschäftigt seit")]
        public DateTime BeschaeftigtSeit { get; set; }

        #endregion

        #region KontoInformation

        [Display(Name = "BIC")]
        public string BIC { get; set; }

        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [Display(Name = "Bankinstitut")]
        public string BankInstitut { get; set; }

        #endregion
        
    }
}