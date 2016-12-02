using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using yoke.novel.Db;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace yoke.novel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int _preSelectNavigation = -1;

        public MainPage()
        {
            DbUtil.Init();
            this.InitializeComponent();
            MainNavigationList.SelectedIndex = 1;
            ShowNavigationBar(App.AlwaysShowNavigation);
        }

        /// <summary>
        /// 导航栏隐现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListBoxItem tappedItem = sender as ListBoxItem;
            if (tappedItem != null && tappedItem.Tag != null && tappedItem.Tag.ToString().Equals("0")) //汉堡按钮
            {
                MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
            }
        }

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void mainNavigationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem tappedItem = MainNavigationList.SelectedItems[0] as ListBoxItem;
            if (tappedItem != null && tappedItem.Tag != null)
            {
                MainSplitView.IsPaneOpen = false;
                _preSelectNavigation = MainNavigationList.SelectedIndex;
                if (tappedItem.Tag.ToString().Equals("1"))
                {
                    //书架
                    MainFrame.Navigate(typeof(BookshelfPage));
                }
                else if (tappedItem.Tag.ToString().Equals("2"))
                {
                    //排行榜
                    MainFrame.Navigate(typeof(RankingPage));
                }
                else if (tappedItem.Tag.ToString().Equals("3"))
                {
                    //搜索
                    MainFrame.Navigate(typeof(SearchPage));
                }
                else if (tappedItem.Tag.ToString().Equals("4"))
                {
                    //下载列表
                    MainFrame.Navigate(typeof(DownloadPage));
                }
                else if (tappedItem.Tag.ToString().Equals("5"))
                {
                    //设置项
                    MainFrame.Navigate(typeof(SettingPage));
                }
            }
        }

        /// <summary>
        /// 设置主页面导航显示方式
        /// </summary>
        /// <param name="show"></param>
        public void ShowNavigationBar(bool show)
        {
            MainSplitView.DisplayMode = show ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
        }
        /// <summary>
        /// 打开导航栏一次
        /// </summary>
        public void ShowNavigationBarOneTime()
        {
            MainSplitView.IsPaneOpen = true;
        }
    }
}
