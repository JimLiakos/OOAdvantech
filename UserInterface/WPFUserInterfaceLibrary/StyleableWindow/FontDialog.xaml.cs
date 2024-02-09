using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace StyleableWindow
{
    /// <summary>
    /// Interaction logic for FontDialog.xaml
    /// </summary>
    /// <MetaDataID>{e0dfb476-37ea-403d-8262-a8f3ad1f0252}</MetaDataID>
    public partial class FontDialog : StyleableWindow.Window
    {
        delegate void InitFontsHandle();

        public static List<FontFamily> FontFamilies = new List<FontFamily>();
        public static Dictionary<string, string> ActualFontFamiliesNames = new Dictionary<string, string>();
        public static void InitFonts()
        {

            new InitFontsHandle(InitFontsAsynch).BeginInvoke(new AsyncCallback(InitFontsCallBack), null);

        }
        static void InitFontsCallBack(IAsyncResult ar)
        {

        }
        static void InitFontsAsynch()
        {
            string fotnsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Microneme\DontWaitWater\FontFiles\";

            FontFamilies = System.Windows.Media.Fonts.GetFontFamilies(fotnsPath).ToList();


            System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Microneme\DontWaitWater\FontFiles\");
            foreach (var fInfo in directoryInfo.GetFiles())
            {
                try
                {
                    var fontFamilies = System.Windows.Media.Fonts.GetFontFamilies(fInfo.FullName).ToList();
                    PrivateFontCollection fontCollection = new PrivateFontCollection();
                    fontCollection.AddFontFile(fInfo.FullName);
                    if (fontCollection.Families.Length > 0)
                    {
                        string familyName = fontCollection.Families[0].Name;
                        string wpfFontFamilyName = null;
                        if (fontFamilies[0].ToString().IndexOf("#") != -1)
                            wpfFontFamilyName = fontFamilies[0].ToString().Substring(fontFamilies[0].ToString().IndexOf("#") + 1);
                        else
                            wpfFontFamilyName = fontFamilies[0].ToString();



                        FontFamily fontFamily = new FontFamily(directoryInfo.FullName + "#" + wpfFontFamilyName  + ","+ directoryInfo.FullName + "#Noto Serif");
                        ActualFontFamiliesNames[familyName] = wpfFontFamilyName;
                        FontData.FontFamilies[familyName] = fontFamily;
                        //FontData.FontFamilies[familyName] = (from fontFamily in FontFamilies where fontFamily.ToString().Substring(fontFamily.ToString().IndexOf("#") + 1) == wpfFontFamilyName select fontFamily).FirstOrDefault();
                    }
                }
                catch (Exception error)
                {
                }
            }

        }

        public FontDialog()
        {
            InitializeComponent();

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Activated += FontDialog_Activated;
        }

        private void FontDialog_Activated(object sender, EventArgs e)
        {
            if (this.GetDataContextObject<FontPresantation>() == null)
                this.GetObjectContext().SetContextInstance(new FontPresantation());
        }


    }
    /// <MetaDataID>{2c2fbcd0-f6f3-4e70-a37c-c3de77e07011}</MetaDataID>
    public class FontPresantation : MarshalByRefObject, System.ComponentModel.INotifyPropertyChanged
    {
        public double FontShadowDepth
        {
            get
            {
                return SelectedFontSizeInpx / 10;
            }

        }

        string _TitlebarText = "Fonts";
        public string TitlebarText
        {
            get
            {
                return _TitlebarText;
            }
            set
            {
                _TitlebarText = value;
            }

        }

        string _FontSize = "30pt";
        public string SelectedFontSize
        {
            get
            {
                return _FontSize;
            }
            set
            {
                if (_FontSize != value && value != null && new FontSizeConverter().IsValid(value))
                {
                    _FontSize = value;
                    _Font.FontSize = SelectedFontSizeInpx;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontSizeInpx)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontSize)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontShadowDepth)));
                }
            }
        }

        public double SelectedFontSizeInpx
        {
            get
            {
                return (double)(new FontSizeConverter().ConvertFrom(SelectedFontSize));

            }
        }

        List<int> _FontSpacing = new List<int> { -1, 0, 1, 2, 3 };
        public List<int> FontSpacing
        {
            get
            {
                return _FontSpacing;
            }
        }

        List<double> _StrokeThickness = new List<double> { 0.5, 1, 1.5, 2, 2.5, 3 };
        public List<double> StrokeThickness
        {
            get
            {
                return _StrokeThickness;
            }
        }

        int _SelectedFontSpacing;
        public int SelectedFontSpacing
        {
            get
            {
                return _SelectedFontSpacing;
            }
            set
            {
                _SelectedFontSpacing = value;
                _Font.FontSpacing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontSpacing)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontStretch)));
            }
        }


        List<String> _FontSizes = new List<string> { "9pt", "11pt", "15pt", "22pt", "30pt", "48pt", "60pt", "72pt" };
        public List<string> FontSizes
        {
            get
            {
                return _FontSizes;
            }
        }
        ViewModelWrappers<FontFamily, FontFamilyModel> FontFamiliesDitionary = new ViewModelWrappers<FontFamily, FontFamilyModel>();

        public List<FontFamilyModel> SystemFontFamilies
        {
            get
            {



                var fontFamilies = from fontFamily in FontData.FontFamilies.Values
                                   select FontFamiliesDitionary.GetViewModelFor(fontFamily, fontFamily);


                return (from fontFamilyModel in fontFamilies
                        orderby fontFamilyModel.Name
                        select fontFamilyModel).ToList();
            }
        }
        FontFamilyModel _SelectdFontFamily;

        public event PropertyChangedEventHandler PropertyChanged;

        FontData _Font;
        public FontData Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
                _SelectdFontFamily = (from fontFamily in SystemFontFamilies where fontFamily.Name == _Font.FontFamilyName select fontFamily).FirstOrDefault();
                _AllCaps = _Font.AllCaps;
                _SelectedShadowBlur = _Font.BlurRadius;

                double px = (double)new System.Windows.FontSizeConverter().ConvertFromString("1px");
                double pt = (double)new System.Windows.FontSizeConverter().ConvertFromString("1pt");
                _FontSize = (Convert.ToInt32(_Font.FontSize * (px / pt))).ToString() + "pt";
                _SelectedFontSpacing = (int)_Font.FontSpacing;
                _SelectedFontFamilyTypeface = GetFontFamilyTypeFace(_Font, SelectdFontFamily);
                _SelectedForeground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_Font.Foreground));
                _AllCaps = _Font.AllCaps;
                _Overline = _Font.Overline;
                _Underline = _Font.Underline;

                _Shadow = _Font.Shadow;
                if (_Shadow)
                {
                    _SelectedShadowColor = (Color)ColorConverter.ConvertFromString(_Font.ShadowColor);
                    _SelectedShadowX = _Font.ShadowXOffset;
                    _SelectedShadowY = _Font.ShadowYOffset;
                    _SelectedShadowBlur = _Font.BlurRadius;
                }

                if (_IsStrokeEnabled)
                {
                    _IsStrokeEnabled = _Font.Stroke;
                    _SelectedStrokeFill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(_Font.StrokeFill));
                    _SelectedStrokeThickness = _Font.StrokeThickness;
                    _SelectedFontSpacing = (int)_Font.FontSpacing;
                }


                //_Overline= _Font.
                //_SelectedFontFamilyTypeface = new FamilyTypefaceModel( (FontStyle)new FontStyleConverter().ConvertFromString( _Font.FontStyle));
                //_SelectedFontFamilyTypeface
                //_SelectdFontFamily = SystemFontFamilies[0];
            }
        }

        private FamilyTypefaceModel GetFontFamilyTypeFace(FontData font, FontFamilyModel fontFamily)
        {
            foreach (var fontTypeFace in fontFamily.FamilyTypefaces)
            {
                if (font.FontStyle == fontTypeFace.ActualFamilyTypeface.Style.ToString() && font.FontWeight == fontTypeFace.ActualFamilyTypeface.Weight.ToString())
                {
                    return fontTypeFace;
                }
            }

            return fontFamily.FamilyTypefaces[0];


        }

        public FontFamilyModel SelectdFontFamily
        {
            get
            {
                if (_SelectdFontFamily == null)
                {                    //_SelectdFontFamily = new FontFamily(new Uri("file:///E:/Projects/OpenVersions/Taste Project/MenuDesigner/MenuDesigner/Resources/Fonts/FoglihtenNo06_076.otf"),"FoglihtenNo06");

                    _SelectdFontFamily = SystemFontFamilies[0];
                }


                return _SelectdFontFamily;
            }
            set
            {

                _SelectdFontFamily = value;
                _Font.FontFamilyName = value.Name;
                _Font.FontStyle = FontStyles.Normal.ToString();
                _Font.FontWeight = FontWeights.Normal.ToString();
                _SelectedFontFamilyTypeface = GetFontFamilyTypeFace(_Font, SelectdFontFamily);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectdFontFamily"));

                //MytextBox.FontFamily = _SelectdFontFamily.ActualFontFamily;
                //MytextBox.FontSize = 70;

                //MtextBox.FontFamily = _SelectdFontFamily.ActualFontFamily;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedFontFamilyTypeface)));
            }
        }
        FamilyTypefaceModel _SelectedFontFamilyTypeface;
        public FamilyTypefaceModel SelectedFontFamilyTypeface
        {
            get
            {
                return _SelectedFontFamilyTypeface;
            }
            set
            {
                _SelectedFontFamilyTypeface = value;
                if (_SelectedFontFamilyTypeface != null)
                {
                    _Font.FontStyle = _SelectedFontFamilyTypeface.ActualFamilyTypeface.Style.ToString();
                    _Font.FontWeight = _SelectedFontFamilyTypeface.ActualFamilyTypeface.Weight.ToString();
                }

                //FontStretches.Normal
                //FontWeights.Normal
                //FontStyles.Normal

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectdFontFamilyTypeface"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedFontWeight"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedFontStretch"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedFontStyle"));
            }
        }

        public FontStyle SelectedFontStyle
        {
            get
            {
                if (SelectedFontFamilyTypeface != null)
                    return SelectedFontFamilyTypeface.ActualFamilyTypeface.Style;
                else
                    return FontStyles.Normal;
            }
        }
        public FontStretch SelectedFontStretch
        {
            get
            {
                if (SelectedFontSpacing == 0|| SelectedFontSpacing<0)
                    return FontStretches.Normal;
                return FontStretch.FromOpenTypeStretch(SelectedFontSpacing);

                if (SelectedFontFamilyTypeface != null)
                    return SelectedFontFamilyTypeface.ActualFamilyTypeface.Stretch;
                else
                    return FontStretches.Normal;
            }
        }

        bool _Shadow;
        public bool Shadow
        {
            get
            {
                return _Shadow;
            }
            set
            {
                _Shadow = value;
                _Font.Shadow = value;
                if (_Font.Shadow)
                {
                    _Font.ShadowColor = new ColorConverter().ConvertToString(SelectedShadowColor);
                    _Font.ShadowXOffset = SelectedShadowX;
                    _Font.ShadowYOffset = SelectedShadowY;
                    _Font.BlurRadius = SelectedShadowBlur;
                }
                else
                {
                    _Font.ShadowColor = null;
                    _Font.ShadowXOffset = 0;
                    _Font.ShadowYOffset = 0;
                    _Font.BlurRadius = 0;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShadowPropertiesVisibility)));
            }
        }

        public System.Windows.Media.Effects.DropShadowEffect DropShadowEffect
        {
            get
            {
                if (_Shadow)
                {
                    double deltaX = SelectedShadowX;
                    double deltaY = SelectedShadowY;


                    deltaY = -deltaY;
                    var rad = Math.Atan2(deltaY, deltaX);

                    var deg = rad * (180 / Math.PI);

                    if (deg < 0)
                        deg = 360 + deg;

                    double a = deltaX;
                    double b = deltaY;
                    if (a < 0)
                        a = -a;
                    if (b < 0)
                        b = -b;
                    double depth = Math.Sqrt(a * a + b * b);

                    var shaddow = new System.Windows.Media.Effects.DropShadowEffect();
                    shaddow.Direction = deg;
                    shaddow.ShadowDepth = depth;

                    shaddow.Opacity = 1;
                    shaddow.BlurRadius = SelectedShadowBlur;
                    shaddow.Color = SelectedShadowColor;

                    return shaddow;
                }
                else
                    return null;

            }
        }
        public System.Windows.Media.Color SelectedColor
        {
            get
            {
                if (_SelectedForeground == null)
                    _SelectedForeground = new SolidColorBrush(Colors.Black);
                if (_SelectedForeground != null)
                    return _SelectedForeground.Color;
                return Colors.Black;
            }
            set
            {
                _SelectedForeground = new SolidColorBrush(value);
                _Font.Foreground = new ColorConverter().ConvertToString(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedForeground)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStroke)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeFill)));
            }
        }

        public System.Windows.Media.Color SelectedStrokeColor
        {
            get
            {

                if (_SelectedStrokeFill == null && !IsStrokeEnabled)
                    return SelectedColor;
                if (_SelectedStrokeFill is SolidColorBrush)
                    return (_SelectedStrokeFill as SolidColorBrush).Color;
                return Colors.White;
            }
            set
            {
                _SelectedStrokeFill = new SolidColorBrush(value);
                _Font.StrokeFill = new ColorConverter().ConvertToString(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeFill)));
            }
        }

        System.Windows.Media.Color _SelectedShadowColor = Colors.LightGray;
        public System.Windows.Media.Color SelectedShadowColor
        {
            get
            {
                return _SelectedShadowColor;
            }
            set
            {
                _SelectedShadowColor = value;
                _Font.ShadowColor = new ColorConverter().ConvertToString(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }
        }

        double _SelectedShadowX = 3;
        public double SelectedShadowX
        {
            get
            {
                return _SelectedShadowX;
            }
            set
            {
                _SelectedShadowX = value;
                _Font.ShadowXOffset = value;

                if (!_ShadowOffset.Contains(value))
                {
                    double theOffset = (from offset in _ShadowBlur where offset < value select offset).FirstOrDefault();
                    if (theOffset > 0)
                        _ShadowOffset.Insert(_ShadowBlur.IndexOf(theOffset), value);
                    else
                        _ShadowOffset.Add(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShadowOffset)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShadowY)));
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }
        }
        List<double> _ShadowOffset = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8 };

        public List<double> ShadowOffset
        {
            get
            {
                return _ShadowOffset;
            }
        }

        List<double> _ShadowBlur = new List<double> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

        public List<double> ShadowBlur
        {
            get
            {
                return _ShadowBlur;
            }
        }

        double _SelectedShadowBlur = 0;
        public double SelectedShadowBlur
        {
            get
            {
                return _SelectedShadowBlur;
            }
            set
            {
                _SelectedShadowBlur = value;
                _Font.BlurRadius = value;
                if (!_ShadowBlur.Contains(value))
                {
                    double theBlur = (from blur in _ShadowBlur where blur < value select blur).FirstOrDefault();
                    if (theBlur > 0)
                        _ShadowBlur.Insert(_ShadowBlur.IndexOf(theBlur), value);
                    else
                        _ShadowBlur.Add(value);

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShadowBlur)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShadowBlur)));

                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }
        }


        double _SelectedShadowY = 3;
        public double SelectedShadowY
        {
            get
            {
                return _SelectedShadowY;
            }
            set
            {
                _SelectedShadowY = value;
                _Font.ShadowYOffset = value;
                if (!_ShadowOffset.Contains(value))
                {
                    double theOffset = (from offset in _ShadowBlur where offset < value select offset).FirstOrDefault();
                    if (theOffset > 0)
                        _ShadowOffset.Insert(_ShadowBlur.IndexOf(theOffset), value);
                    else
                        _ShadowOffset.Add(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShadowOffset)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedShadowY)));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DropShadowEffect)));
            }
        }


        SolidColorBrush _SelectedForeground;

        public Brush SelectedForeground
        {
            get
            {
                if (_SelectedForeground == null)
                    return new SolidColorBrush(SelectedColor);
                else
                    return _SelectedForeground;
            }
        }

        bool _IsStrokeEnabled;
        public bool IsStrokeEnabled
        {
            get
            {
                return _IsStrokeEnabled;
            }
            set
            {
                _IsStrokeEnabled = value;
                _Font.Stroke = value;

                if (_Font.Stroke)
                {
                    _Font.StrokeFill = new ColorConverter().ConvertToString(SelectedStrokeColor);
                    _Font.StrokeThickness = SelectedStrokeThickness;
                }
                else
                {
                    _Font.StrokeFill = null;
                    _Font.StrokeThickness = 0;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StrokeVisibility)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStroke)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeFill)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeThickness)));
            }
        }

        bool _Underline;
        public bool Underline
        {
            get
            {
                return _Underline;
            }
            set
            {
                _Underline = value;
                _Font.Underline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Underline)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Overline)));

            }
        }

        bool _AllCaps;
        public bool AllCaps
        {
            get
            {
                return _AllCaps;
            }
            set
            {
                _AllCaps = value;
                _Font.AllCaps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllCaps)));
            }
        }


        bool _Overline;
        public bool Overline
        {
            get
            {
                return _Overline;
            }
            set
            {
                _Overline = value;
                _Font.Underline = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Underline)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Overline)));

            }
        }
        Brush _SelectedStrokeFill;
        public Brush SelectedStrokeFill
        {
            get
            {
                if (IsStrokeEnabled)
                {
                    if (_SelectedStrokeFill == null)
                        _SelectedStrokeFill = new SolidColorBrush(SelectedStrokeColor);
                    return _SelectedStrokeFill;
                }
                else
                    return SelectedForeground;

            }
        }

        public Brush SelectedStroke
        {
            get
            {
                return SelectedForeground;
            }
        }


        public Visibility StrokeVisibility
        {
            get
            {
                if (IsStrokeEnabled)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility ShadowPropertiesVisibility
        {
            get
            {
                if (Shadow)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }


        double _SelectedStrokeThickness = 1;
        public double SelectedStrokeThickness
        {
            get
            {
                if (IsStrokeEnabled)
                    return _SelectedStrokeThickness;
                else
                    return 0;
            }
            set
            {
                _SelectedStrokeThickness = value;
                _Font.StrokeThickness = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStrokeThickness)));
            }
        }
        public FontWeight SelectedFontWeight
        {
            get
            {

                if (SelectedFontFamilyTypeface != null)
                    return SelectedFontFamilyTypeface.ActualFamilyTypeface.Weight;
                else
                    return FontWeights.Normal;
            }
        }

        public string NewItem
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int fontSize = 0;
                    string _value = value;
                    if (_value != null)
                        _value = _value.Replace("pt", "");

                    if (int.TryParse(_value, out fontSize))
                    {
                        if (_FontSizes.Contains(fontSize.ToString() + "pt"))
                            _FontSizes.Add(fontSize.ToString() + "pt");
                        SelectedFontSize = fontSize.ToString() + "pt";
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NewItem)));
                }
            }
            get
            {
                return SelectedFontSize.ToString();
            }
        }
    }

    /// <MetaDataID>{ad29602c-2ad4-4808-be56-3e389743e6cb}</MetaDataID>
    public class FamilyTypefaceModel
    {

        FamilyTypeface _ActualFamilyTypeface;
        public FamilyTypefaceModel(FamilyTypeface familyTypeface)
        {
            _ActualFamilyTypeface = familyTypeface;
        }


        public FamilyTypeface ActualFamilyTypeface
        {
            get
            {
                return _ActualFamilyTypeface;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name
        {
            get
            {
                if (_ActualFamilyTypeface.Style == FontStyles.Normal &&
                    _ActualFamilyTypeface.Weight == FontWeights.Normal)
                {
                    return "Regular";
                }

                string name = null;
                if (_ActualFamilyTypeface.Weight != FontWeights.Normal)
                    name = _ActualFamilyTypeface.Weight.ToString();

                if (!string.IsNullOrWhiteSpace(name))
                    name += " ";
                if (_ActualFamilyTypeface.Style != FontStyles.Normal)
                    name += _ActualFamilyTypeface.Style.ToString();

                return name;
            }
        }

    }
    /// <MetaDataID>{69b10d44-7d03-466e-993f-f7bcfee01682}</MetaDataID>
    public class FontFamilyModel:MarshalByRefObject
    {
        public FontFamilyModel(FontFamily fontFamily)
        {
            _ActualFontFamily = fontFamily;
            foreach (FamilyTypeface familyTypeface in fontFamily.FamilyTypefaces)
            {
                FamilyTypefaceModel familyTypefaceModel = new FamilyTypefaceModel(familyTypeface);
                if (!_FamilyTypefaces.ContainsKey(familyTypefaceModel.Name))
                    _FamilyTypefaces.Add(familyTypefaceModel.Name, familyTypefaceModel);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        FontFamily _ActualFontFamily;

        public FontFamily ActualFontFamily
        {
            get
            {
                return _ActualFontFamily;
            }
        }

        Dictionary<string, FamilyTypefaceModel> _FamilyTypefaces = new Dictionary<string, FamilyTypefaceModel>();
        public List<FamilyTypefaceModel> FamilyTypefaces
        {
            get
            {
                return _FamilyTypefaces.Values.ToList();
            }
        }

        public string Name
        {
            get
            {
                if (_ActualFontFamily.ToString().IndexOf("#") != -1)
                    return _ActualFontFamily.ToString().Split(',')[0].Split('#')[1];
                else
                    return _ActualFontFamily.ToString();
            }
        }


    }


}
