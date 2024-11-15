using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public static class ParamaterException
    {
        public static void CheckIfStringIsNotNullOrEmpty(string value,string paramName) 
        {
            if (string.IsNullOrEmpty(value)) { throw new ArgumentException("Cannot be null", paramName); }
        }

        public static void CheckIfLongIsBiggerThanZero(long value, string paramName)
        {
            if (value<1) { throw new ArgumentException("Cannot be smaller than 1", paramName); }
        }

        public static void CheckIfIntIsBiggerThanZero(int value, string paramName)
        {
            if (value < 1) { throw new ArgumentException("Cannot be smaller than 1", paramName); }
        }

        public static void CheckIfObjectIfNotNull<T>(T value, string paramName)
        {
            if (value is null) { throw new ArgumentNullException("Cannot be null", paramName); };
        }
        
        public static void CheckIfIEnumerableIsNotNullOrEmpty<T>(IEnumerable<T> value, string paramName)
        {
            if(value is null || !value.Any()) throw new ArgumentException("Cannot be null or empty");
        }


    }
}
