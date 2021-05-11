using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{418CAAA3-6285-4374-AAB5-3D168D94C304}</MetaDataID>
	public class TreeViewAdvCancelEventArgs : TreeViewAdvEventArgs
	{
		private bool _cancel;

		public bool Cancel
		{
			get { return _cancel; }
			set { _cancel = value; }
		}

        /// <MetaDataID>{2A5C0F6B-00E3-4BDF-ACFC-4397843FEE9F}</MetaDataID>
		public TreeViewAdvCancelEventArgs(TreeNode node)
			: base(node)
		{
		}

	}
}
