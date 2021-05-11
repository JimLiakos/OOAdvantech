using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace ConnectableControls.Tree.NodeControls
{
    /// <MetaDataID>{4F7152B8-D58C-4128-8800-1F12290700B5}</MetaDataID>
	public abstract class EditableControl: BindableControl
	{
        /// <MetaDataID>{81b31fdb-ebba-4de2-aca3-1b1807d6cf13}</MetaDataID>
		private Timer _timer;
        /// <MetaDataID>{d22bd247-217b-4744-a62f-06a437d89b5b}</MetaDataID>
		private bool _editFlag;
        /// <MetaDataID>{d31a005d-7d4a-4d2a-aecf-0005cc3f68ff}</MetaDataID>
		private bool _discardChanges;
        /// <MetaDataID>{fdb31e96-f378-487d-98e7-4bbd4f658d4d}</MetaDataID>
		private TreeNode _editNode;

        /// <MetaDataID>{d1ad6693-0707-4d06-8274-3a74c9761a14}</MetaDataID>
		private bool _editEnabled = false;
        /// <MetaDataID>{5eb69204-4de7-41d4-bcec-ae1ca221902b}</MetaDataID>
		[DefaultValue(false)]
		public bool EditEnabled
		{
			get { return _editEnabled; }
			set { _editEnabled = value; }
		}

        /// <MetaDataID>{CA135279-04C1-4A41-B038-2181BE72A896}</MetaDataID>
		protected EditableControl()
		{
			_timer = new Timer();
			_timer.Interval = 500;
			_timer.Tick += new EventHandler(TimerTick);
		}

        /// <MetaDataID>{3D63AE96-04D2-44D8-982C-011CF503F380}</MetaDataID>
		private void TimerTick(object sender, EventArgs e)
		{
			_timer.Stop();
			if (_editFlag)
				BeginEdit();
			_editFlag = false;
		}

        /// <MetaDataID>{9908DFDC-A1F9-44B8-8B43-10E05C31BC4C}</MetaDataID>
		public void SetEditorBounds(EditorContext context)
		{
			Size size = CalculateEditorSize(context);
			context.Editor.Bounds = new Rectangle(context.Bounds.X, context.Bounds.Y,
				Math.Min(size.Width, context.Bounds.Width), context.Bounds.Height);
		}

        /// <MetaDataID>{C30E3607-00C6-4DCA-A754-5B2A68606461}</MetaDataID>
		protected abstract Size CalculateEditorSize(EditorContext context);

        /// <MetaDataID>{BBF3FEB7-DED3-471F-A750-62F49F5C4816}</MetaDataID>
		protected virtual bool CanEdit(TreeNode node)
		{
            if (node.Tag is NodeDisplayedObject
                && (node.Tag as NodeDisplayedObject).CanEdit)
                return false;

			return (node.Tag != null);
		}

        /// <MetaDataID>{7592E124-6262-44FD-8DFC-53F809D3ED9A}</MetaDataID>
		public void BeginEdit()
		{
			if (EditEnabled && Parent.CurrentNode != null && CanEdit(Parent.CurrentNode))
			{
				CancelEventArgs args = new CancelEventArgs();
				OnEditorShowing(args);
				if (!args.Cancel)
				{
					_discardChanges = false;
					Control control = CreateEditor(Parent.CurrentNode);
					_editNode = Parent.CurrentNode;
					control.Disposed += new EventHandler(EditorDisposed);
					Parent.DisplayEditor(control, this);
				}
			}
		}

        /// <MetaDataID>{E5D1EE42-9EE5-40B3-A895-BB58E420D629}</MetaDataID>
		public void EndEdit(bool cancel)
		{
			_discardChanges = cancel;
			Parent.HideEditor();
		}

        /// <MetaDataID>{3BAA24B8-1D93-4ED6-B556-F6EE938DB523}</MetaDataID>
		public virtual void UpdateEditor(Control control)
		{
		}

        /// <MetaDataID>{132AE141-0D4B-497F-BD52-69F76438DD4B}</MetaDataID>
		private void EditorDisposed(object sender, EventArgs e)
		{
			OnEditorHided();
			if (!_discardChanges && _editNode != null)
				ApplyChanges(_editNode);
			_editNode = null;
		}

        /// <MetaDataID>{56F7ECA6-7FC2-4BB8-A443-ACB9D41E5336}</MetaDataID>
		private void ApplyChanges(TreeNode node)
		{
			try
			{
				DoApplyChanges(node);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Value is not valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

        /// <MetaDataID>{DBD40069-379B-49DD-947A-D6BEE0E31E80}</MetaDataID>
		protected abstract void DoApplyChanges(TreeNode node);

        /// <MetaDataID>{2D3F3783-DF9C-46F4-A912-E710E70CEB40}</MetaDataID>
		protected abstract Control CreateEditor(TreeNode node);

        /// <MetaDataID>{EB02882A-0962-4624-A5CC-98BD4C4FDFA6}</MetaDataID>
		public override void MouseDown(TreeNodeAdvMouseEventArgs args)
		{
			_editFlag = (!Parent.UseColumns && args.Button == MouseButtons.Left
				&& args.ModifierKeys == Keys.None && args.Node.IsSelected);
		}

        /// <MetaDataID>{6A80AA3A-3E4F-4CC2-A301-ADF1F30FEB96}</MetaDataID>
		public override void MouseUp(TreeNodeAdvMouseEventArgs args)
		{
			if (_editFlag && args.Node.IsSelected)
				_timer.Start();
		}

        /// <MetaDataID>{A86D0FFA-8DA3-4A38-B07F-ED73773E4869}</MetaDataID>
		public override void MouseDoubleClick(TreeNodeAdvMouseEventArgs args)
		{
			_timer.Stop();
			_editFlag = false;
			if (Parent.UseColumns)
			{
				args.Handled = true;
				BeginEdit();
			}
		}

        /// <MetaDataID>{73B63CD9-E857-480C-BFD9-3A4A585315D2}</MetaDataID>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
				_timer.Dispose();
		}

		#region Events

		public event CancelEventHandler EditorShowing;
        /// <MetaDataID>{4AE53DC1-6E96-4FC0-951B-D3BE0FF66257}</MetaDataID>
		protected void OnEditorShowing(CancelEventArgs args)
		{
			if (EditorShowing != null)
				EditorShowing(this, args);
		}

		public event EventHandler EditorHided;
        /// <MetaDataID>{57E766EA-BEDD-4A5F-8301-62720ECA2F1D}</MetaDataID>
		protected void OnEditorHided()
		{
			if (EditorHided != null)
				EditorHided(this, EventArgs.Empty);
		}

		#endregion

	}
}
