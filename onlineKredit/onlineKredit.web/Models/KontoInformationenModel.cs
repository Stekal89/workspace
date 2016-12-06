using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class KontoInformationenModel
    {
        [Required(ErrorMessage = "Bitte Ihren IBAN angeben.")]
        [StringLength(25, ErrorMessage = "Maximal 25 Zeichen")]
        public string BIC { get; set; }

        [Required(ErrorMessage = "Bitte Ihre Bankleitzahl angeben.")]
        [StringLength(25, ErrorMessage = "Maximal 25 Zeichen")]
        public string IBAN { get; set; }

        [Required(ErrorMessage = "Bitte den Namen Ihres Bankinstituts angeben.")]
        [StringLength(100, ErrorMessage = "Maximal 100 Zeichen.")]
        public string BankInstitut { get; set; }

        [Required]
        public bool IstDeutscheBankKunde { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }

    }
}