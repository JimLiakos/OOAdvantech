using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{61528B0D-78F4-4512-8496-011C73DCB4DB}</MetaDataID>
	internal class NormalInputState : InputState
	{
		private bool _mouseDownFlag = false;

        /// <MetaDataID>{45115F14-BB3A-4CEC-A24E-50CBC06B7CB0}</MetaDataID>
		public NormalInputState(TreeView tree) : base(tree)
		{
		}

        /// <MetaDataID>{857953CE-91FC-498B-BD7B-67FD1B1EEF09}</MetaDataID>
		public override void KeyDown(KeyEventArgs args)
		{
			if (Tree.CurrentNode == null && Tree.Root.Nodes.Count > 0)
				Tree.CurrentNode = Tree.Root.Nodes[0];

			if (Tree.CurrentNode != null)
			{
				switch (args.KeyCode)
				{
					case Keys.Right:
						if (!Tree.CurrentNode.IsExpanded)
							Tree.CurrentNode.IsExpanded = true;
						else if (Tree.CurrentNode.Nodes.Count > 0)
							Tree.SelectedNode = Tree.CurrentNode.Nodes[0];
						args.Handled = true;
						break;
					case Keys.Left:
						if (Tree.CurrentNode.IsExpanded)
							Tree.CurrentNode.IsExpanded = false;
						else if (Tree.CurrentNode.Parent != Tree.Root)
							Tree.SelectedNode = Tree.CurrentNode.Parent;
						args.Handled = true;
						break;
					
					case Keys.Down:
						NavigateForward(1);
						args.Handled = true;
						break;
					case Keys.Up:
						NavigateBackward(1);
						args.Handled = true;
						break;
					case Keys.PageDown:
						NavigateForward(Tree.PageRowCount);
						args.Handled = true;
						break;
					case Keys.PageUp:
						NavigateBackward(Tree.PageRowCount);
						args.Handled = true;
						break;

					case Keys.Home:
						if (Tree.RowMap.Count > 0)
							FocusRow(Tree.RowMap[0]);
						args.Handled = true;
						break;
					case Keys.End:
						if (Tree.RowMap.Count > 0)
							FocusRow(Tree.RowMap[Tree.RowMap.Count-1]);
						args.Handled = true;
						break;
				}
			}
		}

        /// <MetaDataID>{85CA460D-4A34-4162-9331-3743D26AD3B4}</MetaDataID>
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			if (args.Node != null)
			{
				Tree.ItemDragMode = true;
				Tree.ItemDragStart = args.ViewLocation;

				if (args.Button == MouseButtons.Left || args.Button == MouseButtons.Right)
				{
					Tree.BeginUpdate();
					try
					{
						Tree.CurrentNode = args.Node;
						if (args.Node.IsSelected)
							_mouseDownFlag = true;
						else
						{
							_mouseDownFlag = false;
							DoMouseOperation(args);
						}
					}
					finally
					{
						Tree.EndUpdate();
					}
				}

			}
			else
			{
				Tree.ItemDragMode = false;
				MouseDownAtEmptySpace(args);
			}
		}

        /// <MetaDataID>{ACFDCB82-42BE-4218-82AF-87AA4585DBB4}</MetaDataID>
		public override void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
			Tree.ItemDragMode = false;
			if (_mouseDownFlag)
			{
				if (args.Button == MouseButtons.Left)
					DoMouseOperation(args);
				else if (args.Button == MouseButtons.Right)
					Tree.CurrentNode = args.Node;
			}
			_mouseDownFlag = false;
		}


        /// <MetaDataID>{C01C681C-6D50-4517-AB07-270B04E7E9D3}</MetaDataID>
		private void NavigateBackward(int n)
		{
			int row = Math.Max(Tree.CurrentNode.Row - n, 0);
			if (row != Tree.CurrentNode.Row)
				FocusRow(Tree.RowMap[row]);
		}

        /// <MetaDataID>{75EAAC44-A90E-4B6B-B766-79C82BA0844F}</MetaDataID>
		private void NavigateForward(int n)
		{
			int row = Math.Min(Tree.CurrentNode.Row + n, Tree.RowCount - 1);
			if (row != Tree.CurrentNode.Row)
				FocusRow(Tree.RowMap[row]);
		}

        /// <MetaDataID>{EE003157-D84C-4CF7-B49A-B7B8E9C08EBF}</MetaDataID>
		protected virtual void MouseDownAtEmptySpace(TreeNodeAdvMouseEventArgs args)
		{
			Tree.ClearSelection();
		}

        /// <MetaDataID>{0A1A9C2C-B5E2-44AA-88CB-1D43B2DDC6E1}</MetaDataID>
		protected virtual void FocusRow(TreeNode node)
		{
			Tree.SuspendSelectionEvent = true;
			try
			{
				Tree.ClearSelection();
				Tree.CurrentNode = node;
				Tree.SelectionStart = node;
				node.IsSelected = true;
				Tree.ScrollTo(node);
			}
			finally
			{
				Tree.SuspendSelectionEvent = false;
			}
		}

        /// <MetaDataID>{A368143F-CCF5-4BA0-A614-5F7BE10DABA1}</MetaDataID>
		protected bool CanSelect(TreeNode node)
		{
			if (Tree.SelectionMode == TreeSelectionMode.MultiSameParent)
			{
				return (Tree.SelectionStart == null || node.Parent == Tree.SelectionStart.Parent);
			}
			else
				return true;
		}

        /// <MetaDataID>{29F15FBE-CE14-4EDD-BCB8-6A37C458A3AE}</MetaDataID>
		protected virtual void DoMouseOperation(TreeNodeAdvMouseEventArgs args)
		{
			Tree.SuspendSelectionEvent = true;
			try
			{
				Tree.ClearSelection();
				if (args.Node != null)
					args.Node.IsSelected = true;
				Tree.SelectionStart = args.Node;
			}
			finally
			{
				Tree.SuspendSelectionEvent = false;
			}
		}
	}
}
