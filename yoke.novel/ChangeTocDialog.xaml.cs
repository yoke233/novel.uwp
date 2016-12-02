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
using yoke.novel.Http;
using yoke.novel.Model;

// “内容对话框”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上进行了说明

namespace yoke.novel
{
    public sealed partial class ChangeTocDialog : ContentDialog
    {

        public string BookId;
        public BookToc BookToc;
        public TocChapterList TocChapterList;
        public bool ChangeAll;

        public ChangeTocDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            BookToc = (BookToc)e.ClickedItem;
            Loading.IsActive = true;
            TocChapterList = await BookService.GetBookTocChapters(BookToc._id);

            for (int index = 0; index < TocChapterList.chapters.Count; index++)
            {
                var topChapterItem = TocChapterList.chapters[index];
                topChapterItem.index = index;
            }
            ChangeAll = CheckBox.IsChecked.Value;
            Loading.IsActive = false;
            this.Hide();
        }

        private async void ChangeTocDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            BookToc[] tocArray = await BookService.GetBookTocArray(BookId);
            ListView.ItemsSource = tocArray;
            Loading.IsActive = false;
        }
    }
}
