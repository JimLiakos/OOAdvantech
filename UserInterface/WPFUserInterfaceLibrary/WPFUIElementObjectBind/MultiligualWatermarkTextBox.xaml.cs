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
    /// <MetaDataID>{c18a42c2-5a54-49c7-87ea-0232afc115f4}</MetaDataID>
    public partial class MultiligualWatermarkTextBox : UserControl, System.ComponentModel.INotifyPropertyChanged
    {

        #region Constructor

        public MultiligualWatermarkTextBox()
        {

            InitializeComponent();
            this.SizeChanged += MultiligualWatermarkTextBox_SizeChanged;

            try
            {
                var buttonStyle = FindResource("ListViewBarButtonStyle") as Style;
                if (buttonStyle != null)
                    TranslateBtn.Style = buttonStyle;
            }
            catch (Exception error)
            {


            }

        }

        private void MultiligualWatermarkTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        #endregion Constructor

        #region Member Variables


        private string oldText;

        #endregion Member Variables

        #region Properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
                    DependencyProperty.Register(
                    "Text",
                    typeof(string),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata("", new PropertyChangedCallback(TextPropertyChangedCallback)));


        public static void TextPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).TextPropertyChanged();
        }

        private void TextPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FormattedText)));
        }

        public Visibility TranslationButtonVisibile
        {
            get
            {
                if (Translator != null && UnTranslated)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

        }
        public bool UnTranslated
        {
            get { return (bool)GetValue(UnTranslatedProperty); }
            set { SetValue(UnTranslatedProperty, value); }
        }
        public static readonly DependencyProperty UnTranslatedProperty =
                    DependencyProperty.Register(
                    "UnTranslated",
                    typeof(bool),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata(false, new PropertyChangedCallback(UnTranslatedPropertyChangedCallback)));
        public static void UnTranslatedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is MultiligualWatermarkTextBox)
            {
                (d as MultiligualWatermarkTextBox).UnTranslatedPropertyChanged();
                if (e.NewValue is bool && ((bool)e.NewValue) == true)
                {
                    TextDecoration wavyLine = null;
                    try
                    {
                        wavyLine = (d as MultiligualWatermarkTextBox).FindResource("WavyLine") as TextDecoration;
                    }
                    catch (Exception error)
                    {
                    }
                    bool wavyDecorationExist = false;
                    foreach (var eixistingTextDecoration in (d as MultiligualWatermarkTextBox).WatermarkTextBox.TextDecorations)
                        wavyDecorationExist |= eixistingTextDecoration == wavyLine;

                    if (!wavyDecorationExist && wavyLine != null)
                    {

                        (d as MultiligualWatermarkTextBox).WatermarkTextBox.TextDecorations.Add(wavyLine);
                        (d as MultiligualWatermarkTextBox).SpellCheckIsEnabled = (d as MultiligualWatermarkTextBox).WatermarkTextBox.SpellCheck.IsEnabled;
                        (d as MultiligualWatermarkTextBox).SpellCheckEnable = false;
                        (d as MultiligualWatermarkTextBox).WatermarkTextBox.SpellCheck.IsEnabled = false;
                    }
                }
                else
                {
                    TextDecoration wavyLine = null;
                    try
                    {
                        wavyLine = (d as MultiligualWatermarkTextBox).FindResource("WavyLine") as TextDecoration;
                    }
                    catch (Exception error)
                    {
                    }

                    foreach (var textDecoration in (d as MultiligualWatermarkTextBox).WatermarkTextBox.TextDecorations.ToList())
                    {
                        if (textDecoration == wavyLine)
                        {
                            (d as MultiligualWatermarkTextBox).WatermarkTextBox.TextDecorations.Remove(textDecoration);
                        }
                    }
                    (d as MultiligualWatermarkTextBox).SpellCheckEnable = (d as MultiligualWatermarkTextBox).SpellCheckIsEnabled;

                }


            }
            
        }
        private void UnTranslatedPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TranslationButtonVisibile)));
        }

        public bool AcceptsTab
        {
            get { return (bool)GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }
        public static readonly DependencyProperty AcceptsTabProperty =
                    DependencyProperty.Register(
                    "AcceptsTab",
                    typeof(bool),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata(false, new PropertyChangedCallback(AcceptsTabPropertyChangedCallback)));
        public static void AcceptsTabPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).AcceptsTabPropertyChanged();

        }


        private void AcceptsTabPropertyChanged()
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AcceptsTab)));
        }
        public bool AcceptsReturn
        {
            get { return (bool)GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }
        public static readonly DependencyProperty AcceptsReturnProperty =
                    DependencyProperty.Register(
                    "AcceptsReturn",
                    typeof(bool),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata(false, new PropertyChangedCallback(AcceptsReturnPropertyChangedCallback)));
        public static void AcceptsReturnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).AcceptsReturnPropertyChanged();

        }
        private void AcceptsReturnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AcceptsReturn)));
        }


        static VisualBrush _WavyBrush;
        static VisualBrush WavyBrush
        {
            get
            {
                if (_WavyBrush == null)
                {
                    _WavyBrush = new VisualBrush();
                    _WavyBrush.Viewbox = new Rect(0, 0, 3, 2);
                    _WavyBrush.ViewboxUnits = BrushMappingMode.Absolute;
                    _WavyBrush.Viewport = new Rect(0, 0, 6, 8);
                    _WavyBrush.ViewportUnits = BrushMappingMode.Absolute;
                    _WavyBrush.TileMode = TileMode.Tile;
                    _WavyBrush.Visual = new Path()
                    {
                        Data = Geometry.Parse("M 0,1 C 1,0 2,2 3,1"),
                        StrokeThickness = 0.2,
                        StrokeEndLineCap = PenLineCap.Square,
                        StrokeStartLineCap = PenLineCap.Square,
                        Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0xA2, 0x06, 0x8D))
                    };

                    /*
                     *   <VisualBrush.Visual>
                        <Path Data="M 0,1 C 1,0 2,2 3,1" Stroke="#FFA2068D" StrokeThickness="0.2" StrokeEndLineCap="Square" StrokeStartLineCap="Square" />
                    </VisualBrush.Visual>
                     */
                }
                return _WavyBrush;
            }
        }

        /// <summary>
        ///
        ///     Gets a System.Windows.Controls.SpellCheck object that provides access to spelling
        ///     errors in the text contents of a System.Windows.Controls.Primitives.TextBoxBase
        ///     or System.Windows.Controls.RichTextBox.
        ///
        /// </summary>

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
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata(false, new PropertyChangedCallback(SpellCheckEnablePropertyChangedCallback)));


        /// <MetaDataID>{2c072aec-8ab0-450f-9b74-0a689a8e847e}</MetaDataID>
        public static void SpellCheckEnablePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).SpellCheckEnablePropertyChanged();
        }

        /// <MetaDataID>{4ef71894-a041-4596-bffe-be2b80e2b7a5}</MetaDataID>
        private void SpellCheckEnablePropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpellCheckEnable)));
            WatermarkTextBox.SpellCheck.IsEnabled = SpellCheckEnable;
        }


        bool SpellCheckIsEnabled;




        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        public static readonly DependencyProperty WatermarkProperty =
                    DependencyProperty.Register(
                    "Watermark",
                    typeof(string),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata("", new PropertyChangedCallback(WatermarkPropertyChangedCallback)));

        public static void WatermarkPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).WatermarkPropertyChanged();
        }



        private void WatermarkPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Watermark)));
        }







        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
        public static readonly DependencyProperty TextWrappingProperty =
                    DependencyProperty.Register(
                    "TextWrapping",
                    typeof(TextWrapping),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata(TextWrapping.WrapWithOverflow, new PropertyChangedCallback(TextWrappingPropertyChangedCallback)));

        public static void TextWrappingPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).TextWrappingPropertyChanged();
        }

        private void TextWrappingPropertyChanged()
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextWrapping)));
        }

        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }
        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
                    DependencyProperty.Register(
                    "VerticalScrollBarVisibility",
                    typeof(ScrollBarVisibility),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata(ScrollBarVisibility.Hidden, new PropertyChangedCallback(VerticalScrollBarVisibilityPropertyChangedCallback)));

        public static void VerticalScrollBarVisibilityPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).VerticalScrollBarVisibilityPropertyChanged();
        }

        private void VerticalScrollBarVisibilityPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VerticalScrollBarVisibility)));
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
                   typeof(MultiligualWatermarkTextBox),
                   new PropertyMetadata(null,  new PropertyChangedCallback(TranslatorPropertyChangedCallback)));

        private static void TranslatorPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MultiligualWatermarkTextBox)
                (d as MultiligualWatermarkTextBox).UnTranslatedPropertyChanged();

        }

        public string TextFormat
        {
            get { return (string)GetValue(TextFormatProperty); }
            set
            {
                if (value == "") value = "{0}";
                SetValue(TextFormatProperty, value);
            }
        }
        public static readonly DependencyProperty TextFormatProperty =
                    DependencyProperty.Register(
                    "TextFormat",
                    typeof(string),
                    typeof(MultiligualWatermarkTextBox),
                    new PropertyMetadata("{0}"));

        public event PropertyChangedEventHandler PropertyChanged;

        public string FormattedText
        {
            get { return String.Format(TextFormat, Text); }
        }

        #endregion Properties




        private void TranslateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.GetObjectContext().FormObjectConnection.Culture != null)
            {
                using (OOAdvantech.CultureContext cultureContext = new OOAdvantech.CultureContext(this.GetObjectContext().FormObjectConnection.Culture, false))
                {
                    try
                    {
                        Text = Translator.TranslateString(Text, OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name);
                        UnTranslated = false;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnTranslated)));
                    }
                    catch (Exception error)
                    {
                    }
                }
            }
        }




    }
}

namespace WPFUIElementObjectBind
{
    /// <MetaDataID>{b976d125-3e86-4f12-986c-ec91002de372}</MetaDataID>
    public interface ITranslator
    {
        /// <MetaDataID>{e9e4cd8a-d26a-464a-86f8-350cca1d7a2f}</MetaDataID>
        string TranslateString(string strSource, string languageCode);
    }


}