using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{6648F605-0611-49EE-BA59-D52A678A71FB}</MetaDataID>
	internal class TreeColumnCollection : Collection<TreeColumn>
	{
        /// <MetaDataID>{895b5434-b52c-44ef-8a7b-8052ff575c40}</MetaDataID>
		private TreeView _treeView;

        /// <MetaDataID>{11F343AA-1BC8-40A6-AEFB-F4E776E87818}</MetaDataID>
		public TreeColumnCollection(TreeView treeView)
		{
			_treeView = treeView;
		}

        /// <MetaDataID>{7662F45C-1A7B-4360-8162-1A1AFC9A735E}</MetaDataID>
		protected override void InsertItem(int index, TreeColumn item)
		{
			base.InsertItem(index, item);
			item.Index = index;
			item.TreeView = _treeView;
			_treeView.UpdateColumns();
		}

        /// <MetaDataID>{A4314FAA-F196-49F3-8453-977B919BA74A}</MetaDataID>
		protected override void RemoveItem(int index)
		{
			this[index].TreeView = null;
			base.RemoveItem(index);
			_treeView.UpdateColumns();
		}

        /// <MetaDataID>{7B63445E-9DAE-42D7-82C8-BCD3903066BA}</MetaDataID>
		protected override void SetItem(int index, TreeColumn item)
		{
			this[index].TreeView = null;
			base.SetItem(index, item);
			item.Index = index;
			this[index].TreeView = _treeView;
			_treeView.UpdateColumns();
		}

        /// <MetaDataID>{F05F30AE-480E-445D-BEDB-A67E84B7ED22}</MetaDataID>
		protected override void ClearItems()
		{
			foreach (TreeColumn c in Items)
				c.TreeView = null;
			Items.Clear();
			_treeView.UpdateColumns();
		}
	}
}
