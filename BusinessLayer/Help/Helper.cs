using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Help
{
    public static class Helper
    {
        private static readonly Random _random = new Random();

        public static int GenerateRandomSixDigitNumber()
        {
            return _random.Next(100000, 1000000);// الادني مشمول والاقصي غير مشمول
        }

        public static string? GetDescription(string? Description)
        {
            return string.IsNullOrEmpty(Description) ? null : Description;
        }
    }
}
