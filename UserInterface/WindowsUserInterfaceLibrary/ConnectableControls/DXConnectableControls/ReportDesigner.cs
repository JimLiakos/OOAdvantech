using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Drawing.Design;
using ConnectableControls.PropertyEditors;
using DevExpress.XtraReports.Design;
using DevExpress.XtraReports.Localization;
namespace DXConnectableControls.XtraReports.Design
{
    /// <MetaDataID>{10b095fe-14f0-4d71-a173-b9e2e4c9adcd}</MetaDataID>
    public class ReportDesigner : DevExpress.XtraReports.Design.ReportDesigner
    {
        public ReportDesigner(): base()
        {             
        }
        
        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            if (this.RootReport is DXConnectableControls.XtraReports.UI.Report)
            {
                properties.Remove("DataSource");
                properties.Remove("DataMember");
                properties.Remove("DataAdapter");
                properties.Remove("DataSourceSchema");
            }
        }



        protected override void RegisterActionLists(DesignerActionListCollection list)
        {
            if (this.RootReport is DXConnectableControls.XtraReports.UI.Report)
            {
                list.Add(new DatasourceDesignerActionList(this, this.RootReport as DXConnectableControls.XtraReports.UI.Report));
            }
        }

    }

    /// <MetaDataID>{3c9334bd-0c6f-43bb-a0ce-b6415702ef4d}</MetaDataID>
    public class DatasourceDesignerActionList : DevExpress.XtraReports.Design.DataContainerDesignerActionList
    {
        DXConnectableControls.XtraReports.UI.Report Report;
        public DatasourceDesignerActionList(XRComponentDesigner designer,DXConnectableControls.XtraReports.UI.Report report):base(designer)
        {
            Report = report;

        }

        object _ObjectsDataSource;
        [Editor(typeof(EditReportDataSource), typeof(System.Drawing.Design.UITypeEditor))]
        public object ObjectsDataSource
        {
            get
            {
                return Report.ObjectsDataSource;
            }
            set
            {
                
                
                Report.ObjectsDataSource = value;
                DataSource = null;
                DataSource = Report.DataSource;
                
                 
            }
        }

        protected override void FillActionItemCollection(DesignerActionItemCollection actionItems)
        {
            ReportLocalizer oldReportLocalizer = DevExpress.XtraReports.Localization.ReportLocalizer.Active;
            try
            {
                DevExpress.XtraReports.Localization.ReportLocalizer.Active = new MyLocalizer();

                base.FillActionItemCollection(actionItems);
                base.AddPropertyItem(actionItems, "ObjectsDataSource", "ObjectsDataSource");

            }
            finally
            {
                DevExpress.XtraReports.Localization.ReportLocalizer.Active = oldReportLocalizer;

            }
        }

 

 


        
    }

    /// <MetaDataID>{2e1b003a-0d1c-4a8c-ae7d-fab30a685431}</MetaDataID>
    public class MyLocalizer : DevExpress.XtraReports.Localization.ReportLocalizer
    {
        public override string GetLocalizedString(DevExpress.XtraReports.Localization.ReportStringId id)
        {
            //if (id == DevExpress.XtraReports.Localization.ReportStringId.STag_Name_DataBinding)
            //    return "ObjectsDataSource";

            return base.GetLocalizedString(id);
        }
    }
}
