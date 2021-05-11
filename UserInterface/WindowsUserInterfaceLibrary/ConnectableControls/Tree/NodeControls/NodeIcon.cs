using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{0D616D71-DF54-4E57-AE07-C255C8010F8E}</MetaDataID>
	public class NodeIcon : BindableControl
	{

        public NodeIcon()
        {
            DataPropertyName = "Image";
        }
        /// <MetaDataID>{F59D2AC9-2369-4182-8460-63A87C63BCB5}</MetaDataID>
		public override Size MeasureSize(TreeNode node)
		{
            
            
            if (node.Tag is NodeDisplayedObject)
                return (node.Tag as NodeDisplayedObject).GetSize(this);
            Image image = GetIcon(node);
			if (image != null)
				return new Size((int)(image.Size.Width*Parent.TextScaleFactor),(int)(image.Size.Height*Parent.TextScaleFactor));
			else
				return Size.Empty;
		}

        /// <MetaDataID>{0FF793CC-14DC-44E5-A1A7-D5BA479B565F}</MetaDataID>
		public override void Draw(TreeNode node, DrawContext context)
		{
            double textScale = context.Graphics.DpiX / 96;

			Image image = GetIcon(node);
            
			if (image != null)
			{
				Point point = new Point(context.Bounds.X,
					context.Bounds.Y + (context.Bounds.Height - (int)(image.Size.Height * textScale)) / 2);
                Rectangle imageRect = new Rectangle(point.X, point.Y, (int)(image.Size.Width * textScale), (int)(image.Size.Height * textScale));
                context.Graphics.DrawImage(image, imageRect);
			}
		}

        /// <MetaDataID>{7B48EAD9-9771-497E-A690-B73EC5194EEC}</MetaDataID>
		protected virtual Image GetIcon(TreeNode node)
		{
			return GetValue(node) as Image;
		}
	}
}
