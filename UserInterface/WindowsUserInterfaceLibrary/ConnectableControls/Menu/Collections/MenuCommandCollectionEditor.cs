using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.Transactions;

namespace ConnectableControls.Menus.Collections
{
    /// <MetaDataID>{18547DA0-A37B-43C1-8062-4F31168ED9D5}</MetaDataID>
    class MenuCommandCollectionEditor : System.ComponentModel.Design.CollectionEditor
    {
        /// <MetaDataID>{9738F3C3-E708-4020-B3C1-3E8B4EE04FED}</MetaDataID>
        public MenuCommandCollectionEditor(Type type):base(type)
        {

        }

        /// <MetaDataID>{5DAF3825-C2A9-472E-A12B-85B2600BF691}</MetaDataID>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            try
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition())
                {
                    MenuCommandCollection menuCommands = null;
                    if (value is MenuCommandCollection)
                    {
                        menuCommands = new MenuCommandCollection(value as MenuCommandCollection);
                        foreach (Menus.MenuCommand menuCommand in menuCommands)
                            (value as MenuCommandCollection).Remove(menuCommand);
                    }
                    //if (value is List.Models.MenuEdit)
                    //    menuCommands = new MenuCommandCollection((value as List.Models.MenuEdit).MenuCommands);



                    base.EditValue(context, provider, menuCommands);
                    foreach (Menus.MenuCommand menuCommand in menuCommands)
                        (value as MenuCommandCollection).Add(menuCommand);
                    stateTransition.Consistent = true;
                }
            }
            catch (System.Exception error)
            {


            }
            return value;
        }
        /// <MetaDataID>{FF0DB81E-3F19-47D9-91D5-6D208DD70D4E}</MetaDataID>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
