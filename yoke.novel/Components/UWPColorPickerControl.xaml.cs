
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace yoke.novel.Components
{
    public sealed partial class UWPColorPickerControl : UserControl
    {
        public UWPColorPickerControl()
        {
            this.InitializeComponent();
            SelectedColor = Colors.Black;
            Loaded += UWPColorPickerControl_Loaded;
        }

        public delegate void ColorSelectedHandler(object sender, Color color);
        public event ColorSelectedHandler ColorSelected = null;

        public Color SelectedColor
        {
            get;
            set;
        }

        private void UWPColorPickerControl_Loaded(object sender, RoutedEventArgs e)
        {
            addColorGroup("Color A", "#373C20#8DA3A2#FCFEFE#C6DBE6#A8BFCB#524644#DC3E00#A89D9F#CFCAC9#D29A8D#484745#A4A0A0#94857E#E0DBDC#BDB7BB#65524D#DE5B12#99979B#FA6B2D#C2D0DE#655939#A1A494#CBD5D6#DEEBEF#B7C1BB");
            addColorGroup("Color B", "#64604E#DC4508#75A873#CBD1E7#A9B795#404239#C74341#A792A2#EB4772#BCB9C6#453D3B#BB8977#FF6409#D4D0D3#C2B9BD#6A2E3B#E70000#8B8BA9#C0C3FC#C798C1#744437#E72A00#9A989F#FF4D0B#ACBBDA");
            addColorGroup("Color C", "#706B6B#969398#948980#EAEBF2#B4B4C4#3A3F34#A7A69F#889381#CFCFD1#C1B4A7#484D48#818D9E#5295F4#CEDEF7#ACB5C7#645B5C#A19DA5#8D8083#C6C3D1#B5AFC3#554E3D#A2A5A0#908C7F#E6E6DE#B9BEBB");
            addColorGroup("Colod D", "#5C5357#5F5CA7#9F9CAA#A89363#D3C7B9#00481B#094722#6F7766#F0F1F1#A59F98#0F0A03#973137#C96B66#FFF4F7#E096A9#292419#7A6E62#C9BDBC#F4F0EF#AA9A94#000053#3D288D#717ACD#E7EDF7#96ADED");
            addColorGroup("Color E", "#1B121E#627D89#446645#EDE4EB#8692A1#3D3441#C0C2D1#FFFBFF#E9DFF7#CFB60D#595124#D7F900#EBFF00#BFD600#9CA31D#4B2225#DE0000#CDCCD9#FBF4FF#6DD45B#490813#D50000#D00000#DFD5E2#788CB2");

            addColorGroup("Color F", "#775D16#91742D#82612E#24190F#A48B3F#4F4435#A7DF1A#FFFFFF#EADCC0#8C76A2#23231A#787572#515237#D9DEE6#AA7B77#5B5A4A#C60000#BDC4C0#CEE2FF#4D9F92#141610#74776B#D1D7CA#FFFFFF#AE996A");
            addColorGroup("Color G", "#262213#756F67#FFFFFF#C5BCB5#C13232#111210#2F312D#714A4C#FBFCFF#CA0008#342B54#CC2800#ED6F13#EEF6F6#118AB6#211508#784B34#CA4600#F2E3EC#E76A00#212014#7F7967#55513C#C9CAC9#A59C87");
            addColorGroup("Color H", "#1D181A#860000#A70000#CEB5E6#9B80AB#272621#787677#54494E#EFF1F3#95C035#5E4046#4492D7#B20000#68BAFF#4172B0#25395A#E1FFFF#F3FFFF#B3D4F2#5C80B7#DF0000#FF8400#FFFFFF#E9E0C5#646762");
            addColorGroup("Color I", "#717B78#27353A#CACCCA#F7F4F6#979785#161A24#213972#0B46AD#E7E9E9#A0B16F#777A85#3A3C48#7B82AA#A7A5DC#8691B1#777A85#3A3C48#7B82AA#A7A5DC#8691B1#615400#C63200#D8B903#FFEF8D#58AB4C");
        }

        private void addColorGroup(String groupName, String colorDefine)
        {
            var st = colorDefine.Split('#').Where(s => s != "").ToList();
            var pitem = new PivotItem();
            pitem.Header = groupName;
            var grid = new GridView();
            pitem.Content = grid;

            foreach (var s in st)
            {
                var rgb = int.Parse(s, System.Globalization.NumberStyles.HexNumber);
                var r = (byte)((rgb >> 16) & 0xff);
                var g = (byte)((rgb >> 8) & 0xff);
                var b = (byte)((rgb >> 0) & 0xff);
                var color = Color.FromArgb(0xff, r, g, b);
                var rect = new Rectangle();
                rect.Width = 40;
                rect.Height = 40;
                rect.Fill = new SolidColorBrush(color);

                grid.Items.Add(rect);

                rect.PointerPressed += delegate (object sender, PointerRoutedEventArgs e) {
                    if (ColorSelected != null)
                    {
                        SelectedColor = color;
                        ColorSelected(sender, color);
                    }
                };

            }

            definedColorsPivot.Items.Add(pitem);
        }

    }
}