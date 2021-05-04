namespace RoseMetaDataRepository
{
    partial class ModelItemIdentityView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelItemIdentityView));
            this.ConnectionControl = new ConnectableControls.FormConnectionControl();
            this.textBox1 = new ConnectableControls.TextBox();
            this.IdentityLabel = new System.Windows.Forms.Label();
            this.OKBtn = new ConnectableControls.Button();
            this.CancelBtn = new ConnectableControls.Button();
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
            this.ConnectionControl.ViewControlObjectType = "RoseMetaDataRepository.RoseModelItem";
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
            this.textBox1.Location = new System.Drawing.Point(173, 43);
            this.textBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Path = "Identity";
            this.textBox1.Size = new System.Drawing.Size(700, 38);
            this.textBox1.TabIndex = 0;
            this.textBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.textBox1.ViewControlObject = this.ConnectionControl;
            // 
            // IdentityLabel
            // 
            this.IdentityLabel.AutoSize = true;
            this.IdentityLabel.Location = new System.Drawing.Point(32, 50);
            this.IdentityLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.IdentityLabel.Name = "IdentityLabel";
            this.IdentityLabel.Size = new System.Drawing.Size(107, 32);
            this.IdentityLabel.TabIndex = 1;
            this.IdentityLabel.Text = "Identity";
            // 
            // OKBtn
            // 
            this.OKBtn.AllowDrag = false;
            this.OKBtn.ConnectedObjectAutoUpdate = false;
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(928, 5);
            this.OKBtn.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.OnClickOperationCall = ((object)(resources.GetObject("OKBtn.OnClickOperationCall")));
            this.OKBtn.Path = "";
            this.OKBtn.SaveButton = false;
            this.OKBtn.Size = new System.Drawing.Size(200, 55);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            // 
            // 
            // 
            this.OKBtn.TextProperty.Path = null;
            this.OKBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Value = null;
            this.OKBtn.ViewControlObject = this.ConnectionControl;
            // 
            // CancelBtn
            // 
            this.CancelBtn.AllowDrag = false;
            this.CancelBtn.ConnectedObjectAutoUpdate = false;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(928, 74);
            this.CancelBtn.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.OnClickOperationCall = ((object)(resources.GetObject("CancelBtn.OnClickOperationCall")));
            this.CancelBtn.Path = "";
            this.CancelBtn.SaveButton = false;
            this.CancelBtn.Size = new System.Drawing.Size(200, 55);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            // 
            // 
            // 
            this.CancelBtn.TextProperty.Path = null;
            this.CancelBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Value = null;
            this.CancelBtn.ViewControlObject = this.ConnectionControl;
            // 
            // ModelItemIdentityView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(240F, 240F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1160, 157);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.IdentityLabel);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelItemIdentityView";
            this.ShowInTaskbar = false;
            this.Text = "Model Item";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IdentityLabel;
        private ConnectableControls.TextBox textBox1;
        private ConnectableControls.Button CancelBtn;
        private ConnectableControls.Button OKBtn;
        public ConnectableControls.FormConnectionControl ConnectionControl;
    }
}