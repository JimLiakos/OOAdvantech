namespace OOAdvantech.UserInterface.Runtime
{
    /// <MetaDataID>{E6BCBFD1-D398-4689-9100-02E9F75D7F91}</MetaDataID>
    public interface IPathDataDisplayer
    {

        /// <MetaDataID>{2F3B4B2D-C2B3-47A4-8D01-BEF1DBDDF659}</MetaDataID>
        object Path
        {
            get;
            set;
        }


        /// <MetaDataID>{064D7141-BE8B-48F3-86FE-9B3FF628A564}</MetaDataID>
        void LoadControlValues();
        /// <MetaDataID>{0FB3954F-B99F-429D-ACC8-CAE9F83EA9CF}</MetaDataID>
        void SaveControlValues();


        /// <MetaDataID>{36382288-b5e3-452c-86bf-f1ea7a99bc48}</MetaDataID>
        UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get;
        }

        /// <summary>
        /// Defines all paths the main path and subpaths that control use to get data; 
        /// </summary>
        /// <MetaDataID>{e39f4289-acf8-4d56-8cfb-3d034e5ecfdb}</MetaDataID>
        OOAdvantech.Collections.Generic.List<string> Paths
        {
            get;
        }

        /// <MetaDataID>{446b6461-2354-4aff-972c-37cf2f4b9cf5}</MetaDataID>
        bool HasLockRequest
        {
            get;
        }

        /// <MetaDataID>{16e96b2d-9e21-49c1-95e0-a5037c0f068c}</MetaDataID>
        void DisplayedValueChanged(object sender, MemberChangeEventArg change);

        /// <MetaDataID>{a64040d7-1b38-4778-9662-c8c3be483be3}</MetaDataID>
        void LockStateChange(object sender);
    }
}
