namespace OOAdvantech.PersistenceLayer
{
	
	
	/// <MetaDataID>{29F2475F-224E-4F6F-9BCC-A9F88E71A1C1}</MetaDataID>
	[System.AttributeUsage(System.AttributeTargets.Property)]
	public class PersistentPropertya:System.Attribute
	{
		/// <MetaDataID>{AE18DA39-3F79-41EC-A3D6-182C940B1DE7}</MetaDataID>
		public int Length=0;

		/// <MetaDataID>{DDDF64AC-121A-478C-AE62-1E1FCC429024}</MetaDataID>
		public PersistentPropertya()
		{
		}
		/// <MetaDataID>{7F4BE23C-F91C-40F5-8A37-CC16131EF7E4}</MetaDataID>
		string ImplementationFieldName;
		/// <MetaDataID>{17E24ED6-4E33-4529-9D06-F31E7A7FA9EF}</MetaDataID>
		public PersistenceLayer.PersistencyFlag FieldFlag=PersistenceLayer.PersistencyFlag.OnConstruction;
		/// <MetaDataID>{70244087-1813-4F80-82B8-B4288D862DCD}</MetaDataID>
		public PersistentPropertya(string theImplentationField)
		{
			ImplementationFieldName=theImplentationField;
		}
		/// <MetaDataID>{69ACFD27-94C6-4A77-80BF-0A29F6C71E71}</MetaDataID>
		public PersistentPropertya(PersistencyFlag mPersistencyFlag, string theImplentationField)
		{
			FieldFlag=mPersistencyFlag;
			ImplementationFieldName=theImplentationField;
		}

		/// <MetaDataID>{52ADE6B8-AEA7-4AAC-B187-6C3FC21B0F95}</MetaDataID>
		public PersistentPropertya(int theLength , string theImplentationField)
		{
			Length=theLength;
			FieldFlag=PersistenceLayer.PersistencyFlag.OnConstruction;
			ImplementationFieldName=theImplentationField;
		}
		/// <MetaDataID>{7C126270-652A-4425-9E47-3BB49DD4EE5B}</MetaDataID>
		public PersistentPropertya(int theLength)
		{
			Length=theLength;
		}
		/// <MetaDataID>{69ACFD27-94C6-4A77-80BF-0A29F6C71E71}</MetaDataID>
		public PersistentPropertya(int theLength , PersistencyFlag mPersistencyFlag, string theImplentationField)
		{
			Length=theLength;
			FieldFlag=mPersistencyFlag;
			ImplementationFieldName=theImplentationField;
		}
		/// <MetaDataID>{8854C72C-B864-4C27-8835-5CB6F1CCD209}</MetaDataID>
		public System.Reflection.FieldInfo GetImplementationField(System.Reflection.PropertyInfo propertyInfo)
		{
			if(propertyInfo==null)
				throw new System.ArgumentException("The parameter 'propertyInfo' must be not null");
			System.Reflection.FieldInfo fieldInfo=FindField(propertyInfo.DeclaringType,ImplementationFieldName);
			if(fieldInfo==null)
				throw new System.Exception("can't find implementation member \"" +ImplementationFieldName+"\" of Property \""+propertyInfo.DeclaringType.FullName+"."+propertyInfo.Name);
			return fieldInfo;
		}
		/// <MetaDataID>{BC714483-5234-494B-AA0C-845802CD112E}</MetaDataID>
		private System.Reflection.FieldInfo FindField(System.Type ObjectType,string FieldName)
		{
			System.Reflection.FieldInfo Field=ObjectType.GetField(FieldName,System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
			if(Field!=null)
				return Field;
			else
			{
				if(ObjectType.BaseType!=null)
					return FindField(ObjectType.BaseType,FieldName);
			}
			return null;
		}



	}
}
