using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{6CE8371A-1A11-480D-8AD7-A5AC3ADB8FB4}</MetaDataID>
	public class TreeModel : ITreeModel
	{
        #region public Node Root
        /// <MetaDataID>{9E055A5E-F60F-46A1-941F-314339F9EB95}</MetaDataID>
        private Node _root;
        /// <MetaDataID>{FB339D87-2825-4C77-B0A8-18D460F1CD48}</MetaDataID>
        public Node Root
        {
            get { return _root; }
        } 
        #endregion

        #region public Collection<Node> Nodes
        /// <MetaDataID>{F59DD883-9478-47EE-8BB0-352829275630}</MetaDataID>
        public Collection<Node> Nodes
        {
            get { return _root.Nodes; }
        } 
        #endregion

        TreeView TreeView;
        #region Constructor
        /// <MetaDataID>{C7D12856-99DB-47D2-B2E4-23D872685E5F}</MetaDataID>
        public TreeModel(TreeView treeView)
        {
            TreeView = treeView;
            _root = new Node();
            _root.Model = this;
            treeView.Expanded += new EventHandler<TreeViewAdvEventArgs>(Expanded);
        }

        void Expanded(object sender, TreeViewAdvEventArgs e)
        {
            
            
        } 
        #endregion

        #region public TreePath GetPath(Node node)
        /// <MetaDataID>{F28A04A8-5344-410E-B964-AC3A520D4E86}</MetaDataID>
        public TreePath GetPath(Node node)
        {
            if (node == _root)
                return TreePath.Empty;
            else
            {
                Stack<object> stack = new Stack<object>();
                while (node != _root)
                {
                    stack.Push(node);
                    node = node.Parent;
                }
                return new TreePath(stack.ToArray());
            }
        } 
        #endregion

        #region public Node FindNode(TreePath path)
        /// <MetaDataID>{0C9C7D4D-E94C-4C5A-A69E-4B0FD2D42D00}</MetaDataID>
        public Node FindNode(TreePath path)
        {
            if (path.IsEmpty())
                return _root;
            else
                return FindNode(_root, path, 0);
        } 
        #endregion

        #region private Node FindNode(Node root, TreePath path, int level)
        /// <MetaDataID>{6A1271D4-1963-4849-A71C-10218AACBE7D}</MetaDataID>
        private Node FindNode(Node root, TreePath path, int level)
        {
            foreach (Node node in root.Nodes)
                if (node == path.FullPath[level])
                {
                    if (level == path.FullPath.Length - 1)
                        return node;
                    else
                        return FindNode(node, path, level + 1);
                }
            return null;
        } 
        #endregion

		#region ITreeModel Members

        /// <MetaDataID>{6201C8CF-B463-4842-AB8E-08139568C98E}</MetaDataID>
		public System.Collections.IEnumerable GetChildren(TreePath treePath)
		{
            
            if (treePath.IsEmpty())
            {

              
                bool returnValueAsCollection=false;
                object nodeValue = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(TreeView.Path as string, TreeView, out returnValueAsCollection);
                if (nodeValue == null)
                    yield break;
                //    (string path, IPathDataDisplayer pathDataDisplayer,out bool returnValueAsCollection)

                //OOAdvantech.UserInterface.Runtime.DisplayedValue displayedValue = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(TreeView.Path as string, null);

                if (!returnValueAsCollection)
                {
                    object displayedPresentationObj = null;
                    if (TreeView.ValueType != TreeView.PresentationObjectType && TreeView.PresentationObjectType != null)
                        displayedPresentationObj = TreeView.UserInterfaceObjectConnection.GetPresentationObject(nodeValue, TreeView.PresentationObjectType as OOAdvantech.MetaDataRepository.Class, TreeView.ValueType.GetExtensionMetaObject(typeof(Type)) as Type);


                    object presentationObj = null;
                    if (displayedPresentationObj != null)
                        presentationObj = displayedPresentationObj;

                    NodeDisplayedObject nodeDisplayedObject = new NodeDisplayedObject(TreeView, nodeValue, presentationObj);

                    yield return nodeDisplayedObject;
                }
                else
                {
                    //TODO προβλημα με το update δεν περνάει IPathDataDisplayer value

                    object objectCollection = nodeValue;


                    //if (collectionDisplayedValue.Members.ContainsKey("Items"))
                    //    objectCollection = collectionDisplayedValue.Members["Items"].ValuesCollection;
                    if (objectCollection == null)
                        yield break;


                    //object rr = CollectionObjectType;

                    //_CollectionObjectType = ViewControlObject.GetClassifier(_Path as string).TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                    System.Collections.IEnumerator enumerator = objectCollection.GetType().GetMethod("GetEnumerator", new System.Type[0]).Invoke(objectCollection, new object[0]) as System.Collections.IEnumerator;
                    enumerator.Reset();
                    // object obj = enumerator.Current;
                    while (enumerator.MoveNext())
                    {
                        object displayedObj = null;
                        object obj = enumerator.Current;
                        //if (obj is OOAdvantech.UserInterface.Runtime.DisplayedValue)
                        //    displayedObj = obj as OOAdvantech.UserInterface.Runtime.DisplayedValue;
                        //else
                        //    displayedObj = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(obj, null);

                        object displayedPresentationObj = null;
                        if (TreeView.ValueType != TreeView.PresentationObjectType && TreeView.PresentationObjectType != null)
                            displayedPresentationObj = TreeView.UserInterfaceObjectConnection.GetPresentationObject(obj, TreeView.PresentationObjectType as OOAdvantech.MetaDataRepository.Class, TreeView.ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                        yield return new NodeDisplayedObject(TreeView, obj, displayedPresentationObj);
                    }
                }


                //foreach (string str in Environment.GetLogicalDrives())
                //{

                //    RootItem item = new RootItem(str);
                //    yield return item;
                //}
            }
            else
            {
                NodeDisplayedObject parent = treePath.LastNode as NodeDisplayedObject;

                if (!TreeView.UserInterfaceObjectConnection.Isloaded(parent.Value, TreeView.ValueType, TreeView.SubNodesProperty as string))
                {
                    OOAdvantech.Collections.Generic.List<string> paths=new OOAdvantech.Collections.Generic.List<string>();
                    foreach (string path in TreeView.Paths)
                        paths.Add(path.Replace((TreeView.Path as string) , "Root"));
                    TreeView.UserInterfaceObjectConnection.BatchLoadPathsValues(parent.Value, TreeView.ValueType.GetExtensionMetaObject(typeof(Type)) as Type, paths);
                }
                
                bool returnValueAsCollection=false;
                //GetDisplayedValue(string path, IPathDataDisplayer pathDataDisplayer,out bool returnValueAsCollection)
                object objectCollection = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(parent.Value, TreeView.ValueType, TreeView.SubNodesProperty as string, parent, out returnValueAsCollection);
                    


                //if (collectionDisplayedValue.Members.ContainsKey("Items"))
                //    objectCollection = collectionDisplayedValue.Members["Items"].ValuesCollection;
                if (!returnValueAsCollection ||objectCollection == null)
                   yield break;


                //object rr = CollectionObjectType;

                //_CollectionObjectType = ViewControlObject.GetClassifier(_Path as string).TemplateBinding.ParameterSubstitutions[0].ActualParameters[0] as OOAdvantech.MetaDataRepository.Classifier;
                System.Collections.IEnumerator enumerator = objectCollection.GetType().GetMethod("GetEnumerator",new System.Type[0]).Invoke(objectCollection, new object[0]) as System.Collections.IEnumerator;
                enumerator.Reset();
                // object obj = enumerator.Current;
                while (enumerator.MoveNext())
                {
                   // OOAdvantech.UserInterface.Runtime.DisplayedValue displayedObj = null;
                    object obj = enumerator.Current;
                    //if (obj is OOAdvantech.UserInterface.Runtime.DisplayedValue)
                    //    displayedObj = obj as OOAdvantech.UserInterface.Runtime.DisplayedValue;
                    //else
                    //    displayedObj = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(obj, null);

                    object displayedPresentationObj = null;
                    if (TreeView.ValueType != TreeView.PresentationObjectType && TreeView.PresentationObjectType != null)
                        displayedPresentationObj = TreeView.UserInterfaceObjectConnection.GetPresentationObject(obj, TreeView.PresentationObjectType as OOAdvantech.MetaDataRepository.Class, TreeView.ValueType.GetExtensionMetaObject(typeof(Type)) as Type);
                    yield return new NodeDisplayedObject(TreeView, obj, displayedPresentationObj);  
                }


            }
            //Node node = FindNode(treePath);
            //if (node == Root)
            //{
            //    yield break;

            //}
            //if (node != null)
            //    foreach (Node n in node.Nodes)
            //        yield return n;
            //else
            //    yield break;
		}

        /// <MetaDataID>{DB1A8251-9B02-48B5-A2AF-F3D1BB328F2A}</MetaDataID>
		public bool IsLeaf(TreePath treePath)
		{
            return false;
			Node node = FindNode(treePath);
			if (node != null)
				return node.IsLeaf;
			else
				throw new ArgumentException("treePath");
		}

		public event EventHandler<TreeModelEventArgs> NodesChanged;
        /// <MetaDataID>{2D06E964-D0FB-4C69-9BC5-49223D31AC04}</MetaDataID>
		internal void OnNodesChanged(TreeModelEventArgs args)
		{
			if (NodesChanged != null)
				NodesChanged(this, args);
		}

		public event EventHandler<TreePathEventArgs> StructureChanged;
        /// <MetaDataID>{73BA9BA9-B238-4A86-858A-62CD2ABAFD46}</MetaDataID>
		public void OnStructureChanged(TreePathEventArgs args)
		{
			if (StructureChanged != null)
				StructureChanged(this, args);
		}
        public void OnStructureChanged(TreeNode treeNode)
        {
            //GetPath(
            
            //if (StructureChanged != null)
            //    StructureChanged(this, args);
        }
  
		public event EventHandler<TreeModelEventArgs> NodesInserted;
        /// <MetaDataID>{A85FCB67-97D5-41DF-B408-6941D3CAEFCB}</MetaDataID>
		internal void OnNodeInserted(Node parent, int index, Node node)
		{
			if (NodesInserted != null)
			{
				TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
				NodesInserted(this, args);
			}

		}

		public event EventHandler<TreeModelEventArgs> NodesRemoved;
        /// <MetaDataID>{45053809-897A-4DC1-9305-5DB09F9A6AB3}</MetaDataID>
		internal void OnNodeRemoved(Node parent, int index, Node node)
		{
			if (NodesRemoved != null)
			{
				TreeModelEventArgs args = new TreeModelEventArgs(GetPath(parent), new int[] { index }, new object[] { node });
				NodesRemoved(this, args);
			}
		}

		#endregion
	}
}
