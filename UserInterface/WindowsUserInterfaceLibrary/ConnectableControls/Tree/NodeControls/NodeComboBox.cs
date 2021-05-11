using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{822D531C-BF8B-468E-B634-E06009F6920F}</MetaDataID>
	public class NodeComboBox : BaseTextControl
	{
		private object _selectedItem;

		#region Properties

		private int _editorWidth = 100;
		[DefaultValue(100)]
		public int EditorWidth
		{
			get { return _editorWidth; }
			set { _editorWidth = value; }
		}

		private object[]_dropDownItems;
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object[] DropDownItems
		{
			get { return _dropDownItems; }
			set { _dropDownItems = value; }
		}

		#endregion

        /// <MetaDataID>{AFAC329F-50B3-4B50-AB60-6312203EE669}</MetaDataID>
		public NodeComboBox()
		{
		}

        /// <MetaDataID>{30298535-A8C3-4A61-ACC6-EB87F237D74E}</MetaDataID>
		protected override Size CalculateEditorSize(EditorContext context)
		{
			if (Parent.UseColumns)
				return context.Bounds.Size;
			else
				return new Size(EditorWidth, context.Bounds.Height);
		}

        /// <MetaDataID>{15BD5F19-D7BF-4850-A5CE-08A0C3CB93F6}</MetaDataID>
		protected override Control CreateEditor(TreeNode node)
		{
			ComboBox comboBox = new ComboBox();
			if (DropDownItems != null)
				comboBox.Items.AddRange(DropDownItems);
			_selectedItem = GetValue(node);
			comboBox.SelectedItem = _selectedItem;
			comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox.KeyDown += new KeyEventHandler(EditorKeyDown);
			comboBox.SelectedIndexChanged += new EventHandler(EditorSelectedIndexChanged);
			comboBox.Disposed += new EventHandler(EditorDisposed);
			return comboBox;
		}

        /// <MetaDataID>{A5871F82-52F2-4E3A-BFAE-568E07025616}</MetaDataID>
		void EditorDisposed(object sender, EventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			comboBox.KeyDown -= new KeyEventHandler(EditorKeyDown);
			comboBox.SelectedIndexChanged -= new EventHandler(EditorSelectedIndexChanged);
			comboBox.Disposed -= new EventHandler(EditorDisposed);
		}

        /// <MetaDataID>{86F9BFD8-C6AE-45F6-A862-AB1957440417}</MetaDataID>
		void EditorSelectedIndexChanged(object sender, EventArgs e)
		{
			_selectedItem = (sender as ComboBox).SelectedItem;
			Parent.HideEditor();
		}

        /// <MetaDataID>{23010BC0-15FD-47D7-B7E2-40D6FCCF59C9}</MetaDataID>
		public override void UpdateEditor(Control control)
		{
			(control as ComboBox).DroppedDown = true;
		}

        /// <MetaDataID>{0C79D1EB-9CA8-4D08-A59F-A9BCA96087A1}</MetaDataID>
		protected override void DoApplyChanges(TreeNode node)
		{
			SetValue(node, _selectedItem);
		}

        /// <MetaDataID>{E9850538-49A2-45B5-A3FC-1D3BE78EB77D}</MetaDataID>
		void EditorKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
				EndEdit(true);
		}
	}
}
