using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace ConnectableControls.ReportData
{
    /// <MetaDataID>{ec613798-0639-4ae6-857c-c634c0ad0573}</MetaDataID>
    public class ReportData:System.Collections.ArrayList,ITypedList
    {
        public override void CopyTo(Array array)
        {
            base.CopyTo(array);
        }
        public override IEnumerator GetEnumerator()
        {
            return base.GetEnumerator();
        }
        public override IEnumerator GetEnumerator(int index, int count)
        {
            return base.GetEnumerator(index, count);
        }
        public override ArrayList GetRange(int index, int count)
        {
            return base.GetRange(index, count);
        }
        public override object this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }
        public override int Count
        {
            get
            {
                //if (collectionObjectsType != ReportDataSource.DataSourceType)
                //{
                //    Clear();
                //    _Data = null;
                //}
                //object data = Data;
                return base.Count;
            }
        }
        internal System.Collections.Generic.Dictionary<Type, OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource> AllReportDataSources=new Dictionary<Type,OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource>();
        OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource  ReportDataSource;
        OOAdvantech.UserInterface.ReportObjectDataSource.IReport Report;  
        public ReportData( OOAdvantech.UserInterface.ReportObjectDataSource.IReport report  )
        {
            ReportDataSource = report.ReportDataSource;
            Report = report;
            object obj = Data;

        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if (listAccessors != null && listAccessors.Length > 0)
            {
                PropertyDescriptor listAccessor = listAccessors[listAccessors.Length - 1];
                if (listAccessor.PropertyType == typeof(System.Collections.ArrayList))
                {
                    if(!AllReportDataSources.ContainsKey(listAccessor.ComponentType))
                        return new PropertyDescriptorCollection(new PropertyDescriptor[0]);
                    OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource masterReportDataSource = AllReportDataSources[listAccessor.ComponentType];
                    foreach (OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource in masterReportDataSource.DetailDataSources)
                    {
                        if (reportDataSource.Name == listAccessor.Name)
                        {
                            AllReportDataSources[reportDataSource.DataSourceType] = reportDataSource;
                            return TypeDescriptor.GetProperties(reportDataSource.DataSourceType);
                        }
                    }
                }
                else
                {
                    if (AllReportDataSources.ContainsKey(listAccessor.ComponentType))
                    {
                        OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource masterReportDataSource = AllReportDataSources[listAccessor.ComponentType];
                        foreach (OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource in masterReportDataSource.DetailDataSources)
                        {
                            if (reportDataSource.Name == listAccessor.Name)
                            {
                                AllReportDataSources[reportDataSource.DataSourceType] = reportDataSource;
                                return TypeDescriptor.GetProperties(reportDataSource.DataSourceType);
                            }
                        }
                    }
                    return TypeDescriptor.GetProperties(listAccessor.PropertyType);
                }

            }
            AllReportDataSources[ReportDataSource.DataSourceType] = ReportDataSource;
            return TypeDescriptor.GetProperties(ReportDataSource.DataSourceType);
        }
        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return ReportDataSource.Name;
        }
        System.Type collectionObjectsType;
        /// <exclude>Excluded</exclude>
        object _Data;
        public object Data
        {
            get
            {
                
                if (_Data==null&&ReportDataSource is OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource)
                {
                   // if (Report.ReportLoaded)
                    {
                        //if (Report.ReportDesignMode)
                            LoadDesignPreviewData();
                       // else
                        //    LoadData();
                    }
                                        //OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult queryResult = (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).QueryResult;
                    //if (queryResult != null)
                    //{
                    //    this.Clear();
                    //    List<object> results = new List<object>();
                    //    foreach (object obj in queryResult.Result)
                    //        results.Add(obj);
                    //    _Data = results;
                    //    OOAdvantech.UserInterface.PathNode rootObjectNode = new OOAdvantech.UserInterface.PathNode("Root", null);
                    //    foreach (string path in Report.Paths)
                    //        rootObjectNode.AddPath(path);

                    //    Validate(ref rootObjectNode);
                    //    collectionObjectsType = ReportDataSource.DataSourceType;
                    //    this.AddRange(OOAdvantech.UserInterface.ReportObjectDataSource.ObjectSateProvider.GetReportData(_Data as System.Collections.IList, ReportDataSource, rootObjectNode));
                    //}

                }
                return _Data;

            }
            set
            {
                Clear();
                _Data = value;
                if (_Data is IEnumerable)
                {
                    foreach (object obj in (_Data as IEnumerable))
                    {

                    }
                }



                OOAdvantech.UserInterface.PathNode rootObjectNode = new OOAdvantech.UserInterface.PathNode("Root", null);
                foreach (string path in Report.Paths)
                    rootObjectNode.AddPath(path);

                Validate(ref rootObjectNode);
                collectionObjectsType = ReportDataSource.DataSourceType;
                this.AddRange(OOAdvantech.UserInterface.ReportObjectDataSource.ObjectSateProvider.GetReportData(_Data as System.Collections.IList,ReportDataSource,rootObjectNode));

            }
        }
        void Validate(ref OOAdvantech.UserInterface.PathNode rootObjectNode)
        {

            
            OOAdvantech.UserInterface.PathNode validRootPathNode = new OOAdvantech.UserInterface.PathNode("Root",null);
            if (!string.IsNullOrEmpty(ReportDataSource.Path))
                validRootPathNode = validRootPathNode.AddPath(ReportDataSource.Path);
       
            Validate(rootObjectNode, validRootPathNode, ReportDataSource);
            rootObjectNode = validRootPathNode;
            //if(!string.IsNullOrEmpty(ReportDataSource.Path))
            //    validRootPathNode=validRootPathNode.AddPath(ReportDataSource.Path);
            
            //foreach (OOAdvantech.UserInterface.PathNode pathNode in rootObjectNode.Members)
            //{
            //    OOAdvantech.UserInterface.Component component = ReportDataSource.GetMember(pathNode.Name);
            //    OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource detailReportDataSource = component as OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource;
            //    if (detailReportDataSource != null)
            //    {
            //        OOAdvantech.UserInterface.PathNode pathNode = null;
            //        if (!string.IsNullOrEmpty(ReportDataSource.Path))
            //            pathNode = validRootPathNode.AddPath(ReportDataSource.Path);
            //        else
            //            pathNode = validRootPathNode.AddPath(ReportDataSource.Name);
            //    }
            //}
        }
        void Validate(OOAdvantech.UserInterface.PathNode originPathNode,  OOAdvantech.UserInterface.PathNode newPathNode, OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource)
        {

            foreach (OOAdvantech.UserInterface.PathNode pathNode in originPathNode.Members)
            {
                OOAdvantech.UserInterface.Component component = reportDataSource.GetMember(pathNode.Name);
                OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource detailReportDataSource = component as OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource;
                OOAdvantech.UserInterface.ReportObjectDataSource.Member member = component as OOAdvantech.UserInterface.ReportObjectDataSource.Member;
                if (detailReportDataSource != null && detailReportDataSource.MetaObject!=null)
                {
                    OOAdvantech.UserInterface.PathNode validPathNode = null;
                    if (!string.IsNullOrEmpty(detailReportDataSource.Path))
                        validPathNode = newPathNode.AddPath(detailReportDataSource.MetaObject.Name + "." + detailReportDataSource.Path);
                    else
                        validPathNode = newPathNode.AddPath(detailReportDataSource.MetaObject.Name);

                    Validate(pathNode, validPathNode, detailReportDataSource);
                }
                if (member != null && member.MetaObject!=null)
                    if (!string.IsNullOrEmpty(member.Path))
                        newPathNode.AddPath(member.MetaObject.Name+"."+ member.Path);
                    else
                        newPathNode.AddPath(member.MetaObject.Name);


                //OOAdvantech.UserInterface.PathNode pathNode = null;
                //if (!string.IsNullOrEmpty(detailReportDataSource.Path))
                //    pathNode = validRootPathNode.AddPath(ReportDataSource.Path);
                //else
                //    pathNode = validRootPathNode.AddPath(ReportDataSource.Name);
            }

        }



        public void LoadData()
        {
            if (_Data == null && ReportDataSource is OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource)
            {
                try
                {
                    OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult queryResult = (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).QueryResult;
                    if (queryResult != null)
                    {
                        this.Clear();
                        List<object> results = new List<object>();
                        foreach (object obj in queryResult.Result)
                            results.Add(obj);
                        _Data = results;
                        OOAdvantech.UserInterface.PathNode rootObjectNode = new OOAdvantech.UserInterface.PathNode("Root", null);
                        foreach (string path in Report.Paths)
                            rootObjectNode.AddPath(path);

                        Validate(ref rootObjectNode);
                        collectionObjectsType = ReportDataSource.DataSourceType;
                        this.AddRange(OOAdvantech.UserInterface.ReportObjectDataSource.ObjectSateProvider.GetReportData(_Data as System.Collections.IList, ReportDataSource, rootObjectNode));
                    }
                }
                catch (Exception error)
                {
                    _Data = new List<object>();
                    System.Diagnostics.Debug.WriteLine("Error on report data loading");
                }

            }
            
        }
 

        public void LoadDesignPreviewData()
        {
            if (_Data == null && ReportDataSource is OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource)
            {
                try
                {
                    OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult queryResult = (ReportDataSource as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource).QueryResult;
                    if (queryResult != null)
                    {
                        this.Clear();
                        List<object> results = new List<object>();
                        foreach (object obj in queryResult.ResultForReportDesign)
                            results.Add(obj);
                        _Data = results;
                        OOAdvantech.UserInterface.PathNode rootObjectNode = new OOAdvantech.UserInterface.PathNode("Root", null);
                        foreach (string path in Report.Paths)
                            rootObjectNode.AddPath(path);

                        Validate(ref rootObjectNode);
                        collectionObjectsType = ReportDataSource.DataSourceType;
                        this.AddRange(OOAdvantech.UserInterface.ReportObjectDataSource.ObjectSateProvider.GetReportData(_Data as System.Collections.IList, ReportDataSource, rootObjectNode));
                    }
                }
                catch (Exception error)
                {
                    _Data = new List<object>();
                    System.Diagnostics.Debug.WriteLine("Error on report data loading");
                }

            }
        }
    }
}
