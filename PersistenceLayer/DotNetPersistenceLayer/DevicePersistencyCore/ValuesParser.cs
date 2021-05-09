using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech
{
    /// <MetaDataID>{2af09f92-f671-46ad-b912-688362035008}</MetaDataID>
    public static class StringParser
    {
        public static bool TryConvert(this string stringValue, out int value)
        {
            value = 0;

            try
            {
                 
                value = int.Parse(stringValue);
                return true;
            }
            catch (Exception error)
            {
                return false;
            }

        }
        
    }
}
