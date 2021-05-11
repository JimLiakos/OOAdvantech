using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{6EA40FBF-E011-430D-9B70-4FE090E3B635}</MetaDataID>
	public class TreePathEventArgs : EventArgs
	{
		private TreePath _path;
		public TreePath Path
		{
			get { return _path; }
		}

        /// <MetaDataID>{DD0C4204-E917-46ED-A464-6BC88ECFCC9B}</MetaDataID>
		public TreePathEventArgs()
		{
			_path = new TreePath();
		}

        /// <MetaDataID>{4D79E688-3B4C-4637-924B-3718C5985CD8}</MetaDataID>
		public TreePathEventArgs(TreePath path)
		{
			if (path == null)
				throw new ArgumentNullException();

			_path = path;
		}
	}
}
