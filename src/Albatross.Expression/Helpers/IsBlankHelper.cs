using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Linq;

namespace Albatross.Expression.Helpers
{
    public static class BlankHelper
    {
        public static bool IsBlank(object value)
        {
            if (value == null)
                return true;

            switch (value)
            {
                case string s:
                    return string.IsNullOrWhiteSpace(s);
                
                case IEnumerable enumerable:
                    var items = enumerable.Cast<object>();
                    return !items.Any(); 
                
                default:
                    return false; 
            }
        }

        public static bool IsNotBlank(object value) => !IsBlank(value);
    }
}