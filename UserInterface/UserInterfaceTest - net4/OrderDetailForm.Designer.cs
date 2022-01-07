namespace UserInterfaceTest
{
    partial class OrderDetailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderDetailForm));
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.textBox1 = new ConnectableControls.TextBox();
            this.button1 = new ConnectableControls.Button();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDrag = false;
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.AssignPresentationObjectType = "";
            this.Connection.ContainerControl = this;
            this.Connection.CreatePresentationObjectAnyway = false;
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.DragDropTransactionOption = OOAdvantech.UserInterface.Runtime.DragDropTransactionOptions.None;
            this.Connection.IniateTransactionOnInstanceSet = false;
            this.Connection.MasterViewControlObject = null;
            this.Connection.Name = "Connection";
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.SkipErrorCheck = false;
            this.Connection.TransactionObjectLockTimeOut = 0;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Required;
            this.Connection.ViewControlObjectAssembly = null;
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IOrderDetail";
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
            this.textBox1.Location = new System.Drawing.Point(122, 50);
            this.textBox1.Name = "textBox1";
            this.textBox1.Path = "Name";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.textBox1.ViewControlObject = this.Connection;
            // 
            // button1
            // 
            this.button1.AllowDrag = false;
            this.button1.ConnectedObjectAutoUpdate = false;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(52, 230);
            this.button1.Name = "button1";
            this.button1.OnClickOperationCall = ((object)(resources.GetObject("button1.OnClickOperationCall")));
            this.button1.Path = "";
            this.button1.SaveButton = false;
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Save";
            // 
            // 
            // 
            this.button1.TextProperty.Path = null;
            this.button1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Value = null;
            this.button1.ViewControlObject = this.Connection;
            // 
            // OrderDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 326);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "OrderDetailForm";
            this.Text = "OrderDetailForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConnectableControls.Button button1;
        private ConnectableControls.TextBox textBox1;
        public ConnectableControls.FormConnectionControl Connection;
        
    }
}