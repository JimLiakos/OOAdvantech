using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{49B2B427-79C9-4DB5-A3CC-89394D7CF1AA}</MetaDataID>
	public class TreeModelEventArgs: TreePathEventArgs
	{
		private object[] _children;
		public object[] Children
		{
			get { return _children; }
		}

		private int[] _indices;
		public int[] Indices
		{
			get { return _indices; }
		}

        /// <summary>
        /// </summary>
        /// <param name="parent">Path to a parent node</param>
        /// <param name="children">Child nodes</param>
        /// <MetaDataID>{3050DE3D-BD04-4946-8531-8424CDE7E34A}</MetaDataID>
		public TreeModelEventArgs(TreePath parent, object[] children)
			: this(parent, null, children)
		{
		}

        /// <summary>
        /// </summary>
        /// <param name="parent">Path to a parent node</param>
        /// <param name="indices">Indices of children in parent nodes collection</param>
        /// <param name="children">Child nodes</param>
        /// <MetaDataID>{7FC564D5-F6CC-4E50-98B0-50AC2B9E6468}</MetaDataID>
		public TreeModelEventArgs(TreePath parent, int[] indices, object[] children)
			: base(parent)
		{
			if (children == null)
				throw new ArgumentNullException();

			if (indices != null && indices.Length != children.Length)
				throw new ArgumentException("indices and children arrays must have the same length");

			_indices = indices;
			_children = children;
		}
	}
}
