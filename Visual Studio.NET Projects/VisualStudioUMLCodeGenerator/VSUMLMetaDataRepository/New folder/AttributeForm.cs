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
    /// <MetaDataID>{5588167d-4875-4449-aa6f-69ac53c8e57d}</MetaDataID>
    public partial class AttributeForm : Form
    {
        /// <MetaDataID>{3bf7056c-2a56-4e13-a3e8-265a03af62d7}</MetaDataID>
        public AttributeForm()
        {
            InitializeComponent();
            MinimumSize = Size;
        }
        /// <MetaDataID>{88bb68dd-8839-46ec-a714-7ecd44accbf5}</MetaDataID>
        AttributeRealization AttributeRealization;
        /// <MetaDataID>{4e04ffb3-fc52-49d9-ae5c-ca85894961d8}</MetaDataID>
        System.Drawing.Point Mouselocation;
        /// <MetaDataID>{f5272b22-bdd6-4cee-90ad-8654366bdb83}</MetaDataID>
        public AttributeForm(System.Drawing.Point mouselocation, Attribute attribute, AttributeRealization attributeRealization)
        {
            AttributeRealization = attributeRealization;
            Mouselocation = mouselocation;
            InitializeComponent();
            ObjectConnectionControl.Instance = attribute;
            MinimumSize = Size;
            Mouselocation.X = Mouselocation.X - Width / 2;

            

        }
        /// <MetaDataID>{b52e09eb-dc85-47a1-93c6-2e4d8b85bfe7}</MetaDataID>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Location = Mouselocation;

            AttributePresentationObject attributePresentationObject = ObjectConnectionControl.UserInterfaceObjectConnection.PresentationObject as AttributePresentationObject;
            attributePresentationObject.AttributeRealization = AttributeRealization;
        }
    }
}
