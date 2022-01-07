namespace UserInterfaceTest
{
    partial class DevExpressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{7837eece-2e80-431b-80fc-31538793fec7}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{8ac24408-9902-49b1-9a88-3ed99ed2b785}</MetaDataID>
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
        /// <MetaDataID>{be49d7f6-16ba-4208-abaf-9961a843cd7c}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevExpressForm));
            this.ProductPriceLookUpEdit = new DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.gridControl1 = new DXConnectableControls.XtraGrid.GridControl();
            this.bandedGridView1 = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.ItemNameSecP = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Number = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.Quantity = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.ProductPrice = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.col54333736 = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn1 = new DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.SaveBtn = new ConnectableControls.Button();
            this.Test = new ConnectableControls.Button();
            this.col14284947 = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.col5111875 = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.button1 = new ConnectableControls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ProductPriceLookUpEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ProductPriceLookUpEdit
            // 
            this.ProductPriceLookUpEdit.AutoHeight = false;
            this.ProductPriceLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ProductPriceLookUpEdit.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name")});
            this.ProductPriceLookUpEdit.DisplayMember = "Name";
            this.ProductPriceLookUpEdit.Name = "ProductPriceLookUpEdit";
            this.ProductPriceLookUpEdit.ViewControlObject = this.Connection;
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "UserInterfaceTest.OrderPresentationObject";
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
            this.Connection.ViewControlObjectAssembly = "AbstractionsAndPersistency";
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IOrder";
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.bandedGridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ProductPriceLookUpEdit});
            this.gridControl1.Size = new System.Drawing.Size(732, 304);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.ItemNameSecP,
            this.Number,
            this.Quantity,
            this.ProductPrice});
            this.bandedGridView1.GridControl = this.gridControl1;
            // 
            // 
            // 
            this.bandedGridView1.ListConnection.AllowDrag = false;
            this.bandedGridView1.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.bandedGridView1.ListConnection.AssignPresentationObjectType = "";
            this.bandedGridView1.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.bandedGridView1.ListConnection.ConnectedObjectAutoUpdate = false;
            this.bandedGridView1.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.bandedGridView1.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.bandedGridView1.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.bandedGridView1.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.bandedGridView1.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.bandedGridView1.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.bandedGridView1.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.bandedGridView1.ListConnection.Path = "OrderDetails";
            this.bandedGridView1.ListConnection.SelectionMember = "SelOrderDetail";
            this.bandedGridView1.ListConnection.ViewControlObject = this.Connection;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.Click;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Columns.Add(this.ItemNameSecP);
            this.gridBand1.Columns.Add(this.Number);
            this.gridBand1.Columns.Add(this.Quantity);
            this.gridBand1.Columns.Add(this.ProductPrice);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.Width = 300;
            // 
            // ItemNameSecP
            // 
            this.ItemNameSecP.Caption = "Name";
            this.ItemNameSecP.ColumnEdit = null;
            this.ItemNameSecP.Name = "ItemNameSecP";
            this.ItemNameSecP.Visible = true;
            // 
            // Number
            // 
            this.Number.Caption = "Number";
            this.Number.ColumnEdit = null;
            this.Number.Name = "Number";
            this.Number.Visible = true;
            // 
            // Quantity
            // 
            this.Quantity.Caption = "Quantity";
            this.Quantity.ColumnEdit = null;
            this.Quantity.Name = "Quantity";
            this.Quantity.Visible = true;
            // 
            // ProductPrice
            // 
            this.ProductPrice.Caption = "ProductPrice";
            this.ProductPrice.ColumnEdit = this.ProductPriceLookUpEdit;
            this.ProductPrice.Name = "ProductPrice";
            this.ProductPrice.Visible = true;
            // 
            // col54333736
            // 
            this.col54333736.ColumnEdit = null;
            this.col54333736.Name = "col54333736";
            this.col54333736.Visible = true;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.ColumnEdit = null;
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.Visible = true;
            // 
            // SaveBtn
            // 
            this.SaveBtn.AllowDrag = false;
            this.SaveBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SaveBtn.ConnectedObjectAutoUpdate = false;
            this.SaveBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SaveBtn.Location = new System.Drawing.Point(329, 508);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.OnClickOperationCall = ((object)(resources.GetObject("SaveBtn.OnClickOperationCall")));
            this.SaveBtn.Path = "";
            this.SaveBtn.SaveButton = false;
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 1;
            this.SaveBtn.Text = "Save";
            // 
            // 
            // 
            this.SaveBtn.TextProperty.Path = null;
            this.SaveBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Value = null;
            this.SaveBtn.ViewControlObject = this.Connection;
            // 
            // Test
            // 
            this.Test.AllowDrag = false;
            this.Test.ConnectedObjectAutoUpdate = false;
            this.Test.Location = new System.Drawing.Point(63, 508);
            this.Test.Name = "Test";
            this.Test.OnClickOperationCall = ((object)(resources.GetObject("Test.OnClickOperationCall")));
            this.Test.Path = "";
            this.Test.SaveButton = false;
            this.Test.Size = new System.Drawing.Size(75, 23);
            this.Test.TabIndex = 2;
            this.Test.Text = "Test";
            // 
            // 
            // 
            this.Test.TextProperty.Path = null;
            this.Test.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Value = null;
            this.Test.ViewControlObject = this.Connection;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // col14284947
            // 
            this.col14284947.ColumnEdit = null;
            this.col14284947.Name = "col14284947";
            this.col14284947.UserInterfaceObjectConnection = null;
            this.col14284947.Visible = true;
            this.col14284947.VisibleIndex = 0;
            // 
            // col5111875
            // 
            this.col5111875.ColumnEdit = null;
            this.col5111875.Name = "col5111875";
            this.col5111875.UserInterfaceObjectConnection = null;
            this.col5111875.Visible = true;
            this.col5111875.VisibleIndex = 1;
            // 
            // button1
            // 
            this.button1.AllowDrag = false;
            this.button1.ConnectedObjectAutoUpdate = false;
            this.button1.Location = new System.Drawing.Point(62, 351);
            this.button1.Name = "button1";
            this.button1.OnClickOperationCall = ((object)(resources.GetObject("button1.OnClickOperationCall")));
            this.button1.Path = "";
            this.button1.SaveButton = false;
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            // 
            // 
            // 
            this.button1.TextProperty.Path = null;
            this.button1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Value = null;
            this.button1.ViewControlObject = this.Connection;
            // 
            // DevExpressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 543);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.gridControl1);
            this.Name = "DevExpressForm";
            this.Text = "DevExpressForm";
            this.Load += new System.EventHandler(this.DevExpressForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProductPriceLookUpEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{77cbb95f-c4ef-492a-9aee-36295179560f}</MetaDataID>
        private DXConnectableControls.XtraGrid.GridControl gridControl1;
        /// <MetaDataID>{1b1aa06b-440d-4e71-8356-339b5f778b84}</MetaDataID>
        public ConnectableControls.FormConnectionControl Connection;
        /// <MetaDataID>{d7fa5d66-2d00-484e-b945-3a36a98f5506}</MetaDataID>
        private ConnectableControls.Button SaveBtn;
        /// <MetaDataID>{d366550e-5563-4a2f-a6a0-6e08d11924c2}</MetaDataID>
        private ConnectableControls.Button Test;
        /// <MetaDataID>{a63b16dc-cb78-43b0-beb2-3a48d1ebc36f}</MetaDataID>
        private DXConnectableControls.XtraEditors.Repository.CCRepositoryItemLookUpEdit ProductPriceLookUpEdit;
        /// <MetaDataID>{5446cfa7-38eb-47ca-82a3-0a21327a8a75}</MetaDataID>
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        /// <MetaDataID>{cd8f43b3-99cd-46a4-9071-ea0d05e58767}</MetaDataID>
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        /// <MetaDataID>{7019a5d6-74ea-44a2-8b17-e5bae9be1323}</MetaDataID>
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn ItemNameSecP;
        /// <MetaDataID>{27a40af7-0f67-4bce-a17a-53f41e684376}</MetaDataID>
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn Number;
        /// <MetaDataID>{2da73ab4-53b6-4725-a803-3bd5f62d910f}</MetaDataID>
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn Quantity;
        /// <MetaDataID>{ea8aa5ee-2f27-49d5-b69c-851c3af38f19}</MetaDataID>
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn ProductPrice;
        /// <MetaDataID>{c55d5570-1e95-41d4-b879-47663738c0c9}</MetaDataID>
        private DXConnectableControls.XtraGrid.Columns.GridColumn col14284947;
        /// <MetaDataID>{ddff66a7-58ab-4187-8e26-e230f0a6b2e0}</MetaDataID>
        private DXConnectableControls.XtraGrid.Columns.GridColumn col5111875;
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn col54333736;
        private DXConnectableControls.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private ConnectableControls.Button button1;
        
      
 
    }
}