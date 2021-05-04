using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoseAddinFacade
{
    /// <MetaDataID>{2ef9367f-9b37-4a54-a163-4ad9e43e8f54}</MetaDataID>
    public interface IRoseAddin
    {
        /// <MetaDataID>{119addae-3249-4a8d-9ee2-0ba44c44c20d}</MetaDataID>
        bool SelectedContextMenuItem(object roseApplication, string internalName, int mouseLocationX, int mouseLocationY);
        void BrowseRoseItem(string projectFileName, string classifierIdentity, string memberIdentity);
        string MainWindowTitle
        {
            get;
        }
    }

    /// <MetaDataID>{b517a532-e81b-49f0-97fe-45a482e05025}</MetaDataID>
    public static class RoseAddin
    {
        public static IRoseAddin Net2Addin;
        public static IRoseAddin Net4Addin;
    }
}
