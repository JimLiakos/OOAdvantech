using System.Linq;
namespace OOAdvantech.MetaDataRepository
{
    /// <MetaDataID>{5883F983-F36E-419B-A5C8-54B58FDA87F1}</MetaDataID>
    public class ContainedItemsSynchronizer
    {
        /// <MetaDataID>{33505316-F0E8-44FA-81D0-4AD9BBEF51EC}</MetaDataID>
        private MetaObject PlaceIdentifier;
        public void GetAddedObject()
        {
            foreach (AddCommand mAddCommand in AddedObjectsCommands)
            {
                if (mAddCommand.AddedObject == null)
                    mAddCommand.AddedObject = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(mAddCommand.MissingMetaObject, PlaceIdentifier);
            }
        }
        /// <MetaDataID>{6B4A27F1-02FA-4A91-B31E-F02E380AB404}</MetaDataID>
        public void ExecuteAddCommand()
        {

            foreach (AddCommand mAddCommand in AddedObjectsCommands)
            {
                if (mAddCommand.AddedObject == null)
                    mAddCommand.AddedObject = MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(mAddCommand.MissingMetaObject, PlaceIdentifier);
                if (mAddCommand.AddedObject != null && GetMetaDataRepositoryClass(mAddCommand.AddedObject) != GetMetaDataRepositoryClass(mAddCommand.MissingMetaObject))
                    mAddCommand.AddedObject = null;
                if (mAddCommand.AddedObject == null)
                    mAddCommand.AddedObject = MetaObjectsStack.CurrentMetaObjectCreator.CreateMetaObjectInPlace(mAddCommand.MissingMetaObject, PlaceIdentifier);
                if (mAddCommand.AddedObject != null)
                    Updated.Add(mAddCommand.AddedObject);
            }

        }
        /// <MetaDataID>{BA31C2E5-DDB5-4169-A513-A486965633B0}</MetaDataID>
        public void ExecuteDeleteCommand()
        {
            if (MetaObjectsStack.CurrentMetaObjectCreator.SuspendSychronizationDelete)
                return;
            foreach (DeleteCommand mDeleteCommand in DeletedObjectsCommands)
            {

                Updated.Remove(mDeleteCommand.CandidateForDeleteObject);
                int lo = 0;
            }


        }
        /// <summary>Synchronize MetaObject. This means that the MetaObject, after the synchronization is equivalent with the OriginMetaObject. Synchronize is the main operation for Metadata repositories syncrhonization.</summary>
        /// <MetaDataID>{61F10400-59A5-4BED-A397-064CDFD15C6F}</MetaDataID>
        public void Synchronize()
        {

            foreach (MetaObject CurrMetaObject in Source)
            {
                foreach (MetaObject CurrUpdateMetaObject in Updated)
                    if (MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrUpdateMetaObject) == MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrMetaObject))
                    {
                        try
                        {
                            CurrUpdateMetaObject.Synchronize(CurrMetaObject);
                        }
                        catch (System.Exception error)
                        {
                            if(Transactions.Transaction.Current!=null&&Transactions.Transaction.Current.Status==Transactions.TransactionStatus.Aborted)
                            {
                                Transactions.Transaction.Current.Abort(error);
                                throw;
                            }
                        }
                        break;
                    }
            }
        }
        /// <MetaDataID>{B08DC534-C5D1-4C51-8E8D-D9E2CE0129FA}</MetaDataID>
        public System.Collections.Generic.List<AddCommand> AddedObjectsCommands;
        /// <MetaDataID>{59D8E04A-7BF2-4180-A3EA-5072DBD1237B}</MetaDataID>
        public System.Collections.Generic.List<DeleteCommand> DeletedObjectsCommands;
        /// <MetaDataID>{2B94ECB1-4603-4DF9-A100-EE99FF17F2E1}</MetaDataID>
        internal protected ContainedItemsSynchronizer(System.Collections.IList theSource, System.Collections.IList theUpdated, MetaDataRepository.MetaObject placeIdentifier)
        {
            AddedObjectsCommands = new System.Collections.Generic.List<AddCommand>();
            DeletedObjectsCommands = new System.Collections.Generic.List<DeleteCommand>();
            Source = theSource;
            Updated = theUpdated;
            PlaceIdentifier = placeIdentifier;
        }
        /// <MetaDataID>{DF5EB3AF-2C33-497B-977A-F00643EE5B05}</MetaDataID>
        public MetaObjectsStack theMetaObjectsStack;
        /// <MetaDataID>{E83B1DD4-3D32-4B71-8C21-9B32792B3A60}</MetaDataID>
        public System.Collections.IList Source;

        /// <MetaDataID>{04F7C472-39BB-486C-8D1D-88423E33942D}</MetaDataID>
        public System.Collections.IList Updated;


        /// <MetaDataID>{7B5B2E77-403F-427B-AE35-6EF571442D45}</MetaDataID>
        public virtual void FindModifications()
        {
            foreach (MetaObject CurrMetaObject in Source)
            {

                MetaObject NewMetaObject = CurrMetaObject;
                foreach (MetaObject CurrUpdateMetaObject in Updated)
                    if (MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrUpdateMetaObject) == MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrMetaObject))
                    {
                        if (GetMetaDataRepositoryClass(CurrMetaObject) == GetMetaDataRepositoryClass(CurrUpdateMetaObject))
                        {
                            NewMetaObject = null;
                            break;
                        }
                    }
                /*if(NewMetaObject!=null)
				{
					MetaObject tmpMetaObject=MetaObjectsStack.CurrentMetaObjectCreator.FindMetaObjectInPLace(CurrMetaObject.Identity,PlaceIdentifier);
					if(tmpMetaObject!=null)
					{
						AddedObjectsCommands.Add(new AddCommand(NewMetaObject,Updated,true));
						NewMetaObject=null;
					}
				}*/
                if (NewMetaObject != null)
                    AddedObjectsCommands.Add(new AddCommand(NewMetaObject, Updated));
            }
            var source = Source.Cast<MetaObject>().ToList();
            foreach (MetaObject CurrMetaObject in Updated)
            {
                MetaObject DeleteMetaObject = CurrMetaObject;

                

                foreach (MetaObject CurrSourceMetaObject in source.ToList())
                    if (MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrSourceMetaObject) == MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrMetaObject))
                    {
                        
                        if (GetMetaDataRepositoryClass(CurrMetaObject) == GetMetaDataRepositoryClass(CurrSourceMetaObject))
                        {
                            source.Remove(CurrSourceMetaObject);
                            DeleteMetaObject = null;
                            break;
                        }
                        
                    }
                if (DeleteMetaObject != null)
                    DeletedObjectsCommands.Add(new DeleteCommand(DeleteMetaObject, Updated));
            }

        }

        /// <MetaDataID>{e19eebc5-c321-45a7-a5c4-8d3c8809f90b}</MetaDataID>
        protected System.Type GetMetaDataRepositoryClass(MetaObject metaObject)
        {
            System.Type type = metaObject.GetType();
            while (type.Namespace != typeof(MetaObject).Namespace && type != typeof(object))
                type = type.GetMetaData().BaseType;
            return type;

        }



    }
}
