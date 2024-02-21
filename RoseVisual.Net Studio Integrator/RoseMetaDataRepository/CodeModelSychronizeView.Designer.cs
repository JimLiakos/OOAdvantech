namespace RoseMetaDataRepository
{
    partial class CodeModelSychronizeView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{92f572ac-0cbf-45a5-b24b-e9e8e88d8958}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{4a96c2d7-925c-46c2-9c42-66fc872c3810}</MetaDataID>
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
        /// <MetaDataID>{5b3d62c7-0925-40c6-80a1-29f4f6654b45}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeModelSychronizeView));
            this.ModelItemsView = new ConnectableControls.List.ListView();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ModelItemsLabel = new ConnectableControls.Label();
            this.CodelItemsLabel = new ConnectableControls.Label();
            this.CodeItemsView = new ConnectableControls.List.ListView();
            this.DeleteBtn = new ConnectableControls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ModelItemsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodeItemsView)).BeginInit();
            this.SuspendLayout();
            // 
            // ModelItemsView
            // 
            this.ModelItemsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModelItemsView.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.ModelItemsView.EnabledProperty.Path = null;
            this.ModelItemsView.FocusedBackColor = System.Drawing.Color.White;
            this.ModelItemsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            // 
            // 
            // 
            this.ModelItemsView.ListConnection.AllowDrag = false;
            this.ModelItemsView.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.ModelItemsView.ListConnection.AssignPresentationObjectType = "";
            this.ModelItemsView.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.ModelItemsView.ListConnection.ConnectedObjectAutoUpdate = false;
            this.ModelItemsView.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.ModelItemsView.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.ModelItemsView.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ModelItemsView.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.ModelItemsView.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.ModelItemsView.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.ModelItemsView.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.ModelItemsView.ListConnection.Path = "UnAssignedModelItems";
            this.ModelItemsView.ListConnection.SelectionMember = null;
            this.ModelItemsView.ListConnection.ViewControlObject = this.Connection;
            this.ModelItemsView.Location = new System.Drawing.Point(0, 60);
            this.ModelItemsView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.ModelItemsView.Name = "ModelItemsView";
            this.ModelItemsView.RowHeight = 15;
            this.ModelItemsView.Size = new System.Drawing.Size(767, 818);
            this.ModelItemsView.TabIndex = 0;
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = true;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectAssembly = "RoseMetadaRepository";
            this.Connection.ViewControlObjectType = "RoseMetaDataRepository.CodeModelSychronizer";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(29, 26);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ModelItemsLabel);
            this.splitContainer1.Panel1.Controls.Add(this.ModelItemsView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CodelItemsLabel);
            this.splitContainer1.Panel2.Controls.Add(this.CodeItemsView);
            this.splitContainer1.Size = new System.Drawing.Size(1552, 878);
            this.splitContainer1.SplitterDistance = 767;
            this.splitContainer1.SplitterWidth = 17;
            this.splitContainer1.TabIndex = 1;
            // 
            // ModelItemsLabel
            // 
            this.ModelItemsLabel.AutoSize = true;
            this.ModelItemsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ModelItemsLabel.Location = new System.Drawing.Point(7, 15);
            this.ModelItemsLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.ModelItemsLabel.Name = "ModelItemsLabel";
            this.ModelItemsLabel.Path = null;
            this.ModelItemsLabel.Size = new System.Drawing.Size(267, 36);
            this.ModelItemsLabel.TabIndex = 1;
            this.ModelItemsLabel.Text = "From Model Items";
            // 
            // 
            // 
            this.ModelItemsLabel.TextEnableProperty.Path = null;
            this.ModelItemsLabel.ViewControlObject = this.Connection;
            // 
            // CodelItemsLabel
            // 
            this.CodelItemsLabel.AutoSize = true;
            this.CodelItemsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.CodelItemsLabel.Location = new System.Drawing.Point(32, 15);
            this.CodelItemsLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.CodelItemsLabel.Name = "CodelItemsLabel";
            this.CodelItemsLabel.Path = null;
            this.CodelItemsLabel.Size = new System.Drawing.Size(255, 36);
            this.CodelItemsLabel.TabIndex = 2;
            this.CodelItemsLabel.Text = "From Code Items";
            // 
            // 
            // 
            this.CodelItemsLabel.TextEnableProperty.Path = null;
            this.CodelItemsLabel.ViewControlObject = this.Connection;
            // 
            // CodeItemsView
            // 
            this.CodeItemsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CodeItemsView.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.CodeItemsView.EnabledProperty.Path = null;
            this.CodeItemsView.FocusedBackColor = System.Drawing.Color.White;
            this.CodeItemsView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            // 
            // 
            // 
            this.CodeItemsView.ListConnection.AllowDrag = false;
            this.CodeItemsView.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall1")));
            this.CodeItemsView.ListConnection.AssignPresentationObjectType = "";
            this.CodeItemsView.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall1")));
            this.CodeItemsView.ListConnection.ConnectedObjectAutoUpdate = false;
            this.CodeItemsView.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall1")));
            this.CodeItemsView.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall1")));
            this.CodeItemsView.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.CodeItemsView.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall1")));
            this.CodeItemsView.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall1")));
            this.CodeItemsView.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall1")));
            this.CodeItemsView.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData1")));
            this.CodeItemsView.ListConnection.Path = "UnAssignedCodeItems";
            this.CodeItemsView.ListConnection.SelectionMember = null;
            this.CodeItemsView.ListConnection.ViewControlObject = this.Connection;
            this.CodeItemsView.Location = new System.Drawing.Point(0, 60);
            this.CodeItemsView.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.CodeItemsView.Name = "CodeItemsView";
            this.CodeItemsView.RowHeight = 15;
            this.CodeItemsView.Size = new System.Drawing.Size(698, 818);
            this.CodeItemsView.TabIndex = 1;
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.AllowDrag = false;
            this.DeleteBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.DeleteBtn.ConnectedObjectAutoUpdate = false;
            this.DeleteBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DeleteBtn.Location = new System.Drawing.Point(677, 949);
            this.DeleteBtn.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.OnClickOperationCall = ((object)(resources.GetObject("DeleteBtn.OnClickOperationCall")));
            this.DeleteBtn.Path = "";
            this.DeleteBtn.SaveButton = false;
            this.DeleteBtn.Size = new System.Drawing.Size(256, 86);
            this.DeleteBtn.TabIndex = 2;
            this.DeleteBtn.Text = "Delete";
            // 
            // 
            // 
            this.DeleteBtn.TextProperty.Path = null;
            this.DeleteBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Value = null;
            this.DeleteBtn.ViewControlObject = this.Connection;
            // 
            // CodeModelSychronizeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(216F, 216F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1610, 1060);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "CodeModelSychronizeView";
            this.Text = "Delete Unassigned Model-Code Items";
            ((System.ComponentModel.ISupportInitialize)(this.ModelItemsView)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CodeItemsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{732a9e3d-4118-4596-9c61-f2b4cbdde0d8}</MetaDataID>
        private ConnectableControls.List.ListView ModelItemsView;
        /// <MetaDataID>{0fbcb373-91d2-4871-b392-f10e4e3611d0}</MetaDataID>
        private System.Windows.Forms.SplitContainer splitContainer1;
        /// <MetaDataID>{9d27711b-d1f3-4f6d-9fe5-bb7931134bc4}</MetaDataID>
        private ConnectableControls.List.ListView CodeItemsView;
        /// <MetaDataID>{26f1af8a-d19b-469e-ba35-6fc1cd64aef5}</MetaDataID>
        public ConnectableControls.FormConnectionControl Connection;
        /// <MetaDataID>{935420f7-b032-4a79-ab19-fe8eb595f224}</MetaDataID>
        private ConnectableControls.Label ModelItemsLabel;
        /// <MetaDataID>{83094002-4a7d-41cf-9b84-82033a07c82d}</MetaDataID>
        private ConnectableControls.Label CodelItemsLabel;
        /// <MetaDataID>{04d0e477-27cc-4552-89d2-afb1eaa5a2e9}</MetaDataID>
        private ConnectableControls.Button DeleteBtn;
    }
}