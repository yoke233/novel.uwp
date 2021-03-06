﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ServiceStack;
using yoke.novel.Db;
using yoke.novel.Model;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace yoke.novel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BookshelfPage : Page
    {
        private ObservableCollection<BookDb> _listBooks = new ObservableCollection<BookDb>();

        public BookshelfPage()
        {
            this.InitializeComponent();
            initFromDb();
        }

        private void initFromDb()
        {
            using (var conn = DbUtil.GetDbConnection())
            {
                var query = conn.Table<BookDb>().OrderByDescending(v => v.lastOpenTime);
                var bookDbs = query.ToList();
                foreach (var bookDb in bookDbs)
                {
                    _listBooks.Add(bookDb);
                }
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void SearchResultListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            using (var conn = DbUtil.GetDbConnection())
            {
                var book = (BookDb) e.ClickedItem;
                var topChapterItems = book.chapters.FromJson<List<TocChapterItem>>();

                ChapterPageBean chapterPageBean = new ChapterPageBean
                {
                    BookId = book.id,
                    TocId = book.toc_id,
                    ChapterList = topChapterItems,
                    ChapterIndex = book.lastChapterIndex
                };

                this.Frame.Navigate(typeof(ChapterPage), new object[] { chapterPageBean });
            }
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private BookDb Book;
        private void SearchResultListView_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            ListView listView = (ListView)sender;
            allContactsMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            Book = (BookDb) ((FrameworkElement)e.OriginalSource).DataContext;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            using (var conn = DbUtil.GetDbConnection())
            {
                conn.Table<BookDb>().Delete(v => v.id == Book.id);
                conn.Table<ChapterDb>().Delete(v => v.book_id == Book.id);
                _listBooks.Remove(Book);
                this.Bindings.Update();
            }
        }
    }
}