using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using static System.Net.Mime.MediaTypeNames;

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{a6ed6e6e-3a57-49a2-9c0f-d5eadbb61ea0}</MetaDataID>
    public class TwoStateButton:ToggleButton,INotifyPropertyChanged
    {
        public TwoStateButton() {

            

        }


        public event PropertyChangedEventHandler PropertyChanged;



        public string OnDescription
        {
            get
            {
                object value = GetValue(OnDescriptionProperty);
                return value as string;
            }
            set
            {
                SetValue(OnDescriptionProperty, value);
            }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty OnDescriptionProperty =
                    DependencyProperty.Register(
                    "OnDescription",
                    typeof(string),
                    typeof(TwoStateButton),
                    new PropertyMetadata("On", new PropertyChangedCallback(OnDescriptionPropertyChangedCallback)));


        public static void OnDescriptionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is TwoStateButton)
                (d as TwoStateButton).OnDescriptionPropertyChanged();
        }

        private void OnDescriptionPropertyChanged()
        {


        }


        public string OffDescription
        {
            get
            {
                object value = GetValue(OffDescriptionProperty);
                return value as string;
            }
            set
            {
                SetValue(OffDescriptionProperty, value);
            }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty OffDescriptionProperty =
                    DependencyProperty.Register(
                    "OffDescription",
                    typeof(string),
                    typeof(TwoStateButton),
                    new PropertyMetadata("Off", new PropertyChangedCallback(OffDescriptionPropertyChangedCallback)));


        public static void OffDescriptionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is TwoStateButton)
                (d as TwoStateButton).OffDescriptionPropertyChanged();
        }

        private void OffDescriptionPropertyChanged()
        {
          

        }
    }
}
