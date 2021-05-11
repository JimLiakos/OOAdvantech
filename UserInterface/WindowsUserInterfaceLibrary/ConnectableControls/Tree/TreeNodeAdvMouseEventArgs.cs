using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using ConnectableControls.Tree.NodeControls;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{635D5B07-3395-4F3A-B375-A2FB741277A3}</MetaDataID>
	public class TreeNodeAdvMouseEventArgs : MouseEventArgs
	{
		private TreeNode _node;
		public TreeNode Node
		{
			get { return _node; }
			internal set { _node = value; }
		}

		private NodeControl _control;
		public NodeControl Control
		{
			get { return _control; }
			internal set { _control = value; }
		}

		private Point _viewLocation;
		public Point ViewLocation
		{
			get { return _viewLocation; }
			internal set { _viewLocation = value; }
		}

		private Point _absoluteLocation;
		public Point AbsoluteLocation
		{
			get { return _absoluteLocation; }
			internal set { _absoluteLocation = value; }
		}

		private Keys _modifierKeys;
		public Keys ModifierKeys
		{
			get { return _modifierKeys; }
			internal set { _modifierKeys = value; }
		}

		private bool _handled;
		public bool Handled
		{
			get { return _handled; }
			internal set { _handled = value; }
		}

		private Rectangle _controlBounds;
		public Rectangle ControlBounds
		{
			get { return _controlBounds; }
			internal set { _controlBounds = value; }
		}

        /// <MetaDataID>{A1D018F4-F1BC-4367-8E88-288DBB7ECA8D}</MetaDataID>
		public TreeNodeAdvMouseEventArgs(MouseEventArgs args)
			: base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
		{
		}
	}
}
