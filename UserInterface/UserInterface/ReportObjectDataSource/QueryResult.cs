using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech.UserInterface.ReportObjectDataSource
{
    /// <MetaDataID>{1ce79ef6-944d-415a-ba68-f71faea9d568}</MetaDataID>
    public interface IQueryResult
    {
        Type QueryResultType
        {
            get;
        }
        System.Collections.IEnumerable Result
        {
            get;
        }
        System.Collections.IEnumerable ResultForReportDesign
        {
            get;
        }


    }
}
