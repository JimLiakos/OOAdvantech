using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{62D428F6-8844-431A-932D-63AFA799D414}</MetaDataID>
    class EditMenuMetadata: System.Drawing.Design.UITypeEditor
    {
        /// <MetaDataID>{A6DD00BD-53D7-4B08-A31C-47DB633E34C8}</MetaDataID>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            
            try
            {
  
                ConnectableControls.Menus.MenuCommand mainMenu = value as ConnectableControls.Menus.MenuCommand;
                ConnectableControls.Menus.MenuEditor menuEditor = new ConnectableControls.Menus.MenuEditor(mainMenu);
                menuEditor.Location = Control.MousePosition;
                menuEditor.ShowDialog();
                return null;

            }
            catch (Exception error)
            {


            }

            return null;


        }
        /// <MetaDataID>{8174C26E-9930-4D15-95D3-339FCEF5345A}</MetaDataID>
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
