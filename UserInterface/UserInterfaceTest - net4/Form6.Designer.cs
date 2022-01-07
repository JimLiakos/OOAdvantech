namespace UserInterfaceTest
{
    partial class Form6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6));
            this.textBox1 = new ConnectableControls.TextBox();
            this.formConnectionControl1 = new ConnectableControls.FormConnectionControl();
            this.textBox2 = new ConnectableControls.TextBox();
            this.searchTextBox1 = new ConnectableControls.SearchTextBox();
            this.comboBox1 = new ConnectableControls.ComboBox();
            this.comboBox2 = new ConnectableControls.ComboBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.AllowDrag = false;
            this.textBox1.AutoDisable = true;
            this.textBox1.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.textBox1.EnableProperty.Path = null;
            this.textBox1.Location = new System.Drawing.Point(124, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.Path = "NewName";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.textBox1.ViewControlObject = this.formConnectionControl1;
            // 
            // formConnectionControl1
            // 
            this.formConnectionControl1.AllowDrag = false;
            this.formConnectionControl1.AllowDropOperationCall = ((object)(resources.GetObject("formConnectionControl1.AllowDropOperationCall")));
            this.formConnectionControl1.AssignPresentationObjectType = "";
            this.formConnectionControl1.ContainerControl = this;
            this.formConnectionControl1.CreatePresentationObjectAnyway = false;
            this.formConnectionControl1.DragDropOperationCall = ((object)(resources.GetObject("formConnectionControl1.DragDropOperationCall")));
            this.formConnectionControl1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.formConnectionControl1.IniateTransactionOnInstanceSet = false;
            this.formConnectionControl1.MasterViewControlObject = null;
            this.formConnectionControl1.Name = "formConnectionControl1";
            this.formConnectionControl1.RollbackOnExitWithoutAnswer = false;
            this.formConnectionControl1.RollbackOnNegativeAnswer = true;
            this.formConnectionControl1.SkipErrorCheck = false;
            this.formConnectionControl1.TransactionObjectLockTimeOut = 0;
            this.formConnectionControl1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.formConnectionControl1.ViewControlObjectAssembly = "UserInterfaceTest";
            this.formConnectionControl1.ViewControlObjectType = null;
            // 
            // textBox2
            // 
            this.textBox2.AllowDrag = false;
            this.textBox2.AutoDisable = true;
            this.textBox2.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.textBox2.EnableProperty.Path = null;
            this.textBox2.Location = new System.Drawing.Point(124, 127);
            this.textBox2.Name = "textBox2";
            this.textBox2.Path = "RealObject.Name";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 1;
            this.textBox2.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.textBox2.ViewControlObject = this.formConnectionControl1;
            // 
            // searchTextBox1
            // 
            this.searchTextBox1.AllowDrag = false;
            this.searchTextBox1.AllowDropOperationCall = ((object)(resources.GetObject("searchTextBox1.AllowDropOperationCall")));
            this.searchTextBox1.AssignPresentationObjectType = "UserInterfaceTest.ClientPresentationObject";
            this.searchTextBox1.AutoDisable = true;
            this.searchTextBox1.ConnectedObjectAutoUpdate = false;
            this.searchTextBox1.DisplayMember = "Name";
            this.searchTextBox1.DragDropOperationCall = ((object)(resources.GetObject("searchTextBox1.DragDropOperationCall")));
            this.searchTextBox1.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.searchTextBox1.DropDownHeight = 106;
            // 
            // 
            // 
            this.searchTextBox1.EnableProperty.Path = null;
            this.searchTextBox1.InsertOperationCall = ((object)(resources.GetObject("searchTextBox1.InsertOperationCall")));
            this.searchTextBox1.Location = new System.Drawing.Point(124, 201);
            this.searchTextBox1.Name = "searchTextBox1";
            this.searchTextBox1.OperationCall = ((object)(resources.GetObject("searchTextBox1.OperationCall")));
            this.searchTextBox1.Path = "Name";
            this.searchTextBox1.RemoveOperationCall = ((object)(resources.GetObject("searchTextBox1.RemoveOperationCall")));
            this.searchTextBox1.Size = new System.Drawing.Size(131, 19);
            this.searchTextBox1.TabIndex = 2;
            this.searchTextBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.searchTextBox1.ViewControlObject = this.formConnectionControl1;
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
            this.comboBox1.DisplayMember = null;
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
            this.comboBox1.Location = new System.Drawing.Point(124, 90);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.NullValueName = "";
            this.comboBox1.OperationCall = ((object)(resources.GetObject("comboBox1.OperationCall")));
            this.comboBox1.Path = null;
            this.comboBox1.PreLoaded = true;
            this.comboBox1.RemoveOperationCall = ((object)(resources.GetObject("comboBox1.RemoveOperationCall")));
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox1.ViewControlObject = null;
            this.comboBox1.WarnigMessageOnRemove = null;
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
            this.comboBox2.DisplayMember = null;
            this.comboBox2.DragDropOperationCall = ((object)(resources.GetObject("comboBox2.DragDropOperationCall")));
            this.comboBox2.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.comboBox2.EnableCondition = null;
            // 
            // 
            // 
            this.comboBox2.EnableProperty.Path = null;
            this.comboBox2.Enumeration = "";
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.InsertOperationCall = ((object)(resources.GetObject("comboBox2.InsertOperationCall")));
            this.comboBox2.IntegralHeight = false;
            this.comboBox2.Location = new System.Drawing.Point(103, 12);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.NullValueName = "";
            this.comboBox2.OperationCall = ((object)(resources.GetObject("comboBox2.OperationCall")));
            this.comboBox2.Path = null;
            this.comboBox2.PreLoaded = true;
            this.comboBox2.RemoveOperationCall = ((object)(resources.GetObject("comboBox2.RemoveOperationCall")));
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.UpdateStyle = ConnectableControls.UpdateStyle.Immediately;
            this.comboBox2.ViewControlObject = null;
            this.comboBox2.WarnigMessageOnRemove = null;
            // 
            // Form6
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 266);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.searchTextBox1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "Form6";
            this.Text = "Form6";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConnectableControls.TextBox textBox1;
        private ConnectableControls.TextBox textBox2;
        private ConnectableControls.SearchTextBox searchTextBox1;
        private ConnectableControls.FormConnectionControl formConnectionControl1;
        private ConnectableControls.ComboBox comboBox1;
        private ConnectableControls.ComboBox comboBox2;
    }
}