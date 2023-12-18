using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIBaseEx
{
    /// <MetaDataID>{539bad51-aa09-4c87-af8e-c7fe084f6aaf}</MetaDataID>
    public class ViewModelWrappers<TKey, TValue> : Dictionary<TKey, TValue>   where TValue: class
    {

        public delegate void OnNewViewModelWrapperHandler(ViewModelWrappers<TKey, TValue> sender, TKey key, TValue value);

        public TValue GetViewModelFor(TKey _object, params object[] args)
        {
            if(_object == null) 
                return null;
            TValue value = default(TValue);
            if (!TryGetValue(_object, out value))
            {
                value = Activator.CreateInstance(typeof(TValue), args) as TValue;
                this[_object] = value;

                OnNewViewModelWrapper?.Invoke(this, _object, value);


            }
            return value;
             
        }
        public  event OnNewViewModelWrapperHandler OnNewViewModelWrapper;
    }
}
