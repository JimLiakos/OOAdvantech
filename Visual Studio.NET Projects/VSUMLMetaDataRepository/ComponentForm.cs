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
    /// <MetaDataID>{d8039c0b-dd47-4cf6-b297-9be502a55db2}</MetaDataID>
    public partial class ComponentForm : Form
    {
        System.Drawing.Point Mouselocation;
        public ComponentForm()
        {
        }
        public ComponentForm(System.Drawing.Point mouselocation,OOAdvantech.VSUMLMetaDataRepository.Component component)
        {
            Mouselocation = mouselocation;
            InitializeComponent();
            MinimumSize = Size;
            ObjectConnectionControl.Instance = component;
            Mouselocation.X = Mouselocation.X - Width / 2;
        }

        //public ComponentForm()
        //{
        //    InitializeComponent();
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Location = new System.Drawing.Point(Mouselocation.X, Mouselocation.Y);
        }
    }
}
