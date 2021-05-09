using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OOAdvantech.PersistenceLayer;

namespace OOAdvantech.Collections.Generic
{
    /// <MetaDataID>{05bc0fa8-ac02-48e2-bd1e-3464013c8a82}</MetaDataID>
    public class MultilingualSet<T> : Set<T>, IMultilingual
    {
        public IDictionary Values
        {
            get
            {
                if (theObjects is IMultilingual)
                    return (theObjects as IMultilingual).Values;
                else
                    return null;
            }
        }

        public CultureInfo DefaultLanguage
        {
            get
            {
                if (theObjects is IMultilingual)
                    return (theObjects as IMultilingual).DefaultLanguage;
                else
                    return null;
            }
        }

        protected internal override ObjectCollection theObjects
        {
            get
            {
                if (_theObjects == null)
                {
#if !DeviceDotNet
                    if (ObjectCollectionFactory.MonoStateObjectCollectionFactory == null)
                        AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime,  Version=1.0.2.0, Culture=neutral, PublicKeyToken=95eeb2468d93212b"));
                    _theObjects = ObjectCollectionFactory.MonoStateObjectCollectionFactory.CreateMultilingualOnMemoryCollection();
#else
                    if (ObjectCollectionFactory.MonoStateObjectCollectionFactory == null)
                        AccessorBuilder.CreateInstance(ModulePublisher.ClassRepository.GetType("OOAdvantech.PersistenceLayerRunTime.ObjectCollectionFactory", "PersistenceLayerRunTime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
                    _theObjects = ObjectCollectionFactory.MonoStateObjectCollectionFactory.CreateMultilingualOnMemoryCollection();
#endif
                }
                return _theObjects;
            }
        }


    }
}
