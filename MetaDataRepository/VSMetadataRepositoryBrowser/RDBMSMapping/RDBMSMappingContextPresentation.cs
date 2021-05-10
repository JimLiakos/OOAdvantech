using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    /// <MetaDataID>{16e5bd0d-52c0-4e2b-bc57-f613f30e28cf}</MetaDataID>
    public class RDBMSMappingContextPresentation : OOAdvantech.UserInterface.Runtime.PresentationObject<OOAdvantech.CodeMetaDataRepository.RDBMSMappingContext>
    {
        /// <MetaDataID>{15d8cbb1-a1ec-4c31-a95e-5d61fb62e90f}</MetaDataID>
        public RDBMSMappingContextPresentation(OOAdvantech.CodeMetaDataRepository.RDBMSMappingContext rdbmsMappingContext)
            : base(rdbmsMappingContext)
        {
              
        }

        /// <MetaDataID>{8847ff9e-9681-4b96-a3fc-74082fafdf5a}</MetaDataID>
        bool UnderTransaction;
        /// <exclude>Excluded</exclude>
        OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection _DataBaseConnection;
        /// <MetaDataID>{4406042a-ca07-4e16-b335-5f217dd73ba8}</MetaDataID>
        public OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection DataBaseConnection
        {
            get
            {
                if (_DataBaseConnection == null)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(RealObject.ObjectStorage);
                    var dataBaseConnections = from dataBaseConnection in storage.GetObjectCollection<OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection>()
                                              select dataBaseConnection;
                    foreach (OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection dataBaseConnection in dataBaseConnections)
                    {
                        _DataBaseConnection = dataBaseConnection;
                        break;
                    }
                    if(_DataBaseConnection ==null)
                        _DataBaseConnection = RealObject.ObjectStorage.NewObject<OOAdvantech.RDBMSMetaDataRepository.DataBaseConnection>();
                }
                if (_DataBaseConnection != null && OOAdvantech.Transactions.Transaction.Current != null && !UnderTransaction)
                {
                    OOAdvantech.Transactions.Transaction.Current.TransactionCompleted += new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);

                    UnderTransaction = true;
                }

                return _DataBaseConnection;
            }
        }

        /// <MetaDataID>{04f7b684-f674-447e-8276-91923136e85e}</MetaDataID>
        void OnTransactionCompleted(OOAdvantech.Transactions.Transaction transaction)
        {
            UnderTransaction = false;
            transaction.TransactionCompleted -= new OOAdvantech.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);
            RealObject.Save();
            
        }
        /// <MetaDataID>{c3e28ed3-53d3-45ec-a805-6cac65744547}</MetaDataID>
        public List<string> RDBMSDataBaseTypes
        {
            get
            {
                return new List<string>() { "MSSQL", "ORACLE", "MySQL" };
            }
        }

    }
}
