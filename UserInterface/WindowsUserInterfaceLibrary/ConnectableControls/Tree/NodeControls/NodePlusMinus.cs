using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ConnectableControls;
using ConnectableControls.Properties;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{0FA8128C-E689-4386-B947-C3802EBC93FC}</MetaDataID>
	internal class NodePlusMinus : NodeControl
	{
        /// <MetaDataID>{ddf7207f-6124-45e3-bb8a-5d349f0ee755}</MetaDataID>
		public const int ImageSize = 9;
        /// <MetaDataID>{14c47de6-a1d3-45ed-9ae9-34de5de6f05c}</MetaDataID>
		public const int Width = 16;
        /// <MetaDataID>{5dc21488-02f4-4810-95c1-8c9e5ce88b64}</MetaDataID>
		private Bitmap _plus;
        /// <MetaDataID>{c751e2c1-2010-4f6e-9983-a67e489ebe41}</MetaDataID>
		private Bitmap _minus;

        /// <MetaDataID>{CEC7D817-CA16-4C15-B81A-97E7FD78F4D7}</MetaDataID>
		public NodePlusMinus()
		{
            
			_plus = Resources.plus;
			_minus = Resources.minus;
		}

        /// <MetaDataID>{C959A14C-FBCA-4433-961D-856E2DEE95BE}</MetaDataID>
		public override Size MeasureSize(TreeNode node)
		{         
			return new Size((int)(Width* node.Tree.TextScaleFactor),(int)( Width* node.Tree.TextScaleFactor));
		}

        /// <MetaDataID>{E61BB358-9F2C-4A4D-A72C-BE30668EC0EB}</MetaDataID>
		public override void Draw(TreeNode node, DrawContext context)
		{
            double textScale = context.Graphics.DpiX / 96;
			if (node.CanExpand)
			{
				Rectangle r = context.Bounds;
                Brush textBrush = SystemBrushes.ControlText;
                int dy = (int)Math.Round((float)(r.Height - (int)(ImageSize * textScale)) / 2);
				if (Application.RenderWithVisualStyles)
				{
					VisualStyleRenderer renderer;
					if (node.IsExpanded)
						renderer = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
					else
						renderer = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
                    renderer.DrawBackground(context.Graphics, new Rectangle(r.X, r.Y + dy, (int)(ImageSize * textScale), (int)(ImageSize * textScale)));
				}
				else
				{
					Image img;
					if (node.IsExpanded)
						img = _minus;
					else
						img = _plus;
                    
					context.Graphics.DrawImageUnscaled(img, new Point(r.X, r.Y + dy));
				}
			}
		}

        /// <MetaDataID>{86BB9A7D-35B5-4E0F-9101-E4DDC8623D08}</MetaDataID>
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			if (args.Button == MouseButtons.Left)
			{
				args.Handled = true;
				if (args.Node.CanExpand)
					args.Node.IsExpanded = !args.Node.IsExpanded;
			}
		}

        /// <MetaDataID>{0BFDA06D-FB36-42D0-BA49-9A6F7A07B726}</MetaDataID>
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			args.Handled = true; // Supress expand/collapse when double click on plus/minus
		}
	}
}
