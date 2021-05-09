using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Collections.Generic;
using System;
using System.Windows.Media.Imaging;
namespace StorageManagmentStudio
{
    /// <MetaDataID>{5ff6dfdb-34ba-4c0a-bbe3-9bee2284eab0}</MetaDataID>
    [BackwardCompatibilityID("{5ff6dfdb-34ba-4c0a-bbe3-9bee2284eab0}"), OOAdvantech.MetaDataRepository.Persistent()]
    public class StorageServerLink : System.MarshalByRefObject, IStorageMetadataNode, OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {



        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{0d0cb1e4-b14f-49a0-a6e5-2eafb8c5b116}</MetaDataID>
        private OOAdvantech.ObjectStateManagerLink OSMLink = new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        string _StorageServerInstanceName;
        /// <MetaDataID>{dedfb1a0-1ba4-4045-a1b3-682f9e418928}</MetaDataID>
        [PersistentMember("_StorageServerInstanceName"), BackwardCompatibilityID("+2")]
        public string StorageServerInstanceName
        {
            get
            {
                return _StorageServerInstanceName;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StorageServerInstanceName = value;
                    stateTransition.Consistent = true;
                }
            }
        }



        /// <exclude>Excluded</exclude>
        string _StorageServerUrl;
        /// <MetaDataID>{2c85f3c0-8788-412f-bab6-6410194f7608}</MetaDataID>
        [PersistentMember("_StorageServerUrl"), BackwardCompatibilityID("+1")]
        public string StorageServerUrl
        {
            get
            {
                return _StorageServerUrl;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _StorageServerUrl = value;
                    stateTransition.Consistent = true;
                }
            }
        }


        #region IStorageMetadataNode Members

        /// <MetaDataID>{645f5dc9-54a0-4895-a07b-06d11180294e}</MetaDataID>
        public string Name
        {
            get
            {
                return StorageServerUrl;
            }
            set
            {

            }
        }

        /// <MetaDataID>{bd68c6b8-3d04-4f6d-a181-c84cf4cd73cd}</MetaDataID>
        OOAdvantech.MetaDataRepository.StorageServer _StorageServer;
        /// <MetaDataID>{8b21a2c6-374b-4e9b-ab15-0da4d6fdc044}</MetaDataID>
        public OOAdvantech.MetaDataRepository.StorageServer StorageServer
        {
            get
            {
                //if (_StorageServer == null)
                //    _StorageServer = OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService.GetStorageServer(StorageServerUrl);

                return _StorageServer;

            }
        }
        /// <MetaDataID>{4187b41d-01e1-4bb9-9e4f-8ef6d59a9f5f}</MetaDataID>
        public System.Collections.Generic.IList<IStorageMetadataNode> SubNodes
        {
            get
            {
                var storageServer = StorageServer;
                if (storageServer != null)
                    return new List<IStorageMetadataNode>() { new StoragesFolder(storageServer),new UsersFolder(storageServer) };
                else
                    return new List<IStorageMetadataNode>();
            }

        }


        public System.Windows.Media.ImageSource NodeIco
        {
            get
            {
                return null;
                //return new BitmapImage(new Uri("/StorageManagmentStudio;component/Resources/Storage.png", UriKind.Relative));
            }
        }
        #endregion

        #region IObjectStateEventsConsumer Members

        /// <MetaDataID>{abb87350-0e11-4345-ba98-9c1b9f119ed0}</MetaDataID>
        public void OnCommitObjectState()
        {

        }

        /// <MetaDataID>{bf8536c2-fc9c-4e7f-ba4b-ff51dff8be83}</MetaDataID>
        public void OnActivate()
        {
            if (string.IsNullOrEmpty(StorageServerUrl))
                return;
            OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService.BeginGetStorageServer(StorageServerUrl, new AsyncCallback(GetStorageServerCallback), OOAdvantech.Transactions.Transaction.Current);

        }

        public void TestAsynch()
        {
            OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService.BeginGetStorageServer(StorageServerUrl, new AsyncCallback(GetStorageServerCallback), OOAdvantech.Transactions.Transaction.Current);
        }



        #endregion

        void GetStorageServerCallback(IAsyncResult result)
        {
            _StorageServer = OOAdvantech.PersistenceLayer.ObjectStorage.PersistencyService.EndGetStorageServer(result);
            if (ObjectChangeState != null)
                ObjectChangeState(this, "SubNodes");
        }
    }


    /// <MetaDataID>{f911f9d2-1282-42e5-8e97-07e4a4bb0e0b}</MetaDataID>
    public static class PersistencyServiceAsynch
    {

        //static object MethodInvoke(System.Reflection.MethodInfo methodInfo, OOAdvantech.Transactions.Transaction transaction, object obj, ref object[] parameters)
        //{
        //    if (transaction != null)
        //    {
        //        using (SystemStateTransition stateTransition = new SystemStateTransition(transaction))
        //        {
        //            return methodInfo.Invoke(obj, parameters);
        //            stateTransition.Consistent = true;
        //        }
        //    }
        //    else
        //    {
        //        return methodInfo.Invoke(obj, parameters);
        //    }

        //}
        //public class MethodInvokeState
        //{

        //    public InvokeHandle MetodInvokeHandler;
        //    public OOAdvantech.Transactions.Transaction Transaction;
        //    public System.AsyncCallback Callback;

        //}
        //static void MethodInvokeCallback(IAsyncResult result)
        //{


        //    var methodInvokeState = result.AsyncState as MethodInvokeState;
        //    if (methodInvokeState.Transaction != null)
        //    {

        //        using (SystemStateTransition stateTransition = new SystemStateTransition(methodInvokeState.Transaction))
        //        {
        //            methodInvokeState.Callback(result); 
        //            stateTransition.Consistent = true;
        //        }
        
        //    }
        //    else
        //        methodInvokeState.Callback(result);




        //}
        //public delegate object InvokeHandle(System.Reflection.MethodInfo methodInfo, OOAdvantech.Transactions.Transaction transaction, object obj, ref object[] parameters);

        // static  GetStorageServerHandle GetStorageServerAsynchCaller;
        public static System.IAsyncResult BeginGetStorageServer(this OOAdvantech.PersistenceLayer.IPersistencyService persistencyService, string storageServerLocation, System.AsyncCallback callback, object asyncState)
        {

            var _methodInvoke = new OOAdvantech.AccessorBuilder.InvokeHandle(OOAdvantech.AccessorBuilder.MethodInvoke);
            object[] _params = new object[1] { storageServerLocation };
            return _methodInvoke.BeginInvoke(persistencyService.GetType().GetMethod("GetStorageServer"), OOAdvantech.Transactions.Transaction.Current, persistencyService, ref _params, new AsyncCallback(OOAdvantech.AccessorBuilder.MethodInvokeCallback), new OOAdvantech.AccessorBuilder.MethodInvokeState() { MetodInvokeHandler = _methodInvoke, Transaction = OOAdvantech.Transactions.Transaction.Current, Callback = callback });
        }

        public static OOAdvantech.MetaDataRepository.StorageServer EndGetStorageServer(this OOAdvantech.PersistenceLayer.IPersistencyService persistencyService, System.IAsyncResult result)
        {
            var methodInvokeState = result.AsyncState as OOAdvantech.AccessorBuilder.MethodInvokeState;
            object[] parameters = null;
            object retval = methodInvokeState.MetodInvokeHandler.EndInvoke(ref parameters, result);
            return retval as OOAdvantech.MetaDataRepository.StorageServer;
        }


    }


    //

    //int DoWork(ref int m2, int m1, out int m3)
    //System.IAsyncResult BeginDoWork(ref int m2, int m1, System.AsyncCallback callback, object asyncState);
    //int EndDoWork(ref int m2, out int m3, System.IAsyncResult result);
}
