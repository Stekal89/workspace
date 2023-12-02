using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Models
{
    public class FinanzielleSituationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sie müssen ihr monatliches Netto-Einkommen eingeben.")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte Netto-Einkommen eingeben")]
        [Range(500, 10000, ErrorMessage = "Wert muss zwischen 500 und 10000 liegen!")]
        [Display(Name = "Netto-Einkommen (14x jährlich)")]
        public double NettoEinkommen { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Bitte die aktuellen Wohnkosten eingeben.")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte aktuellen Wohnkosten eingeben")]
        [Range(0, 10000, ErrorMessage = "Wert muss zwischen 0 und 10000 liegen!")]
        [Display(Name = "Wohnkosten (Miete, Heizung, Strom)")]
        public double Wohnkosten { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Sie müssen anderwertige Einkünfte aus wie zB. Alimente oder Unterhalt eingeben.")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl für anderwertige Einkünfte eingeben.")]
        [Range(0, 10000, ErrorMessage = "Wert muss zwischen 0 und 10000 liegen!")]
        [Display(Name = "Einkünfte aus Alimenten, Unterhalte usw.")]
        public double EinkuenfteAlimenteUnterhalt { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Bitte um Eingabe der verpflichtenden Zahlungen wie Alimente und Unterhalt.")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl eingeben")]
        [Range(0, 10000, ErrorMessage = "Wert muss zwischen 0 und 10000 liegen")]
        [Display(Name = "Zahlungen für Unterhalt, Alimente usw.")]
        public double UnterhaltsZahlungen { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Bitte Ratenverpflichtungen eingeben.")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl eingeben.")]
        [Range(0, 10000, ErrorMessage = "Wert muss zwischen 0 und 10000 liegen.")]
        [Display(Name = "Raten-Verpflichtungen")]
        public double RatenVerpflichtungen { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }
    }
}