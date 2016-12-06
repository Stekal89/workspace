using System.Collections.Generic;

namespace onlineKredit.web.Models
{
    public class OrtModel : LookUpModel
    {
        /* Da ich hier vom LookUpModel erbe, und ich insgesammt 4 Eigenschaften benötige, muss ich nur
         * mehr 2 Eigenschaften für mein "OrtModel" zusätzlich erzeugen. */

        // Es wird davon ausgegangen, dass der User sowieso in Österreich wohnt, da er sonst keinen Kredit bekommen würde.
        public string FK_Land { get; set; }

        public string PostleitZahl { get; set; }

        /// Diese Variable benötige ich, damit ich in der Oberfläche 
        /// die Postleitzahl und den Ort in einem Dropdown anzeigen kann.
        public string PLZUndOrt { get; set; }
    }
}