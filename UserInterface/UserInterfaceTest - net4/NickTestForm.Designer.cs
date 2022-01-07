namespace UserInterfaceTest
{
    partial class NickTestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NickTestForm));
            this.Connection = new ConnectableControls.FormConnectionControl();
            this.button1 = new ConnectableControls.Button();
            this.ClientName = new ConnectableControls.TextBox();
            this.StartDateTimePicker = new ConnectableControls.DateTimePicker();
            this.EndDateTimePicker = new ConnectableControls.DateTimePicker();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.ContainerControl = this;
            this.Connection.MasterViewControlObject = null;
            this.Connection.MetaData = ((object)(resources.GetObject("Connection.MetaData")));
            this.Connection.Name = "Connection";
            this.Connection.AssignPresentationObjectType= null;
            this.Connection.RollbackOnExitWithoutAnswer = false;
            this.Connection.RollbackOnNegativeAnswer = true;
            this.Connection.TransactionOption = OOAdvantech.Transactions.TransactionOption.Required;
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IClient";
            // 
            // button1
            // 
            this.button1.ConnectedObjectAutoUpdate = false;
            this.button1.Location = new System.Drawing.Point(521, 46);
            this.button1.Name = "button1";
            this.button1.OnClickOperationCall = ((object)(resources.GetObject("button1.OnClickOperationCall")));
            this.button1.Path = "";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.TransactionOption = OOAdvantech.Transactions.TransactionOption.Supported;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Value = null;
            this.button1.ViewControlObject = this.Connection;
            // 
            // ClientName
            // 
            this.ClientName.AcceptsTab = true;
            this.ClientName.AutoDisable = true;
            this.ClientName.ConnectedObjectAutoUpdate = false;
            this.ClientName.Location = new System.Drawing.Point(71, 86);
            this.ClientName.Name = "ClientName";
            this.ClientName.Path = "Name";
            this.ClientName.Size = new System.Drawing.Size(100, 20);
            this.ClientName.TabIndex = 2;
            this.ClientName.ViewControlObject = this.Connection;
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.AutoDisable = true;
            this.StartDateTimePicker.ConnectedObjectAutoUpdate = false;
            this.StartDateTimePicker.Location = new System.Drawing.Point(49, 34);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Path = null;
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 3;
            this.StartDateTimePicker.ViewControlObject = this.Connection;
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.AutoDisable = true;
            this.EndDateTimePicker.ConnectedObjectAutoUpdate = false;
            this.EndDateTimePicker.Location = new System.Drawing.Point(49, 60);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Path = null;
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 4;
            this.EndDateTimePicker.ViewControlObject = this.Connection;
            // 
            // NickTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 481);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.ClientName);
            this.Controls.Add(this.button1);
            this.Name = "NickTestForm";
            this.Text = "NickTestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ConnectableControls.FormConnectionControl Connection;
        private ConnectableControls.TextBox ClientName;
        private ConnectableControls.Button button1;
        private ConnectableControls.DateTimePicker EndDateTimePicker;
        private ConnectableControls.DateTimePicker StartDateTimePicker;
    }
}