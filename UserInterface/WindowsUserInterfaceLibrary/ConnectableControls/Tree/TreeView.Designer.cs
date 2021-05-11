using System.Windows.Forms;

namespace ConnectableControls.Tree
{
	partial class TreeView
	{


        /// <MetaDataID>{82cd972b-2ad8-459d-a2f7-48e5c7a61e0b}</MetaDataID>
		private System.ComponentModel.IContainer components = null;

        /// <MetaDataID>{258d5edc-fae4-4da1-a6ae-f6b78dfa0e02}</MetaDataID>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
        /// <MetaDataID>{c7134a59-e88f-4ff9-8571-507c09d2a023}</MetaDataID>
		private void InitializeComponent()
		{
			this._vScrollBar = new System.Windows.Forms.VScrollBar();
			this._hScrollBar = new System.Windows.Forms.HScrollBar();
			this.SuspendLayout();
			// 
			// _vScrollBar
			// 
			this._vScrollBar.LargeChange = 1;
			this._vScrollBar.Location = new System.Drawing.Point(0, 0);
			this._vScrollBar.Maximum = 0;
			this._vScrollBar.Name = "_vScrollBar";
			this._vScrollBar.Size = new System.Drawing.Size(13, 80);
			this._vScrollBar.TabIndex = 1;
			this._vScrollBar.ValueChanged += new System.EventHandler(this._vScrollBar_ValueChanged);
			// 
			// _hScrollBar
			// 
			this._hScrollBar.LargeChange = 1;
			this._hScrollBar.Location = new System.Drawing.Point(0, 0);
			this._hScrollBar.Maximum = 0;
			this._hScrollBar.Name = "_hScrollBar";
			this._hScrollBar.Size = new System.Drawing.Size(80, 13);
			this._hScrollBar.TabIndex = 2;
			this._hScrollBar.ValueChanged += new System.EventHandler(this._hScrollBar_ValueChanged);
			// 
			// TreeViewAdv
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this._vScrollBar);
			this.Controls.Add(this._hScrollBar);
			this.ResumeLayout(false);

		}
		#endregion

        /// <MetaDataID>{ab84584a-6464-47ac-ac0e-6277a0590413}</MetaDataID>
		private VScrollBar _vScrollBar;
        /// <MetaDataID>{d7e64156-9303-4e3f-a96b-582575b74670}</MetaDataID>
		private HScrollBar _hScrollBar;
	}
}
