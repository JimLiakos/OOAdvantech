namespace OOAdvantech.VSUMLMetaDataRepository
{
    partial class ClassifiertForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassifiertForm));
            this.ComponentPages = new System.Windows.Forms.TabControl();
            this.General = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PersistentBtn = new ConnectableControls.CheckBox();
            this.ObjectConnectionControl = new ConnectableControls.FormConnectionControl();
            this.AbstractBtn = new ConnectableControls.CheckBox();
            this.NameLabel = new ConnectableControls.Label();
            this.ComponentName = new ConnectableControls.TextBox();
            this.DocumentationLabel = new ConnectableControls.Label();
            this.Documentation = new ConnectableControls.TextBox();
            this.VisibilityGroupBox = new System.Windows.Forms.GroupBox();
            this.PackageBtn = new ConnectableControls.RadioButton();
            this.ProtectedBtn = new ConnectableControls.RadioButton();
            this.PrivateBtn = new ConnectableControls.RadioButton();
            this.PublicBtn = new ConnectableControls.RadioButton();
            this.Realizes = new System.Windows.Forms.TabPage();
            this.listView2 = new ConnectableControls.List.ListView();
            this.Detail = new System.Windows.Forms.TabPage();
            this.label1 = new ConnectableControls.Label();
            this.GenericsParameters = new ConnectableControls.List.ListView();
            this.Operations = new System.Windows.Forms.TabPage();
            this.OperationList = new ConnectableControls.List.ListView();
            this.Attributes = new System.Windows.Forms.TabPage();
            this.AttributeList = new ConnectableControls.List.ListView();
            this.button1 = new ConnectableControls.Button();
            this.ComponentPages.SuspendLayout();
            this.General.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.VisibilityGroupBox.SuspendLayout();
            this.Realizes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listView2)).BeginInit();
            this.Detail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GenericsParameters)).BeginInit();
            this.Operations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationList)).BeginInit();
            this.Attributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AttributeList)).BeginInit();
            this.SuspendLayout();
            // 
            // ComponentPages
            // 
            this.ComponentPages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ComponentPages.Controls.Add(this.General);
            this.ComponentPages.Controls.Add(this.Realizes);
            this.ComponentPages.Controls.Add(this.Detail);
            this.ComponentPages.Controls.Add(this.Operations);
            this.ComponentPages.Controls.Add(this.Attributes);
            this.ComponentPages.Location = new System.Drawing.Point(0, 0);
            this.ComponentPages.Name = "ComponentPages";
            this.ComponentPages.SelectedIndex = 0;
            this.ComponentPages.Size = new System.Drawing.Size(388, 392);
            this.ComponentPages.TabIndex = 1;
            // 
            // General
            // 
            this.General.Controls.Add(this.tableLayoutPanel1);
            this.General.Location = new System.Drawing.Point(4, 22);
            this.General.Name = "General";
            this.General.Padding = new System.Windows.Forms.Padding(3);
            this.General.Size = new System.Drawing.Size(380, 366);
            this.General.TabIndex = 0;
            this.General.Text = "General";
            this.General.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.NameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ComponentName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DocumentationLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Documentation, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.VisibilityGroupBox, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 360);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.PersistentBtn);
            this.groupBox1.Controls.Add(this.AbstractBtn);
            this.groupBox1.Location = new System.Drawing.Point(3, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 29);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // PersistentBtn
            // 
            this.PersistentBtn.AllowDrag = false;
            this.PersistentBtn.AutoDisable = true;
            this.PersistentBtn.AutoSize = true;
            this.PersistentBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.PersistentBtn.EnableProperty.Path = "CanBePersistent";
            this.PersistentBtn.Location = new System.Drawing.Point(128, 9);
            this.PersistentBtn.Name = "PersistentBtn";
            this.PersistentBtn.Path = "Persistent";
            this.PersistentBtn.Size = new System.Drawing.Size(72, 17);
            this.PersistentBtn.TabIndex = 1;
            this.PersistentBtn.Text = "Persistent";
            this.PersistentBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PersistentBtn.UseVisualStyleBackColor = true;
            this.PersistentBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ObjectConnectionControl
            // 
            this.ObjectConnectionControl.AllowDrag = false;
            this.ObjectConnectionControl.AllowDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.AllowDropOperationCall")));
            this.ObjectConnectionControl.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.ClassifierPresentationObject";
            this.ObjectConnectionControl.ContainerControl = this;
            this.ObjectConnectionControl.CreatePresentationObjectAnyway = false;
            this.ObjectConnectionControl.DragDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.DragDropOperationCall")));
            this.ObjectConnectionControl.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ObjectConnectionControl.IniateTransactionOnInstanceSet = false;
            this.ObjectConnectionControl.MasterViewControlObject = null;
            this.ObjectConnectionControl.Name = "ObjectConnectionControl";
            this.ObjectConnectionControl.RollbackOnExitWithoutAnswer = false;
            this.ObjectConnectionControl.RollbackOnNegativeAnswer = true;
            this.ObjectConnectionControl.SkipErrorCheck = false;
            this.ObjectConnectionControl.TransactionObjectLockTimeOut = 0;
            this.ObjectConnectionControl.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.ObjectConnectionControl.ViewControlObjectAssembly = "OOAdvantech";
            this.ObjectConnectionControl.ViewControlObjectType = "OOAdvantech.MetaDataRepository.Classifier";
            // 
            // AbstractBtn
            // 
            this.AbstractBtn.AllowDrag = false;
            this.AbstractBtn.AutoDisable = true;
            this.AbstractBtn.AutoSize = true;
            this.AbstractBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.AbstractBtn.EnableProperty.Path = "CanBeAbstract";
            this.AbstractBtn.Location = new System.Drawing.Point(57, 9);
            this.AbstractBtn.Name = "AbstractBtn";
            this.AbstractBtn.Path = "Abstract";
            this.AbstractBtn.Size = new System.Drawing.Size(65, 17);
            this.AbstractBtn.TabIndex = 0;
            this.AbstractBtn.Text = "Abstract";
            this.AbstractBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.AbstractBtn.UseVisualStyleBackColor = true;
            this.AbstractBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(19, 8);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Path = null;
            this.NameLabel.Size = new System.Drawing.Size(38, 13);
            this.NameLabel.TabIndex = 0;
            this.NameLabel.Text = "Name:";
            // 
            // 
            // 
            this.NameLabel.TextEnableProperty.Path = null;
            this.NameLabel.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ComponentName
            // 
            this.ComponentName.AllowDrag = false;
            this.ComponentName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ComponentName.AutoDisable = true;
            this.ComponentName.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.ComponentName.EnableProperty.Path = null;
            this.ComponentName.Location = new System.Drawing.Point(63, 5);
            this.ComponentName.Name = "ComponentName";
            this.ComponentName.Path = "RealObject.Name";
            this.ComponentName.Size = new System.Drawing.Size(308, 20);
            this.ComponentName.TabIndex = 1;
            this.ComponentName.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.ComponentName.ViewControlObject = this.ObjectConnectionControl;
            // 
            // DocumentationLabel
            // 
            this.DocumentationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DocumentationLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.DocumentationLabel, 2);
            this.DocumentationLabel.Location = new System.Drawing.Point(3, 112);
            this.DocumentationLabel.Name = "DocumentationLabel";
            this.DocumentationLabel.Path = null;
            this.DocumentationLabel.Size = new System.Drawing.Size(82, 13);
            this.DocumentationLabel.TabIndex = 2;
            this.DocumentationLabel.Text = "Documentation:";
            // 
            // 
            // 
            this.DocumentationLabel.TextEnableProperty.Path = null;
            this.DocumentationLabel.ViewControlObject = this.ObjectConnectionControl;
            // 
            // Documentation
            // 
            this.Documentation.AllowDrag = false;
            this.Documentation.AutoDisable = true;
            this.tableLayoutPanel1.SetColumnSpan(this.Documentation, 2);
            this.Documentation.ConnectedObjectAutoUpdate = false;
            this.Documentation.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.Documentation.EnableProperty.Path = null;
            this.Documentation.Location = new System.Drawing.Point(3, 128);
            this.Documentation.Multiline = true;
            this.Documentation.Name = "Documentation";
            this.Documentation.Path = null;
            this.Documentation.Size = new System.Drawing.Size(368, 229);
            this.Documentation.TabIndex = 3;
            this.Documentation.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Documentation.ViewControlObject = this.ObjectConnectionControl;
            // 
            // VisibilityGroupBox
            // 
            this.VisibilityGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.VisibilityGroupBox, 2);
            this.VisibilityGroupBox.Controls.Add(this.PackageBtn);
            this.VisibilityGroupBox.Controls.Add(this.ProtectedBtn);
            this.VisibilityGroupBox.Controls.Add(this.PrivateBtn);
            this.VisibilityGroupBox.Controls.Add(this.PublicBtn);
            this.VisibilityGroupBox.Location = new System.Drawing.Point(3, 33);
            this.VisibilityGroupBox.Name = "VisibilityGroupBox";
            this.VisibilityGroupBox.Size = new System.Drawing.Size(368, 34);
            this.VisibilityGroupBox.TabIndex = 4;
            this.VisibilityGroupBox.TabStop = false;
            this.VisibilityGroupBox.Text = "Visibility";
            // 
            // PackageBtn
            // 
            this.PackageBtn.AllowDrag = false;
            this.PackageBtn.AutoDisable = true;
            this.PackageBtn.AutoSize = true;
            this.PackageBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.PackageBtn.EnableProperty.Path = null;
            this.PackageBtn.Location = new System.Drawing.Point(264, 11);
            this.PackageBtn.Name = "PackageBtn";
            this.PackageBtn.Path = "Package";
            this.PackageBtn.Size = new System.Drawing.Size(68, 17);
            this.PackageBtn.TabIndex = 3;
            this.PackageBtn.TabStop = true;
            this.PackageBtn.Text = "Package";
            this.PackageBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PackageBtn.UseVisualStyleBackColor = true;
            this.PackageBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ProtectedBtn
            // 
            this.ProtectedBtn.AllowDrag = false;
            this.ProtectedBtn.AutoDisable = true;
            this.ProtectedBtn.AutoSize = true;
            this.ProtectedBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.ProtectedBtn.EnableProperty.Path = null;
            this.ProtectedBtn.Location = new System.Drawing.Point(185, 11);
            this.ProtectedBtn.Name = "ProtectedBtn";
            this.ProtectedBtn.Path = "Protected";
            this.ProtectedBtn.Size = new System.Drawing.Size(71, 17);
            this.ProtectedBtn.TabIndex = 2;
            this.ProtectedBtn.TabStop = true;
            this.ProtectedBtn.Text = "Protected";
            this.ProtectedBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ProtectedBtn.UseVisualStyleBackColor = true;
            this.ProtectedBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // PrivateBtn
            // 
            this.PrivateBtn.AllowDrag = false;
            this.PrivateBtn.AutoDisable = true;
            this.PrivateBtn.AutoSize = true;
            this.PrivateBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.PrivateBtn.EnableProperty.Path = null;
            this.PrivateBtn.Location = new System.Drawing.Point(119, 11);
            this.PrivateBtn.Name = "PrivateBtn";
            this.PrivateBtn.Path = "Private";
            this.PrivateBtn.Size = new System.Drawing.Size(58, 17);
            this.PrivateBtn.TabIndex = 1;
            this.PrivateBtn.TabStop = true;
            this.PrivateBtn.Text = "Private";
            this.PrivateBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PrivateBtn.UseVisualStyleBackColor = true;
            this.PrivateBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // PublicBtn
            // 
            this.PublicBtn.AllowDrag = false;
            this.PublicBtn.AutoDisable = true;
            this.PublicBtn.AutoSize = true;
            this.PublicBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.PublicBtn.EnableProperty.Path = null;
            this.PublicBtn.Location = new System.Drawing.Point(57, 11);
            this.PublicBtn.Name = "PublicBtn";
            this.PublicBtn.Path = "Public";
            this.PublicBtn.Size = new System.Drawing.Size(54, 17);
            this.PublicBtn.TabIndex = 0;
            this.PublicBtn.TabStop = true;
            this.PublicBtn.Text = "Public";
            this.PublicBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PublicBtn.UseVisualStyleBackColor = true;
            this.PublicBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // Realizes
            // 
            this.Realizes.Controls.Add(this.listView2);
            this.Realizes.Location = new System.Drawing.Point(4, 22);
            this.Realizes.Name = "Realizes";
            this.Realizes.Padding = new System.Windows.Forms.Padding(3);
            this.Realizes.Size = new System.Drawing.Size(380, 366);
            this.Realizes.TabIndex = 1;
            this.Realizes.Text = "Modules";
            this.Realizes.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView2.BackColor = System.Drawing.SystemColors.Control;
            this.listView2.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.listView2.EnabledProperty.Path = null;
            this.listView2.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.listView2.ListConnection.AllowDrag = false;
            this.listView2.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.listView2.ListConnection.AssignPresentationObjectType = "";
            this.listView2.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.listView2.ListConnection.ConnectedObjectAutoUpdate = false;
            this.listView2.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.listView2.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.listView2.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.listView2.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.listView2.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.listView2.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.listView2.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.listView2.ListConnection.Path = "Components";
            this.listView2.ListConnection.SelectionMember = null;
            this.listView2.ListConnection.ViewControlObject = this.ObjectConnectionControl;
            this.listView2.Location = new System.Drawing.Point(6, 6);
            this.listView2.Name = "listView2";
            this.listView2.RowHeight = 15;
            this.listView2.Size = new System.Drawing.Size(371, 352);
            this.listView2.TabIndex = 3;
            this.listView2.Text = "listView2";
            // 
            // Detail
            // 
            this.Detail.Controls.Add(this.label1);
            this.Detail.Controls.Add(this.GenericsParameters);
            this.Detail.Location = new System.Drawing.Point(4, 22);
            this.Detail.Name = "Detail";
            this.Detail.Size = new System.Drawing.Size(380, 366);
            this.Detail.TabIndex = 2;
            this.Detail.Text = "Detail";
            this.Detail.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 79);
            this.label1.Name = "label1";
            this.label1.Path = null;
            this.label1.Size = new System.Drawing.Size(110, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Template Parameters:";
            // 
            // 
            // 
            this.label1.TextEnableProperty.Path = null;
            this.label1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // GenericsParameters
            // 
            this.GenericsParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GenericsParameters.BackColor = System.Drawing.SystemColors.Control;
            this.GenericsParameters.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.GenericsParameters.EnabledProperty.Path = null;
            this.GenericsParameters.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.GenericsParameters.ListConnection.AllowDrag = false;
            this.GenericsParameters.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall1")));
            this.GenericsParameters.ListConnection.AssignPresentationObjectType = "";
            this.GenericsParameters.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall1")));
            this.GenericsParameters.ListConnection.ConnectedObjectAutoUpdate = false;
            this.GenericsParameters.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall1")));
            this.GenericsParameters.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall1")));
            this.GenericsParameters.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.GenericsParameters.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall1")));
            this.GenericsParameters.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall1")));
            this.GenericsParameters.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall1")));
            this.GenericsParameters.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData1")));
            this.GenericsParameters.ListConnection.Path = "RealObject.OwnedTemplateSignature.OwnedParameters";
            this.GenericsParameters.ListConnection.SelectionMember = null;
            this.GenericsParameters.ListConnection.ViewControlObject = this.ObjectConnectionControl;
            this.GenericsParameters.Location = new System.Drawing.Point(8, 96);
            this.GenericsParameters.Name = "GenericsParameters";
            this.GenericsParameters.RowHeight = 15;
            this.GenericsParameters.Size = new System.Drawing.Size(364, 237);
            this.GenericsParameters.TabIndex = 0;
            // 
            // Operations
            // 
            this.Operations.Controls.Add(this.OperationList);
            this.Operations.Location = new System.Drawing.Point(4, 22);
            this.Operations.Name = "Operations";
            this.Operations.Size = new System.Drawing.Size(380, 366);
            this.Operations.TabIndex = 3;
            this.Operations.Text = "Operations";
            this.Operations.UseVisualStyleBackColor = true;
            // 
            // OperationList
            // 
            this.OperationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OperationList.BackColor = System.Drawing.SystemColors.Control;
            this.OperationList.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.OperationList.EnabledProperty.Path = null;
            this.OperationList.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.OperationList.ListConnection.AllowDrag = false;
            this.OperationList.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall2")));
            this.OperationList.ListConnection.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.OperationPresentationObject";
            this.OperationList.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall2")));
            this.OperationList.ListConnection.ConnectedObjectAutoUpdate = false;
            this.OperationList.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall2")));
            this.OperationList.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall2")));
            this.OperationList.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.OperationList.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall2")));
            this.OperationList.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall2")));
            this.OperationList.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall2")));
            this.OperationList.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData2")));
            this.OperationList.ListConnection.Path = "Operations";
            this.OperationList.ListConnection.SelectionMember = null;
            this.OperationList.ListConnection.ViewControlObject = this.ObjectConnectionControl;
            this.OperationList.Location = new System.Drawing.Point(3, 0);
            this.OperationList.Name = "OperationList";
            this.OperationList.RowHeight = 18;
            this.OperationList.Size = new System.Drawing.Size(374, 360);
            this.OperationList.TabIndex = 0;
            this.OperationList.Text = "listView1";
            // 
            // Attributes
            // 
            this.Attributes.Controls.Add(this.AttributeList);
            this.Attributes.Location = new System.Drawing.Point(4, 22);
            this.Attributes.Name = "Attributes";
            this.Attributes.Padding = new System.Windows.Forms.Padding(3);
            this.Attributes.Size = new System.Drawing.Size(380, 366);
            this.Attributes.TabIndex = 4;
            this.Attributes.Text = "Attributes";
            this.Attributes.UseVisualStyleBackColor = true;
            // 
            // AttributeList
            // 
            this.AttributeList.BackColor = System.Drawing.SystemColors.Control;
            this.AttributeList.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.AttributeList.EnabledProperty.Path = null;
            this.AttributeList.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.AttributeList.ListConnection.AllowDrag = false;
            this.AttributeList.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall3")));
            this.AttributeList.ListConnection.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.AttributePresentationObject";
            this.AttributeList.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall3")));
            this.AttributeList.ListConnection.ConnectedObjectAutoUpdate = false;
            this.AttributeList.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall3")));
            this.AttributeList.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall3")));
            this.AttributeList.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.AttributeList.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall3")));
            this.AttributeList.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall3")));
            this.AttributeList.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall3")));
            this.AttributeList.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData3")));
            this.AttributeList.ListConnection.Path = "Attributes";
            this.AttributeList.ListConnection.SelectionMember = null;
            this.AttributeList.ListConnection.ViewControlObject = this.ObjectConnectionControl;
            this.AttributeList.Location = new System.Drawing.Point(0, 0);
            this.AttributeList.Name = "AttributeList";
            this.AttributeList.RowHeight = 18;
            this.AttributeList.Size = new System.Drawing.Size(380, 366);
            this.AttributeList.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.AllowDrag = false;
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.ConnectedObjectAutoUpdate = false;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(157, 398);
            this.button1.Name = "button1";
            this.button1.OnClickOperationCall = ((object)(resources.GetObject("button1.OnClickOperationCall")));
            this.button1.Path = "";
            this.button1.SaveButton = false;
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Value = null;
            this.button1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ClassifiertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 427);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ComponentPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassifiertForm";
            this.ShowInTaskbar = false;
            this.Text = "Class Properties";
            this.ComponentPages.ResumeLayout(false);
            this.General.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.VisibilityGroupBox.ResumeLayout(false);
            this.VisibilityGroupBox.PerformLayout();
            this.Realizes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listView2)).EndInit();
            this.Detail.ResumeLayout(false);
            this.Detail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GenericsParameters)).EndInit();
            this.Operations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationList)).EndInit();
            this.Attributes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AttributeList)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.TabControl ComponentPages;
        private System.Windows.Forms.TabPage General;
        private System.Windows.Forms.TabPage Realizes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ConnectableControls.Label NameLabel;
        private ConnectableControls.TextBox ComponentName;
        private ConnectableControls.Label DocumentationLabel;
        private ConnectableControls.TextBox Documentation;
        private ConnectableControls.FormConnectionControl ObjectConnectionControl;
        private System.Windows.Forms.GroupBox VisibilityGroupBox;
        private ConnectableControls.RadioButton PublicBtn;
        private ConnectableControls.RadioButton PrivateBtn;
        private ConnectableControls.RadioButton ProtectedBtn;
        private ConnectableControls.RadioButton PackageBtn;
        private ConnectableControls.CheckBox AbstractBtn;
        private ConnectableControls.CheckBox PersistentBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private ConnectableControls.List.ListView listView2;
        private ConnectableControls.Button button1;
        private System.Windows.Forms.TabPage Detail;
        private ConnectableControls.Label label1;
        private ConnectableControls.List.ListView GenericsParameters;
        private System.Windows.Forms.TabPage Operations;
        private ConnectableControls.List.ListView OperationList;
        private System.Windows.Forms.TabPage Attributes;
        private ConnectableControls.List.ListView AttributeList;




    }
}