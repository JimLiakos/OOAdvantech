using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using System.ComponentModel.Design;
using DevExpress.XtraReports.Localization;
using System.ComponentModel;
using DevExpress.XtraReports.Design;
using System.Drawing.Design;
using DevExpress.Data.Design;
using ConnectableControls;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.UserInterface.ReportObjectDataSource;
using System.Xml.Linq;

namespace DXConnectableControls.XtraReports.UI
{
    /// <MetaDataID>{03839a4c-1590-445a-b955-d317ea907855}</MetaDataID>
    //[XRDesigner("DXConnectableControls.XtraReports.Design.ReportDesigner," + "DXConnectableControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=655cd54f8de5326f", typeof(IRootDesigner))]
    public class Report : XtraReport, IReport
    {
        string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            internal set
            {
                _FileName = value;
            }
        }
        
        

        public static new Report FromFile(string path, bool loadState)
        {
            Report report = XtraReport.FromFile(path, loadState) as Report;
            if (report != null)
                report._FileName = path;

            return report;
        }
        public Report()
            : base()
        {
            if (ClonedReport != null)
            {
                InDesignMode = ClonedReport.ReportDesignMode;
                _ObjectsDataSource = ClonedReport._ObjectsDataSource;
                DataSource = new ConnectableControls.ReportData.ReportData(this);
                object data = (DataSource as ConnectableControls.ReportData.ReportData).Data;
            }

        }
        internal bool InDesignMode;
        //protected override void OnBeforePrint(System.Drawing.Printing.PrintEventArgs e)
        //{
        //    if (DesignMode||InDesignMode)
        //    {
        //        if (DataSource is ConnectableControls.ReportData.ReportData)
        //            (DataSource as ConnectableControls.ReportData.ReportData).LoadDesignPreviewData();
        //        base.OnBeforePrint(e);
        //    }
        //    else
        //    {
        //        if (DataSource is ConnectableControls.ReportData.ReportData)
        //            (DataSource as ConnectableControls.ReportData.ReportData).LoadData();
        //        base.OnBeforePrint(e);
        //    }
        //}
        //protected override XtraReport CloneReport()
        //{
        //    XtraReport cloneReport = base.CloneReport();
        //    if (cloneReport is Report)
        //        (cloneReport as Report).InDesignMode = DesignMode; 
        //    return cloneReport;
        //}
        public bool ReportDesignMode
        {
            get
            {
                return InDesignMode | DesignMode;
            }
        }
 
        protected override void SerializeProperties(DevExpress.XtraReports.Serialization.XRSerializer serializer)
        {
            base.SerializeProperties(serializer);
            _ObjectsDataSource.EnsurePersistency();
            serializer.SerializeValue("ObjectsDataSource", ObjectsDataSourceMetaData.ToString(), typeof(string));
        }
        
        protected override void DeserializeProperties(DevExpress.XtraReports.Serialization.XRSerializer serializer)
        {
            base.DeserializeProperties(serializer);
            string xml = serializer.DeserializeValue("ObjectsDataSource", typeof(string), null) as string;
            if (!string.IsNullOrEmpty(xml))
            {
                ObjectsDataSourceMetaData = XDocument.Parse(xml);
                

                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                try
                {
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", ObjectsDataSourceMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                }
                catch (OOAdvantech.PersistenceLayer.StorageException error)
                {
                    ObjectsDataSourceMetaData = new XDocument();
                    try
                    {
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", ObjectsDataSourceMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }
                    catch (Exception errorB)
                    {
                        return;
                    }
                }

                OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT reportDataSource FROM OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource reportDataSource ");
                OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource objectsDataSource = null; ;
                foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                {
                    objectsDataSource = setInstance["reportDataSource"] as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource;
                    break;
                }
                if (objectsDataSource != null &&
                    _ObjectsDataSource != null &&
                    _ObjectsDataSource.AssemblyFullName == objectsDataSource.AssemblyFullName &&
                    _ObjectsDataSource.TypeFullName == objectsDataSource.TypeFullName &&
                    _ObjectsDataSource.QueryResultPath == objectsDataSource.QueryResultPath )
                {
                    return;
                }


                _ObjectsDataSource = objectsDataSource;
                if (_ObjectsDataSource == null)
                {
                    _ObjectsDataSource = new OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource();
                    storage.CommitTransientObjectState(_ObjectsDataSource);
                }

                _ObjectsDataSource.Report = this;

                DataSource = new ConnectableControls.ReportData.ReportData(this);
                _ReportLoaded = true;
            }



        }

     

        private static void GetPaths(XtraReportBase xtraReport, List<string> paths)
        {
            if (xtraReport.Bands != null)
            {
                foreach (Band band in xtraReport.Bands)
                {
                    if (band is XtraReportBase)
                        GetPaths(band as XtraReportBase, paths);


                    foreach (XRControl control in band.Controls)
                    {
                        //if (control is XRFieldEmbeddableControl)
                        {

                            foreach (XRBinding binding in control.DataBindings)
                            {
                                paths.Add(binding.DataMember);

                            }

                        }
                    }
                }
            }
        }

        static Report ClonedReport;
        protected override XtraReport CloneReport()
        {
            try
            {
                ClonedReport = this;
                XtraReport report= base.CloneReport();
                return report;

            }
            finally
            {
                ClonedReport = null;
            }
            
        }

        XDocument ObjectsDataSourceMetaData;
        OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource _ObjectsDataSource;
        [Editor(typeof(ConnectableControls.PropertyEditors.EditReportDataSource), typeof(System.Drawing.Design.UITypeEditor))]
        public object ObjectsDataSource
        {
            get
            {
                if (ObjectsDataSourceMetaData == null)
                {
                    ObjectsDataSourceMetaData = new XDocument();
                    OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                    storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporaryOperationCallStorage", ObjectsDataSourceMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    _ObjectsDataSource = new OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource();
                    _ObjectsDataSource.Report = this;
                    //ClearDataContext();
                    
                    DataSource = new ConnectableControls.ReportData.ReportData(this);
                    
                    
                    storage.CommitTransientObjectState(_ObjectsDataSource);

                }


                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (_ObjectsDataSource == null)
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(ObjectsDataSourceMetaData.ToString(), "none");
                else
                    metaDataVaue = new UserInterfaceMetaData.MetaDataValue(ObjectsDataSourceMetaData.ToString(), _ObjectsDataSource.Name);

                metaDataVaue.MetaDataAsObject = _ObjectsDataSource;
                return metaDataVaue;



            }
            
            set
            {
                if (value is OOAdvantech.UserInterface.OperationCall && DesignMode)
                {
                    TypeDescriptor.GetProperties(this).Find("ObjectsDataSource", false).SetValue(this, _ObjectsDataSource);
                    return;
                }
                string metaData = value as string;
                UserInterfaceMetaData.MetaDataValue metaDataVaue = null;
                if (value is MetaDataValue)
                {
                    metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                    metaDataVaue = value as UserInterfaceMetaData.MetaDataValue;
                }
                OOAdvantech.PersistenceLayer.ObjectStorage storage = null;
                if (metaDataVaue == null)
                    return;


                if (ObjectsDataSourceMetaData == null && metaDataVaue.XMLMetaData != null || (metaDataVaue != null && metaDataVaue.MetaDataAsObject == null))
                {
                    _ObjectsDataSource = null;
                    ObjectsDataSourceMetaData = new XDocument();
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            ObjectsDataSourceMetaData=XDocument.Parse(metaData);
                    }
                    catch (Exception error)
                    {
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(metaData))
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("TemporarylistViewStorage", ObjectsDataSourceMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        else
                        {
                            ObjectsDataSourceMetaData = new XDocument();
                            storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", ObjectsDataSourceMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                        }





                    }
                    catch (OOAdvantech.PersistenceLayer.StorageException error)
                    {
                        ObjectsDataSourceMetaData = new XDocument();
                        storage = OOAdvantech.PersistenceLayer.ObjectStorage.NewStorage("TemporarylistViewStorage", ObjectsDataSourceMetaData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
                    }

                    OOAdvantech.Collections.StructureSet set = storage.Execute("SELECT reportDataSource FROM OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource reportDataSource ");
                    foreach (OOAdvantech.Collections.StructureSet setInstance in set)
                    {
                        _ObjectsDataSource = setInstance["reportDataSource"] as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource;
                        break;
                    }
                    if (_ObjectsDataSource == null)
                    {
                        
                        _ObjectsDataSource = new OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource();
                        storage.CommitTransientObjectState(_ObjectsDataSource);
                    }

                    _ObjectsDataSource.Report = this;
                    //ClearDataContext();
                    
                    DataSource = new ConnectableControls.ReportData.ReportData(this);
                }
                else
                {
                    if (metaDataVaue.MetaDataAsObject is OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource)
                    {
                        this.SuspendLayout();
                        _ObjectsDataSource = metaDataVaue.MetaDataAsObject as OOAdvantech.UserInterface.ReportObjectDataSource.ReportRootDataSource;
                        //if (DataSource is ConnectableControls.ReportData.ReportData)
                        //{
                        //    DataSource = null;
                        //    DataSource = new ConnectableControls.ReportData.ReportData(this);
                        //}
                        //else
                        {
                            //ClearDataContext();
                            //if (DataSource is System.Collections.ArrayList)
                            //    (DataSource as System.Collections.ArrayList).Clear();

                            //DataSource = null;
                            DataSource = new ConnectableControls.ReportData.ReportData(this);
                            
                        }

                        this.ResumeLayout();

                    }
                }
                _ReportLoaded = true;
                return;
            }
        }


        //System.ComponentModel.Design.IDesignerHost _DesignerHost;
        //public override System.ComponentModel.ISite Site
        //{
        //    get
        //    {
        //        return base.Site;
        //    }
        //    set
        //    {
        //        base.Site = value;
        //        if (value != null && value.Container is System.ComponentModel.Design.IDesignerHost)
        //        {
        //            _DesignerHost = value.Container as System.ComponentModel.Design.IDesignerHost;
        //            (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded += new System.ComponentModel.Design.ComponentEventHandler(OnComponentAdded);
        //            (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentRemoved += new System.ComponentModel.Design.ComponentEventHandler(OnComponentRemoved);
        //            //(_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentChanged += new System.ComponentModel.Design.ComponentChangedEventHandler(ComponentChanged);
        //        }
        //        else
        //        {
        //            if (_DesignerHost != null)
        //            {
        //                (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentAdded -= new System.ComponentModel.Design.ComponentEventHandler(OnComponentAdded);
        //                (_DesignerHost as System.ComponentModel.Design.IComponentChangeService).ComponentRemoved -= new System.ComponentModel.Design.ComponentEventHandler(OnComponentRemoved);
        //                _DesignerHost = null;
        //            }
        //        }
        //    }
        //}

        //void OnComponentRemoved(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        //{

        //    if (!_DesignerHost.Loading)
        //    {
        //    }




        //}
        //DevExpress.XtraGrid.Views.Base.BaseView NewView;
        //DevExpress.XtraGrid.Views.Base.BaseView OldView;
        //void OnComponentAdded(object sender, System.ComponentModel.Design.ComponentEventArgs e)
        //{
        //    if (!_DesignerHost.Loading)
        //    {

        //    }


        //}







        #region IReport Members

        Classifier IReport.GetClassifier(string typeFullName, bool caseSensitive)
        {
            return OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource.GetClassifier(typeFullName, caseSensitive, this);
        }

        bool IReport.CanBeTransient(Member member)
        {
            string path = null;
            if (member.DataSource != null)
                path = GetPath(member.DataSource);
            if (string.IsNullOrEmpty(path))
                path = member.Name;
            else
                path += "." + member.Name;

            foreach (string dataBindingPath in (this as IReport).Paths)
            {
                if (dataBindingPath.IndexOf(path) == 0)
                    return false;
            }
            return true;

        }
        bool IReport.CanBeTransient(ReportDataSource detailReportDataSource)
        {
            string path = GetPath(detailReportDataSource);
            if (string.IsNullOrEmpty(path))
                return true;
            foreach (string dataBindingPath in (this as IReport).Paths)
            {
                if (dataBindingPath.IndexOf(path) == 0)
                    return false;
            }
            if (detailReportDataSource.MetaObject == null)
                return false;
            return true;


        }

        private string GetPath(ReportDataSource detailReportDataSource)
        {
            if (detailReportDataSource.MasterDataSource == null)
                return null;
            string path = GetPath(detailReportDataSource.MasterDataSource);
            if (string.IsNullOrEmpty(path))
                return detailReportDataSource.Name;
            else
                return path + "." + detailReportDataSource.Name; ;


        }


        List<string> IReport.Paths
        {
            get
            {
                List<string> paths = new List<string>();
                GetPaths(this, paths);
                return paths;
            }
        }
        OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource IReport.ReportDataSource
        {
            get
            {
                return _ObjectsDataSource;
            }
        }
        #endregion

        #region IReport Members


        bool _ReportLoaded = false;
        public bool ReportLoaded
        {
            get 
            {
                return _ReportLoaded;
            }
        }

        #endregion
    }
}
