using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class KontoAbfrageModel
    {
        [Required (ErrorMessage = "Bitte Ihre Angaben zum Konto machen, mittels dem Sie den Kredit abwickeln möchten.")]
        [Display(Name = "Kontaktabfrage")]
        public int ID_KontoAbfrage { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }

        public List<KontoAbfrageMoeglichkeitModel> AlleKontoAbfrageMoeglichkeitenAngaben { get; set; }
    }
}