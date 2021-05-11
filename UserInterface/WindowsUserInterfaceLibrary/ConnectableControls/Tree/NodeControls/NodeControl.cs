using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{33DC78AF-D71B-40ED-A587-E9B0E461D7EF}</MetaDataID>
	[DesignTimeVisible(false), ToolboxItem(false)]
	public abstract class NodeControl: Component
	{
		#region Properties

        /// <MetaDataID>{1baba736-a1f0-4da8-a15f-f67726febe84}</MetaDataID>
		private TreeView _parent;
        /// <MetaDataID>{152b0179-19d7-45ba-9817-0a7478988f4a}</MetaDataID>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TreeView Parent
		{
			get { return _parent; }
			set 
			{
				if (value != _parent)
				{
					if (_parent != null)
						_parent.NodeControls.Remove(this);

					if (value != null)
						value.NodeControls.Add(this);
				}
			}
		}

        /// <MetaDataID>{c3e41b6f-05e0-4618-9d3e-8dd4b08f8e35}</MetaDataID>
		private IToolTipProvider _toolTipProvider;
        /// <MetaDataID>{601d2b43-5ff0-42d5-ac72-7c329d2ca243}</MetaDataID>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IToolTipProvider ToolTipProvider
		{
			get { return _toolTipProvider; }
			set { _toolTipProvider = value; }
		}

        /// <MetaDataID>{d58ac099-8db6-4333-9aa4-26c75b54da5e}</MetaDataID>
		private int _column;
        /// <MetaDataID>{02cd0ca2-cc6a-44d0-a6ee-26d52dd6f138}</MetaDataID>
		[DefaultValue(0)]
		public int Column
		{
			get { return _column; }
			set 
			{
				if (_column < 0)
					throw new ArgumentOutOfRangeException("value");

				_column = value;
				if (_parent != null)
					_parent.FullUpdate();
			}
		}

		#endregion

        /// <MetaDataID>{A9FFC278-B910-4CA6-B883-217B7983C3C0}</MetaDataID>
		internal void AssignParent(TreeView parent)
		{
			_parent = parent;
		}

        /// <MetaDataID>{463A7816-7A65-43A5-B432-E355F7FDAF96}</MetaDataID>
		public abstract Size MeasureSize(TreeNode node);

        /// <MetaDataID>{F9414C84-7390-4587-99D9-0FD64632659C}</MetaDataID>
		public abstract void Draw(TreeNode node, DrawContext context);

        /// <MetaDataID>{E3302822-7802-4736-A55E-9F23CCE03E24}</MetaDataID>
		public virtual string GetToolTip(TreeNode node)
		{
			if (ToolTipProvider != null)
				return ToolTipProvider.GetToolTip(node);
			else
				return string.Empty;
		}

        /// <MetaDataID>{11FDCE2E-B38A-430F-B5B1-DF6EC9748F74}</MetaDataID>
		public virtual void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
		}

        /// <MetaDataID>{A677740A-70C6-401F-9B75-37A0DA5A3283}</MetaDataID>
		public virtual void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
		}

        /// <MetaDataID>{07B0B39F-3CDD-4B31-A999-6A1F3253DF0A}</MetaDataID>
		public virtual void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
		}

        /// <MetaDataID>{6FE58BBA-92A7-457A-84C8-079868EA026A}</MetaDataID>
		public virtual void KeyDown(KeyEventArgs args)
		{
		}

        /// <MetaDataID>{F4E0CA59-7A1E-4C76-B446-7976C241AD6C}</MetaDataID>
		public virtual void KeyUp(KeyEventArgs args)
		{
		}
	}
}
