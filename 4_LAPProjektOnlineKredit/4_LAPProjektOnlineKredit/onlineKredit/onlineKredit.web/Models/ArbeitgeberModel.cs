﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Models
{
    public class ArbeitgeberModel
    {
        [Required(ErrorMessage = "Bitte Firmennamen eingeben.")]
        [StringLength(100, ErrorMessage = "Maximal 100 Zeichen!")]
        [Display(Name = "Firma")]
        public string Firma { get; set; }

        [Required(ErrorMessage = "Bitte Beschäftigungsart wählen.")]
        [Display(Name = "Beschäftigungsart")]
        public int ID_BeschaeftigungsArt { get; set; }

        [Required(ErrorMessage = "Bitte Branche wählen.")]

        [Display(Name = "Branche")]
        public int ID_Branche { get; set; }

        [Required(ErrorMessage = "Bitte Datum wählen, seit dem sie in dieser Firma beschäftigt sind.")]
        [DataType(DataType.Date)]
        [Display(Name = "Beschäftigt seit")]
        [ValidHireDate]
        [DisplayFormat(DataFormatString ="{0:yyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BeschaeftigtSeit { get; set; }


        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public sealed class ValidHireDate : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime einstellDatum = Convert.ToDateTime(value);
                DateTime aktDatum = DateTime.Now;

                if (einstellDatum >= aktDatum)
                    return new ValidationResult("Ungültiges Einstelldatum!");
                else
                    return ValidationResult.Success;
                
            }
        }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }



        public List<BeshaeftigungsArtModel> AlleBeschaeftigungsArtAngabenWeb { get; set; }
        public List<BrancheModel> AlleBranchenAngabenWeb { get; set; }
    }
}