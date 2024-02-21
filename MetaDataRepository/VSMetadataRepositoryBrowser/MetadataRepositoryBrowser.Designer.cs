namespace VSMetadataRepositoryBrowser
{
    partial class MetadataRepositoryBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{7f5fd7aa-afb2-4bbc-8dc6-c14447ee8ef6}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{34e1603c-e7de-4a7c-ac73-f0846e0e7b44}</MetaDataID>
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
        /// <MetaDataID>{14af86fd-5f9e-4ecf-9d62-708714e58371}</MetaDataID>
        private void InitializeComponent()
        {
            ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetadataRepositoryBrowser));
            this.nodeIcon1 = new ConnectableControls.Tree.NodeControls.NodeIcon();
            this.MetaObjectsTree = new ConnectableControls.Tree.TreeView();
            this.Connection = new ConnectableControls.FormConnectionControl();
            nodeTextBox1 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
            // 
            // MetaObjectsTree
            // 
            this.MetaObjectsTree.AllowDrag = false;
            this.MetaObjectsTree.AllowDropOperationCall = ((object)(resources.GetObject("MetaObjectsTree.AllowDropOperationCall")));
            this.MetaObjectsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MetaObjectsTree.AssignPresentationObjectType = "";
            this.MetaObjectsTree.BackColor = System.Drawing.SystemColors.Window;
            this.MetaObjectsTree.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("MetaObjectsTree.BeforeShowContextMenuOperationCall")));
            this.MetaObjectsTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MetaObjectsTree.CheckUncheckPath = null;
            this.MetaObjectsTree.ConnectedObjectAutoUpdate = false;
            this.MetaObjectsTree.Cursor = System.Windows.Forms.Cursors.Default;
            this.MetaObjectsTree.CutOperationCall = ((object)(resources.GetObject("MetaObjectsTree.CutOperationCall")));
            this.MetaObjectsTree.DeleteNodeOperationCall = ((object)(resources.GetObject("MetaObjectsTree.DeleteNodeOperationCall")));
            this.MetaObjectsTree.DisplayMember = "Name";
            this.MetaObjectsTree.DragDropMarkColor = System.Drawing.Color.Blue;
            this.MetaObjectsTree.DragDropMarkWidth = 1F;
            this.MetaObjectsTree.DragDropOperationCall = ((object)(resources.GetObject("MetaObjectsTree.DragDropOperationCall")));
            this.MetaObjectsTree.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.MetaObjectsTree.EditNodeOperationCall = ((object)(resources.GetObject("MetaObjectsTree.EditNodeOperationCall")));
            // 
            // 
            // 
            this.MetaObjectsTree.EnabledProperty.Path = null;
            this.MetaObjectsTree.ImagePath = "Image";
            this.MetaObjectsTree.InsertNodeOperationCall = ((object)(resources.GetObject("MetaObjectsTree.InsertNodeOperationCall")));
            this.MetaObjectsTree.LineColor = System.Drawing.SystemColors.ControlDark;
            this.MetaObjectsTree.LoadOnDemand = true;
            this.MetaObjectsTree.Location = new System.Drawing.Point(0, 0);
            this.MetaObjectsTree.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MetaObjectsTree.MetaData = ((object)(resources.GetObject("MetaObjectsTree.MetaData")));
            this.MetaObjectsTree.Model = null;
            this.MetaObjectsTree.Name = "MetaObjectsTree";
            this.MetaObjectsTree.NodeControls.Add(this.nodeIcon1);
            this.MetaObjectsTree.NodeControls.Add(nodeTextBox1);
            this.MetaObjectsTree.NodeObjectType = "VSMetadataRepositoryBrowser.MetaObjectTreeNode";
            this.MetaObjectsTree.Path = "(ViewControlObject)";
            this.MetaObjectsTree.RecursiveLoadSteps = 0;
            this.MetaObjectsTree.RowHeight = 36D;
            this.MetaObjectsTree.SelectedNode = null;
            this.MetaObjectsTree.SelectionMember = null;
            this.MetaObjectsTree.Size = new System.Drawing.Size(432, 625);
            this.MetaObjectsTree.SubNodesProperty = "ContainedObjects";
            this.MetaObjectsTree.TabIndex = 0;
            this.MetaObjectsTree.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.MetaObjectsTree.ViewControlObject = this.Connection;
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
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectAssembly = "VSMetadataRepositoryBrowser";
            this.Connection.ViewControlObjectType = "VSMetadataRepositoryBrowser.MetaObjectTreeNode";
            // 
            // MetadataRepositoryBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(429, 622);
            this.ControlBox = false;
            this.Controls.Add(this.MetaObjectsTree);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "MetadataRepositoryBrowser";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{28c23e19-b37f-4cc7-b60c-4876628d12ab}</MetaDataID>
        /// <MetaDataID>{77cac7b3-73a9-4c67-846d-e1d79993d996}</MetaDataID>
        private ConnectableControls.Tree.TreeView MetaObjectsTree;
        /// <MetaDataID>{af65a1d6-55eb-4f4a-84b8-e8896b0c838e}</MetaDataID>
        private ConnectableControls.FormConnectionControl Connection;
        /// <MetaDataID>{00b85f04-cb08-41c5-8117-8d815da9dfd2}</MetaDataID>
        private ConnectableControls.Tree.NodeControls.NodeIcon nodeIcon1;

    }
}
