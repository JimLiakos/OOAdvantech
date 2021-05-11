namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{27B1AF42-F694-4C60-875D-784D82C1BF20}</MetaDataID>
    partial class ListMetaDataEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// <MetaDataID>{ce9e9322-a76f-4aad-9021-78727a0949a8}</MetaDataID>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        /// <MetaDataID>{5E752737-37F0-4E08-8D61-8FC2858CDDB5}</MetaDataID>
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
        /// <MetaDataID>{003A81B5-9D82-40B3-A80D-051CC46B8FFD}</MetaDataID>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListMetaDataEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DownBtn = new System.Windows.Forms.Button();
            this.UpBtn = new System.Windows.Forms.Button();
            this.ColumnsList = new System.Windows.Forms.ListView();
            this.ColumnSet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.setToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.ColumnSet.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Dock = System.Windows.Forms.DockStyle.None;
            this.treeView.LineColor = System.Drawing.Color.Black;
            this.treeView.Location = new System.Drawing.Point(-6, 12);
            this.treeView.Size = new System.Drawing.Size(190, 244);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DownBtn);
            this.groupBox1.Controls.Add(this.UpBtn);
            this.groupBox1.Controls.Add(this.ColumnsList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.groupBox1.Location = new System.Drawing.Point(3, 259);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(204, 163);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Colums";
            // 
            // DownBtn
            // 
            this.DownBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DownBtn.Image = ((System.Drawing.Image)(resources.GetObject("DownBtn.Image")));
            this.DownBtn.Location = new System.Drawing.Point(168, 58);
            this.DownBtn.Name = "DownBtn";
            this.DownBtn.Size = new System.Drawing.Size(33, 31);
            this.DownBtn.TabIndex = 2;
            this.DownBtn.UseVisualStyleBackColor = true;
            this.DownBtn.Click += new System.EventHandler(this.DownBtn_Click);
            // 
            // UpBtn
            // 
            this.UpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpBtn.Image = ((System.Drawing.Image)(resources.GetObject("UpBtn.Image")));
            this.UpBtn.Location = new System.Drawing.Point(168, 21);
            this.UpBtn.Name = "UpBtn";
            this.UpBtn.Size = new System.Drawing.Size(33, 31);
            this.UpBtn.TabIndex = 1;
            this.UpBtn.UseVisualStyleBackColor = true;
            this.UpBtn.Click += new System.EventHandler(this.UpBtn_Click);
            // 
            // ColumnsList
            // 
            this.ColumnsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ColumnsList.FullRowSelect = true;
            this.ColumnsList.HideSelection = false;
            this.ColumnsList.Location = new System.Drawing.Point(2, 12);
            this.ColumnsList.MultiSelect = false;
            this.ColumnsList.Name = "ColumnsList";
            this.ColumnsList.Size = new System.Drawing.Size(161, 147);
            this.ColumnsList.TabIndex = 0;
            this.ColumnsList.UseCompatibleStateImageBehavior = false;
            this.ColumnsList.View = System.Windows.Forms.View.List;
            this.ColumnsList.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.ColumnsList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyUp);
            this.ColumnsList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // ColumnSet
            // 
            this.ColumnSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripTextBox1,
            this.setToolStripMenuItem});
            this.ColumnSet.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.ColumnSet.Name = "contextMenuStrip1";
            this.ColumnSet.Size = new System.Drawing.Size(182, 78);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "row",
            "compo",
            "index"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 23);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.Tag = "TE";
            this.toolStripTextBox1.Text = "Name";
            this.toolStripTextBox1.ToolTipText = "Column Name";
            // 
            // setToolStripMenuItem
            // 
            this.setToolStripMenuItem.Name = "setToolStripMenuItem";
            this.setToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.setToolStripMenuItem.Text = "Set";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(213, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.tableLayoutPanel.SetRowSpan(this.propertyGrid, 2);
            this.propertyGrid.Size = new System.Drawing.Size(205, 419);
            this.propertyGrid.TabIndex = 1;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.propertyGrid, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(421, 425);
            this.tableLayoutPanel.TabIndex = 3;
            // 
            // ListMetaDataEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(421, 425);
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "ListMetaDataEditor";
            this.Text = "Lists Meta Data";
            this.Controls.SetChildIndex(this.treeView, 0);
            this.Controls.SetChildIndex(this.tableLayoutPanel, 0);
            this.groupBox1.ResumeLayout(false);
            this.ColumnSet.ResumeLayout(false);
            this.ColumnSet.PerformLayout();
            this.tableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        /// <MetaDataID>{1b43cfc6-83e2-479b-a136-4a40401aab08}</MetaDataID>
        private System.Windows.Forms.GroupBox groupBox1;
        /// <MetaDataID>{3aaf0c39-7321-4eac-bf7a-4832d81eeaae}</MetaDataID>
        private System.Windows.Forms.ListView ColumnsList;
        /// <MetaDataID>{192a54f4-c8a0-4828-b6d8-bbe747c31342}</MetaDataID>
        private System.Windows.Forms.ContextMenuStrip ColumnSet;
        /// <MetaDataID>{3d4ebb45-0d93-45e7-b5ef-1ece2fbc867c}</MetaDataID>
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        /// <MetaDataID>{ba18b936-367f-4177-bfbe-d02900c6f250}</MetaDataID>
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        /// <MetaDataID>{29279d3b-bd7f-465e-8171-5ee4ed8ba0a4}</MetaDataID>
        private System.Windows.Forms.ToolStripMenuItem setToolStripMenuItem;
        /// <MetaDataID>{616bea8b-86b7-4e2c-b07f-57541bdb74a6}</MetaDataID>
        private System.Windows.Forms.PropertyGrid propertyGrid;
        /// <MetaDataID>{f22e47fa-b1c1-4ffa-92ae-d367282dbe6a}</MetaDataID>
        private System.Windows.Forms.Button UpBtn;
        /// <MetaDataID>{6c2ec3c7-e853-43a8-adb3-0de6e14080d3}</MetaDataID>
        private System.Windows.Forms.Button DownBtn;
        /// <MetaDataID>{896bdfc4-de39-48f0-9381-7237cf377f92}</MetaDataID>
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    }
}
