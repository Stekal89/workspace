using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class KontoInformationenModel
    {
        public string BIC { get; set; }

        public string IBAN { get; set; }

        public string BankInstitut { get; set; }

        public bool IstDeutscheBankKunde { get; set; }

    }
}