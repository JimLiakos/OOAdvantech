using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.Json;

namespace OOAdvantech
{
    /// <MetaDataID>{a6a905f8-8da3-4983-9cb4-37de81be5503}</MetaDataID>
    public class Multilingual: IMultilingual
    {

        /// <MetaDataID>{307c6aa8-57fd-435f-8ea1-2e65a20ee987}</MetaDataID>
        public Multilingual()
        {
            Values = new Dictionary<string, object>();
        }


        /// <MetaDataID>{2dd74378-f0ef-4561-b44e-d4c3e626ace9}</MetaDataID>
        public Multilingual(IMultilingual multilingual)
        {
            if (multilingual.DefaultLanguage != null)
                Def = multilingual.DefaultLanguage.Name;
            Values = new Dictionary<string, object>();
            foreach (System.Collections.DictionaryEntry entry in multilingual.Values)
                Values.Add((entry.Key as System.Globalization.CultureInfo).Name, entry.Value);
        }

        /// <MetaDataID>{70a69973-b4f4-415f-95d9-6e56c7536158}</MetaDataID>
        public Multilingual(Multilingual multilingualCopy)
        {
            Def = multilingualCopy.Def;
            Values = new Dictionary<string, object>(multilingualCopy.Values);
        }

        /// <MetaDataID>{48736162-5ea1-4be3-a606-96a99c237be9}</MetaDataID>
        public string Def { get; set; }

        /// <MetaDataID>{3c81da63-85d4-4f94-8931-5d6aa017e652}</MetaDataID>
        public Dictionary<string, object> Values { get; set; }

        [JsonIgnore]
        public bool HasValue
        {
            get
            {
                var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
                bool useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
                object value =null;
                if (Values.TryGetValue(cultureInfo.Name, out value))
                    return true;
                else
                    return false;
            }
        }

        public System.Globalization.CultureInfo DefaultLanguage
        {
            get
            {
                if (Def != null)
                    return System.Globalization.CultureInfo.GetCultureInfo(Def);
                else
                    return null;

            }
        }

        IDictionary IMultilingual.Values => this.Values;


        /// <MetaDataID>{8a2884ff-4d74-41a1-a00d-d83288d9aeeb}</MetaDataID>
        public T GetValue<T>()
        {

            var cultureInfo = OOAdvantech.CultureContext.CurrentNeutralCultureInfo;
            bool useDefaultCultureValue = OOAdvantech.CultureContext.UseDefaultCultureValue;
            object value = default(T);
            if (Values.TryGetValue(cultureInfo.Name, out value))
            {
                if (value is T)
                    return (T)value;
                else
                    return default(T);
            }
            else
            {
                if (useDefaultCultureValue && !string.IsNullOrWhiteSpace(Def))
                {
                    if (Values.TryGetValue(Def, out value))
                    {
                        if (value is T)
                            return (T)value;
                        else
                            return default(T);

                    }
                }

                return default(T);
            }
        }


        /// <MetaDataID>{65e4dff9-60e3-4886-af67-5279fe876387}</MetaDataID>
        public void SetValue<T>(T value)
        {
            Values[OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name] = value;
        }
    }
}
