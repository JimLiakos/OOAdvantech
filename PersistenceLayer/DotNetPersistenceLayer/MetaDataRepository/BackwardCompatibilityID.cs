using System;

namespace OOAdvantech.MetaDataRepository
{
	/// <summary>With this attribute class you can define your identity for
	/// a Meta object. For example the default identity of class is
	/// "assembly strong name" "dot" "class full name". Is it useful?
	/// In case where the class is persistent and there are objects 
	/// at object storage place, if you change the name of class or 
	/// namespace you have lose the link between the class and 
	/// persistent objects. If you define identity of class for example 
	/// a GUID string then there isn't problem. </summary>
	/// <MetaDataID>{BCDC72EA-3932-409D-B08F-EEDDFF5E98E1}</MetaDataID>
	[System.AttributeUsage(System.AttributeTargets.All)]
	public class BackwardCompatibilityID : System.Attribute
	{ 
		/// <MetaDataID>{A20C5A09-1B7C-49DD-A680-148AD5AF6240}</MetaDataID>
		public BackwardCompatibilityID(string id)
		{ 
			ID=id; 
		}
		/// <MetaDataID>{54784A2A-D66A-4C78-8E9D-9B3791C5E6CC}</MetaDataID>
		/// <exclude>Excluded</exclude>
		private string ID;
		/// <MetaDataID>{328F813D-7862-408E-B503-BE47FC379296}</MetaDataID>
		public override string ToString()
		{
			return ID;
		}
	
	}
}
