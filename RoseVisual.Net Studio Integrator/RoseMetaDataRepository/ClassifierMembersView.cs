using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RoseMetaDataRepository
{
    /// <MetaDataID>{4714f705-a5ee-4793-9899-ce5fb1ad6d70}</MetaDataID>
    [ComVisible(false)]
    internal partial class ClassifierMembersView : Form
    { 
        public ClassifierMembersView()
        {
            InitializeComponent();
            Members.LostFocus += new EventHandler(MembersLostFocus);
        }

        void MembersLostFocus(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
             
        }


        private void MembersMouseUp(object sender, MouseEventArgs e)
        {
            if (Members.SelectedIndicies.Length > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }

        }
    }
}