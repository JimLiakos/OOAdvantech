using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFUIElementObjectBind
{
    /// <summary>
    /// Exposes attached behaviors that can be
    /// applied to Button objects.
    /// </summary>
    /// <MetaDataID>{a1ff759b-25a6-4d4f-81b4-233ed2cae12d}</MetaDataID>
    public static class ButtonClosePopupBehavior
    {
        #region ClosePopupOnClick

        public static bool GetClosePopupOnClick(Button button)
        {
            return (bool)button.GetValue(ClosePopupOnClickProperty);
        }

        public static void SetClosePopupOnClick(Button button, bool value)
        {
            button.SetValue(ClosePopupOnClickProperty, value);
        }

        public static readonly DependencyProperty ClosePopupOnClickProperty =
            DependencyProperty.RegisterAttached(
            "ClosePopupOnClick",
            typeof(bool),
            typeof(ButtonClosePopupBehavior),
            new UIPropertyMetadata(false, OnClosePopupOnClickChanged));

        static void OnClosePopupOnClickChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            Button button = depObj as Button;
            if (button == null)
                return;

            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
                button.Click += OnClicked;
            else
                button.Click -= OnClicked;
        }

        static void OnClicked(object sender, RoutedEventArgs e)
        {

            // Only react to the Selected event raised by the TreeViewItem
            // whose IsSelected property was modified.  Ignore all ancestors
            // who are merely reporting that a descendant's Selected fired.
            if (!Object.ReferenceEquals(sender, e.OriginalSource))
                return;

            Button button = e.OriginalSource as Button;

            if (button != null)
            {
                var popupList = FindChilds<System.Windows.Controls.Primitives.Popup>(Window.GetWindow(button));
                var popup = FindParentPopup(button, popupList);
                if (popup != null)
                    popup.IsOpen = false;
            }
        }

        static System.Windows.Controls.Primitives.Popup FindParentPopup(DependencyObject child, List<System.Windows.Controls.Primitives.Popup> popupList)
        {
            foreach (var popup in popupList)
            {
                if (popup.Child == child)
                    return popup;
            }
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            return FindParentPopup(parentObject, popupList);
        }

        public static List<T> FindChilds<T>(DependencyObject parent) where T : DependencyObject
        {

            List<T> foundChilds = new List<T>();

            if (parent == null)
                return foundChilds;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChilds.AddRange(FindChilds<T>(child));

                    // If the child is found, break so we do not overwrite the found child.
                    //if (foundChild != null) break;
                }
                else
                {
                    // child element found.
                    foundChilds.Add((T)child);

                    foundChilds.AddRange(FindChilds<T>(child));
                    break;
                }
            }
            return foundChilds;
        }
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {

            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        //public static T FindParent<T>(DependencyObject child)  where T : DependencyObject
        //{
        //    //get parent item
        //    DependencyObject parentObject = VisualTreeHelper.GetParent(child);

        //    //we've reached the end of the tree
        //    if (parentObject == null) return null;

        //    //check if the parent matches the type we're looking for
        //    T parent = parentObject as T;
        //    if (parent != null)
        //    {
        //        return parent;
        //    }
        //    else
        //    {
        //        return FindParent<T>(parentObject);
        //    }
        //}





        #endregion // ClosePopupOnClick
    }
}
