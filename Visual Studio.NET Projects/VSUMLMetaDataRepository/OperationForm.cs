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
    /// <MetaDataID>{2662d4f8-5e1a-4419-aee5-ced9e1a404dc}</MetaDataID>
    public partial class OperationForm : Form
    {
        public OperationForm()
        {
            InitializeComponent();
        }
        Method Method;
        System.Drawing.Point Mouselocation;
        public OperationForm(System.Drawing.Point mouselocation, Operation vsUMLOperation,Method method)
        {
            Mouselocation = mouselocation;
            InitializeComponent();
            Mouselocation.X = Mouselocation.X - Width / 2;
            ObjectConnectionControl.Instance = vsUMLOperation;
            MinimumSize = Size;
            Method = method;
        }
        protected override void OnLoad(EventArgs e)
        {
            Location = Mouselocation;
            base.OnLoad(e);

            OperationPresentationObject operationPresentationObject = ObjectConnectionControl.UserInterfaceObjectConnection.PresentationObject as OperationPresentationObject;
            operationPresentationObject.Method = Method;
            if (Method != null)
                Text = "Metod Properties";
        }
    }
}
