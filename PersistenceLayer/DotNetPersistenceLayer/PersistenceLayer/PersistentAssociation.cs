namespace OOAdvantech.PersistenceLayer
{
	using System;
	/// <MetaDataID>{17029338-6D08-4058-AFF6-E9E5366DCEB1}</MetaDataID>
	/// <summary></summary>
	 [System.AttributeUsage(System.AttributeTargets.Property|System.AttributeTargets.Field)]
	public class PersistentAssociationa : Attribute 
	{
		 
		 /// <MetaDataID>{3FE38E73-F570-477B-80DC-1F5D609EAE94}</MetaDataID>
		 /// <summary></summary>
		private long DestClassObjectsLowLimit;
		 /// <MetaDataID>{49F81DFD-C99B-4C06-9DA2-5AC1CAE0B9C5}</MetaDataID>
		 /// <summary></summary>
		private long DestClassObjectsHightLimit;
		 /// <MetaDataID>{3433C218-7D7F-4B7A-AC5D-FB225937BD99}</MetaDataID>
		 /// <summary></summary>
		private long RootClassObjectsLowLimit;
		 /// <MetaDataID>{8B4857A9-4A16-4C71-BDB9-0E5761FF6673}</MetaDataID>
		 /// <summary></summary>
		private long RootClassObjectsHightLimit;


		 /// <summary></summary>
		 /// <MetaDataID>{4FE66733-E8E0-4DC9-810B-1841C431557B}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		public void GetMultiplicity(ref long RootLowLimit, ref long RootHightLimit, ref long DestLowLimit, ref long DestHightLimit)
		{
			RootLowLimit=RootClassObjectsLowLimit;
			RootHightLimit=RootClassObjectsHightLimit;
			DestLowLimit=DestClassObjectsLowLimit;
			DestHightLimit=DestClassObjectsHightLimit;
		}
	 

		/// <summary></summary>
		/// <MetaDataID>{26685A6B-C838-4F0F-8FBF-6CC6E1F2F066}</MetaDataID>
		public PersistenceLayer.PersistencyFlag RelationshipFlag;
		/// <summary></summary>
		/// <MetaDataID>{CFB202BA-93DA-4546-9157-7AC94A83CC90}</MetaDataID>
		/// <param name="DestHightLimit"></param>
		/// <param name="DestLowLimit"></param>
		/// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role,uint DestLowLimit, uint DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(0,0,DestLowLimit,DestHightLimit);
		 }
		/// <summary></summary>
		/// <MetaDataID>{3AC3449B-61B8-476B-8983-2BA17F5CA3FB}</MetaDataID>
		/// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role)
		 {

			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(0,0,0,0);
	 
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{92A21DBC-4F5C-451B-B3F2-A8AD025FF10D}</MetaDataID>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(0,0,0,0);
	 
		 }

		 /// <MetaDataID>{F7759D33-E69F-4AF5-AC59-ABCE03DCBF16}</MetaDataID>
		 /// <summary></summary>
		public string AssociationName;

		/// <MetaDataID>{3ADC9CBE-F3FD-46D5-A159-006ADB778231}</MetaDataID>
		public System.Type OtherEndType;
		/// <MetaDataID>{4B0D585B-D7DB-4A42-BACC-5F54465AD24D}</MetaDataID>
		string ImplementationFieldName;
		 /// <MetaDataID>{15DF0767-261A-4CB3-A379-0E56061A7E95}</MetaDataID>
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


		 //System.Reflection.FieldInfo ImplementationField=null;
		 /// <MetaDataID>{0E7D492A-C07D-4386-85BA-A3A9289F5431}</MetaDataID>
		 public System.Reflection.FieldInfo GetImplementationField(System.Reflection.PropertyInfo propertyInfo)
		 {
			 if(propertyInfo==null)
				 throw new System.ArgumentException("The parameter 'propertyInfo' must be not null");

			 System.Reflection.FieldInfo fieldInfo=FindField(propertyInfo.DeclaringType,ImplementationFieldName);
			 if(fieldInfo==null)
				 throw new System.Exception("can't find implementation member \"" +ImplementationFieldName+"\" of association \""+AssociationName+"\" in class \""+propertyInfo.DeclaringType.FullName+"\"");
			 return fieldInfo;
		 }

		/// <MetaDataID>{AD11F4C0-4F91-46E0-A4E1-3525BEB8586E}</MetaDataID>
 		public bool IsRoleA;


		 /// <summary></summary>
		 /// <MetaDataID>{9C96B104-3940-4776-9C55-7A435DBEBAAA}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, uint RootLowLimit, uint RootHightLimit, uint DestLowLimit, uint DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
                 IsRoleA=true;
			 else
                 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(RootLowLimit,RootHightLimit,DestLowLimit,DestHightLimit);
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{D1368D56-27E6-4732-8017-4F16CE854C43}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, uint RootLowLimit, Multiplicity RootHightLimit, uint DestLowLimit, Multiplicity DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(RootLowLimit,-1,DestLowLimit,-1);
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{3A67AE73-EE5F-4FD8-A2CC-12C6EB1F2608}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, uint RootLowLimit, Multiplicity RootHightLimit, uint DestLowLimit, uint DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(RootLowLimit,-1,DestLowLimit,DestHightLimit);
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{FB2876E6-F749-4B9B-AAD2-9AEBE2D9A814}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, uint RootLowLimit, uint RootHightLimit, uint DestLowLimit, Multiplicity DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(RootLowLimit,RootHightLimit,DestLowLimit,-1);
		 }

		 /// <summary></summary>
		 /// <MetaDataID>{24E2B054-8BCD-4E11-9CB9-01DD54E3DCB5}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag, uint RootLowLimit, uint RootHightLimit, uint DestLowLimit, uint DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(RootLowLimit,RootHightLimit,DestLowLimit,DestHightLimit);
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{5F3A52B7-C68C-4296-AFA5-8EBC02F2B59E}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag, uint RootLowLimit, Multiplicity RootHightLimit, uint DestLowLimit, Multiplicity DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(RootLowLimit,-1,DestLowLimit,-1);
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{A8DC048B-AF07-4B41-B6F0-08BAD661A58B}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag, uint RootLowLimit, Multiplicity RootHightLimit, uint DestLowLimit, uint DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(RootLowLimit,-1,DestLowLimit,DestHightLimit);
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{F8FB8C7A-4D5F-424A-AD99-E6CD1628CB4D}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag, uint RootLowLimit, uint RootHightLimit, uint DestLowLimit, Multiplicity DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(RootLowLimit,RootHightLimit,DestLowLimit,-1);
		 }

		 /// <summary></summary>
		 /// <MetaDataID>{A9464157-1440-4B62-B006-4E5CA6A20C41}</MetaDataID>
		 /// <param name="DestValue"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag, uint DestLowLimit, Multiplicity DestValue)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(0,0,DestLowLimit,-1);
		 }
			 /// <summary></summary>
			 /// <MetaDataID>{0202473F-936F-4277-B6A2-5FA720ACD113}</MetaDataID>
			 /// <param name="DestValue"></param>
			 /// <param name="DestLowLimit"></param>
			 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, uint DestLowLimit, Multiplicity DestValue)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(PersistenceLayer.PersistencyFlag.LazyFetching);
			 SetMultiplicity(0,0,DestLowLimit,-1);

		 }

		 /// <summary></summary>
		 /// <MetaDataID>{34B7664C-B330-4D09-B466-5FE74B1A5FA8}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="mPersistencyFlag"></param>
		 /// <param name="theAssociationName"></param>
		 public PersistentAssociationa(string theAssociationName,System.Type theOtherEndType,string theImplMemberName,AssotiationEnd role, PersistencyFlag mPersistencyFlag, uint DestLowLimit, uint DestHightLimit)
		 {
			 ImplementationFieldName=theImplMemberName;
			 if(role==AssotiationEnd.RoleA)
				 IsRoleA=true;
			 else
				 IsRoleA=false;
			 AssociationName=theAssociationName;
			 OtherEndType=theOtherEndType;
			 SetPersistencyFlag(mPersistencyFlag);
			 SetMultiplicity(0,0,DestLowLimit,DestHightLimit);
			 int lo=0;
		 }
		 /// <summary></summary>
		 /// <MetaDataID>{C82C1453-BC7E-4F54-A300-DCF493404448}</MetaDataID>
		 /// <param name="thePersistencyFlag"></param>
		private void SetPersistencyFlag(PersistencyFlag thePersistencyFlag)
		 {
			 uint flag=(uint)(thePersistencyFlag&PersistenceLayer.PersistencyFlag.OnConstruction);
			 if(flag==0)
				 RelationshipFlag=PersistenceLayer.PersistencyFlag.LazyFetching;
			 else
				 RelationshipFlag=PersistenceLayer.PersistencyFlag.OnConstruction;
/*			 flag=(uint)(thePersistencyFlag&PersistenceLayer.PersistencyFlag.PropagateDelete);
			 if(flag!=0)
				 RelationshipFlag|=PersistenceLayer.PersistencyFlag.PropagateDelete;
*/			 
			 flag=(uint)(thePersistencyFlag&PersistenceLayer.PersistencyFlag.ReferentialIntegrity);
			 if(flag!=0)
				 RelationshipFlag|=PersistenceLayer.PersistencyFlag.ReferentialIntegrity;

			 flag=(uint)(thePersistencyFlag&PersistenceLayer.PersistencyFlag.CascadeDelete);
			 if(flag!=0)
				 RelationshipFlag|=PersistenceLayer.PersistencyFlag.CascadeDelete;

		 }
		 /// <summary></summary>
		 /// <MetaDataID>{E3F549AC-DB05-4732-AAA6-04FC4A38B173}</MetaDataID>
		 /// <param name="DestHightLimit"></param>
		 /// <param name="DestLowLimit"></param>
		 /// <param name="RootHightLimit"></param>
		 /// <param name="RootLowLimit"></param>
		private void SetMultiplicity(long RootLowLimit, long RootHightLimit, long DestLowLimit, long DestHightLimit)
		 {
			 RootClassObjectsLowLimit=RootLowLimit;
			 RootClassObjectsHightLimit=RootHightLimit;
			 DestClassObjectsLowLimit=DestLowLimit;
			 DestClassObjectsHightLimit=DestHightLimit;
		 }
	}
}
