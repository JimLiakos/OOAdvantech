using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{458a42f8-a66b-4c04-af90-8e80d192a9b8}</MetaDataID>
    public interface IPage
    {
        /// <MetaDataID>{ff5e5caa-ab73-4981-9c01-08d8f15c341e}</MetaDataID>
        bool Closing(System.ComponentModel.CancelEventArgs e);

        /// <MetaDataID>{02de8990-bd40-49aa-b753-80db59f3f455}</MetaDataID>
        List<IPage> NavigationServicePages { get; }

        /// <MetaDataID>{ca65729b-c1a5-41b7-8058-b334c2a5cdbe}</MetaDataID>
        void Close();

        /// <MetaDataID>{657a4ccb-4236-408f-81cc-95d93ca699da}</MetaDataID>
        void ObjectContextTransactionAborted();
    }
}
