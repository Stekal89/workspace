using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datum
{
    class Program
    {
        static void Main(string[] args)
        {
            

            DateTime erstelltAm = new DateTime(2016, 01, 15);
            DateTime aktDatum = DateTime.Now;

            // Differenz in Tage, Stunden, Minuten 
            TimeSpan ts = aktDatum - erstelltAm;

            // vergangene Tage
            int vergangeneTage = ts.Days;

            Console.WriteLine("Vergangene Tage: {0} ", vergangeneTage);
            Console.ReadLine();
            
            Console.ReadKey();
        }
    }
}
