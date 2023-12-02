using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimeValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("\nTag eingeben: ");
            string tagEingabe = Console.ReadLine();

            Console.Write("\nMonat eingeben: ");
            string monatEingabe = Console.ReadLine();

            Console.Write("\nJahr eingeben: ");
            string jahrEingabe = Console.ReadLine();

            int tag;
            int monat;
            int jahr;

            int.TryParse(tagEingabe, out tag);
            int.TryParse(monatEingabe, out monat);
            int.TryParse(jahrEingabe, out jahr);

            DateTime gebDat = new DateTime(jahr, monat, tag);

            Console.WriteLine("\n\nGeburtsdatum:" + gebDat);

            int alter = AlterEinerPerson(gebDat);

            Console.WriteLine("\nDu bist " + alter + " Jahre alt.");

            Console.WriteLine("\n\n\nZum beenden beliebige Taste drücken....");
            Console.ReadKey();
        }

        /// <summary>
        /// Ermittelt das Alter einer Person
        /// </summary>
        /// <param name="gebDat">Geburtsdatum</param>
        /// <returns>Das Alter einer Person</returns>
        static int AlterEinerPerson(DateTime gebDat)
        {
            DateTime aktDatum = DateTime.Now;
            if (aktDatum.Month <= gebDat.Month && aktDatum.Day < gebDat.Day)
            {
                return aktDatum.Year - gebDat.Year - 1;
            }
            else
            {
                return aktDatum.Year - gebDat.Year;
            }
        }
    }
}
