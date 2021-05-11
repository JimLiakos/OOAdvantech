using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectableControls.PropertyEditors
{
    /// <MetaDataID>{d94c98f8-ab83-4097-846e-bc77931a28c8}</MetaDataID>
    public class EditReportDataSource : System.Drawing.Design.UITypeEditor
    {
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string metaData = value as string;
            OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource reportDataSource = null; 
            if (value is UserInterfaceMetaData.MetaDataValue)
            {
                metaData = (value as UserInterfaceMetaData.MetaDataValue).XMLMetaData;
                reportDataSource = (value as UserInterfaceMetaData.MetaDataValue).MetaDataAsObject as OOAdvantech.UserInterface.ReportObjectDataSource.ReportDataSource;
            }

            if (reportDataSource != null)
            {
                ReportDataSourceForm reportDataSourceForm = new ReportDataSourceForm();
                reportDataSourceForm.Connection.Instance = reportDataSource;
                reportDataSourceForm.ShowDialog();

                if (value is UserInterfaceMetaData.MetaDataValue)
                {
                    value = new UserInterfaceMetaData.MetaDataValue((value as UserInterfaceMetaData.MetaDataValue).XMLMetaData, reportDataSource.Name);
                    (value as UserInterfaceMetaData.MetaDataValue).MetaDataAsObject = reportDataSource;
                }
            }
            return value;
                 
        }
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.Modal;
        }
    }
}
