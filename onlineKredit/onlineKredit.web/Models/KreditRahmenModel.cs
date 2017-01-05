using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class KreditRahmenModel
    {
        [Required(ErrorMessage = "Bitte wählen sie ihren gewünschten Betrag!")]
        [Display(Name ="Gewünschter Betrag")]
        [Range(1000,100000, ErrorMessage ="Betrag muss zwischen 500 € und 10000 € liegen.")]
        public int GewuenschterBetrag { get; set; }

        [Required(ErrorMessage ="Es muss eine gewünschte Laufzeit angegeben sein! (minimum 1 Monat)")]
        [Display(Name ="Laufzeit in Monaten")]
        [Range(1,120, ErrorMessage ="Laufzeit muss zwischen 1 und 120 Monaten liegen.")]
        public short Laufzeit { get; set; }

        ///// <summary>
        ///// Wird benötigt, wenn man von der Zusammenfassung die Daten, anzeigen bzw. speichern möchte!!!
        ///// </summary>
        //[HiddenInput(DisplayValue = false)]
        //public int KundenID { get; set; }
    }
}