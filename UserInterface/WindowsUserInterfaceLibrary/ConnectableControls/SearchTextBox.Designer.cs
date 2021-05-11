using System.Collections.Generic;
using System.ComponentModel;
namespace ConnectableControls
{
    /// <MetaDataID>{137131C0-2D25-4BA4-B045-1BCC34FB1F03}</MetaDataID>
    partial class SearchTextBox
    {

        /// <MetaDataID>{68792c0d-fdcc-4525-b058-cb0af882e010}</MetaDataID>
        protected List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> _DependencyProperties = new List<OOAdvantech.UserInterface.Runtime.IDependencyProperty>();
        /// <MetaDataID>{c08dcb11-6957-42db-a5c5-641701d32aa6}</MetaDataID>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<OOAdvantech.UserInterface.Runtime.IDependencyProperty> DependencyProperties
        {
            get
            {
                return _DependencyProperties;
            }
        }

        /// <MetaDataID>{f6fe930b-9bb8-40ea-bfa5-ecbf9d0bfac5}</MetaDataID>
        public virtual void InitializeControl()
        {

        }

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{0352042f-4ff3-4c76-8ca1-2c405ed5b6b8}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{C11B515E-94B4-4735-9E7B-F984C7D659E0}</MetaDataID>
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
        /// <MetaDataID>{FECC154B-4D55-4578-8B47-62950CD581FC}</MetaDataID>
        private void InitializeComponent()
        {
            this.TextBox = new System.Windows.Forms.TextBox();
            this.SearchBtn = new ConnectableControls.SearchButton();
            this.SuspendLayout();
            // 
            // TextBox
            // 
       
            this.TextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.TextBox.Location = new System.Drawing.Point(3, 3);
            this.TextBox.Name = "TextBox";
            this.TextBox.Size = new System.Drawing.Size(107, 13);
            this.TextBox.TabIndex = 0;
            // 
            // SearchBtn
            // 
            this.SearchBtn.ImageIndex = 0;
            this.SearchBtn.Location = new System.Drawing.Point(107, 0);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(20, 20);
            this.SearchBtn.TabIndex = 1;
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.UserInterfaceObjectConnection = null;
            this.SearchBtn.Click += new System.EventHandler(this.OnSearchButtonClick);
            // 
            // SearchTextBox
            // 
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.TextBox);
            this.Size = new System.Drawing.Size(131, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <MetaDataID>{c0bc0879-53ea-4724-ad1d-8680fabc902e}</MetaDataID>
        internal System.Windows.Forms.TextBox TextBox;
        /// <MetaDataID>{e9ff29df-ab01-4431-b993-73f0a7ba60ca}</MetaDataID>
        internal SearchButton SearchBtn;
    }
}
