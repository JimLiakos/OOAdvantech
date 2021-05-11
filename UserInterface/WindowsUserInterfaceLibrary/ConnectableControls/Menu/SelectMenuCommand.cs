using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{d644d3da-35cc-4f25-99a5-ce449bb03058}</MetaDataID>
    public class MenuSelection
    {
        public readonly  string SelectedName;
        public readonly ConnectableControls.Menus.MenuCommand Selections;
        public MenuSelection(string selectedName, ConnectableControls.Menus.MenuCommand selections)
        {
            SelectedName = selectedName;
            Selections = selections;

        }
        public override string ToString()
        {
            return SelectedName;
        }

    }

    /// <MetaDataID>{2e5f3bd3-a722-4f91-81c7-fb06f41aa6db}</MetaDataID>
    public class SelectMenuCommand : System.Drawing.Design.UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value  is MenuSelection)
            {
                int returnDir = 0;
                 ConnectableControls.Menus.PopupMenu popupMenu = new ConnectableControls.Menus.PopupMenu();
                value= popupMenu.TrackPopup(
                Control.MousePosition,
                Control.MousePosition,
                ConnectableControls.Menus.Common.Direction.Horizontal,
                ((value as MenuSelection).Selections),
                0,
                ConnectableControls.Menus.GapPosition.None, false, null, false, ref returnDir);
                return value;
            }
            return null;


            
        }    
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
