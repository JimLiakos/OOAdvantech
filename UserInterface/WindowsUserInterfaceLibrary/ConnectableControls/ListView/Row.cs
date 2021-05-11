using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.ListView
{
    class Row:XPTable.Models.Row, IRow
    {
        #region IRow Members

        public object CollectionObject
        {
            get 
            {
                return _CollectionObject;
            }
        }

        #endregion


        internal object _PresentationObject;
        public object PresentationObject
        {
            get
            {
                if (_PresentationObject != null)
                    return _PresentationObject;
                return CollectionObject;
            }
        }
        public object _CollectionObject;

    }
}
