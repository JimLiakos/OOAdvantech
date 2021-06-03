using System;
using OOAdvantech.Collections.Generic;

namespace OOAdvantech.PersistenceLayer
{
    /// <MetaDataID>{07703273-1DCE-4FBC-A55F-E9E0D8A061BD}</MetaDataID>
    /// <summary>ObjectCollection  manage the objects of AssociationEnd of a classes relation with multiplicity greater than 1. </summary>
	public interface ObjectCollection:OOAdvantech.Transactions.ITransactionalObject
	{
        #if !DeviceDotNet 
        /// <MetaDataID>{3e1e4a8e-1d5c-432d-ab8a-aee79de55e61}</MetaDataID>
        System.Linq.IQueryable QueryableCollection
        {
            get;
        }
#endif

#if DEBUG
		void CheckIndexes();
#endif

		/// <MetaDataID>{E9C3FC71-0909-47C0-B940-1DAE14D46C24}</MetaDataID>
		int Count
		{
			get;
		}
        /// <MetaDataID>{f4069b89-0863-4f87-abac-d747dba489c3}</MetaDataID>
        object this[int index] {get;}
	
		/// <MetaDataID>{467C2B47-E6B9-440B-B346-0DE595376907}</MetaDataID>
		/// <summary>Check if exist the object in collection, actually if there is link between the owner of the object collection and the checked object. </summary>
		bool Contains(object theObject);
		/// <MetaDataID>{4ED73F9A-E393-4A0E-BAF7-0CA27C250B05}</MetaDataID>
		/// <summary>Remove all objects.
		/// Actually you remove all the links between the owner of the object collection and the objects that contain the ObjectCollection. </summary>
		void RemoveAll();

		/// <MetaDataID>{9CDD9351-B4E4-46E5-8137-C117967CABFE}</MetaDataID>
		/// <summary>Adds objects massively. </summary>
		/// <param name="objects">
		/// This parameter defines the collection of objects which will be added.
		/// </param>
		void AddObjects(ObjectCollection objects);

        /// <summary>Removes objects massively. </summary>
        /// <param name="objects">
        /// This parameter defines the collection of objects which will be removed.
        /// </param>
        /// <MetaDataID>{19d94680-59d5-476f-9d06-b02a61370b95}</MetaDataID>
		void RemoveObjects(ObjectCollection objects);

        /// <MetaDataID>{5c0066eb-6b19-450a-98ba-129454a457fd}</MetaDataID>
        bool CanDeletePermanently(object theObject);
	
		/// <summary>Remove an object actually you remove the link between the owner of the object collection and the removed object. </summary>
		/// <MetaDataID>{E64388AC-23F4-460B-BB44-A0459F339255}</MetaDataID>
		/// <param name="theObject"></param>
		void Remove(object theObject);
		/// <summary>Add an object actually you make a link between the owner of the object collection and the added object. </summary>
		/// <MetaDataID>{38E8E480-970B-46BD-9E90-60A8EF212F9F}</MetaDataID>
		/// <param name="theObject"></param>
		void Add(object theObject);
		/// <summary>Return an enumerator to access the linked objects. </summary>
		/// <MetaDataID>{55702647-77A7-422D-BAF4-FF4586C5910B}</MetaDataID>
		System.Collections.IEnumerator GetEnumerator();

		/// <MetaDataID>{35378F45-6DC5-4CCA-8CC9-CA23514B2CD3}</MetaDataID>
		ObjectCollection GetObjects(string criterion);


        /// <MetaDataID>{4f38cabd-67fa-4ebe-98c0-788202048024}</MetaDataID>
        void Insert(int index, object item);
        /// <MetaDataID>{640014bd-06f1-4558-8201-f42346ea76a7}</MetaDataID>
        void RemoveAt(int index);
        /// <MetaDataID>{4ff7eaa8-65a0-4919-acd5-57f3885d3ed6}</MetaDataID>
        int IndexOf(object item);
        /// <MetaDataID>{f14491d5-6555-49a3-9e12-3907e9490798}</MetaDataID>
        int IndexOf(object item, int index);
        /// <MetaDataID>{edcec974-4a49-4c60-8cf0-947251c26b7b}</MetaDataID>
        int IndexOf(object item, int index, int count);
        List<object> ToThreadSafeList();

		System.Collections.ICollection ToThreadSafeSet(Type setType);
        
    }
}
