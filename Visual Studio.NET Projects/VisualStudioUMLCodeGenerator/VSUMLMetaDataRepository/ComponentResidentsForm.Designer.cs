namespace OOAdvantech.VSUMLMetaDataRepository
{
    partial class ComponentResidentsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComponentResidentsForm));
            this.ModelItemsLabel = new ConnectableControls.Label();
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.listView1 = new ConnectableControls.List.ListView();
            this.UpdateModelBtn = new ConnectableControls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ModelItemsLabel
            // 
            this.ModelItemsLabel.AutoSize = true;
            this.ModelItemsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ModelItemsLabel.Location = new System.Drawing.Point(12, 9);
            this.ModelItemsLabel.Name = "ModelItemsLabel";
            this.ModelItemsLabel.Path = null;
            this.ModelItemsLabel.Size = new System.Drawing.Size(132, 17);
            this.ModelItemsLabel.TabIndex = 2;
            this.ModelItemsLabel.Text = "Component Items";
            // 
            // 
            // 
            this.ModelItemsLabel.TextEnableProperty.Path = null;
            this.ModelItemsLabel.ViewControlObject = this.Connection;
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "OOAdvantech.VSUMLMetaDataRepository.ComponentPresentationObject";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = true;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.Connection.ViewControlObjectAssembly = "VSUMLMetaDataRepository";
            this.Connection.ViewControlObjectType = "OOAdvantech.VSUMLMetaDataRepository.Component";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.DragDropMarkColor = System.Drawing.Color.Blue;
            // 
            // 
            // 
            this.listView1.EnabledProperty.Path = null;
            this.listView1.FocusedBackColor = System.Drawing.Color.White;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            // 
            // 
            // 
            this.listView1.ListConnection.AllowDrag = false;
            this.listView1.ListConnection.AllowDropOperationCall = ((object)(resources.GetObject("resource.AllowDropOperationCall")));
            this.listView1.ListConnection.AssignPresentationObjectType = "";
            this.listView1.ListConnection.BeforeShowContextMenuOperationCall = ((object)(resources.GetObject("resource.BeforeShowContextMenuOperationCall")));
            this.listView1.ListConnection.ConnectedObjectAutoUpdate = false;
            this.listView1.ListConnection.DeleteRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.DeleteRowOperationCall")));
            this.listView1.ListConnection.DragDropOperationCall = ((object)(resources.GetObject("resource.DragDropOperationCall")));
            this.listView1.ListConnection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.listView1.ListConnection.EditRowOperationCall = ((object)(resources.GetObject("resource.EditRowOperationCall")));
            this.listView1.ListConnection.InsertRowOperationCall = ((UserInterfaceMetaData.MetaDataValue)(resources.GetObject("resource.InsertRowOperationCall")));
            this.listView1.ListConnection.LoadListOperationCall = ((object)(resources.GetObject("resource.LoadListOperationCall")));
            this.listView1.ListConnection.MetaData = ((object)(resources.GetObject("resource.MetaData")));
            this.listView1.ListConnection.Path = "Residents";
            this.listView1.ListConnection.SelectionMember = null;
            this.listView1.ListConnection.ViewControlObject = this.Connection;
            this.listView1.Location = new System.Drawing.Point(12, 38);
            this.listView1.MultiSelect = true;
            this.listView1.Name = "listView1";
            this.listView1.RowHeight = 15;
            this.listView1.Size = new System.Drawing.Size(360, 363);
            this.listView1.TabIndex = 3;
            this.listView1.Text = "listView1";
            // 
            // UpdateModelBtn
            // 
            this.UpdateModelBtn.AllowDrag = false;
            this.UpdateModelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.UpdateModelBtn.ConnectedObjectAutoUpdate = false;
            this.UpdateModelBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.UpdateModelBtn.Location = new System.Drawing.Point(146, 409);
            this.UpdateModelBtn.Name = "UpdateModelBtn";
            this.UpdateModelBtn.OnClickOperationCall = ((object)(resources.GetObject("UpdateModelBtn.OnClickOperationCall")));
            this.UpdateModelBtn.Path = "";
            this.UpdateModelBtn.SaveButton = false;
            this.UpdateModelBtn.Size = new System.Drawing.Size(92, 23);
            this.UpdateModelBtn.TabIndex = 4;
            this.UpdateModelBtn.Text = "Update Model";
            // 
            // 
            // 
            this.UpdateModelBtn.TextProperty.Path = "UpdateButtonText";
            this.UpdateModelBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.UpdateModelBtn.UseVisualStyleBackColor = true;
            this.UpdateModelBtn.Value = null;
            this.UpdateModelBtn.ViewControlObject = this.Connection;
            // 
            // ComponentResidentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 444);
            this.Controls.Add(this.UpdateModelBtn);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.ModelItemsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ComponentResidentsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ComponentResidentsForm";
            ((System.ComponentModel.ISupportInitialize)(this.listView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConnectableControls.Label ModelItemsLabel;
        private ConnectableControls.List.ListView listView1;
        public ConnectableControls.FormConnectionControl Connection;
        private ConnectableControls.Button UpdateModelBtn;
    }
}