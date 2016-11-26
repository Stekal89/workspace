using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace onlineKredit.web.Models
{
    public enum Geschlecht
    {
        Männlch,
        Weiblich
    }

    public class PersoenlicheDatenModel
    {
        public Geschlecht Geschlecht { get; set; }

        public string Titel { get; set; }

        public string Vorname { get; set; }

        public string Nachname { get; set; }

        public DateTime GeburtsDatum { get; set; }

        public string Staatsbuergerschaft { get; set; }

        public int AnzahlKinder { get; set; }

        public string Familienstand { get; set; }

        public string Wohnart { get; set; }

        public string IdentifikationsArt { get; set; }

        public string IdentifikationsNummer { get; set; }

    }
}