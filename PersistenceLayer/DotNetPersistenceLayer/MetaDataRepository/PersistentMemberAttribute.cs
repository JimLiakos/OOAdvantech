using System;
#if PORTABLE
using System.PCL.Reflection;
#else
using System.Reflection;
#endif

namespace OOAdvantech.MetaDataRepository
{

    /// <MetaDataID>{9b887968-64d1-4ac3-a320-c0aebce3367b}</MetaDataID>
	public enum PersistencyFlag
	{
		Default=0,
		/// <summary>
		///The persistent fields or linked objects are loaded,
		///at the time where the object goes from the passive 
		///to operative mode.
		/// </summary>
		OnConstruction=1,
		/// <summary>
		///The persistent fields or linked objects aren’t loaded, at the time where the 
		///object goes from the passive to operative mode, but at the time there are needed.
		/// </summary>
		LazyFetching=2,
		/// <summary>
		///System can’t delete permanently an object of destination Class 
		///if it has link with the object of root class. Root class is the class 
		///that contain the field and destination class is the class (type) of field.
		/// </summary>
		ReferentialIntegrity=4,
		/// <summary>
		///If we decide to delete a link with the object of the field, 
		///system will try to delete the object of the field.
		/// </summary>
		CascadeDelete=8,

        /// <summary>
        /// Allow objects link where the first is presistent and second is transient under presistent association
        /// </summary>
        AllowTransient=16,

        /// <summary>
        /// Allow  AssociationEndRealization to override behavior of Association end
        /// </summary>
        Override = 32


    };

	/// <summary>
	/// </summary>
	/// <MetaDataID>{FA13B032-5BB0-48B4-85A9-E3FC35C1FBDC}</MetaDataID>
	[System.AttributeUsage(System.AttributeTargets.Property|System.AttributeTargets.Field)]
	public class PersistentMember : System.Attribute
	{
		/// <MetaDataID>{01E37E91-DEDF-478D-A8BB-7ABF9B57B846}</MetaDataID>
		public PersistentMember()
		{
		}

		/// <MetaDataID>{B394825E-ACE3-477B-B3FC-5EC5893436D4}</MetaDataID>
		public int Length=0;

		/// <MetaDataID>{C166AA89-47B4-4B9F-9B7C-8816162F6488}</MetaDataID>
		public string ImplementationFieldName;
		/// <MetaDataID>{5515F532-40F7-4871-91A4-FA3EE7AAF70A}</MetaDataID>
		public PersistencyFlag PersistencyFlag;//=PersistencyFlag.OnConstruction;
		/// <MetaDataID>{1C33C3F9-795D-4265-8C7E-EE54B961E06C}</MetaDataID>
		 public PersistentMember(string theImplentationField)
		{
			ImplementationFieldName=theImplentationField;
            if (!string.IsNullOrEmpty(ImplementationFieldName))
                ImplementationFieldName = ImplementationFieldName.Trim();
		}
		/// <MetaDataID>{D58B33F3-6EFE-49B1-ABD3-F9347004E371}</MetaDataID>
		 public PersistentMember(PersistencyFlag mPersistencyFlag)
		{
			PersistencyFlag=mPersistencyFlag;
		}
		/// <MetaDataID>{D8A214C9-FBBB-41C8-B21D-0D28F8AC3D5F}</MetaDataID>
		 public PersistentMember(PersistencyFlag mPersistencyFlag, string theImplentationField)
		{
			PersistencyFlag=mPersistencyFlag;
			ImplementationFieldName=theImplentationField;
            if (!string.IsNullOrEmpty(ImplementationFieldName))
                ImplementationFieldName = ImplementationFieldName.Trim();

		}

		/// <MetaDataID>{63039678-2E49-4149-9074-C87F0594D8FE}</MetaDataID>
		 public PersistentMember(int theLength, string theImplentationField)
		{
			Length=theLength;
			PersistencyFlag=PersistencyFlag.OnConstruction;
			ImplementationFieldName=theImplentationField;
            if (!string.IsNullOrEmpty(ImplementationFieldName))
                ImplementationFieldName = ImplementationFieldName.Trim();

		}
		/// <MetaDataID>{3E9FE776-8261-400B-8E71-8AEBDB902B44}</MetaDataID>
		 public PersistentMember(int theLength)
		{
			Length=theLength;
		}
		/// <MetaDataID>{A23AEBBC-D62C-4B1C-8CBC-73C160ED9116}</MetaDataID>
		 public PersistentMember(int theLength, PersistencyFlag mPersistencyFlag, string theImplentationField)
		{
			Length=theLength;
			PersistencyFlag=mPersistencyFlag;
			ImplementationFieldName=theImplentationField;
            if (!string.IsNullOrEmpty(ImplementationFieldName))
                ImplementationFieldName = ImplementationFieldName.Trim();

		}


         /// <MetaDataID>{B0D407F2-C2D6-4220-8641-893605E058EF}</MetaDataID>
         internal  System.Reflection.FieldInfo GetImplementationField(System.Reflection.PropertyInfo propertyInfo)
         {
             if (ImplementationFieldName == null)
                 ImplementationFieldName = "";
             if (propertyInfo == null)
                 throw new System.ArgumentNullException("The parameter 'propertyInfo' must be not null");
             System.Reflection.FieldInfo fieldInfo = FindField(propertyInfo.DeclaringType, ImplementationFieldName);
             if (fieldInfo == null)
                 throw new System.Exception("System can't find implementation member \"" + ImplementationFieldName + "\" of Property \"" + propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name);
             return fieldInfo;
         }
		/// <MetaDataID>{AFFE73BF-D8D1-49FF-AC53-66BBB9910CA8}</MetaDataID>
		private System.Reflection.FieldInfo FindField(System.Type ObjectType,string FieldName)
		{
			System.Reflection.FieldInfo Field=ObjectType.GetMetaData().GetField(FieldName,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);
			if(Field!=null)
				return Field;
			else
			{
				if(ObjectType.GetMetaData().BaseType!=null)
					return FindField(ObjectType.GetMetaData().BaseType,FieldName);
			}
			return null;
		}
	}




    /// <summary>
    /// </summary>
    /// <MetaDataID>{FA13B032-5BB0-48B4-85A9-E3FC35C1FBDC}</MetaDataID>
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
    public class ImplementationMember : System.Attribute
    {
         
        /// <MetaDataID>{C166AA89-47B4-4B9F-9B7C-8816162F6488}</MetaDataID>
        public string ImplementationFieldName;
     
        /// <MetaDataID>{1C33C3F9-795D-4265-8C7E-EE54B961E06C}</MetaDataID>
        public ImplementationMember(string theImplentationField)
        {
            ImplementationFieldName = theImplentationField;
            if (!string.IsNullOrEmpty(ImplementationFieldName))
                ImplementationFieldName = ImplementationFieldName.Trim();
        }

       
     
        /// <MetaDataID>{B0D407F2-C2D6-4220-8641-893605E058EF}</MetaDataID>
        internal System.Reflection.FieldInfo GetImplementationField(System.Reflection.PropertyInfo propertyInfo)
        {
            if (ImplementationFieldName == null)
                ImplementationFieldName = "";
            if (propertyInfo == null)
                throw new System.ArgumentNullException("The parameter 'propertyInfo' must be not null");
            System.Reflection.FieldInfo fieldInfo = FindField(propertyInfo.DeclaringType, ImplementationFieldName);
            if (fieldInfo == null)
                throw new System.Exception("System can't find implementation member \"" + ImplementationFieldName + "\" of Property \"" + propertyInfo.DeclaringType.FullName + "." + propertyInfo.Name);
            return fieldInfo;
        }
        /// <MetaDataID>{AFFE73BF-D8D1-49FF-AC53-66BBB9910CA8}</MetaDataID>
        private System.Reflection.FieldInfo FindField(System.Type ObjectType, string FieldName)
        {
            System.Reflection.FieldInfo Field = ObjectType.GetMetaData().GetField(FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (Field != null)
                return Field;
            else
            {
                if (ObjectType.GetMetaData().BaseType != null)
                    return FindField(ObjectType.GetMetaData().BaseType, FieldName);
            }
            return null;
        }
    }
}
