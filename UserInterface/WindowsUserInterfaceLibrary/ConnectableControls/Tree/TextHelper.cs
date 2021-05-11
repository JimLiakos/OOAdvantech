using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{B49F4323-7E6D-41A7-B90B-D537C38832C9}</MetaDataID>
	public static class TextHelper
	{
        /// <MetaDataID>{D7E6DE59-92C0-46BA-A49D-A295F1B41AD0}</MetaDataID>
		public static StringAlignment TranslateAligment(HorizontalAlignment aligment)
		{
			if (aligment == HorizontalAlignment.Left)
				return StringAlignment.Near;
			else if (aligment == HorizontalAlignment.Right)
				return StringAlignment.Far;
			else
				return StringAlignment.Center;
		}
	}
}
