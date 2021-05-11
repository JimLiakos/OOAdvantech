namespace ConnectableControls.Menus
{
    partial class MenuEditor
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
            this.MenuDesigner = new System.Windows.Forms.Panel();
            this.MenuPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.MenuPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MenuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuDesigner
            // 
            this.MenuDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MenuDesigner.Location = new System.Drawing.Point(235, 3);
            this.MenuDesigner.Name = "MenuDesigner";
            this.MenuDesigner.Size = new System.Drawing.Size(144, 353);
            this.MenuDesigner.TabIndex = 0;
            // 
            // MenuPropertyGrid
            // 
            this.MenuPropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.MenuPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MenuPropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.MenuPropertyGrid.Name = "MenuPropertyGrid";
            this.MenuPropertyGrid.Size = new System.Drawing.Size(226, 353);
            this.MenuPropertyGrid.TabIndex = 2;
            this.MenuPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.MenuPropertyGrid_PropertyValueChanged);
            // 
            // MenuPanel
            // 
            this.MenuPanel.ColumnCount = 2;
            this.MenuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MenuPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.MenuPanel.Controls.Add(this.MenuDesigner, 1, 0);
            this.MenuPanel.Controls.Add(this.MenuPropertyGrid, 0, 0);
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MenuPanel.Location = new System.Drawing.Point(0, 0);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.RowCount = 1;
            this.MenuPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MenuPanel.Size = new System.Drawing.Size(382, 359);
            this.MenuPanel.TabIndex = 3;
            // 
            // MenuEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 359);
            this.Controls.Add(this.MenuPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MenuEditor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MenuEditor";
            this.MenuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MenuDesigner;
        private System.Windows.Forms.PropertyGrid MenuPropertyGrid;
        private System.Windows.Forms.TableLayoutPanel MenuPanel;
    }
}

