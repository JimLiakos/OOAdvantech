namespace OOAdvantech.VSUMLMetaDataRepository
{
    partial class OperationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperationForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Documentation = new ConnectableControls.TextBox();
            this.ObjectConnectionControl = new ConnectableControls.FormConnectionControl();
            this.AttributeName = new ConnectableControls.TextBox();
            this.NameLabel = new ConnectableControls.Label();
            this.AttributeProperties = new System.Windows.Forms.GroupBox();
            this.SealedBtn = new ConnectableControls.RadioButton();
            this.DefaultBtn = new ConnectableControls.RadioButton();
            this.NewBtn = new ConnectableControls.RadioButton();
            this.OverrideBtn = new ConnectableControls.RadioButton();
            this.Virtual = new ConnectableControls.RadioButton();
            this.AbstractBtn = new ConnectableControls.RadioButton();
            this.DocumentationLabel = new ConnectableControls.Label();
            this.StaticChk = new ConnectableControls.CheckBox();
            this.VisibilityGroup = new System.Windows.Forms.GroupBox();
            this.PublicBtn = new ConnectableControls.RadioButton();
            this.PrivateBtn = new ConnectableControls.RadioButton();
            this.ProtectedBtn = new ConnectableControls.RadioButton();
            this.PackageBtn = new ConnectableControls.RadioButton();
            this.DetailTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new ConnectableControls.Label();
            this.comboBox1 = new ConnectableControls.ComboBox();
            this.label2 = new ConnectableControls.Label();
            this.listView1 = new ConnectableControls.List.ListView();
            this.button1 = new ConnectableControls.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.AttributeProperties.SuspendLayout();
            this.VisibilityGroup.SuspendLayout();
            this.DetailTab.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.DetailTab);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(380, 394);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(372, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Documentation, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.AttributeName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.NameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.AttributeProperties, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.DocumentationLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.StaticChk, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.VisibilityGroup, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 368);
            this.tableLayoutPanel1.TabIndex = 1;
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
            this.Documentation.Size = new System.Drawing.Size(364, 197);
            this.Documentation.TabIndex = 7;
            this.Documentation.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Documentation.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ObjectConnectionControl
            // 
            this.ObjectConnectionControl.AllowDrag = false;
            this.ObjectConnectionControl.AllowDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.AllowDropOperationCall")));
            this.ObjectConnectionControl.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.OperationPresentationObject";
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
            this.ObjectConnectionControl.ViewControlObjectAssembly = "OOAdvantech";
            this.ObjectConnectionControl.ViewControlObjectType = "OOAdvantech.MetaDataRepository.BehavioralFeature";
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
            this.AttributeName.Location = new System.Drawing.Point(63, 5);
            this.AttributeName.Name = "AttributeName";
            this.AttributeName.Path = "RealObject.Name";
            this.AttributeName.Size = new System.Drawing.Size(304, 20);
            this.AttributeName.TabIndex = 2;
            this.AttributeName.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.AttributeName.ViewControlObject = this.ObjectConnectionControl;
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(19, 8);
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
            // AttributeProperties
            // 
            this.AttributeProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.AttributeProperties, 2);
            this.AttributeProperties.Controls.Add(this.SealedBtn);
            this.AttributeProperties.Controls.Add(this.DefaultBtn);
            this.AttributeProperties.Controls.Add(this.NewBtn);
            this.AttributeProperties.Controls.Add(this.OverrideBtn);
            this.AttributeProperties.Controls.Add(this.Virtual);
            this.AttributeProperties.Controls.Add(this.AbstractBtn);
            this.AttributeProperties.Location = new System.Drawing.Point(3, 103);
            this.AttributeProperties.Name = "AttributeProperties";
            this.AttributeProperties.Size = new System.Drawing.Size(364, 39);
            this.AttributeProperties.TabIndex = 5;
            this.AttributeProperties.TabStop = false;
            this.AttributeProperties.Text = "Override Kind";
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
            this.AbstractBtn.EnableProperty.Path = "CanBeAbstract";
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
            // StaticChk
            // 
            this.StaticChk.AllowDrag = false;
            this.StaticChk.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StaticChk.AutoDisable = true;
            this.StaticChk.AutoSize = true;
            this.StaticChk.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.StaticChk.EnableProperty.Path = "CanBeStatic";
            this.StaticChk.Location = new System.Drawing.Point(63, 76);
            this.StaticChk.Name = "StaticChk";
            this.StaticChk.Path = "IsStatic";
            this.StaticChk.Size = new System.Drawing.Size(53, 17);
            this.StaticChk.TabIndex = 8;
            this.StaticChk.Text = "Static";
            this.StaticChk.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.StaticChk.UseVisualStyleBackColor = true;
            this.StaticChk.ViewControlObject = this.ObjectConnectionControl;
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
            this.VisibilityGroup.Location = new System.Drawing.Point(3, 33);
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
            this.PublicBtn.EnableProperty.Path = "CanChangeVisibility";
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
            this.PrivateBtn.EnableProperty.Path = "CanChangeVisibility";
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
            this.ProtectedBtn.EnableProperty.Path = "CanChangeVisibility";
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
            this.PackageBtn.EnableProperty.Path = "CanChangeVisibility";
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
            // DetailTab
            // 
            this.DetailTab.Controls.Add(this.tableLayoutPanel2);
            this.DetailTab.Location = new System.Drawing.Point(4, 22);
            this.DetailTab.Name = "DetailTab";
            this.DetailTab.Size = new System.Drawing.Size(372, 368);
            this.DetailTab.TabIndex = 1;
            this.DetailTab.Text = "Detail";
            this.DetailTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.22222F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 77.77778F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBox1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.listView1, 0, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(369, 362);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Path = null;
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Return Type:";
            // 
            // 
            // 
            this.label1.TextEnableProperty.Path = null;
            this.label1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // comboBox1
            // 
            this.comboBox1.AllowDrag = false;
            this.comboBox1.AllowDropOperationCall = ((object)(resources.GetObject("comboBox1.AllowDropOperationCall")));
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.AssignPresentationObjectType = "";
            this.comboBox1.AutoDisable = true;
            this.comboBox1.AutoInsert = true;
            this.comboBox1.AutoSuggest = false;
            this.comboBox1.ChooseFromEnum = false;
            this.comboBox1.ConnectedObjectAutoUpdate = false;
            this.comboBox1.DisplayMember = "";
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
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Location = new System.Drawing.Point(84, 34);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.NullValueName = "";
            this.comboBox1.OperationCall = ((object)(resources.GetObject("comboBox1.OperationCall")));
            this.comboBox1.Path = "ReturnType";
            this.comboBox1.PreLoaded = true;
            this.comboBox1.RemoveOperationCall = ((object)(resources.GetObject("comboBox1.RemoveOperationCall")));
            this.comboBox1.Size = new System.Drawing.Size(282, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.comboBox1.ViewControlObject = this.ObjectConnectionControl;
            this.comboBox1.WarnigMessageOnRemove = null;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label2, 2);
            this.label2.Location = new System.Drawing.Point(3, 77);
            this.label2.Name = "label2";
            this.label2.Path = null;
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Operation Parameters:";
            // 
            // 
            // 
            this.label2.TextEnableProperty.Path = null;
            this.label2.ViewControlObject = this.ObjectConnectionControl;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.listView1, 2);
            this.listView1.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.listView1.EnabledProperty.Path = null;
            this.listView1.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.listView1.ListConnection.AllowDrag = false;
            this.listView1.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.listView1.ListConnection.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.ParameterPresentationObject";
            this.listView1.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.listView1.ListConnection.ConnectedObjectAutoUpdate = false;
            this.listView1.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.listView1.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.listView1.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.listView1.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.listView1.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.listView1.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.listView1.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.listView1.ListConnection.Path = "Parameters";
            this.listView1.ListConnection.SelectionMember = null;
            this.listView1.ListConnection.ViewControlObject = this.ObjectConnectionControl;
            this.listView1.Location = new System.Drawing.Point(3, 93);
            this.listView1.Name = "listView1";
            this.listView1.RowHeight = 15;
            this.listView1.Size = new System.Drawing.Size(363, 266);
            this.listView1.TabIndex = 3;
            this.listView1.Text = "listView1";
            // 
            // button1
            // 
            this.button1.AllowDrag = false;
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.ConnectedObjectAutoUpdate = false;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(153, 397);
            this.button1.Name = "button1";
            this.button1.OnClickOperationCall = ((object)(resources.GetObject("button1.OnClickOperationCall")));
            this.button1.Path = "";
            this.button1.SaveButton = false;
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            // 
            // 
            // 
            this.button1.TextProperty.Path = null;
            this.button1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Value = null;
            this.button1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // OperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 425);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperationForm";
            this.ShowInTaskbar = false;
            this.Text = "Operation Properties";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.AttributeProperties.ResumeLayout(false);
            this.AttributeProperties.PerformLayout();
            this.VisibilityGroup.ResumeLayout(false);
            this.VisibilityGroup.PerformLayout();
            this.DetailTab.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private ConnectableControls.FormConnectionControl ObjectConnectionControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ConnectableControls.TextBox Documentation;
        private ConnectableControls.TextBox AttributeName;
        private ConnectableControls.Label NameLabel;
        private System.Windows.Forms.GroupBox VisibilityGroup;
        private ConnectableControls.RadioButton PublicBtn;
        private ConnectableControls.RadioButton PrivateBtn;
        private ConnectableControls.RadioButton ProtectedBtn;
        private ConnectableControls.RadioButton PackageBtn;
        private System.Windows.Forms.GroupBox AttributeProperties;
        private ConnectableControls.Label DocumentationLabel;
        private ConnectableControls.RadioButton NewBtn;
        private ConnectableControls.RadioButton OverrideBtn;
        private ConnectableControls.RadioButton Virtual;
        private ConnectableControls.RadioButton AbstractBtn;
        private ConnectableControls.RadioButton DefaultBtn;
        private ConnectableControls.CheckBox StaticChk;
        private ConnectableControls.RadioButton SealedBtn;
        private ConnectableControls.Button button1;
        private System.Windows.Forms.TabPage DetailTab;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ConnectableControls.Label label1;
        private ConnectableControls.ComboBox comboBox1;
        private ConnectableControls.Label label2;
        private ConnectableControls.List.ListView listView1;
    }
}