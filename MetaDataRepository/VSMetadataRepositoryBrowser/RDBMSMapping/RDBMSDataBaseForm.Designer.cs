namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    partial class RDBMSDataBaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{3d396335-42af-4d86-bd7b-e0a8696ce122}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{a1320073-1143-41e8-9925-d33241b0b5c3}</MetaDataID>
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
        /// <MetaDataID>{257029d2-6dd3-440b-9e0e-4feccef19bc4}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RDBMSDataBaseForm));
            this.ConnectionStringTXB = new ConnectableControls.TextBox();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.RDBMSDataBaseTypeCBX = new ConnectableControls.ComboBox();
            this.ConnectionStringLB = new System.Windows.Forms.Label();
            this.DataBaseType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ConnectionStringTXB
            // 
            this.ConnectionStringTXB.AllowDrag = false;
            this.ConnectionStringTXB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnectionStringTXB.AutoDisable = true;
            this.ConnectionStringTXB.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.ConnectionStringTXB.EnableProperty.Path = null;
            this.ConnectionStringTXB.Location = new System.Drawing.Point(126, 39);
            this.ConnectionStringTXB.Name = "ConnectionStringTXB";
            this.ConnectionStringTXB.Path = "DataBaseConnection.ConnectionString";
            this.ConnectionStringTXB.Size = new System.Drawing.Size(225, 20);
            this.ConnectionStringTXB.TabIndex = 0;
            this.ConnectionStringTXB.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.ConnectionStringTXB.ViewControlObject = this.Connection;
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "VSMetadataRepositoryBrowser.RDBMSMapping.RDBMSMappingContextPresentation";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = false;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.RequiredNested;
            this.Connection.ViewControlObjectAssembly = "CodeMetaDataRepository";
            this.Connection.ViewControlObjectType = null;
            // 
            // RDBMSDataBaseTypeCBX
            // 
            this.RDBMSDataBaseTypeCBX.AllowDrag = false;
            this.RDBMSDataBaseTypeCBX.AllowDropOperationCall = ((object)(resources.GetObject("RDBMSDataBaseTypeCBX.AllowDropOperationCall")));
            this.RDBMSDataBaseTypeCBX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RDBMSDataBaseTypeCBX.AssignPresentationObjectType = "";
            this.RDBMSDataBaseTypeCBX.AutoDisable = true;
            this.RDBMSDataBaseTypeCBX.AutoInsert = false;
            this.RDBMSDataBaseTypeCBX.AutoSuggest = false;
            this.RDBMSDataBaseTypeCBX.ChooseFromEnum = false;
            this.RDBMSDataBaseTypeCBX.ConnectedObjectAutoUpdate = false;
            this.RDBMSDataBaseTypeCBX.DisplayMember = null;
            this.RDBMSDataBaseTypeCBX.DragDropOperationCall = ((object)(resources.GetObject("RDBMSDataBaseTypeCBX.DragDropOperationCall")));
            this.RDBMSDataBaseTypeCBX.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.RDBMSDataBaseTypeCBX.EnableCondition = null;
            // 
            // 
            // 
            this.RDBMSDataBaseTypeCBX.EnableProperty.Path = null;
            this.RDBMSDataBaseTypeCBX.Enumeration = "";
            this.RDBMSDataBaseTypeCBX.FormattingEnabled = true;
            this.RDBMSDataBaseTypeCBX.InsertOperationCall = ((object)(resources.GetObject("RDBMSDataBaseTypeCBX.InsertOperationCall")));
            this.RDBMSDataBaseTypeCBX.IntegralHeight = false;
            this.RDBMSDataBaseTypeCBX.Location = new System.Drawing.Point(126, 75);
            this.RDBMSDataBaseTypeCBX.Name = "RDBMSDataBaseTypeCBX";
            this.RDBMSDataBaseTypeCBX.NullValueName = "";
            this.RDBMSDataBaseTypeCBX.OperationCall = ((object)(resources.GetObject("RDBMSDataBaseTypeCBX.OperationCall")));
            this.RDBMSDataBaseTypeCBX.Path = "DataBaseConnection.RDBMSDataBaseType";
            this.RDBMSDataBaseTypeCBX.PreLoaded = true;
            this.RDBMSDataBaseTypeCBX.RemoveOperationCall = ((object)(resources.GetObject("RDBMSDataBaseTypeCBX.RemoveOperationCall")));
            this.RDBMSDataBaseTypeCBX.Size = new System.Drawing.Size(225, 21);
            this.RDBMSDataBaseTypeCBX.TabIndex = 1;
            this.RDBMSDataBaseTypeCBX.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.RDBMSDataBaseTypeCBX.ViewControlObject = this.Connection;
            this.RDBMSDataBaseTypeCBX.WarnigMessageOnRemove = null;
            // 
            // ConnectionStringLB
            // 
            this.ConnectionStringLB.AutoSize = true;
            this.ConnectionStringLB.Location = new System.Drawing.Point(29, 46);
            this.ConnectionStringLB.Name = "ConnectionStringLB";
            this.ConnectionStringLB.Size = new System.Drawing.Size(91, 13);
            this.ConnectionStringLB.TabIndex = 2;
            this.ConnectionStringLB.Text = "Connection String";
            // 
            // DataBaseType
            // 
            this.DataBaseType.AutoSize = true;
            this.DataBaseType.Location = new System.Drawing.Point(39, 78);
            this.DataBaseType.Name = "DataBaseType";
            this.DataBaseType.Size = new System.Drawing.Size(81, 13);
            this.DataBaseType.TabIndex = 3;
            this.DataBaseType.Text = "DataBase Type";
            // 
            // RDBMSDataBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 135);
            this.Controls.Add(this.DataBaseType);
            this.Controls.Add(this.ConnectionStringLB);
            this.Controls.Add(this.RDBMSDataBaseTypeCBX);
            this.Controls.Add(this.ConnectionStringTXB);
            this.Name = "RDBMSDataBaseForm";
            this.Text = "RDBMSDataBaseForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <MetaDataID>{875573f4-344d-4037-bd91-bbfd4db2bb04}</MetaDataID>
        private ConnectableControls.FormConnectionControl Connection;
        /// <MetaDataID>{498f58ef-ba02-406a-b76c-25f17a592132}</MetaDataID>
        private ConnectableControls.TextBox ConnectionStringTXB;
        /// <MetaDataID>{0f0315dc-5f46-4de1-afb1-8e8d19960959}</MetaDataID>
        private ConnectableControls.ComboBox RDBMSDataBaseTypeCBX;
        /// <MetaDataID>{cdd41ab2-65d8-4111-9016-9c00ff9246e4}</MetaDataID>
        private System.Windows.Forms.Label DataBaseType;
        /// <MetaDataID>{a211a95d-d624-44ea-8736-61e095a3ee4b}</MetaDataID>
        private System.Windows.Forms.Label ConnectionStringLB;
    }
}