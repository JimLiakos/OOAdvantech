using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.Transactions;
using System.Windows.Media.Imaging;

namespace StorageManagmentStudio
{
    /// <MetaDataID>StorageManagmentStudio.StoragePresentation</MetaDataID>
    public class StoragePresentation:MarshalByRefObject,IStorageMetadataNode
    {

        OOAdvantech.MetaDataRepository.StorageReference StorageReference;

        public StoragePresentation(OOAdvantech.MetaDataRepository.StorageReference storageReference)
        {
            StorageReference = storageReference;
            _SubNodes = new List<IStorageMetadataNode>();

            try
            {

                var testa = OOAdvantech.Linq.ObjectValuesCollector.GetObjectValues(storageReference,
                        storageRef =>
                            new
                            {
                                OwnedElementNames = from component in storageRef.Storage.Components
                                                    from presistentClass in component.Residents.OfType<OOAdvantech.MetaDataRepository.Class>()
                                                    where presistentClass.Persistent == true
                                                    orderby presistentClass.Name
                                                    select new PersistentClass { Class = presistentClass, Name = presistentClass.Name }

                            });
                
                _SubNodes.Add(
                    new ClassesFolder( (from storageMetadataNode in testa.OwnedElementNames.ToList()
                             orderby storageMetadataNode.Name
                            select storageMetadataNode).ToList()));

            }
            catch (Exception error)
            {
                
            }

        }


        #region IStorageMetadataNode Members

        public string Name
        {
            get
            {

                //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
                //{
                    return StorageReference.Name; 
                //    stateTransition.Consistent = true;
                //}
        
            }
            set
            {
                
            }
        }

        List<IStorageMetadataNode> _SubNodes;

        public IList<IStorageMetadataNode> SubNodes
        {
            get 
            {
                if(_SubNodes==null)
                 _SubNodes= new List<IStorageMetadataNode>();
                return _SubNodes;
            }
        }



        public System.Windows.Media.ImageSource NodeIco
        {
            get
            {
                
                return new BitmapImage(new Uri("/StorageManagmentStudio;component/Resources/Storage.png",UriKind.Relative));
            }
        }

        #endregion
    }

    /// <MetaDataID>StorageManagmentStudio.StoragePresentation</MetaDataID>
    public class PersistentClass : MarshalByRefObject, IStorageMetadataNode
    {
        public OOAdvantech.MetaDataRepository.Class Class { get; set; }




        #region IStorageMetadataNode Members

        string _Name;
        public string Name
        {
            get
            {
                return _Name;
                
            }
            set
            {
                _Name = value;
            }
        }

        public IList<IStorageMetadataNode> SubNodes
        {
            get 
            {
                return new List<IStorageMetadataNode>();
            }
        }

      

        public System.Windows.Media.ImageSource NodeIco
        {
            get 
            {
             
                return new BitmapImage(new Uri("/StorageManagmentStudio;component/Resources/VSObject_Class.png",UriKind.Relative));
            }
        }

        #endregion
    }
}
