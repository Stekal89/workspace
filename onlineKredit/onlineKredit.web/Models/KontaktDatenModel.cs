using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace onlineKredit.web.Models
{
    public class KontaktDatenModel
    {
        [Required(ErrorMessage = "Bitte Strasse angeben, in der sie wohnen.")]
        [StringLength(50, ErrorMessage = "Maximal 50 Zeichen!")]
        public string Strasse { get; set; }

        [Required(ErrorMessage = "Bitte Hausnummer angeben, in der sie wohnen.")]
        [StringLength(50, ErrorMessage = "Maximal 20 Zeichen!")]
        public string Hausnummer { get; set; }

        // Keine Eingabepflicht
        [StringLength(20, ErrorMessage = "Maximal 20 Zeichen!")]
        public string Stiege { get; set; }

        // Keine Eingabepflicht
        [StringLength(20, ErrorMessage = "Maximal 20 Zeichen!")]
        public string Tuer { get; set; }

        // Dropdownliste / Multi??
        [Required(ErrorMessage = "Bitte Postleitzahl wählen")]
        public int FK_Ort { get; set; }
        
        [DataType(DataType.EmailAddress, ErrorMessage = "Keine gültige E-Mail Adresse!!")]
        [StringLength(100, ErrorMessage = "Maximal 100 Zeichen!")]
        public string EMail { get; set; }
                                                                        // E-Mail oder Telefonnummer müssen nicht zwingend angegeben werden, da sich 
                                                                        // der Mitarbeiter auch per Postsendeadresse in Verbindung setzen kann!!!
        [StringLength(100, ErrorMessage = "Maximal 100 Zeichen!")]
        public string TelefonNummer { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int KundenID { get; set; }

        public List<OrtModel> AlleOrtsAngabenWeb { get; set; }
    }
}