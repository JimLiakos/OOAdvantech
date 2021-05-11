using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{B5B05FBB-5A16-4C26-ABF9-F7E6D0E22C6F}</MetaDataID>
	public class TreeNode
	{
        /// <MetaDataID>{60e63711-c987-4f75-8f8c-1b1968564977}</MetaDataID>
		private Collection<TreeNode> _nodes;
        /// <MetaDataID>{0c0fcd52-4fe6-4c48-8292-a03e8c200615}</MetaDataID>
		private ReadOnlyCollection<TreeNode> _children;

		#region Properties

        /// <MetaDataID>{fc2d2e8a-7480-4e3c-9aee-6595be733eff}</MetaDataID>
		private TreeView _tree;
        /// <MetaDataID>{3c684958-e663-4332-99ef-fa04a94a960d}</MetaDataID>
		internal TreeView Tree
		{
			get { return _tree; }
		}

        /// <MetaDataID>{c9b0560a-5b59-444b-85d9-d7d46ed77c5f}</MetaDataID>
		private int _row;
        /// <MetaDataID>{ab968bca-b99f-485c-a492-35047b55e609}</MetaDataID>
		internal int Row
		{
			get { return _row; }
			set { _row = value; }
		}

        /// <MetaDataID>{3b73f14e-70f6-43a3-aae8-e14b57a1a86e}</MetaDataID>
		private bool _isSelected;
        /// <MetaDataID>{30109f35-d7a8-4c61-ac2a-bad19dd64b0b}</MetaDataID>
		public bool IsSelected
		{
			get { return _isSelected; }
			set 
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					if (Tree.IsMyNode(this))
					{
						if (_isSelected)
						{
							if (!_tree.Selection.Contains(this))
								_tree.Selection.Add(this);

							if (_tree.Selection.Count == 1)
								_tree.CurrentNode = this;
						}
						else
							_tree.Selection.Remove(this);
						_tree.UpdateView();
						_tree.OnSelectionChanged();
					}
				}
			}
		}

        /// <MetaDataID>{1c0a1ce3-b95f-479d-908b-9914f9b6eb71}</MetaDataID>
		private bool _isLeaf;
        /// <MetaDataID>{f73a20c9-64c4-4ee9-b8f7-d539f12907a4}</MetaDataID>
		public bool IsLeaf
		{
			get { return _isLeaf; }
			internal set { _isLeaf = value; }
		}

        /// <MetaDataID>{be82f86f-eadf-4227-a656-e401d0d34450}</MetaDataID>
		private bool _isExpandedOnce;
        /// <MetaDataID>{dc1c81d8-66ef-4bb5-a610-ef54a6106e0c}</MetaDataID>
		public bool IsExpandedOnce
		{
			get { return _isExpandedOnce; }
			internal set { _isExpandedOnce = value; }
		}

        /// <MetaDataID>{7f7f23c2-5912-4b1c-96d1-df4f3c859752}</MetaDataID>
		private bool _isExpanded;
        /// <MetaDataID>{8dd5ff5d-e7ec-455c-80c8-b661aeb52dc1}</MetaDataID>
		public bool IsExpanded
		{
			get { return _isExpanded; }
			set 
			{ 
				if (Tree.IsMyNode(this) && _isExpanded != value)
				{
					if (value)
						Tree.OnExpanding(this);
					else
						Tree.OnCollapsing(this);

					if (value && !_isExpandedOnce)
					{
						Cursor oldCursor = Tree.Cursor;
						Tree.Cursor = Cursors.WaitCursor;
						try
						{
							Tree.ReadChilds(this);
						}
						finally
						{
							Tree.Cursor = oldCursor;
						}
					}
					_isExpanded = value; //&& CanExpand;
					if (_isExpanded == value)
						Tree.SmartFullUpdate();
					else
						Tree.UpdateView();

					if (value)
						Tree.OnExpanded(this);
					else
						Tree.OnCollapsed(this);
				}
			}
		}

        /// <MetaDataID>{3e2c8b85-0469-4dd7-8893-2bd9cfa73a97}</MetaDataID>
		private TreeNode _parent;
        /// <MetaDataID>{ba6d9470-21b9-4aea-b799-d60a03e65234}</MetaDataID>
		public TreeNode Parent
		{
			get { return _parent; }
			internal set 
            { 
                _parent = value;

                if (value == null)
                {
                    foreach (TreeNode n in Nodes)
                    {
                        n.Parent = null;

                    }
                }
            }
		}

        /// <MetaDataID>{c91d9c03-7340-4159-9e84-9cf9aada77cf}</MetaDataID>
		public int Level
		{
			get
			{
				if (_parent == null)
					return 0;
				else
					return _parent.Level + 1;
			}
		}

        /// <MetaDataID>{61ec748d-3db8-4873-91f9-d45f1567328a}</MetaDataID>
		public TreeNode NextNode
		{
			get
			{
				if (_parent != null)
				{
					int index = _parent.Nodes.IndexOf(this);
					if (index < _parent.Nodes.Count - 1)
						return _parent.Nodes[index + 1];
				}
				return null;
			}
		}

        /// <MetaDataID>{f12cc26a-849b-4f4a-a353-794423e02674}</MetaDataID>
		internal TreeNode BottomNode
		{
			get
			{
				TreeNode parent = this.Parent;
				if (parent != null)
				{
					if (parent.NextNode != null)
						return parent.NextNode;
					else
						return parent.BottomNode;
				}
				return null;
			}
		}

        /// <MetaDataID>{328e8f04-fe42-45b6-a1d7-1938b67a8788}</MetaDataID>
		public bool CanExpand
		{
			get
			{
				return (_nodes.Count > 0 || (!IsExpandedOnce && !IsLeaf));
			}
		}

        /// <MetaDataID>{7702d3db-3fb6-492b-9c1e-b79e22dd78f1}</MetaDataID>
		private object _tag;
        /// <MetaDataID>{baa515f7-a844-43ed-a634-8469309b99f5}</MetaDataID>
		public object Tag
		{
			get { return _tag; }
		}

        /// <MetaDataID>{10e60b34-f8fa-4f50-b1f3-d14074eab868}</MetaDataID>
		internal Collection<TreeNode> Nodes
		{
			get { return _nodes; }
		}

        /// <MetaDataID>{74532d9c-4630-4f1d-9efb-c699387025a2}</MetaDataID>
		public ReadOnlyCollection<TreeNode> Children
		{
			get
			{
				return _children;
			}
		}

		#endregion

        /// <MetaDataID>{D99E4DF7-E627-4796-9CB4-498156994EDE}</MetaDataID>
		internal TreeNode(TreeView tree, object tag)
		{
			_row = -1;
			_tree = tree;
			_nodes = new Collection<TreeNode>();
			_children = new ReadOnlyCollection<TreeNode>(_nodes);
			_tag = tag;

            if (tag is NodeDisplayedObject)
                (tag as NodeDisplayedObject).TreeNode = this;

		}

        /// <MetaDataID>{5B1D12F4-6780-4DCE-B5BD-1D793E436878}</MetaDataID>
		public override string ToString()
		{
			if (Tag != null)
				return Tag.ToString();
			else
				return base.ToString();
		}
	}
}
