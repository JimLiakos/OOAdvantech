namespace UserInterfaceTest
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new ConnectableControls.Button();
            this.OrdersReportBtn = new ConnectableControls.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(448, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.AllowDrag = false;
            this.button2.ConnectedObjectAutoUpdate = false;
            this.button2.Location = new System.Drawing.Point(448, 70);
            this.button2.Name = "button2";
            this.button2.OnClickOperationCall = ((object)(resources.GetObject("button2.OnClickOperationCall")));
            this.button2.Path = "";
            this.button2.SaveButton = false;
            this.button2.Size = new System.Drawing.Size(104, 41);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            // 
            // 
            // 
            this.button2.TextProperty.Path = null;
            this.button2.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Value = null;
            this.button2.ViewControlObject = null;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // OrdersReportBtn
            // 
            this.OrdersReportBtn.AllowDrag = false;
            this.OrdersReportBtn.ConnectedObjectAutoUpdate = false;
            this.OrdersReportBtn.Location = new System.Drawing.Point(448, 126);
            this.OrdersReportBtn.Name = "OrdersReportBtn";
            this.OrdersReportBtn.OnClickOperationCall = ((object)(resources.GetObject("OrdersReportBtn.OnClickOperationCall")));
            this.OrdersReportBtn.Path = "";
            this.OrdersReportBtn.SaveButton = false;
            this.OrdersReportBtn.Size = new System.Drawing.Size(104, 41);
            this.OrdersReportBtn.TabIndex = 2;
            this.OrdersReportBtn.Text = "Order Report";
            // 
            // 
            // 
            this.OrdersReportBtn.TextProperty.Path = null;
            this.OrdersReportBtn.TransactionOption = OOAdvantech.Transactions.TransactionOption.Suppress;
            this.OrdersReportBtn.UseVisualStyleBackColor = true;
            this.OrdersReportBtn.Value = null;
            this.OrdersReportBtn.ViewControlObject = null;
            this.OrdersReportBtn.Click += new System.EventHandler(this.OrdersReportBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 332);
            this.Controls.Add(this.OrdersReportBtn);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private ConnectableControls.ViewControlObject viewControlObject1;
        private System.Windows.Forms.Button button1;
        private ConnectableControls.Button button2;
        private ConnectableControls.Button OrdersReportBtn;
    }
}