using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{b6491249-0a7c-4d48-bb10-07744e3eba00}</MetaDataID>
	public struct DropPosition
	{
        /// <MetaDataID>{fd760a2d-69f9-41aa-b409-83dc64b7fb04}</MetaDataID>
		private TreeNode _node;
        /// <MetaDataID>{53c8b761-b98b-4580-9515-bba2f4a2df7d}</MetaDataID>
		public TreeNode Node
		{
			get { return _node; }
			set { _node = value; }
		}

        /// <MetaDataID>{eb7004ca-5f38-4e5a-88ea-c84dca426d4b}</MetaDataID>
		private NodePosition _position;
        /// <MetaDataID>{18c79ed3-5952-4afa-b9bd-eba7016a1a40}</MetaDataID>
		public NodePosition Position
		{
			get { return _position; }
			set { _position = value; }
		}
	}
}
