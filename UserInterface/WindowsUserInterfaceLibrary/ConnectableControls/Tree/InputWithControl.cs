using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{79C7226E-FDCB-4A02-B6A6-80D9924D61E5}</MetaDataID>
	internal class InputWithControl: NormalInputState
	{
        /// <MetaDataID>{5FD4AB02-1555-4EED-A2B4-80E3E876827A}</MetaDataID>
		public InputWithControl(TreeView tree): base(tree)
		{
		}

        /// <MetaDataID>{1D7DA915-60BA-49F5-9B4F-6E012FF6404D}</MetaDataID>
		protected override void DoMouseOperation(TreeNodeAdvMouseEventArgs args)
		{
			if (Tree.SelectionMode == TreeSelectionMode.Single)
			{
				base.DoMouseOperation(args);
			}
			else if (CanSelect(args.Node))
			{
				args.Node.IsSelected = !args.Node.IsSelected;
				Tree.SelectionStart = args.Node;
			}
		}

        /// <MetaDataID>{754E1DDD-5D26-4BF5-A95F-2A209FC3DDAA}</MetaDataID>
		protected override void MouseDownAtEmptySpace(TreeNodeAdvMouseEventArgs args)
		{
		}
	}
}
