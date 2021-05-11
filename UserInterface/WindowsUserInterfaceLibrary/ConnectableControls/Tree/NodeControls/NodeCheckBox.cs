using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using ConnectableControls;
using ConnectableControls.Properties;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{8340469C-F40F-45D6-B2D5-76D897EFED68}</MetaDataID>
	public class NodeCheckBox : BindableControl
	{
		public const int ImageSize = 13;
		public const int Width = 13;
		private Bitmap _check;
		private Bitmap _uncheck;
		private Bitmap _unknown;

		#region Properties

		private bool _threeState;
		[DefaultValue(false)]
		public bool ThreeState
		{
			get { return _threeState; }
			set { _threeState = value; }
		}

		private bool _editEnabled = true;
		[DefaultValue(true)]
		public bool EditEnabled
		{
			get { return _editEnabled; }
			set { _editEnabled = value; }
		}

		#endregion

        /// <MetaDataID>{48B9A4A8-39A1-4E21-A612-ACEC1E988C2F}</MetaDataID>
		public NodeCheckBox()
			: this(string.Empty)
		{
            DataPropertyName = "Checked";
		}

        /// <MetaDataID>{D4491174-A6A4-4578-9E89-CA0D9829C313}</MetaDataID>
		public NodeCheckBox(string propertyName)
		{
			_check = Resources.check;
			_uncheck = Resources.uncheck;
			_unknown = Resources.unknown;
			DataPropertyName = propertyName;
		}

        /// <MetaDataID>{229EE76F-091B-4D2F-A7B7-77E7DE1222BE}</MetaDataID>
		public override Size MeasureSize(TreeNode node)
		{
			return new Size(Width, Width);
		}

        /// <MetaDataID>{0EACF7F1-BEF0-4305-8B91-2F9D77DDF4DB}</MetaDataID>
		public override void Draw(TreeNode node, DrawContext context)
		{
			Rectangle r = context.Bounds;
			int dy = (int)Math.Round((float)(r.Height - ImageSize) / 2);
			CheckState state = GetCheckState(node);
			if (Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer renderer;
				if (state == CheckState.Indeterminate)
					renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.MixedNormal);
				else if (state == CheckState.Checked)
					renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.CheckedNormal);
				else
					renderer = new VisualStyleRenderer(VisualStyleElement.Button.CheckBox.UncheckedNormal);
				renderer.DrawBackground(context.Graphics, new Rectangle(r.X, r.Y + dy, ImageSize, ImageSize));
			}
			else
			{
				Image img;
				if (state == CheckState.Indeterminate)
					img = _unknown;
				else if (state == CheckState.Checked)
					img = _check;
				else
					img = _uncheck;
				context.Graphics.DrawImage(img, new Point(r.X, r.Y + dy));
				//ControlPaint.DrawCheckBox(context.Graphics, r, state2);
			}
		}

        /// <MetaDataID>{1AD477F8-A731-404F-B2C7-F8D1E6EBEF61}</MetaDataID>
		protected virtual CheckState GetCheckState(TreeNode node)
		{
			object obj = GetValue(node);
			if (obj is CheckState)
				return (CheckState)obj;
			else if (obj is bool)
				return (bool)obj ? CheckState.Checked : CheckState.Unchecked;
			else
				return CheckState.Unchecked;
		}

        /// <MetaDataID>{3C0F95BE-8480-4CE0-88DB-B34030190A81}</MetaDataID>
		protected virtual void SetCheckState(TreeNode node, CheckState value)
		{
			Type type = GetPropertyType(node);
			if (type == typeof(CheckState))
			{
				SetValue(node, value);
				OnCheckStateChanged(node);
			}
			else if (type == typeof(bool))
			{
				SetValue(node, value != CheckState.Unchecked);
				OnCheckStateChanged(node);
			}
		}

        /// <MetaDataID>{57012E01-FC78-4B4B-AD4D-6ABA0D622E83}</MetaDataID>
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			if (args.Button == MouseButtons.Left && EditEnabled)
			{
				CheckState state = GetCheckState(args.Node);
				state = GetNewState(state);
				SetCheckState(args.Node, state);
				args.Handled = true;
                Parent.UpdateView();
			}
		}

        /// <MetaDataID>{9A8E8321-A81F-4A66-B0E7-C9B9FCA84CB4}</MetaDataID>
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			args.Handled = true;
		}

        /// <MetaDataID>{5003375C-3620-4B10-97B3-A8F5569A8B3F}</MetaDataID>
		private CheckState GetNewState(CheckState state)
		{
			if (state == CheckState.Indeterminate)
				return CheckState.Unchecked;
			else if(state == CheckState.Unchecked)
				return CheckState.Checked;
			else 
				return ThreeState ? CheckState.Indeterminate : CheckState.Unchecked;
		}

        /// <MetaDataID>{D40309F1-16A2-4912-B75B-165D90E56CD1}</MetaDataID>
		public override void KeyDown(KeyEventArgs args)
		{
			if (args.KeyCode == Keys.Space && EditEnabled)
			{
				Parent.BeginUpdate();
				try
				{
					if (Parent.CurrentNode != null)
					{
						CheckState value = GetNewState(GetCheckState(Parent.CurrentNode));
						foreach (TreeNode node in Parent.Selection)
							SetCheckState(node, value);
					}
				}
				finally
				{
					Parent.EndUpdate();
				}
				args.Handled = true;
			}
		}

		public event EventHandler<TreePathEventArgs> CheckStateChanged;
        /// <MetaDataID>{8B49C2D8-8DE9-4353-872F-0AB3E5D8E276}</MetaDataID>
		protected void OnCheckStateChanged(TreePathEventArgs args)
		{
			if (CheckStateChanged != null)
				CheckStateChanged(this, args);
		}

        /// <MetaDataID>{3BF6125B-62B8-428B-9987-58F08CDDAF6A}</MetaDataID>
		protected void OnCheckStateChanged(TreeNode node)
		{
			TreePath path = this.Parent.GetPath(node);
			OnCheckStateChanged(new TreePathEventArgs(path));
		}

	}
}
