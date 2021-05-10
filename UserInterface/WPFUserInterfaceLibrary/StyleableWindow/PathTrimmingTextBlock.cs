using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StyleableWindow
{
    /// <MetaDataID>{2c704799-9eeb-4b9e-800d-c1e9b01f68ff}</MetaDataID>
    public class PathTrimmingTextBlock : TextBlock, INotifyPropertyChanged
    {

        FrameworkElement _container;


        public PathTrimmingTextBlock()
        {
            this.Loaded += new RoutedEventHandler(PathTrimmingTextBlock_Loaded);
        }

        void PathTrimmingTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Parent == null) throw new InvalidOperationException("PathTrimmingTextBlock must have a container such as a Grid.");

            _container = (FrameworkElement)this.Parent;
            _container.SizeChanged += new SizeChangedEventHandler(container_SizeChanged);

            Text = GetTrimmedPath(_container.ActualWidth);
        }

        void container_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Text = GetTrimmedPath(_container.ActualWidth);
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(PathTrimmingTextBlock), new UIPropertyMetadata(""));

        string GetTrimmedPath(double width)
        {

            string splitChar = "/";
            string filename = Path.Split('/').Last(); // System.IO.Path.GetFileName(Path);
            string directory = "";
            if (Path.Split('/').Length > 1)
                directory = Path.Replace("/" + filename, "");
            if (string.IsNullOrWhiteSpace(directory))
            {

                splitChar = " ";
                filename = Path.Split(' ').Last(); // System.IO.Path.GetFileName(Path);
                directory = "";
                if (Path.Split(' ').Length > 1)
                    directory = Path.Replace(" " + filename, "");
            }



            FormattedText formatted;
            bool widthOK = false;
            bool changedWidth = false;
            int count = 100;
            if(Path!=null)
                count =Path.Length * 100;
            do
            {
                formatted = new FormattedText(
                    ("{0}..." + splitChar + "{1}").FormatWith(directory, filename),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    FontFamily.GetTypefaces().First(),
                    FontSize,
                    Foreground
                    );

                widthOK = formatted.Width < width;

                if (!widthOK)
                {
                    changedWidth = true;

                    if (directory.Length > 2)
                        directory = directory.Substring(0, directory.Length - 1);

                    if (directory.Length == 0)
                        return "..." + splitChar + filename;
                }
                count--;
            } while (!widthOK&&count>0);

            if (!changedWidth)
            {
                return Path;
            }
            return "{0}...{1}".FormatWith(directory, splitChar + filename);
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion


    }
    /// <MetaDataID>{add48ff2-fc89-4a5a-ad87-0a8f3ac3b3ef}</MetaDataID>
    static class Extensions
    {
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
