namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    partial class RDBMSMappingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RDBMSMappingForm));
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.MetaObjectSpecificView = new ConnectableControls.DynamicViewContainer();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "VSMetadataRepositoryBrowser.MetaObjectMappingPresentation";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectAssembly = "OOAdvantech";
            this.Connection.ViewControlObjectType = "OOAdvantech.MetaDataRepository.MetaObject";
            // 
            // MetaObjectSpecificView
            // 
            this.MetaObjectSpecificView.BackColor = System.Drawing.SystemColors.Control;
            this.MetaObjectSpecificView.DefaultHostedViewType = null;
            this.MetaObjectSpecificView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetaObjectSpecificView.DockHostedView = ConnectableControls.HostedViewDock.Fill;
            this.MetaObjectSpecificView.HostedViewIdentityPath = "MetaObjectView";
            this.MetaObjectSpecificView.Location = new System.Drawing.Point(0, 0);
            this.MetaObjectSpecificView.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MetaObjectSpecificView.Name = "MetaObjectSpecificView";
            this.MetaObjectSpecificView.Path = "RealObject";
            this.MetaObjectSpecificView.Size = new System.Drawing.Size(1321, 795);
            this.MetaObjectSpecificView.TabIndex = 0;
            this.MetaObjectSpecificView.ViewControlObject = this.Connection;
            // 
            // RDBMSMappingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1321, 795);
            this.ControlBox = false;
            this.Controls.Add(this.MetaObjectSpecificView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RDBMSMappingForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectableControls.DynamicViewContainer MetaObjectSpecificView;
        public ConnectableControls.FormConnectionControl Connection;

    }
}