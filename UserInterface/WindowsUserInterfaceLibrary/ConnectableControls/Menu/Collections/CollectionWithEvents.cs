// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetConnectableControls.com
// *****************************************************************************

using System;
using System.Collections;

namespace ConnectableControls.Menus.Collections
{
    
    // Declare the event signatures
    public delegate void CollectionClear();
    public delegate void CollectionChange(int index, object value);

	/// <MetaDataID>{6828AB58-6FA7-46B2-8A9F-C6726D8A5AE9}</MetaDataID>
    public class CollectionWithEvents :IList
    {
        // Collection change events
        public event CollectionClear Clearing;
        public event CollectionClear Cleared;
        public event CollectionChange Inserting;
        public event CollectionChange Inserted;
        public event CollectionChange Removing;
        public event CollectionChange Removed;
	
        // Overrides for generating events
		/// <MetaDataID>{954BDD6F-83D6-4FEE-8658-D30D242EE96D}</MetaDataID>
        protected virtual void OnClear()
        {
            // Any attached event handlers?
            if (Clearing != null)
                Clearing();
        }	

		/// <MetaDataID>{ECC8E81C-F451-4BE4-8009-EAE479FE5AA5}</MetaDataID>
        protected virtual void OnClearComplete()
        {
            // Any attached event handlers?
            if (Cleared != null)
                Cleared();
        }	

		/// <MetaDataID>{93AF1E56-CE22-4356-8BA3-EEC4A0FBB2C1}</MetaDataID>
        protected virtual void OnInsert(int index, object value)
        {
            // Any attached event handlers?
            if (Inserting != null)
                Inserting(index, value);
        }

		/// <MetaDataID>{972DD05A-BE90-4C30-B6B0-B263EA7D090F}</MetaDataID>
        protected virtual void OnInsertComplete(int index, object value)
        {
            // Any attached event handlers?
            if (Inserted != null)
                Inserted(index, value);
        }
        protected virtual void OnSetComplete(int index, object oldValue, object newValue)
        {
            
        }
        protected virtual void OnValidate(object value)
        {
            
        }

		/// <MetaDataID>{535930A9-B3B1-4647-8DC3-E8761B12B4CC}</MetaDataID>
        protected virtual void OnRemove(int index, object value)
        {
            // Any attached event handlers?
            if (Removing != null)
                Removing(index, value);
        }

		/// <MetaDataID>{3586614F-49AD-4E66-8D95-50DD266A7902}</MetaDataID>
        protected virtual void OnRemoveComplete(int index, object value)
        {
            // Any attached event handlers?
            if (Removed != null)
                Removed(index, value);
        }

   
        ArrayList list;
        protected ArrayList InnerList
        {
            get
            {
                if (this.list == null)
                {
                    this.list = new ArrayList();
                }
                return this.list;
            }
        }
 


        #region IList Members

        protected IList List
        {
            get
            {
                return this;
            }
        }
 

        public int Add(object value)
        {
            this.OnValidate(value);
            this.OnInsert(this.InnerList.Count, value);
            int num1 = this.InnerList.Add(value);
            try
            {
                this.OnInsertComplete(num1, value);
            }
            catch
            {
                this.InnerList.RemoveAt(num1);
                throw;
            }
            return num1;

        }

        public void Clear()
        {
            this.OnClear();
            this.InnerList.Clear();
            this.OnClearComplete();
        }

 


        public bool Contains(object value)
        {
            return this.InnerList.Contains(value);
        }




        public int IndexOf(object value)
        {
            return this.InnerList.IndexOf(value);
        }




        public void Insert(int index, object value)
        {
            if ((index < 0) || (index > this.InnerList.Count))
            {
                throw new ArgumentOutOfRangeException("index", "Argument out of range");
            }
            this.OnValidate(value);
            this.OnInsert(index, value);
            this.InnerList.Insert(index, value);
            try
            {
                this.OnInsertComplete(index, value);
            }
            catch
            {
                this.InnerList.RemoveAt(index);
                throw;
            }
        }

 


        public bool IsFixedSize
        {
            get { return this.InnerList.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return this.InnerList.IsReadOnly; }
        }

        public void Remove(object value)
        {
            this.OnValidate(value);
            int num1 = this.InnerList.IndexOf(value);
            if (num1 < 0)
            {
                throw new ArgumentException("Remove argument not found");
            }
            this.OnRemove(num1, value);
            this.InnerList.RemoveAt(num1);
            try
            {
                this.OnRemoveComplete(num1, value);
            }
            catch
            {
                this.InnerList.Insert(num1, value);
                throw;
            }

        }

        public void RemoveAt(int index)
        {
            if ((index < 0) || (index >= this.InnerList.Count))
            {
                throw new ArgumentOutOfRangeException("index", "Argument out of range");
            }
            object obj1 = this.InnerList[index];
            this.OnValidate(obj1);
            this.OnRemove(index, obj1);
            this.InnerList.RemoveAt(index);
            try
            {
                this.OnRemoveComplete(index, obj1);
            }
            catch
            {
                this.InnerList.Insert(index, obj1);
                throw;
            }

        }

        public object this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.InnerList.Count))
                {
                    throw new ArgumentOutOfRangeException("index", "Argument out of range");
                }
                return this.InnerList[index];
            }
            set
            {
                if ((index < 0) || (index >= this.InnerList.Count))
                {
                    throw new ArgumentOutOfRangeException("index", "Argument out of range");
                }
                this.OnValidate(value);
                object obj1 = this.InnerList[index];
                this.OnSet(index, obj1, value);
                this.InnerList[index] = value;
                try
                {
                    this.OnSetComplete(index, obj1, value);
                }
                catch
                {
                    this.InnerList[index] = obj1;
                    throw;
                }
            }

        }


        #endregion
        protected virtual void OnSet(int index, object oldValue, object newValue)
        {
        }



        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            this.InnerList.CopyTo(array, index);
        }

        public int Count
        {
            get 
            {
                return this.InnerList.Count;

            }
        }

        public bool IsSynchronized
        {
            get
            {
                return this.InnerList.IsSynchronized;
            }
        }

        public object SyncRoot
        {
            get { return this.InnerList.SyncRoot; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        #endregion
    }
}
