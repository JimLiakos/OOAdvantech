using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{1ED5374E-707D-4DEF-880F-F75808D0362B}</MetaDataID>
	public class TreeViewAdvEventArgs: EventArgs
	{
		private TreeNode _node;

		public TreeNode Node
		{
			get { return _node; }
		}

        /// <MetaDataID>{46949103-7268-4A80-8BE7-204997ED525B}</MetaDataID>
		public TreeViewAdvEventArgs(TreeNode node)
		{
			_node = node;
		}
	}
}
