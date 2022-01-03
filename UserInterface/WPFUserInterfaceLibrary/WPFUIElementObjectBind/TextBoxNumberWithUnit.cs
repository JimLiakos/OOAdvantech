using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace System.Windows.Controls
{
    /// <MetaDataID>{47bc6c08-7c96-4afa-8ba8-00ce7d382057}</MetaDataID>
    public class TextBoxNumberWithUnit:TextBox
    {

        public TextBoxNumberWithUnit()
        {

        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (IsFocused)
                Text = Number.ToString();
            else
                Text = Number.ToString() + Unit;
            
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            char decPoint= System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
            foreach(char ch in e.Text )
            {
                bool numericTextPart = false;
                if (Integer)
                    numericTextPart = Char.IsDigit(ch);
                else
                    numericTextPart = Char.IsDigit(ch) || ':'.Equals(ch) || decPoint.Equals(ch);

                if (!numericTextPart)
                {
                    e.Handled = true;
                    break;
                }
            }
            
            base.OnPreviewTextInput(e);
        }
        public decimal Number
        {
            get
            {
                object value = GetValue(NumberProperty);
                if (value is decimal)
                    return (decimal)value;
                else
                    return default(decimal);
            }
            set
            {
                SetValue(NumberProperty, value);
            }
        }

        public static readonly DependencyProperty NumberProperty =
                    DependencyProperty.Register(
                    "Number",
                    typeof(decimal),
                    typeof(TextBoxNumberWithUnit),
                    new PropertyMetadata(default(decimal), new PropertyChangedCallback(NumberPropertyChangedCallback)));

        

        public static void NumberPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is TextBoxNumberWithUnit)
                (d as TextBoxNumberWithUnit).NumberPropertyChanged();
        }

        private void NumberPropertyChanged()
        {
            if (IsFocused)
                Text = Number.ToString();
            else
                Text = Number.ToString() + Unit;
        }


        public bool Integer
        {
            get
            {
                object value = GetValue(IntegerProperty);
                if (value is bool)
                    return (bool)value;

                return default(bool);
            }
            set
            {
                SetValue(IntegerProperty, value);
            }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty IntegerProperty =
                    DependencyProperty.Register(
                    "Integer",
                    typeof(bool),
                    typeof(TextBoxNumberWithUnit),
                    new PropertyMetadata(false, new PropertyChangedCallback(IntegerPropertyChangedCallback)));


        public static void IntegerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is TextBoxNumberWithUnit)
                (d as TextBoxNumberWithUnit).IntegerPropertyChanged();
        }


        public string Unit
        {
            get
            {
                object value = GetValue(UnitProperty);
                return value as string;
            }
            set
            {
                SetValue(UnitProperty, value);
            }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty UnitProperty =
                    DependencyProperty.Register(
                    "Unit",
                    typeof(string),
                    typeof(TextBoxNumberWithUnit),
                    new PropertyMetadata("", new PropertyChangedCallback(UnitPropertyChangedCallback)));


        public static void UnitPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is TextBoxNumberWithUnit)
                (d as TextBoxNumberWithUnit).UnitPropertyChanged();
        }

        private void UnitPropertyChanged()
        {
            if (IsFocused)
                Text = Number.ToString();
            else
                Text = Number.ToString() + Unit;

        }

        private void IntegerPropertyChanged()
        {
            if (IsFocused)
                Text = Number.ToString();
            else
                Text = Number.ToString() + Unit;

        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            decimal result = 0;
            decimal.TryParse(Text, out result);
            Number = result;
            Text = Number.ToString() + Unit;
            base.OnLostFocus(e);
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (IsFocused)
                Text = Number.ToString();
            else
                Text = Number.ToString() + Unit;

            base.OnGotFocus(e);
        }




    }
}
