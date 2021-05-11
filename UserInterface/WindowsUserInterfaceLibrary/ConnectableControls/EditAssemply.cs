using System.ComponentModel;
using System;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ConnectableControls
{

    /// <MetaDataID>{84C287ED-54FB-4538-A68B-454AC23924B8}</MetaDataID>
    public class EditAssemply : System.Drawing.Design.UITypeEditor
    {
        /// <MetaDataID>{15EEB08B-BAA0-4A6E-A6E8-49B207D51FC4}</MetaDataID>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "dll files (*.dll)|*.dll|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    return System.Reflection.AssemblyName.GetAssemblyName(openFileDialog.FileName).FullName;
                }
                catch (System.Exception error)
                {
                }
            }
            return value;
        }
        /// <MetaDataID>{B02B4B38-0E4D-487C-8593-55C5BC48A61C}</MetaDataID>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
