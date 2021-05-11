using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ConnectableControls.Tree.NodeControls;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{7cd87e47-f842-42c7-910f-0842e3fa9930}</MetaDataID>
	public struct EditorContext
	{
		private TreeNode _currentNode;
		public TreeNode CurrentNode
		{
			get { return _currentNode; }
			set { _currentNode = value; }
		}

		private Control _editor;
		public Control Editor
		{
			get { return _editor; }
			set { _editor = value; }
		}

		private NodeControl _owner;
		public NodeControl Owner
		{
			get { return _owner; }
			set { _owner = value; }
		}

		private Rectangle _bounds;
		public Rectangle Bounds
		{
			get { return _bounds; }
			set { _bounds = value; }
		}
	}
}
