using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Models
{
    public enum Geschlecht
    {
        [Display(Name = "Herr")]
        Männlch,

        [Display(Name = "Frau")]
        Weiblich
    }

    public class PersoenlicheDatenModel
    {
        [Required]
        [EnumDataType(typeof(Geschlecht))]
        public Geschlecht Geschlecht { get; set; }
        
        [Display(Name = "Titel")]
        public int? ID_Titel { get; set; }  // weil nicht jede Person einen Titel hat, kann sie 
                                            // in der Tabelle auch NULL sein, deswegen verwenden
                                            // wir hier einen NULLABLE INTEGER "int?"
        
        [Required(ErrorMessage = "Bitte den Vornamen eingeben.")]
        [StringLength(50, ErrorMessage = "Maximal 50 Zeichen!")]
        public string Vorname { get; set; }

        [Required(ErrorMessage = "Bitte Nachnamen eingeben.")]
        [StringLength(50, ErrorMessage = "Maximal 50 Zeichen!")]
        public string Nachname { get; set; }

        [Required(ErrorMessage = "Bitte Geburtsdatum wählen.")]
        [DataType(DataType.Date)]
        public DateTime GeburtsDatum { get; set; }

        [Required(ErrorMessage = "Bitte Staatsbürgerschaft auswählen.")]
        [StringLength(3, ErrorMessage = "Maximal 3 Zeichen!")]
        [Display(Name = "Staatsbürgerschaft")]
        public string ID_Staatsbuergerschaft { get; set; } // Von Tabelle Land

        [Display(Name = "Anzahl unterhaltspflichtiger Kinder")]
        public int AnzahlKinder { get; set; }

        [Required(ErrorMessage = "Bitte Familienstand auswählen")]
        [Display(Name = "aktueller Familienstand")]
        public int ID_Familienstand { get; set; }

        [Required(ErrorMessage = "Bitte Wohnart auswählen")]
        [Display(Name = "aktuelle Wohnsituation")]
        public int ID_Wohnart { get; set; }

        [Required(ErrorMessage = "Bitte Ihre höchst abgeschlossenen Ausbildung auswählen")]
        [Display(Name = "höchst abgeschlossene Ausbildung")]
        public int ID_SchulAbschluss { get; set; }

        [Required(ErrorMessage = "Bitte Identifikationsart wählen. (Reisepass, Fürhrerschein,...")]
        [Display(Name = "Identifikationstyp")]
        public int ID_IdentifikationsArt { get; set; }


        [StringLength(30, ErrorMessage = "max. 30 Zeichen erlaubt")]
        [Display(Name = "Identifikations-Nummer")]
        public string IdentifikationsNummer { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }

        public List<TitelModel> AlleTitelAngabenWeb { get; set; }
        public List<LaenderModel> AlleLaenderAngabenWeb { get; set; } // für Staatsbürgerschaft
        public List<FamilienStandsModel> AlleFamilienstandsAngabenWeb { get; set; }
        public List<SchulabschlussModel> AlleSchulabschlussAngabenWeb { get; set; }
        public List<IdentifikationsArtModel> AlleIdentifikationsArtAngabenWeb { get; set; }
        public List<WohnartModel> AlleWohnartsAngabenWeb { get; set; }
    }
}