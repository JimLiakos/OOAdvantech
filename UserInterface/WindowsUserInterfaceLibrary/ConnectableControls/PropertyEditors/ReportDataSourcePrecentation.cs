using System;
using System.Collections.Generic;
using System.Text;
using OOAdvantech.UserInterface.ReportObjectDataSource;
using System.Drawing;
using System.ComponentModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{1d5f1eac-24a4-45d0-9c7a-970132a55561}</MetaDataID>
    public class ReportDataSourcePrecentation : OOAdvantech.UserInterface.Runtime.PresentationObject<ReportDataSource>
    {
        /// <MetaDataID>{af559ab9-f53c-4089-bf4f-3c7d43824b8d}</MetaDataID>
        public ReportDataSourcePrecentation(ReportDataSource reportDataSource)
            : base(reportDataSource)
        {

        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <exclude>Excluded</exclude>
        ReportDataNode _ReportDataSourceAsNode;
        /// <MetaDataID>{10431870-2c8c-46a1-b1c5-4cbef0bd7203}</MetaDataID>
        public ReportDataNode ReportDataSourceAsNode
        {
            get
            {
                if (_ReportDataSourceAsNode == null)
                    _ReportDataSourceAsNode = new ReportNode(RealObject, null);
                return _ReportDataSourceAsNode;
            }
        }

        /// <exclude>Excluded</exclude>
        ReportDataNode _SelectedNode;
        /// <MetaDataID>{3c992cab-26af-413a-ba3c-1c8ab9716a8b}</MetaDataID>
        public ReportDataNode SelectedNode
        {
            get
            {

                return _SelectedNode;
            }
            set
            {

                _SelectedNode = value;
                //if (ObjectChangeState != null)
                //    ObjectChangeState(this, "SelectedObject");

            }

        }





    }


    /// <MetaDataID>{f38496e6-6f7e-4c5a-a1cf-2eb46f83a30e}</MetaDataID>
    public class ReportDataNode : OOAdvantech.UserInterface.Runtime.IMetadataSelectionResolver
    {

        public OOAdvantech.MetaDataRepository.Classifier GetClassifierFor(ITypeDescriptorContext context)
        {
            if (context.PropertyDescriptor.Name == "Path")
                return ReportDataSource.Type;

            throw new NotImplementedException();
        }


        public virtual bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (propertyDescriptor == "Path" && metaObject is OOAdvantech.MetaDataRepository.Attribute)
                return true;
            else if (propertyDescriptor == "Path" && metaObject is OOAdvantech.MetaDataRepository.AssociationEnd)
                return true;
            else
                return false;


        }

        [Browsable(false)]
        public object ReportMetaDataObject
        {
            get
            {
                if (ReportDataSource != null)
                    return this.ReportDataSource;
                else
                    return this.ReportDataSourceMember;
            }
        }
        [Browsable(false)]
        public OOAdvantech.MetaDataRepository.Classifier Type
        {
            get
            {
                if (ReportDataSource != null)
                {
                    if (ReportDataSource.Type == null && ReportDataSource.MasterDataSource != null)
                        return ReportDataSource.MasterDataSource.Type;
                    return ReportDataSource.Type;
                }
                else
                    return ReportDataSourceMember.OrgType;
            }
        }

        [Browsable(false)]
        public Image Image
        {
            get
            {
                if (ReportDataSource != null)
                    return ConnectableControls.Properties.Resources.DataSource;

                if (ReportDataSourceMember != null && ReportDataSourceMember.Type != null)
                {
                    Classifier type = ReportDataSourceMember.Type;

                    Type netType = type.GetExtensionMetaObject(typeof(Type)) as Type;
                    if (netType == null)
                        return ConnectableControls.Properties.Resources.DataSource;
                    if (netType == typeof(string))
                        return ConnectableControls.Properties.Resources.TypeString;
                    if (netType == typeof(System.Int16) ||
                        netType == typeof(System.Int32) ||
                        netType == typeof(System.Int64))
                        return ConnectableControls.Properties.Resources.TypeInt;
                    if (netType == typeof(System.Single) ||
                        netType == typeof(System.Double) ||
                        netType == typeof(System.Decimal))
                        return ConnectableControls.Properties.Resources.Typedecimal;
                    if (netType == typeof(DateTime))
                        return ConnectableControls.Properties.Resources.TypeDateTime;
                    if (netType == typeof(bool))
                        return ConnectableControls.Properties.Resources.TypeBool;

                }
                return ConnectableControls.Properties.Resources.DataSource;
            }
        }
        public string Name
        {
            get
            {
                if (ReportDataSource != null)
                    return ReportDataSource.Name;
                else
                    return ReportDataSourceMember.Name;
            }
            set
            {
                if (ReportDataSource != null)
                    ReportDataSource.Name = value;
                else
                    ReportDataSourceMember.Name = value;
                if (ReportDataSourceMember != null)
                {


                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        ReportDataSourceMember.DataSource.HasCustomSetings = true;
                        ReportDataSourceMember.DataSource.EnsurePersistency();

                        stateTransition.Consistent = true;
                    }


                }
                if (Parent != null)
                    Parent.Update("SubReportDataNodes");
            }
        }


        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public virtual Object Path
        {
            get
            {
                if (ReportDataSource != null)
                    return ReportDataSource.Path;
                else
                    return ReportDataSourceMember.Path;
            }
            set
            {
                string path = null;
                if (value is string)
                    path = value as string;
                else if (value is MetaData)
                {
                    path = (value as MetaData).Path;
                    if ((value as MetaData).MetaObject == null)
                        path = null;
                    else
                    {
                        if ((value as MetaData).MetaObject is AssociationEnd &&
                            ((value as MetaData).MetaObject as AssociationEnd).CollectionClassifier != null &&
                            ReportDataSourceMember != null)
                        {

                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                ReportDataSource = ReportDataSourceMember.DataSource.MakeMemberDetailDataSource(ReportDataSourceMember, path);
                                ReportDataSourceMember.DataSource.HasCustomSetings = true;
                                ReportDataSourceMember.DataSource.EnsurePersistency();
                                stateTransition.Consistent = true;
                            }
                        }

                    }

                }

                if (ReportDataSource != null)
                    ReportDataSource.Path = path;
                else
                    ReportDataSourceMember.Path = path;
                if (Parent != null)
                    Parent.Update("SubReportDataNodes");
            }
        }

        public readonly ReportDataNode Parent;
        protected OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource ReportDataSource;
        public ReportDataNode(OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource, ReportDataNode parent)
        {
            Parent = parent;
            ReportDataSource = reportDataSource;


        }

        private void LoadSubReportDataNodes()
        {
            if (_SubReportDataNodes == null)
            {
                _SubReportDataNodes = new List<ReportDataNode>();

                if (ReportDataSource != null)
                {
                    foreach (ReportDataSource detailDataSource in ReportDataSource.DetailDataSources)
                        _SubReportDataNodes.Add(new ReportDataSourceNode(detailDataSource, this));

                    foreach (Member dataSourceMember in ReportDataSource.DataSourceMembers)
                        _SubReportDataNodes.Add(new ReportDataSourceMemberNode(dataSourceMember, this));
                }
            }
        }

        protected OOAdvantech.UserInterface.ReportObjectDataSource.Member ReportDataSourceMember;
        public ReportDataNode(OOAdvantech.UserInterface.ReportObjectDataSource.Member reportDataSourceMember, ReportDataNode parent)
        {
            Parent = parent;
            ReportDataSourceMember = reportDataSourceMember;
        }

        protected List<ReportDataNode> _SubReportDataNodes = null;
        [Browsable(false)]
        public List<ReportDataNode> SubReportDataNodes
        {
            get
            {
                if (_SubReportDataNodes == null)
                    LoadSubReportDataNodes();
                return new List<ReportDataNode>(_SubReportDataNodes);

            }
        }

        internal void NewMember(string p)
        {
            throw new NotImplementedException();
        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        public void LoadTypeMembers()
        {

            if (ReportDataSource != null)
            {
                Classifier type = ReportDataSource.Type;
            }


        }

        public void AddDataSourceMember()
        {
            if (ReportMetaDataObject is ReportDataSource)
            {
                //SelectedNode.NewMember("newmember");
                _SubReportDataNodes.Add(new ReportDataNode((ReportMetaDataObject as ReportDataSource).NewMember("newmember"), this));
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "SubReportDataNodes");

            }


        }
        protected void Update(string member)
        {
            if (member == "SubReportDataNodes")
                _SubReportDataNodes = null;

            if (ObjectChangeState != null)
                ObjectChangeState(this, member);
        }
        public void AddDetailDataSource()
        {
            if (ReportMetaDataObject is ReportDataSource)
            {
                //SelectedNode.NewMember("newmember");
                _SubReportDataNodes.Add(new ReportDataNode((ReportMetaDataObject as ReportDataSource).NewDetailDataSource("NewDetailDataSource"), this));
                if (ObjectChangeState != null)
                    ObjectChangeState(this, "SubReportDataNodes");

            }
        }

        void DeleteNode(ReportDataNode reportDataNode)
        {
            if (_SubReportDataNodes.Contains(reportDataNode))
            {
                _SubReportDataNodes.Remove(reportDataNode);
                if (reportDataNode.ReportDataSourceMember != null)
                    ReportDataSource.DeleteMember(reportDataNode.ReportDataSourceMember);

                if (reportDataNode.ReportDataSource != null)
                    ReportDataSource.DeleteDetailDataSource(reportDataNode.ReportDataSource);

                if (ObjectChangeState != null)
                    ObjectChangeState(this, "SubReportDataNodes");
                

            }


        }
        public void DeleteNode()
        {
            if (Parent != null)
            {
                Parent.DeleteNode(this);
                
            }

        }

    }

    /// <MetaDataID>{5b2487ce-2d33-4c9d-b948-d4665436bf2b}</MetaDataID>
    public class ReportDataSourceNode : ReportDataNode
    {

        public ReportDataSourceNode(OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource, ReportDataNode parent)
            : base(reportDataSource, parent)
        {
        }
    }


    /// <MetaDataID>{e0ff0185-3f82-4582-a506-52dfd736d263}</MetaDataID>
    public class ReportNode : ReportDataSourceNode
    {

        /// <exclude>Excluded</exclude>
        int _PathReportNodesDepth;

        public int PathReportNodesDepth
        {
            get
            {
                if (ReportDataSource is OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource)
                    return (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).PathNodesDepth;
                return 0;
            }
            set
            {
                if (ReportDataSource is OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource)
                    (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).PathNodesDepth = value;
            }
        }

        public ReportNode(OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource, ReportDataNode parent)
            : base(reportDataSource, parent)
        {
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override object Path
        {
            get
            {
                if (ReportDataSource != null)
                    return ReportDataSource.Path;
                else
                    return ReportDataSourceMember.Path;
            }
            set
            {
                string path = null;
                if (value is string)
                    path = value as string;
                else if (value is MetaData)
                {
                    path = (value as MetaData).Path;
                    if ((value as MetaData).MetaObject == null)
                        path = null;
                    else
                    {
                        if ((value as MetaData).MetaObject is AssociationEnd &&
                            ((value as MetaData).MetaObject as AssociationEnd).CollectionClassifier != null &&
                            ReportDataSourceMember != null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                ReportDataSource = ReportDataSourceMember.DataSource.MakeMemberDetailDataSource(ReportDataSourceMember, path);
                                ReportDataSourceMember.DataSource.HasCustomSetings = true;
                                ReportDataSourceMember.DataSource.EnsurePersistency();
                                stateTransition.Consistent = true;
                            }
                        }

                    }

                }
                else if (value is OOAdvantech.MetaDataRepository.Attribute)
                {
                    if ((value as OOAdvantech.MetaDataRepository.Attribute).IsStatic)
                    {
                        path = (value as OOAdvantech.MetaDataRepository.Attribute).FullName;
                        path = null;

                        OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult queryResult = ((value as OOAdvantech.MetaDataRepository.Attribute).GetExtensionMetaObject(typeof(System.Reflection.PropertyInfo)) as System.Reflection.PropertyInfo).GetValue(null, null) as OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult;
                        Type queryResultType = queryResult.QueryResultType.GetInterface(typeof(System.Collections.Generic.IEnumerable<>).Name).GetGenericArguments()[0];
                        ReportRootType = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(queryResultType);

                    }
                }

                if (ReportDataSource != null)
                    ReportDataSource.Path = path;
                else
                    ReportDataSourceMember.Path = path;

                _SubReportDataNodes = null;
                Update("SubReportDataNodes");
            }
        }


        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object QueryResultPath
        {
            get
            {
                if (ReportDataSource is OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource )
                    return (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).QueryResultPath;
                else
                    return ReportDataSourceMember.Path;
            }
            set
            {
                string path = null;
                if (value is string)
                    path = value as string;
                else if (value is MetaData)
                {
                    path = (value as MetaData).Path;
                    if ((value as MetaData).MetaObject == null)
                        path = null;
                    else
                    {
                        if ((value as MetaData).MetaObject is AssociationEnd &&
                            ((value as MetaData).MetaObject as AssociationEnd).CollectionClassifier != null &&
                            ReportDataSourceMember != null)
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                ReportDataSource = ReportDataSourceMember.DataSource.MakeMemberDetailDataSource(ReportDataSourceMember, path);
                                ReportDataSourceMember.DataSource.HasCustomSetings = true;
                                ReportDataSourceMember.DataSource.EnsurePersistency();
                                stateTransition.Consistent = true;
                            }
                        }

                    }

                }
                else if (value is OOAdvantech.MetaDataRepository.Attribute && (value as OOAdvantech.MetaDataRepository.Attribute).IsStatic)
                {
                    path = (value as OOAdvantech.MetaDataRepository.Attribute).FullName;
                }

                if (ReportDataSource != null)
                    (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).QueryResultPath= path;

                _SubReportDataNodes = null;
                Update("SubReportDataNodes");
            }
        }
        
        public override bool CanItAccept(OOAdvantech.MetaDataRepository.MetaObject metaObject, string propertyDescriptor)
        {
            if (metaObject is Classifier)
                return true;
            else if (metaObject is OOAdvantech.MetaDataRepository.Attribute && (metaObject as OOAdvantech.MetaDataRepository.Attribute).Type.IsA(OOAdvantech.MetaDataRepository.Classifier.GetClassifier(typeof(OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult))))
                return true;
            else
                return false;

        }

        //EditAssemply

        [Editor(typeof(EditAssemply), typeof(System.Drawing.Design.UITypeEditor))]
        public object ReportAssembly
        {
            get
            {
                return ReportDataSource.AssemblyFullName;
            }
            set
            {
                //if (value is OOAdvantech.MetaDataRepository.Classifier)
                //    ReportDataSource.AssemblyFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;

                if (value is string)
                    ReportDataSource.AssemblyFullName = value as string;
                _SubReportDataNodes = null;
                Update("SubReportDataNodes");
            }
        }


        internal OOAdvantech.MetaDataRepository.Component Component
        {
            get
            {
                try
                {
                    System.Reflection.Assembly dotNetAssembly = null;

                    foreach (System.Reflection.Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (loadedAssembly.FullName == ReportDataSource.AssemblyFullName)
                            dotNetAssembly = loadedAssembly;
                    }
                    if (dotNetAssembly == null)
                        dotNetAssembly = System.Reflection.Assembly.Load(ReportDataSource.AssemblyFullName);
                    System.Type metaObjectMapperType = ModulePublisher.ClassRepository.GetType("OOAdvantech.DotNetMetaDataRepository.MetaObjectMapper", "");
                    OOAdvantech.MetaDataRepository.Component assembly = metaObjectMapperType.GetMethod("FindMetaObjectFor").Invoke(null, new object[1] { dotNetAssembly }) as OOAdvantech.MetaDataRepository.Component;
                    if (assembly == null)
                        assembly = ModulePublisher.ClassRepository.CreateInstance("OOAdvantech.DotNetMetaDataRepository.Assembly", "", dotNetAssembly, new System.Type[1] { typeof(System.Reflection.Assembly) }) as OOAdvantech.MetaDataRepository.Component;
                    return assembly;
                }
                catch (System.Exception error)
                {


                }
                return null;

            }

        }

        /// <MetaDataID>{1BFCA073-9EA1-4DF7-A4CE-177CB4487050}</MetaDataID>
        [Editor(typeof(EditMetaData), typeof(System.Drawing.Design.UITypeEditor))]
        public object ReportRootType
        {
            get
            {
                return ReportDataSource.TypeFullName;
            }
            set
            {
                if (value is OOAdvantech.MetaDataRepository.Classifier)
                    ReportDataSource.TypeFullName = (value as OOAdvantech.MetaDataRepository.Classifier).FullName;

                if (value is string)
                    ReportDataSource.TypeFullName = value as string;
                _SubReportDataNodes = null;
                Update("SubReportDataNodes");

            }
        }



    }

    /// <MetaDataID>{b8f315b8-9f97-4da8-8f00-bd189020ebcf}</MetaDataID>
    public class ReportDataSourceMemberNode : ReportDataNode
    {

        public ReportDataSourceMemberNode(OOAdvantech.UserInterface.ReportObjectDataSource.Member reportDataSourceMember, ReportDataNode parent)
            : base(reportDataSourceMember, parent)
        {

        }


    }


}
