using System;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using ConnectableControls.ListView;
using System.Linq;
namespace ConnectableControls.PropertyEditors
{

    /// <MetaDataID>{39174A7F-5BAB-4231-BFDC-10242A09C4B1}</MetaDataID>
    public class MetaDataNavigator : System.Windows.Forms.Form
    {

        public enum NavigatorType
        {
            Type,
            InstanceMember,
            StaticMember

        }
        /// <MetaDataID>{8F7414F4-DC4A-4FBC-B37B-ACD1D4E3777B}</MetaDataID>
        protected MetaDataNavigator()
        {

            InitializeComponent();

        }




        /// <MetaDataID>{e19e2dc5-5737-41b1-b3e1-38c7e0443009}</MetaDataID>
        internal NavigatorType Type = NavigatorType.Type;
        /// <MetaDataID>{0CED9E02-BDAF-42B6-BA97-F901622C661F}</MetaDataID>
        internal abstract class MetaObjectItemView : System.Windows.Forms.TreeNode
        {
            internal IColumn Column;
            protected bool AutoExpand = false;
            public OOAdvantech.MetaDataRepository.MetaObject MetaObject;
            /// <MetaDataID>{B31A3A0F-1677-4327-BECC-72DA6D9A5F84}</MetaDataID>
            public MetaObjectItemView(OOAdvantech.MetaDataRepository.MetaObject metaObject, System.Windows.Forms.TreeView treeView, System.Windows.Forms.TreeNodeCollection parenNodeColection)
                : base(metaObject.Name)
            {
                treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
                string temp = metaObject.Name;
                MetaObject = metaObject;
                if (!parenNodeColection.Contains(this))
                    parenNodeColection.Add(this);
                treeView.FindForm().FormClosed += new FormClosedEventHandler(FormClosed);

            }

            /// <MetaDataID>{7FDF6EC1-6847-4F3D-AF76-F306E62CBE1D}</MetaDataID>
            void FormClosed(object sender, FormClosedEventArgs e)
            {
                if (this.Column != null && this.Column.Owner == null && this.Column.ColumnMetaData != null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(this.Column.ColumnMetaData);

            }

            /// <MetaDataID>{350CBEF4-49D8-4FE5-A7C2-21DD723D3A60}</MetaDataID>
            void treeView_AfterCheck(object sender, TreeViewEventArgs e)
            {
                if (!(this is AttributeItemView) && !(this is AssociationEndItemView) && Checked)
                    Checked = false;


                //if (!Checked && IsSubNodeChecked)
                //    Checked = true;

            }



            /// <MetaDataID>{30295D8B-2083-4806-A2CB-CCB8D887A6FA}</MetaDataID>
            public abstract void Expand();

            public bool IsSubNodeChecked
            {
                get
                {
                    if (Checked)
                        return true;
                    else
                    {
                        foreach (TreeNode treeNode in Nodes)
                        {
                            MetaObjectItemView metaObjectItemView = treeNode as MetaObjectItemView;


                            if (metaObjectItemView != null && metaObjectItemView.IsSubNodeChecked)
                                return true;
                        }
                    }
                    return false;
                }

            }
            public bool IsChecked
            {
                set
                {
                    //if (value && !Checked)
                    //{

                    //    Checked=value;
                    //    if((Parent as MetaObjectItemView) != null)
                    //        (Parent as MetaObjectItemView).IsChecked=true;
                    //}

                }
            }





        }


        /// <MetaDataID>{C16D3B4A-A62E-4879-B063-98E251971FBC}</MetaDataID>
        internal class AttributeItemView : MetaObjectItemView
        {
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.Attribute Attribute;
            /// <MetaDataID>{B16D8B3D-CD47-4D24-9D73-23BBE3C1EE0E}</MetaDataID>
            public AttributeItemView(OOAdvantech.MetaDataRepository.Attribute attribute, System.Windows.Forms.TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(attribute, treeView, parenNodeColection)
            {

                treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
                ImageIndex = 5;
                SelectedImageIndex = 5;
                Attribute = attribute;


                if ((treeView.FindForm() as MetaDataNavigator).ListView != null)
                {
                    foreach (IColumn column in (treeView.FindForm() as MetaDataNavigator).ListView.Columns)
                    {
                        string columnPath = column.Path as string;
                        TreeNode treeNode = this;
                        string path = null;
                        while (treeNode.Parent != null)
                        {
                            if (treeNode is AssociationEndItemView ||
                                treeNode is AttributeItemView ||
                                treeNode is OperationItemView)
                            {
                                if (path == null)
                                    path = treeNode.Text;
                                else
                                    path = treeNode.Text + "." + path;
                            }
                            treeNode = treeNode.Parent;
                        }
                        string member = null;
                        if (path == columnPath && !string.IsNullOrEmpty(path))
                        {
                            Column = column;
                            Checked = true;
                        }
                        else if (path != null && columnPath.IndexOf(path) == 0)
                        {
                            columnPath = columnPath.Substring(path.Length + 1);
                            int nPos = columnPath.IndexOf(".");
                            if (nPos != -1)
                                member = columnPath.Substring(0, nPos);
                            else
                                member = columnPath;

                        }
                        else if (path == null)
                        {
                            int nPos = columnPath.IndexOf(".");
                            if (nPos != -1)
                                member = columnPath.Substring(0, nPos);
                            else
                                member = columnPath;


                        }

                        if (!string.IsNullOrEmpty(member))
                        {
                            foreach (OOAdvantech.MetaDataRepository.Attribute subAttribute in Attribute.Type.GetAttributes(true))
                            {
                                if (subAttribute.Name == member)
                                {
                                    AutoExpand = true;
                                    break;
                                }
                            }
                            if (!AutoExpand)
                            {
                                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Attribute.Type.GetAssociateRoles(true))
                                {
                                    if (associationEnd.Name == member)
                                    {
                                        AutoExpand = true;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }

                OOAdvantech.MetaDataRepository.Classifier classifier = attribute.Type;
                int subItems = 0;
                foreach (OOAdvantech.MetaDataRepository.Attribute inAttribute in classifier.GetAttributes(true))
                {
                    if (inAttribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    subItems++;

                }
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                {
                    if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !associationEnd.Navigable)
                        continue;
                    subItems++;
                }
                foreach (OOAdvantech.MetaDataRepository.Operation operation in classifier.GetOperations(true))
                {
                    if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    subItems++;
                }
                if (subItems > 0)
                {
                    Nodes.Add("Loadding...");
                    if (AutoExpand)
                        Expand();
                }

            }

            /// <MetaDataID>{28625B10-BC9B-47D1-A8C5-50DB26FF6C03}</MetaDataID>
            void treeView_AfterCheck(object sender, TreeViewEventArgs e)
            {
                if (Checked && (Parent as MetaObjectItemView) != null)
                    (Parent as MetaObjectItemView).IsChecked = true;

            }
            /// <MetaDataID>{1B228820-2B3D-4C26-B033-D03C511064A2}</MetaDataID>
            public override void Expand()
            {
                if (Loaded)
                    return;
                try
                {
                    Loaded = true;
                    Nodes.Clear();
                    OOAdvantech.MetaDataRepository.Classifier classifier = Attribute.Type;
                    if (Attribute.Getter != null || Attribute.Setter != null)
                    {
                        TreeNode treeNode = Nodes.Add("Accessors");
                        treeNode.ImageIndex = 7;
                        treeNode.SelectedImageIndex = 7;
                        if (Attribute.Getter != null)
                            new OperationItemView(Attribute.Getter, TreeView, treeNode.Nodes);
                        if (Attribute.Setter != null)
                            new OperationItemView(Attribute.Setter, TreeView, treeNode.Nodes);
                    }

                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (from _attribute in classifier.GetAttributes(true) orderby _attribute.Name select _attribute).ToArray())
                    {
                        if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                            continue;
                        new AttributeItemView(attribute, TreeView, Nodes);
                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in (from _associationEnd in classifier.GetAssociateRoles(true) orderby _associationEnd.Name select _associationEnd))
                    {
                        if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !associationEnd.Navigable)
                            continue;
                        new AssociationEndItemView(associationEnd, TreeView, Nodes);
                    }
                    foreach (OOAdvantech.MetaDataRepository.Operation operation in (from _operation in classifier.GetOperations(true) orderby _operation.Name select _operation))
                    {
                        if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                            continue;
                        new OperationItemView(operation, TreeView, Nodes);
                    }
                }
                catch (Exception error)
                {



                }
            }
        }


        /// <MetaDataID>{907CACB6-9B24-4E07-9BBE-C61318467A8F}</MetaDataID>
        internal class ParameterItemView : MetaObjectItemView
        {
            /// <MetaDataID>{B7A1AA10-B2D6-4B70-B1C4-B7C2AD07D492}</MetaDataID>
            public override void Expand()
            {

            }
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.Parameter Parameter;
            /// <MetaDataID>{574E508D-8F6F-44FA-A039-D5B25E5FC6F5}</MetaDataID>
            public ParameterItemView(OOAdvantech.MetaDataRepository.Parameter parameter, System.Windows.Forms.TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(parameter, treeView, parenNodeColection)
            {

                treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
                ImageIndex = 8;
                SelectedImageIndex = 8;
                Parameter = parameter;

            }

            /// <MetaDataID>{F472A22F-FFA3-49D2-A9F3-C5E2B7C169D2}</MetaDataID>
            void treeView_AfterCheck(object sender, TreeViewEventArgs e)
            {
                if (Checked && (Parent as MetaObjectItemView) != null)
                    (Parent as MetaObjectItemView).IsChecked = true;

            }
        }



        /// <MetaDataID>{33BB7B7E-5DEA-4DA9-948D-749DF79691F0}</MetaDataID>
        internal class OperationItemView : MetaObjectItemView
        {
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.Operation Operation;
            /// <MetaDataID>{726BBFF4-03C4-4C64-854E-BEB5D16373EE}</MetaDataID>
            public OperationItemView(OOAdvantech.MetaDataRepository.Operation operation, System.Windows.Forms.TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(operation, treeView, parenNodeColection)
            {

                treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
                ImageIndex = 2;
                SelectedImageIndex = 2;
                Operation = operation;
                if (operation.IsStatic)
                {
                    ImageIndex = 9;
                    SelectedImageIndex = 9;
                }
                try
                {


                    OOAdvantech.MetaDataRepository.Classifier classifier = operation.ReturnType;
                    if (classifier == null)
                        return;
                    int subItems = 0;
                    foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                    {
                        if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                            continue;
                        subItems++;

                    }
                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in classifier.GetAssociateRoles(true))
                    {
                        if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                            continue;
                        subItems++;
                    }
                    foreach (OOAdvantech.MetaDataRepository.Operation inOperation in classifier.GetOperations(true))
                    {
                        if (inOperation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                            continue;
                        subItems++;
                    }

                    if (operation.Parameters.Count > 0)
                        subItems++;

                    if (subItems > 0)
                    {
                        Nodes.Add("Loadding...");
                        if (AutoExpand)
                            Expand();
                    }
                }
                catch (System.Exception error)
                {

                }

            }

            /// <MetaDataID>{3A604097-9932-4136-8BE0-4092B011FB10}</MetaDataID>
            void treeView_AfterCheck(object sender, TreeViewEventArgs e)
            {
                if (Checked && (Parent as MetaObjectItemView) != null)
                    (Parent as MetaObjectItemView).IsChecked = true;

            }
            /// <MetaDataID>{93BCA018-E624-4F73-A5A2-1765789066DC}</MetaDataID>
            public override void Expand()
            {
                if (Loaded)
                    return;
                Loaded = true;
                Nodes.Clear();
                if (Operation.Parameters.Count > 0)
                {
                    TreeNode treeNode = Nodes.Add("Parameters");
                    treeNode.ImageIndex = 7;
                    treeNode.SelectedImageIndex = 7;
                    foreach (OOAdvantech.MetaDataRepository.Parameter parameter in Operation.Parameters)
                        new ParameterItemView(parameter, TreeView, treeNode.Nodes);

                }
                OOAdvantech.MetaDataRepository.Classifier classifier = Operation.ReturnType;
                foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (from _attribute in classifier.GetAttributes(false) orderby _attribute.Name select _attribute).ToArray())
                {
                    if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    new AttributeItemView(attribute, TreeView, Nodes);
                }
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in (from _associationEnd in classifier.GetAssociateRoles(false) orderby _associationEnd.Name select _associationEnd))
                {
                    if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    new AssociationEndItemView(associationEnd, TreeView, Nodes);
                }
                foreach (OOAdvantech.MetaDataRepository.Operation operation in (from _operation in classifier.GetOperations(false) orderby _operation.Name select _operation))
                {
                    if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    new OperationItemView(operation, TreeView, Nodes);
                }


            }
        }



        /// <MetaDataID>{39DB07B2-BF4C-4511-9C32-A6A8E4997505}</MetaDataID>
        internal class AssociationEndItemView : MetaObjectItemView
        {
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.AssociationEnd AssociationEnd;
            /// <MetaDataID>{89476D00-21CB-4A53-BA24-7136A643A23B}</MetaDataID>
            public AssociationEndItemView(OOAdvantech.MetaDataRepository.AssociationEnd associationEnd, System.Windows.Forms.TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(associationEnd, treeView, parenNodeColection)
            {
                ImageIndex = 5;
                SelectedImageIndex = 5;
                AssociationEnd = associationEnd;


                if ((treeView.FindForm() as MetaDataNavigator).ListView != null)
                {
                    foreach (IColumn column in (treeView.FindForm() as MetaDataNavigator).ListView.Columns)
                    {
                        string columnPath = column.Path as string;
                        TreeNode treeNode = this;
                        string path = null;
                        while (treeNode.Parent != null)
                        {
                            if (treeNode is AssociationEndItemView ||
                                treeNode is AttributeItemView ||
                                treeNode is OperationItemView)
                            {
                                if (path == null)
                                    path = treeNode.Text;
                                else
                                    path = treeNode.Text + "." + path;
                            }
                            treeNode = treeNode.Parent;
                        }
                        string member = null;
                        if (path == columnPath && !string.IsNullOrEmpty(path))
                        {
                            Column = column;
                            Checked = true;
                        }
                        else if (path != null && columnPath.IndexOf(path) == 0)
                        {
                            columnPath = columnPath.Substring(path.Length + 1);
                            int nPos = columnPath.IndexOf(".");
                            if (nPos != -1)
                                member = columnPath.Substring(0, nPos);
                            else
                                member = columnPath;
                        }
                        else if (path == null)
                        {
                            int nPos = columnPath.IndexOf(".");
                            if (nPos != -1)
                                member = columnPath.Substring(0, nPos);
                            else
                                member = columnPath;


                        }

                        if (!string.IsNullOrEmpty(member))
                        {
                            foreach (OOAdvantech.MetaDataRepository.Attribute attribute in AssociationEnd.Specification.GetAttributes(true))
                            {
                                if (attribute.Name == member)
                                {
                                    AutoExpand = true;
                                    break;
                                }
                            }
                            if (!AutoExpand)
                            {
                                foreach (OOAdvantech.MetaDataRepository.AssociationEnd subAssociationEnd in AssociationEnd.Specification.GetAssociateRoles(true))
                                {
                                    if (subAssociationEnd.Name == member)
                                    {
                                        AutoExpand = true;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
                OOAdvantech.MetaDataRepository.Classifier classifier = associationEnd.Specification;
                int subItems = 0;
                foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                {
                    if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    subItems++;

                }
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd inAssociationEnd in classifier.GetAssociateRoles(true))
                {
                    if (inAssociationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !inAssociationEnd.Navigable)
                        continue;
                    subItems++;
                }
                foreach (OOAdvantech.MetaDataRepository.Operation operation in classifier.GetOperations(true))
                {
                    if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    subItems++;
                }
                if (subItems > 0)
                {
                    Nodes.Add("Loadding...");
                    if (AutoExpand)
                        Expand();
                }

            }
            /// <MetaDataID>{4FDA6709-CB16-4369-AE7F-2340886722CB}</MetaDataID>
            public override void Expand()
            {
                if (Loaded)
                    return;
                Loaded = true;
                Nodes.Clear();
                OOAdvantech.MetaDataRepository.Classifier classifier = AssociationEnd.Specification;
                if (AssociationEnd.Getter != null || AssociationEnd.Setter != null)
                {
                    TreeNode treeNode = Nodes.Add("Accessors");
                    treeNode.ImageIndex = 7;
                    treeNode.SelectedImageIndex = 7;
                    if (AssociationEnd.Getter != null)
                        new OperationItemView(AssociationEnd.Getter, TreeView, treeNode.Nodes);
                    if (AssociationEnd.Setter != null)
                        new OperationItemView(AssociationEnd.Setter, TreeView, treeNode.Nodes);
                }


                foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (from _attribute in classifier.GetAttributes(true) orderby _attribute.Name select _attribute).ToArray())
                {
                    if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    new AttributeItemView(attribute, TreeView, Nodes);
                }
                foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in (from _associationEnd in classifier.GetAssociateRoles(true) orderby _associationEnd.Name select _associationEnd))
                {
                    if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !associationEnd.Navigable)
                        continue;
                    new AssociationEndItemView(associationEnd, TreeView, Nodes);
                }
                foreach (OOAdvantech.MetaDataRepository.Operation operation in (from _operation in classifier.GetOperations(true) orderby _operation.Name select _operation))
                {
                    if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        continue;
                    try
                    {
                        new OperationItemView(operation, TreeView, Nodes);
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }
                }


            }
        }

        /// <MetaDataID>{32778E36-C227-4F6D-AA83-4775A6F51434}</MetaDataID>
        internal class ClassifierItemView : MetaObjectItemView
        {
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.Classifier Classifier;
            /// <MetaDataID>{46BC58FE-3E2B-42CE-AD74-9DD5B8C8F13D}</MetaDataID>
            public ClassifierItemView(OOAdvantech.MetaDataRepository.Classifier classifier, System.Windows.Forms.TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(classifier, treeView, parenNodeColection)
            {
                Classifier = classifier;

                try
                {
                    #region if ((treeView.FindForm() as MetaDataNavigator).ListView != null)
                    if ((treeView.FindForm() as MetaDataNavigator).ListView != null)
                    {
                        foreach (IColumn column in (treeView.FindForm() as MetaDataNavigator).ListView.Columns)
                        {
                            string columnPath = column.Path as string;
                            TreeNode treeNode = this;
                            string path = null;
                            while (treeNode.Parent != null)
                            {
                                if (treeNode is AssociationEndItemView ||
                                    treeNode is AttributeItemView ||
                                    treeNode is OperationItemView)
                                {
                                    if (path == null)
                                        path = treeNode.Text;
                                    else
                                        path = treeNode.Text + "." + path;
                                }
                                treeNode = treeNode.Parent;
                            }
                            string member = null;
                            if (path == columnPath && !string.IsNullOrEmpty(path))
                            {
                                Column = column;
                                Checked = true;
                            }
                            else if (path != null && columnPath.IndexOf(path) == 0)
                            {
                                columnPath = columnPath.Substring(path.Length);
                                int nPos = columnPath.IndexOf(".");
                                if (nPos != -1)
                                    member = columnPath.Substring(0, nPos);
                            }
                            else if (path == null)
                            {
                                int nPos = columnPath.IndexOf(".");
                                if (nPos != -1)
                                    member = columnPath.Substring(0, nPos);
                                else
                                    member = columnPath;


                            }

                            if (!string.IsNullOrEmpty(member))
                            {
                                foreach (OOAdvantech.MetaDataRepository.Attribute attribute in Classifier.GetAttributes(true))
                                {
                                    if (attribute.Name == member)
                                    {
                                        AutoExpand = true;
                                        break;
                                    }
                                }
                                if (!AutoExpand)
                                {
                                    foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Classifier.GetAssociateRoles(true))
                                    {
                                        if (associationEnd.Name == member)
                                        {
                                            AutoExpand = true;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    #endregion

                    #region All other ifs
                    if (classifier is OOAdvantech.MetaDataRepository.Interface)
                    {
                        ImageIndex = 6;
                        SelectedImageIndex = 6;
                    }
                    if (classifier is OOAdvantech.MetaDataRepository.Class)
                    {
                        ImageIndex = 1;
                        SelectedImageIndex = 1;
                    }

                    if ((treeView.FindForm() as MetaDataNavigator).Type == MetaDataNavigator.NavigatorType.InstanceMember)
                    {

                        int subItems = 0;
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (from _attribute in  classifier.GetAttributes(false) orderby _attribute.Name select _attribute).ToArray())
                        {
                            if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                                continue;
                            subItems++;

                        }
                        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in (from _associationEnd in classifier.GetAssociateRoles(false) orderby _associationEnd.Name select _associationEnd))
                        {
                            if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !associationEnd.Navigable)
                                continue;
                            subItems++;
                        }
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in (from _operation in  classifier.GetOperations(false) orderby _operation.Name select _operation))
                        {
                            if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                                continue;
                            subItems++;
                        }
                        subItems += (int)classifier.GetGeneralClasifiers().Count;
                        if (classifier is OOAdvantech.MetaDataRepository.Class)
                            subItems += (int)(classifier as OOAdvantech.MetaDataRepository.Class).GetInterfaces().Count;



                        if (subItems > 0)
                        {
                            Nodes.Add("Loadding...");
                            if (AutoExpand)
                                Expand();
                        }
                    }
                    if ((treeView.FindForm() as MetaDataNavigator).Type == MetaDataNavigator.NavigatorType.StaticMember)
                    {

                        int subItems = 0;
                        //foreach (OOAdvantech.MetaDataRepository.Attribute attribute in classifier.GetAttributes(true))
                        //{

                        //    if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                        //        continue;
                        //    subItems++;

                        //}
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in classifier.GetOperations(true))
                        {
                            if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !operation.IsStatic)
                                continue;
                            subItems++;
                        }
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (from _attribute in  classifier.GetAttributes(false) orderby _attribute.Name select _attribute).ToArray())
                        {
                            if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !attribute.IsStatic)
                                continue;
                            subItems++;
                        }

                        if (subItems > 0)
                        {
                            Nodes.Add("Loadding...");
                            if (AutoExpand)
                                Expand();
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ClassifierItemView exception");
                    throw;
                }
            }

            /// <MetaDataID>{18C85B77-FED5-4382-A7DA-DE35D128370A}</MetaDataID>
            public override void Expand()
            {
                if (Loaded)
                    return;
                Loaded = true;
                Nodes.Clear();
                try
                {
                    if ((TreeView.FindForm() as MetaDataNavigator).Type == MetaDataNavigator.NavigatorType.StaticMember)
                    {
                        System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject> metaObjects = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>();
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in Classifier.GetOperations(true))
                        {
                            if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !operation.IsStatic)
                                continue;
                            metaObjects.Add(operation);
                        }
                        metaObjects.Sort(new MetaObjectComparer());
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in metaObjects)
                            new OperationItemView(operation, TreeView, Nodes);
                        metaObjects.Clear();
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in (from _attribute in  Classifier.GetAttributes(false) orderby _attribute.Name select _attribute).ToArray())
                        {
                            if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !attribute.IsStatic)
                                continue;
                            metaObjects.Add(attribute);
                        }
                        metaObjects.Sort(new MetaObjectComparer());
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in metaObjects)
                            new AttributeItemView(attribute, TreeView, Nodes);


                    }
                    else
                    {
                        if (Classifier is OOAdvantech.MetaDataRepository.Class)
                        {
                            foreach (OOAdvantech.MetaDataRepository.Classifier baseClassifier in (Classifier as OOAdvantech.MetaDataRepository.Class).GetInterfaces())
                                new ClassifierItemView(baseClassifier, TreeView, Nodes);

                        }
                        foreach (OOAdvantech.MetaDataRepository.Classifier baseClassifier in Classifier.GetGeneralClasifiers())
                        {
                           //if (baseClassifier is OOAdvantech.MetaDataRepository.Class)
                            {
                                try
                                {
                                    new ClassifierItemView(baseClassifier, TreeView, Nodes);
                                }
                                catch (System.Exception error)
                                {
                                    MessageBox.Show("ClassifierItemView expand1 ex");
                                }
                            }
                        }
                        System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject> metaObjects = new System.Collections.Generic.List<OOAdvantech.MetaDataRepository.MetaObject>();
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in Classifier.GetAttributes(false))
                        {
                            if (attribute.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                                continue;
                            metaObjects.Add(attribute);
                            //new AttributeItemView(attribute, TreeView, Nodes);
                        }
                        metaObjects.Sort(new MetaObjectComparer());
                        foreach (OOAdvantech.MetaDataRepository.Attribute attribute in metaObjects)
                            new AttributeItemView(attribute, TreeView, Nodes);

                        metaObjects.Clear();
                        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in Classifier.GetAssociateRoles(false))
                        {
                            if (associationEnd.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic || !associationEnd.Navigable)
                                continue;
                            metaObjects.Add(associationEnd);
                            //new AssociationEndItemView(associationEnd, TreeView, Nodes);
                        }
                        metaObjects.Sort(new MetaObjectComparer());
                        foreach (OOAdvantech.MetaDataRepository.AssociationEnd associationEnd in metaObjects)
                            new AssociationEndItemView(associationEnd, TreeView, Nodes);


                        metaObjects.Clear();
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in Classifier.GetOperations(false))
                        {
                            if (operation.Visibility != OOAdvantech.MetaDataRepository.VisibilityKind.AccessPublic)
                                continue;
                            metaObjects.Add(operation);
                            //new OperationItemView(operation, TreeView, Nodes);
                        }
                        metaObjects.Sort(new MetaObjectComparer());
                        foreach (OOAdvantech.MetaDataRepository.Operation operation in metaObjects)
                            new OperationItemView(operation, TreeView, Nodes);

                    }
                }
                catch (System.Exception error)
                {
                    MessageBox.Show("ClassifierItemView expand2 ex");
                    throw;
                }

            }
        }
        class MetaObjectComparer : System.Collections.Generic.IComparer<OOAdvantech.MetaDataRepository.MetaObject>
        {

            #region IComparer<MetaObject> Members

            public int Compare(OOAdvantech.MetaDataRepository.MetaObject x, OOAdvantech.MetaDataRepository.MetaObject y)
            {
                return x.Name.CompareTo(y.Name);
            }

            #endregion
        }

        /// <MetaDataID>{6D92844C-F295-4D1D-A2A7-7848BF7725CE}</MetaDataID>
        internal class NamespacetItemView : MetaObjectItemView
        {
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.Namespace Namespace;
            /// <MetaDataID>{278BE5D9-90EE-4941-8BD9-14018B0A0188}</MetaDataID>
            public NamespacetItemView(OOAdvantech.MetaDataRepository.Namespace _namespace, TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(_namespace, treeView, parenNodeColection)
            {
                Namespace = _namespace;
                Nodes.Add("Loadding...");
                if (AutoExpand)
                    Expand();
                Text = _namespace.FullName;
            }
            /// <MetaDataID>{6DA2CEA0-ED90-4055-84C7-DB67760149C8}</MetaDataID>
            public override void Expand()
            {
                if (Loaded)
                    return;
                Loaded = true;
                OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.MetaObject> ownedElements = Namespace.OwnedElements;
                Nodes.Clear();
                foreach (OOAdvantech.MetaDataRepository.MetaObject metaObject in ownedElements)
                {
                    if (metaObject.ImplementationUnit != ((this.Parent as ComponentItemView).MetaObject as OOAdvantech.MetaDataRepository.Component))
                        continue;

                    if (metaObject is OOAdvantech.MetaDataRepository.Classifier)
                    {
                        try
                        {
                            new ClassifierItemView(metaObject as OOAdvantech.MetaDataRepository.Classifier, TreeView, Nodes);
                        }
                        catch (System.Exception error)
                        {
                            throw;
                        }



                    }
                }
            }
        }
        /// <MetaDataID>{546D3F1A-2451-48DC-B1A6-9E74E68D78A6}</MetaDataID>
        internal class ComponentItemView : MetaObjectItemView
        {
            bool Loaded = false;
            OOAdvantech.MetaDataRepository.Component Component;
            /// <MetaDataID>{12746A13-7E4F-4B5C-A5F0-D4BB331995BA}</MetaDataID>
            public ComponentItemView(OOAdvantech.MetaDataRepository.Component component, System.Windows.Forms.TreeView treeView, TreeNodeCollection parenNodeColection)
                : base(component, treeView, parenNodeColection)
            {
                Component = component;
                Nodes.Add("Loadding...");
                if (AutoExpand)
                    Expand();

                SelectedImageIndex = 14;
                ImageIndex = 14;



            }
            System.Windows.Forms.TreeNode ReferenceNode = null;
            /// <MetaDataID>{AFABAA68-955B-40F7-8D37-03B7291916C2}</MetaDataID>
            public override void Expand()
            {
                if (Loaded)
                    return;

                Loaded = true;
                OOAdvantech.Collections.Generic.Set<OOAdvantech.MetaDataRepository.Dependency> references = Component.ClientDependencies;
                Nodes.Clear();
                if (references.Count > 0)
                {
                    ReferenceNode = Nodes.Add("Reference");
                    ReferenceNode.SelectedImageIndex = 12;
                    ReferenceNode.ImageIndex = 12;
                    ReferenceNode.TreeView.AfterExpand += new TreeViewEventHandler(TreeView_AfterExpand);
                    ReferenceNode.TreeView.AfterCollapse += new TreeViewEventHandler(TreeView_AfterCollapse);

                    foreach (OOAdvantech.MetaDataRepository.Dependency dependency in references)
                    {
                        new ComponentItemView(dependency.Supplier as OOAdvantech.MetaDataRepository.Component, TreeView, ReferenceNode.Nodes);
                    }
                }


                System.Collections.ArrayList namespaces = new System.Collections.ArrayList();
                foreach (OOAdvantech.MetaDataRepository.MetaObject residentMetaObject in Component.Residents)
                {
                    if (residentMetaObject is OOAdvantech.MetaDataRepository.Classifier &&
                        residentMetaObject.Namespace != null &&
                        !namespaces.Contains(residentMetaObject.Namespace))
                    {
                        namespaces.Add(residentMetaObject.Namespace);

                    }
                }
                foreach (OOAdvantech.MetaDataRepository.Namespace _namespace in namespaces)
                {
                    NamespacetItemView ns = new NamespacetItemView(_namespace, TreeView, Nodes);
                    //ns.Text="nikos";
                    //   Nodes.Add(ns);

                }

            }

            void TreeView_AfterCollapse(object sender, TreeViewEventArgs e)
            {
                if (e.Node == ReferenceNode)
                {
                    ReferenceNode.SelectedImageIndex = 12;
                    ReferenceNode.ImageIndex = 12;
                }

            }

            void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
            {
                if (e.Node == ReferenceNode)
                {
                    ReferenceNode.SelectedImageIndex = 13;
                    ReferenceNode.ImageIndex = 13;
                }
            }
        }
        /// <MetaDataID>{db3bacdd-9c89-4a32-9c98-75c7cb4799dc}</MetaDataID>
        protected OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver ObjectMemberViewControl;
        /// <MetaDataID>{64f737f7-e624-4d10-87c4-c0024cd87a54}</MetaDataID>
        protected OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection ViewControlObject;
        /// <MetaDataID>{dfff8a7c-d704-41f2-980c-8effb2f04f8c}</MetaDataID>
        OOAdvantech.MetaDataRepository.MetaObject MetaObject;
        /// <MetaDataID>{08b6e944-09a7-4bbd-843a-708eb9c08aea}</MetaDataID>
        string PropertyDescriptor;
        /// <MetaDataID>{3A35E11F-814D-4228-8AA5-B7DF41FA5CAD}</MetaDataID>
        public MetaDataNavigator(OOAdvantech.MetaDataRepository.MetaObject metaObject, OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver objectMemberViewControl, NavigatorType navigatorType, string propertyDescriptor)
        {
            Type = navigatorType;
            ObjectMemberViewControl = objectMemberViewControl;
            MetaObject = metaObject;
            PropertyDescriptor = propertyDescriptor;
            InitializeComponent();
        }
        /// <MetaDataID>{c34efbe5-4b46-4826-ab97-861bf55f7d1a}</MetaDataID>
        public MetaDataNavigator(OOAdvantech.MetaDataRepository.MetaObject metaObject, OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection viewControlObject, NavigatorType navigatorType, string propertyDescriptor)
        {
            Type = navigatorType;
            ViewControlObject = viewControlObject;
            MetaObject = metaObject;
            PropertyDescriptor = propertyDescriptor;
            InitializeComponent();
        }


        /// <MetaDataID>{8c3d98ae-8129-43a7-b1fa-9848708696d6}</MetaDataID>
        protected ListConnection ListView;
        /// <MetaDataID>{6765DE83-6D79-4B86-83F6-0BDD4C2822AF}</MetaDataID>
        public MetaDataNavigator(OOAdvantech.MetaDataRepository.MetaObject metaObject, OOAdvantech.UserInterface.Runtime.IConnectableControl objectMemberViewControl, ListConnection listView, NavigatorType navigatorType, string propertyDescriptor)
        {
            this.Type = navigatorType;
            this.ListView = listView;


            ObjectMemberViewControl = objectMemberViewControl;
            MetaObject = metaObject;
            PropertyDescriptor = propertyDescriptor;
            InitializeComponent();
        }

        float? _TextScaleFactor;
        public float TextScaleFactor
        {
            get
            {
                if (!_TextScaleFactor.HasValue)
                    _TextScaleFactor = this.CreateGraphics().DpiX / 96;
                return _TextScaleFactor.Value;
            }
        }

        /// <MetaDataID>{c8130422-324d-46d9-be4d-181c7292c89a}</MetaDataID>
        TreeNode NoneTreeNode;
        /// <MetaDataID>{45f23b73-e7e8-43c5-810d-e115b214a652}</MetaDataID>
        TreeNode ViewControlObjectTreeNode;
        /// <MetaDataID>{5E997584-F8FA-4D5F-9E63-A0049FF91F93}</MetaDataID>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            treeView.ItemHeight = (int)(treeView.ItemHeight * TextScaleFactor);


            if (Parent != null)
                return;
            try
            {
                NoneTreeNode = treeView.Nodes.Add("None");
                NoneTreeNode.SelectedImageIndex = 10;
                NoneTreeNode.ImageIndex = 10;

                if (MetaObject == null)
                {
                    Close();
                }
                else
                {
                    treeView.ImageList = internalImages;
                    if (MetaObject is OOAdvantech.MetaDataRepository.Component)
                    {

                        new ComponentItemView(MetaObject as OOAdvantech.MetaDataRepository.Component, treeView, treeView.Nodes);
                    }
                    else
                    {

                        new ClassifierItemView(MetaObject as OOAdvantech.MetaDataRepository.Classifier, treeView, treeView.Nodes);
                    }

                    ViewControlObjectTreeNode = treeView.Nodes.Add("ViewControlObject");
                    ViewControlObjectTreeNode.SelectedImageIndex = 11;
                    ViewControlObjectTreeNode.ImageIndex = 11;
                }

                if (this.Width + this.Location.X > Screen.PrimaryScreen.Bounds.Width)
                    this.Location = new Point(this.Location.X - this.Width, this.Location.Y);

                if (this.Height + this.Location.Y > Screen.PrimaryScreen.Bounds.Height)
                    this.Location = new Point(this.Location.X, this.Location.Y - this.Height);
            }

            catch (System.Exception error)
            {
                throw;
            }

        }
        /// <MetaDataID>{0ce88cb9-38a9-42be-bb38-9cdd99f7cc96}</MetaDataID>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }
        /// <MetaDataID>{be5777aa-1faa-43cb-8092-1e6f094ed876}</MetaDataID>
        public OOAdvantech.MetaDataRepository.MetaObject SelectedMetaObject
        {
            get
            {
                if (treeView.SelectedNode is MetaObjectItemView)
                    return (treeView.SelectedNode as MetaObjectItemView).MetaObject;
                else
                    return null;

            }

        }
        /// <MetaDataID>{D5EC135B-319C-4EFC-9A15-0DA612980305}</MetaDataID>
        public string SelectedMember;

        /// <MetaDataID>{d8d4e35e-91c8-4ec9-817c-0b649b03a33e}</MetaDataID>
        System.Windows.Forms.TreeNode ClickedNode;
        /// <MetaDataID>{D75307C3-8AEE-4AFF-BEC2-D21463FC6138}</MetaDataID>
        private void treeView_DoubleClick(object sender, System.EventArgs e)
        {

            System.Windows.Forms.TreeNode SelectedNode = treeView.SelectedNode;
            if (SelectedNode == null)
                return;


            if (SelectedNode != ClickedNode)
                return;


            if (SelectedNode == NoneTreeNode)
            {
                SelectedMember = SelectedNode.FullPath.Replace("\\", ".");
                DialogResult = DialogResult.OK;
                TreeNode treeNode = treeView.GetNodeAt(treeView.PointToClient(MousePosition));

                Close();
            }
            if (SelectedNode == ViewControlObjectTreeNode)
            {
                SelectedMember = "(ViewControlObject)";
                DialogResult = DialogResult.OK;
                Close();
            }
            if (Type == NavigatorType.Type && (SelectedNode as MetaObjectItemView) != null && (SelectedNode as MetaObjectItemView).MetaObject is OOAdvantech.MetaDataRepository.Classifier)
            {
                try
                {
                    if (ViewControlObject != null && ViewControlObject.CanItAccept((SelectedNode as MetaObjectItemView).MetaObject, PropertyDescriptor))
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }

                }
                catch (System.Exception error)
                {
                    System.Windows.Forms.MessageBox.Show(error.Message);
                }
            }

            if ((ObjectMemberViewControl) != null &&
                (SelectedNode as MetaObjectItemView) != null &&
                ObjectMemberViewControl.CanItAccept((SelectedNode as MetaObjectItemView).MetaObject, PropertyDescriptor))
            {
                if (Type == NavigatorType.InstanceMember)
                {
                    SelectedMember = null;
                    while (SelectedNode.Parent != null)
                    {
                        if (SelectedNode is AssociationEndItemView ||
                            SelectedNode is AttributeItemView ||
                            SelectedNode is OperationItemView)
                        {
                            if (SelectedMember == null)
                                SelectedMember = SelectedNode.Text;
                            else
                                SelectedMember = SelectedNode.Text + "." + SelectedMember;
                        }
                        SelectedNode = SelectedNode.Parent;
                    }
                }
                else if (Type == NavigatorType.Type)
                {
                    while (SelectedNode.Parent != null)
                    {
                        if (SelectedNode is NamespacetItemView ||
                            SelectedNode is ClassifierItemView ||
                            SelectedNode is OperationItemView)
                        {
                            if (SelectedMember == null)
                                SelectedMember = SelectedNode.Text;
                            else
                                SelectedMember = SelectedNode.Text + "." + SelectedMember;
                        }
                        SelectedNode = SelectedNode.Parent;
                    }

                }
                else if (Type == NavigatorType.StaticMember)
                {
                    while (SelectedNode.Parent != null)
                    {
                        if (SelectedNode is NamespacetItemView ||
                           SelectedNode is ClassifierItemView ||
                           SelectedNode is OperationItemView)
                        {
                            if (SelectedMember == null)
                                SelectedMember = SelectedNode.Text;
                            else
                                SelectedMember = SelectedNode.Text + "." + SelectedMember;
                        }
                        SelectedNode = SelectedNode.Parent;
                    }


                }


                else
                    SelectedMember = SelectedNode.FullPath.Replace("\\", ".");


                DialogResult = DialogResult.OK;
                Close();

            }



        }
        /// <MetaDataID>{1c8d852a-1977-4a8f-8cc3-2830b22ef061}</MetaDataID>
        protected internal TreeView treeView;

        /// <MetaDataID>{FACBCDD3-9E6C-4E45-9A31-C54388584233}</MetaDataID>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetaDataNavigator));
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeView
            // 
            resources.ApplyResources(this.treeView, "treeView");
            this.treeView.ItemHeight = 16;
            this.treeView.Name = "treeView";
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
            this.treeView.Click += new System.EventHandler(this.treeView_Click);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            // 
            // MetaDataNavigator
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.treeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MetaDataNavigator";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);

        }

        static System.Windows.Forms.ImageList _internalImages;
        /// <MetaDataID>{02732cad-14fd-4b72-a914-1a10863671cc}</MetaDataID>
        protected System.Windows.Forms.ImageList internalImages
        {
            get
            {
                if(_internalImages==null)
                {
                    _internalImages = ResourceHelper.LoadBitmapStrip(typeof(MetaDataNavigator),
                    "ConnectableControls.Members.bmp",
                    new Size(16, 16),
                    new Point(0, 0));
                }
                return _internalImages;

            }
        }

        /// <MetaDataID>{955E3968-F6FE-4F33-8912-6ACF2695BEF4}</MetaDataID>
        static MetaDataNavigator()
        {
            //try
            //{

            //    _internalImages = ResourceHelper.LoadBitmapStrip(typeof(MetaDataNavigator),
            //        "ConnectableControls.Members.bmp",
            //        new Size(16, 16),
            //        new Point(0, 0));
            //}
            //catch (System.Exception Error)
            //{
            //    //MessageBox.Show(Error.ToString());
            //}
        }

        /// <MetaDataID>{07B8946E-6533-4124-B212-3AE5FA6C3D4E}</MetaDataID>
        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node is MetaObjectItemView)
                (e.Node as MetaObjectItemView).Expand();


        }

        /// <MetaDataID>{38755E7F-3A28-453E-8918-65417203D8A9}</MetaDataID>
        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {

        }

        /// <MetaDataID>{484221ef-bbdf-48e5-90ee-4bd41fd558c3}</MetaDataID>
        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {

        }

        /// <MetaDataID>{95b77e6d-0764-40a7-abae-d6983e357abb}</MetaDataID>
        private void treeView_Click(object sender, EventArgs e)
        {
            ClickedNode = treeView.GetNodeAt(treeView.PointToClient(MousePosition));
        }
    }
}
