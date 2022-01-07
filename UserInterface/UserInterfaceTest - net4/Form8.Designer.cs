namespace UserInterfaceTest
{
    partial class Form8
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
            ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form8));
            ConnectableControls.Tree.TreeColumn treeColumn1 = new ConnectableControls.Tree.TreeColumn();
            ConnectableControls.Tree.TreeColumn treeColumn2 = new ConnectableControls.Tree.TreeColumn();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.treeView1 = new ConnectableControls.Tree.TreeView();
            this.nodeTextBox1 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            this.nodeTextBox2 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            nodeTextBox3 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            this.SuspendLayout();
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
            this.Connection.ViewControlObjectAssembly = null;
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IOrder";
            // 
            // treeView1
            // 
            this.treeView1.AllowDrag = false;
            this.treeView1.AllowDropOperationCall = ((object)(resources.GetObject("treeView1.AllowDropOperationCall")));
            this.treeView1.AssignPresentationObjectType = null;
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("treeView1.BeforeShowContextMenuOperationCall")));
            this.treeView1.CheckUncheckPath = null;
            treeColumn1.Header = "Test1";
            treeColumn2.Header = "Test2";
            this.treeView1.Columns.Add(treeColumn1);
            this.treeView1.Columns.Add(treeColumn2);
            this.treeView1.ConnectedObjectAutoUpdate = false;
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView1.CutOperationCall = ((object)(resources.GetObject("treeView1.CutOperationCall")));
            this.treeView1.DeleteNodeOperationCall = ((object)(resources.GetObject("treeView1.DeleteNodeOperationCall")));
            this.treeView1.DisplayMember = null;
            this.treeView1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView1.DragDropMarkWidth = 1F;
            this.treeView1.DragDropOperationCall = ((object)(resources.GetObject("treeView1.DragDropOperationCall")));
            this.treeView1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.treeView1.EditNodeOperationCall = ((object)(resources.GetObject("treeView1.EditNodeOperationCall")));
            // 
            // 
            // 
            this.treeView1.EnabledProperty.Path = null;
            this.treeView1.ImagePath = null;
            this.treeView1.InsertNodeOperationCall = ((object)(resources.GetObject("treeView1.InsertNodeOperationCall")));
            this.treeView1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView1.LoadOnDemand = true;
            this.treeView1.Location = new System.Drawing.Point(26, 12);
            this.treeView1.MetaData = ((object)(resources.GetObject("treeView1.MetaData")));
            this.treeView1.Model = null;
            this.treeView1.Name = "treeView1";
            this.treeView1.NodeControls.Add(this.nodeTextBox1);
            this.treeView1.NodeControls.Add(this.nodeTextBox2);
            this.treeView1.NodeControls.Add(nodeTextBox3);
            this.treeView1.NodeObjectType = "AbstractionsAndPersistency.INodeObject";
            this.treeView1.Path = "CDriveFolder";
            this.treeView1.RecursiveLoadSteps = 0;
            this.treeView1.RowHeight = 16D;
            this.treeView1.SelectedNode = null;
            this.treeView1.SelectionMember = null;
            this.treeView1.Size = new System.Drawing.Size(237, 316);
            this.treeView1.SubNodesProperty = "SubNodeObjects";
            this.treeView1.TabIndex = 0;
            this.treeView1.Text = "treeView1";
            this.treeView1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.treeView1.UseColumns = true;
            this.treeView1.ViewControlObject = this.Connection;
            // 
            // Form8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 368);
            this.Controls.Add(this.treeView1);
            this.Name = "Form8";
            this.Text = "Form8";
            this.Load += new System.EventHandler(this.Form8_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public ConnectableControls.FormConnectionControl Connection;
        private ConnectableControls.Tree.TreeView treeView1;
        private ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox2;
    }
}