using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{6f0d7578-43cf-4030-ac67-bf02ad71d6f6}</MetaDataID>
    public partial class AssociationForm : Form
    {
        /// <MetaDataID>{d90e6acd-df9b-4fd4-9e19-5e25fba9ac1d}</MetaDataID>
        public AssociationForm()
        {
            InitializeComponent();
            MinimumSize = Size;
        }

        System.Drawing.Point Mouselocation;
        public AssociationForm(System.Drawing.Point mouselocation, Association association)
        {
            Mouselocation = mouselocation;
            
            InitializeComponent();
            Mouselocation.X = Mouselocation.X - Width / 2;
            ObjectConnectionControl.Instance = association;
            MinimumSize = Size;
        }
        protected override void OnLoad(EventArgs e)
        {
            Location = Mouselocation;
            base.OnLoad(e);
        }
    }
}
