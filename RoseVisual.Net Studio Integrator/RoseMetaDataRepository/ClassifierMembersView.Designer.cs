namespace RoseMetaDataRepository
{
    partial class ClassifierMembersView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassifierMembersView));
            this.ConnectionControl = new ConnectableControls.FormConnectionControl();
            this.Members = new ConnectableControls.List.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.Members)).BeginInit();
            this.SuspendLayout();
            // 
            // ConnectionControl
            // 
            this.ConnectionControl.AllowDrag = false;
            this.ConnectionControl.AllowDropOperationCall = ((object)(resources.GetObject("ConnectionControl.AllowDropOperationCall")));
            this.ConnectionControl.AssignPresentationObjectType = "";
            this.ConnectionControl.ContainerControl = this;
            this.ConnectionControl.CreatePresentationObjectAnyway = false;
            this.ConnectionControl.DragDropOperationCall = ((object)(resources.GetObject("ConnectionControl.DragDropOperationCall")));
            this.ConnectionControl.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.ConnectionControl.IniateTransactionOnInstanceSet = false;
            this.ConnectionControl.MasterViewControlObject = null;
            this.ConnectionControl.Name = "ConnectionControl";
            this.ConnectionControl.RollbackOnExitWithoutAnswer = false;
            this.ConnectionControl.RollbackOnNegativeAnswer = true;
            this.ConnectionControl.SkipErrorCheck = false;
            this.ConnectionControl.TransactionObjectLockTimeOut = 0;
            this.ConnectionControl.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.ConnectionControl.ViewControlObjectAssembly = null;
            this.ConnectionControl.ViewControlObjectType = "RoseMetaDataRepository.ClassifierPresentation";
            // 
            // Members
            // 
            this.Members.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Members.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Members.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.Members.EnabledProperty.Path = null;
            this.Members.FocusedBackColor = System.Drawing.Color.White;
            this.Members.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            // 
            // 
            // 
            this.Members.ListConnection.AllowDrag = false;
            this.Members.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.Members.ListConnection.AssignPresentationObjectType = "";
            this.Members.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.Members.ListConnection.ConnectedObjectAutoUpdate = false;
            this.Members.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.Members.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.Members.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Members.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.Members.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.Members.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.Members.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.Members.ListConnection.Path = "Members";
            this.Members.ListConnection.SelectionMember = "SelectedMember";
            this.Members.ListConnection.ViewControlObject = this.ConnectionControl;
            this.Members.Location = new System.Drawing.Point(0, 0);
            this.Members.Margin = new System.Windows.Forms.Padding(0);
            this.Members.Name = "Members";
            this.Members.RowHeight = 15;
            this.Members.Size = new System.Drawing.Size(779, 467);
            this.Members.TabIndex = 0;
            this.Members.Text = "table1";
            this.Members.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MembersMouseUp);
            // 
            // ClassifierMembersView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(779, 467);
            this.ControlBox = false;
            this.Controls.Add(this.Members);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassifierMembersView";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.Members)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public ConnectableControls.FormConnectionControl ConnectionControl;
        private ConnectableControls.List.ListView Members;
    }
}