using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class LogInModel
    {
        [StringLength(50, ErrorMessage = "max. 50 Zeichen erlaubt.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [Display(Name = "Benutzername")]
        public string Benutzername { get; set; }

        [StringLength(50, ErrorMessage = "max. 50 Zeichen erlaubt.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pflichtfeld")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Passwort { get; set; }
    }
}