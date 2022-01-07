namespace UserInterfaceTest
{
    partial class ClientsList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientsList));
            this.ConnectionControl = new ConnectableControls.FormConnectionControl();
            this.ClientsListView = new ConnectableControls.List.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.ClientsListView)).BeginInit();
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
            this.ConnectionControl.MasterViewControlObject = null;
            this.ConnectionControl.Name = "ConnectionControl";
            this.ConnectionControl.RollbackOnExitWithoutAnswer = false;
            this.ConnectionControl.RollbackOnNegativeAnswer = true;
            this.ConnectionControl.SkipErrorCheck = false;
            this.ConnectionControl.TransactionObjectLockTimeOut = 0;
            this.ConnectionControl.TransactionOption = OOAdvantech.Transactions.TransactionOption.Required;
            this.ConnectionControl.ViewControlObjectAssembly = null;
            this.ConnectionControl.ViewControlObjectType = null;
            // 
            // ClientsListView
            // 
            this.ClientsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientsListView.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.ClientsListView.EnabledProperty.Path = null;
            this.ClientsListView.FocusedBackColor = System.Drawing.Color.White;
            this.ClientsListView.GridColor = System.Drawing.Color.PeachPuff;
            this.ClientsListView.GridLines = ConnectableControls.List.Models.GridLines.Rows;
            // 
            // 
            // 
            this.ClientsListView.ListConnection.AllowDrag = false;
            this.ClientsListView.ListConnection.AllowDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.AllowDropOperationCall")));
            this.ClientsListView.ListConnection.AssignPresentationObjectType = null;
            this.ClientsListView.ListConnection.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.ClientsListView.ListConnection.ConnectedObjectAutoUpdate = false;
            this.ClientsListView.ListConnection.DeleteRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.ClientsListView.ListConnection.DragDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DragDropOperationCall")));
            this.ClientsListView.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ClientsListView.ListConnection.EditRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.EditRowOperationCall")));
            this.ClientsListView.ListConnection.InsertRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.ClientsListView.ListConnection.LoadListOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.LoadListOperationCall")));
            this.ClientsListView.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.ClientsListView.ListConnection.Path = null;
            this.ClientsListView.ListConnection.SelectionMember = null;
            this.ClientsListView.ListConnection.ViewControlObject = this.ConnectionControl;
            this.ClientsListView.Location = new System.Drawing.Point(0, 0);
            this.ClientsListView.Name = "ClientsListView";
            this.ClientsListView.RowHeight = 23;
            this.ClientsListView.Size = new System.Drawing.Size(776, 445);
            this.ClientsListView.TabIndex = 0;
            this.ClientsListView.Text = "listView1";
            // 
            // ClientsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 445);
            this.Controls.Add(this.ClientsListView);
            this.Name = "ClientsList";
            this.Text = "ClientsList";
            ((System.ComponentModel.ISupportInitialize)(this.ClientsListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectableControls.FormConnectionControl ConnectionControl;
        private ConnectableControls.List.ListView ClientsListView;
    }
}