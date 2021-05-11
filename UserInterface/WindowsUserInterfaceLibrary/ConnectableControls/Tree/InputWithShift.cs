using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{35763F8B-4FFC-4920-AA47-02D4F5C849FF}</MetaDataID>
	internal class InputWithShift: NormalInputState
	{
        /// <MetaDataID>{68C102D2-D263-4447-A91F-FF26A55AF613}</MetaDataID>
		public InputWithShift(TreeView tree): base(tree)
		{
		}

        /// <MetaDataID>{75CCE925-1EF1-4914-A66A-22F9F2694A77}</MetaDataID>
		protected override void FocusRow(TreeNode node)
		{
			Tree.SuspendSelectionEvent = true;
			try
			{
				if (Tree.SelectionMode == TreeSelectionMode.Single || Tree.SelectionStart == null)
					base.FocusRow(node);
				else if (CanSelect(node))
				{
					SelectAllFromStart(node);
					Tree.CurrentNode = node;
					Tree.ScrollTo(node);
				}
			}
			finally
			{
				Tree.SuspendSelectionEvent = false;
			}
		}

        /// <MetaDataID>{87ED82D6-0BC2-4388-90D9-85432E2F6520}</MetaDataID>
		protected override void DoMouseOperation(TreeNodeAdvMouseEventArgs args)
		{
			if (Tree.SelectionMode == TreeSelectionMode.Single || Tree.SelectionStart == null)
			{
				base.DoMouseOperation(args);
			}
			else if (CanSelect(args.Node))
			{
				Tree.SuspendSelectionEvent = true;
				try
				{
					SelectAllFromStart(args.Node);
				}
				finally
				{
					Tree.SuspendSelectionEvent = false;
				}
			}
		}

        /// <MetaDataID>{2E48B0B0-611C-4024-8C09-004D70B51DAF}</MetaDataID>
		protected override void MouseDownAtEmptySpace(TreeNodeAdvMouseEventArgs args)
		{
		}

        /// <MetaDataID>{96F724D6-4AE2-40BC-811D-326A80285363}</MetaDataID>
		private void SelectAllFromStart(TreeNode node)
		{
			Tree.ClearSelection();
			int a = node.Row;
			int b = Tree.SelectionStart.Row;
			for (int i = Math.Min(a, b); i <= Math.Max(a, b); i++)
			{
				if (Tree.SelectionMode == TreeSelectionMode.Multi || Tree.RowMap[i].Parent == node.Parent)
					Tree.RowMap[i].IsSelected = true;
			}
		}
	}
}
