using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using yoke.novel.Model;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace yoke.novel
{
    public sealed partial class ChapterListDialog : ContentDialog
    {
        public int index = -1;
        public TocChapterItem SelectedChapter;

        public ChapterListDialog()
        {
            this.InitializeComponent();
        }
        public List<TocChapterItem> Chapters { get; set; }

        public void init(List<TocChapterItem> Chapters, int index)
        {
            this.Chapters = Chapters;
            ListView.ItemsSource = Chapters;
            ListView.SelectedIndex = index;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            this.SelectedChapter = (TocChapterItem) e.ClickedItem;
            this.Hide();
        }

        private void ChapterListDialog_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ListView.ScrollIntoView(Chapters[ListView.SelectedIndex], ScrollIntoViewAlignment.Leading);
        }
    }
}