using System;
using System.Collections.Generic;
using System.Text;
//using DisplayedValue = OOAdvantech.UserInterface.Runtime.DisplayedValue;

using IObjectMemberViewControl = OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl;
using OperationCaller = OOAdvantech.UserInterface.Runtime.OperationCaller ;
using IConnectableControl = OOAdvantech.UserInterface.Runtime.IConnectableControl;
using IPathDataDisplayer = OOAdvantech.UserInterface.Runtime.IPathDataDisplayer;


namespace ConnectableControls
{
    /// <MetaDataID>{6b91fa5d-d834-4dbd-903b-36023a16c73d}</MetaDataID>
    public class CollectionView
    { 
         
        //public void ComponentChanged(object sender, System.ComponentModel.Design.ComponentChangedEventArgs e)
        //{
        //    if (SearchOperationCall != null && e.Member.Name == "Name")
        //    {
        //        if (SearchOperationCall.ReturnValueDestination == e.OldValue as string)
        //            SearchOperationCall.ReturnValueDestination = e.NewValue as string;
        //        foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in SearchOperationCall.ParameterLoaders)
        //        {
        //            if (parameterLoader.Source == e.OldValue as string)
        //                parameterLoader.Source = e.NewValue as string;

        //        }
        //    }
        //}

        /// <MetaDataID>{7f62bbc3-9e94-4bb5-b35d-6992fc86d07c}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl CollectionViewControl;
        /// <MetaDataID>{28f64ccd-8680-4a73-b197-74a4ac46377c}</MetaDataID>
        public CollectionView(OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl collectionViewControl)
        { 
            CollectionViewControl = collectionViewControl;
            if (CollectionViewControl == null)
                throw new System.ArgumentNullException("collectionViewControl");

        }


        //System.Windows.Forms.Control GetSourceForParameter(string parameterName)
        //{


        //    foreach (OOAdvantech.UserInterface.ParameterLoader parameterLoader in SearchOperationCall.ParameterLoaders)
        //    {
        //        if (parameterLoader.Name == parameterName)
        //        {
        //            if (parameterLoader.Source == "this")
        //                return CollectionViewControl as System.Windows.Forms.Control;
        //            return CollectionViewControl.ViewControlObject.GetControlWithName(parameterLoader.Source);
        //        }
        //    }
        //    return null;
        //}

        //internal static System.Windows.Forms.Control GetControlWithName(System.Windows.Forms.Control.ControlCollection controls, string controlName)
        //{
        //    if (controls.ContainsKey(controlName))
        //        return controls[controlName];

        //    foreach (System.Windows.Forms.Control containedControl in controls)
        //    {
        //        System.Windows.Forms.Control control = GetControlWithName(containedControl.Controls, controlName);
        //        if (control != null)
        //            return control;
        //    }
        //    return null;
        //}


        /// <MetaDataID>{78dcf613-c0b1-41a7-b9e6-17112e6fc065}</MetaDataID>
        OOAdvantech.UserInterface.OperationCall _SearchOperationCall;
        /// <MetaDataID>{7d5791ce-5dab-4828-840f-ccfebeec484a}</MetaDataID>
        public OOAdvantech.UserInterface.OperationCall SearchOperationCall
        {
            set
            {
                _SearchOperationCaller = null;
                _SearchOperationCall = value;
            }
            get
            {
                return _SearchOperationCall;
            }
        }
        /// <MetaDataID>{bf0d65be-2812-49a7-9766-ede850497c54}</MetaDataID>
        public System.Reflection.MethodInfo SearchMethod
        {
            get
            {
                OOAdvantech.MetaDataRepository.Operation operation = SearchOperation;
                if (operation == null)
                    return null;
                return operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
            }
        }

        /// <MetaDataID>{beb00b26-2128-49b8-a58b-fab9b11ec2f6}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Operation SearchOperation
        {
            get
            {
                if (SearchOperationCaller == null)
                    return null;
                else
                    return SearchOperationCaller.Operation;
            }
        }

        /// <MetaDataID>{862746d9-3c39-450f-9a68-f449a9efaef8}</MetaDataID>
        public object InvokeSearchOperation()
        {
            if (SearchOperationCaller != null && SearchOperation != null)
                return SearchOperationCaller.Invoke();
            else
                return null;

        }
        //public System.Windows.Forms.Control SearchOperationDestanationControl
        //{
        //    get
        //    {
        //        if (SearchOperationCall.ReturnValueDestination == "this")
        //            return CollectionViewControl as System.Windows.Forms.Control;

        //        return CollectionViewControl.ViewControlObject.GetControlWithName(SearchOperationCall.ReturnValueDestination);
        //    }

        //}






        /// <MetaDataID>{55e71ca8-7c38-47c8-8d94-f7978d34dee8}</MetaDataID>
        public OOAdvantech.UserInterface.OperationCall _InsertOperationCall;
        /// <MetaDataID>{d244281a-ca77-415c-9428-db0128804b91}</MetaDataID>
        public OOAdvantech.UserInterface.OperationCall InsertOperationCall
        {
            get
            {
                return _InsertOperationCall;
            }
            set
            {
                _InsertOperationCall = value;
                _InsertOperationCaller = null;
            }
        }
        /// <MetaDataID>{664e383b-3856-45ca-9d8c-c7fac8a89f63}</MetaDataID>
        public System.Reflection.MethodInfo InsertMethod
        {
            get
            {
                OOAdvantech.MetaDataRepository.Operation operation = InsertOperation;
                if (operation == null)
                    return null;
                return operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
            }
        }

        /// <MetaDataID>{36583f70-e65b-458e-97bc-74ec77d4dbbf}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _InsertOperationCaller;
        /// <MetaDataID>{0f0e40a2-6bbc-4cc5-bf24-27c0cecb6a0d}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.OperationCaller InsertOperationCaller
        {
            get
            {
                if (_InsertOperationCall == null||CollectionViewControl==null||CollectionViewControl.UserInterfaceObjectConnection == null)
                    return null;
                if (_InsertOperationCaller != null)
                    return _InsertOperationCaller;
                _InsertOperationCaller = new OperationCaller(_InsertOperationCall, CollectionViewControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource);
                return _InsertOperationCaller;
            }
        }


        /// <MetaDataID>{8f59f31f-13e9-4cd0-a9f5-3357433ced1b}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _RemoveOperationCaller;
        /// <MetaDataID>{7f584f96-f09e-435e-8043-25a1a71692f7}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.OperationCaller RemoveOperationCaller
        {
            get
            {
                if (_RemoveOperationCall == null || CollectionViewControl == null || CollectionViewControl.UserInterfaceObjectConnection == null)
                    return null;

                if (_RemoveOperationCaller != null)
                    return _RemoveOperationCaller;
                _RemoveOperationCaller = new OperationCaller(_RemoveOperationCall, CollectionViewControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource);
                return _RemoveOperationCaller;
            }
        }

        /// <MetaDataID>{a5f78234-7699-4922-a05b-5754c29ce921}</MetaDataID>
        OOAdvantech.UserInterface.Runtime.OperationCaller _SearchOperationCaller;
        /// <MetaDataID>{2fe77c1f-35ea-4c2c-b765-2e6781635880}</MetaDataID>
        public OOAdvantech.UserInterface.Runtime.OperationCaller SearchOperationCaller
        {
            get
            {
                if (_SearchOperationCall == null || CollectionViewControl == null || CollectionViewControl.UserInterfaceObjectConnection == null)
                    return null;

                if (_SearchOperationCaller != null)
                    return _SearchOperationCaller;
                _SearchOperationCaller = new OperationCaller(_SearchOperationCall, CollectionViewControl as OOAdvantech.UserInterface.Runtime.IOperationCallerSource);
                return _SearchOperationCaller;
            }
        }


        /// <MetaDataID>{35735e39-d76c-4407-ad01-55cd16b6c48d}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Operation InsertOperation
        {
            get
            {
                if (InsertOperationCaller == null)
                    return null;
                else
                    return InsertOperationCaller.Operation;
            }
        }


        /// <MetaDataID>{68f90524-9aa7-4a49-b3d0-0ded42bf0141}</MetaDataID>
        public OOAdvantech.UserInterface.OperationCall _RemoveOperationCall;
        /// <MetaDataID>{7ebe46f7-77d4-48a2-8ad5-68b1b74c872f}</MetaDataID>
        public OOAdvantech.UserInterface.OperationCall RemoveOperationCall
        {
            get
            {
                return _RemoveOperationCall;
            }
            set
            {
                _RemoveOperationCall = value;
                _RemoveOperation = null;
                _RemoveOperationCaller = null;
            }
        }

        /// <MetaDataID>{21568d67-715d-4a89-85f9-01b8b603b0a6}</MetaDataID>
        public System.Reflection.MethodInfo RemoveMethod
        {
            get
            {
                OOAdvantech.MetaDataRepository.Operation operation = RemoveOperation;
                if (operation == null)
                    return null;
                return operation.GetExtensionMetaObject(typeof(System.Reflection.MethodInfo)) as System.Reflection.MethodInfo;
            }
        }
        /// <MetaDataID>{72a2549b-3f22-44bf-a492-d05e7bb210be}</MetaDataID>
        OOAdvantech.MetaDataRepository.Operation _RemoveOperation;
        /// <MetaDataID>{4bb863cb-84ee-43db-ac2d-3252f1364131}</MetaDataID>
        public OOAdvantech.MetaDataRepository.Operation RemoveOperation
        {
            get
            {
                if (RemoveOperationCaller == null)
                    return null;
                else
                    return RemoveOperationCaller.Operation;

            }
        }
    }
}
