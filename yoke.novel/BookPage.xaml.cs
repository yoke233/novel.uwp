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
using ServiceStack;
using yoke.novel.Db;
using yoke.novel.Http;
using yoke.novel.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace yoke.novel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BookPage : Page
    {
        public BookPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private BookDetail bookDetail;
        private BookToc bookToc;
        private TocChapterList tocChapterList;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters?.Length == 1 && (parameters[0] as SearchBook) != null)
            {
                SearchBook _book = (SearchBook) parameters[0];

                BookTitle.Text = _book.title;

                bookDetail = await BookService.GetDetail(_book._id);
                bookDetail.cover = BookService.GetCover(bookDetail.cover);

                InfoPanel.Visibility = Visibility;
                Loading.IsActive = false;

                BookToc[] tocArray = await BookService.GetBookTocArray(_book._id);
                TocBox.ItemsSource = tocArray;
                if (tocArray.Length > 0)
                {
                    TocBox.SelectedIndex = 0;
                }

                this.Bindings.Update();
            }
        }

        /// <summary>
        /// 点击后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        /// <summary>
        /// 下载目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Loading.IsActive = true;
            bookToc = (BookToc) TocBox.SelectionBoxItem;
            if (bookToc != null)
            {
                tocChapterList = await BookService.GetBookTocChapters(bookToc._id);
                for (int index = 0; index < tocChapterList.chapters.Count; index++)
                {
                    var topChapterItem = tocChapterList.chapters[index];
                    topChapterItem.index = index;
                }
                ChapterList.ItemsSource = tocChapterList.chapters;
                Loading.IsActive = false;

                using (var conn = DbUtil.GetDbConnection())
                {
                    var bookDb = conn.Table<BookDb>().Where(v => v.id == bookDetail._id).Take(1).FirstOrDefault();
                    if (bookDb != null)
                    {
                        bookDb.toc_id = bookToc._id;
                        bookDb.lastOpenTime = DateTime.Now;
                        bookDb.updated = tocChapterList.updated;
                        bookDb.chapters = tocChapterList.chapters.ToJson();
                        conn.Update(bookDb);
                    }
                    else
                    {
                        bookDb = new BookDb();
                        bookDb.id = bookDetail._id;
                        bookDb.cover = bookDetail.cover;
                        bookDb.title = bookDetail.title;
                        bookDb.author = bookDetail.author;
                        bookDb.lastChapter = bookDetail.lastChapter;
                        bookDb.updated = bookDetail.updated;
                        bookDb.toc_id = bookToc._id;
                        bookDb.lastOpenTime = DateTime.Now;
                        bookDb.chapters = tocChapterList.chapters.ToJson();
                        conn.Insert(bookDb);
                    }
                }
            }
        }

        private void ChapterList_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ChapterPageBean chapterPageBean = new ChapterPageBean
            {
                BookId = bookDetail._id,
                TocId = bookToc._id,
                ChapterList = tocChapterList.chapters,
                ChapterIndex = ((TocChapterItem) e.ClickedItem).index
            };

            this.Frame.Navigate(typeof(ChapterPage), new object[] {chapterPageBean});
        }
    }
}