using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.UserInterface.Runtime;
using System.Drawing;

namespace ConnectableControls.Tree
{
    /// <MetaDataID>{62e28366-638e-4704-9a2f-b2e9486498ff}</MetaDataID>
    public class NodeDisplayedObject : OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {

        public void LockStateChange(object sender)
        {
        }

        public UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get
            {
                return TreeView.UserInterfaceObjectConnection;
            }
        }

        /// <MetaDataID>{e0fc9bab-104a-4be4-8fa4-af8d4de7d6e8}</MetaDataID>
        TreeView TreeView;
        /// <MetaDataID>{4a877c3e-c470-4de2-8f7a-76df56eb965b}</MetaDataID>
        readonly object _Value;
        /// <MetaDataID>{1efa786b-2bb7-4053-9fac-70bbadc24afd}</MetaDataID>
        readonly object _PresentationObject;
        public bool HasLockRequest
        {
            get
            {
                return false;
            }
        }
        /// <MetaDataID>{a5f1bd9e-ada1-4669-808e-07ee120bcab1}</MetaDataID>
        public void TransactionLocked(bool locked)
        {
        }


        /// <MetaDataID>{56074bbf-6999-4473-82e6-c0f006e2c105}</MetaDataID>
        public NodeDisplayedObject(TreeView treeView, object value, object presentationObject)
        {
            if (value == null)
                throw new System.ArgumentException("value must be not null", "displayedValue");

            if (treeView == null)
                throw new System.ArgumentException("treeView must be not null", "treeView");

            _PresentationObject = presentationObject;
            _Value = value;
            TreeView = treeView;
        }

        /// <MetaDataID>{649042d5-66d6-4551-9337-915e7ff56910}</MetaDataID>
        public object PresentationObject
        {
            get
            {
                if (_PresentationObject == null)
                    return _Value;
                else
                    return _PresentationObject;
            }
        }
        /// <MetaDataID>{4718c797-bf79-41de-b2dd-6fbca561aec8}</MetaDataID>
        public object Value
        {
            get
            {
                return _Value;
            }
        }
        /// <MetaDataID>{aa9c28f7-c2cf-4028-8503-c71a4d69027c}</MetaDataID>
        private static Graphics _measureGraphics = Graphics.FromImage(new Bitmap(1, 1));
        /// <MetaDataID>{6e6d714c-9ef2-4271-bd2f-eb36c4b6ea65}</MetaDataID>
        TreeNode _TreeNode;
        /// <MetaDataID>{26e7f9c8-05f0-4f19-9e7b-2fd94a3a97a8}</MetaDataID>
        internal TreeNode TreeNode
        {
            get
            {
                return _TreeNode;
            }
            set
            {
                _TreeNode = value;
            }

        }
        /// <MetaDataID>{6c81dddd-087f-4388-9b1c-094e37a6dc7e}</MetaDataID>
        public bool CanEdit
        {
            get
            {
                if (string.IsNullOrEmpty((TreeView.DisplayMember as string)))
                    return false;


                return TreeView.UserInterfaceObjectConnection.CanEditValue(TreeView.DisplayMember as string, this);
            }

        }
        /// <MetaDataID>{31ba2572-1bcc-4ecf-a6d9-5d96fa11cf0f}</MetaDataID>
        string _Text;

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_Text))
                return _Text;
            if (string.IsNullOrEmpty((TreeView.DisplayMember as string)) && PresentationObject != null)
            {
                _Text = PresentationObject.ToString();
                return _Text;
            }
            bool returnValueAsCollection = false;
            if (!string.IsNullOrEmpty((TreeView.DisplayMember as string)) && PresentationObject != null)
            {
                _Text = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(PresentationObject, TreeView.PresentationObjectType, TreeView.DisplayMember as string, this, out returnValueAsCollection) as string;
                return _Text;
            }
            return base.ToString();
        }
        /// <MetaDataID>{c1107585-79ac-4869-900e-de075f4e3305}</MetaDataID>
        public string Text
        {
            get
            {
                return ToString();
            }
            set
            {
                _Text = value;
                TreeView.UserInterfaceObjectConnection.SetValue(PresentationObject, value, TreeView.PresentationObjectType, TreeView.DisplayMember as string);


            }
        }
        /// <MetaDataID>{5b0b3f5b-a008-45a5-bdb3-e3c45de6292f}</MetaDataID>
        bool _Checked;
        /// <MetaDataID>{2d43161b-8c6f-4026-8421-f88248749548}</MetaDataID>
        public bool Checked
        {
            get
            {
                if (!string.IsNullOrEmpty((TreeView.CheckUncheckPath as string)) && PresentationObject != null)
                {
                    bool returnValueAsCollection = false;
                    object value = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(PresentationObject, TreeView.PresentationObjectType, TreeView.CheckUncheckPath as string, this, out returnValueAsCollection);
                    if (value is bool)
                        return (bool)value;

                }
                return false;
            }
            set
            {
                //_Checked = value;
                if (!string.IsNullOrEmpty((TreeView.CheckUncheckPath as string)) && PresentationObject != null)
                    TreeView.UserInterfaceObjectConnection.SetValue(PresentationObject, value, TreeView.PresentationObjectType, TreeView.CheckUncheckPath as string);
            }
        }

        /// <MetaDataID>{defba8a2-9e57-4108-a0c7-9d4628d0a6d2}</MetaDataID>
        Image _Image;
        /// <MetaDataID>{8f359ac3-640a-4482-97a8-6579ea8ee300}</MetaDataID>
        public Image Image
        {
            get
            {
                if (_Image != null)
                    return _Image;
                if (!string.IsNullOrEmpty((TreeView.ImagePath as string)) && PresentationObject != null)
                {
                    bool returnValueAsCollection = false;
                    _Image = TreeView.UserInterfaceObjectConnection.GetDisplayedValue(PresentationObject, TreeView.PresentationObjectType, TreeView.ImagePath as string, this, out returnValueAsCollection) as Image;
                    if (_Image != null)
                        this._IconSize = _Image.Size;
                }
                return _Image;
            }
            //set
            //{
            //    Image = value;
            //    TreeView.UserInterfaceObjectConnection.SetValue(PresentationObject, value, TreeView.PresentationObjectType, TreeView.DisplayMember as string);
            //}
        }

        #region IPathDataDisplayer Members






        /// <MetaDataID>{da05ed5e-d91c-4695-a2ef-99e821fc5989}</MetaDataID>
        bool InDisplayedValueChanged = false;
        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg memberChangeEventArg)
        {
            //if (InDisplayedValueChanged)
            //    return;
            try
            {
                _LabelSize = new Size(-1, -1);
                _IconSize = new Size(-1, -1);

                _Text = null;
                _Image = null;
                InDisplayedValueChanged = true;

                if (TreeNode.Parent == null)
                    return;
                (TreeView.Model as TreeModel).OnStructureChanged(new TreePathEventArgs(TreeView.GetPath(TreeNode)));
            }
            finally
            {
                InDisplayedValueChanged = false;
            }
        }



        /// <MetaDataID>{b1f4573e-4b4c-4fa6-a9f2-ed5bd5384290}</MetaDataID>
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get
            {
                return new OOAdvantech.Collections.Generic.List<string>();

            }
        }

        #endregion

        /// <MetaDataID>{aa162737-e80a-41f0-b0e8-9ed1c1236e4d}</MetaDataID>
        Size _LabelSize = new Size(-1, -1);
        Size _IconSize = new Size(-1, -1);
        /// <MetaDataID>{10ffbec3-8509-4ab1-8be2-593682dec498}</MetaDataID>
        internal Size GetSize(ConnectableControls.Tree.NodeControls.NodeControl nodeControl)
        {
            if (nodeControl is NodeControls.BaseTextControl)
            {
                if (_LabelSize.Height == -1)
                {
                    _LabelSize = GetLabelSize(Text, (nodeControl as NodeControls.BaseTextControl).Font);
                    return _LabelSize;
                }
                else
                    return _LabelSize;
            }
            if (nodeControl is NodeControls.NodeIcon)
            {
                if (_IconSize.Height == -1)
                {
                    if (Image == null)
                        return Size.Empty;
                    else
                        _IconSize = Image.Size;
                    return new Size((int)(_IconSize.Width * this.TreeNode.Tree.TextScaleFactor), (int)(_IconSize.Height * this.TreeNode.Tree.TextScaleFactor));
                }
                else

                    return new Size((int)(_IconSize.Width * this.TreeNode.Tree.TextScaleFactor), (int)(_IconSize.Height * this.TreeNode.Tree.TextScaleFactor));
            }

            throw new System.NotImplementedException();

        }

        /// <MetaDataID>{e7df0ddb-d017-4f0c-9dcc-0a6a8a500696}</MetaDataID>
        protected Size GetLabelSize(string label, Font font)
        {
            if (label == null)
                label = "";

            SizeF s = _measureGraphics.MeasureString(label, font);
            if (!s.IsEmpty)
                return new Size((int)s.Width , (int)s.Height);
            else
                return new Size(10, font.Height);
        }


        #region IPathDataDisplayer Members

        public object Path
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void LoadControlValues()
        {
            throw new NotImplementedException();
        }

        public void SaveControlValues()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
