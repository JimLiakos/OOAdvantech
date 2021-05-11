using System;
using System.Text;
using System.Collections.ObjectModel;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{2058B196-500F-4A64-B867-6D95DC9FAE19}</MetaDataID>
	public class TreePath
	{
		public static readonly TreePath Empty = new TreePath();

		private object[] _path;
		public object[] FullPath
		{
			get { return _path; }
		}

        #region public object LastNode
        public object LastNode
        {
            get
            {
                if (_path.Length > 0)
                    return _path[_path.Length - 1];
                else
                    return null;
            }
        } 
        #endregion

        #region public object FirstNode
        public object FirstNode
        {
            get
            {
                if (_path.Length > 0)
                    return _path[0];
                else
                    return null;
            }
        } 
        #endregion

        #region Constructors
        /// <MetaDataID>{9826A481-6862-4151-9FE1-4CE18FAD8054}</MetaDataID>
        public TreePath()
        {
            _path = new object[0];
        }

        /// <MetaDataID>{F9D6B32B-98C8-40E3-847C-AC73A1181216}</MetaDataID>
        public TreePath(object node)
        {
            _path = new object[] { node };
        }

        /// <MetaDataID>{E6A9F787-9841-4DFE-ABEE-34A963C670AA}</MetaDataID>
        public TreePath(object[] path)
        {
            _path = path;
        } 
        #endregion

        /// <MetaDataID>{3FB79CA9-60BD-41A6-B00F-8C7FF1BEC45A}</MetaDataID>
		public bool IsEmpty()
		{
			return (_path.Length == 0);
		}
	}
}
