using System;
namespace OOAdvantech.UserInterface.Runtime
{
    public delegate object ObjectUIWrapper(object _object);
    ///// <MetaDataID>{f1ef99c2-0726-49a9-a355-2b2136bcb95e}</MetaDataID>
    //public class PresentationObjectsController
    //{
    //    /// <MetaDataID>{8469b1f0-081d-49f2-9a9e-2bebc3eb17de}</MetaDataID>
    //    public static string[] GetPathsFor(System.Type type)
    //    {

    //         object[] attributes= type.GetCustomAttributes(typeof(PresentationObjectPaths),true);
    //         int pathsCount = 0;
    //         foreach (PresentationObjectPaths attribute in attributes)
    //             pathsCount+= attribute.Paths.Length;

    //         string[] paths = new string[pathsCount];
    //         int i = 0;
    //         foreach (PresentationObjectPaths attribute in attributes)
    //             foreach(string path in attribute.Paths)
    //                 paths[i++]=path;
    //         return paths;

    //    }
    //}
    /// <MetaDataID>{6e21e4f5-2ec2-413c-8be2-439375910e07}</MetaDataID>
    public interface IPresentationObject
    {
        /// <MetaDataID>{69dcb154-4c8a-4405-9f9e-df468f2db237}</MetaDataID>
        object GetRealObject();
        /// <MetaDataID>{d6a15159-178c-49ba-b73d-ca963b51df8f}</MetaDataID>
        void FormClosed();





        /// <MetaDataID>{e5b42002-41b2-478c-87e9-c6391060b12f}</MetaDataID>
        void Initialize();
    }
    /// <MetaDataID>{D0665687-6876-43ED-81B3-5ADE883FF5C8}</MetaDataID>
    public class PresentationObject<T> : MarshalByRefObject,IPresentationObject, System.ComponentModel.INotifyPropertyChanged where T : class
    {

        /// <MetaDataID>{ee672f46-79a0-4714-ad73-0019f2edd59c}</MetaDataID>
        public object NativeObject
        {
            get
            {

                //if (RealObject is  System.MarshalByRefObject && System.Runtime.Remoting.RemotingServices.GetRealProxy(RealObject) is UIProxy)
                //    return (System.Runtime.Remoting.RemotingServices.GetRealProxy(RealObject) as UIProxy)._RealTransparentProxy;
                //else
                    return RealObject;
            }
        }
        public virtual void Initialize()
        {

        }

        /// <MetaDataID>{972bd526-eedf-42e9-9c28-6b838b7f8da1}</MetaDataID>
        public virtual void FormClosed()
        {

        }
        /// <MetaDataID>{6aa73024-4981-4602-bcd5-c503c10b7a87}</MetaDataID>
        public override bool Equals(object obj)
        {
            if (obj is PresentationObject<T> &&RealObject!=null&& RealObject.Equals((obj as PresentationObject<T>).RealObject))
                return true;
            else if (obj is PresentationObject<T> && RealObject!=null&&!RealObject.Equals((obj as PresentationObject<T>).RealObject))
                return false;

            return base.Equals(obj);
        }
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{73C16710-84DE-4197-9338-3B1B2E8F417B}</MetaDataID>
        private T _RealObject;

        /// <MetaDataID>{899D1D29-CDE7-4291-8291-6BBF527CEDF3}</MetaDataID>
        virtual public T RealObject
        {
            get
            {
                return _RealObject;
            }
        }

        /// <MetaDataID>{ab840d28-91ca-4ba5-8ef5-7bad4124405d}</MetaDataID>
        UserInterface.Runtime.UserInterfaceObjectConnection _UserInterfaceObjectConnection;
        /// <MetaDataID>{1071d0f4-eca6-41b5-a1ab-2a85617bbee1}</MetaDataID>
        public UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            internal set
            {
                _UserInterfaceObjectConnection = value;
            }
            get
            {
                return _UserInterfaceObjectConnection;
            }

        }

        /// <MetaDataID>{25b9332b-30c5-494d-810a-eaed32b415d2}</MetaDataID>
        public PresentationObject(T realObject) 
        {
            //TODO οταν αλλάζει
            if (Remoting.RemotingServices.IsOutOfProcess(realObject as System.MarshalByRefObject)&&!realObject.GetType().IsCOMObject)
                _RealObject = realObject;//new UIProxy(realObject as System.MarshalByRefObject, typeof(T)).GetTransparentProxy() as T;
            else
                _RealObject = realObject;


            //bool flag = _RealObject.Equals(realObject);
            //System.Collections.Generic.Dictionary<object, string> testColl = new System.Collections.Generic.Dictionary<object, string>();
            //testColl[_RealObject] = "Liaks";
            //string sfs = testColl[realObject];

        }

        #region IPresentationObject Members

        /// <MetaDataID>{b88fdd61-5df0-4022-a929-7df1192498b8}</MetaDataID>
        object IPresentationObject.GetRealObject()
        {
            return RealObject;
        }

        #endregion

        /// <MetaDataID>{428f64ed-b749-4996-b0b2-f5e567a80ead}</MetaDataID>
        public static bool CanPresent(OOAdvantech.MetaDataRepository.Class wrapperClass, OOAdvantech.MetaDataRepository.Classifier wrappedClass)
        {
            if (wrappedClass == null || wrapperClass == null)
                return false;

     
            foreach (OOAdvantech.MetaDataRepository.Classifier classifier in wrapperClass.GetAllGeneralClasifiers())
            {
                if (!(classifier is OOAdvantech.MetaDataRepository.Class))
                    continue;
                
                if (classifier.TemplateBinding != null && (classifier.TemplateBinding.Signature.Template as OOAdvantech.MetaDataRepository.Classifier).FullName == typeof(OOAdvantech.UserInterface.Runtime.PresentationObject<>).FullName)
                {
                    OOAdvantech.MetaDataRepository.Classifier realObjectClass = (classifier.TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier);
                    foreach (OOAdvantech.MetaDataRepository.Operation operation in wrapperClass.GetOperations(wrapperClass.Name, true))
                    {
                        if (operation.Parameters.Count == 1)
                        {
                            foreach (OOAdvantech.MetaDataRepository.Parameter parameter in operation.Parameters)
                            {
                                if(parameter.Type!=null&&wrappedClass.FullName!=null)
                                    if (parameter.Type.IsA(realObjectClass)|| parameter.Type.FullName == wrappedClass.FullName)
                                        return true;
                            }
                        }
                    }
                }
            }
            return false;
            
        }

        

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        
    }
}
