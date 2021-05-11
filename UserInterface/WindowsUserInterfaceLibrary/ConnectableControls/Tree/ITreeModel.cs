using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{A6098A04-0C90-4285-A976-ED71606CB273}</MetaDataID>
	public interface ITreeModel
	{
        /// <MetaDataID>{5CAB4FAE-A69D-4308-895B-C84A0A364A4C}</MetaDataID>
		IEnumerable GetChildren(TreePath treePath);
        /// <MetaDataID>{0FDD492F-33F8-45A9-8E60-F0829C912666}</MetaDataID>
		bool IsLeaf(TreePath treePath);

		event EventHandler<TreeModelEventArgs> NodesChanged; 
		event EventHandler<TreeModelEventArgs> NodesInserted;
		event EventHandler<TreeModelEventArgs> NodesRemoved; 
		event EventHandler<TreePathEventArgs> StructureChanged;
	}
}
