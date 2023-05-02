using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
#if !DeviceDotNet
using System.Windows.Media;
#else
using Xamarin.Forms;
#endif
using OOAdvantech.MetaDataRepository;


namespace UIBaseEx
{
    /// <MetaDataID>{195500ba-0d86-45f0-8618-55cdf3f41802}</MetaDataID>
    [BackwardCompatibilityID("{032cf3b1-dc37-4fd7-b806-62dc7c512dda}")]
    [Persistent()]
    public struct FontData
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <exclude>Excluded</exclude>
        string _Uri;
        /// <MetaDataID>{b34aaa7a-87d2-44ef-82a7-c886b5987515}</MetaDataID>
        [PersistentMember(nameof(_Uri))]
        [BackwardCompatibilityID("+19")]
        public string Uri
        {
            get => _Uri;
            set
            {
                if (_Uri != value)
                    _Uri = value;
            }
        }


#if !DeviceDotNet
        /// <MetaDataID>{76a657d0-547d-46a5-9f23-1606cf7c7f46}</MetaDataID>
        public static IHtmlView HtmlView;
#endif


        /// <exclude>Excluded</exclude>
        bool _Underline;

        /// <MetaDataID>{0154cf8e-fdec-4d59-afb3-5264bc9d14b1}</MetaDataID>
        [PersistentMember(nameof(_Underline))]
        [BackwardCompatibilityID("+17")]
        public bool Underline
        {
            get
            {
                return _Underline;
            }
            set
            {

                if (_Underline != value)
                    _Underline = value;

            }
        }

        /// <exclude>Excluded</exclude>
        bool _Overline;
        /// <MetaDataID>{47d7edbc-cb9f-47fa-9cad-543c01df38e6}</MetaDataID>
        [PersistentMember(nameof(_Overline))]
        [BackwardCompatibilityID("+18")]
        public bool Overline
        {
            get
            {
                return _Overline;
            }
            set
            {
                if (_Overline != value)
                {
                    _Overline = value;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _FontSpacing;
        /// <MetaDataID>{ef3ea834-cb7d-4a28-9374-122d916e3d4b}</MetaDataID>
        [PersistentMember(nameof(_FontSpacing))]
        [BackwardCompatibilityID("+15")]
        public double FontSpacing
        {
            get
            {
                return _FontSpacing;
            }

            set
            {
                _FontSpacing = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _ShadowColor;


        /// <MetaDataID>{2c574792-bfe3-4780-af08-eb028f243816}</MetaDataID>
        [PersistentMember(nameof(_ShadowColor))]
        [BackwardCompatibilityID("+14")]
        public string ShadowColor
        {
            get
            {
                return _ShadowColor;
            }

            set
            {
                _ShadowColor = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _BlurRadius;
        /// <MetaDataID>{3ef32454-6630-4cb3-bd43-32f50bd82d48}</MetaDataID>
        [PersistentMember(nameof(_BlurRadius))]
        [BackwardCompatibilityID("+13")]
        public double BlurRadius
        {
            get
            {
                return _BlurRadius;
            }

            set
            {
                _BlurRadius = value;
            }
        }

        /// <exclude>Excluded</exclude> 
        double _ShadowYOffset;

        /// <MetaDataID>{792046d1-35dc-4b62-8089-6c1344ff9010}</MetaDataID>
        [PersistentMember(nameof(_ShadowYOffset))]
        [BackwardCompatibilityID("+12")]
        public double ShadowYOffset
        {
            get
            {
                return _ShadowYOffset;
            }

            set
            {
                _ShadowYOffset = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _ShadowXOffset;

        /// <MetaDataID>{a6dfbbd6-698b-4f09-9ae4-109a798540da}</MetaDataID>
        [PersistentMember(nameof(_ShadowXOffset))]
        [BackwardCompatibilityID("+11")]
        public double ShadowXOffset
        {
            get
            {
                return _ShadowXOffset;
            }

            set
            {
                _ShadowXOffset = value;
            }
        }


        /// <exclude>Excluded</exclude>
        double _StrokeThickness;

        /// <MetaDataID>{58b8a233-4b74-4971-8b4b-d1b5e6491b13}</MetaDataID>
        [PersistentMember(nameof(_StrokeThickness))]
        [BackwardCompatibilityID("+10")]
        public double StrokeThickness
        {
            get
            {
                return _StrokeThickness;
            }

            set
            {
                _StrokeThickness = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _StrokeFill;
        /// <MetaDataID>{2cbf9b18-34de-4ddb-a71a-65b149eb1175}</MetaDataID>
        [PersistentMember(nameof(_StrokeFill))]
        [BackwardCompatibilityID("+9")]
        public string StrokeFill
        {
            get
            {
                return _StrokeFill;
            }

            set
            {
                _StrokeFill = value;
            }
        }

        /// <exclude>Excluded</exclude>
        double _FontSize;
        /// <MetaDataID>{7c86d356-60a4-40b6-bbb4-be07d049a90f}</MetaDataID>
        [PersistentMember(nameof(_FontSize))]
        [BackwardCompatibilityID("+4")]
        public double FontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                _FontSize = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _FontFamilyName;
        /// <MetaDataID>{5ef08afa-7d55-4753-a3a2-618391e917d6}</MetaDataID>
        [PersistentMember(nameof(_FontFamilyName))]
        [BackwardCompatibilityID("+1")]
        public string FontFamilyName
        {
            get
            {
                return _FontFamilyName;
            }
            set
            {
                _FontFamilyName = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _FontWeight;
        /// <MetaDataID>{5d426456-b1f0-4e1b-bd56-67106b91e1e9}</MetaDataID>
        [PersistentMember(nameof(_FontWeight))]
        [BackwardCompatibilityID("+2")]
        public string FontWeight
        {
            get
            {
                if (string.IsNullOrEmpty(_FontWeight))
                    _FontWeight = "Normal";
                return _FontWeight;
            }
            set
            {
                _FontWeight = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _FontStyle;
        /// <MetaDataID>{5ca1a878-bcf9-417a-b450-e3a6d973aeaf}</MetaDataID>
        [PersistentMember(nameof(_FontStyle))]
        [BackwardCompatibilityID("+3")]
        public string FontStyle
        {
            get
            {
                if (string.IsNullOrEmpty(_FontStyle))
                    _FontStyle = "Normal";
                return _FontStyle;
            }
            set
            {
                _FontStyle = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _Foreground;
        /// <MetaDataID>{fc25a2f7-27d3-424f-afb1-6498eb8c1d17}</MetaDataID>
        [PersistentMember(nameof(_Foreground))]
        [BackwardCompatibilityID("+8")]
        public string Foreground
        {
            get
            {
                if (string.IsNullOrEmpty(_Foreground))
                    _Foreground = "#FF000000";
                return _Foreground;
            }

            set
            {
                if (_Foreground != value)
                    _Foreground = value;
            }
        }




        /// <exclude>Excluded</exclude>
        bool _Stroke;

        /// <MetaDataID>{c45f0206-c163-4a5c-87c4-40d1c7e7903a}</MetaDataID>
        [PersistentMember(nameof(_Stroke))]
        [BackwardCompatibilityID("+5")]
        public bool Stroke
        {
            get
            {
                return _Stroke;
            }

            set
            {

                if (_Stroke != value)
                    _Stroke = value;
            }
        }

        /// <exclude>Excluded</exclude>
        bool _AllCaps;

        /// <MetaDataID>{bdb5bef6-519e-4a91-9038-d99e62002e66}</MetaDataID>
        [PersistentMember(nameof(_AllCaps))]
        [BackwardCompatibilityID("+6")]
        public bool AllCaps
        {
            get
            {
                return _AllCaps;
            }

            set
            {
                if (_AllCaps != value)
                    _AllCaps = value;
            }
        }
        /// <exclude>Excluded</exclude>
        bool _Shadow;

        /// <MetaDataID>{d074b06b-8a50-4a64-9348-0f880c616da2}</MetaDataID>
        [PersistentMember(nameof(_Shadow))]
        [BackwardCompatibilityID("+7")]
        public bool Shadow
        {
            get
            {
                return _Shadow;
            }

            set
            {
                if (_Shadow != value)
                    _Shadow = value;
            }
        }


        /// <MetaDataID>{a1232e9e-aae9-4cca-9f0c-676fb191be1e}</MetaDataID>
        public override int GetHashCode()
        {
            int num = -1162279000;
            num = (-1521134295 * num) + GetHashCode(AllCaps);
            num = (-1521134295 * num) + GetHashCode(FontFamilyName);
            num = (-1521134295 * num) + GetHashCode(FontSize);
            num = (-1521134295 * num) + GetHashCode(FontSpacing);
            num = (-1521134295 * num) + GetHashCode(FontWeight);
            num = (-1521134295 * num) + GetHashCode(Foreground);
            num = (-1521134295 * num) + GetHashCode(Shadow);
            num = (-1521134295 * num) + GetHashCode(Stroke);
            num = (-1521134295 * num) + GetHashCode(ShadowColor);
            num = (-1521134295 * num) + GetHashCode(ShadowXOffset);
            num = (-1521134295 * num) + GetHashCode(ShadowXOffset);
            num = (-1521134295 * num) + GetHashCode(ShadowYOffset);
            num = (-1521134295 * num) + GetHashCode(StrokeFill);
            num = (-1521134295 * num) + GetHashCode(StrokeThickness);
            num = (-1521134295 * num) + GetHashCode(BlurRadius);
            return num;
        }

        /// <MetaDataID>{43350dff-00e4-4559-9715-bbe7da21bc10}</MetaDataID>
        private int GetHashCode(object partValue)
        {
            if (partValue == null)
                return 0;
            else
                return partValue.GetHashCode();
        }

        /// <MetaDataID>{59044d03-de1d-4245-a15d-5651ccf80a4a}</MetaDataID>
        public static bool operator ==(FontData left, FontData right)
        {
            if (left.AllCaps == right.AllCaps &&
                left.FontFamilyName == right.FontFamilyName &&
                left.FontSize == right.FontSize &&
                left.FontSpacing == right.FontSpacing &&
                left.FontStyle == right.FontStyle &&
                left.FontWeight == right.FontWeight &&
                left.Foreground == right.Foreground &&
                left.Shadow == right.Shadow &&
                left.Stroke == right.Stroke &&
                left.ShadowColor == right.ShadowColor &&
                left.ShadowXOffset == right.ShadowXOffset &&
                left.ShadowYOffset == right.ShadowYOffset &&
                left.StrokeFill == left.StrokeFill &&
                left.StrokeThickness == left.StrokeThickness &&
                left.BlurRadius == right.BlurRadius)
                return true;
            else
                return false;
        }
        /// <MetaDataID>{995a3666-704e-4b33-933b-fd18f889906d}</MetaDataID>
        public static bool operator !=(FontData left, FontData right)
        {
            return !(left == right);
        }



#if !DeviceDotNet

        /// <MetaDataID>{22e68105-c87e-495d-a8f1-b810668675ba}</MetaDataID>
        public static System.Collections.Generic.Dictionary<string, FontFamily> FontFamilies = new System.Collections.Generic.Dictionary<string, FontFamily>();

        /// <MetaDataID>{6a985711-b270-4799-9a37-72286de817ff}</MetaDataID>
        public double GetTextMedline(string text)
        {
            if (text == null)
                text = "";

            if (AllCaps)
                text = text.ToUpper();

            FontFamily fontFamily = FontData.FontFamilies[FontFamilyName];
            FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
            FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
            FontStretch fontStretch = FontStretches.Normal;
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

            double fontSize = FontSize;
            FormattedText formatted_text = new FormattedText(text,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              typeface,
                                              fontSize,
                                              Brushes.Black);

            double y_top, y_baseline, y_caps, y_lowercase,
                y_descent, y_bottom, x_origin, x_start, x_end, x_right;
            GetTextMetrics(0, 0, formatted_text, typeface, fontSize,
                out y_top, out y_baseline, out y_caps, out y_lowercase,
                out y_descent, out y_bottom, out x_origin, out x_start,
                out x_end, out x_right);

            //view FontMeasure.jpg for out parameters

            return y_caps + ((y_descent - y_caps) / 2);
        }

        /// <MetaDataID>{b314b70f-4b95-41e6-a498-8f442137c60c}</MetaDataID>
        public double GetTextCapsLine(string text)
        {
            if (text == null)
                text = "";

            if (AllCaps)
                text = text.ToUpper();

            FontFamily fontFamily = FontData.FontFamilies[FontFamilyName];
            FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
            FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
            FontStretch fontStretch = FontStretches.Normal;
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

            double fontSize = FontSize;
            FormattedText formatted_text = new FormattedText(text,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              typeface,
                                              fontSize,
                                              Brushes.Black);

            double y_top, y_baseline, y_caps, y_lowercase,
                y_descent, y_bottom, x_origin, x_start, x_end, x_right;
            GetTextMetrics(0, 0, formatted_text, typeface, fontSize,
                out y_top, out y_baseline, out y_caps, out y_lowercase,
                out y_descent, out y_bottom, out x_origin, out x_start,
                out x_end, out x_right);

            //view FontMeasure.jpg for out parameters

            return y_caps;
        }

        /// <MetaDataID>{f318eb47-58e6-47e3-afee-0dd9ee14a22c}</MetaDataID>
        public double GetTextDescenLine(string text)
        {
            if (text == null)
                text = "";

            if (AllCaps)
                text = text.ToUpper();

            FontFamily fontFamily = FontData.FontFamilies[FontFamilyName];
            FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
            FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
            FontStretch fontStretch = FontStretches.Normal;
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

            double fontSize = FontSize;
            FormattedText formatted_text = new FormattedText(text,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              typeface,
                                              fontSize,
                                              Brushes.Black);

            double y_top, y_baseline, y_caps, y_lowercase,
                y_descent, y_bottom, x_origin, x_start, x_end, x_right;
            GetTextMetrics(0, 0, formatted_text, typeface, fontSize,
                out y_top, out y_baseline, out y_caps, out y_lowercase,
                out y_descent, out y_bottom, out x_origin, out x_start,
                out x_end, out x_right);

            //view FontMeasure.jpg for out parameters

            return y_descent;
        }



        // Get information about the text's dimensions.
        /// <MetaDataID>{73766b27-0b1d-4f33-b520-2ca2e5946c3e}</MetaDataID>
        public void GetTextMetrics(double x, double y,
                    FormattedText formatted_text, Typeface typeface,
                    double em_size,
                    out double y_top, out double y_baseline,
                    out double y_caps, out double y_lowercase,
                    out double y_descent, out double y_bottom,
                    out double x_origin, out double x_start,
                    out double x_end, out double x_right)
        {
            y_top = y;
            y_bottom = y_top + formatted_text.Height;
            y_baseline = y_top + formatted_text.Baseline;
            y_caps = y_baseline - typeface.CapsHeight * em_size;
            y_lowercase = y_baseline - typeface.XHeight * em_size;
            y_descent = y_bottom + formatted_text.OverhangAfter;

            x_origin = x;
            x_start = x_origin + formatted_text.OverhangLeading;
            x_right = x_origin + formatted_text.Width;
            x_end = x_right - formatted_text.OverhangTrailing;


            //view FontMeasure.jpg for out parameters
        }
        /// <MetaDataID>{97492902-7d5f-4fda-944e-4e748b938d00}</MetaDataID>
        public double GetHtml5TextBaseLine(string text)
        {

            if (text == null)
                text = "";

            if (AllCaps)
                text = text.ToUpper();


            lock (htmlFontFamilies)
            {
                bool call = true;
                if (call && HtmlView != null)
                {
                    FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
                    FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
                    FontStretch fontStretch = FontStretches.Normal;
                    double fontSize = FontSize;
                    var start = System.DateTime.Now;
                    object result = HtmlView.InvockeJSMethod("GetTextMetrics", new object[] { text, FontFamilyName, fontSize, fontStyle.ToString(), fontWeight.ToString() });
                    if (!htmlFontFamilies.ContainsKey(FontFamilyName))
                    {
                        result = HtmlView.InvockeJSMethod("GetTextMetrics", new object[] { text, FontFamilyName, fontSize, fontStyle.ToString(), fontWeight.ToString() });
                        htmlFontFamilies[FontFamilyName] = true;
                    }

                    var res = (System.DateTime.Now - start).TotalMilliseconds;
                    dynamic dynamicObject = result as System.Dynamic.ExpandoObject;
                    if (dynamicObject != null)
                    {
                        TextMetrics textMetrics = new TextMetrics();

                        textMetrics.Height = dynamicObject.Height;
                        textMetrics.Width = dynamicObject.Width;
                        textMetrics.Ascent = dynamicObject.Ascent;
                        textMetrics.Baseline = dynamicObject.Baseline;
                        textMetrics.Bottom = dynamicObject.Bottom;
                        textMetrics.CapHeight = dynamicObject.CapHeight;
                        textMetrics.Descent = dynamicObject.Descent;
                        textMetrics.Tittle = dynamicObject.Tittle;
                        textMetrics.Top = dynamicObject.Top;

                        return textMetrics.Baseline - textMetrics.Top;

                    }
                }
            }

            return 0;
        }

        /// <MetaDataID>{47fa28b4-def8-44bd-8971-c5cc27581534}</MetaDataID>
        public double GetTextBaseLine(string text)
        {
            if (text == null)
                text = "";

            if (AllCaps)
                text = text.ToUpper();
            FontFamily fontFamily = FontData.FontFamilies[FontFamilyName];
            FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
            FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
            FontStretch fontStretch = FontStretches.Normal;
            double fontSize = FontSize;
            FormattedText ft = new FormattedText(text,
                                            System.Globalization.CultureInfo.CurrentCulture,
                                              FlowDirection.LeftToRight,
                                              new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                              fontSize,
                                              Brushes.Black);
            return ft.Baseline;
        }

        /// <MetaDataID>{80a4948d-38c1-44e2-8a28-789112a2d91b}</MetaDataID>
        static Dictionary<string, bool> htmlFontFamilies = new Dictionary<string, bool>();

        /// <MetaDataID>{470441e9-75fc-4243-ac40-e54c2a89060c}</MetaDataID>
        static Dictionary<string, bool> htmlLetterSpaceFontFamilies = new Dictionary<string, bool>();
        /// <MetaDataID>{1763464f-7908-40ed-aa3d-904b3b8ef711}</MetaDataID>
        public double Html5GetFontSpacingCorrection(string text, double textWidth)
        {
            if (text == null)
                text = "";
            if (AllCaps)
                text = text.ToUpper();
            lock (htmlFontFamilies)
            {
                bool call = true;
                if (call && HtmlView != null)
                {
                    FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
                    FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
                    FontStretch fontStretch = FontStretches.Normal;
                    double fontSize = FontSize;

                    var start = System.DateTime.Now;
                    object result = HtmlView.InvockeJSMethod("GetLetterSpacingCorrection", new object[] { text, FontFamilyName, fontSize, fontStyle.ToString(), fontWeight.ToString(), textWidth });
                    if (!htmlLetterSpaceFontFamilies.ContainsKey(FontFamilyName))
                    {
                        result = HtmlView.InvockeJSMethod("GetLetterSpacingCorrection", new object[] { text, FontFamilyName, fontSize, fontStyle.ToString(), fontWeight.ToString(), textWidth });
                        htmlLetterSpaceFontFamilies[FontFamilyName] = true;
                    }

                    var res = (System.DateTime.Now - start).TotalMilliseconds;
                    if (result is double)
                        return (double)result;
                }

                return 0;
            }

        }

        /// <MetaDataID>{deeddd19-81e1-4c02-aa5e-c098bdae7921}</MetaDataID>
        public Size Html5MeasureText(string text)
        {
            if (text == null)
                text = "";
            if (AllCaps)
                text = text.ToUpper();
            lock (htmlFontFamilies)
            {
                bool call = true;
                if (call && HtmlView != null)
                {
                    FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
                    FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
                    FontStretch fontStretch = FontStretches.Normal;
                    double fontSize = FontSize;

                    var start = System.DateTime.Now;
                    object result = HtmlView.InvockeJSMethod("GetTextMetrics", new object[] { text, FontFamilyName, fontSize, fontStyle.ToString(), fontWeight.ToString() });
                    if (!htmlFontFamilies.ContainsKey(FontFamilyName))
                    {
                        result = HtmlView.InvockeJSMethod("GetTextMetrics", new object[] { text, FontFamilyName, fontSize, fontStyle.ToString(), fontWeight.ToString() });
                        htmlFontFamilies[FontFamilyName] = true;
                    }

                    var res = (System.DateTime.Now - start).TotalMilliseconds;
                    dynamic dynamicObject = result as System.Dynamic.ExpandoObject;
                    if (dynamicObject != null)
                    {
                        TextMetrics textMetrics = new TextMetrics();

                        try
                        {
                            textMetrics.Width = dynamicObject.Width;
                            textMetrics.Height = dynamicObject.Height;

                            textMetrics.Ascent = dynamicObject.Ascent;
                            textMetrics.Baseline = dynamicObject.Baseline;
                            textMetrics.Bottom = dynamicObject.Bottom;
                            textMetrics.CapHeight = dynamicObject.CapHeight;
                            textMetrics.Descent = dynamicObject.Descent;
                            textMetrics.Tittle = dynamicObject.Tittle;
                            textMetrics.Top = dynamicObject.Top;
                        }
                        catch (Exception error)
                        {
                            throw;
                        }
                        return new Size(textMetrics.Width, textMetrics.Height);

                    }
                }

                return new Size(0, 0);
            }

        }


        /// <MetaDataID>{e75559b1-c248-4a15-881f-e3ba85d59783}</MetaDataID>
        public Size MeasureText(string text)
        {
            if (text == null)
                text = "";
            if (AllCaps)
                text = text.ToUpper();

            var descenLine = GetTextDescenLine(text);

            FontFamily fontFamily = FontData.FontFamilies[FontFamilyName];
            FontStyle fontStyle = (FontStyle)new FontStyleConverter().ConvertFromString(FontStyle);
            FontWeight fontWeight = (FontWeight)new FontWeightConverter().ConvertFromString(FontWeight);
            FontStretch fontStretch = FontStretches.Normal;
            double fontSize = FontSize;
            Typeface typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
            GlyphTypeface glyphTypeface;

            if (text.Trim().Length > 0)
            {
                Size size = MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
                if (text.Length - 1 > 0)
                    size = new Size(size.Width + ((text.Length - 1) * FontSpacing), descenLine);


                return size;
            }
            //return  MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
            {
                return MeasureTextSize(text, fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
            }

            double totalWidth = 0;
            double height = 0;

            for (int n = 0; n < text.Length; n++)
            {
                if (glyphTypeface.CharacterToGlyphMap.ContainsKey(text[n]))
                {
                    ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];

                    double width = glyphTypeface.AdvanceWidths[glyphIndex] * fontSize;

                    double glyphHeight = glyphTypeface.AdvanceHeights[glyphIndex] * fontSize;

                    if (glyphHeight > height)
                        height = glyphHeight;
                    totalWidth = totalWidth + width + FontSpacing;
                }
                else
                {
                    var charSize = MeasureText(" ");// MeasureTextSize(text[n].ToString(), fontFamily, fontStyle, fontWeight, fontStretch, fontSize);
                    if (charSize.Height > height)
                        height = charSize.Height;
                    totalWidth = totalWidth + charSize.Width + FontSpacing;
                }


            }
            totalWidth -= FontSpacing;
            if (totalWidth < 0)
                totalWidth = 0;



            return new Size(totalWidth, height);
        }
        /// <MetaDataID>{8f4b31ad-41b8-405f-b4c3-d34b08d1e7dc}</MetaDataID>
        public static Size MeasureTextSize(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            if (text == null)
                text = "";

            FormattedText ft = new FormattedText(text,
                                               System.Globalization.CultureInfo.CurrentCulture,
                                                 FlowDirection.LeftToRight,
                                                 new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),
                                                 fontSize,
                                                 Brushes.Black);
            return new Size(ft.Width, ft.Height);
        }
#endif

    }


    /// <MetaDataID>{4c2f215a-4c1a-4240-8d7c-18befb7dc30b}</MetaDataID>
    struct TextMetrics
    {
        public double CapHeight;
        public double Baseline;
        public double XHeight;
        public double Descent;
        public double Bottom;
        public double Ascent;
        public double Tittle;
        public double Top;
        public double Height;
        public double Width;
    }
}
