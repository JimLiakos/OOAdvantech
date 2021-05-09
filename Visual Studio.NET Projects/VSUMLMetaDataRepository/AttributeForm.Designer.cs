namespace OOAdvantech.VSUMLMetaDataRepository
{
    partial class AttributeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{0c618ce2-a110-466d-9b23-1f1758534fc5}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{8911ae8d-815b-476b-b965-d4616dc6c2a6}</MetaDataID>
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
        /// <MetaDataID>{595d658b-9072-4bff-9b4e-c265e855995f}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttributeForm));
            this.AttributePages = new System.Windows.Forms.TabControl();
            this.GeneralTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox3 = new ConnectableControls.ComboBox();
            this.ObjectConnectionControl = new ConnectableControls.FormConnectionControl();
            this.label3 = new ConnectableControls.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SealedBtn = new ConnectableControls.RadioButton();
            this.DefaultBtn = new ConnectableControls.RadioButton();
            this.NewBtn = new ConnectableControls.RadioButton();
            this.OverrideBtn = new ConnectableControls.RadioButton();
            this.Virtual = new ConnectableControls.RadioButton();
            this.AbstractBtn = new ConnectableControls.RadioButton();
            this.Documentation = new ConnectableControls.TextBox();
            this.AttributeName = new ConnectableControls.TextBox();
            this.NameLabel = new ConnectableControls.Label();
            this.VisibilityGroup = new System.Windows.Forms.GroupBox();
            this.PublicBtn = new ConnectableControls.RadioButton();
            this.PrivateBtn = new ConnectableControls.RadioButton();
            this.ProtectedBtn = new ConnectableControls.RadioButton();
            this.PackageBtn = new ConnectableControls.RadioButton();
            this.DocumentationLabel = new ConnectableControls.Label();
            this.DetailTab = new System.Windows.Forms.TabPage();
            this.AttributeProperties = new System.Windows.Forms.GroupBox();
            this.EventChck = new ConnectableControls.CheckBox();
            this.PropertyChck = new ConnectableControls.CheckBox();
            this.PersistentBtn = new ConnectableControls.CheckBox();
            this.StaticBtn = new ConnectableControls.CheckBox();
            this.BackwardCompatibilityIDBtn = new ConnectableControls.Button();
            this.label2 = new ConnectableControls.Label();
            this.textBox2 = new ConnectableControls.TextBox();
            this.PersistentGroup = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new ConnectableControls.CheckBox();
            this.textBox1 = new ConnectableControls.TextBox();
            this.label1 = new ConnectableControls.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PropertyBodyChck = new ConnectableControls.CheckBox();
            this.SetLB = new ConnectableControls.Label();
            this.GetLB = new ConnectableControls.Label();
            this.comboBox2 = new ConnectableControls.ComboBox();
            this.comboBox1 = new ConnectableControls.ComboBox();
            this.AttributePages.SuspendLayout();
            this.GeneralTab.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.VisibilityGroup.SuspendLayout();
            this.DetailTab.SuspendLayout();
            this.AttributeProperties.SuspendLayout();
            this.PersistentGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // AttributePages
            // 
            this.AttributePages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributePages.Controls.Add(this.GeneralTab);
            this.AttributePages.Controls.Add(this.DetailTab);
            this.AttributePages.Location = new System.Drawing.Point(2, 0);
            this.AttributePages.Name = "AttributePages";
            this.AttributePages.SelectedIndex = 0;
            this.AttributePages.Size = new System.Drawing.Size(378, 424);
            this.AttributePages.TabIndex = 0;
            // 
            // GeneralTab
            // 
            this.GeneralTab.Controls.Add(this.tableLayoutPanel1);
            this.GeneralTab.Location = new System.Drawing.Point(4, 22);
            this.GeneralTab.Name = "GeneralTab";
            this.GeneralTab.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTab.Size = new System.Drawing.Size(370, 398);
            this.GeneralTab.TabIndex = 0;
            this.GeneralTab.Text = "General";
            this.GeneralTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Documentation, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.AttributeName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.NameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.VisibilityGroup, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.DocumentationLabel, 0, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 398);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBox3
            // 
            this.comboBox3.AllowDrag = false;
            this.comboBox3.AllowDropOperationCall = ((object)(resources.GetObject("comboBox3.AllowDropOperationCall")));
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.AssignPresentationObjectType = "";
            this.comboBox3.AutoDisable = true;
            this.comboBox3.AutoInsert = true;
            this.comboBox3.AutoSuggest = false;
            this.comboBox3.ChooseFromEnum = false;
            this.comboBox3.ConnectedObjectAutoUpdate = false;
            this.comboBox3.DisplayMember = "";
            this.comboBox3.DragDropOperationCall = ((object)(resources.GetObject("comboBox3.DragDropOperationCall")));
            this.comboBox3.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox3.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox3.EnableProperty.Path = null;
            this.comboBox3.Enumeration = "";
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.InsertOperationCall = ((object)(resources.GetObject("comboBox3.InsertOperationCall")));
            this.comboBox3.IntegralHeight = false;
            this.comboBox3.Location = new System.Drawing.Point(78, 34);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.NullValueName = "";
            this.comboBox3.OperationCall = ((object)(resources.GetObject("comboBox3.OperationCall")));
            this.comboBox3.Path = "Type";
            this.comboBox3.PreLoaded = true;
            this.comboBox3.RemoveOperationCall = ((object)(resources.GetObject("comboBox3.RemoveOperationCall")));
            this.comboBox3.Size = new System.Drawing.Size(289, 21);
            this.comboBox3.TabIndex = 10;
            this.comboBox3.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.comboBox3.ViewControlObject = this.ObjectConnectionControl;
            this.comboBox3.WarnigMessageOnRemove = null;
            // 
            // ObjectConnectionControl
            // 
            this.ObjectConnectionControl.AllowDrag = false;
            this.ObjectConnectionControl.AllowDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.AllowDropOperationCall")));
            this.ObjectConnectionControl.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.AttributePresentationObject";
            this.ObjectConnectionControl.ContainerControl = this;
            this.ObjectConnectionControl.CreatePresentationObjectAnyway = false;
            this.ObjectConnectionControl.DragDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.DragDropOperationCall")));
            this.ObjectConnectionControl.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ObjectConnectionControl.IniateTransactionOnInstanceSet = false;
            this.ObjectConnectionControl.MasterViewControlObject = null;
            this.ObjectConnectionControl.Name = "ObjectConnectionControl";
            this.ObjectConnectionControl.RollbackOnExitWithoutAnswer = false;
            this.ObjectConnectionControl.RollbackOnNegativeAnswer = false;
            this.ObjectConnectionControl.SkipErrorCheck = false;
            this.ObjectConnectionControl.TransactionObjectLockTimeOut = 0;
            this.ObjectConnectionControl.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.ObjectConnectionControl.ViewControlObjectAssembly = "VSUMLMetaDataRepository";
            this.ObjectConnectionControl.ViewControlObjectType = "OOAdvantech.VSUMLMetaDataRepository.Attribute";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 38);
            this.label3.Name = "label3";
            this.label3.Path = null;
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = " Type:";
            // 
            // 
            // 
            this.label3.TextEnableProperty.Path = null;
            this.label3.ViewControlObject = this.ObjectConnectionControl;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.SealedBtn);
            this.groupBox2.Controls.Add(this.DefaultBtn);
            this.groupBox2.Controls.Add(this.NewBtn);
            this.groupBox2.Controls.Add(this.OverrideBtn);
            this.groupBox2.Controls.Add(this.Virtual);
            this.groupBox2.Controls.Add(this.AbstractBtn);
            this.groupBox2.Location = new System.Drawing.Point(3, 103);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 39);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Override Kind";
            // 
            // SealedBtn
            // 
            this.SealedBtn.AllowDrag = false;
            this.SealedBtn.AutoDisable = true;
            this.SealedBtn.AutoSize = true;
            this.SealedBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.SealedBtn.EnableProperty.Path = "CanChangeOverrideKind";
            this.SealedBtn.Location = new System.Drawing.Point(249, 17);
            this.SealedBtn.Name = "SealedBtn";
            this.SealedBtn.Path = "IsSealed";
            this.SealedBtn.Size = new System.Drawing.Size(58, 17);
            this.SealedBtn.TabIndex = 6;
            this.SealedBtn.TabStop = true;
            this.SealedBtn.Text = "Sealed";
            this.SealedBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.SealedBtn.UseVisualStyleBackColor = true;
            this.SealedBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // DefaultBtn
            // 
            this.DefaultBtn.AllowDrag = false;
            this.DefaultBtn.AutoDisable = true;
            this.DefaultBtn.AutoSize = true;
            this.DefaultBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.DefaultBtn.EnableProperty.Path = "CanChangeOverrideKind";
            this.DefaultBtn.Location = new System.Drawing.Point(307, 17);
            this.DefaultBtn.Name = "DefaultBtn";
            this.DefaultBtn.Path = "IsNone";
            this.DefaultBtn.Size = new System.Drawing.Size(51, 17);
            this.DefaultBtn.TabIndex = 5;
            this.DefaultBtn.TabStop = true;
            this.DefaultBtn.Text = "None";
            this.DefaultBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.DefaultBtn.UseVisualStyleBackColor = true;
            this.DefaultBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // NewBtn
            // 
            this.NewBtn.AllowDrag = false;
            this.NewBtn.AutoDisable = true;
            this.NewBtn.AutoSize = true;
            this.NewBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.NewBtn.EnableProperty.Path = "CanChangeOverrideKind";
            this.NewBtn.Location = new System.Drawing.Point(202, 17);
            this.NewBtn.Name = "NewBtn";
            this.NewBtn.Path = "IsNew";
            this.NewBtn.Size = new System.Drawing.Size(47, 17);
            this.NewBtn.TabIndex = 3;
            this.NewBtn.TabStop = true;
            this.NewBtn.Text = "New";
            this.NewBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.NewBtn.UseVisualStyleBackColor = true;
            this.NewBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // OverrideBtn
            // 
            this.OverrideBtn.AllowDrag = false;
            this.OverrideBtn.AutoDisable = true;
            this.OverrideBtn.AutoSize = true;
            this.OverrideBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.OverrideBtn.EnableProperty.Path = "CanChangeOverrideKind";
            this.OverrideBtn.Location = new System.Drawing.Point(137, 17);
            this.OverrideBtn.Name = "OverrideBtn";
            this.OverrideBtn.Path = "IsOverride";
            this.OverrideBtn.Size = new System.Drawing.Size(65, 17);
            this.OverrideBtn.TabIndex = 2;
            this.OverrideBtn.TabStop = true;
            this.OverrideBtn.Text = "Override";
            this.OverrideBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.OverrideBtn.UseVisualStyleBackColor = true;
            this.OverrideBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // Virtual
            // 
            this.Virtual.AllowDrag = false;
            this.Virtual.AutoDisable = true;
            this.Virtual.AutoSize = true;
            this.Virtual.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.Virtual.EnableProperty.Path = "CanChangeOverrideKind";
            this.Virtual.Location = new System.Drawing.Point(83, 17);
            this.Virtual.Name = "Virtual";
            this.Virtual.Path = "IsVirtual";
            this.Virtual.Size = new System.Drawing.Size(54, 17);
            this.Virtual.TabIndex = 1;
            this.Virtual.TabStop = true;
            this.Virtual.Text = "Virtual";
            this.Virtual.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.Virtual.UseVisualStyleBackColor = true;
            this.Virtual.ViewControlObject = this.ObjectConnectionControl;
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
            this.AbstractBtn.EnableProperty.Path = "CanChangeOverrideKind";
            this.AbstractBtn.Location = new System.Drawing.Point(19, 17);
            this.AbstractBtn.Name = "AbstractBtn";
            this.AbstractBtn.Path = "IsAbstract";
            this.AbstractBtn.Size = new System.Drawing.Size(64, 17);
            this.AbstractBtn.TabIndex = 0;
            this.AbstractBtn.TabStop = true;
            this.AbstractBtn.Text = "Abstract";
            this.AbstractBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.AbstractBtn.UseVisualStyleBackColor = true;
            this.AbstractBtn.ViewControlObject = this.ObjectConnectionControl;
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
            this.Documentation.Location = new System.Drawing.Point(3, 168);
            this.Documentation.Multiline = true;
            this.Documentation.Name = "Documentation";
            this.Documentation.Path = null;
            this.Documentation.Size = new System.Drawing.Size(364, 227);
            this.Documentation.TabIndex = 7;
            this.Documentation.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Documentation.ViewControlObject = this.ObjectConnectionControl;
            // 
            // AttributeName
            // 
            this.AttributeName.AllowDrag = false;
            this.AttributeName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributeName.AutoDisable = true;
            this.AttributeName.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.AttributeName.EnableProperty.Path = null;
            this.AttributeName.Location = new System.Drawing.Point(78, 5);
            this.AttributeName.Name = "AttributeName";
            this.AttributeName.Path = "RealObject.Name";
            this.AttributeName.Size = new System.Drawing.Size(289, 20);
            this.AttributeName.TabIndex = 2;
            this.AttributeName.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.AttributeName.ViewControlObject = this.ObjectConnectionControl;
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(34, 8);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Path = null;
            this.NameLabel.Size = new System.Drawing.Size(38, 13);
            this.NameLabel.TabIndex = 3;
            this.NameLabel.Text = "Name:";
            // 
            // 
            // 
            this.NameLabel.TextEnableProperty.Path = null;
            this.NameLabel.ViewControlObject = this.ObjectConnectionControl;
            // 
            // VisibilityGroup
            // 
            this.VisibilityGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.VisibilityGroup, 2);
            this.VisibilityGroup.Controls.Add(this.PublicBtn);
            this.VisibilityGroup.Controls.Add(this.PrivateBtn);
            this.VisibilityGroup.Controls.Add(this.ProtectedBtn);
            this.VisibilityGroup.Controls.Add(this.PackageBtn);
            this.VisibilityGroup.Location = new System.Drawing.Point(3, 63);
            this.VisibilityGroup.Name = "VisibilityGroup";
            this.VisibilityGroup.Size = new System.Drawing.Size(364, 34);
            this.VisibilityGroup.TabIndex = 4;
            this.VisibilityGroup.TabStop = false;
            this.VisibilityGroup.Text = "Visibility";
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
            this.PublicBtn.EnableProperty.Path = "OwnerIsClassOrStruct";
            this.PublicBtn.Location = new System.Drawing.Point(55, 11);
            this.PublicBtn.Name = "PublicBtn";
            this.PublicBtn.Path = "Public";
            this.PublicBtn.Size = new System.Drawing.Size(54, 17);
            this.PublicBtn.TabIndex = 7;
            this.PublicBtn.TabStop = true;
            this.PublicBtn.Text = "Public";
            this.PublicBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PublicBtn.UseVisualStyleBackColor = true;
            this.PublicBtn.ViewControlObject = this.ObjectConnectionControl;
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
            this.PrivateBtn.EnableProperty.Path = "OwnerIsClassOrStruct";
            this.PrivateBtn.Location = new System.Drawing.Point(125, 11);
            this.PrivateBtn.Name = "PrivateBtn";
            this.PrivateBtn.Path = "Private";
            this.PrivateBtn.Size = new System.Drawing.Size(58, 17);
            this.PrivateBtn.TabIndex = 6;
            this.PrivateBtn.TabStop = true;
            this.PrivateBtn.Text = "Private";
            this.PrivateBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PrivateBtn.UseVisualStyleBackColor = true;
            this.PrivateBtn.ViewControlObject = this.ObjectConnectionControl;
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
            this.ProtectedBtn.EnableProperty.Path = "OwnerIsClassOrStruct";
            this.ProtectedBtn.Location = new System.Drawing.Point(199, 11);
            this.ProtectedBtn.Name = "ProtectedBtn";
            this.ProtectedBtn.Path = "Protected";
            this.ProtectedBtn.Size = new System.Drawing.Size(71, 17);
            this.ProtectedBtn.TabIndex = 5;
            this.ProtectedBtn.TabStop = true;
            this.ProtectedBtn.Text = "Protected";
            this.ProtectedBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ProtectedBtn.UseVisualStyleBackColor = true;
            this.ProtectedBtn.ViewControlObject = this.ObjectConnectionControl;
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
            this.PackageBtn.EnableProperty.Path = "OwnerIsClassOrStruct";
            this.PackageBtn.Location = new System.Drawing.Point(286, 11);
            this.PackageBtn.Name = "PackageBtn";
            this.PackageBtn.Path = "Package";
            this.PackageBtn.Size = new System.Drawing.Size(68, 17);
            this.PackageBtn.TabIndex = 4;
            this.PackageBtn.TabStop = true;
            this.PackageBtn.Text = "Package";
            this.PackageBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PackageBtn.UseVisualStyleBackColor = true;
            this.PackageBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // DocumentationLabel
            // 
            this.DocumentationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DocumentationLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.DocumentationLabel, 2);
            this.DocumentationLabel.Location = new System.Drawing.Point(3, 152);
            this.DocumentationLabel.Name = "DocumentationLabel";
            this.DocumentationLabel.Path = null;
            this.DocumentationLabel.Size = new System.Drawing.Size(82, 13);
            this.DocumentationLabel.TabIndex = 6;
            this.DocumentationLabel.Text = "Documentation:";
            // 
            // 
            // 
            this.DocumentationLabel.TextEnableProperty.Path = null;
            this.DocumentationLabel.ViewControlObject = this.ObjectConnectionControl;
            // 
            // DetailTab
            // 
            this.DetailTab.Controls.Add(this.AttributeProperties);
            this.DetailTab.Controls.Add(this.BackwardCompatibilityIDBtn);
            this.DetailTab.Controls.Add(this.label2);
            this.DetailTab.Controls.Add(this.textBox2);
            this.DetailTab.Controls.Add(this.PersistentGroup);
            this.DetailTab.Controls.Add(this.groupBox1);
            this.DetailTab.Location = new System.Drawing.Point(4, 22);
            this.DetailTab.Name = "DetailTab";
            this.DetailTab.Padding = new System.Windows.Forms.Padding(3);
            this.DetailTab.Size = new System.Drawing.Size(370, 398);
            this.DetailTab.TabIndex = 1;
            this.DetailTab.Text = "Detail";
            this.DetailTab.UseVisualStyleBackColor = true;
            // 
            // AttributeProperties
            // 
            this.AttributeProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributeProperties.Controls.Add(this.EventChck);
            this.AttributeProperties.Controls.Add(this.PropertyChck);
            this.AttributeProperties.Controls.Add(this.PersistentBtn);
            this.AttributeProperties.Controls.Add(this.StaticBtn);
            this.AttributeProperties.Location = new System.Drawing.Point(6, 25);
            this.AttributeProperties.Name = "AttributeProperties";
            this.AttributeProperties.Size = new System.Drawing.Size(356, 34);
            this.AttributeProperties.TabIndex = 7;
            this.AttributeProperties.TabStop = false;
            // 
            // EventChck
            // 
            this.EventChck.AllowDrag = false;
            this.EventChck.AutoDisable = true;
            this.EventChck.AutoSize = true;
            this.EventChck.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.EventChck.EnableProperty.Path = "CanChangeEventValue";
            this.EventChck.Location = new System.Drawing.Point(130, 11);
            this.EventChck.Name = "EventChck";
            this.EventChck.Path = "IsEvent";
            this.EventChck.Size = new System.Drawing.Size(54, 17);
            this.EventChck.TabIndex = 4;
            this.EventChck.Text = "Event";
            this.EventChck.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.EventChck.UseVisualStyleBackColor = true;
            this.EventChck.ViewControlObject = this.ObjectConnectionControl;
            // 
            // PropertyChck
            // 
            this.PropertyChck.AllowDrag = false;
            this.PropertyChck.AutoDisable = true;
            this.PropertyChck.AutoSize = true;
            this.PropertyChck.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.PropertyChck.EnableProperty.Path = "CanChangePropertyValue";
            this.PropertyChck.Location = new System.Drawing.Point(57, 11);
            this.PropertyChck.Name = "PropertyChck";
            this.PropertyChck.Path = "IsProperty";
            this.PropertyChck.Size = new System.Drawing.Size(65, 17);
            this.PropertyChck.TabIndex = 3;
            this.PropertyChck.Text = "Property";
            this.PropertyChck.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PropertyChck.UseVisualStyleBackColor = true;
            this.PropertyChck.ViewControlObject = this.ObjectConnectionControl;
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
            this.PersistentBtn.Location = new System.Drawing.Point(265, 11);
            this.PersistentBtn.Name = "PersistentBtn";
            this.PersistentBtn.Path = "Persistent";
            this.PersistentBtn.Size = new System.Drawing.Size(72, 17);
            this.PersistentBtn.TabIndex = 2;
            this.PersistentBtn.Text = "Persistent";
            this.PersistentBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.PersistentBtn.UseVisualStyleBackColor = true;
            this.PersistentBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // StaticBtn
            // 
            this.StaticBtn.AllowDrag = false;
            this.StaticBtn.AutoDisable = true;
            this.StaticBtn.AutoSize = true;
            this.StaticBtn.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.StaticBtn.EnableProperty.Path = "OwnerIsClassOrStruct";
            this.StaticBtn.Location = new System.Drawing.Point(192, 11);
            this.StaticBtn.Name = "StaticBtn";
            this.StaticBtn.Path = "IsStatic";
            this.StaticBtn.Size = new System.Drawing.Size(53, 17);
            this.StaticBtn.TabIndex = 1;
            this.StaticBtn.Text = "Static";
            this.StaticBtn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.StaticBtn.UseVisualStyleBackColor = true;
            this.StaticBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // BackwardCompatibilityIDBtn
            // 
            this.BackwardCompatibilityIDBtn.AllowDrag = false;
            this.BackwardCompatibilityIDBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BackwardCompatibilityIDBtn.ConnectedObjectAutoUpdate = false;
            this.BackwardCompatibilityIDBtn.Location = new System.Drawing.Point(155, 330);
            this.BackwardCompatibilityIDBtn.Name = "BackwardCompatibilityIDBtn";
            this.BackwardCompatibilityIDBtn.OnClickOperationCall = ((object)(resources.GetObject("BackwardCompatibilityIDBtn.OnClickOperationCall")));
            this.BackwardCompatibilityIDBtn.Path = "";
            this.BackwardCompatibilityIDBtn.SaveButton = false;
            this.BackwardCompatibilityIDBtn.Size = new System.Drawing.Size(190, 23);
            this.BackwardCompatibilityIDBtn.TabIndex = 6;
            this.BackwardCompatibilityIDBtn.Text = "Generate Backward Compatibility ID";
            this.BackwardCompatibilityIDBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.BackwardCompatibilityIDBtn.UseVisualStyleBackColor = true;
            this.BackwardCompatibilityIDBtn.Value = null;
            this.BackwardCompatibilityIDBtn.ViewControlObject = this.ObjectConnectionControl;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 307);
            this.label2.Name = "label2";
            this.label2.Path = null;
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Backward Compatibility ID";
            // 
            // 
            // 
            this.label2.TextEnableProperty.Path = null;
            this.label2.ViewControlObject = this.ObjectConnectionControl;
            // 
            // textBox2
            // 
            this.textBox2.AllowDrag = false;
            this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBox2.AutoDisable = true;
            this.textBox2.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.textBox2.EnableProperty.Path = null;
            this.textBox2.Location = new System.Drawing.Point(155, 304);
            this.textBox2.Name = "textBox2";
            this.textBox2.Path = "BackwardCompatibilityID";
            this.textBox2.Size = new System.Drawing.Size(190, 20);
            this.textBox2.TabIndex = 4;
            this.textBox2.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.textBox2.ViewControlObject = this.ObjectConnectionControl;
            // 
            // PersistentGroup
            // 
            this.PersistentGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PersistentGroup.Controls.Add(this.checkBox1);
            this.PersistentGroup.Controls.Add(this.textBox1);
            this.PersistentGroup.Controls.Add(this.label1);
            this.PersistentGroup.Location = new System.Drawing.Point(6, 188);
            this.PersistentGroup.Name = "PersistentGroup";
            this.PersistentGroup.Size = new System.Drawing.Size(356, 82);
            this.PersistentGroup.TabIndex = 1;
            this.PersistentGroup.TabStop = false;
            this.PersistentGroup.Text = "Persistent Property";
            // 
            // checkBox1
            // 
            this.checkBox1.AllowDrag = false;
            this.checkBox1.AutoDisable = true;
            this.checkBox1.AutoSize = true;
            this.checkBox1.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.checkBox1.EnableProperty.Path = "IsPersistentProperty";
            this.checkBox1.Location = new System.Drawing.Point(149, 59);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Path = "AutoGenerate";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(92, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "AutoGenerate";
            this.checkBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // textBox1
            // 
            this.textBox1.AllowDrag = false;
            this.textBox1.AutoDisable = true;
            this.textBox1.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.textBox1.EnableProperty.Path = "EditImplementetionField";
            this.textBox1.Location = new System.Drawing.Point(149, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.Path = "ImplementetionField";
            this.textBox1.Size = new System.Drawing.Size(190, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.textBox1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 27);
            this.label1.Name = "label1";
            this.label1.Path = null;
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Implementation Filed";
            // 
            // 
            // 
            this.label1.TextEnableProperty.Path = null;
            this.label1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.PropertyBodyChck);
            this.groupBox1.Controls.Add(this.SetLB);
            this.groupBox1.Controls.Add(this.GetLB);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Location = new System.Drawing.Point(6, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBox1.Size = new System.Drawing.Size(356, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Property";
            // 
            // PropertyBodyChck
            // 
            this.PropertyBodyChck.AllowDrag = false;
            this.PropertyBodyChck.AutoDisable = true;
            this.PropertyBodyChck.AutoSize = true;
            this.PropertyBodyChck.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.PropertyBodyChck.EnableProperty.Path = null;
            this.PropertyBodyChck.Location = new System.Drawing.Point(149, 62);
            this.PropertyBodyChck.Name = "PropertyBodyChck";
            this.PropertyBodyChck.Path = "HasBody";
            this.PropertyBodyChck.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PropertyBodyChck.Size = new System.Drawing.Size(72, 17);
            this.PropertyBodyChck.TabIndex = 5;
            this.PropertyBodyChck.Text = "Has Body";
            this.PropertyBodyChck.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.PropertyBodyChck.UseVisualStyleBackColor = true;
            this.PropertyBodyChck.ViewControlObject = this.ObjectConnectionControl;
            // 
            // SetLB
            // 
            this.SetLB.AutoSize = true;
            this.SetLB.Location = new System.Drawing.Point(186, 33);
            this.SetLB.Name = "SetLB";
            this.SetLB.Path = null;
            this.SetLB.Size = new System.Drawing.Size(26, 13);
            this.SetLB.TabIndex = 4;
            this.SetLB.Text = "Set:";
            // 
            // 
            // 
            this.SetLB.TextEnableProperty.Path = null;
            this.SetLB.ViewControlObject = this.ObjectConnectionControl;
            // 
            // GetLB
            // 
            this.GetLB.AutoSize = true;
            this.GetLB.Location = new System.Drawing.Point(10, 33);
            this.GetLB.Name = "GetLB";
            this.GetLB.Path = null;
            this.GetLB.Size = new System.Drawing.Size(27, 13);
            this.GetLB.TabIndex = 3;
            this.GetLB.Text = "Get:";
            // 
            // 
            // 
            this.GetLB.TextEnableProperty.Path = null;
            this.GetLB.ViewControlObject = this.ObjectConnectionControl;
            // 
            // comboBox2
            // 
            this.comboBox2.AllowDrag = false;
            this.comboBox2.AllowDropOperationCall = ((object)(resources.GetObject("comboBox2.AllowDropOperationCall")));
            this.comboBox2.AssignPresentationObjectType = "";
            this.comboBox2.AutoDisable = true;
            this.comboBox2.AutoInsert = false;
            this.comboBox2.AutoSuggest = false;
            this.comboBox2.ChooseFromEnum = true;
            this.comboBox2.ConnectedObjectAutoUpdate = false;
            this.comboBox2.DisplayMember = null;
            this.comboBox2.DragDropOperationCall = ((object)(resources.GetObject("comboBox2.DragDropOperationCall")));
            this.comboBox2.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox2.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox2.EnableProperty.Path = "IsProperty";
            this.comboBox2.Enumeration = "";
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.InsertOperationCall = ((object)(resources.GetObject("comboBox2.InsertOperationCall")));
            this.comboBox2.IntegralHeight = false;
            this.comboBox2.Location = new System.Drawing.Point(218, 30);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.NullValueName = "";
            this.comboBox2.OperationCall = ((object)(resources.GetObject("comboBox2.OperationCall")));
            this.comboBox2.Path = "SetPropertyMethod";
            this.comboBox2.PreLoaded = true;
            this.comboBox2.RemoveOperationCall = ((object)(resources.GetObject("comboBox2.RemoveOperationCall")));
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 2;
            this.comboBox2.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox2.ViewControlObject = this.ObjectConnectionControl;
            this.comboBox2.WarnigMessageOnRemove = null;
            // 
            // comboBox1
            // 
            this.comboBox1.AllowDrag = false;
            this.comboBox1.AllowDropOperationCall = ((object)(resources.GetObject("comboBox1.AllowDropOperationCall")));
            this.comboBox1.AssignPresentationObjectType = "";
            this.comboBox1.AutoDisable = true;
            this.comboBox1.AutoInsert = false;
            this.comboBox1.AutoSuggest = false;
            this.comboBox1.ChooseFromEnum = true;
            this.comboBox1.ConnectedObjectAutoUpdate = false;
            this.comboBox1.DisplayMember = null;
            this.comboBox1.DragDropOperationCall = ((object)(resources.GetObject("comboBox1.DragDropOperationCall")));
            this.comboBox1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox1.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox1.EnableProperty.Path = "IsProperty";
            this.comboBox1.Enumeration = "";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.InsertOperationCall = ((object)(resources.GetObject("comboBox1.InsertOperationCall")));
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Location = new System.Drawing.Point(43, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.NullValueName = "";
            this.comboBox1.OperationCall = ((object)(resources.GetObject("comboBox1.OperationCall")));
            this.comboBox1.Path = "GetPropertyMethod";
            this.comboBox1.PreLoaded = true;
            this.comboBox1.RemoveOperationCall = ((object)(resources.GetObject("comboBox1.RemoveOperationCall")));
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox1.ViewControlObject = this.ObjectConnectionControl;
            this.comboBox1.WarnigMessageOnRemove = null;
            // 
            // AttributeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 425);
            this.Controls.Add(this.AttributePages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttributeForm";
            this.ShowInTaskbar = false;
            this.Text = "Attribute Properties";
            this.AttributePages.ResumeLayout(false);
            this.GeneralTab.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.VisibilityGroup.ResumeLayout(false);
            this.VisibilityGroup.PerformLayout();
            this.DetailTab.ResumeLayout(false);
            this.DetailTab.PerformLayout();
            this.AttributeProperties.ResumeLayout(false);
            this.AttributeProperties.PerformLayout();
            this.PersistentGroup.ResumeLayout(false);
            this.PersistentGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

}

        #endregion

        /// <MetaDataID>{b4ff93c8-3c2b-4b1f-ada7-95ee1b2bdb12}</MetaDataID>
        private System.Windows.Forms.TabControl AttributePages;
        /// <MetaDataID>{157fa141-65db-4633-ba0a-bd6a56cab156}</MetaDataID>
        private System.Windows.Forms.TabPage GeneralTab;
        /// <MetaDataID>{490071f1-8034-4fcf-8d6b-ceed6ba68b4b}</MetaDataID>
        private System.Windows.Forms.TabPage DetailTab;
        /// <MetaDataID>{abff5bc9-e432-4485-9157-9b5071bd03e1}</MetaDataID>
        private ConnectableControls.FormConnectionControl ObjectConnectionControl;
        /// <MetaDataID>{b5207cbc-5c48-4d3c-991a-b110d457facd}</MetaDataID>
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        /// <MetaDataID>{5dc4b76b-b7b7-46bf-8f68-70119ea591ce}</MetaDataID>
        private ConnectableControls.TextBox AttributeName;
        /// <MetaDataID>{e4ce5f18-3e7b-4a05-b9c1-e673948ed66b}</MetaDataID>
        private ConnectableControls.Label NameLabel;
        /// <MetaDataID>{58dc2a69-9aa1-42e4-87f7-fecb9a265838}</MetaDataID>
        private System.Windows.Forms.GroupBox VisibilityGroup;
        /// <MetaDataID>{724b94e7-8ea1-400d-b7af-a1c80dd113f5}</MetaDataID>
        private ConnectableControls.RadioButton PackageBtn;
        /// <MetaDataID>{4bd2e642-cd69-45a3-9f66-516c8c67a098}</MetaDataID>
        private ConnectableControls.RadioButton ProtectedBtn;
        /// <MetaDataID>{1466fded-ef7f-453c-98fb-b789fb44458f}</MetaDataID>
        private ConnectableControls.RadioButton PrivateBtn;
        /// <MetaDataID>{55f3356b-8c9e-4dda-9c09-cecc9ecaee43}</MetaDataID>
        private ConnectableControls.RadioButton PublicBtn;
        /// <MetaDataID>{e1a96d9c-c229-4353-89f1-1269fc4f05b0}</MetaDataID>
        private ConnectableControls.Label DocumentationLabel;
        /// <MetaDataID>{beeea167-5132-460d-b340-2fec883b2b15}</MetaDataID>
        private ConnectableControls.TextBox Documentation;
        /// <MetaDataID>{8a1b07b8-7855-4a1f-ae02-41cea082a705}</MetaDataID>
        private System.Windows.Forms.GroupBox groupBox1;
        /// <MetaDataID>{5b677e23-937e-42f0-9b91-9f8932556ed7}</MetaDataID>
        private ConnectableControls.ComboBox comboBox1;
        /// <MetaDataID>{e0fddf0a-9cb5-438a-af93-7abfe2eb39e5}</MetaDataID>
        private ConnectableControls.ComboBox comboBox2;
        /// <MetaDataID>{cdc35dc1-217a-4119-9d89-ffbdfad784db}</MetaDataID>
        private ConnectableControls.Label GetLB;
        /// <MetaDataID>{44e119f6-caa0-47c7-a009-e48cbde5da1e}</MetaDataID>
        private ConnectableControls.Label SetLB;
        /// <MetaDataID>{c8eabe6b-62f6-4cde-9c32-08583068deb2}</MetaDataID>
        private ConnectableControls.CheckBox PropertyBodyChck;
        /// <MetaDataID>{cc091679-a7c6-4959-a7ba-0c1e92f8573b}</MetaDataID>
        private System.Windows.Forms.GroupBox PersistentGroup;
        /// <MetaDataID>{dc988b33-12b7-45e6-88b2-e366cfabed3c}</MetaDataID>
        private ConnectableControls.TextBox textBox1;
        /// <MetaDataID>{a6af6498-8a38-4c46-8a9e-ce720fbec58a}</MetaDataID>
        private ConnectableControls.Label label1;
        /// <MetaDataID>{45eb03ac-e90e-4c54-a98e-92359480009d}</MetaDataID>
        private ConnectableControls.CheckBox checkBox1;
        /// <MetaDataID>{5e4d3efe-e4ae-44b0-98d8-22f72844d96e}</MetaDataID>
        private ConnectableControls.TextBox textBox2;
        /// <MetaDataID>{7e0ee15e-798f-4935-976e-c930d402c334}</MetaDataID>
        private ConnectableControls.Label label2;
        /// <MetaDataID>{b296b063-083e-4611-9bf8-2109c324d275}</MetaDataID>
        private ConnectableControls.Button BackwardCompatibilityIDBtn;
        /// <MetaDataID>{47a6b823-b364-4438-8ae7-cf9500379ae1}</MetaDataID>
        private System.Windows.Forms.GroupBox groupBox2;
        /// <MetaDataID>{b45bd42d-770e-4f47-91eb-0bf6a8987806}</MetaDataID>
        private ConnectableControls.RadioButton SealedBtn;
        /// <MetaDataID>{921013f3-e072-47e8-b530-989c0d3bd23d}</MetaDataID>
        private ConnectableControls.RadioButton DefaultBtn;
        /// <MetaDataID>{473f538d-457a-43a3-9167-930c16459c61}</MetaDataID>
        private ConnectableControls.RadioButton NewBtn;
        /// <MetaDataID>{1173bf03-a3a8-4d79-a29f-3fb01f05162a}</MetaDataID>
        private ConnectableControls.RadioButton OverrideBtn;
        /// <MetaDataID>{f866938e-5833-4556-9715-af795d2f248c}</MetaDataID>
        private ConnectableControls.RadioButton Virtual;
        /// <MetaDataID>{369a273c-f8d4-448e-a6c1-a1e39539b969}</MetaDataID>
        private ConnectableControls.RadioButton AbstractBtn;
        private System.Windows.Forms.GroupBox AttributeProperties;
        private ConnectableControls.CheckBox EventChck;
        private ConnectableControls.CheckBox PropertyChck;
        private ConnectableControls.CheckBox PersistentBtn;
        private ConnectableControls.CheckBox StaticBtn;
        private ConnectableControls.Label label3;
        private ConnectableControls.ComboBox comboBox3;
        
      
       

        
        
        
        
        
    }
}