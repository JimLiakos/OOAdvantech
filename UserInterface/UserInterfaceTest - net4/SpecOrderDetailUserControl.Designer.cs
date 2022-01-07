namespace UserInterfaceTest
{
    partial class SpecOrderDetailUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{294dcbb7-97e3-435f-927c-a190e777b97b}</MetaDataID>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecOrderDetailUserControl));
            this.textBox1 = new ConnectableControls.TextBox();
            this.textBox2 = new ConnectableControls.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Amount";
            // 
            // SpecOrderDetailUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Name = "SpecOrderDetailUserControl";
            this.Size = new System.Drawing.Size(226, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ConnectableControls.TextBox textBox1;
        private ConnectableControls.TextBox textBox2;
        /// <MetaDataID>{4d1d95f4-736c-43c4-9ae6-a086c8ae9d36}</MetaDataID>
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
