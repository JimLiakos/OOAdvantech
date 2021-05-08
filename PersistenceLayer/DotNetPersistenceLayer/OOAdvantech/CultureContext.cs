using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Globalization;
using System;

namespace OOAdvantech
{
    /// <MetaDataID>{43ae5752-905a-4f5e-87a8-c3eeb4cfb1bd}</MetaDataID>
    public class CultureContext : System.IDisposable
    {

        bool UseDefaultCultureWhenValueMissing;

        bool PreviousUseDefaultCultureWhenValueMissing;
        System.Globalization.CultureInfo CultureInfo;
        System.Globalization.CultureInfo PreviousCultureInfo;
        public CultureContext(System.Globalization.CultureInfo cultureInfo,bool useDefaultCultureWhenValueMissing)
        {
            
            PreviousCultureInfo = CallContext.GetData("Culture") as System.Globalization.CultureInfo;
            if(CallContext.GetData("UseDefaultCultureWhenValueMissing")!=null)
                PreviousUseDefaultCultureWhenValueMissing = (bool)CallContext.GetData("UseDefaultCultureWhenValueMissing");
            UseDefaultCultureWhenValueMissing = useDefaultCultureWhenValueMissing;
            CultureInfo = cultureInfo;
            CallContext.SetData("Culture", cultureInfo);
            CallContext.SetData("UseDefaultCultureWhenValueMissing", useDefaultCultureWhenValueMissing);

            //using
        }

      

       static System.Globalization.CultureInfo _CurrentCultureInfo;
       public static System.Globalization.CultureInfo CurrentCultureInfo
        {
            get
            {
                var currentCultureInfo= CallContext.GetData("Culture") as System.Globalization.CultureInfo;
                if (currentCultureInfo == null)
                    currentCultureInfo = _CurrentCultureInfo;
                if (currentCultureInfo == null)
                    currentCultureInfo =System.Globalization.CultureInfo.CurrentCulture;


                return currentCultureInfo;
            }
            set
            {
                _CurrentCultureInfo = value;
            }
        }

        internal static CultureInfo GetNeutralCultureInfo(string culture)
        {
            var currentCultureInfo =  System.Globalization.CultureInfo.GetCultureInfo(culture);
            if (NeutralCultures.ContainsKey(currentCultureInfo.Name))
                return NeutralCultures[currentCultureInfo.Name];

            if (currentCultureInfo.Parent != null && NeutralCultures.ContainsKey(currentCultureInfo.Parent.Name))
                return NeutralCultures[currentCultureInfo.Parent.Name];
            else
                return null;

        }

        static Dictionary<string, System.Globalization.CultureInfo> NeutralCultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.NeutralCultures).ToDictionary(x => x.Name);
        public static System.Globalization.CultureInfo CurrentNeutralCultureInfo
        {
            get
            {
                var currentCultureInfo = CallContext.GetData("Culture") as System.Globalization.CultureInfo;
                if (currentCultureInfo == null)
                    currentCultureInfo = _CurrentCultureInfo;
                if (currentCultureInfo == null)
                    currentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;

                if(NeutralCultures.ContainsKey(currentCultureInfo.Name))
                    return NeutralCultures[currentCultureInfo.Name];

                if (currentCultureInfo.Parent != null && NeutralCultures.ContainsKey(currentCultureInfo.Parent.Name))
                    return NeutralCultures[currentCultureInfo.Parent.Name];
                else
                    return null;

                
            }
            set
            {
                _CurrentCultureInfo = value;
            }
        }



        public static bool UseDefaultCultureValue
        {
            get
            {

                object value = CallContext.GetData("UseDefaultCultureWhenValueMissing");
                if (value is bool)
                    return (bool)value;
                else
                    return false;
            }
        }

        public void Dispose()
        {
            if (PreviousCultureInfo == null)
            {
                CallContext.FreeNamedDataSlot("Culture");
                CallContext.FreeNamedDataSlot("UseDefaultCultureWhenValueMissing");
            }
            else
            {
                CallContext.SetData("Culture", PreviousCultureInfo);
                CallContext.SetData("UseDefaultCultureWhenValueMissing", PreviousUseDefaultCultureWhenValueMissing);
            }
        }
    }
}