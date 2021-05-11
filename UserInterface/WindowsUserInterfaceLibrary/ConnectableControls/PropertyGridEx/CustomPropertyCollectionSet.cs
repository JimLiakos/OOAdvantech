namespace ConnectableControls
{
    using System;
    using System.Collections;
    using System.Reflection;

    /// <MetaDataID>{0f801928-a89f-4f04-802c-9bf6690a17f2}</MetaDataID>
    public class CustomPropertyCollectionSet : System.Collections.CollectionBase
	{
		
		
		public virtual int Add(CustomPropertyCollection value)
		{
			return base.List.Add(value);
		}
		
		public virtual int Add()
		{
			return base.List.Add(new CustomPropertyCollection());
		}
		
		public virtual CustomPropertyCollection this[int index]
		{
			get
			{
				return ((CustomPropertyCollection) base.List[index]);
			}
			set
			{
				base.List[index] = value;
			}
		}
		
		public virtual object ToArray()
		{
			ArrayList list = new ArrayList();
			list.AddRange(base.List);
			return list.ToArray(typeof(CustomPropertyCollection));
		}
		
	}
}
