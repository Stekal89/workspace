namespace onlineKredit.web.Models
{
    internal class OrtModel : LookUpModel
    {
        /* Da ich hier vom LookUpModel erbe, und ich insgesammt 4 Eigenschaften benötige, muss ich nur
         * mehr 2 Eigenschaften für mein "OrtModel" zusätzlich erzeugen. */

        // Es wird davon ausgegangen, dass der User sowieso in Österreich wohnt, da er sonst keinen Kredit bekommen würde.
        public string FK_Land { get; set; }

        public int PostleitZahl { get; set; }
    }
}