namespace UserInterfaceTest
{
    
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{82f5fb8b-0116-4344-b0bd-ea6850f300f4}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{e25fdf85-1d7b-4f82-b2a7-ec0ba047ad04}</MetaDataID>
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
        /// <MetaDataID>{4a617bb9-0958-4245-893a-7489d356b015}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.TestBtn = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.OrderDetailList = new ConnectableControls.List.ListView();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.listView1 = new ConnectableControls.List.ListView();
            this.listView2 = new ConnectableControls.List.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.OrderDetailList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listView2)).BeginInit();
            this.SuspendLayout();
            // 
            // TestBtn
            // 
            this.TestBtn.Location = new System.Drawing.Point(196, 332);
            this.TestBtn.Name = "TestBtn";
            this.TestBtn.Size = new System.Drawing.Size(75, 23);
            this.TestBtn.TabIndex = 3;
            this.TestBtn.Text = "Test";
            this.TestBtn.UseVisualStyleBackColor = true;
            this.TestBtn.Click += new System.EventHandler(this.TestBtn_Click);
            // 
            // Save
            // 
            this.Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Save.Location = new System.Drawing.Point(320, 332);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 5;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // OrderDetailList
            // 
            this.OrderDetailList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OrderDetailList.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.OrderDetailList.EnabledProperty.Path = null;
            this.OrderDetailList.FocusedBackColor = System.Drawing.Color.White;
            this.OrderDetailList.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            // 
            // 
            // 
            this.OrderDetailList.ListConnection.AllowDrag = false;
            this.OrderDetailList.ListConnection.AllowDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.AllowDropOperationCall")));
            this.OrderDetailList.ListConnection.AssignPresentationObjectType = "";
            this.OrderDetailList.ListConnection.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.OrderDetailList.ListConnection.ConnectedObjectAutoUpdate = false;
            this.OrderDetailList.ListConnection.DeleteMenuCommand = null;
            this.OrderDetailList.ListConnection.DeleteRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.OrderDetailList.ListConnection.DragDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DragDropOperationCall")));
            this.OrderDetailList.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.OrderDetailList.ListConnection.EditMenuCommand = null;
            this.OrderDetailList.ListConnection.EditRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.EditRowOperationCall")));
            this.OrderDetailList.ListConnection.InsertMenuCommand = null;
            this.OrderDetailList.ListConnection.InsertRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.OrderDetailList.ListConnection.LoadListOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.LoadListOperationCall")));
            this.OrderDetailList.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.OrderDetailList.ListConnection.Path = null;
            this.OrderDetailList.ListConnection.SelectionMember = null;
            this.OrderDetailList.ListConnection.ViewControlObject = null;
            this.OrderDetailList.Location = new System.Drawing.Point(21, 40);
            this.OrderDetailList.MultiSelect = true;
            this.OrderDetailList.Name = "OrderDetailList";
            this.OrderDetailList.RowHeight = 23;
            this.OrderDetailList.Size = new System.Drawing.Size(219, 264);
            this.OrderDetailList.TabIndex = 2;
            this.OrderDetailList.Text = "table1";
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
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Required;
            this.Connection.ViewControlObjectAssembly = null;
            this.Connection.ViewControlObjectType = "UserInterfaceTest.Company";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.listView1.EnabledProperty.Path = null;
            this.listView1.FocusedBackColor = System.Drawing.Color.White;
            this.listView1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            // 
            // 
            // 
            this.listView1.ListConnection.AllowDrag = false;
            this.listView1.ListConnection.AllowDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.AllowDropOperationCall2")));
            this.listView1.ListConnection.AssignPresentationObjectType = "";
            this.listView1.ListConnection.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.BeforeShowContextMenuOperationCall2")));
            this.listView1.ListConnection.ConnectedObjectAutoUpdate = false;
            this.listView1.ListConnection.DeleteRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall2")));
            this.listView1.ListConnection.DragDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DragDropOperationCall2")));
            this.listView1.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.listView1.ListConnection.EditRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.EditRowOperationCall2")));
            this.listView1.ListConnection.InsertRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall2")));
            this.listView1.ListConnection.LoadListOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.LoadListOperationCall2")));
            this.listView1.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData2")));
            this.listView1.ListConnection.Path = null;
            this.listView1.ListConnection.SelectionMember = null;
            this.listView1.ListConnection.ViewControlObject = this.Connection;
            this.listView1.Location = new System.Drawing.Point(246, 40);
            this.listView1.MultiSelect = true;
            this.listView1.Name = "listView1";
            this.listView1.RowHeight = 23;
            this.listView1.Size = new System.Drawing.Size(213, 264);
            this.listView1.TabIndex = 6;
            this.listView1.Text = "table1";
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.listView2.EnabledProperty.Path = null;
            this.listView2.FocusedBackColor = System.Drawing.Color.White;
            this.listView2.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            // 
            // 
            // 
            this.listView2.ListConnection.AllowDrag = false;
            this.listView2.ListConnection.AllowDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.AllowDropOperationCall1")));
            this.listView2.ListConnection.AssignPresentationObjectType = "";
            this.listView2.ListConnection.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.BeforeShowContextMenuOperationCall1")));
            this.listView2.ListConnection.ConnectedObjectAutoUpdate = false;
            this.listView2.ListConnection.DeleteRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall1")));
            this.listView2.ListConnection.DragDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DragDropOperationCall1")));
            this.listView2.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.listView2.ListConnection.EditRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.EditRowOperationCall1")));
            this.listView2.ListConnection.InsertRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall1")));
            this.listView2.ListConnection.LoadListOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.LoadListOperationCall1")));
            this.listView2.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData1")));
            this.listView2.ListConnection.Path = null;
            this.listView2.ListConnection.SelectionMember = null;
            this.listView2.ListConnection.ViewControlObject = this.Connection;
            this.listView2.Location = new System.Drawing.Point(465, 40);
            this.listView2.MultiSelect = true;
            this.listView2.Name = "listView2";
            this.listView2.RowHeight = 23;
            this.listView2.Size = new System.Drawing.Size(233, 264);
            this.listView2.TabIndex = 7;
            this.listView2.Text = "table1";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 403);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.TestBtn);
            this.Controls.Add(this.OrderDetailList);
            this.Name = "Form3";
            this.Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)(this.OrderDetailList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{7c49f7b2-bb27-451f-a4a9-448d961ab9b2}</MetaDataID>
        private ConnectableControls.List.ListView OrderDetailList;
        /// <MetaDataID>{233e6e5d-d2df-41d8-abb4-8d5a1ab4c3d2}</MetaDataID>
        private System.Windows.Forms.Button TestBtn;
        /// <MetaDataID>{44345962-5bc2-40ab-8209-cb3f648627be}</MetaDataID>
        private System.Windows.Forms.Button Save;
        /// <MetaDataID>{23937254-cede-40dd-ae07-ec354ca5a3d2}</MetaDataID>
        public ConnectableControls.FormConnectionControl Connection;
        /// <MetaDataID>{6bcce76c-56b9-4d3c-922e-89e7cc4d6432}</MetaDataID>
        private ConnectableControls.List.ListView listView1;
        /// <MetaDataID>{05f6a7a0-3051-4f1f-9d80-bdeea23888b0}</MetaDataID>
        private ConnectableControls.List.ListView listView2;

    }
}