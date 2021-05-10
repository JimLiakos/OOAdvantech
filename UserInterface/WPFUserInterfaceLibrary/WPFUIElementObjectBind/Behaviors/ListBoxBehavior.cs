using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{cc1b29ab-c9bd-45df-85a9-4293267a759d}</MetaDataID>
    public static class ListBoxBehavior
    {
        public static bool GetScrollSelectedIntoView(ListBox listBox)
        {
            return (bool)listBox.GetValue(ScrollSelectedIntoViewProperty);
        }

        public static void SetScrollSelectedIntoView(ListBox listBox, bool value)
        {
            listBox.SetValue(ScrollSelectedIntoViewProperty, value);
        }

        public static readonly DependencyProperty ScrollSelectedIntoViewProperty =
            DependencyProperty.RegisterAttached("ScrollSelectedIntoView", typeof(bool), typeof(ListBoxBehavior),
                                                new UIPropertyMetadata(false, OnScrollSelectedIntoViewChanged));

        private static void OnScrollSelectedIntoViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as Selector;
            if (selector == null) return;


            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
            {
                selector.AddHandler(Selector.SelectionChangedEvent, new RoutedEventHandler(ListBoxSelectionChangedHandler));
            }
            else
            {
                selector.RemoveHandler(Selector.SelectionChangedEvent, new RoutedEventHandler(ListBoxSelectionChangedHandler));
            }



        }


        public static bool GetUnselectAllOnEmptyClick(ListBox listBox)
        {
            return (bool)listBox.GetValue(UnselectAllOnEmptyClickProperty);
        }

        public static void SetUnselectAllOnEmptyClick(ListBox listBox, bool value)
        {
            listBox.SetValue(UnselectAllOnEmptyClickProperty, value);
        }


        public static readonly DependencyProperty UnselectAllOnEmptyClickProperty =
         DependencyProperty.RegisterAttached("UnselectAllOnEmptyClick", typeof(bool), typeof(ListBoxBehavior),
                                             new UIPropertyMetadata(false, OnUnselectAllOnEmptyClickChanged));

        private static void OnUnselectAllOnEmptyClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selector = d as Selector;
            if (selector == null) return;


            if (e.NewValue is bool == false)
                return;



            if ((bool)e.NewValue)
            {
                selector.AddHandler(Selector.PreviewMouseLeftButtonUpEvent, new RoutedEventHandler(ListBoxMouseDownHandler));
            }
            else
            {
                selector.RemoveHandler(Selector.PreviewMouseLeftButtonUpEvent, new RoutedEventHandler(ListBoxMouseDownHandler));
            }

        }


        private static void ListBoxSelectionChangedHandler(object sender, RoutedEventArgs e)
        {
            if (!(sender is ListBox)) return;

            var listBox = (sender as ListBox);
            if (listBox.SelectedItem != null)
            {
                listBox.Dispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        listBox.UpdateLayout();
                        if (listBox.SelectedItem != null)
                            listBox.ScrollIntoView(listBox.SelectedItem);
                    }));
            }
        }

        private static void ListBoxMouseDownHandler(object sender, RoutedEventArgs e)
        {

            System.Windows.Input.MouseButtonEventArgs eventArgs = e as System.Windows.Input.MouseButtonEventArgs;

            if (!(sender is ListBox)) return;

            var listBox = (sender as ListBox);

            HitTestResult r = VisualTreeHelper.HitTest(listBox, eventArgs.GetPosition(listBox));
            if (r != null && r.VisualHit is System.Windows.Controls.ScrollViewer)
            {
                listBox.UnselectAll();
            }
            else
            {

            }
            //    listBox.UnselectAll();

            //if (listBox.SelectedItem != null)
            //{
            //    listBox.Dispatcher.BeginInvoke(
            //        (Action)(() =>
            //        {
            //            listBox.UpdateLayout();
            //            if (listBox.SelectedItem != null)
            //                listBox.ScrollIntoView(listBox.SelectedItem);
            //        }));
            //}
        }
    }
}
