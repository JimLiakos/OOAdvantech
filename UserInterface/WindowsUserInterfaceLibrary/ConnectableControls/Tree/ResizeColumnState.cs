using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Drawing;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{966EE9F0-BED7-4030-B624-AE34D6177791}</MetaDataID>
	internal class ResizeColumnState: InputState
	{
		private const int MinColumnWidth = 10;

		private Point _initLocation;
		private TreeColumn _column;
		private int _initWidth;

        /// <MetaDataID>{C4069089-1F8C-4AC9-AB13-CE3E3C33369E}</MetaDataID>
		public ResizeColumnState(TreeView tree, TreeColumn column, Point p)
			: base(tree)
		{
			_column = column;
			_initLocation = p;
			_initWidth = column.Width;
		}

        /// <MetaDataID>{B564ABD4-F390-456D-89E4-4D90B3B1F107}</MetaDataID>
		public override void KeyDown(KeyEventArgs args)
		{
			args.Handled = true;
			if (args.KeyCode == Keys.Escape)
				FinishResize();
		}

        /// <MetaDataID>{A71AC3C7-D0AA-469C-AFDD-DFDAE0CD29BB}</MetaDataID>
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
		}

        /// <MetaDataID>{9724A592-965B-46F1-B66A-65D8087B8626}</MetaDataID>
		public override void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
			FinishResize();
		}

        /// <MetaDataID>{E7BAD98C-B7F0-4612-904C-E48172029973}</MetaDataID>
		private void FinishResize()
		{
			Tree.ChangeInput();
			Tree.FullUpdate();
			Tree.OnColumnWidthChanged(_column);
		}

        /// <MetaDataID>{F26F5CF6-FD79-4FC9-9683-109BFB1CC98F}</MetaDataID>
		public override bool MouseMove(MouseEventArgs args)
		{
			int w = _initWidth + args.Location.X - _initLocation.X;
			_column.Width = Math.Max(MinColumnWidth, w);
			Tree.UpdateView();
			return true;
		}
	}
}
