using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFUIElementObjectBind.Behaviors
{
    /// <summary>
    /// Exposes attached behaviors that can be
    /// applied to TreeViewItem objects.
    /// </summary>
    /// <MetaDataID>{a893b73f-5b5e-4f2f-a303-9cb08df6fbeb}</MetaDataID>
    public static class TreeViewItemBehavior
    {
        #region IsBroughtIntoViewWhenSelected

        public static bool GetIsBroughtIntoViewWhenSelected(TreeViewItem treeViewItem)
        {
            return (bool)treeViewItem.GetValue(IsBroughtIntoViewWhenSelectedProperty);
        }

        public static void SetIsBroughtIntoViewWhenSelected(
          TreeViewItem treeViewItem, bool value)
        {
            treeViewItem.SetValue(IsBroughtIntoViewWhenSelectedProperty, value);
        }

        public static readonly DependencyProperty IsBroughtIntoViewWhenSelectedProperty =
            DependencyProperty.RegisterAttached(
            "IsBroughtIntoViewWhenSelected",
            typeof(bool),
            typeof(TreeViewItemBehavior),
            new UIPropertyMetadata(false, OnIsBroughtIntoViewWhenSelectedChanged));

        static void OnIsBroughtIntoViewWhenSelectedChanged(
          DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            TreeViewItem item = depObj as TreeViewItem;
            if (item == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                item.Selected += OnTreeViewItemSelected;
            else
                item.Selected -= OnTreeViewItemSelected;

            if ((bool)e.NewValue)
                item.Expanded += Item_Expanded;
            else
                item.Expanded -= Item_Expanded;
        }
        private static void Item_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            TreeViewItem item = e.OriginalSource as TreeViewItem;

            TreeViewItem lastChildNode = null;
            foreach (var subItem in item.Items.OfType<object>().ToList())
            {
                if (item.ItemContainerGenerator.ContainerFromItem(subItem) is TreeViewItem)
                    lastChildNode = item.ItemContainerGenerator.ContainerFromItem(subItem) as TreeViewItem;
                
                
              
            }
           
            if (lastChildNode != null)
                lastChildNode.BringIntoView();
        }

        static void OnTreeViewItemSelected(object sender, RoutedEventArgs e)
        {
            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified. Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (item != null)
                item.BringIntoView();
        }

        #endregion // IsBroughtIntoViewWhenSelected
    }
}
