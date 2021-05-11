using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{BC3B99C7-BB62-4C10-8D62-C82F9C5B80D7}</MetaDataID>
	public class TreeColumn: IDisposable
	{
		private StringFormat _headerFormat;

		#region Properties

		private TreeView _treeView;
		internal TreeView TreeView
		{
			get { return _treeView; }
			set { _treeView = value; }
		}

		private int _index;
		[Browsable(false)]
		public int Index
		{
			get { return _index; }
			internal set { _index = value; }
		}

		private string _header;
		[Localizable(true)]
		public string Header
		{
			get { return _header; }
			set 
			{ 
				_header = value;
				if (TreeView != null)
					TreeView.UpdateHeaders();
			}
		}

		private int _width;
		[DefaultValue(50), Localizable(true)]
		public int Width
		{
			get { return _width; }
			set 
			{
				if (_width != value)
				{
					if (value < 0)
						throw new ArgumentOutOfRangeException("value");

					_width = value;
					if (TreeView != null)
						TreeView.ChangeColumnWidth(this);
				}
			}
		}

		private bool _visible = true;
		[DefaultValue(true)]
		public bool IsVisible
		{
			get { return _visible; }
			set 
			{ 
				_visible = value;
				if (TreeView != null)
					TreeView.FullUpdate();
			}
		}

		private HorizontalAlignment _textAlign = HorizontalAlignment.Left;
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign
		{
			get { return _textAlign; }
			set { _textAlign = value; }
		}

		#endregion

        /// <MetaDataID>{6A25891D-72A6-425C-A05F-FDD6C1A7172F}</MetaDataID>
		public TreeColumn(): 
			this(string.Empty, 50)
		{
		}

        /// <MetaDataID>{6AA15798-F429-43E8-83AD-3D7FEFE8AD1D}</MetaDataID>
		public TreeColumn(string header, int width)
		{
			_header = header;
			_width = width;

			_headerFormat = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces);
			_headerFormat.LineAlignment = StringAlignment.Center;
			_headerFormat.Trimming = StringTrimming.EllipsisCharacter;
		}

        /// <MetaDataID>{1E46DFF8-6808-4394-BC41-7361AD990FF6}</MetaDataID>
		public override string ToString()
		{
			if (string.IsNullOrEmpty(Header))
				return GetType().Name;
			else
				return Header;
		}

        /// <MetaDataID>{C363D950-08BC-4400-966C-A4213B0D0136}</MetaDataID>
		public void Draw(Graphics gr, Rectangle bounds, Font font)
		{
			_headerFormat.Alignment = TextHelper.TranslateAligment(TextAlign);
			gr.DrawString(Header + " ", font, SystemBrushes.WindowText, bounds, _headerFormat);
		}

		#region IDisposable Members

        /// <MetaDataID>{FA505A70-49CA-47A0-B0D0-E7F08F290B67}</MetaDataID>
		public void Dispose()
		{
			Dispose(true);
		}

        /// <MetaDataID>{7811BCED-F84F-4A7A-A331-2DE61B19F461}</MetaDataID>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				_headerFormat.Dispose();
		}

		#endregion
	}
}
