namespace ConnectableControls.PropertyEditors
{
    partial class ReportDataSourceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportDataSourceForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new ConnectableControls.Tree.TreeView();
            this.nodeIcon1 = new ConnectableControls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox2 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.propertyGrid = new ConnectableControls.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(444, 338);
            this.splitContainer1.SplitterDistance = 153;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.AllowDrag = false;
            this.treeView1.AllowDropOperationCall = ((object)(resources.GetObject("treeView1.AllowDropOperationCall")));
            this.treeView1.AssignPresentationObjectType = "";
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("treeView1.BeforeShowContextMenuOperationCall")));
            this.treeView1.CheckUncheckPath = "";
            this.treeView1.ConnectedObjectAutoUpdate = false;
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView1.CutOperationCall = ((object)(resources.GetObject("treeView1.CutOperationCall")));
            this.treeView1.DeleteNodeOperationCall = ((object)(resources.GetObject("treeView1.DeleteNodeOperationCall")));
            this.treeView1.DisplayMember = "Name";
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.DragDropMarkColor = System.Drawing.Color.Blue;
            this.treeView1.DragDropMarkWidth = 1F;
            this.treeView1.DragDropOperationCall = ((object)(resources.GetObject("treeView1.DragDropOperationCall")));
            this.treeView1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.treeView1.EditNodeOperationCall = ((object)(resources.GetObject("treeView1.EditNodeOperationCall")));
            // 
            // 
            // 
            this.treeView1.EnabledProperty.Path = null;
            this.treeView1.ImagePath = "Image";
            this.treeView1.InsertNodeOperationCall = ((object)(resources.GetObject("treeView1.InsertNodeOperationCall")));
            this.treeView1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView1.LoadOnDemand = true;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.MetaData = ((object)(resources.GetObject("treeView1.MetaData")));
            this.treeView1.Model = null;
            this.treeView1.Name = "treeView1";
            this.treeView1.NodeControls.Add(this.nodeIcon1);
            this.treeView1.NodeControls.Add(this.nodeTextBox2);
            this.treeView1.NodeObjectType = "ConnectableControls.PropertyEditors.ReportDataNode";
            this.treeView1.Path = "ReportDataSourceAsNode";
            this.treeView1.RecursiveLoadSteps = 1;
            this.treeView1.RowHeight = 16D;
            this.treeView1.SelectedNode = null;
            this.treeView1.SelectionMember = "SelectedNode";
            this.treeView1.Size = new System.Drawing.Size(153, 338);
            this.treeView1.SubNodesProperty = "SubReportDataNodes";
            this.treeView1.TabIndex = 0;
            this.treeView1.Text = "treeView1";
            this.treeView1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.treeView1.ViewControlObject = this.Connection;
            // 
            // nodeTextBox2
            // 
            this.nodeTextBox2.EditEnabled = true;
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "ConnectableControls.PropertyEditors.ReportDataSourcePrecentation";
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
            this.Connection.ViewControlObjectType = "OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource";
            // 
            // propertyGrid
            // 
            this.propertyGrid.AllowDrag = false;
            this.propertyGrid.AutoDisable = true;
            this.propertyGrid.ConnectedObjectAutoUpdate = false;
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Path = "SelectedNode";
            this.propertyGrid.Size = new System.Drawing.Size(287, 338);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.propertyGrid.ViewControlObject = this.Connection;
            this.propertyGrid.Click += new System.EventHandler(this.propertyGrid_Click);
            // 
            // ReportDataSourceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(444, 338);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ReportDataSourceForm";
            this.Text = "Report Data Source";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectableControls.Tree.TreeView treeView1;
        public FormConnectionControl Connection;
        private ConnectableControls.Tree.NodeControls.NodeIcon nodeIcon1;
        private ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox2;
        private PropertyGrid propertyGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}