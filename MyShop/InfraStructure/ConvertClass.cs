using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyShop.InfraStructure
{
    public static class ConvertClass
    {
        
        public static string PersianToEnglish(this string persianStr)
        {
            if (string.IsNullOrWhiteSpace(persianStr))
            {
                return null;
            }
            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9'
            };
            
            foreach (var item in persianStr)
            {
                char val;
                if(!LettersDictionary.TryGetValue(item, out val))
                {
                    continue;
                }
              
                persianStr = persianStr.Replace(item, val);
            }
            return persianStr;
        }
    }
}
