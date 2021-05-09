using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

namespace OOAdvantech.VSUMLMetaDataRepository
{
    /// <MetaDataID>{beaf7033-4ab4-4507-b1d5-575d55f6ac0e}</MetaDataID>
    public partial class ComponentResidentsForm : Form
    {
        System.Drawing.Point SatrtLocation;
        public ComponentResidentsForm(Microsoft.VisualStudio.Uml.Components.IComponent vsUMLModule, System.Drawing.Point location)
        {
            InitializeComponent();
            SatrtLocation = location;
            SatrtLocation.X = SatrtLocation.X - Width / 2;
            Component component = MetaObjectMapper.FindMetaObjectFor(vsUMLModule as ModelElement) as Component;
            if(component==null)
                component=new Component(vsUMLModule,(vsUMLModule as ModelElement).GetModel());
            Connection.Instance = component;

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Location = SatrtLocation;
        }

        public bool UpdateModel;
    }
}
