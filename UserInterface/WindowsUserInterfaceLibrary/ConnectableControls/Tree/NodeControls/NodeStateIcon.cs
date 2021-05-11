using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ConnectableControls;
using ConnectableControls.Properties;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{36392324-16FC-4430-967B-4E4B0386150D}</MetaDataID>
	public class NodeStateIcon: NodeIcon
	{
		private Image _leaf;
		private Image _opened;
		private Image _closed;

        /// <MetaDataID>{C932D105-41F9-4B63-BF9D-4BFC01D2A40B}</MetaDataID>
		public NodeStateIcon()
		{
			_leaf = MakeTransparent(Resources.Leaf);
			_opened = MakeTransparent(Resources.Folder);
			_closed = MakeTransparent(Resources.FolderClosed);
		}

        /// <MetaDataID>{6F435BD9-3EF4-4199-B65C-A3B5859F8EFD}</MetaDataID>
		private static Image MakeTransparent(Bitmap bitmap)
		{
			bitmap.MakeTransparent(bitmap.GetPixel(0,0));
			return bitmap;
		}

        /// <MetaDataID>{B999A2CB-B89A-426A-ADC3-5B07D64E60BD}</MetaDataID>
		protected override Image GetIcon(TreeNode node)
		{
			Image icon = base.GetIcon(node);
			if (icon != null)
				return icon;
			else if (node.IsLeaf)
				return _leaf;
			else if (node.CanExpand && node.IsExpanded)
				return _opened;
			else
				return _closed;
		}
	}
}
