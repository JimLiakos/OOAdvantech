namespace OOAdvantech.Collections
{

	///


	/// <summary>This class defines a set of structure instance. 
	/// The structure of instances constructed dynamically from persistency system, 
	/// when you execute an OQL query. 
	/// You can iterate on set and you can get the values of instance properties.</summary>
	/// <example>
	/// <code lang="C#">
	/// 
	/// StorageSession storage=StorageSession.OpenStorage("Town","TownServer","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
	/// string OQLQuery="SELECT person.Name Name,person.Address.Street Street "+
	/// "FROM Person person "+
	/// "WHERE person.Parent.Address.City=person.City";
	/// StructureSet structureSet=storage.Execute(OQLQuery);
	/// foreach(StructureSet structureSetInstance in StructureSet)
	/// {
	/// 	string name=structureSetInstance["Name"] as string;
	/// 	string street=structureSetInstance["Street"] as string;
	/// 	System.Console.WriteLine("Name: "+name+", Street: "+street);
	/// }
	/// 
	/// structureSet.Close();
	/// </code>
	/// <code lang="Visual Basic">
	/// 
	/// Dim storage As StorageSession
	/// Dim structureSet As StructureSet
	/// Dim OQLQuery As String
	/// storage = StorageSession.OpenStorage("Town", "TownServer", "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider")
	/// OQLQuery = "SELECT person.Name Name,person.Address.Street Street "
	/// OQLQuery += "FROM Person person "
	/// OQLQuery += "WHERE person.Parent.Address.City=person.City"
	/// structureSet = storage.Execute(OQLQuery)
	/// For Each structureSetInstance As StructureSet In structureSet
	/// 	Dim name As String = structureSetInstance("Name")
	/// 	Dim street As String = structureSetInstance("Street")
	/// 	Console.WriteLine("Name: " + name + ", Street: " + street)
	/// Next
	/// structureSet.Close();
	/// </code>
	/// </example>
	/// <MetaDataID>{46247860-3DDF-4818-B18A-272083F2277D}</MetaDataID>
	public abstract class StructureSet: System.MarshalByRefObject, Remoting.IExtMarshalByRefObject
	{
		///<summary>
		///If you set the page size property then StructureSet 
		///automatically set the page count property with
		/// number of structure instances \ page size.
		///</summary>
		/// <MetaDataID>{D5A212D2-583E-4F4A-8034-53A681D56F6D}</MetaDataID>
		public abstract int PageCount
		{
			get;
 
		}
		///<summary>
		///Define the number of structure instances that contain a page.
		///</summary>
		/// <MetaDataID>{1AA0E50C-1BEA-4DC1-97FD-A68946B68DA6}</MetaDataID>
		public abstract int PageSize
		{
			get;
			set;
 
		}
		/// <MetaDataID>{42882CE0-CB79-4CD3-B260-D97835F505B0}</MetaDataID>
		public abstract void Close();




		/// <summary>Define the Structure instance type members.
		/// The StructureSet is a collection structure instances. 
		/// Structure instance has type and type has members. 
		/// Therefore members define the metadata of StructureSet. 
		/// The StructureSet operate as cursor you can set it at the start
		/// and you can call the MoveNext method through the end.
		///  At any time through the end you can get values of structure 
		/// instance through member class.</summary>
		/// <example>
		/// <code lang="C#">
		/// StorageSession storage=StorageSession.OpenStorage("Town","TownServer","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
		/// string OQLQuery="SELECT person.Name Name,person.Address.Street Street "+
		/// "FROM Person person "+
		/// "WHERE person.Parent.Address.City=person.City";
		/// StructureSet structureSet=storage.Execute(OQLQuery);
		/// foreach(StructureSet structureSetInstance in StructureSet)
		/// {
		/// 	string name=structureSetInstance.Members["Name"].Value as string;
		/// 	string street=structureSetInstance.Members["Street"].Value as string;
		/// 	System.Console.WriteLine("Name: "+name+", Street: "+street);
		/// }
		/// structureSet.Close();
		/// </code>
		/// <code lang="Visual Basic">
		/// 
		/// Dim storage As StorageSession
		/// Dim structureSet As StructureSet
		/// Dim OQLQuery As String
		/// storage = StorageSession.OpenStorage("Town", "TownServer", "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider")
		/// OQLQuery = "SELECT person.Name Name,person.Address.Street Street "
		/// OQLQuery += "FROM Person person "
		/// OQLQuery += "WHERE person.Parent.Address.City=person.City"
		/// structureSet = storage.Execute(OQLQuery)
		/// For Each structureSetInstance As StructureSet In structureSet
		/// 	Dim name As String = structureSetInstance.Members("Name").Value
		/// 	Dim street As String = structureSetInstance.Members("Street").Value
		/// 	Console.WriteLine("Name: " + name + ", Street: " + street)
		/// Next
		/// structureSet.Close();
		/// </code>
		/// </example>
		/// <MetaDataID>{C2965960-D4F8-4704-8A49-F2D56117176D}</MetaDataID>
		public MemberList Members;
		/// <summary></summary>
		/// <MetaDataID>{24F38B4F-577E-477F-A16E-C60284018746}</MetaDataID>
		public RowEnumerator GetEnumerator()
		{
			return new RowEnumerator(this);
		
		}
		/// <MetaDataID>{38E233C4-B89D-4251-94CF-B264DD270199}</MetaDataID>
		/// <summary>Define an indexer. 
		/// You give the member name and return the value of structure instance member.
		/// If you haven't call the MoveNext method or the MoveNext return false and call indexer then indexer raise exception.</summary>
		public object this[string Index ]
		{
			get
			{
				object mvalue=Members[Index].Value;
				return mvalue;
			}
		}
		/// <MetaDataID>{B8FBF12B-7E23-429A-9355-249CEC59CE82}</MetaDataID>
		/// <summary></summary>
		public class RowEnumerator:Remoting.IExtMarshalByRefObject
		{
			
			/// <summary></summary>
			/// <MetaDataID>{8192BDFA-2BAF-4ABC-81A1-8D4F77EE989F}</MetaDataID>
			public bool MoveNext()
			{
				return TheStructureSet.MoveNext();
			}

			/// <MetaDataID>{D0131618-67A0-4A0E-B5DC-A8D20AA024F7}</MetaDataID>
			/// <summary></summary>
			private StructureSet TheStructureSet;
			/// <summary></summary>
			/// <MetaDataID>{D076274C-EBE5-4807-87C6-474677A0A165}</MetaDataID>
			/// <param name="theStructureSet"></param>
			 public RowEnumerator(StructureSet theStructureSet) 
			{
				TheStructureSet=theStructureSet;
				 TheStructureSet.MoveFirst();
			}
			/// <MetaDataID>{11D07394-A257-4F56-849C-F3F1F49472FA}</MetaDataID>
			/// <summary></summary>
			public StructureSet Current 
			{
				get 
				{
					return TheStructureSet;
				}
			}
		}
	
		public enum StructSearchDirection {SearchForward = 1,SearchBackward = -1};

		/// <summary>Define the storage session that produce the StructureSet. </summary>
		/// <MetaDataID>{96EB809E-4CAA-4934-BAF4-884D21464DDB}</MetaDataID>
		public ObjectStorage SourceStorageSession;
	
	
		/// <summary></summary>
		/// <MetaDataID>{37D0451F-D35D-473B-BF70-9394294DA55C}</MetaDataID>
		public abstract void MoveFirst();
	
		/// <summary>The default position of the StructureSet is prior to the first record. Therefore, you must call MoveNext to begin accessing any data.
		/// Return Value true if there are more rows; otherwise, false</summary>
		/// <MetaDataID>{8B621B1B-DF94-4322-8620-34DE58D1AA76}</MetaDataID>
		public abstract bool MoveNext();

		///<summary>
		///The StructureSet operate as cursor you can set it at the page you want 
		///with MoveToPage method and then you can call the MoveNextPage method through the end.
		///</summary>
		/// <example>
		/// <code lang="C#">
		/// 
		///	StorageSession storage=StorageSession.OpenStorage("Town","TownServer","OOAdvantech.MSSQLPersistenceRunTime.StorageProvider");
		///	string OQLQuery="SELECT person.Name Name,person.Address.Street Street "+
		///	"FROM Person person "+
		///	"WHERE person.Parent.Address.City=person.City";
		///	StructureSet structureSet=storage.Execute(OQLQuery);
		///	int startPageNumber=0;
		///	structureSet.PageSize=100;
		///	while(structureSet.MoveNextPage())
		///	{
		///		while(structureSet.MoveNext)
		///		{
		///			string name=structureSet.Members["Name"].Value as string;
		///			string street=structureSet.Members["Street"].Value as string;
		///			System.Console.WriteLine("Name: "+name+", Street: "+street);
		///		}
		///	}
		///
		///	//Alternatively you can use StructureSet like this.
		///	if(structureSet.PageCount>10)
		///	{
		///		structureSet.MoveToPage(9); //zero base
		///		do
		///		{
		///			while(structureSet.MoveNext)
		///			{
		///				string name=structureSet.Members["Name"].Value as string;
		///				string street=structureSet.Members["Street"].Value as string;
		///				System.Console.WriteLine("Name: "+name+", Street: "+street);
		///			}
		///		}while(structureSet.MoveNextPage());
		///	}
		///	structureSet.Close();
		///	
		///	</code>
		/// <code lang="Visual Basic">
		/// 
		///	Dim storage As StorageSession
		///	Dim structureSet As StructureSet
		///	Dim OQLQuery As String
		/// 
		///	storage = StorageSession.OpenStorage("Town", "TownServer", "OOAdvantech.MSSQLPersistenceRunTime.StorageProvider")
		///	OQLQuery = "SELECT person.Name Name,person.Address.Street Street "
		///	OQLQuery += "FROM Person person "
		///	OQLQuery += "WHERE person.Parent.Address.City=person.City"
		///	
		///	structureSet = storage.Execute(OQLQuery)
		///	Dim startPageNumber As Integer = 0
		///	structureSet.PageSize = 100
		///	While structureSet.MoveNextPage()
		///		While structureSet.MoveNext
		///			Dim name As String = structureSet("Name")
		///			Dim street As String = structureSet("Street")
		///			Console.WriteLine("Name: " + name + ", Street: " + street)
		///		End While
		///	End While
		///
		///	'Alternatively you can use StructureSet like this.
		///	If structureSet.PageCount > 10 Then
		///		structureSet.MoveToPage(9)
		///		Do
		///			While structureSet.MoveNext
		///				Dim name As String = structureSet("Name")
		///				Dim street As String = structureSet("Street")
		///				Console.WriteLine("Name: " + name + ", Street: " + street)
		///			End While
		///		Loop While structureSet.MoveNextPage()
		///	End If
		///	</code>
		///	</example>
		/// <MetaDataID>{63183CA1-E9CE-4bf2-A986-29560DF0B5AE}</MetaDataID>
		public abstract void MoveToPage(int pageNumber);

		/// <MetaDataID>{1E3CB238-65AE-4a46-A727-B5C8BE44F74D}</MetaDataID>
		public abstract bool MoveNextPage();
	

		/// <MetaDataID>{05F64608-5A41-41aa-A849-C6FBFD7AD3D0}</MetaDataID>
		public abstract int PagingActivated
		{
			get;
			set;
 
		}

	
	}
}
