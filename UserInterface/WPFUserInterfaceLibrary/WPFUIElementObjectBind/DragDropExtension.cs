using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFUIElementObjectBind
{
    /// <summary>
    /// Provides extended support for drag drop operation
    /// </summary>
    /// <MetaDataID>{baea8096-ed99-4079-9540-0b88c42150f6}</MetaDataID>
    public static class DragDropExtension
    {
        public static readonly DependencyProperty ScrollOnDragDropProperty =
            DependencyProperty.RegisterAttached("ScrollOnDragDrop",
                typeof(bool),
                typeof(DragDropExtension),
                new PropertyMetadata(false, HandleScrollOnDragDropChanged));

        public static bool GetScrollOnDragDrop(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (bool)element.GetValue(ScrollOnDragDropProperty);
        }

        public static void SetScrollOnDragDrop(DependencyObject element, bool value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(ScrollOnDragDropProperty, value);
        }

        private static void HandleScrollOnDragDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement container = d as FrameworkElement;

            if (d == null)
            {
                Debug.Fail("Invalid type!");
                return;
            }

            Unsubscribe(container);

            if (true.Equals(e.NewValue))
            {
                Subscribe(container);
            }
        }

        private static void Subscribe(FrameworkElement container)
        {
            container.PreviewDragOver += OnContainerPreviewDragOver;
        }

        private static void OnContainerPreviewDragOver(object sender, DragEventArgs e)
        {
            FrameworkElement container = sender as FrameworkElement;

            if (container == null)
            {
                return;
            }

            ScrollViewer scrollViewer = GetFirstVisualChild<ScrollViewer>(container);

            if (scrollViewer == null)
            {
                return;
            }

            double tolerance = 60;
            double verticalPos = e.GetPosition(container).Y;
            double offset = 20;

            if (verticalPos < tolerance) // Top of visible list? 
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offset); //Scroll up. 
            }
            else if (verticalPos > container.ActualHeight - tolerance) //Bottom of visible list? 
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offset); //Scroll down.     
            }
        }

        private static void Unsubscribe(FrameworkElement container)
        {
            container.PreviewDragOver -= OnContainerPreviewDragOver;
        }

        private static T GetFirstVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = GetFirstVisualChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }

            return null;
        }
    }

    /// <MetaDataID>{7ae23a39-00a5-415b-9592-5d417412189d}</MetaDataID>
    public interface IDragDropTarget
    {
        /// <MetaDataID>{3a9c7278-11ab-4c80-a064-b9985297fa73}</MetaDataID>
        void DragEnter(object sender, DragEventArgs e);

        /// <MetaDataID>{74f60b3c-6346-42f1-9f63-47d3016c18a6}</MetaDataID>
        void DragLeave(object sender, DragEventArgs e);

        /// <MetaDataID>{2e536b6d-3f44-4c4e-b706-4020a715ff29}</MetaDataID>
        void DragOver(object sender, DragEventArgs e);

        /// <MetaDataID>{66b60780-81a8-4a4f-a880-c99146d5d54c}</MetaDataID>
        void Drop(object sender, DragEventArgs e);
    }
}
