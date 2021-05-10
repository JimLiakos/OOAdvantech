using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Specialized;

namespace PushBindingExtension
{
    /// <MetaDataID>{cf221ba2-45c2-44eb-88bf-9dfd64d69562}</MetaDataID>
    public class PushBindingCollection : FreezableCollection<PushBinding>
    {
        public PushBindingCollection(FrameworkElement targetObject)
        {
            TargetObject = targetObject;
            ((INotifyCollectionChanged)this).CollectionChanged += CollectionChanged;
        }

        void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (PushBinding pushBinding in e.NewItems)
                {
                    pushBinding.SetupTargetBinding(TargetObject);
                }
            }
        }

        public FrameworkElement TargetObject
        {
            get;
            private set;
        }
    }
}
