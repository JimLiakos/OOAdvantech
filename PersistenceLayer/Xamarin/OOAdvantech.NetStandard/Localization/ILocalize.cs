using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOAdvantech.Localization
{
    /// <summary>
    ///     /// Implementations of this interface MUST convert iOS and Android
    ///     /// platform-specific locales to a value supported in .NET because
    ///     /// ONLY valid .NET cultures can have their RESX resources loaded and used.
    ///     /// </summary>
    /// <remarks>
    ///     /// Lists of valid .NET cultures can be found here:
    ///     ///   http://www.localeplanet.com/dotnet/
    ///     ///   http://www.csharp-examples.net/culture-names/
    ///     /// You should always test all the locales implemented in your application.
    ///     /// </remarks>
    /// <MetaDataID>{c1d245ae-6a74-424d-bff0-00e12205327a}</MetaDataID>
    public interface ILocalize
    {
        ///	<summary>
        /// This method must evaluate platform-specific locale settings
        /// and convert them (when necessary) to a valid .NET locale.
        /// </summary>
        CultureInfo GetCurrentCultureInfo();

        /// <summary>
        /// CurrentCulture and CurrentUICulture must be set in the platform project, 
        /// because the Thread object can't be accessed in a PCL.
        /// </summary>
        void SetLocale(CultureInfo ci);
    }

    /// <summary>
    /// Helper class for splitting locales like
    ///   iOS: ms_MY, gsw_CH
    ///   Android: in-ID
    /// into parts so we can create a .NET culture (or fallback culture)
    /// </summary>
    /// <MetaDataID>{9e5150d5-f0c0-410b-8687-174ae7e8c735}</MetaDataID>
    public class PlatformCulture
    {
        public PlatformCulture(string platformCultureString)
        {
            if (String.IsNullOrEmpty(platformCultureString))
                throw new ArgumentException("Expected culture identifier", "platformCultureString"); // in C# 6 use nameof(platformCultureString)

            PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
            var dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);
            if (dashIndex > 0)
            {
                var parts = PlatformString.Split('-');
                LanguageCode = parts[0];
                LocaleCode = parts[1];
            }
            else
            {
                LanguageCode = PlatformString;
                LocaleCode = "";
            }
        }
        public string PlatformString { get; private set; }
        public string LanguageCode { get; private set; }
        public string LocaleCode { get; private set; }
        public override string ToString()
        {
            return PlatformString;
        }
    }
}

