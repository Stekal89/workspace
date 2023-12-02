using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatumGueltigkeitsUeberpruefung
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime datum = new DateTime(2017, 1,1);
            DateTime datum2 = new DateTime(2017,2,3);
            DateTime datum3 = DateTime.Now;
            DateTime datum4 = new DateTime(2017,8,9);


            bool gueltig = DatumGueltigkeitsUeberpruefen(datum);
            Console.WriteLine("Gültig: " + gueltig);

            gueltig = DatumGueltigkeitsUeberpruefen(datum2);
            Console.WriteLine("Gültig: " + gueltig);

            gueltig = DatumGueltigkeitsUeberpruefen(datum3);
            Console.WriteLine("Gültig: " + gueltig);

            gueltig = DatumGueltigkeitsUeberpruefen(datum4);
            Console.WriteLine("Gültig: " + gueltig);

            Console.ReadKey();
        }

        public static bool DatumGueltigkeitsUeberpruefen(DateTime datum)
        {
            Console.WriteLine("\n\n");
            if (datum <= DateTime.Now)
            {
                Console.WriteLine("Heute ist der: " + DateTime.Now);
                Console.WriteLine("Eingabe: " + datum);
                Console.WriteLine("Karte abgelaufen, oder Karte läuft Heute ab!");
                return false;
            }
            else
            {
                Console.WriteLine("Heute ist der: " + DateTime.Now);
                Console.WriteLine("Eingabe: " + datum);
                Console.WriteLine("Datum gültig.");
                return true;
            }
        }
    }
}
