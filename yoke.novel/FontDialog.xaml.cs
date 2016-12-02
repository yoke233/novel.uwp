using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using yoke.novel.Utils;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace yoke.novel
{
    public sealed partial class FontDialog : ContentDialog
    {

        public InstalledFont FontName;

        public Color FontColor;

        public Color BgColor;

        public int SFontSize;

        public int SLineSize;

        public FontDialog()
        {
            this.InitializeComponent();
            var fontList = InstalledFont.GetFonts();
            FontList.ItemsSource = fontList;
        }

        public void init(InstalledFont fontName, Color fontColor, Color bgColor, int fontSize, int lineSize)
        {
            FontName = fontName;
            FontColor = fontColor;
            BgColor = bgColor;
            SFontSize = fontSize;
            SLineSize = lineSize;

            if (FontName != null)
            {
                FontList.SelectedIndex = FontName.FamilyIndex;
            }
            colorPicker.SelectedColor = FontColor;
            colorPicker2.SelectedColor = BgColor;
            colorRect.Fill = new SolidColorBrush(FontColor);
            colorRect2.Fill = new SolidColorBrush(BgColor);
            FontSizeInput.Text = SFontSize+"";
            LineSizeInput.Text = SLineSize + "";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            FontName = (InstalledFont) FontList.SelectedItem;
            SFontSize = int.Parse(FontSizeInput.Text);
            SLineSize = int.Parse(LineSizeInput.Text);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }


        private void UWPColorPickerControl_ColorSelected(object sender, Windows.UI.Color color)
        {
            colorFlyout.Hide();
            colorRect.Fill = new SolidColorBrush(color);
            FontColor = color;
        }

        private void ColorPicker2_OnColorSelected(object sender, Color color)
        {
            colorFlyout2.Hide();
            colorRect2.Fill = new SolidColorBrush(color);
            BgColor = color;
        }
    }
}
