using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{1f9cd4ba-1126-41f5-ba3b-7438e1b1cd00}</MetaDataID>
    public class Grouping<TKey, TElement>:IGrouping<TKey,TElement> 
    {    
         
        /// <MetaDataID>{333f0edd-4a30-4dea-a8a7-4a8392bf7d8c}</MetaDataID>
        TKey Key;
        /// <MetaDataID>{c102692d-79dc-44a8-9446-02ee9c560368}</MetaDataID>
        DynamicTypeDataRetrieve<TElement> _Members;
        /// <MetaDataID>{2f3d8291-311e-4728-a183-26fb9519df21}</MetaDataID>
        IEnumerator<TElement> _Enumerator;
        /// <MetaDataID>{e32726af-47ba-4597-a4c5-6d102f2a5c88}</MetaDataID>
        internal Grouping(TKey key, object members)
        {    
            Key=key; 
            if(members is DynamicTypeDataRetrieve<TElement>)
                _Members = members as DynamicTypeDataRetrieve<TElement>;
            if (members is IEnumerator<TElement>)
                _Enumerator = members as IEnumerator<TElement>;

        }
        #region IGrouping<TKey,TElement> Members

        /// <MetaDataID>{51bd97b8-de3f-4e96-be1c-77bd5080fe19}</MetaDataID>
        TKey IGrouping<TKey, TElement>.Key
        {
            get
            {
                return Key;
            }
        }

        #endregion

        #region IEnumerable<TElement> Members

        /// <MetaDataID>{662bf570-d36a-4d90-89ee-500e6861feba}</MetaDataID>
        IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator()
        {
            if(_Members!=null)
                return _Members.GetEnumerator();
            _Enumerator.Reset();
            return _Enumerator;
        }

        #endregion

        #region IEnumerable Members

        /// <MetaDataID>{cca2bc33-c8b9-46e4-b60a-1771b7f6c256}</MetaDataID>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _Members.GetEnumerator();
        }

        #endregion
    }
}
