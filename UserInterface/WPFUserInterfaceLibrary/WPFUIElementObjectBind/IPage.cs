using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{458a42f8-a66b-4c04-af90-8e80d192a9b8}</MetaDataID>
    public interface IPage
    {
        bool Closing(System.ComponentModel.CancelEventArgs e);

        List<IPage> NavigationServicePages { get; }

        void Close();
    }
}
