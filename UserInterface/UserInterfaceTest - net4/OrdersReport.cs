using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace UserInterfaceTest
{
    /// <MetaDataID>{9a226b2e-0faf-4b35-b4db-ebc90da9b02d}</MetaDataID>
    public partial class OrdersReport :DXConnectableControls.XtraReports.UI.Report
    {
        public OrdersReport()
        {
            InitializeComponent();
        }

    }

    //public class OrdersReportData : ArrayList, ITypedList
    //{
    //    #region ITypedList Members

    //    public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
    //    {
    //        return TypeDescriptor.GetProperties(typeof(AbstractionsAndPersistency.IOrder));
    //    }

    //    public string GetListName(PropertyDescriptor[] listAccessors)
    //    {
    //        return "Order";
    //    }

    //    #endregion
    //}
}
