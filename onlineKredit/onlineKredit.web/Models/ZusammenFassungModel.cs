using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class ZusammenFassungModel
    {
        #region Allgemein

        public int KundenID { get; set; }

        #endregion

        #region Kreditrahmen
        
        public double GewuenschterBetrag { get; set; }
        public short Laufzeit { get; set; }

        #endregion

        #region FinanzielleSituation

        public double NettoEinkommen { get; set; }
        public double Wohnkosten { get; set; }
        public double EinkuenfteAlimenteUnterhalt { get; set; }
        public double UnterhaltsZahlungen { get; set; }
        public double RatenVerpflichtungen { get; set; }

        #endregion

        #region PersoenlicheDaten
        
        public string Geschlecht { get; set; }
        public string Titel { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public DateTime GeburtsDatum { get; set; }
        public string Staatsbuergerschaft { get; set; } 
        public int AnzahlKinder { get; set; }
        public string Familienstand { get; set; }
        public string Wohnart { get; set; }
        public string SchulAbschluss { get; set; }
        public string IdentifikationsArt { get; set; }
        public string IdentifikationsNummer { get; set; }

        #endregion

        #region KontaktDaten
        
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string Stiege { get; set; }
        public string Tuer { get; set; }
        public string Ort { get; set; }
        public string EMail { get; set; }
        public string TelefonNummer { get; set; }

        #endregion

        #region Arbeitgeber
        
        public string Firma { get; set; }
        public string BeschaeftigungsArt { get; set; }
        public string Branche { get; set; }
        public DateTime BeschaeftigtSeit { get; set; }

        #endregion

        #region KontoInformation
        
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public string BankInstitut { get; set; }

        #endregion
        
    }
}