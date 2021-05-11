namespace ConnectableControls
{
    partial class UserControl
    {






        /// <summary> 
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{5cbb63e0-5d67-4bf6-9b8d-2203345f3cce}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        /// <MetaDataID>{fb116cae-36b3-4760-817d-63008143cb68}</MetaDataID>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl));
            this.Connection = new ConnectableControls.ViewControlObject(this.components);
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "";
            this.Connection.ContainerControl = this;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectAssembly = null;
            this.Connection.ViewControlObjectType = null;
            // 
            // UserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(127, 90);
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{68f43197-2ba5-4290-b506-9452853d3470}</MetaDataID>
        public ConnectableControls.ViewControlObject Connection;

    }
}
