using System;

namespace ConnectableControls.Menus.Common
{
    
	/// <summary>
	/// Summary description for Clipboard.
	/// </summary>
	/// <MetaDataID>{24C6D316-98C2-4303-9879-52D2C59F8F1D}</MetaDataID>
	public class Clipboard
	{
		/// <MetaDataID>{DDEEB884-675C-4396-88F4-61BA8F46F870}</MetaDataID>
		[Serializable]
		public 	class DataObject
		{
			public Guid RefrenceObject=Guid.Empty;
			
		}
		/// <MetaDataID>{225896F7-816A-43FB-9EE0-95FCB9DAEAD6}</MetaDataID>
		Clipboard()
		{
		}
		static System.Collections.Hashtable ReferenceObjects=new System.Collections.Hashtable();
		
		/// <MetaDataID>{EFC1D49E-2107-4443-943E-2B9B3A7C62F4}</MetaDataID>
		public static void SetData(object data,bool copy)
		{
			if(data==null)
				return;
			if(data is System.Windows.Forms.IDataObject)
				System.Windows.Forms.Clipboard.SetDataObject(data,copy);
			else if(data.GetType().IsSerializable)
			{
				System.Windows.Forms.DataObject dataObject=new System.Windows.Forms.DataObject();
				dataObject.SetData(data.GetType(),data);
				System.Windows.Forms.Clipboard.SetDataObject(dataObject,copy);
			}
			else
			{
				DataObject _DataObject;
				if(System.Windows.Forms.Clipboard.GetDataObject().GetDataPresent(data.GetType()))
				{
					_DataObject= System.Windows.Forms.Clipboard.GetDataObject().GetData(data.GetType()) as DataObject;
					ReferenceObjects.Remove(_DataObject.RefrenceObject);
				}

				_DataObject=new DataObject();
				_DataObject.RefrenceObject=Guid.NewGuid();

				ReferenceObjects.Add(_DataObject.RefrenceObject,data);
				System.Windows.Forms.DataObject dataObject=new System.Windows.Forms.DataObject();
				dataObject.SetData(data.GetType(),_DataObject);

				System.Windows.Forms.Clipboard.SetDataObject(dataObject,copy);

			}
		}
		/// <MetaDataID>{FFA6BA80-7DDA-45CB-81CA-13E642179545}</MetaDataID>
		public static object GetData(Type type)
		{
			object asasas=System.Windows.Forms.Clipboard.GetDataObject();
			bool asa=System.Windows.Forms.Clipboard.GetDataObject().GetDataPresent(type);
			object data= System.Windows.Forms.Clipboard.GetDataObject().GetData(type);
			if(!(data is DataObject))
			{
				return data;
			}
			else
			{
				if(ReferenceObjects.Contains((data as DataObject).RefrenceObject))
				{
					object ReferenceObject=ReferenceObjects[(data as DataObject).RefrenceObject];
					return ReferenceObject;

				}
				else
					return null;

			}
				
		}
	}
}
