using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Help
{
    public static class Helper
    {
        public static bool IsArabicLang(string input)
        {
            return input.Any(c => c >= 0x0600 && c <= 0x06FF); // Arabic Unicode block
        }
    }
}
