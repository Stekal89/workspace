using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class KreditKartenModel
    {
        /// <summary>
        /// Daten für die Kreditkartentabelle
        /// </summary>

        [Required(ErrorMessage = "Kreditkarteninhaber muss angegeben werden.")]
        [StringLength(50, ErrorMessage = "max. 50 Zeichen erlaubt")]
        [Display(Name = "Kreditkarteninhaber")]
        public string KreditKartenInhaber { get; set; }

        [Required CreditCard(ErrorMessage = "Kreditkartennummer muss angegeben werden")]
        [StringLength(23, ErrorMessage = "max. 23 Zeichen erlaubt")]
        [Display(Name = "Kreditkartennummer")]
        public string KreditKartenNummer { get; set; }
        
        [Required(ErrorMessage = "Gültigkeitsdatum muss eingegeben werden.")]
        [ValidCardDate] /// Eigen erstellte Validierung
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "{0:yyy - MM - dd}")]
        [Display(Name = "gültig bis")]
        public DateTime KreditKartenGueltigBis { get; set; }

        public sealed class ValidCardDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                /// Weise der Variable "datum" das eingegebene Datum zu
                DateTime datum = Convert.ToDateTime(value);
                DateTime aktDatum = DateTime.Now;   // Datum bekommt den Wert des heutigen Datums
                
                /// Überprüfe ob Datum schon vorbei ist
                if (datum <= DateTime.Now)
                    return new ValidationResult("Karte ist abgelaufen!");   /// ValidationMessage
                else
                    return ValidationResult.Success;    /// Freigabe (Valide)
            }
        }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }
    }
}