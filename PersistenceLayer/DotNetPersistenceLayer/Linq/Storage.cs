using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
#if PORTABLE 
using System.PCL.Reflection;
#endif

namespace OOAdvantech.Linq
{
    /// <MetaDataID>{e8633449-7b67-4bf5-8d3f-05452bd86b8e}</MetaDataID>
    public class Storage
    {
    
        /// <MetaDataID>{f4086e12-12d8-453a-a8c5-10a3677ac0ad}</MetaDataID>
        public OOAdvantech.PersistenceLayer.ObjectStorage ObjectStorage;
        /// <MetaDataID>{41e469bf-0eae-4241-b49b-41657634acee}</MetaDataID>
        public Storage(OOAdvantech.PersistenceLayer.ObjectStorage objectStorage)
        {
            if (objectStorage == null)
                throw new ArgumentNullException();

            ObjectStorage = objectStorage;
            Provider = new StorageProvider(this);

        }

        /// <MetaDataID>{1092e2b7-3b54-4792-8aa8-a76ed50c1c04}</MetaDataID>
        public IQueryable GetObjectCollection(Type tEntity  ) 
        {
            IQueryable collection = (IQueryable)Activator.CreateInstance(
                              typeof(ObjectCollection<>).MakeGenericType(
                                new[] { tEntity }),
                                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                                new object[] { this }, null);
            return collection;
        }

        /// <MetaDataID>{871a917a-2c7d-4002-ab9b-6b95dad4b813}</MetaDataID>
        public ObjectCollection<TEntity> GetObjectCollection<TEntity>() where TEntity : class
        {

            ObjectCollection<TEntity> collection = new ObjectCollection<TEntity>(this);
            //(ObjectCollection<TEntity>)Activator.CreateInstance(
            //                  typeof(ObjectCollection<>).MakeGenericType(
            //                    new[] { typeof(TEntity) }),
            //                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
            //                    new object[] { this }, null);
            return collection;
        }




        /// <MetaDataID>{8326aa8b-adf6-435c-9e1c-2d8fb8eac5d9}</MetaDataID>
        internal StorageProvider Provider;
    }

    /// <MetaDataID>{2db1e446-d850-444d-8ea9-31bc0fbbdf26}</MetaDataID>
    static public class DerivedMembersExpresionBuilder<TEntity> where TEntity : class
    {

        /// <MetaDataID>{43d44bee-69b1-4ac6-a501-73e9d02dcb80}</MetaDataID>
        public static ObjectCollection<TEntity> ObjectCollection 
        {
            get
            {
                return new ObjectCollection<TEntity>(default(Storage));
            }
        }
        /// <MetaDataID>{b0f584ee-f8e6-445c-a032-0abef95765a1}</MetaDataID>
        public static bool ErrorCheck(System.Reflection.PropertyInfo propertyInfo, System.Collections.Generic.List<string> derivedMemberErrors)
        {

            return false;
            //LINQStorageObjectQuery LINQStorageObjectQuery = new LINQStorageObjectQuery((propertyInfo.GetCustomAttributes(typeof(OOAdvantech.MetaDataRepository.DerivedMember), false)[0] as OOAdvantech.MetaDataRepository.DerivedMember).Expression);


            //OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dervideMemberDataNodeRoot = (LINQStorageObjectQuery.QueryResult as IDynamicTypeDataRetrieve).RootDataNode;
            ////OOAdvantech.MetaDataRepository.ObjectQueryLanguage.DataNode dervideMemberDataNode = dervideMemberDataNodeRoot.ObjectQuery.SelectListItems[0];


            //if (!OOAdvantech.MetaDataRepository.Classifier.GetClassifier(propertyInfo.DeclaringType).IsA(dervideMemberDataNodeRoot.Classifier) &&
            //                  dervideMemberDataNodeRoot.Classifier != OOAdvantech.MetaDataRepository.Classifier.GetClassifier(propertyInfo.DeclaringType))
            //{
            //    //System.Windows.Forms.MessageBox.Show("Derived member query result type mismatch");
            //    derivedMemberErrors.Add("Derived member query source type mismatch.");
            //}

            //if (!MetaDataRepository.Classifier.GetClassifier(TypeHelper.GetElementType(propertyInfo.PropertyType)).IsA(MetaDataRepository.Classifier.GetClassifier(LINQStorageObjectQuery.QueryResult.Type)) &&
            //                MetaDataRepository.Classifier.GetClassifier(LINQStorageObjectQuery.QueryResult.Type) != OOAdvantech.MetaDataRepository.Classifier.GetClassifier(TypeHelper.GetElementType(propertyInfo.PropertyType)))
            //{
            //    //System.Windows.Forms.MessageBox.Show("Derived member query result type mismatch");
            //    derivedMemberErrors.Add("Derived member query source type mismatch.");
            //}


            //if (derivedMemberErrors.Count > 0)
            //    return true;
            //else
            //    return false;
        }

       
        

    }
}
