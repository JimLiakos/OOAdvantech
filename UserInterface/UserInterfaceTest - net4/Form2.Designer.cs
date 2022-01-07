namespace UserInterfaceTest
{
    partial class Form2
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
            DXConnectableControls.XtraGrid.Views.Grid.GridView OrderItemsList;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.OrderItemsList_Description = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.OrderItemsList_Amount = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.OrderItemsList_Invoiced = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.OrderItemsList_Product = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.OrderItemsList_Client = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.OrderItemsList_Price = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.OrderItemsList_QAmount = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.Date = new DXConnectableControls.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DXConnectableControls.XtraGrid.GridControl();
            this.ViewControlObject = new ConnectableControls.FormConnectionControl();
            this.comboBox1 = new ConnectableControls.ComboBox();
            this.Invoiced = new ConnectableControls.CheckBox();
            this.dateTimePicker1Au = new ConnectableControls.DateTimePicker();
            this.button1 = new ConnectableControls.Button();
            this.Client = new ConnectableControls.SearchTextBox();
            this.textBox1 = new ConnectableControls.TextBox();
            this.PriceList = new ConnectableControls.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.SearchOrder = new ConnectableControls.Button();
            this.button2 = new ConnectableControls.Button();
            this.Amount = new System.Windows.Forms.Label();
            this.textBox2 = new ConnectableControls.TextBox();
            this.ClientB = new ConnectableControls.TextBox();
            this.comboBox2 = new ConnectableControls.ComboBox();
            this.treeView1 = new ConnectableControls.Tree.TreeView();
            this.nodeTextBox1 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            this.dynamicViewContainer1 = new ConnectableControls.DynamicViewContainer();
            this.OrderState = new ConnectableControls.ComboBox();
            OrderItemsList = new DXConnectableControls.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(OrderItemsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OrderItemsList
            // 
            OrderItemsList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.OrderItemsList_Description,
            this.OrderItemsList_Amount,
            this.OrderItemsList_Invoiced,
            this.OrderItemsList_Product,
            this.OrderItemsList_Client,
            this.OrderItemsList_Price,
            this.OrderItemsList_QAmount,
            this.Date});
            OrderItemsList.GridControl = this.gridControl1;
            // 
            // 
            // 
            OrderItemsList.ListConnection.AllowDrag = false;
            OrderItemsList.ListConnection.AllowDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.AllowDropOperationCall")));
            OrderItemsList.ListConnection.AssignPresentationObjectType = "UserInterfaceTest.OrderDetailPresentationObject";
            OrderItemsList.ListConnection.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            OrderItemsList.ListConnection.ConnectedObjectAutoUpdate = false;
            OrderItemsList.ListConnection.DeleteRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            OrderItemsList.ListConnection.DragDropOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.DragDropOperationCall")));
            OrderItemsList.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            OrderItemsList.ListConnection.EditRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.EditRowOperationCall")));
            OrderItemsList.ListConnection.InsertRowOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            OrderItemsList.ListConnection.LoadListOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("resource.LoadListOperationCall")));
            OrderItemsList.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            OrderItemsList.ListConnection.Path = "RealObject.OrderDetails";
            OrderItemsList.ListConnection.SelectionMember = "SelOrderDetail";
            OrderItemsList.ListConnection.ViewControlObject = this.ViewControlObject;
            OrderItemsList.Name = "OrderItemsList";
            // 
            // OrderItemsList_Description
            // 
            resources.ApplyResources(this.OrderItemsList_Description, "OrderItemsList_Description");
            this.OrderItemsList_Description.ColumnEdit = null;
            this.OrderItemsList_Description.Name = "OrderItemsList_Description";
            // 
            // OrderItemsList_Amount
            // 
            resources.ApplyResources(this.OrderItemsList_Amount, "OrderItemsList_Amount");
            this.OrderItemsList_Amount.ColumnEdit = null;
            this.OrderItemsList_Amount.Name = "OrderItemsList_Amount";
            // 
            // OrderItemsList_Invoiced
            // 
            resources.ApplyResources(this.OrderItemsList_Invoiced, "OrderItemsList_Invoiced");
            this.OrderItemsList_Invoiced.ColumnEdit = null;
            this.OrderItemsList_Invoiced.Name = "OrderItemsList_Invoiced";
            // 
            // OrderItemsList_Product
            // 
            resources.ApplyResources(this.OrderItemsList_Product, "OrderItemsList_Product");
            this.OrderItemsList_Product.ColumnEdit = null;
            this.OrderItemsList_Product.Name = "OrderItemsList_Product";
            // 
            // OrderItemsList_Client
            // 
            resources.ApplyResources(this.OrderItemsList_Client, "OrderItemsList_Client");
            this.OrderItemsList_Client.ColumnEdit = null;
            this.OrderItemsList_Client.Name = "OrderItemsList_Client";
            // 
            // OrderItemsList_Price
            // 
            resources.ApplyResources(this.OrderItemsList_Price, "OrderItemsList_Price");
            this.OrderItemsList_Price.ColumnEdit = null;
            this.OrderItemsList_Price.Name = "OrderItemsList_Price";
            // 
            // OrderItemsList_QAmount
            // 
            resources.ApplyResources(this.OrderItemsList_QAmount, "OrderItemsList_QAmount");
            this.OrderItemsList_QAmount.ColumnEdit = null;
            this.OrderItemsList_QAmount.Name = "OrderItemsList_QAmount";
            // 
            // Date
            // 
            resources.ApplyResources(this.Date, "Date");
            this.Date.ColumnEdit = null;
            this.Date.Name = "Date";
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip = ((DevExpress.Utils.DefaultBoolean)(resources.GetObject("gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip")));
            this.gridControl1.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("gridControl1.EmbeddedNavigator.Anchor")));
            this.gridControl1.EmbeddedNavigator.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("gridControl1.EmbeddedNavigator.BackgroundImageLayout")));
            this.gridControl1.EmbeddedNavigator.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("gridControl1.EmbeddedNavigator.ImeMode")));
            this.gridControl1.EmbeddedNavigator.TextLocation = ((DevExpress.XtraEditors.NavigatorButtonsTextLocation)(resources.GetObject("gridControl1.EmbeddedNavigator.TextLocation")));
            this.gridControl1.EmbeddedNavigator.ToolTipIconType = ((DevExpress.Utils.ToolTipIconType)(resources.GetObject("gridControl1.EmbeddedNavigator.ToolTipIconType")));
            resources.ApplyResources(this.gridControl1, "gridControl1");
            this.gridControl1.MainView = OrderItemsList;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            OrderItemsList});
            // 
            // ViewControlObject
            // 
            this.ViewControlObject.AllowDrag = false;
            this.ViewControlObject.AllowDropOperationCall = ((object)(resources.GetObject("ViewControlObject.AllowDropOperationCall")));
            this.ViewControlObject.AssignPresentationObjectType = "UserInterfaceTest.OrderPresentationObject";
            this.ViewControlObject.ContainerControl = this;
            this.ViewControlObject.CreatePresentationObjectAnyway = false;
            this.ViewControlObject.DragDropOperationCall = ((object)(resources.GetObject("ViewControlObject.DragDropOperationCall")));
            this.ViewControlObject.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ViewControlObject.IniateTransactionOnInstanceSet = false;
            this.ViewControlObject.MasterViewControlObject = null;
            this.ViewControlObject.Name = "ViewControlObject";
            this.ViewControlObject.RollbackOnExitWithoutAnswer = false;
            this.ViewControlObject.RollbackOnNegativeAnswer = true;
            this.ViewControlObject.SkipErrorCheck = false;
            this.ViewControlObject.TransactionObjectLockTimeOut = 0;
            this.ViewControlObject.TransactionOption = OOAdvantech.Transactions.TransactionOption.Required;
            this.ViewControlObject.ViewControlObjectAssembly = "AbstractionsAndPersistency";
            this.ViewControlObject.ViewControlObjectType = "AbstractionsAndPersistency.IOrder";
            // 
            // comboBox1
            // 
            this.comboBox1.AllowDrag = false;
            this.comboBox1.AllowDropOperationCall = ((object)(resources.GetObject("comboBox1.AllowDropOperationCall")));
            this.comboBox1.AssignPresentationObjectType = "";
            this.comboBox1.AutoDisable = true;
            this.comboBox1.AutoInsert = true;
            this.comboBox1.AutoSuggest = true;
            this.comboBox1.ChooseFromEnum = false;
            this.comboBox1.ConnectedObjectAutoUpdate = false;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.DragDropOperationCall = ((object)(resources.GetObject("comboBox1.DragDropOperationCall")));
            this.comboBox1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox1.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox1.EnableProperty.Path = null;
            this.comboBox1.Enumeration = "";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.InsertOperationCall = ((object)(resources.GetObject("comboBox1.InsertOperationCall")));
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.NullValueName = "";
            this.comboBox1.OperationCall = ((object)(resources.GetObject("comboBox1.OperationCall")));
            this.comboBox1.Path = "RealObject.Client";
            this.comboBox1.PreLoaded = true;
            this.comboBox1.RemoveOperationCall = ((object)(resources.GetObject("comboBox1.RemoveOperationCall")));
            this.comboBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox1.ViewControlObject = this.ViewControlObject;
            this.comboBox1.WarnigMessageOnRemove = "Θα το σβήσω";
            // 
            // Invoiced
            // 
            this.Invoiced.AllowDrag = false;
            this.Invoiced.AutoDisable = true;
            resources.ApplyResources(this.Invoiced, "Invoiced");
            this.Invoiced.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.Invoiced.EnableProperty.Path = null;
            this.Invoiced.Name = "Invoiced";
            this.Invoiced.Path = "Invoiced";
            this.Invoiced.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Invoiced.UseVisualStyleBackColor = true;
            this.Invoiced.ViewControlObject = this.ViewControlObject;
            // 
            // dateTimePicker1Au
            // 
            this.dateTimePicker1Au.AllowDrag = false;
            this.dateTimePicker1Au.AutoDisable = true;
            this.dateTimePicker1Au.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.dateTimePicker1Au.EnableProperty.Path = null;
            resources.ApplyResources(this.dateTimePicker1Au, "dateTimePicker1Au");
            this.dateTimePicker1Au.Name = "dateTimePicker1Au";
            this.dateTimePicker1Au.Path = "RealObject.OrderDate";
            this.dateTimePicker1Au.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.dateTimePicker1Au.ViewControlObject = this.ViewControlObject;
            // 
            // button1
            // 
            this.button1.AllowDrag = false;
            this.button1.ConnectedObjectAutoUpdate = false;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.OnClickOperationCall = ((object)(resources.GetObject("button1.OnClickOperationCall")));
            this.button1.Path = "";
            this.button1.SaveButton = true;
            this.button1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Value = null;
            this.button1.ViewControlObject = this.ViewControlObject;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Client
            // 
            this.Client.AllowDrag = false;
            this.Client.AllowDrop = true;
            this.Client.AllowDropOperationCall = ((object)(resources.GetObject("Client.AllowDropOperationCall")));
            this.Client.AssignPresentationObjectType = "";
            this.Client.AutoDisable = true;
            this.Client.ConnectedObjectAutoUpdate = false;
            this.Client.DisplayMember = "Name";
            this.Client.DragDropOperationCall = ((object)(resources.GetObject("Client.DragDropOperationCall")));
            this.Client.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Client.DropDownHeight = 106;
            // 
            // 
            // 
            this.Client.EnableProperty.Path = null;
            this.Client.InsertOperationCall = ((object)(resources.GetObject("Client.InsertOperationCall")));
            resources.ApplyResources(this.Client, "Client");
            this.Client.Name = "Client";
            this.Client.OperationCall = ((object)(resources.GetObject("Client.OperationCall")));
            this.Client.Path = "RealObject.Client";
            this.Client.RemoveOperationCall = ((object)(resources.GetObject("Client.RemoveOperationCall")));
            this.Client.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Client.ViewControlObject = this.ViewControlObject;
            // 
            // textBox1
            // 
            this.textBox1.AllowDrag = false;
            this.textBox1.AutoDisable = true;
            this.textBox1.ConnectedObjectAutoUpdate = false;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.textBox1.Path = "RealObject.Client.Name";
            this.textBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.textBox1.ViewControlObject = this.ViewControlObject;
            // 
            // PriceList
            // 
            this.PriceList.AllowDrag = false;
            this.PriceList.AllowDropOperationCall = ((object)(resources.GetObject("PriceList.AllowDropOperationCall")));
            this.PriceList.AssignPresentationObjectType = "";
            this.PriceList.AutoDisable = true;
            this.PriceList.AutoInsert = false;
            this.PriceList.AutoSuggest = false;
            this.PriceList.ChooseFromEnum = false;
            this.PriceList.ConnectedObjectAutoUpdate = false;
            this.PriceList.DisplayMember = "Name";
            this.PriceList.DragDropOperationCall = ((object)(resources.GetObject("PriceList.DragDropOperationCall")));
            this.PriceList.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.PriceList.EnableCondition = null;
            // 
            // 
            // 
            this.PriceList.EnableProperty.Path = null;
            this.PriceList.Enumeration = "";
            this.PriceList.FormattingEnabled = true;
            this.PriceList.InsertOperationCall = ((object)(resources.GetObject("PriceList.InsertOperationCall")));
            resources.ApplyResources(this.PriceList, "PriceList");
            this.PriceList.Name = "PriceList";
            this.PriceList.NullValueName = "";
            this.PriceList.OperationCall = ((object)(resources.GetObject("PriceList.OperationCall")));
            this.PriceList.Path = "PriceList";
            this.PriceList.PreLoaded = true;
            this.PriceList.RemoveOperationCall = ((object)(resources.GetObject("PriceList.RemoveOperationCall")));
            this.PriceList.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PriceList.ViewControlObject = this.ViewControlObject;
            this.PriceList.WarnigMessageOnRemove = null;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.Client, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // SearchOrder
            // 
            this.SearchOrder.AllowDrag = false;
            this.SearchOrder.ConnectedObjectAutoUpdate = false;
            resources.ApplyResources(this.SearchOrder, "SearchOrder");
            this.SearchOrder.Name = "SearchOrder";
            this.SearchOrder.OnClickOperationCall = ((object)(resources.GetObject("SearchOrder.OnClickOperationCall")));
            this.SearchOrder.Path = "";
            this.SearchOrder.SaveButton = false;
            this.SearchOrder.TransactionOption = OOAdvantech.Transactions.TransactionOption.RequiresNew;
            this.SearchOrder.UseVisualStyleBackColor = true;
            this.SearchOrder.Value = null;
            this.SearchOrder.ViewControlObject = this.ViewControlObject;
            // 
            // button2
            // 
            this.button2.AllowDrag = false;
            this.button2.ConnectedObjectAutoUpdate = false;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.OnClickOperationCall = ((object)(resources.GetObject("button2.OnClickOperationCall")));
            this.button2.Path = "";
            this.button2.SaveButton = false;
            this.button2.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Value = null;
            this.button2.ViewControlObject = this.ViewControlObject;
            // 
            // Amount
            // 
            resources.ApplyResources(this.Amount, "Amount");
            this.Amount.Name = "Amount";
            // 
            // textBox2
            // 
            this.textBox2.AllowDrag = false;
            this.textBox2.AutoDisable = true;
            this.textBox2.ConnectedObjectAutoUpdate = false;
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            this.textBox2.Path = "RealObject.ItemsNumber";
            this.textBox2.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.textBox2.ViewControlObject = this.ViewControlObject;
            // 
            // ClientB
            // 
            this.ClientB.AcceptsTab = true;
            this.ClientB.AllowDrag = false;
            this.ClientB.AutoDisable = true;
            this.ClientB.ConnectedObjectAutoUpdate = false;
            resources.ApplyResources(this.ClientB, "ClientB");
            this.ClientB.Name = "ClientB";
            this.ClientB.Path = "RealObject.Client.Name";
            this.ClientB.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ClientB.ViewControlObject = this.ViewControlObject;
            // 
            // comboBox2
            // 
            this.comboBox2.AllowDrag = false;
            this.comboBox2.AllowDrop = true;
            this.comboBox2.AllowDropOperationCall = ((object)(resources.GetObject("comboBox2.AllowDropOperationCall")));
            this.comboBox2.AssignPresentationObjectType = "UserInterfaceTest.ClientPresentationObject";
            this.comboBox2.AutoDisable = false;
            this.comboBox2.AutoInsert = false;
            this.comboBox2.AutoSuggest = false;
            this.comboBox2.ChooseFromEnum = false;
            this.comboBox2.ConnectedObjectAutoUpdate = false;
            this.comboBox2.DisplayMember = "RealObject.Name";
            this.comboBox2.DragDropOperationCall = ((object)(resources.GetObject("comboBox2.DragDropOperationCall")));
            this.comboBox2.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox2.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox2.EnableProperty.Path = "Order.Invoiced";
            this.comboBox2.Enumeration = "";
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.InsertOperationCall = ((object)(resources.GetObject("comboBox2.InsertOperationCall")));
            resources.ApplyResources(this.comboBox2, "comboBox2");
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.NullValueName = "";
            this.comboBox2.OperationCall = ((object)(resources.GetObject("comboBox2.OperationCall")));
            this.comboBox2.Path = "RealObject.Client";
            this.comboBox2.PreLoaded = true;
            this.comboBox2.RemoveOperationCall = ((object)(resources.GetObject("comboBox2.RemoveOperationCall")));
            this.comboBox2.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox2.ViewControlObject = this.ViewControlObject;
            this.comboBox2.WarnigMessageOnRemove = "Θα το σβήσω";
            // 
            // treeView1
            // 
            this.treeView1.AllowDrag = true;
            this.treeView1.AllowDrop = true;
            this.treeView1.AllowDropOperationCall = ((object)(resources.GetObject("treeView1.AllowDropOperationCall")));
            resources.ApplyResources(this.treeView1, "treeView1");
            this.treeView1.AssignPresentationObjectType = "";
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.BeforeShowContextMenuOperationCall = ((ConnectableControls.MetaDataValue)(resources.GetObject("treeView1.BeforeShowContextMenuOperationCall")));
            this.treeView1.CheckUncheckPath = null;
            this.treeView1.ConnectedObjectAutoUpdate = false;
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView1.CutOperationCall = ((object)(resources.GetObject("treeView1.CutOperationCall")));
            this.treeView1.DeleteNodeOperationCall = ((object)(resources.GetObject("treeView1.DeleteNodeOperationCall")));
            this.treeView1.DisplayMember = "Name";
            this.treeView1.DragDropMarkColor = System.Drawing.Color.Teal;
            this.treeView1.DragDropMarkWidth = 2F;
            this.treeView1.DragDropOperationCall = ((object)(resources.GetObject("treeView1.DragDropOperationCall")));
            this.treeView1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.RequiresNewOnMove;
            this.treeView1.EditNodeOperationCall = ((object)(resources.GetObject("treeView1.EditNodeOperationCall")));
            // 
            // 
            // 
            this.treeView1.EnabledProperty.Path = "ListEnabled";
            this.treeView1.ImagePath = null;
            this.treeView1.InsertNodeOperationCall = ((object)(resources.GetObject("treeView1.InsertNodeOperationCall")));
            this.treeView1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView1.LoadOnDemand = true;
            this.treeView1.MetaData = ((object)(resources.GetObject("treeView1.MetaData")));
            this.treeView1.Model = null;
            this.treeView1.Name = "treeView1";
            this.treeView1.NodeControls.Add(this.nodeTextBox1);
            this.treeView1.NodeObjectType = "AbstractionsAndPersistency.ICategory";
            this.treeView1.Path = "Category";
            this.treeView1.RecursiveLoadSteps = 1;
            this.treeView1.SelectedNode = null;
            this.treeView1.SelectionMember = "";
            this.treeView1.SubNodesProperty = "SubCategories";
            this.treeView1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.treeView1.ViewControlObject = this.ViewControlObject;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.EditEnabled = true;
            // 
            // dynamicViewContainer1
            // 
            this.dynamicViewContainer1.DefaultHostedViewType = "UserInterfaceTest.SpecOrderDetailUserControl";
            this.dynamicViewContainer1.HostedViewIdentityPath = "OrderDetailView";
            resources.ApplyResources(this.dynamicViewContainer1, "dynamicViewContainer1");
            this.dynamicViewContainer1.Name = "dynamicViewContainer1";
            this.dynamicViewContainer1.Path = "SelOrderDetail";
            this.dynamicViewContainer1.ViewControlObject = this.ViewControlObject;
            // 
            // OrderState
            // 
            this.OrderState.AllowDrag = false;
            this.OrderState.AllowDrop = true;
            this.OrderState.AllowDropOperationCall = ((object)(resources.GetObject("OrderState.AllowDropOperationCall")));
            this.OrderState.AssignPresentationObjectType = "";
            this.OrderState.AutoDisable = false;
            this.OrderState.AutoInsert = false;
            this.OrderState.AutoSuggest = false;
            this.OrderState.ChooseFromEnum = true;
            this.OrderState.ConnectedObjectAutoUpdate = false;
            this.OrderState.DisplayMember = "";
            this.OrderState.DragDropOperationCall = ((object)(resources.GetObject("OrderState.DragDropOperationCall")));
            this.OrderState.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.OrderState.EnableCondition = null;
            // 
            // 
            // 
            this.OrderState.EnableProperty.Path = null;
            this.OrderState.Enumeration = "";
            this.OrderState.FormattingEnabled = true;
            this.OrderState.InsertOperationCall = ((object)(resources.GetObject("OrderState.InsertOperationCall")));
            resources.ApplyResources(this.OrderState, "OrderState");
            this.OrderState.Name = "OrderState";
            this.OrderState.NullValueName = "";
            this.OrderState.OperationCall = ((object)(resources.GetObject("OrderState.OperationCall")));
            this.OrderState.Path = "RealObject.State";
            this.OrderState.PreLoaded = true;
            this.OrderState.RemoveOperationCall = ((object)(resources.GetObject("OrderState.RemoveOperationCall")));
            this.OrderState.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.OrderState.ViewControlObject = this.ViewControlObject;
            this.OrderState.WarnigMessageOnRemove = "Θα το σβήσω";
            // 
            // Form2
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.OrderState);
            this.Controls.Add(this.dynamicViewContainer1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.ClientB);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.Amount);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SearchOrder);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.PriceList);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Invoiced);
            this.Controls.Add(this.dateTimePicker1Au);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form2";
            ((System.ComponentModel.ISupportInitialize)(OrderItemsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConnectableControls.TextBox textBox1;
        private ConnectableControls.SearchTextBox Client;
        private ConnectableControls.Button button1;
        private ConnectableControls.DateTimePicker dateTimePicker1Au;
        private ConnectableControls.CheckBox Invoiced;
        private ConnectableControls.ComboBox comboBox1;
        public ConnectableControls.FormConnectionControl ViewControlObject;
        private ConnectableControls.ComboBox PriceList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UserInterfaceTest.ClientUserControl UserControlTest;
        private ConnectableControls.Button SearchOrder;
        private ConnectableControls.Button button2;
        private ConnectableControls.TextBox textBox2;
        private System.Windows.Forms.Label Amount;
        private ConnectableControls.TextBox ClientB;
        private ConnectableControls.ComboBox comboBox2;
        private ConnectableControls.Tree.TreeView treeView1;
        private ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        private ConnectableControls.DynamicViewContainer dynamicViewContainer1;
        private ConnectableControls.ComboBox OrderState;
        private DXConnectableControls.XtraGrid.GridControl gridControl1;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_Description;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_Amount;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_Invoiced;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_Product;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_Client;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_Price;
        private DXConnectableControls.XtraGrid.Columns.GridColumn OrderItemsList_QAmount;
        private DXConnectableControls.XtraGrid.Columns.GridColumn Date;
    }
}