using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{446D1012-1BA9-4C84-AE1C-59B6D244EF63}</MetaDataID>
	public abstract class BaseTextControl : EditableControl
	{
		private static Graphics _measureGraphics = Graphics.FromImage(new Bitmap(1, 1));
		private StringFormat _format;
		private Pen _focusPen;

		#region Properties

		protected Font _font = null;
		public Font Font
		{
			get
			{
                if (_font == null && this.Parent == null)
                    return Control.DefaultFont;
                else if (_font == null && this.Parent != null && this.Parent.Font != null)
                    return this.Parent.Font;
                else
                    return _font;
			}
			set
			{
				if (value == Control.DefaultFont)
					_font = null;
				else
					_font = value;
			}
		}

        /// <MetaDataID>{977AD62F-CC41-4B63-BB5C-448715ECCFDC}</MetaDataID>
		protected bool ShouldSerializeFont()
		{
			return (_font != null);
		}

		private HorizontalAlignment _textAlign = HorizontalAlignment.Left;
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign
		{
			get { return _textAlign; }
			set { _textAlign = value; }
		}

		private StringTrimming _trimming = StringTrimming.None;
		[DefaultValue(StringTrimming.None)]
		public StringTrimming Trimming
		{
			get { return _trimming; }
			set { _trimming = value; }
		}

		#endregion

        /// <MetaDataID>{E1040F99-E52F-4555-87E8-137B0CC3639C}</MetaDataID>
		protected BaseTextControl()
		{
			_focusPen = new Pen(Color.Black);
			_focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			_format = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox);
			_format.LineAlignment = StringAlignment.Center;
		}

        /// <MetaDataID>{F6733478-7646-4A5E-BD1E-EF0A631423A3}</MetaDataID>
		public override Size MeasureSize(TreeNode node)
		{
			return GetLabelSize(node);
		}

        /// <MetaDataID>{8C8C008A-B1D2-476A-9BEC-6111E2D707B2}</MetaDataID>
		protected Size GetLabelSize(TreeNode node)
		{

            if (node.Tag is NodeDisplayedObject)
                return (node.Tag as NodeDisplayedObject).GetSize(this);

			return GetLabelSize(GetLabel(node));
		}

        /// <MetaDataID>{69B201F5-BBCD-4372-9CE1-CA06AAA39BD8}</MetaDataID>
		protected Size GetLabelSize(string label)
		{
			SizeF s = _measureGraphics.MeasureString(label, Font);
			if (!s.IsEmpty)
                return new Size((int)s.Width , (int)s.Height);
			else
				return new Size(10, Font.Height);
		}

        /// <MetaDataID>{D6586852-BB46-49A5-A1E8-22A7B9CF9774}</MetaDataID>
		public override void Draw(TreeNode node, DrawContext context)
		{
			if (context.CurrentEditorOwner == this && node == Parent.CurrentNode)
				return;

			Rectangle clipRect = context.Bounds;
			Brush text = SystemBrushes.ControlText;

			string label = GetLabel(node);
			Size s = GetLabelSize(label);
			Rectangle focusRect = new Rectangle(clipRect.X, clipRect.Y, s.Width, clipRect.Height);

			if (context.DrawSelection == DrawSelectionMode.Active)
			{
				text = SystemBrushes.HighlightText;
				context.Graphics.FillRectangle(SystemBrushes.Highlight, focusRect);
			}
			else if (context.DrawSelection == DrawSelectionMode.Inactive)
			{
				text = SystemBrushes.ControlText;
				context.Graphics.FillRectangle(SystemBrushes.InactiveBorder, focusRect);
			}
			else if (context.DrawSelection == DrawSelectionMode.FullRowSelect)
			{
				text = SystemBrushes.HighlightText;
			}

			if (!context.Enabled)
				text = SystemBrushes.GrayText;

			if (context.DrawFocus)
			{
				focusRect.Width--;
				focusRect.Height--;
				context.Graphics.DrawRectangle(Pens.Gray, focusRect);
				context.Graphics.DrawRectangle(_focusPen, focusRect);
			}
			_format.Alignment = TextHelper.TranslateAligment(TextAlign);
			_format.Trimming = Trimming;
			context.Graphics.DrawString(label, Font, text, clipRect, _format);
		}

        /// <MetaDataID>{0EC5065F-E9FA-4D54-AC59-0FEF66BCAB43}</MetaDataID>
		protected virtual string GetLabel(TreeNode node)
		{
			if (node.Tag != null)
			{
				if (string.IsNullOrEmpty(DataPropertyName))
					return node.Tag.ToString();
				else
				{
					object obj = GetValue(node);
					if (obj != null)
						return obj.ToString();
				}
			}
			return string.Empty;
		}

        /// <MetaDataID>{69667350-4E18-4D6F-A98B-43D6A8C548D0}</MetaDataID>
		protected virtual void SetLabel(TreeNode node, string value)
		{
			SetValue(node, value);
		}

        /// <MetaDataID>{899D290E-B52F-4D51-9926-9EF5716B0D83}</MetaDataID>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_focusPen.Dispose();
				_format.Dispose();
			}
		}

	}
}
