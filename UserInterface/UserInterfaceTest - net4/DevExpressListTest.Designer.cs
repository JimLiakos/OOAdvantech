namespace UserInterfaceTest
{
    partial class DevExpressListTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevExpressListTest));
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.gridControl1 = new DXConnectableControls.XtraGrid.GridControl();
            this.gridView1 = new DXConnectableControls.XtraGrid.Views.Grid.GridView();
            this.col59572368P = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDrag = true;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.MasterViewControlObject = null;
            this.Connection.MetaData = ((object)(resources.GetObject("Connection.MetaData")));
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IOrder";
            // 
            // gridControl1
            // 
            this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl1.Location = new System.Drawing.Point(12, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(604, 259);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col59572368P});
            this.gridView1.GridControl = this.gridControl1;
            // 
            // 
            // 

            this.gridView1.Name = "gridView1";
            // 
            // col59572368P
            // 
            this.col59572368P.Caption = "col59572368";
            this.col59572368P.ColumnEdit = null;
            this.col59572368P.Name = "col59572368P";
            this.col59572368P.Visible = true;
            this.col59572368P.VisibleIndex = 0;
            // 
            // DevExpressListTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 342);
            this.Controls.Add(this.gridControl1);
            this.Name = "DevExpressListTest";
            this.Text = "DevExpressListTest";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            
            this.ResumeLayout(false);

        }

        #endregion

        private DXConnectableControls.XtraGrid.GridControl gridControl1;
        private DXConnectableControls.XtraGrid.Views.Grid.GridView gridView1;
        private DXConnectableControls.XtraGrid.Columns.GridColumn col280257639p;
        private DXConnectableControls.XtraGrid.Columns.GridColumn col59572368P;
        public ConnectableControls.FormConnectionControl Connection;
    }
}