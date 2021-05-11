using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ConnectableControls.ListView;
//using ConnectableControls.List.Models;

namespace ConnectableControls.PropertyEditors
{

    /// <MetaDataID>{F43B5F71-9469-49DC-A949-F9E009ACDDF0}</MetaDataID>
    internal partial class ListMetaDataEditor : MetaDataNavigator
    {

        /// <MetaDataID>{A34AE9DB-6D8F-4F54-B007-4CBCA850D887}</MetaDataID>
        class ColumnItemView : System.Windows.Forms.ListViewItem
        {
            public readonly MetaObjectItemView MetaObjectItemView;
            /// <MetaDataID>{B6D3F432-93AD-4ECD-9AA9-0F5F38680947}</MetaDataID>
            public ColumnItemView(MetaObjectItemView metaObjectItemView)
                :base(metaObjectItemView.FullPath.Replace(@"\", ".").Replace(metaObjectItemView.TreeView.Nodes[0].Text + ".", ""))
            {
                TreeNode treeNode = metaObjectItemView;
                MetaObjectItemView = metaObjectItemView;
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
                MetaObjectItemView.Column.Path = path;
                SubItems[0].Text = path;
            }

        }


        /// <MetaDataID>{f52cca35-757d-4207-a9e2-60454fab9a62}</MetaDataID>
        IColumn SelectedColumn
        {
            get
            {
                if (ColumnsList.SelectedItems.Count > 0)
                    return (ColumnsList.SelectedItems[0] as ColumnItemView).MetaObjectItemView.Column;
                else
                    return null;
            }
            set
            {
                if (ColumnsList.SelectedItems.Count > 0)
                {
                    (ColumnsList.SelectedItems[0] as ColumnItemView).MetaObjectItemView.Column = value;
                    propertyGrid.SelectedObject = value;
                }
                else
                    propertyGrid.SelectedObject = null;


            }

        }

        /// <MetaDataID>{9c0d9d99-773a-4408-a4bc-b45381e85f74}</MetaDataID>
        ConnectableControls.Menus.MenuCommand ColumnsListMenu = new ConnectableControls.Menus.MenuCommand("ColumnsListMenu");
        /// <MetaDataID>{05A36EB7-20EB-45B5-BFE0-C1F3696F916C}</MetaDataID>
        public ListMetaDataEditor()
        {
            InitializeComponent();
            Controls.Remove(treeView);
            tableLayoutPanel.Controls.Add(treeView, 0, 0);
            treeView.Dock = DockStyle.Fill;
            List<string> columnTypeNames = ListView.GetColumnTypesNames();
            foreach (string ColumnTypeName in columnTypeNames)
            {
                ConnectableControls.Menus.MenuCommand menuCommand = new ConnectableControls.Menus.MenuCommand("ColumnTypeName ");
                menuCommand.Click += new EventHandler(MenuCommandClick);
                ColumnsListMenu.MenuCommands.Add(menuCommand);
            }
            propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);
 
        }

        /// <MetaDataID>{b8a1cc0a-77b4-4439-9c39-326629f5b23f}</MetaDataID>
        void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.PropertyDescriptor.Name == "Name")
            {
                if (propertyGrid.SelectedObject is IColumn)
                    (propertyGrid.SelectedObject as IColumn).Name = (propertyGrid.SelectedObject as IColumn).Name;
            }
            
        }

        /// <MetaDataID>{A90085A6-BB13-4ADF-89CE-FA6D42C99768}</MetaDataID>
        void MenuCommandClick(object sender, EventArgs e)
        {
            //CheckBoxColumn
            string columnType=(sender as ConnectableControls.Menus.MenuCommand).Text;
            SelectedColumn= ListView.ChangeColumnType(SelectedColumn, columnType);

        }

     
        /// <MetaDataID>{EC1B52CB-C5DC-4F98-939D-95B4A135EB79}</MetaDataID>
        public ListMetaDataEditor(OOAdvantech.MetaDataRepository.MetaObject metaObject, OOAdvantech.UserInterface.Runtime.IObjectMemberViewControl objectMemberViewControl, ListConnection listView, string propertyDescriptor)
            : base(metaObject, objectMemberViewControl, listView, NavigatorType.InstanceMember, propertyDescriptor)
        {
            InitializeComponent();
            Controls.Remove(treeView);
            tableLayoutPanel.Controls.Add(treeView, 0, 0);
            treeView.Dock = DockStyle.Fill;
            this.Load += new EventHandler(ListMetaDataEditor_Load);
            this.treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
            propertyGrid.SelectedObject = SelectedColumn;
            //if (SelectedColumn != null)
            //    SelectedColumn.PropertyChanged += new List.Events.ColumnEventHandler(m_PTest_PropertyChanged);
            foreach (string columnType in listView.HostingListView.GetColumnTypesNames())
            {
                ConnectableControls.Menus.MenuCommand menuCommand = new ConnectableControls.Menus.MenuCommand(columnType);
                menuCommand.Click += new EventHandler(MenuCommandClick);
                ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("ButtonColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("CheckBoxColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("ColorColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("ComboBoxColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("DateTimeColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("ImageColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("NumberColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("ProgressBarColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
                //menuCommand = new ConnectableControls.Menus.MenuCommand("SearchBoxColumn");
                //menuCommand.Click += new EventHandler(MenuCommandClick);
                //ColumnsListMenu.MenuCommands.Add(menuCommand);
            }
            propertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(propertyGrid_PropertyValueChanged);

        }

        //void m_PTest_PropertyChanged(object sender, List.Events.ColumnEventArgs e)
        //{
        //    if (e.EventType == List.Events.ColumnEventType.Type)
        //    {
        //        propertyGrid.SelectedObject = null;

        //        if (SelectedColumn != null)
        //            SelectedColumn.PropertyChanged -= new List.Events.ColumnEventHandler(m_PTest_PropertyChanged);

        //        SelectedColumn = Column.CreateColumn(e.Column.Type);

        //        if (SelectedColumn != null)
        //            SelectedColumn.PropertyChanged += new List.Events.ColumnEventHandler(m_PTest_PropertyChanged);
        //        propertyGrid.SelectedObject = SelectedColumn;

        //    }
        //}

        /// <MetaDataID>{285d3d64-58dc-44c8-86d7-25d86f407018}</MetaDataID>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            foreach (IColumn column in new System.Collections.ArrayList(ListView.Columns))
            {
                bool exist = false;
                foreach (ColumnItemView columnItemView in ColumnsList.Items)
                {
                    if (columnItemView.MetaObjectItemView.Column == column)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                {

                    if (System.Windows.Forms.MessageBox.Show("The path of column '" + (column).Name + "' is invalid.\n Do you want delete column?", "ListView", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        ListView.RemoveColumn(column);
                }
            }
        }
        /// <MetaDataID>{4B74D61A-F124-4BEF-BEAC-2A5C6F55E21D}</MetaDataID>
        void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {

            ColumnsList.Items.Clear();
            // listView1.View = View.Details;
            //listView1.
            //listView1.Columns.Add("Column Name");


            System.Collections.Generic.List<MetaDataNavigator.MetaObjectItemView> columnsNode = new List<MetaObjectItemView>();
            if (treeView.Nodes.Count > 0)
            {
                GetColumnNodes(treeView.Nodes[1], columnsNode);
                foreach (MetaDataNavigator.MetaObjectItemView columnNode in columnsNode)
                {
                    if (columnNode.Column == null)
                        columnNode.Column = ListView.AddColumn(columnNode.FullPath.Replace(".", "\\"));
                    if (columnNode.Column.Owner == null)
                        ListView.AddColumn(columnNode.Column);

                }


                columnsNode.Sort(new columnSort());
                int i = 0;

                foreach (MetaDataNavigator.MetaObjectItemView columnNode in columnsNode)
                {
                    columnNode.EnsureVisible();

                    columnNode.Column.Owner = (ObjectMemberViewControl as ListConnection).HostingListView;

                    bool exist = false;
                    foreach (ColumnItemView columnItemView in ColumnsList.Items)
                    {
                        if (columnItemView.MetaObjectItemView.Column == columnNode.Column)
                        {
                            exist = true;
                            break;
                        }
                    }
                    if(!exist)
                        ColumnsList.Items.Add(new ColumnItemView(columnNode));
                    //if (columnNode.Column != null)
                    //    columnNode.Column.Order = i++;
                }

            }
            ColumnsList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);


        }

        /// <MetaDataID>{1D7E96CD-EA1F-4279-8E98-F4FCEE34E664}</MetaDataID>
        void ListMetaDataEditor_Load(object sender, EventArgs e)
        {

            ColumnsList.View = View.Details;
            ColumnsList.Columns.Add("Column Path");
            ColumnsList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        /// <MetaDataID>{66C033AF-2431-4DC1-B0CA-77DD2443FC3B}</MetaDataID>
        void GetColumnNodes(TreeNode rootNode, System.Collections.Generic.List<MetaDataNavigator.MetaObjectItemView> columnsNode)
        {
            foreach (TreeNode subNode in rootNode.Nodes)
            {
                if (subNode is MetaDataNavigator.MetaObjectItemView && subNode.Checked)
                    columnsNode.Add(subNode as MetaDataNavigator.MetaObjectItemView);
                else
                    if (subNode is MetaDataNavigator.MetaObjectItemView && !subNode.Checked && (subNode as MetaDataNavigator.MetaObjectItemView).Column != null)
                        ListView.RemoveColumn((subNode as MetaDataNavigator.MetaObjectItemView).Column);
                GetColumnNodes(subNode, columnsNode);
            }
        }

        /// <MetaDataID>{37BAE59A-0DEF-444E-A9B6-0BC8AF577BC0}</MetaDataID>
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        /// <MetaDataID>{F8712E5E-6B68-4D68-BC3C-F9F7F5B07406}</MetaDataID>
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                foreach (Menus.MenuCommand menuCommand in ColumnsListMenu.MenuCommands)
                    menuCommand.Checked = false;
                if (ColumnsListMenu.MenuCommands.Count > 0)
                {

                    ConnectableControls.Menus.PopupMenu popupMenu = new ConnectableControls.Menus.PopupMenu();
                    int returnDir = 0;
                    string selectedColumnTypeName = ListView.GetColumnTypeName(SelectedColumn);
                    foreach (Menus.MenuCommand menuCommand in ColumnsListMenu.MenuCommands)
                        if (menuCommand.Text == selectedColumnTypeName)
                        {
                            menuCommand.Checked = true;
                            break;
                        }


                    popupMenu.TrackPopup(
                        Control.MousePosition,
                        Control.MousePosition,
                        ConnectableControls.Menus.Common.Direction.Horizontal,
                        this.ColumnsListMenu,
                        0,
                        ConnectableControls.Menus.GapPosition.None, false, null, false, ref returnDir);
                }

            }





        }

        /// <MetaDataID>{B5BAC87B-7C76-4494-9E45-7682E02A53DB}</MetaDataID>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnsList.SelectedItems.Count > 0)
            {
                
                propertyGrid.SelectedObject = null;
                propertyGrid.SelectedObject = (ColumnsList.SelectedItems[0] as ColumnItemView).MetaObjectItemView.Column;
            }
            else
                propertyGrid.SelectedObject = null;



        }

        /// <MetaDataID>{B855C278-7A10-4D93-AA4C-EF488CD6DEEC}</MetaDataID>
        private void UpBtn_Click(object sender, EventArgs e)
        {
            if (ColumnsList.SelectedItems.Count > 0)
            {
                ColumnItemView columnItemView = ColumnsList.SelectedItems[0] as ColumnItemView;
                if (columnItemView.Index == 0)
                    return;
                int index = columnItemView.Index;
                columnItemView.Remove();
                ColumnsList.Items.Insert(index - 1, columnItemView).Selected = true;
                ColumnsList.Focus();
                int i = 0;
                ListView.Columns.Clear();
                foreach (ColumnItemView currentColumnItemView in ColumnsList.Items)
                {
                    currentColumnItemView.MetaObjectItemView.Column.Order = i++;
                    ListView.Columns.Add(currentColumnItemView.MetaObjectItemView.Column);

                }

                return;
            }


        }

        /// <MetaDataID>{A09901DF-515E-4AFC-B758-80694FED1ED0}</MetaDataID>
        private void DownBtn_Click(object sender, EventArgs e)
        {
            if (ColumnsList.SelectedItems.Count > 0)
            {
                ColumnItemView columnItemView = ColumnsList.SelectedItems[0] as ColumnItemView;

                if (columnItemView.Index == ColumnsList.Items.Count - 1)
                    return;
                int index = columnItemView.Index;
                columnItemView.Remove();
                ColumnsList.Items.Insert(index + 1, columnItemView).Selected = true;
                ColumnsList.Focus();
                int i = 0;
                ListView.Columns.Clear();

                foreach (ColumnItemView currentColumnItemView in ColumnsList.Items)
                {
                    currentColumnItemView.MetaObjectItemView.Column.Order = i++;
                    ListView.Columns.Add(currentColumnItemView.MetaObjectItemView.Column);

                }
                return;
            }

        }

        /// <MetaDataID>{E3F20A9B-0F8B-406B-A075-7A86F4EC0280}</MetaDataID>
        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete && ColumnsList.SelectedItems.Count > 0)
                ColumnsList.Items.Remove(ColumnsList.SelectedItems[0]);

        }


    }

    /// <MetaDataID>{130742E8-4E5A-4CF9-8B90-D831131F0684}</MetaDataID>
    internal class columnSort : System.Collections.Generic.Comparer<MetaDataNavigator.MetaObjectItemView>
    {
        /// <MetaDataID>{FF217E35-8F11-4319-A885-B21514517210}</MetaDataID>
        public override int Compare(MetaDataNavigator.MetaObjectItemView x, MetaDataNavigator.MetaObjectItemView y)
        {
            if (x.Column == null && y.Column == null)
                return 0;
            if (x.Column == null && y.Column != null)
                return -1;
            if (x.Column != null && y.Column == null)
                return 1;
            return x.Column.Order.CompareTo(y.Column.Order);
        }
    }
}

