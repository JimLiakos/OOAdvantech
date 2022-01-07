namespace UserInterfaceTest
{
    partial class OrdersListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrdersListForm));
            this.ConnectionControl = new ConnectableControls.FormConnectionControl();
            this.OrdersListView = new ConnectableControls.List.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersListView)).BeginInit();
            this.SuspendLayout();
            // 
            // ConnectionControl
            // 
            this.ConnectionControl.AllowDrag = false;
            this.ConnectionControl.AllowDropOperationCall = ((object)(resources.GetObject("ConnectionControl.AllowDropOperationCall")));
            this.ConnectionControl.AssignPresentationObjectType = "";
            this.ConnectionControl.ContainerControl = this;
            this.ConnectionControl.CreatePresentationObjectAnyway = false;
            this.ConnectionControl.DragDropOperationCall = ((object)(resources.GetObject("ConnectionControl.DragDropOperationCall")));
            this.ConnectionControl.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ConnectionControl.IniateTransactionOnInstanceSet = false;
            this.ConnectionControl.MasterViewControlObject = null;
            this.ConnectionControl.Name = "ConnectionControl";
            this.ConnectionControl.RollbackOnExitWithoutAnswer = false;
            this.ConnectionControl.RollbackOnNegativeAnswer = true;
            this.ConnectionControl.SkipErrorCheck = false;
            this.ConnectionControl.TransactionObjectLockTimeOut = 0;
            this.ConnectionControl.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.ConnectionControl.ViewControlObjectAssembly = null;
            this.ConnectionControl.ViewControlObjectType = "UserInterfaceTest.OrdersPresentation";
            // 
            // OrdersListView
            // 
            this.OrdersListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrdersListView.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.OrdersListView.EnabledProperty.Path = null;
            this.OrdersListView.FocusedBackColor = System.Drawing.Color.White;
            this.OrdersListView.GridColor = System.Drawing.Color.PeachPuff;
            this.OrdersListView.GridLines = ConnectableControls.List.Models.GridLines.Rows;
            // 
            // 
            // 
            this.OrdersListView.ListConnection.AllowDrag = false;
            this.OrdersListView.ListConnection.AllowDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.AllowDropOperationCall")));
            this.OrdersListView.ListConnection.AssignPresentationObjectType = "";
            this.OrdersListView.ListConnection.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.OrdersListView.ListConnection.ConnectedObjectAutoUpdate = false;
            this.OrdersListView.ListConnection.DeleteRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.OrdersListView.ListConnection.DragDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DragDropOperationCall")));
            this.OrdersListView.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.OrdersListView.ListConnection.EditRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.EditRowOperationCall")));
            this.OrdersListView.ListConnection.InsertRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.OrdersListView.ListConnection.LoadListOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.LoadListOperationCall")));
            this.OrdersListView.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.OrdersListView.ListConnection.Path = null;
            this.OrdersListView.ListConnection.SelectionMember = null;
            this.OrdersListView.ListConnection.ViewControlObject = this.ConnectionControl;
            this.OrdersListView.Location = new System.Drawing.Point(0, 0);
            this.OrdersListView.Name = "OrdersListView";
            this.OrdersListView.RowHeight = 25;
            this.OrdersListView.Size = new System.Drawing.Size(736, 520);
            this.OrdersListView.TabIndex = 0;
            this.OrdersListView.Text = "listView1";
            // 
            // OrdersListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 520);
            this.Controls.Add(this.OrdersListView);
            this.Name = "OrdersListForm";
            this.Text = "OrdersListForm";
            ((System.ComponentModel.ISupportInitialize)(this.OrdersListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectableControls.List.ListView OrdersListView;
        public ConnectableControls.FormConnectionControl ConnectionControl;
    }
}