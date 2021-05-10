using System.Linq;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using UIBaseEx;

namespace WPFUIElementObjectBind
{

    /// <summary>
    /// This class generates a Geometry from a block of text in a specific font, weight, etc.
    /// and renders it to WPF as a shape.
    /// </summary>
    /// <MetaDataID>{6deefa13-45ac-4bb1-9340-c7db0cebc793}</MetaDataID>
    class StrokeTextBlock : Shape
    {

        public StrokeTextBlock()
        {
            Fill = new SolidColorBrush(Colors.Black);
            //SetValue(FillProperty)
        }
        /// <summary>
        /// Data member that holds the generated geometry
        /// </summary>
        private Geometry _textGeometry;

        #region Depdendency Properties
        public static readonly DependencyProperty TextProperty =
                        DependencyProperty.Register("Text", typeof(string), typeof(StrokeTextBlock),
                            new FrameworkPropertyMetadata(string.Empty,
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty OriginPointProperty =
                        DependencyProperty.Register("Origin", typeof(Point), typeof(StrokeTextBlock),
                            new FrameworkPropertyMetadata(new Point(0, 0),
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(StrokeTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(StrokeTextBlock),
                                new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(StrokeTextBlock),
                               new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(StrokeTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(StrokeTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty UnderlineProperty =
             DependencyProperty.Register("Underline", typeof(bool), typeof(StrokeTextBlock),
                 new FrameworkPropertyMetadata(false,
                     FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                        OnPropertyChanged));


        public static readonly DependencyProperty OverLineProperty =
              DependencyProperty.Register("OverLine", typeof(bool), typeof(StrokeTextBlock),
                  new FrameworkPropertyMetadata(false,
                      FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                         OnPropertyChanged));

        public static readonly DependencyProperty AllCapsProperty =
             DependencyProperty.Register("AllCaps", typeof(bool), typeof(StrokeTextBlock),
                 new FrameworkPropertyMetadata(false,
                     FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                        OnPropertyChanged));

        #endregion

        #region Property Accessors

        [Bindable(true), Category("Appearance")]

        public bool Underline
        {
            get { return (bool)GetValue(UnderlineProperty); }
            set { SetValue(UnderlineProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        public bool OverLine
        {
            get { return (bool)GetValue(OverLineProperty); }
            set { SetValue(OverLineProperty, value); }
        }


        [Bindable(true), Category("AllCaps")]
        public bool AllCaps
        {
            get { return (bool)GetValue(AllCapsProperty); }
            set { SetValue(AllCapsProperty, value); }
        }


        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(PointConverter))]
        public Point Origin
        {
            get { return (Point)GetValue(OriginPointProperty); }
            set { SetValue(OriginPointProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [Localizability(LocalizationCategory.Font)]
        [TypeConverter(typeof(FontFamilyConverter))]
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontStretchConverter))]
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontStyleConverter))]
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontWeightConverter))]
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        /// <summary>
        /// This method is called to retrieve the geometry that defines the shape.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get { return _textGeometry ?? Geometry.Empty; }
        }

        /// <summary>
        /// This method is called when any of our dependency properties change - it
        /// changes the geometry so it is drawn properly.
        /// </summary>
        /// <param name="d">Depedency Object</param>
        /// <param name="e">EventArgs</param>
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StrokeTextBlock)d).CreateTextGeometry();
        }

        /// <summary>
        /// This method creates the text geometry.
        /// </summary>
        private void CreateTextGeometry()
        {
            string text = Text;
            if (text == null)
                text = "";

            if (AllCaps)
                text = text.ToUpper();
            var formattedText = new FormattedText(text, Thread.CurrentThread.CurrentUICulture,
                    FlowDirection.LeftToRight,
                        new Typeface(FontFamily, FontStyle, FontWeight, FontStretch),
                                  FontSize, Brushes.Black);

            if (Underline)
                formattedText.SetTextDecorations(GetTextDecoration(TextDecorationLocation.Underline));// TextDecorations.Underline);
            else if (OverLine)
                formattedText.SetTextDecorations(GetTextDecoration(TextDecorationLocation.OverLine));//TextDecorations.OverLine);
            if (text != null && string.IsNullOrWhiteSpace(text))
                _textGeometry = formattedText.BuildGeometry(new Point(Origin.X + FontSize, Origin.Y));
            else
                _textGeometry = formattedText.BuildGeometry(Origin);


        }

        TextDecorationCollection GetTextDecoration(TextDecorationLocation textDecorationLocation)
        {


            int fSz = 44; //fontsize, change this

            //double d = fSz / baseSize;

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(0.0, 1), false, false);
                context.PolyLineTo(new[] {
                new Point(1.5, 0),
                new Point(3, 3),
                new Point(4.5, 1.5),
                new Point(6, 0),
                new Point(7.5, 3),
                new Point(9, 1.5)
            }, true, true);
            }

            //geometry.Transform = new ScaleTransform(2, 2);

            var brushPattern = new GeometryDrawing
            {
                Pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 1),
                Geometry = geometry
            };

            var brush = new DrawingBrush(brushPattern)
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0, 0, 0.15, 1.2), //change this
                ViewportUnits = BrushMappingMode.RelativeToBoundingBox,
                Viewbox = new Rect(0, 0, 1, 1),
                ViewboxUnits = BrushMappingMode.RelativeToBoundingBox,
                Stretch = Stretch.Fill
            };

            TextDecoration squiggly = new System.Windows.TextDecoration();
            squiggly.Pen = new Pen(brush, 1.2); //fSz / (double)baseSize * 1.2
            squiggly.Location = textDecorationLocation;

            return new TextDecorationCollection() { squiggly };
            //FormattedText ft = new FormattedText("Hello, how are you?", System.Globalization.CultureInfo.CurrentCulture,
            //    FlowDirection.LeftToRight, new Typeface("Arial"), fSz, new SolidColorBrush(Colors.Green));
            //ft.SetTextDecorations(new TextDecorationCollection() { squiggly });


        }
    }


    /// <MetaDataID>{c0999055-8b5b-4227-b86d-60f76aac55a0}</MetaDataID>
    public class OutlineTextBlock : System.Windows.Controls.StackPanel
    {
        public OutlineTextBlock()
        {

            Orientation = System.Windows.Controls.Orientation.Horizontal;


            //Background = new SolidColorBrush(Colors.AliceBlue);
        }

        #region Depdendency Properties


        public static readonly DependencyProperty FillProperty =
                 DependencyProperty.Register("Fill", typeof(Brush), typeof(OutlineTextBlock),
                     new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black),
                         FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                            OnPropertyChanged));

        public static readonly DependencyProperty StrokeProperty =
         DependencyProperty.Register("Stroke", typeof(Brush), typeof(OutlineTextBlock),
             new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black),
                 FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnPropertyChanged));


        public static readonly DependencyProperty StrokeThicknessProperty =
         DependencyProperty.Register("StrokeThickness", typeof(double), typeof(OutlineTextBlock),
             new FrameworkPropertyMetadata(default(double),
                 FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                    OnPropertyChanged));



        public static readonly DependencyProperty TextProperty =
                        DependencyProperty.Register("Text", typeof(string), typeof(OutlineTextBlock),
                            new FrameworkPropertyMetadata(string.Empty,
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty OriginPointProperty =
                        DependencyProperty.Register("Origin", typeof(Point), typeof(OutlineTextBlock),
                            new FrameworkPropertyMetadata(new Point(0, 0),
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(OutlineTextBlock),
                                new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));


        public static readonly DependencyProperty FontSpacingProperty =
                        DependencyProperty.Register("FontSpacing", typeof(double), typeof(OutlineTextBlock),
                            new FrameworkPropertyMetadata(default(double),
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));


        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty UnderlineProperty =
             DependencyProperty.Register("Underline", typeof(bool), typeof(OutlineTextBlock),
                 new FrameworkPropertyMetadata(false,
                     FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                        OnPropertyChanged));


        public static readonly DependencyProperty OverLineProperty =
              DependencyProperty.Register("OverLine", typeof(bool), typeof(OutlineTextBlock),
                  new FrameworkPropertyMetadata(false,
                      FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                         OnPropertyChanged));

        public static readonly DependencyProperty AllCapsProperty =
             DependencyProperty.Register("AllCaps", typeof(bool), typeof(OutlineTextBlock),
                 new FrameworkPropertyMetadata(false,
                     FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                        OnPropertyChanged));

        #endregion

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OutlineTextBlock)d).CreateText();
        }
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            FormattedText ft = new FormattedText(text,
                                               System.Globalization.CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
        public static Size MeasureText(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface glyphTypeface;

            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
            }

            double totalWidth = 0;
            double height = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

                double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

                if (glyphHeight > height)
                {
                    height = glyphHeight;
                }

                totalWidth += width;
            }

            return new Size(totalWidth, height);
        }
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            double fontSpacing = FontSpacing;

            if (FontSpacing != 0 && ActualWidth > 0)
            {
                double fontSpaceDif = 1;
                try
                {
                    FontData font = new FontData() { AllCaps = this.AllCaps, FontFamilyName = this.FontFamily.FamilyNames.Values.FirstOrDefault(), FontSize = this.FontSize, FontSpacing = this.FontSpacing, FontStyle = this.FontStyle.ToString(), FontWeight = this.FontWeight.ToString() };
                    Size textSize = font.MeasureText(Text);//, FontFamily, FontStyle, FontWeight, FontStretches.Normal, FontSize);
                    fontSpaceDif = textSize.Width - ActualWidth;
                }
                catch (System.Exception error)
                {
                }
                if (fontSpaceDif > 3 || fontSpaceDif < -3)
                {

                    fontSpacing += fontSpaceDif / (Text.Length - 1);
                    StrokeTextBlock outlineTextBlock = null;
                    Children.Clear();
                    foreach (char _char in Text)
                    {

                        Size size = MeasureText(" ", FontFamily, FontStyle, FontWeight, FontStretches.Normal, FontSize);
                        if (_char == ' ' && outlineTextBlock != null)
                            outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + size.Width + fontSpacing, outlineTextBlock.Margin.Bottom);

                        else if (_char == ' ' && outlineTextBlock == null)
                            outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left + size.Width, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + fontSpacing, outlineTextBlock.Margin.Bottom);
                        else if (outlineTextBlock != null)
                            outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + fontSpacing, outlineTextBlock.Margin.Bottom);

                        outlineTextBlock = new StrokeTextBlock();
                        outlineTextBlock.FontFamily = FontFamily;
                        outlineTextBlock.FontSize = FontSize;
                        outlineTextBlock.FontStretch = FontStretch;
                        outlineTextBlock.FontStyle = FontStyle;
                        outlineTextBlock.FontWeight = FontWeight;
                        outlineTextBlock.AllCaps = AllCaps;
                        outlineTextBlock.Fill = Fill;
                        outlineTextBlock.Stroke = Stroke;
                        outlineTextBlock.StrokeThickness = StrokeThickness;
                        outlineTextBlock.Underline = Underline;
                        outlineTextBlock.OverLine = OverLine;
                        outlineTextBlock.Text = "" + _char;
                        Children.Add(outlineTextBlock);
                    }
                }
            }
        }
        private void CreateText()
        {
            FontData font = new FontData() { AllCaps = this.AllCaps, FontFamilyName = this.FontFamily.FamilyNames.Values.FirstOrDefault(), FontSize = this.FontSize, FontSpacing = this.FontSpacing, FontStyle = this.FontStyle.ToString(), FontWeight = this.FontWeight.ToString() };


            Children.Clear();
            if (string.IsNullOrWhiteSpace(Text))
                return;
            StrokeTextBlock outlineTextBlock = null;
            if (FontSpacing == 0)
            {
                //Margin = new Thickness(5, 5, 5, 5);
                outlineTextBlock = new StrokeTextBlock();
                outlineTextBlock.FontFamily = FontFamily;
                outlineTextBlock.FontSize = FontSize;
                outlineTextBlock.FontStretch = FontStretch;
                outlineTextBlock.FontStyle = FontStyle;
                outlineTextBlock.FontWeight = FontWeight;
                outlineTextBlock.AllCaps = AllCaps;
                outlineTextBlock.Fill = Fill;
                outlineTextBlock.Stroke = Stroke;
                outlineTextBlock.StrokeThickness = StrokeThickness;
                outlineTextBlock.Underline = Underline;
                outlineTextBlock.OverLine = OverLine;
                outlineTextBlock.Text = Text;

                Children.Add(outlineTextBlock);
                UpdateLayout();
            }
            else
            {


                int i = 0;

                foreach (char _char in Text)
                {

                    Size size = MeasureText(" ", FontFamily, FontStyle, FontWeight, FontStretches.Normal, FontSize);
                    if (_char == ' ' && outlineTextBlock != null)
                        outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + size.Width + FontSpacing, outlineTextBlock.Margin.Bottom);

                    else if (_char == ' ' && outlineTextBlock == null)
                        outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left + size.Width, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + FontSpacing, outlineTextBlock.Margin.Bottom);
                    else if (outlineTextBlock != null)
                        outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + FontSpacing, outlineTextBlock.Margin.Bottom);

                    outlineTextBlock = new StrokeTextBlock();
                    outlineTextBlock.FontFamily = FontFamily;
                    outlineTextBlock.FontSize = FontSize;
                    outlineTextBlock.FontStretch = FontStretch;
                    outlineTextBlock.FontStyle = FontStyle;
                    outlineTextBlock.FontWeight = FontWeight;
                    outlineTextBlock.AllCaps = AllCaps;
                    outlineTextBlock.Fill = Fill;
                    outlineTextBlock.Stroke = Stroke;
                    outlineTextBlock.StrokeThickness = StrokeThickness;
                    outlineTextBlock.Underline = Underline;
                    outlineTextBlock.OverLine = OverLine;
                    outlineTextBlock.Text = "" + _char;
                    Children.Add(outlineTextBlock);
                }
                UpdateLayout();
                if (FontSpacing != 0 && ActualWidth > 0)
                {
                    double fontSpacing = FontSpacing;
                    double fontSpaceDif = 1;
                    try
                    {
                        Size textSize = font.MeasureText(Text);//, FontFamily, FontStyle, FontWeight, FontStretches.Normal, FontSize);
                        fontSpaceDif = textSize.Width - ActualWidth;
                        if (fontSpaceDif < 0)
                            fontSpaceDif = 0;
                    }
                    catch (System.Exception error)
                    {
                    }

                    if (fontSpaceDif > 3 || fontSpaceDif < -3)
                    {
                        fontSpacing += fontSpaceDif / (Text.Length - 1);

                        Children.Clear();
                        foreach (char _char in Text)
                        {

                            Size size = MeasureText(" ", FontFamily, FontStyle, FontWeight, FontStretches.Normal, FontSize);
                            if (_char == ' ' && outlineTextBlock != null)
                                outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + size.Width + fontSpacing, outlineTextBlock.Margin.Bottom);

                            else if (_char == ' ' && outlineTextBlock == null)
                                outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left + size.Width, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + fontSpacing, outlineTextBlock.Margin.Bottom);
                            else if (outlineTextBlock != null)
                                outlineTextBlock.Margin = new Thickness(outlineTextBlock.Margin.Left, outlineTextBlock.Margin.Top, outlineTextBlock.Margin.Right + fontSpacing, outlineTextBlock.Margin.Bottom);

                            outlineTextBlock = new StrokeTextBlock();
                            outlineTextBlock.FontFamily = FontFamily;
                            outlineTextBlock.FontSize = FontSize;
                            outlineTextBlock.FontStretch = FontStretch;
                            outlineTextBlock.FontStyle = FontStyle;
                            outlineTextBlock.FontWeight = FontWeight;
                            outlineTextBlock.AllCaps = AllCaps;
                            outlineTextBlock.Fill = Fill;
                            outlineTextBlock.Stroke = Stroke;
                            outlineTextBlock.StrokeThickness = StrokeThickness;
                            outlineTextBlock.Underline = Underline;
                            outlineTextBlock.OverLine = OverLine;
                            outlineTextBlock.Text = "" + _char;
                            Children.Add(outlineTextBlock);
                        }
                    }
                }
            }

        }


        #region Property Accessors

        [Bindable(true), Category("Appearance")]

        public bool Underline
        {
            get { return (bool)GetValue(UnderlineProperty); }
            set { SetValue(UnderlineProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        public bool OverLine
        {
            get { return (bool)GetValue(OverLineProperty); }
            set { SetValue(OverLineProperty, value); }
        }


        [Bindable(true), Category("AllCaps")]
        public bool AllCaps
        {
            get { return (bool)GetValue(AllCapsProperty); }
            set { SetValue(AllCapsProperty, value); }
        }


        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(PointConverter))]
        public Point Origin
        {
            get { return (Point)GetValue(OriginPointProperty); }
            set { SetValue(OriginPointProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [Localizability(LocalizationCategory.Font)]
        [TypeConverter(typeof(FontFamilyConverter))]
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }


        [Bindable(true), Category("Appearance")]
        //[TypeConverter(typeof(FontSizeConverter))]
        [Localizability(LocalizationCategory.None)]
        public double FontSpacing
        {
            get { return (double)GetValue(FontSpacingProperty); }
            set { SetValue(FontSpacingProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontStretchConverter))]
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontStyleConverter))]
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        [TypeConverter(typeof(FontWeightConverter))]
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        [Bindable(true), Category("Appearance")]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }


        public Brush Fill
        {
            get { return GetValue(FillProperty) as Brush; }
            set { SetValue(FillProperty, value); }
        }

        public Brush Stroke
        {
            get { return GetValue(StrokeProperty) as Brush; }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }


        #endregion


    }

}
