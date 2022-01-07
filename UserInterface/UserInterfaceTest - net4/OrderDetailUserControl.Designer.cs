namespace UserInterfaceTest
{
    partial class OrderDetailUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderDetailUserControl));
            this.textBox1 = new ConnectableControls.TextBox();
            this.textBox2 = new ConnectableControls.TextBox();
            this.SuspendLayout();
            // 
            // Connection
            // 
            this.Connection.AllowDropOperationCall = ((object)(resources.GetObject("Connection.AllowDropOperationCall")));
            this.Connection.DragDropOperationCall = ((object)(resources.GetObject("Connection.DragDropOperationCall")));
            this.Connection.ViewControlObjectType = "AbstractionsAndPersistency.IOrderDetail";
            // 
            // textBox1
            // 
            this.textBox1.AllowDrag = false;
            this.textBox1.AutoDisable = true;
            this.textBox1.Location = new System.Drawing.Point(77, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Path = "Name";
            this.textBox1.Size = new System.Drawing.Size(128, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.UpdateStyle = ConnectableControls.UpdateStyle.OnLostFocus;
            this.textBox1.ViewControlObject = this.Connection;
            // 
            // textBox2
            // 

            this.textBox2.AllowDrag = false;
            this.textBox2.AutoDisable = true;
            this.textBox2.Location = new System.Drawing.Point(77, 39);
            this.textBox2.Name = "textBox2";
            this.textBox2.Path = "Quantity.Amount";
            this.textBox2.Size = new System.Drawing.Size(128, 20);
            this.textBox2.TabIndex = 1;
            this.textBox2.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.textBox2.ViewControlObject = this.Connection;
            // 
            // OrderDetailUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "OrderDetailUserControl";
            this.Size = new System.Drawing.Size(226, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConnectableControls.TextBox textBox1;
        private ConnectableControls.TextBox textBox2;
    }
}
