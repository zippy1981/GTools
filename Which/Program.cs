using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace Which
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Default;

            decimal[] numbers = {
                0.01m,
                0.10m,
                1.12m,
                1.00m,
                100.00m,
                123.00m,
                200.00m,
                1230913321,11m,
            };

            string[] isocodes = { "EUR", "USD", "GBP" };
            
            foreach (string isocode in isocodes)
            {
                foreach (decimal number in numbers)
                {
                    AmountWithCurrency pos = new AmountWithCurrency(number, isocode);
                    AmountWithCurrency neg = new AmountWithCurrency(-number, isocode);
                    Console.WriteLine(string.Format("{0,20} {1,20} {2,20}",
                        pos, neg, pos.AsNoteValue()));

                } 
            }
            Console.ReadLine();
        }
    }
}
