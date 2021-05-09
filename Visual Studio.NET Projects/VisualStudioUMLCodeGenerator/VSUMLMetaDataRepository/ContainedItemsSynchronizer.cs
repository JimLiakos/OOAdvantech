using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{8ffee059-681c-49fd-9c24-7dd220343f8e}</MetaDataID>
    public class ContainedItemsSynchronizer : OOAdvantech.MetaDataRepository.ContainedItemsSynchronizer
    {
        public ContainedItemsSynchronizer(System.Collections.IList theSource, System.Collections.IList theUpdated, OOAdvantech.MetaDataRepository.MetaObject placeIdentifier)
            : base(theSource, theUpdated, placeIdentifier)
        {
        }
        public override void FindModifications()
        {
            foreach (OOAdvantech.MetaDataRepository.MetaObject CurrMetaObject in Source)
            {

                OOAdvantech.MetaDataRepository.MetaObject NewMetaObject = CurrMetaObject;
                foreach (OOAdvantech.MetaDataRepository.MetaObject CurrUpdateMetaObject in Updated)
                    if (OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrUpdateMetaObject) ==OOAdvantech.MetaDataRepository. MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrMetaObject))
                    {
                        //if (GetMetaDataRepositoryClass(CurrMetaObject) == GetMetaDataRepositoryClass(CurrUpdateMetaObject))
                        {
                            NewMetaObject = null;
                            break;
                        }
                        //else
                        //{

                        //}
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
                    AddedObjectsCommands.Add(new OOAdvantech.MetaDataRepository. AddCommand(NewMetaObject, Updated));
            }
            foreach (OOAdvantech.MetaDataRepository.MetaObject CurrMetaObject in Updated)
            {
                OOAdvantech.MetaDataRepository.MetaObject DeleteMetaObject = CurrMetaObject;
                foreach (OOAdvantech.MetaDataRepository.MetaObject CurrSourceMetaObject in Source)
                    if (OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrSourceMetaObject) == OOAdvantech.MetaDataRepository.MetaObjectsStack.CurrentMetaObjectCreator.GetIdentity(CurrMetaObject))
                    {
                        if (GetMetaDataRepositoryClass(CurrMetaObject) == GetMetaDataRepositoryClass(CurrSourceMetaObject))
                        {
                            DeleteMetaObject = null;
                            break;
                        }
                    }
                if (DeleteMetaObject != null)
                    DeletedObjectsCommands.Add(new OOAdvantech.MetaDataRepository.DeleteCommand(DeleteMetaObject, Updated));
            }
        }

    }
}
