using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{26358DCD-31EA-492E-83B4-540D1ED958AE}</MetaDataID>
	public class Node
	{
		#region NodeCollection

        /// <MetaDataID>{661D326F-C51E-42E1-BFB4-FBD8CB2ADABF}</MetaDataID>
		private class NodeCollection : Collection<Node>
		{
			private Node _owner;

            /// <MetaDataID>{1B80DAE9-0EF8-47D3-AA1D-9A8D6032BAE7}</MetaDataID>
			public NodeCollection(Node owner)
			{
				_owner = owner;
			}

            /// <MetaDataID>{96E6385E-0E0E-4488-B393-D4FDAA07DB0F}</MetaDataID>
			protected override void ClearItems()
			{
				while (this.Count != 0)
					this.RemoveAt(this.Count - 1);
			}

            /// <MetaDataID>{A06DBAEF-48A0-46F0-A0C3-7000DC2AF42C}</MetaDataID>
			protected override void InsertItem(int index, Node item)
			{
				if (item == null)
					throw new ArgumentNullException("item");

				if (item.Parent != _owner)
				{
					if (item.Parent != null)
						item.Parent.Nodes.Remove(item);
					item._parent = _owner;
					base.InsertItem(index, item);

					TreeModel model = _owner.FindModel();
					if (model != null)
						model.OnNodeInserted(_owner, index, item);
				}
			}

            /// <MetaDataID>{F7CAE0A4-0351-48F2-B569-F2BC638EA6A6}</MetaDataID>
			protected override void RemoveItem(int index)
			{
				Node item = this[index];
				item._parent = null;
				base.RemoveItem(index);

				TreeModel model = _owner.FindModel();
				if (model != null)
					model.OnNodeRemoved(_owner, index, item);
			}

            /// <MetaDataID>{E95C0F4E-F5CB-46A7-B45A-51EAED03FE93}</MetaDataID>
			protected override void SetItem(int index, Node item)
			{
				if (item == null)
					throw new ArgumentNullException("item");

				RemoveAt(index);
				InsertItem(index, item);
			}
		}

		#endregion

		#region Properties

		private TreeModel _model;
		internal TreeModel Model
		{
			get { return _model; }
			set { _model = value; }
		}

        bool SubNodesLoaded = false;
		private NodeCollection _nodes;
		public Collection<Node> Nodes
		{
			get 
            {
                //if (TreeView != null && !SubNodesLoaded)
                //{
                //    SubNodesLoaded = true;
                //    object objectCollection = null;
                //    OOAdvantech.UserInterface.Runtime.DisplayedValue collectionDisplayedValue = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, TreeView.ValueType, TreeView.SubNodesProerty as string, null);

                //    if (collectionDisplayedValue.Members.ContainsKey("Items"))
                //        objectCollection = collectionDisplayedValue.Members["Items"].ValuesCollection;
                //    if (objectCollection == null)
                //        return _nodes;


                //    //object rr = CollectionObjectType;

                //    //_CollectionObjectType = ViewControlObject.GetClassifier(_Path as string).TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                //    System.Collections.IEnumerator enumerator = objectCollection.GetType().GetMethod("GetEnumerator").Invoke(objectCollection, new object[0]) as System.Collections.IEnumerator;
                //    enumerator.Reset();
                //    // object obj = enumerator.Current;
                //    while (enumerator.MoveNext())
                //    {
                //        OOAdvantech.UserInterface.Runtime. DisplayedValue displayedObj = null;
                //        object obj = enumerator.Current;
                //        if (obj is OOAdvantech.UserInterface.Runtime.DisplayedValue)
                //            displayedObj = obj as OOAdvantech.UserInterface.Runtime.DisplayedValue;
                //        else
                //            displayedObj = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(obj, null);
                //        _nodes.Add(new Node(displayedObj, TreeView));

                //    }
                //}



                return _nodes; 
            }
		}

		private Node _parent;
		public Node Parent
		{
			get { return _parent; }
			set 
			{
				if (value != _parent)
				{
					if (_parent != null)
						_parent.Nodes.Remove(this);

					if (value != null)
						value.Nodes.Add(this);
				}
			}
		}

		public int Index
		{
			get
			{
				if (_parent != null)
					return _parent.Nodes.IndexOf(this);
				else
					return -1;
			}
		}

		public Node PreviousNode
		{
			get
			{
				int index = Index;
				if (index > 0)
					return _parent.Nodes[index - 1];
				else
					return null;
			}
		}

		public Node NextNode
		{
			get
			{
				int index = Index;
				if (index >= 0 && index < _parent.Nodes.Count - 1)
					return _parent.Nodes[index + 1];
				else
					return null;
			}
		}

		private string _text;
		public virtual string Text
		{
			get
            {
                //if (TreeView != null)
                //{
                //    return TreeView.UserInterfaceObjectConnection.GetDisplayedValue(DisplayedValue.Value, TreeView.ValueType, TreeView.DisplayMember as string, null).Value as string; 
                //}
                return _text; 
            }
			set 
			{
				if (_text != value)
				{
					_text = value;
					NotifyModel();
				}
			}
		}

		private CheckState _checkState;
		public virtual CheckState CheckState
		{
			get { return _checkState; }
			set 
			{
				if (_checkState != value)
				{
					_checkState = value;
					NotifyModel();
				}
			}
		}

		public bool IsChecked
		{
			get 
			{ 
				return CheckState != CheckState.Unchecked;
			}
			set 
			{
				if (value)
					CheckState = CheckState.Checked;
				else
					CheckState = CheckState.Unchecked;
			}
		}

		public virtual bool IsLeaf
		{
			get
			{
				return false;
			}
		}

		#endregion

        /// <MetaDataID>{B55D22B2-A220-4E3B-AE8F-94D338C1E303}</MetaDataID>
		public Node()
			: this(string.Empty)
		{
		}
        //ConnectableControls.Tree.TreeView TreeView;
        //OOAdvantech.UserInterface.Runtime.DisplayedValue DisplayedValue  ;
        //public Node(OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue,ConnectableControls.Tree.TreeView treeView)
        //    : this("string.Empty")
        //{
        //    TreeView = treeView;
        //    DisplayedValue = displayedValue;
        //}


        /// <MetaDataID>{B57BAFD6-553C-4FCC-9807-40B5CFC288E6}</MetaDataID>
		public Node(string text)
		{
			_text = text;
			_nodes = new NodeCollection(this);
		}

        /// <MetaDataID>{D9FA6F86-7153-4FF9-BE47-78029A12D0A8}</MetaDataID>
		private TreeModel FindModel()
		{
			Node node = this;
			while (node != null)
			{
				if (node.Model != null)
					return node.Model;
				node = node.Parent;
			}
			return null;
		}

        /// <MetaDataID>{DF1681C7-F95D-4BF9-9FBC-0EB148E8B2E6}</MetaDataID>
		protected void NotifyModel()
		{
			TreeModel model = FindModel();
			if (model != null && Parent != null)
			{
				TreePath path = model.GetPath(Parent);
				if (path != null)
				{
					TreeModelEventArgs args = new TreeModelEventArgs(path, new int[] { Index }, new object[] { this });
					model.OnNodesChanged(args);
				}
			}
		}
	}
}
