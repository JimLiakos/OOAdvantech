using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

using Xamarin.Forms;


namespace OOAdvantech.WindowsUniversal
{
    /// <MetaDataID>{a1a4ef78-5675-46f7-8032-eef223f9c5a2}</MetaDataID>
    public class Localize : OOAdvantech.Localization.ILocalize
    {
        public void SetLocale(CultureInfo ci)
        {
            //Thread.CurrentThread.CurrentCulture = ci;
            //Thread.CurrentThread.CurrentUICulture = ci;

            //Console.WriteLine("CurrentCulture set: " + ci.Name);
        }

        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";

            // this gets called a lot - try/catch can be expensive so consider caching or something

            CultureInfo ci = new CultureInfo(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            return ci;
        }


    }
}
