using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace yoke.novel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchPage : Page
    {

        private ObservableCollection<SearchBook> _listBooks = new ObservableCollection<SearchBook>();

        public SearchPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SearchAutoComplete autoComplete = await SearchService.AutoComplete(SearchBox.Text);
            if (autoComplete != null && autoComplete.ok)
            {
                SearchBox.ItemsSource = autoComplete.keywords;
            }
        }

        private async void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Loading.IsActive = true;
            SearchResult result = await SearchService.Search(SearchBox.Text);
            if (result != null && result.ok)
            {
                _listBooks.Clear();
                result.books.ForEach((b) =>
                {
                    b.cover = BookService.GetCover(b.cover);
                    _listBooks.Add(b);
                });
                Loading.IsActive = false;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BlogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(BookPage), new object[] { e.ClickedItem });
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
