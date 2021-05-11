using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{65507044-408C-4E33-85D0-7E92AA1FEF88}</MetaDataID>
	public class NodeTextBox: BaseTextControl
	{
		private const int MinTextBoxWidth = 30;
        private System.Windows.Forms.TextBox _textBox;

        /// <MetaDataID>{A67724BC-6126-4DAD-8B17-1F4C26201BB2}</MetaDataID>
		public NodeTextBox()
		{
            DataPropertyName = "Text";
		}

        /// <MetaDataID>{500E6283-0F21-4A84-9619-527DE40FB1F1}</MetaDataID>
		protected override Size CalculateEditorSize(EditorContext context)
		{
			if (Parent.UseColumns)
				return context.Bounds.Size;
			else
			{
                System.Windows.Forms.TextBox textBox = context.Editor as System.Windows.Forms.TextBox;
				Size size = GetLabelSize(textBox.Text);
				int width = Math.Max(size.Width + Font.Height, MinTextBoxWidth); // reserve a place for new typed character
				return new Size(width, size.Height);
			}
		}

        /// <MetaDataID>{4A9B2210-3155-47AF-8855-D782F7487F1A}</MetaDataID>
		public override void KeyDown(KeyEventArgs args)
		{
			if (args.KeyCode == Keys.F2 && Parent.CurrentNode != null)
			{
				args.Handled = true;
				BeginEdit();
			}
		}

        /// <MetaDataID>{B918FE41-F989-416E-BF1B-820158E2468B}</MetaDataID>
		protected override Control CreateEditor(TreeNode node)
		{
            System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
			textBox.TextAlign = TextAlign;
			textBox.Text = GetLabel(node);
			textBox.BorderStyle = BorderStyle.FixedSingle;
			textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
			textBox.Disposed += new EventHandler(textBox_Disposed);
			textBox.TextChanged += new EventHandler(textBox_TextChanged);
			_label = textBox.Text;
			_textBox = textBox;
			return textBox;
		}

		private string _label;
        /// <MetaDataID>{6A2DF3D7-3951-4241-B758-9669A113BAB6}</MetaDataID>
		private void textBox_TextChanged(object sender, EventArgs e)
		{
			_label = _textBox.Text;
			Parent.UpdateEditorBounds();
		}

        /// <MetaDataID>{0257E973-AE7D-43C7-B381-C457A5F8DC7F}</MetaDataID>
		private void textBox_Disposed(object sender, EventArgs e)
		{
			_textBox.KeyDown -= new KeyEventHandler(textBox_KeyDown);
			_textBox.Disposed -= new EventHandler(textBox_Disposed);
			_textBox.TextChanged -= new EventHandler(textBox_TextChanged);
			_textBox = null;
		}

        /// <MetaDataID>{BCCB7764-6848-4CBC-9C91-B1B62783AA17}</MetaDataID>
		protected override void DoApplyChanges(TreeNode node)
		{
			string oldLabel = GetLabel(node);
			if (oldLabel != _label)
			{
				SetLabel(node, _label);
				OnLabelChanged();
			}
		}

        /// <MetaDataID>{CB7474C6-07CC-4B29-8AED-B159727AFB89}</MetaDataID>
		void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				EndEdit(true);
			else if (e.KeyCode == Keys.Enter)
				EndEdit(false);
		}

        /// <MetaDataID>{34DE1116-F783-432F-ACF8-02699318CB8C}</MetaDataID>
		public void Cut()
		{
			if (_textBox != null)
				_textBox.Cut();
		}

        /// <MetaDataID>{6FDD845A-CCCC-4303-A4CE-6E22F9E92EB3}</MetaDataID>
		public void Copy()
		{
			if (_textBox != null)
				_textBox.Copy();
		}

        /// <MetaDataID>{FC2D028F-A3C7-4418-A9FB-DF424925A8E6}</MetaDataID>
		public void Paste()
		{
			if (_textBox != null)
				_textBox.Paste();
		}

        /// <MetaDataID>{CAEB9119-9C7A-47AC-8EF8-FDE2118D8EF9}</MetaDataID>
		public void Delete()
		{
			if (_textBox != null)
			{
				int len = Math.Max(_textBox.SelectionLength, 1);
				if (_textBox.SelectionStart < _textBox.Text.Length)
				{
					int start = _textBox.SelectionStart;
					_textBox.Text = _textBox.Text.Remove(_textBox.SelectionStart, len);
					_textBox.SelectionStart = start;
				}
			}
		}

		public event EventHandler LabelChanged;
        /// <MetaDataID>{978E2A85-06BA-4F0E-BC10-E7277CA0CF7C}</MetaDataID>
		protected void OnLabelChanged()
		{
			if (LabelChanged != null)
				LabelChanged(this, EventArgs.Empty);
		}
	}
}
