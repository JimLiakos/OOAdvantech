using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{0c10a00f-fc86-426a-a3d5-3ea07dd8ab40}</MetaDataID>
    public class DataGridExtender:IUIElementExtender,OOAdvantech.UserInterface.Runtime.IPathDataDisplayer
    {

        /// <MetaDataID>{4671eca7-2f3f-469f-b553-dd672574e8dc}</MetaDataID>
        DataGrid DataGrid;
        /// <MetaDataID>{fcae8d76-4ecb-4af7-bbc1-0f8462e36505}</MetaDataID>
        public void Attach(System.Windows.FrameworkElement UIElement,UIElementManager uiElementManager)
        { 
              
            DataGrid = UIElement as DataGrid;
            string path = null;
            if (uiElementManager.GetPropertyPath(DataGrid as System.Windows.FrameworkElement, DataGrid.ItemsSourceProperty,out path))
            {

                foreach (DataGridColumn column in DataGrid.Columns)
                {
                    if (column is DataGridBoundColumn)
                    {
                        if (((column as DataGridBoundColumn).Binding is System.Windows.Data.Binding) && _Paths.Contains(path + "." + ((column as DataGridBoundColumn).Binding as System.Windows.Data.Binding).Path.Path))
                            _Paths.Add(path + "." + ((column as DataGridBoundColumn).Binding as System.Windows.Data.Binding).Path.Path);
                    }
                    else if(column is DataGridTemplateColumn)
                    {
                        System.Windows.DataTemplate cellEditingTemplate = (column as DataGridTemplateColumn).CellEditingTemplate;

                        //for (var i = 0; i < VisualTreeHelper.GetChildrenCount(column); i++)
                        //{
                        //    var child = VisualTreeHelper.GetChild(column, i);
                        //    //if (child != null && child.GetType() == typeof(T))
                        //    //    return child as T;
                        //}
                    }
                }
            }
           // DataGrid.SelectedCellsChanged += new SelectedCellsChangedEventHandler(DataGrid_SelectedCellsChanged);
            DataGrid.BeginningEdit += new EventHandler<DataGridBeginningEditEventArgs>(DataGrid_BeginningEdit);
        }

        void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //OOAdvantech.UserInterface.DynamicUIProxy dynamicUIProxy = e.Row.DataContext as OOAdvantech.UserInterface.DynamicUIProxy;
            //if (dynamicUIProxy != null && e.Column is DataGridBoundColumn)
            //{
            //    string path = ((e.Column as DataGridBoundColumn).Binding as System.Windows.Data.Binding).Path.Path;
            //    if (!dynamicUIProxy.UserInterfaceObjectConnection.CanEditValue(dynamicUIProxy.Target, dynamicUIProxy.TargetType, path, this))
            //    {
            //        e.Cancel = true;
            //    }
            //}
            

        }

        /// <MetaDataID>{d4f0855b-afe6-4dd5-9ae1-05ffbaf3a0d2}</MetaDataID>
        void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //foreach (var cell in e.AddedCells)
            //{
            //    OOAdvantech.UserInterface.DynamicUIProxy dynamicUIProxy = cell.Item as OOAdvantech.UserInterface.DynamicUIProxy;
            //    if (dynamicUIProxy != null && cell.Column is DataGridBoundColumn)
            //    {
            //        string path = ((cell.Column as DataGridBoundColumn).Binding as System.Windows.Data.Binding).Path.Path;
            //        bool ret = false;
            //        dynamicUIProxy.UserInterfaceObjectConnection.GetDisplayedValue(dynamicUIProxy.Target, OOAdvantech.MetaDataRepository.Classifier.GetClassifier(dynamicUIProxy.TargetType), path, this, out ret);
            //    }
            //}
        }



        #region IPathDataDisplayer Members

        public object Path
        {
            get
            {
                return "";
            }
            set
            {
                
            }
        }

        public void LoadControlValues()
        {
            
        }

        public void SaveControlValues()
        {
            
        }

        public OOAdvantech.UserInterface.Runtime.UserInterfaceObjectConnection UserInterfaceObjectConnection
        {
            get 
            {
                if (DataGrid.DataContext is OOAdvantech.UserInterface.DynamicUIProxy)
                    return (DataGrid.DataContext as OOAdvantech.UserInterface.DynamicUIProxy).UserInterfaceObjectConnection;
                else
                    return null;
            }
        }

        List<string> _Paths = new List<string>();
        public OOAdvantech.Collections.Generic.List<string> Paths
        {
            get { return new OOAdvantech.Collections.Generic.List<string>(); }
        }

        public bool HasLockRequest
        {
            get { return true;}
        }

        public void DisplayedValueChanged(object sender, OOAdvantech.UserInterface.Runtime.MemberChangeEventArg change)
        {
            
        }

        public void LockStateChange(object sender)
        {
            
        }

        #endregion
    }
}
