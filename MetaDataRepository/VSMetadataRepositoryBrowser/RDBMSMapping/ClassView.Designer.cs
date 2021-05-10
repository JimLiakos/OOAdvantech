namespace VSMetadataRepositoryBrowser.RDBMSMapping
{
    partial class ClassView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{be2d61e4-3d50-4bce-9522-65279b4cb7fd}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{313ecba5-4534-40fc-a3d9-0be01668cc0c}</MetaDataID>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        /// <MetaDataID>{3e8a58cf-c2c4-46b9-8de1-0355890f7953}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassView));
            this.RDBMSMappingContextCBX = new ConnectableControls.ComboBox();
            this.MappingContextLB = new System.Windows.Forms.Label();
            this.MainTableCBX = new ConnectableControls.ComboBox();
            this.ClassGroupBox = new System.Windows.Forms.GroupBox();
            this.RefreshBtn = new ConnectableControls.Button();
            this.label5 = new ConnectableControls.Label();
            this.comboBox2 = new ConnectableControls.ComboBox();
            this.checkBox1 = new ConnectableControls.CheckBox();
            this.ManualSelectRelationReferenceTable = new ConnectableControls.CheckBox();
            this.label3 = new ConnectableControls.Label();
            this.comboBox1 = new ConnectableControls.ComboBox();
            this.label4 = new ConnectableControls.Label();
            this.ReferenceColumn = new ConnectableControls.ComboBox();
            this.RelationTableLB = new System.Windows.Forms.Label();
            this.TableWithReferenceColumns = new ConnectableControls.ComboBox();
            this.RelatedClassLB = new System.Windows.Forms.Label();
            this.RelatedConcreteClass = new ConnectableControls.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ObjectIDMappedColumnsCBX = new ConnectableControls.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MappedColumnsCBX = new ConnectableControls.ComboBox();
            this.TableLB = new System.Windows.Forms.Label();
            this.treeView1 = new ConnectableControls.Tree.TreeView();
            this.nodeIcon1 = new ConnectableControls.Tree.NodeControls.NodeIcon();
            this.nodeTextBox1 = new ConnectableControls.Tree.NodeControls.NodeTextBox();
            this.ClassGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "VSMetadataRepositoryBrowser.ClassViewPresantation";
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.ViewControlObjectAssembly = "OOAdvantech";
            this.Connection.ViewControlObjectType = "OOAdvantech.MetaDataRepository.Class";
            // 
            // RDBMSMappingContextCBX
            // 
            this.RDBMSMappingContextCBX.AllowDrag = false;
            this.RDBMSMappingContextCBX.AllowDropOperationCall = ((object)(resources.GetObject("RDBMSMappingContextCBX.AllowDropOperationCall")));
            this.RDBMSMappingContextCBX.AssignPresentationObjectType = "";
            this.RDBMSMappingContextCBX.AutoDisable = true;
            this.RDBMSMappingContextCBX.AutoInsert = false;
            this.RDBMSMappingContextCBX.AutoSuggest = false;
            this.RDBMSMappingContextCBX.ChooseFromEnum = false;
            this.RDBMSMappingContextCBX.ConnectedObjectAutoUpdate = false;
            this.RDBMSMappingContextCBX.DisplayMember = "Name";
            this.RDBMSMappingContextCBX.DragDropOperationCall = ((object)(resources.GetObject("RDBMSMappingContextCBX.DragDropOperationCall")));
            this.RDBMSMappingContextCBX.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.RDBMSMappingContextCBX.EnableCondition = null;
            // 
            // 
            // 
            this.RDBMSMappingContextCBX.EnableProperty.Path = null;
            this.RDBMSMappingContextCBX.Enumeration = "";
            this.RDBMSMappingContextCBX.FormattingEnabled = true;
            this.RDBMSMappingContextCBX.InsertOperationCall = ((object)(resources.GetObject("RDBMSMappingContextCBX.InsertOperationCall")));
            this.RDBMSMappingContextCBX.IntegralHeight = false;
            this.RDBMSMappingContextCBX.Location = new System.Drawing.Point(355, 19);
            this.RDBMSMappingContextCBX.Name = "RDBMSMappingContextCBX";
            this.RDBMSMappingContextCBX.NullValueName = "";
            this.RDBMSMappingContextCBX.OperationCall = ((object)(resources.GetObject("RDBMSMappingContextCBX.OperationCall")));
            this.RDBMSMappingContextCBX.Path = "SelectedRDBMSMappingContext";
            this.RDBMSMappingContextCBX.PreLoaded = true;
            this.RDBMSMappingContextCBX.RemoveOperationCall = ((object)(resources.GetObject("RDBMSMappingContextCBX.RemoveOperationCall")));
            this.RDBMSMappingContextCBX.Size = new System.Drawing.Size(235, 21);
            this.RDBMSMappingContextCBX.TabIndex = 0;
            this.RDBMSMappingContextCBX.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.RDBMSMappingContextCBX.ViewControlObject = this.Connection;
            this.RDBMSMappingContextCBX.WarnigMessageOnRemove = null;
            // 
            // MappingContextLB
            // 
            this.MappingContextLB.AutoSize = true;
            this.MappingContextLB.Location = new System.Drawing.Point(262, 22);
            this.MappingContextLB.Name = "MappingContextLB";
            this.MappingContextLB.Size = new System.Drawing.Size(87, 13);
            this.MappingContextLB.TabIndex = 1;
            this.MappingContextLB.Text = "Mapping Context";
            // 
            // MainTableCBX
            // 
            this.MainTableCBX.AllowDrag = false;
            this.MainTableCBX.AllowDropOperationCall = ((object)(resources.GetObject("MainTableCBX.AllowDropOperationCall")));
            this.MainTableCBX.AssignPresentationObjectType = "";
            this.MainTableCBX.AutoDisable = true;
            this.MainTableCBX.AutoInsert = false;
            this.MainTableCBX.AutoSuggest = false;
            this.MainTableCBX.ChooseFromEnum = false;
            this.MainTableCBX.ConnectedObjectAutoUpdate = false;
            this.MainTableCBX.DisplayMember = "Name";
            this.MainTableCBX.DragDropOperationCall = ((object)(resources.GetObject("MainTableCBX.DragDropOperationCall")));
            this.MainTableCBX.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.MainTableCBX.EnableCondition = null;
            // 
            // 
            // 
            this.MainTableCBX.EnableProperty.Path = null;
            this.MainTableCBX.Enumeration = "";
            this.MainTableCBX.FormattingEnabled = true;
            this.MainTableCBX.InsertOperationCall = ((object)(resources.GetObject("MainTableCBX.InsertOperationCall")));
            this.MainTableCBX.IntegralHeight = false;
            this.MainTableCBX.Location = new System.Drawing.Point(355, 46);
            this.MainTableCBX.Name = "MainTableCBX";
            this.MainTableCBX.NullValueName = "";
            this.MainTableCBX.OperationCall = ((object)(resources.GetObject("MainTableCBX.OperationCall")));
            this.MainTableCBX.Path = "MainTable";
            this.MainTableCBX.PreLoaded = true;
            this.MainTableCBX.RemoveOperationCall = ((object)(resources.GetObject("MainTableCBX.RemoveOperationCall")));
            this.MainTableCBX.Size = new System.Drawing.Size(235, 21);
            this.MainTableCBX.TabIndex = 2;
            this.MainTableCBX.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.MainTableCBX.ViewControlObject = this.Connection;
            this.MainTableCBX.WarnigMessageOnRemove = null;
            // 
            // ClassGroupBox
            // 
            this.ClassGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ClassGroupBox.Controls.Add(this.RefreshBtn);
            this.ClassGroupBox.Controls.Add(this.label5);
            this.ClassGroupBox.Controls.Add(this.comboBox2);
            this.ClassGroupBox.Controls.Add(this.checkBox1);
            this.ClassGroupBox.Controls.Add(this.ManualSelectRelationReferenceTable);
            this.ClassGroupBox.Controls.Add(this.label3);
            this.ClassGroupBox.Controls.Add(this.comboBox1);
            this.ClassGroupBox.Controls.Add(this.label4);
            this.ClassGroupBox.Controls.Add(this.ReferenceColumn);
            this.ClassGroupBox.Controls.Add(this.RelationTableLB);
            this.ClassGroupBox.Controls.Add(this.TableWithReferenceColumns);
            this.ClassGroupBox.Controls.Add(this.RelatedClassLB);
            this.ClassGroupBox.Controls.Add(this.RelatedConcreteClass);
            this.ClassGroupBox.Controls.Add(this.label2);
            this.ClassGroupBox.Controls.Add(this.ObjectIDMappedColumnsCBX);
            this.ClassGroupBox.Controls.Add(this.label1);
            this.ClassGroupBox.Controls.Add(this.MappedColumnsCBX);
            this.ClassGroupBox.Controls.Add(this.TableLB);
            this.ClassGroupBox.Controls.Add(this.treeView1);
            this.ClassGroupBox.Controls.Add(this.MappingContextLB);
            this.ClassGroupBox.Controls.Add(this.MainTableCBX);
            this.ClassGroupBox.Controls.Add(this.RDBMSMappingContextCBX);
            this.ClassGroupBox.ImeMode = System.Windows.Forms.ImeMode.On;
            this.ClassGroupBox.Location = new System.Drawing.Point(9, 11);
            this.ClassGroupBox.Name = "ClassGroupBox";
            this.ClassGroupBox.Size = new System.Drawing.Size(671, 446);
            this.ClassGroupBox.TabIndex = 4;
            this.ClassGroupBox.TabStop = false;
            this.ClassGroupBox.Text = "Class Mapping Data";
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.AllowDrag = false;
            this.RefreshBtn.ConnectedObjectAutoUpdate = false;
            this.RefreshBtn.Location = new System.Drawing.Point(596, 19);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.OnClickOperationCall = ((object)(resources.GetObject("RefreshBtn.OnClickOperationCall")));
            this.RefreshBtn.Path = "";
            this.RefreshBtn.SaveButton = false;
            this.RefreshBtn.Size = new System.Drawing.Size(56, 23);
            this.RefreshBtn.TabIndex = 22;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Value = null;
            this.RefreshBtn.ViewControlObject = this.Connection;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(206, 229);
            this.label5.Name = "label5";
            this.label5.Path = null;
            this.label5.Size = new System.Drawing.Size(143, 23);
            this.label5.TabIndex = 21;
            this.label5.Text = "Indexer column";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 
            // 
            this.label5.TextEnableProperty.Path = "FirstRoleIndexerColumnLabel";
            this.label5.ViewControlObject = this.Connection;
            // 
            // comboBox2
            // 
            this.comboBox2.AllowDrag = false;
            this.comboBox2.AllowDropOperationCall = ((object)(resources.GetObject("comboBox2.AllowDropOperationCall")));
            this.comboBox2.AssignPresentationObjectType = "";
            this.comboBox2.AutoDisable = true;
            this.comboBox2.AutoInsert = false;
            this.comboBox2.AutoSuggest = false;
            this.comboBox2.ChooseFromEnum = false;
            this.comboBox2.ConnectedObjectAutoUpdate = false;
            this.comboBox2.DisplayMember = "Name";
            this.comboBox2.DragDropOperationCall = ((object)(resources.GetObject("comboBox2.DragDropOperationCall")));
            this.comboBox2.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox2.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox2.EnableProperty.Path = "FirstRoleHasIndexerColumn";
            this.comboBox2.Enumeration = "";
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.InsertOperationCall = ((object)(resources.GetObject("comboBox2.InsertOperationCall")));
            this.comboBox2.IntegralHeight = false;
            this.comboBox2.Location = new System.Drawing.Point(355, 231);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.NullValueName = "";
            this.comboBox2.OperationCall = ((object)(resources.GetObject("comboBox2.OperationCall")));
            this.comboBox2.Path = "FirstRoleIndexerColumn";
            this.comboBox2.PreLoaded = true;
            this.comboBox2.RemoveOperationCall = ((object)(resources.GetObject("comboBox2.RemoveOperationCall")));
            this.comboBox2.Size = new System.Drawing.Size(235, 21);
            this.comboBox2.TabIndex = 20;
            this.comboBox2.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox2.ViewControlObject = this.Connection;
            this.comboBox2.WarnigMessageOnRemove = null;
            // 
            // checkBox1
            // 
            this.checkBox1.AllowDrag = false;
            this.checkBox1.AutoDisable = true;
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.checkBox1.EnableProperty.Path = "ChooseRoleWithReferenceColumns";
            this.checkBox1.Location = new System.Drawing.Point(265, 181);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Path = "ReferenceColumnsOnRoleB";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox1.Size = new System.Drawing.Size(183, 17);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "Reference columns on other type";
            this.checkBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.ViewControlObject = this.Connection;
            // 
            // ManualSelectRelationReferenceTable
            // 
            this.ManualSelectRelationReferenceTable.AllowDrag = false;
            this.ManualSelectRelationReferenceTable.AutoDisable = true;
            this.ManualSelectRelationReferenceTable.AutoSize = true;
            this.ManualSelectRelationReferenceTable.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ManualSelectRelationReferenceTable.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.ManualSelectRelationReferenceTable.EnableProperty.Path = "ChooseTableWithReferenceColumns";
            this.ManualSelectRelationReferenceTable.Location = new System.Drawing.Point(482, 181);
            this.ManualSelectRelationReferenceTable.Name = "ManualSelectRelationReferenceTable";
            this.ManualSelectRelationReferenceTable.Path = "SelectManualTableWithReferenceColumns";
            this.ManualSelectRelationReferenceTable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ManualSelectRelationReferenceTable.Size = new System.Drawing.Size(94, 17);
            this.ManualSelectRelationReferenceTable.TabIndex = 18;
            this.ManualSelectRelationReferenceTable.Text = "Manual Select";
            this.ManualSelectRelationReferenceTable.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ManualSelectRelationReferenceTable.UseVisualStyleBackColor = true;
            this.ManualSelectRelationReferenceTable.ViewControlObject = this.Connection;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(206, 256);
            this.label3.Name = "label3";
            this.label3.Path = null;
            this.label3.Size = new System.Drawing.Size(143, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Reference columns";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 
            // 
            this.label3.TextEnableProperty.Path = "SecondRoleReferenceColumnLabel";
            this.label3.ViewControlObject = this.Connection;
            // 
            // comboBox1
            // 
            this.comboBox1.AllowDrag = false;
            this.comboBox1.AllowDropOperationCall = ((object)(resources.GetObject("comboBox1.AllowDropOperationCall")));
            this.comboBox1.AssignPresentationObjectType = "";
            this.comboBox1.AutoDisable = true;
            this.comboBox1.AutoInsert = false;
            this.comboBox1.AutoSuggest = false;
            this.comboBox1.ChooseFromEnum = false;
            this.comboBox1.ConnectedObjectAutoUpdate = false;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.DragDropOperationCall = ((object)(resources.GetObject("comboBox1.DragDropOperationCall")));
            this.comboBox1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox1.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox1.EnableProperty.Path = "UseRelationTable";
            this.comboBox1.Enumeration = "";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.InsertOperationCall = ((object)(resources.GetObject("comboBox1.InsertOperationCall")));
            this.comboBox1.IntegralHeight = false;
            this.comboBox1.Location = new System.Drawing.Point(355, 258);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.NullValueName = "";
            this.comboBox1.OperationCall = ((object)(resources.GetObject("comboBox1.OperationCall")));
            this.comboBox1.Path = "SecondRoleReferenceColumn";
            this.comboBox1.PreLoaded = true;
            this.comboBox1.RemoveOperationCall = ((object)(resources.GetObject("comboBox1.RemoveOperationCall")));
            this.comboBox1.Size = new System.Drawing.Size(235, 21);
            this.comboBox1.TabIndex = 16;
            this.comboBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox1.ViewControlObject = this.Connection;
            this.comboBox1.WarnigMessageOnRemove = null;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(206, 203);
            this.label4.Name = "label4";
            this.label4.Path = null;
            this.label4.Size = new System.Drawing.Size(143, 23);
            this.label4.TabIndex = 15;
            this.label4.Text = "Reference columns";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 
            // 
            this.label4.TextEnableProperty.Path = "FirstRoleReferenceColumnLabel";
            this.label4.ViewControlObject = this.Connection;
            // 
            // ReferenceColumn
            // 
            this.ReferenceColumn.AllowDrag = false;
            this.ReferenceColumn.AllowDropOperationCall = ((object)(resources.GetObject("ReferenceColumn.AllowDropOperationCall")));
            this.ReferenceColumn.AssignPresentationObjectType = "";
            this.ReferenceColumn.AutoDisable = true;
            this.ReferenceColumn.AutoInsert = false;
            this.ReferenceColumn.AutoSuggest = false;
            this.ReferenceColumn.ChooseFromEnum = false;
            this.ReferenceColumn.ConnectedObjectAutoUpdate = false;
            this.ReferenceColumn.DisplayMember = "Name";
            this.ReferenceColumn.DragDropOperationCall = ((object)(resources.GetObject("ReferenceColumn.DragDropOperationCall")));
            this.ReferenceColumn.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ReferenceColumn.EnableCondition = null;
            // 
            // 
            // 
            this.ReferenceColumn.EnableProperty.Path = "ChooseTableWithReferenceColumns";
            this.ReferenceColumn.Enumeration = "";
            this.ReferenceColumn.FormattingEnabled = true;
            this.ReferenceColumn.InsertOperationCall = ((object)(resources.GetObject("ReferenceColumn.InsertOperationCall")));
            this.ReferenceColumn.IntegralHeight = false;
            this.ReferenceColumn.Location = new System.Drawing.Point(355, 205);
            this.ReferenceColumn.Name = "ReferenceColumn";
            this.ReferenceColumn.NullValueName = "";
            this.ReferenceColumn.OperationCall = ((object)(resources.GetObject("ReferenceColumn.OperationCall")));
            this.ReferenceColumn.Path = "FirstRoleReferenceColumn";
            this.ReferenceColumn.PreLoaded = true;
            this.ReferenceColumn.RemoveOperationCall = ((object)(resources.GetObject("ReferenceColumn.RemoveOperationCall")));
            this.ReferenceColumn.Size = new System.Drawing.Size(235, 21);
            this.ReferenceColumn.TabIndex = 13;
            this.ReferenceColumn.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ReferenceColumn.ViewControlObject = this.Connection;
            this.ReferenceColumn.WarnigMessageOnRemove = null;
            // 
            // RelationTableLB
            // 
            this.RelationTableLB.AutoSize = true;
            this.RelationTableLB.Location = new System.Drawing.Point(203, 154);
            this.RelationTableLB.Name = "RelationTableLB";
            this.RelationTableLB.Size = new System.Drawing.Size(146, 13);
            this.RelationTableLB.TabIndex = 12;
            this.RelationTableLB.Text = "Table with reference columns";
            // 
            // TableWithReferenceColumns
            // 
            this.TableWithReferenceColumns.AllowDrag = false;
            this.TableWithReferenceColumns.AllowDropOperationCall = ((object)(resources.GetObject("TableWithReferenceColumns.AllowDropOperationCall")));
            this.TableWithReferenceColumns.AssignPresentationObjectType = "";
            this.TableWithReferenceColumns.AutoDisable = true;
            this.TableWithReferenceColumns.AutoInsert = false;
            this.TableWithReferenceColumns.AutoSuggest = false;
            this.TableWithReferenceColumns.ChooseFromEnum = false;
            this.TableWithReferenceColumns.ConnectedObjectAutoUpdate = false;
            this.TableWithReferenceColumns.DisplayMember = "Name";
            this.TableWithReferenceColumns.DragDropOperationCall = ((object)(resources.GetObject("TableWithReferenceColumns.DragDropOperationCall")));
            this.TableWithReferenceColumns.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.TableWithReferenceColumns.EnableCondition = null;
            // 
            // 
            // 
            this.TableWithReferenceColumns.EnableProperty.Path = "ChooseTableWithReferenceColumns";
            this.TableWithReferenceColumns.Enumeration = "";
            this.TableWithReferenceColumns.FormattingEnabled = true;
            this.TableWithReferenceColumns.InsertOperationCall = ((object)(resources.GetObject("TableWithReferenceColumns.InsertOperationCall")));
            this.TableWithReferenceColumns.IntegralHeight = false;
            this.TableWithReferenceColumns.Location = new System.Drawing.Point(355, 154);
            this.TableWithReferenceColumns.Name = "TableWithReferenceColumns";
            this.TableWithReferenceColumns.NullValueName = "";
            this.TableWithReferenceColumns.OperationCall = ((object)(resources.GetObject("TableWithReferenceColumns.OperationCall")));
            this.TableWithReferenceColumns.Path = "TableWithReferenceColumns";
            this.TableWithReferenceColumns.PreLoaded = true;
            this.TableWithReferenceColumns.RemoveOperationCall = ((object)(resources.GetObject("TableWithReferenceColumns.RemoveOperationCall")));
            this.TableWithReferenceColumns.Size = new System.Drawing.Size(235, 21);
            this.TableWithReferenceColumns.TabIndex = 11;
            this.TableWithReferenceColumns.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.TableWithReferenceColumns.ViewControlObject = this.Connection;
            this.TableWithReferenceColumns.WarnigMessageOnRemove = null;
            // 
            // RelatedClassLB
            // 
            this.RelatedClassLB.AutoSize = true;
            this.RelatedClassLB.Location = new System.Drawing.Point(231, 130);
            this.RelatedClassLB.Name = "RelatedClassLB";
            this.RelatedClassLB.Size = new System.Drawing.Size(118, 13);
            this.RelatedClassLB.TabIndex = 10;
            this.RelatedClassLB.Text = "Related Concrete Class";
            // 
            // RelatedConcreteClass
            // 
            this.RelatedConcreteClass.AllowDrag = false;
            this.RelatedConcreteClass.AllowDropOperationCall = ((object)(resources.GetObject("RelatedConcreteClass.AllowDropOperationCall")));
            this.RelatedConcreteClass.AssignPresentationObjectType = "";
            this.RelatedConcreteClass.AutoDisable = true;
            this.RelatedConcreteClass.AutoInsert = false;
            this.RelatedConcreteClass.AutoSuggest = false;
            this.RelatedConcreteClass.ChooseFromEnum = false;
            this.RelatedConcreteClass.ConnectedObjectAutoUpdate = false;
            this.RelatedConcreteClass.DisplayMember = "Name";
            this.RelatedConcreteClass.DragDropOperationCall = ((object)(resources.GetObject("RelatedConcreteClass.DragDropOperationCall")));
            this.RelatedConcreteClass.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.RelatedConcreteClass.EnableCondition = null;
            // 
            // 
            // 
            this.RelatedConcreteClass.EnableProperty.Path = "ChooseRelatedClass";
            this.RelatedConcreteClass.Enumeration = "";
            this.RelatedConcreteClass.FormattingEnabled = true;
            this.RelatedConcreteClass.InsertOperationCall = ((object)(resources.GetObject("RelatedConcreteClass.InsertOperationCall")));
            this.RelatedConcreteClass.IntegralHeight = false;
            this.RelatedConcreteClass.Location = new System.Drawing.Point(355, 127);
            this.RelatedConcreteClass.Name = "RelatedConcreteClass";
            this.RelatedConcreteClass.NullValueName = "";
            this.RelatedConcreteClass.OperationCall = ((object)(resources.GetObject("RelatedConcreteClass.OperationCall")));
            this.RelatedConcreteClass.Path = "SelectedRelationConcreteClass";
            this.RelatedConcreteClass.PreLoaded = true;
            this.RelatedConcreteClass.RemoveOperationCall = ((object)(resources.GetObject("RelatedConcreteClass.RemoveOperationCall")));
            this.RelatedConcreteClass.Size = new System.Drawing.Size(235, 21);
            this.RelatedConcreteClass.TabIndex = 9;
            this.RelatedConcreteClass.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.RelatedConcreteClass.ViewControlObject = this.Connection;
            this.RelatedConcreteClass.WarnigMessageOnRemove = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(220, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "ObjectID Mapped Column";
            // 
            // ObjectIDMappedColumnsCBX
            // 
            this.ObjectIDMappedColumnsCBX.AllowDrag = false;
            this.ObjectIDMappedColumnsCBX.AllowDropOperationCall = ((object)(resources.GetObject("ObjectIDMappedColumnsCBX.AllowDropOperationCall")));
            this.ObjectIDMappedColumnsCBX.AssignPresentationObjectType = "";
            this.ObjectIDMappedColumnsCBX.AutoDisable = true;
            this.ObjectIDMappedColumnsCBX.AutoInsert = false;
            this.ObjectIDMappedColumnsCBX.AutoSuggest = false;
            this.ObjectIDMappedColumnsCBX.ChooseFromEnum = false;
            this.ObjectIDMappedColumnsCBX.ConnectedObjectAutoUpdate = false;
            this.ObjectIDMappedColumnsCBX.DisplayMember = "Name";
            this.ObjectIDMappedColumnsCBX.DragDropOperationCall = ((object)(resources.GetObject("ObjectIDMappedColumnsCBX.DragDropOperationCall")));
            this.ObjectIDMappedColumnsCBX.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ObjectIDMappedColumnsCBX.EnableCondition = null;
            // 
            // 
            // 
            this.ObjectIDMappedColumnsCBX.EnableProperty.Path = null;
            this.ObjectIDMappedColumnsCBX.Enumeration = "";
            this.ObjectIDMappedColumnsCBX.FormattingEnabled = true;
            this.ObjectIDMappedColumnsCBX.InsertOperationCall = ((object)(resources.GetObject("ObjectIDMappedColumnsCBX.InsertOperationCall")));
            this.ObjectIDMappedColumnsCBX.IntegralHeight = false;
            this.ObjectIDMappedColumnsCBX.Location = new System.Drawing.Point(355, 73);
            this.ObjectIDMappedColumnsCBX.Name = "ObjectIDMappedColumnsCBX";
            this.ObjectIDMappedColumnsCBX.NullValueName = "";
            this.ObjectIDMappedColumnsCBX.OperationCall = ((object)(resources.GetObject("ObjectIDMappedColumnsCBX.OperationCall")));
            this.ObjectIDMappedColumnsCBX.Path = "ObjectIDMappedColumn";
            this.ObjectIDMappedColumnsCBX.PreLoaded = true;
            this.ObjectIDMappedColumnsCBX.RemoveOperationCall = ((object)(resources.GetObject("ObjectIDMappedColumnsCBX.RemoveOperationCall")));
            this.ObjectIDMappedColumnsCBX.Size = new System.Drawing.Size(235, 21);
            this.ObjectIDMappedColumnsCBX.TabIndex = 7;
            this.ObjectIDMappedColumnsCBX.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ObjectIDMappedColumnsCBX.ViewControlObject = this.Connection;
            this.ObjectIDMappedColumnsCBX.WarnigMessageOnRemove = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(224, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Member Mapped Column";
            // 
            // MappedColumnsCBX
            // 
            this.MappedColumnsCBX.AllowDrag = false;
            this.MappedColumnsCBX.AllowDropOperationCall = ((object)(resources.GetObject("MappedColumnsCBX.AllowDropOperationCall")));
            this.MappedColumnsCBX.AssignPresentationObjectType = "";
            this.MappedColumnsCBX.AutoDisable = true;
            this.MappedColumnsCBX.AutoInsert = false;
            this.MappedColumnsCBX.AutoSuggest = false;
            this.MappedColumnsCBX.ChooseFromEnum = false;
            this.MappedColumnsCBX.ConnectedObjectAutoUpdate = false;
            this.MappedColumnsCBX.DisplayMember = "Name";
            this.MappedColumnsCBX.DragDropOperationCall = ((object)(resources.GetObject("MappedColumnsCBX.DragDropOperationCall")));
            this.MappedColumnsCBX.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.MappedColumnsCBX.EnableCondition = null;
            // 
            // 
            // 
            this.MappedColumnsCBX.EnableProperty.Path = null;
            this.MappedColumnsCBX.Enumeration = "";
            this.MappedColumnsCBX.FormattingEnabled = true;
            this.MappedColumnsCBX.InsertOperationCall = ((object)(resources.GetObject("MappedColumnsCBX.InsertOperationCall")));
            this.MappedColumnsCBX.IntegralHeight = false;
            this.MappedColumnsCBX.Location = new System.Drawing.Point(355, 100);
            this.MappedColumnsCBX.Name = "MappedColumnsCBX";
            this.MappedColumnsCBX.NullValueName = "";
            this.MappedColumnsCBX.OperationCall = ((object)(resources.GetObject("MappedColumnsCBX.OperationCall")));
            this.MappedColumnsCBX.Path = "MappedColumn";
            this.MappedColumnsCBX.PreLoaded = true;
            this.MappedColumnsCBX.RemoveOperationCall = ((object)(resources.GetObject("MappedColumnsCBX.RemoveOperationCall")));
            this.MappedColumnsCBX.Size = new System.Drawing.Size(235, 21);
            this.MappedColumnsCBX.TabIndex = 5;
            this.MappedColumnsCBX.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.MappedColumnsCBX.ViewControlObject = this.Connection;
            this.MappedColumnsCBX.WarnigMessageOnRemove = null;
            // 
            // TableLB
            // 
            this.TableLB.AutoSize = true;
            this.TableLB.Location = new System.Drawing.Point(315, 49);
            this.TableLB.Name = "TableLB";
            this.TableLB.Size = new System.Drawing.Size(34, 13);
            this.TableLB.TabIndex = 4;
            this.TableLB.Text = "Table";
            // 
            // treeView1
            // 
            this.treeView1.AllowDrag = false;
            this.treeView1.AllowDropOperationCall = ((object)(resources.GetObject("treeView1.AllowDropOperationCall")));
            this.treeView1.AssignPresentationObjectType = "";
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("treeView1.BeforeShowContextMenuOperationCall")));
            this.treeView1.CheckUncheckPath = null;
            this.treeView1.ConnectedObjectAutoUpdate = false;
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView1.CutOperationCall = ((object)(resources.GetObject("treeView1.CutOperationCall")));
            this.treeView1.DeleteNodeOperationCall = ((object)(resources.GetObject("treeView1.DeleteNodeOperationCall")));
            this.treeView1.DisplayMember = "Name";
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
            this.treeView1.Location = new System.Drawing.Point(6, 19);
            this.treeView1.MetaData = ((object)(resources.GetObject("treeView1.MetaData")));
            this.treeView1.Model = null;
            this.treeView1.Name = "treeView1";
            this.treeView1.NodeControls.Add(this.nodeIcon1);
            this.treeView1.NodeControls.Add(this.nodeTextBox1);
            this.treeView1.NodeObjectType = "VSMetadataRepositoryBrowser.MetaObjectTreeNode";
            this.treeView1.Path = "ClassAsTreeNode";
            this.treeView1.RecursiveLoadSteps = 0;
            this.treeView1.SelectedNode = null;
            this.treeView1.SelectionMember = "SelectedMember";
            this.treeView1.Size = new System.Drawing.Size(189, 411);
            this.treeView1.SubNodesProperty = "ContainedObjects";
            this.treeView1.TabIndex = 3;
            this.treeView1.Text = "treeView1";
            this.treeView1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.treeView1.ViewControlObject = this.Connection;
            // 
            // ClassView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ClassGroupBox);
            this.Name = "ClassView";
            this.Size = new System.Drawing.Size(690, 467);
            this.ClassGroupBox.ResumeLayout(false);
            this.ClassGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{4e589be5-bd95-4033-8593-49adbf881295}</MetaDataID>
        private ConnectableControls.ComboBox RDBMSMappingContextCBX;
        /// <MetaDataID>{552bcf7f-bbfc-453a-9e5b-5e06857a639b}</MetaDataID>
        private System.Windows.Forms.Label MappingContextLB;
        /// <MetaDataID>{1e2fbe74-70aa-4243-b9a5-9f7ccd0c83a0}</MetaDataID>
        private ConnectableControls.ComboBox MainTableCBX;
        /// <MetaDataID>{3635c65a-6bfa-43ec-a3d8-018b3636e096}</MetaDataID>
        private System.Windows.Forms.GroupBox ClassGroupBox;
        /// <MetaDataID>{000712d0-e9c8-4b7f-93b9-51194fa16b8f}</MetaDataID>
        private ConnectableControls.Tree.TreeView treeView1;
        /// <MetaDataID>{4bc6fa96-8425-433f-8549-1b00b13fea27}</MetaDataID>
        private ConnectableControls.Tree.NodeControls.NodeIcon nodeIcon1;
        /// <MetaDataID>{fbfe6647-de31-463c-a345-97095b7dfd78}</MetaDataID>
        private ConnectableControls.Tree.NodeControls.NodeTextBox nodeTextBox1;
        /// <MetaDataID>{78a88830-7f1f-481c-91ec-94bdf1e90fef}</MetaDataID>
        private System.Windows.Forms.Label TableLB;
        /// <MetaDataID>{0c08ba36-a886-4615-96c4-c2223735959e}</MetaDataID>
        private ConnectableControls.ComboBox MappedColumnsCBX;
        /// <MetaDataID>{c52b8c7b-9b35-4777-8643-ba957dd37ad6}</MetaDataID>
        private System.Windows.Forms.Label label1;
        /// <MetaDataID>{11d0b2f4-8bae-4539-bd59-06641d3d5e88}</MetaDataID>
        private System.Windows.Forms.Label label2;
        /// <MetaDataID>{61050256-d46c-45c3-a7e6-5e4ece45eceb}</MetaDataID>
        private ConnectableControls.ComboBox ObjectIDMappedColumnsCBX;
        /// <MetaDataID>{384a52b1-80ec-4ca4-b026-f0b24f766d72}</MetaDataID>
        private System.Windows.Forms.Label RelatedClassLB;
        /// <MetaDataID>{c9a4f4ec-77e0-48cf-9af5-e8490f592022}</MetaDataID>
        private ConnectableControls.ComboBox RelatedConcreteClass;
        /// <MetaDataID>{787961ca-9427-449a-99ef-9b36185da93d}</MetaDataID>
        private System.Windows.Forms.Label RelationTableLB;
        /// <MetaDataID>{71a030e8-8f8d-4cfb-91ed-f681d97e14d2}</MetaDataID>
        private ConnectableControls.ComboBox TableWithReferenceColumns;
        /// <MetaDataID>{cdd95f10-f429-456b-85c6-ec41f6bf53a2}</MetaDataID>
        private ConnectableControls.ComboBox ReferenceColumn;
        /// <MetaDataID>{eec9dda2-47d9-4f76-a018-a8cdac0b982e}</MetaDataID>
        private ConnectableControls.Label label4;
        /// <MetaDataID>{70f13545-0652-444b-9138-7d0d5072634b}</MetaDataID>
        private ConnectableControls.Label label3;
        /// <MetaDataID>{d56443fe-02dc-472f-ad2d-97ff8698c0bd}</MetaDataID>
        private ConnectableControls.ComboBox comboBox1;
        /// <MetaDataID>{2ce875e1-072a-4d97-89a0-4dd4a540fe8f}</MetaDataID>
        private ConnectableControls.CheckBox ManualSelectRelationReferenceTable;
        /// <MetaDataID>{e8f05c6f-eb36-4805-88fb-981253f01ccd}</MetaDataID>
        private ConnectableControls.CheckBox checkBox1;
        /// <MetaDataID>{bb5fd00b-2635-489c-9669-94bb74021c1a}</MetaDataID>
        private ConnectableControls.Label label5;
        /// <MetaDataID>{71d91287-7457-4c34-b4fb-f56cf96dddd8}</MetaDataID>
        private ConnectableControls.ComboBox comboBox2;
        private ConnectableControls.Button RefreshBtn;
    }
}
