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
    public partial class ClassifiertForm : Form
    {
        System.Drawing.Point Mouselocation;
        public ClassifiertForm()
        {

            InitializeComponent();
        }
        public ClassifiertForm(System.Drawing.Point mouselocation, OOAdvantech.MetaDataRepository.Classifier classifier)
        {
            Mouselocation = mouselocation;
            InitializeComponent();
            MinimumSize = Size;
            ObjectConnectionControl.Instance = classifier;
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

            foreach (var item in (OOAdvantech.CodeMetaDataRepository.IDEManager.GetCurrentDTE().CommandBars as Microsoft.VisualStudio.CommandBars.CommandBars).OfType<Microsoft.VisualStudio.CommandBars.CommandBar>().ToArray())
            {
                if (item.Name == "Model Explorer Context Menu")
                {
                    foreach (Microsoft.VisualStudio.CommandBars.CommandBarControl comm in item.Controls)
                    {
                        
                    }
                }
                System.Diagnostics.Debug.WriteLine(item.Name);
            }
        }
    }
}
