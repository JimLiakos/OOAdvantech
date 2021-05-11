using System;
using System.Windows.Forms;
namespace ConnectableControls.Tree
{
    /// <MetaDataID>{97C8B7B1-2371-49F8-9658-F349449AD8D6}</MetaDataID>
	internal abstract class InputState
	{
        /// <MetaDataID>{e1ed9b28-c7cb-4c55-9a77-286b9d12fc10}</MetaDataID>
		private TreeView _tree;

        /// <MetaDataID>{06265ab9-0262-4944-8833-0b949e2aa2b6}</MetaDataID>
		public TreeView Tree
		{
			get { return _tree; }
		}

        /// <MetaDataID>{6D51D5C6-B7A2-48CE-AEF0-FA93A85469EC}</MetaDataID>
		public InputState(TreeView tree)
		{
			_tree = tree;
		}

        /// <MetaDataID>{19D7F6FA-66F7-47D7-9348-3809F1441CEB}</MetaDataID>
		public abstract void KeyDown(System.Windows.Forms.KeyEventArgs args);
        /// <MetaDataID>{9C451874-ECD2-43BC-A622-68D6ECD00B55}</MetaDataID>
		public abstract void MouseDown(TreeNodeAdvMouseEventArgs args);
        /// <MetaDataID>{6A7A54FE-51B1-4150-BAE4-337D889242C0}</MetaDataID>
		public abstract void MouseUp(TreeNodeAdvMouseEventArgs args);

        /// <summary>
        /// handle OnMouseMove event
        /// </summary>
        /// <param name="args"></param>
        /// <returns>true if event was handled and should be dispatched</returns>
        /// <MetaDataID>{3FB69873-7688-44E9-988F-A3E67211E5A5}</MetaDataID>
		public virtual bool MouseMove(MouseEventArgs args)
		{
			return false;
		}
	}
}
