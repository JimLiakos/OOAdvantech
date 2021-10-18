using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace System.Windows.Controls
{

    
    /// <MetaDataID>{d62200e2-d5b5-49a6-bcaa-9a0f2df9bdf6}</MetaDataID>
    public partial class EditableTextBlock : UserControl, System.ComponentModel.INotifyPropertyChanged
    {

        #region Constructor

        /// <MetaDataID>{33252ccc-47d3-45e9-9927-913f1a2eb84f}</MetaDataID>
        public EditableTextBlock()
        {


            InitializeComponent();
            base.Focusable = true;
            base.FocusVisualStyle = null;
            ContentTemplate = FindResource("DisplayModeTemplate") as DataTemplate;

            Loaded += EditableTextBlock_Loaded;
        }
        public Xceed.Wpf.Toolkit.WatermarkTextBox WatermarkTextBox
        {
            get
            {
                if (this.ContentTemplate != null)
                    return this.ContentTemplate.FindName("WatermarkTextBox", this) as Xceed.Wpf.Toolkit.WatermarkTextBox;
                else
                    return null;
            }

        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
            if (WatermarkTextBox != null)
            {

            }
        }
   
        private void EditableTextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            //var bind = BindingOperations.GetBinding(this, EditableTextBlock.TextProperty);
            //if(bind!=null)
            //{
            //    Xceed.Wpf.Toolkit.WatermarkTextBox watermarkTextBox = this.FindName("WatermarkTextBox") as Xceed.Wpf.Toolkit.WatermarkTextBox;
            //    var waterMarkBindin = BindingOperations.GetBinding(watermarkTextBox, Xceed.Wpf.Toolkit.WatermarkTextBox.TextProperty);
            //    if(waterMarkBindin!=null)
            //        waterMarkBindin.UpdateSourceTrigger = bind.UpdateSourceTrigger;
            //}
        }

        #endregion Constructor

        #region Member Variables

        // We keep the old text when we go into editmode
        // in case the user aborts with the escape key
        /// <MetaDataID>{c6330305-6fe8-4275-a1e9-b5d80101cced}</MetaDataID>
        private string oldText;

        #endregion Member Variables

        #region Properties

        /// <MetaDataID>{5fa030ce-f12d-446e-b945-00c5f545f9ba}</MetaDataID>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty TextProperty =
                    DependencyProperty.Register(
                    "Text",
                    typeof(string),
                    typeof(EditableTextBlock),
                    new PropertyMetadata("", new PropertyChangedCallback(TextPropertyChangedCallback)));


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock)
                (d as EditableTextBlock).TextPropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void TextPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedText)));
        }


        public bool UnTranslated
        {
            get { return (bool)GetValue(UnTranslatedProperty); }
            set { SetValue(UnTranslatedProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty UnTranslatedProperty =
                    DependencyProperty.Register(
                    "UnTranslated",
                    typeof(bool),
                    typeof(EditableTextBlock),
                    new PropertyMetadata(false, new PropertyChangedCallback(UnTranslatedPropertyChangedCallback)));


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void UnTranslatedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock)
                (d as EditableTextBlock).UnTranslatedPropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void UnTranslatedPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
        }



        public TextBoxInputType TextBoxInputType
        {
            get { return (TextBoxInputType)GetValue(TextBoxInputTypeProperty); }
            set { SetValue(TextBoxInputTypeProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty TextBoxInputTypeProperty =
                    DependencyProperty.Register(
                    "TextBoxInputType",
                    typeof(TextBoxInputType),
                    typeof(EditableTextBlock),
                    new PropertyMetadata(TextBoxInputType.String, new PropertyChangedCallback(TextBoxInputTypePropertyChangedCallback)));


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void TextBoxInputTypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock)
                (d as EditableTextBlock).TextBoxInputTypePropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void TextBoxInputTypePropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextBoxInputType)));
        }


        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextHorizontalAlignmentProperty); }
            set { SetValue(TextHorizontalAlignmentProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
                    DependencyProperty.Register(
                    "TextAlignment",
                    typeof(TextAlignment),
                    typeof(EditableTextBlock),
                    new PropertyMetadata(TextAlignment.Left, new PropertyChangedCallback(TextHorizontalAlignmentPropertyChangedCallback)));


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void TextHorizontalAlignmentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock)
                (d as EditableTextBlock).TextHorizontalAlignmentPropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void TextHorizontalAlignmentPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextAlignment)));
        }

        public HorizontalAlignment LabelHorizontalAlignment
        {
            get
            {
                if (TextAlignment == TextAlignment.Left)
                    return HorizontalAlignment.Left;

                if (TextAlignment == TextAlignment.Right)
                    return HorizontalAlignment.Right;

                if (TextAlignment == TextAlignment.Center)
                    return HorizontalAlignment.Center;

                return HorizontalAlignment.Stretch;
            }
        }


        public bool SpellCheckEnable
        {
            get { return (bool)GetValue(SpellCheckEnableProperty); }
            set { SetValue(SpellCheckEnableProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty SpellCheckEnableProperty =
                    DependencyProperty.Register(
                    "SpellCheckEnable",
                    typeof(bool),
                    typeof(EditableTextBlock),
                    new PropertyMetadata(false, new PropertyChangedCallback(SpellCheckEnablePropertyChangedCallback)));


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void SpellCheckEnablePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock)
                (d as EditableTextBlock).SpellCheckEnablePropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void SpellCheckEnablePropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpellCheckEnable)));
        }






        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        /// <MetaDataID>{547e83a9-7d89-4737-8cb1-f343771e7a7c}</MetaDataID>
        public static readonly DependencyProperty WatermarkProperty =
                    DependencyProperty.Register(
                    "Watermark",
                    typeof(string),
                    typeof(EditableTextBlock),
                    new PropertyMetadata("", new PropertyChangedCallback(WatermarkPropertyChangedCallback)));



        public static void WatermarkPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditableTextBlock)
                (d as EditableTextBlock).WatermarkPropertyChanged();
        }


        private void WatermarkPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Watermark)));
        }


        public WPFUIElementObjectBind.ITranslator Translator
        {
            get { return (WPFUIElementObjectBind.ITranslator)GetValue(TranslatorProperty); }
            set { SetValue(TranslatorProperty, value); }
        }

        public static readonly DependencyProperty TranslatorProperty =
                   DependencyProperty.Register(
                   "Translator",
                   typeof(WPFUIElementObjectBind.ITranslator),
                   typeof(EditableTextBlock),
                   new PropertyMetadata(null));


        /// <MetaDataID>{bb2e9862-9324-49a5-9d31-2757edf966d3}</MetaDataID>
        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }
        /// <MetaDataID>{cbcc25d5-331b-4dce-8089-05a70e1606d9}</MetaDataID>
        public static readonly DependencyProperty IsEditableProperty =
                    DependencyProperty.Register(
                    "IsEditable",
                    typeof(bool),
                    typeof(EditableTextBlock),
                    new PropertyMetadata(true));

        /// <MetaDataID>{aea21a93-63f9-457f-bb89-dc8aeb4fec96}</MetaDataID>
        public bool IsInEditMode
        {
            get
            {
                if (IsEditable)
                    return (bool)GetValue(IsInEditModeProperty);
                else
                    return false;
            }
            set
            {
                if (IsEditable)
                {
                    if (value) oldText = Text;
                    SetValue(IsInEditModeProperty, value);
                }
            }
        }
        /// <MetaDataID>{e9105318-13cc-4cf6-858b-7f9061a41f24}</MetaDataID>
        public static readonly DependencyProperty IsInEditModeProperty =
                    DependencyProperty.Register(
                    "IsInEditMode",
                    typeof(bool),
                    typeof(EditableTextBlock),
                    new PropertyMetadata(false, new PropertyChangedCallback(IsInEditModePropertyChangedCallback)));



        public static void IsInEditModePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is EditableTextBlock)
                (d as EditableTextBlock).IsInEditModePropertyChanged();

        }
        private void IsInEditModePropertyChanged()
        {

            if (IsInEditMode)
            {
                ContentTemplate = FindResource("EditModeTemplate") as DataTemplate;
            }
            else
            {
                ContentTemplate = FindResource("DisplayModeTemplate") as DataTemplate;
            }



        }

        /// <MetaDataID>{5eed40a2-32b7-47ec-94ce-ea649069b531}</MetaDataID>
        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set
            {
                if (value == "") value = "{0}";
                SetValue(TextFormatProperty, value);
            }
        }
        /// <MetaDataID>{ac6d51df-17e5-4623-8b72-dfa19c497b47}</MetaDataID>
        public static readonly DependencyProperty TextFormatProperty =
                    DependencyProperty.Register(
                    "TextFormat",
                    typeof(string),
                    typeof(EditableTextBlock),
                    new PropertyMetadata("{0}"));

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{36269433-fcf9-4f90-9e45-db5cc8617ee8}</MetaDataID>
        public string FormattedText
        {
            get { return String.Format(TextFormat, Text); }
        }

        #endregion Properties

        #region Event Handlers

        public event DragObjectEventHandler DragObject;

        /// <MetaDataID>{616a69b1-c945-493c-880f-c6b7a00ac685}</MetaDataID>
        double orgHeight;
        // Invoked when we enter edit mode.
        /// <MetaDataID>{cee692f0-25e6-44f7-9940-db8e8cfe1352}</MetaDataID>
        void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            
            TextBox txt = sender as TextBox;

            // txt.FontSize = FontSize;
            //orgHeight = Height;
            //txt.Height = _ActualHeight * 1.4;
            //Height = _ActualHeight*1.6;
            // Give the TextBox input focus
            txt.Focus();

            txt.SelectAll();
        }

        private void TextBox_PreviewLostFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextBoxText))
                this.Text = TextBoxText;

        }

        // Invoked when we exit edit mode.
        /// <MetaDataID>{705d3528-3d0c-4b0a-8252-e3e86ac58bfe}</MetaDataID>
        void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {

          
            
            
            var scope = FocusManager.GetFocusScope(this);
            Button focusedButton = null;
            Button translateBtn = null;
            if (scope != null)
            {
                focusedButton = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) as Button;
                translateBtn = WPFUIElementObjectBind.ObjectContext.FindChilds<Button>(this as DependencyObject).FirstOrDefault();
            }
            if (focusedButton == null || focusedButton != translateBtn)
                this.IsInEditMode = false;
        }

        TextBox spelcheckTextBox = new TextBox();



    
        private void TranslateBtn_LostFocus(object sender, RoutedEventArgs e)
        {

            var scope = FocusManager.GetFocusScope(this);
            TextBox focusedText = null;
            TextBox textBox = null;
            if (scope != null)
            {
                focusedText = FocusManager.GetFocusedElement(FocusManager.GetFocusScope(this)) as TextBox;
                textBox = WPFUIElementObjectBind.ObjectContext.FindChilds<TextBox>(this as DependencyObject).FirstOrDefault();
            }
            if (focusedText == null || focusedText != textBox)
                this.IsInEditMode = false;
        }


        // Invoked when the user edits the annotation.
        /// <MetaDataID>{8f3a3eb6-0255-4463-82de-a5fb401a2c8c}</MetaDataID>
        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.IsInEditMode = false;
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                this.IsInEditMode = false;
                Text = oldText;
                e.Handled = true;
            }
        }
        string TextBoxText;
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBoxText = (sender as Xceed.Wpf.Toolkit.WatermarkTextBox).Text;
        }

        /// <MetaDataID>{3b4a0388-1f6e-4f9d-a363-8976736c3a00}</MetaDataID>
        double _ActualHeight;
        private Point? dragStartPoint;
        #endregion Event Handlers

        /// <MetaDataID>{c2eb339c-6785-4681-94d4-94f8461640b2}</MetaDataID>
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _ActualHeight = ActualHeight;
            if (e.ClickCount == 2)
                IsInEditMode = true;
        }

        private void TranslateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.GetObjectContext().FormObjectConnection.Culture != null)
            {
                var culture = this.GetObjectContext().FormObjectConnection.Culture;
                var proxy = OOAdvantech.UserInterface.Runtime.UIProxy.GetUIProxy(this.DataContext);

                if (proxy != null && proxy.UserInterfaceObjectConnection != null && proxy.UserInterfaceObjectConnection.Culture != null)
                    culture = proxy.UserInterfaceObjectConnection.Culture;
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(culture, false))
                {
                    try
                    {
                        
                        string text = Translator.TranslateString(Text, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
                        if (text == Text)
                            Text = text + " ";

                        Text = text;
                    }
                    catch (Exception error)
                    {
                    }
                }
            }
        }

        private void TranslateBtn_GotFocus(object sender, RoutedEventArgs e)
        {
            IsInEditMode = true;
        }

        private void WatermarkTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.TextBoxInputType == TextBoxInputType.Decimal)
            {

                char decPoint = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
                foreach (char ch in e.Text)
                {
                    bool numericTextPart = Char.IsDigit(ch) || ':'.Equals(ch) || decPoint.Equals(ch);
                    if (!numericTextPart)
                    {
                        e.Handled = true;
                        break;
                    }
                }


            }
        }

        private void DisplayMode_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;
            else
            {
                if (!this.dragStartPoint.HasValue)
                    this.dragStartPoint = new Point?(e.GetPosition(this));

                Vector diff = e.GetPosition(this) - this.dragStartPoint.Value;
                if (diff.Y > 3 || diff.Y < 3 || diff.X > 3 || diff.X < 3)
                {
                    DragObject?.Invoke(this);
                }
            }

        }

        

       
    }

    public delegate void DragObjectEventHandler(object sender);
    /// <MetaDataID>{538ea73b-f376-4668-9543-ad631c4cf220}</MetaDataID>
    public enum TextBoxInputType
    {
        String,
        Integer,
        Decimal
    }
}

