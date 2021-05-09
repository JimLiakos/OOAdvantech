using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OOAdvantech.Localization
{
    /// <MetaDataID>{a7bae3f2-1548-4e4b-8d12-6a3f0d2c0bc6}</MetaDataID>
    [ContentProperty(nameof(Text))]
    public class ResoucesStrings<T> : IMarkupExtension
    {
        readonly CultureInfo ci = null;

        public ResoucesStrings()
        {
            IDeviceInstantiator deviceInstantiator = DependencyService.Get<IDeviceInstantiator>();

            ILocalize localize = deviceInstantiator.GetDeviceSpecific(typeof(ILocalize)) as ILocalize;

            if (localize != null)
                ci = localize.GetCurrentCultureInfo();
           
        }
        public string Text { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            //string typ = typeof(T).FullName;
            //return "t_Sunny";

            if (Text == null)
                return "";


            //ResourceManager temp = new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly);
            ResourceManager temp = new ResourceManager(typeof(T));
            var translation = temp.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, typeof(T).FullName, ci.Name),
                    "Text");
#else
				translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}
