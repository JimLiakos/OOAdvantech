namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{39009C99-965C-4D40-924D-FD68731D706D}</MetaDataID>
    partial class OperationCallMetaDataEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{0B2B67B8-2A01-42A7-B02F-ADECA261E804}</MetaDataID>
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
        /// <MetaDataID>{4E0437E2-1753-4EF8-B7EB-657E0E6496D8}</MetaDataID>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PropertyGrid = new ConnectableControls.PropertyGridEx();
            this.BrowseCodeBtn = new System.Windows.Forms.Button();
            this.RefreshPropertyValue = new ConnectableControls.CheckBox();
            this.SuspendLayout();
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.PropertyGrid.DocCommentDescription.AutoEllipsis = true;
            this.PropertyGrid.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.PropertyGrid.DocCommentDescription.Location = new System.Drawing.Point(3, 18);
            this.PropertyGrid.DocCommentDescription.Name = "";
            this.PropertyGrid.DocCommentDescription.Size = new System.Drawing.Size(326, 37);
            this.PropertyGrid.DocCommentDescription.TabIndex = 1;
            this.PropertyGrid.DocCommentImage = null;
            // 
            // 
            // 
            this.PropertyGrid.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.PropertyGrid.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.PropertyGrid.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.PropertyGrid.DocCommentTitle.Name = "";
            this.PropertyGrid.DocCommentTitle.Size = new System.Drawing.Size(326, 15);
            this.PropertyGrid.DocCommentTitle.TabIndex = 0;
            this.PropertyGrid.DocCommentTitle.UseMnemonic = false;
            this.PropertyGrid.Location = new System.Drawing.Point(-4, 28);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.PropertyGrid.Size = new System.Drawing.Size(332, 286);
            this.PropertyGrid.TabIndex = 0;
            // 
            // 
            // 
            this.PropertyGrid.ToolStrip.AccessibleName = "ToolBar";
            this.PropertyGrid.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.PropertyGrid.ToolStrip.AllowMerge = false;
            this.PropertyGrid.ToolStrip.AutoSize = false;
            this.PropertyGrid.ToolStrip.CanOverflow = false;
            this.PropertyGrid.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.PropertyGrid.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.PropertyGrid.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.PropertyGrid.ToolStrip.Name = "";
            this.PropertyGrid.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.PropertyGrid.ToolStrip.Size = new System.Drawing.Size(332, 25);
            this.PropertyGrid.ToolStrip.TabIndex = 1;
            this.PropertyGrid.ToolStrip.TabStop = true;
            this.PropertyGrid.ToolStrip.Text = "PropertyGridToolBar";
            this.PropertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.PropertyGrid_SelectedGridItemChanged);
            // 
            // BrowseCodeBtn
            // 
            this.BrowseCodeBtn.Location = new System.Drawing.Point(1, 0);
            this.BrowseCodeBtn.Name = "BrowseCodeBtn";
            this.BrowseCodeBtn.Size = new System.Drawing.Size(91, 26);
            this.BrowseCodeBtn.TabIndex = 2;
            this.BrowseCodeBtn.Text = "Browse code";
            this.BrowseCodeBtn.UseVisualStyleBackColor = true;
            this.BrowseCodeBtn.Click += new System.EventHandler(this.BrowseCode_Click);
            // 
            // RefreshPropertyValue
            // 
            this.RefreshPropertyValue.AllowDrag = false;
            this.RefreshPropertyValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshPropertyValue.AutoDisable = true;
            this.RefreshPropertyValue.ConnectedObjectAutoUpdate = false;
            // 
            // 
            // 
            this.RefreshPropertyValue.EnableProperty.Path = null;
            this.RefreshPropertyValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.RefreshPropertyValue.Location = new System.Drawing.Point(5, 286);
            this.RefreshPropertyValue.Name = "RefreshPropertyValue";
            this.RefreshPropertyValue.Path = null;
            this.RefreshPropertyValue.Size = new System.Drawing.Size(316, 28);
            this.RefreshPropertyValue.TabIndex = 1;
            this.RefreshPropertyValue.Text = "Refresh Form after operation return ";
            this.RefreshPropertyValue.UpdateStyle = ConnectableControls.UpdateStyle.OnSaveControlsValue;
            this.RefreshPropertyValue.UseVisualStyleBackColor = true;
            this.RefreshPropertyValue.ViewControlObject = null;
            this.RefreshPropertyValue.Visible = false;
            this.RefreshPropertyValue.CheckedChanged += new System.EventHandler(this.RefreshPropertyValue_CheckedChanged);
            // 
            // OperationCallMetaDataEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(328, 314);
            this.Controls.Add(this.BrowseCodeBtn);
            this.Controls.Add(this.RefreshPropertyValue);
            this.Controls.Add(this.PropertyGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "OperationCallMetaDataEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Operation Call";
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridEx PropertyGrid;
        private CheckBox RefreshPropertyValue;
        private System.Windows.Forms.Button BrowseCodeBtn;
    }
}