using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{D0291E04-A92B-467D-9AE5-1D016774D177}</MetaDataID>
	public interface IToolTipProvider
	{
        /// <MetaDataID>{E31D0B86-F8B8-4911-A56D-A55C04CC2B50}</MetaDataID>
		string GetToolTip(TreeNode node);
	}
}
