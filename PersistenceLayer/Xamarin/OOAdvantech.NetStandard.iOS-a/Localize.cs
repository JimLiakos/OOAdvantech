using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace OOAdvantech.iOS
{
    /// <MetaDataID>{2397c408-eeed-4310-a231-4553fcdcb40c}</MetaDataID>
    public class Localize : OOAdvantech.Localization.ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            try
            {
                return CultureInfo.GetCultureInfo(
                    NSLocale.CurrentLocale.LocaleIdentifier.Replace('_', '-'));
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.GetCultureInfo(NSLocale.CurrentLocale.LanguageCode);
            }
        }

        public void SetLocale(CultureInfo ci)
        {
            
        }
    }
}