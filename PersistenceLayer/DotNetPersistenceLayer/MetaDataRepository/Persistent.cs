using System;
using System.Collections.Generic;

namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{e258c5bd-32eb-4bd6-b7e4-31e6228be08a}</MetaDataID>
	public enum PersistencyType
	{
		NormaClass =0,
		historyClass =1
	}

	//Error prone αν αλλάξει το FullName στην method PersistenceLayer.StorageInstanceRef.GetStorageInstanceRef(object persistentObject)
	/// <summary>
	/// </summary>
	/// <MetaDataID>{B546D772-70CF-4567-8C61-579C9D308C0F}</MetaDataID>
	[AttributeUsage(AttributeTargets.All)]
	public class Persistent : OOAdvantech.Transactions.TransactionalAttribute
	{
		
		/// <MetaDataID>{698745C1-C6F9-4A7A-B35F-AE957D857300}</MetaDataID>
		public PersistencyType PersistencyType;

		/// <MetaDataID>{CE924C63-2527-445C-87C1-B0E0F45918F4}</MetaDataID>
		public int NumberOfObject;
		
		/// <MetaDataID>{2B8F4DAF-7E85-4B5C-A41A-AFB79AB50109}</MetaDataID>
		public Persistent()
		{
		}

		/// <MetaDataID>{4EEBC3C0-AAAE-45EC-98D7-BFA67F39997D}</MetaDataID>
		public string ExtMetaData=null;
		/// <MetaDataID>{710C6CFE-CCFF-4F75-9CD6-23AE699378B0}</MetaDataID>
		private PersistencyFlag MemberPersistencyFlag;

		/// <MetaDataID>{2DF690E1-B29A-4428-9E8A-EF20094C6305}</MetaDataID>
		public Persistent(string Settings)
		{
			ExtMetaData=Settings;
		}

		/// <MetaDataID>{19194105-DFAC-44FF-AF9E-4A46517D2A98}</MetaDataID>
		public Persistent(PersistencyType persistencyType,int numberOfObject)
		{
			PersistencyType=persistencyType;
			NumberOfObject=numberOfObject;
			ExtMetaData=null;
			//MemberPersistencyFlag=PersistencyFlag.AtConstruction;
			
		}
		/// <MetaDataID>{2D1EB711-DB59-4A7A-A5FB-2DB5A024286E}</MetaDataID>
		public Persistent(string Settings,PersistencyType persistencyType,int numberOfObject)
		{
			PersistencyType=persistencyType;
			NumberOfObject=numberOfObject;
			ExtMetaData=Settings;
		}
	}

    /// <MetaDataID>{1e341ed5-acc5-4df4-b059-e1a447353fac}</MetaDataID>
    public interface IDerivedMemberExpression
    {
        /// <MetaDataID>{70cf18f4-ac26-4704-bc1a-22b0209b9693}</MetaDataID>
        System.Linq.IQueryable QueryableCollection
        {
            get;
        }


    }
    /// <MetaDataID>{8c43a4af-808c-4d45-9b82-0742fdebb306}</MetaDataID>
    public interface IDerivedMemberExpression<TSource, TResult> : IDerivedMemberExpression
    {

        /// <MetaDataID>{fba6e949-4af4-4ba0-80e9-866bf4bab1fc}</MetaDataID>
        IEnumerable<TResult> GetCollection(TSource _object);
        /// <MetaDataID>{025695f5-cfd0-4ac3-8c74-d0e4513ca478}</MetaDataID>
        TResult GetValue(TSource _object);
    }

    /// <MetaDataID>{30a896af-5e3c-49bf-a0b5-a911cd1c2c05}</MetaDataID>
    [AttributeUsage(AttributeTargets.Property)]
    public class DerivedMember : System.Attribute
    {
        Type DerivedMemberExpressionType;
        IDerivedMemberExpression DerivedMemberExpression;
        
        
        /// <MetaDataID>{fd44b35b-085a-4a18-9589-b871933ea623}</MetaDataID>
        public System.Linq.Expressions.Expression Expression
        {
            get
            {
                if (DerivedMemberExpression == null)
                    DerivedMemberExpression = System.Activator.CreateInstance(DerivedMemberExpressionType) as IDerivedMemberExpression;
                return DerivedMemberExpression.QueryableCollection.Expression;

            }
        }

        /// <MetaDataID>{0a95f14b-6da0-423f-b82a-7e275732a7d6}</MetaDataID>
        public DerivedMember(Type derivedMemberExpressionType)
        {
            DerivedMemberExpressionType = derivedMemberExpressionType;
            //System.Reflection.PropertyInfo propertyInfo = query.GetProperty("QueryExpression", BindingFlags.Static | BindingFlags.Public);
            //if(propertyInfo!=null)
            //    Expression = (propertyInfo.GetValue(null, null) as System.Linq.IQueryable).Expression;
        }
#if !DeviceDotNet
#endif
    }
}
