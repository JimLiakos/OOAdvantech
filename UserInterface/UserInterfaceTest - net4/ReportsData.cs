using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech.PersistenceLayer;

namespace UserInterfaceTest
{
    /// <MetaDataID>{10159d52-2926-4851-bd19-3a619e3f7117}</MetaDataID>
    public static class ReportsData
    {
        public static OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult OrdersReportData
        {
            get
            {
                return new OrdersReportData();
            }
        }
    }


    /// <MetaDataID>{af284e98-1b02-40f3-b9c3-692c62555326}</MetaDataID>
    class OrdersReportData : OOAdvantech.UserInterface.ReportObjectDataSource.IQueryResult
    {

        #region IQueryResult Members
        public System.Collections.IEnumerable ResultForReportDesign 
        {
            get
            {
                return Result;
            }
        }

        public Type QueryResultType
        {
            get
            {
                ObjectStorage objectStorage = null;
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var result = from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                             select new
                             {
                                 Client = order.Client,
                                 Order = order,
                                 OrderItems = from orderDetail in order.OrderDetails
                                              select orderDetail
                             };


                return result.GetType();
            }
        }

        public System.Collections.IEnumerable Result
        {
            get
            {
                ObjectStorage objectStorage = ObjectStorage.OpenStorage("Abstractions",
                    @"localhost\sqlexpress",
                    "OOAdvantech.MSSQLPersistenceRunTime.EmbeddedStorageProvider");

                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var result = from order in storage.GetObjectCollection<AbstractionsAndPersistency.IOrder>()
                             select new
                             {
                                 Client = order.Client,
                                 Order = order,
                                 OrderItems = from orderDetail in order.OrderDetails
                                              select orderDetail
                             };


                foreach (var tmp in result)
                {

                }
                return result;
            }
        }

        #endregion
    }

}
