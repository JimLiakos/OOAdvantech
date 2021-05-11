using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{06F7B855-3C8B-4BAD-933B-D7414B8F1108}</MetaDataID>
	internal class NodeControlsCollection : Collection<NodeControl>
	{
        /// <MetaDataID>{00a0fb60-8b27-4af6-97b1-27a7357613dc}</MetaDataID>
		private TreeView _tree;

        /// <MetaDataID>{371459D0-1B05-4560-BBC1-771A10105919}</MetaDataID>
		public NodeControlsCollection(TreeView tree)
		{
			_tree = tree;
		}

        /// <MetaDataID>{D9FFB6F5-2E5D-4F3B-B660-6E539A03FC24}</MetaDataID>
		protected override void ClearItems()
		{
			_tree.BeginUpdate();
			try
			{
				while (this.Count != 0)
					this.RemoveAt(this.Count - 1);
			}
			finally
			{
				_tree.EndUpdate();
			}
		}

        /// <MetaDataID>{BFFAB482-BCFD-411C-BC24-C0177E111AEA}</MetaDataID>
		protected override void InsertItem(int index, NodeControl item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item.Parent != _tree)
			{
				if (item.Parent != null)
				{
					item.Parent.NodeControls.Remove(item);
				}
				base.InsertItem(index, item);
				item.AssignParent(_tree);
				_tree.FullUpdate();
			}
		}

        /// <MetaDataID>{0F5629D0-AE07-494C-8F22-62034D18EE12}</MetaDataID>
		protected override void RemoveItem(int index)
		{
			NodeControl value = this[index];
			value.AssignParent(null);
			base.RemoveItem(index);
			_tree.FullUpdate();
		}

        /// <MetaDataID>{58908B2C-8F0A-4F54-AD37-FA52F6166C02}</MetaDataID>
		protected override void SetItem(int index, NodeControl item)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			_tree.BeginUpdate();
			try
			{
				RemoveAt(index);
				InsertItem(index, item);
			}
			finally
			{
				_tree.EndUpdate();
			}
		}
	}

    /// <MetaDataID>{32486D3B-7BEF-4DA2-9CF0-262DB00E719E}</MetaDataID>
	internal class NodeControlCollectionEditor : CollectionEditor
	{
        /// <MetaDataID>{20ADB853-0473-46F2-868E-06F0991B96EC}</MetaDataID>
		private Type[] _types;

        /// <MetaDataID>{2C4FCD55-3823-499F-8A4E-64D86E86F984}</MetaDataID>
		public NodeControlCollectionEditor(Type type)
			: base(type)
		{
			_types = new Type[] { typeof(NodeTextBox), typeof(NodeComboBox), typeof(NodeCheckBox),
				typeof(NodeStateIcon), typeof(NodeIcon)  };
		}

        /// <MetaDataID>{9AB3F84B-74D1-43B0-B163-3339954E3050}</MetaDataID>
		protected override System.Type[] CreateNewItemTypes()
		{
			return _types;
		}
	}
}
