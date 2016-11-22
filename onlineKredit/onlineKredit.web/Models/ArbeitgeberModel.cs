using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public class ArbeitgeberModel
    {
        public string Firma { get; set; }

        public string BeschaeftigungsArt { get; set; }

        public string Branche { get; set; }

        public DateTime BeschaeftigtSeit { get; set; }

    }
}