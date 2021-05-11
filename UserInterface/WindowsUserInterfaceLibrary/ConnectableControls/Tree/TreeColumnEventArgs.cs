using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{8F603E21-6643-41AB-A0C8-19EE396D52BC}</MetaDataID>
	public class TreeColumnEventArgs: EventArgs
	{
		private TreeColumn _column;
		public TreeColumn Column
		{
			get { return _column; }
		}

        /// <MetaDataID>{6D564047-29AC-4EEE-8F89-8ADA9DA149CE}</MetaDataID>
		public TreeColumnEventArgs(TreeColumn column)
		{
			_column = column;
		}
	}
}
