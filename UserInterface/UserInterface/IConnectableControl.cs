using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{35b99e31-aaab-402f-af88-70cc0837658e}</MetaDataID>
    public interface IConnectableControl : IMetadataSelectionResolver
    {

        /// <MetaDataID>{0f5e191e-0d4e-42fd-89e4-c82b8219394d}</MetaDataID>
        void InitializeControl();

        

        /// <MetaDataID>{413af8fe-e4e5-4be3-81e1-91c4561de2e8}</MetaDataID>
        UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get;
            set;
        }
        /// <MetaDataID>{7591a2dd-3151-497d-bd35-fe2a1506f060}</MetaDataID>
        List<IDependencyProperty> DependencyProperties
        {
            get;
        }
    

        /// <MetaDataID>{37181919-8f09-4823-b20d-c8053530b240}</MetaDataID>
        bool IsPropertyReadOnly(string propertyName);


        /// <MetaDataID>{bb6fece1-d06c-48ed-bf95-b714b7389091}</MetaDataID>
        void UserInterfaceObjectConnectionChangeState(ViewControlObjectState oldState, ViewControlObjectState newState);
    }

    /// <MetaDataID>{3dbf7de0-0fc9-4657-acd8-13af6dc9c4bf}</MetaDataID>
    public interface IDependencyProperty
    {
        /// <MetaDataID>{4b76c2e7-11f9-452f-9f50-406ecf791998}</MetaDataID>
        string Path
        {
            get;
        }
        /// <MetaDataID>{8d114cbe-1871-4eac-a92a-27ddc5f9a6db}</MetaDataID>
        object Value
        {
            get;
            set;
        }

        /// <MetaDataID>{4a671a9e-4fd0-41da-839c-d730d9231772}</MetaDataID>
        void LoadPropertyValue();
    }

    /// <MetaDataID>{7a9838e1-4886-422e-9fee-5a384d6c969b}</MetaDataID>
    public interface IMetadataSelectionResolver
    {
        /// <MetaDataID>{335938d5-15b1-40fa-9cb8-7723357fb42a}</MetaDataID>
        bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor);
        /// <MetaDataID>{92e5744d-5a59-455b-9bcd-25af5855d8fc}</MetaDataID>
        OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(System.ComponentModel.ITypeDescriptorContext context);
    }
    
}
