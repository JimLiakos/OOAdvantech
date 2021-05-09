namespace OOAdvantech.VSUMLMetaDataRepository
{
    partial class ComponentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentForm));
            this.ComponentPages = new System.Windows.Forms.TabControl();
            this.General = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.NameLabel = new ConnectableControls.Label();
            this.ObjectConnectionControl = new ConnectableControls.FormConnectionControl();
            this.ComponentName = new ConnectableControls.TextBox();
            this.DocumentationLabel = new ConnectableControls.Label();
            this.Documentation = new ConnectableControls.TextBox();
            this.ProjectSelector = new ConnectableControls.ComboBox();
            this.label1 = new ConnectableControls.Label();
            this.Realizes = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.AllClasses = new ConnectableControls.CheckBox();
            this.ClassifierListView = new ConnectableControls.List.ListView();
            this.ComponentPages.SuspendLayout();
            this.General.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.Realizes.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClassifierListView)).BeginInit();
            this.SuspendLayout();
            // 
            // ComponentPages
            // 
            this.ComponentPages.Controls.Add(this.General);
            this.ComponentPages.Controls.Add(this.Realizes);
            this.ComponentPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComponentPages.Location = new System.Drawing.Point(0, 0);
            this.ComponentPages.Name = "ComponentPages";
            this.ComponentPages.SelectedIndex = 0;
            this.ComponentPages.Size = new System.Drawing.Size(378, 416);
            this.ComponentPages.TabIndex = 1;
            // 
            // General
            // 
            this.General.Controls.Add(this.tableLayoutPanel1);
            this.General.Location = new System.Drawing.Point(4, 22);
            this.General.Name = "General";
            this.General.Padding = new System.Windows.Forms.Padding(3);
            this.General.Size = new System.Drawing.Size(370, 390);
            this.General.TabIndex = 0;
            this.General.Text = "General";
            this.General.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.NameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ComponentName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.DocumentationLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Documentation, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ProjectSelector, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(364, 384);
            this.tableLayoutPanel1.TabIndex = 0;
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
            // ObjectConnectionControl
            // 
            this.ObjectConnectionControl.AllowDrag = false;
            this.ObjectConnectionControl.AllowDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.AllowDropOperationCall")));
            this.ObjectConnectionControl.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.ComponentPresentationObject";
            this.ObjectConnectionControl.ContainerControl = this;
            this.ObjectConnectionControl.CreatePresentationObjectAnyway = false;
            this.ObjectConnectionControl.DragDropOperationCall = ((object)(resources.GetObject("ObjectConnectionControl.DragDropOperationCall")));
            this.ObjectConnectionControl.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ObjectConnectionControl.IniateTransactionOnInstanceSet = false;
            this.ObjectConnectionControl.MasterViewControlObject = null;
            this.ObjectConnectionControl.Name = "ObjectConnectionControl";
            this.ObjectConnectionControl.RollbackOnExitWithoutAnswer = true;
            this.ObjectConnectionControl.RollbackOnNegativeAnswer = true;
            this.ObjectConnectionControl.SkipErrorCheck = false;
            this.ObjectConnectionControl.TransactionObjectLockTimeOut = 0;
            this.ObjectConnectionControl.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.ObjectConnectionControl.ViewControlObjectAssembly = "VSUMLMetaDataRepository";
            this.ObjectConnectionControl.ViewControlObjectType = "OOAdvantech.VSUMLMetaDataRepository.Component";
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
            this.ComponentName.Size = new System.Drawing.Size(298, 20);
            this.ComponentName.TabIndex = 1;
            this.ComponentName.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.ComponentName.ViewControlObject = this.ObjectConnectionControl;
            // 
            // DocumentationLabel
            // 
            this.DocumentationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DocumentationLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.DocumentationLabel, 2);
            this.DocumentationLabel.Location = new System.Drawing.Point(3, 67);
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
            this.Documentation.Location = new System.Drawing.Point(3, 83);
            this.Documentation.Multiline = true;
            this.Documentation.Name = "Documentation";
            this.Documentation.Path = null;
            this.Documentation.Size = new System.Drawing.Size(358, 298);
            this.Documentation.TabIndex = 3;
            this.Documentation.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.Documentation.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ProjectSelector
            // 
            this.ProjectSelector.AllowDrag = false;
            this.ProjectSelector.AllowDropOperationCall = ((object)(resources.GetObject("ProjectSelector.AllowDropOperationCall")));
            this.ProjectSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectSelector.AssignPresentationObjectType = "";
            this.ProjectSelector.AutoDisable = true;
            this.ProjectSelector.AutoInsert = false;
            this.ProjectSelector.AutoSuggest = false;
            this.ProjectSelector.ChooseFromEnum = false;
            this.ProjectSelector.ConnectedObjectAutoUpdate = false;
            this.ProjectSelector.DisplayMember = "Name";
            this.ProjectSelector.DragDropOperationCall = ((object)(resources.GetObject("ProjectSelector.DragDropOperationCall")));
            this.ProjectSelector.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ProjectSelector.EnableCondition = null;
            // 
            // 
            // 
            this.ProjectSelector.EnableProperty.Path = null;
            this.ProjectSelector.Enumeration = "";
            this.ProjectSelector.FormattingEnabled = true;
            this.ProjectSelector.InsertOperationCall = ((object)(resources.GetObject("ProjectSelector.InsertOperationCall")));
            this.ProjectSelector.IntegralHeight = false;
            this.ProjectSelector.Location = new System.Drawing.Point(63, 34);
            this.ProjectSelector.Name = "ProjectSelector";
            this.ProjectSelector.NullValueName = "";
            this.ProjectSelector.OperationCall = ((object)(resources.GetObject("ProjectSelector.OperationCall")));
            this.ProjectSelector.Path = "AssignedProject";
            this.ProjectSelector.PreLoaded = true;
            this.ProjectSelector.RemoveOperationCall = ((object)(resources.GetObject("ProjectSelector.RemoveOperationCall")));
            this.ProjectSelector.Size = new System.Drawing.Size(298, 21);
            this.ProjectSelector.TabIndex = 4;
            this.ProjectSelector.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.ProjectSelector.ViewControlObject = this.ObjectConnectionControl;
            this.ProjectSelector.WarnigMessageOnRemove = null;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 38);
            this.label1.Name = "label1";
            this.label1.Path = null;
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Project:";
            // 
            // 
            // 
            this.label1.TextEnableProperty.Path = null;
            this.label1.ViewControlObject = this.ObjectConnectionControl;
            // 
            // Realizes
            // 
            this.Realizes.Controls.Add(this.tableLayoutPanel2);
            this.Realizes.Location = new System.Drawing.Point(4, 22);
            this.Realizes.Name = "Realizes";
            this.Realizes.Padding = new System.Windows.Forms.Padding(3);
            this.Realizes.Size = new System.Drawing.Size(370, 390);
            this.Realizes.TabIndex = 1;
            this.Realizes.Text = "Realizes";
            this.Realizes.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.AllClasses, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ClassifierListView, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.291667F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.70834F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(364, 384);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // AllClasses
            // 
            this.AllClasses.AllowDrag = false;
            this.AllClasses.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AllClasses.AutoDisable = true;
            this.AllClasses.AutoSize = true;
            this.AllClasses.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.AllClasses.EnableProperty.Path = null;
            this.AllClasses.Location = new System.Drawing.Point(3, 5);
            this.AllClasses.Name = "AllClasses";
            this.AllClasses.Path = "ShowAllClasses";
            this.AllClasses.Size = new System.Drawing.Size(104, 17);
            this.AllClasses.TabIndex = 0;
            this.AllClasses.Text = "Show all classes";
            this.AllClasses.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.AllClasses.UseVisualStyleBackColor = true;
            this.AllClasses.ViewControlObject = this.ObjectConnectionControl;
            // 
            // ClassifierListView
            // 
            this.ClassifierListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.ClassifierListView, 2);
            this.ClassifierListView.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.ClassifierListView.EnabledProperty.Path = null;
            this.ClassifierListView.FocusedBackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.ClassifierListView.ListConnection.AllowDrag = false;
            this.ClassifierListView.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.ClassifierListView.ListConnection.AssignPresentationObjectType = null;
            this.ClassifierListView.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.ClassifierListView.ListConnection.ConnectedObjectAutoUpdate = false;
            this.ClassifierListView.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.ClassifierListView.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.ClassifierListView.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ClassifierListView.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.ClassifierListView.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.ClassifierListView.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.ClassifierListView.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.ClassifierListView.ListConnection.Path = "Classifiers";
            this.ClassifierListView.ListConnection.SelectionMember = null;
            this.ClassifierListView.ListConnection.ViewControlObject = this.ObjectConnectionControl;
            this.ClassifierListView.Location = new System.Drawing.Point(3, 30);
            this.ClassifierListView.Name = "ClassifierListView";
            this.ClassifierListView.RowHeight = 17;
            this.ClassifierListView.Size = new System.Drawing.Size(358, 351);
            this.ClassifierListView.TabIndex = 1;
            this.ClassifierListView.Text = "ClassifierListView";
            // 
            // ComponentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 416);
            this.Controls.Add(this.ComponentPages);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ComponentForm";
            this.ShowInTaskbar = false;
            this.Text = "Component Properties";
            this.ComponentPages.ResumeLayout(false);
            this.General.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.Realizes.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClassifierListView)).EndInit();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private ConnectableControls.CheckBox AllClasses;
        private ConnectableControls.List.ListView ClassifierListView;
        private ConnectableControls.ComboBox comboBox1;
        private ConnectableControls.ComboBox ProjectSelector;
        private ConnectableControls.Label label1;
        
       
        
    }
}