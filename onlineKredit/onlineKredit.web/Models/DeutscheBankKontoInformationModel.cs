using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class DeutscheBankKontoInformationModel
    {
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Bitte Ihren IBAN angeben.")]
        [StringLength(30, ErrorMessage = "Maximal 30 Zeichen")]
        [Display(Name = "BIC")]
        public string BIC { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage = "Bitte Ihre Bankleitzahl angeben.")]
        [StringLength(30, ErrorMessage = "Maximal 30 Zeichen")]
        [Display(Name = "IBAN")]
        public string IBAN { get; set; }

        [HiddenInput(DisplayValue = false)]
        [StringLength(100, ErrorMessage = "Maximal 100 Zeichen.")]
        [Display(Name = "Bankinstitut")]
        public string BankInstitut { get; set; }

        /// <summary>
        /// Diese Information wird im Controller gesetzt!!!
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public bool IstDeutscheBankKunde { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }
    }
}