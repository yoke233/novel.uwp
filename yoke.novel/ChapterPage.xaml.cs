using System;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ServiceStack;
using yoke.novel.Db;
using yoke.novel.Http;
using yoke.novel.Model;
using yoke.novel.Utils;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace yoke.novel
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ChapterPage : Page
    {
        private Boolean ChangeBookSource = false;
        private Boolean ChangeChapterSource = false;

        private ChapterPageBean chapter;

        private InstalledFont InstalledFont;

        private Color FontColor = Colors.Black;

        private Color BgColor = Colors.White;

        private int FontSize = 24;

        private int LineSize = 45;

        private Windows.Storage.ApplicationDataContainer localSettings;

        public ChapterPage()
        {
            this.InitializeComponent();

            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("InstalledFont"))
            {
                InstalledFont = localSettings.Values["InstalledFont"].ToString().FromJson<InstalledFont>();
            }
            if (localSettings.Values.ContainsKey("FontColor"))
            {
                string str = localSettings.Values["FontColor"].ToString();
                str = str.Substring(3);
                var rgb = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
                var r = (byte) ((rgb >> 16) & 0xff);
                var g = (byte) ((rgb >> 8) & 0xff);
                var b = (byte) ((rgb >> 0) & 0xff);
                FontColor = Color.FromArgb(0xff, r, g, b);
            }
            if (localSettings.Values.ContainsKey("BgColor"))
            {
                string str = localSettings.Values["BgColor"].ToString();
                str = str.Substring(3);
                var rgb = int.Parse(str, System.Globalization.NumberStyles.HexNumber);
                var r = (byte) ((rgb >> 16) & 0xff);
                var g = (byte) ((rgb >> 8) & 0xff);
                var b = (byte) ((rgb >> 0) & 0xff);
                BgColor = Color.FromArgb(0xff, r, g, b);
            }
            if (localSettings.Values.ContainsKey("SFontSize"))
            {
                FontSize = (int) localSettings.Values["SFontSize"];
            }
            if (localSettings.Values.ContainsKey("SLineSize"))
            {
                LineSize = (int) localSettings.Values["SLineSize"];
            }

            applySetting();
        }

        private string text;

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            object[] parameters = e.Parameter as object[];
            if (parameters?.Length == 1 && (parameters[0] as ChapterPageBean) != null)
            {
                chapter = (ChapterPageBean) parameters[0];
                render();
            }
        }

        private async void render()
        {
            RichTextBlock.Focus(FocusState.Programmatic);
            Loading.IsActive = true;
            RichTextBlock.Blocks.Clear();
            using (var conn = DbUtil.GetDbConnection())
            {
                ChapterDb chapterDb;
                var a =
                    conn.Query<ChapterDb>("select * from ChapterDb where book_id = ? and toc_id = ? and `index` = ?;",
                        chapter.BookId,
                        chapter.TocId,
                        chapter.ChapterIndex);
                if (a.Count == 1)
                {
                    chapterDb = a[0];
                    text = chapterDb.body;
                }
                else
                {
                    ChapterResult result =
                        await Chapter2Service.GetDetail(chapter.ChapterList[chapter.ChapterIndex].link);
                    if (result != null)
                    {
                        if (result.chapter.id != null)
                        {
                            text = result.chapter.cpContent;
                        }
                        else
                        {
                            text = result.chapter.body;
                        }

                        chapterDb = new ChapterDb();
                        chapterDb.book_id = chapter.BookId;
                        chapterDb.toc_id = chapter.TocId;
                        chapterDb.index = chapter.ChapterIndex;
                        chapterDb.body = text;
                        chapterDb.title = result.chapter.title;
                        conn.Insert(chapterDb);
                    }
                    else
                    {
                        chapterDb = new ChapterDb();
                        text = "章节出现异常";
                        chapterDb.title = text;
                        chapterDb.body = text;
                    }
                }

                PageTitle.Text = chapterDb.title;

                var bookDb = conn.Table<BookDb>().Where(v => v.id == chapter.BookId).Take(1).FirstOrDefault();
                if (bookDb != null)
                {
                    if (ChangeBookSource)
                    {
                        ChangeBookSource = false;
                        bookDb.toc_id = chapter.TocId;
                        bookDb.lastChapterIndex = chapter.ChapterIndex;
                    }
                    else if (ChangeChapterSource)
                    {
                        ChangeChapterSource = false;
                        chapter.ChapterIndex = bookDb.lastChapterIndex;
                        chapter.TocId = bookDb.toc_id;
                    }
                    else
                    {
                        bookDb.lastChapterIndex = chapter.ChapterIndex;
                    }
                    bookDb.lastOpenTime = DateTime.Now;
                    conn.Update(bookDb);
                }
            }

            // Create paragraph
            foreach (var s in text.Split(new string[] {"\n"}, System.StringSplitOptions.RemoveEmptyEntries))
            {
                Paragraph paragraph = new Paragraph();
                // Create run and set text
                Run run = new Run {Text = s.Trim()};

                // Add run to the paragraph
                paragraph.Inlines.Add(run);
                paragraph.TextIndent = 50;

                RichTextBlock.Blocks.Add(paragraph);
            }

            // Add paragraph to the rich text block
            ScrollViewer.ChangeView(0, 0, ScrollViewer.ZoomFactor);
            Loading.IsActive = false;
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void PrevPage_OnClick(object sender, RoutedEventArgs e)
        {
            if (chapter.ChapterIndex > 0)
            {
                chapter.ChapterIndex = chapter.ChapterIndex - 1;
                render();
            }
        }

        private void NextPage_OnClick(object sender, RoutedEventArgs e)
        {
            if (chapter.ChapterIndex < (chapter.ChapterList.Count - 1))
            {
                chapter.ChapterIndex = chapter.ChapterIndex + 1;
                render();
            }
        }

        private void RichTextBlock_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Left || e.Key == VirtualKey.GoBack)
            {
                PrevPage_OnClick(sender, null);
            }
            else if (e.Key == VirtualKey.Right || e.Key == VirtualKey.GoForward)
            {
                NextPage_OnClick(sender, null);
            }
            else if (e.Key == VirtualKey.Space)
            {
                ScrollViewer.ChangeView(ScrollViewer.HorizontalOffset,
                    ScrollViewer.VerticalOffset + ScrollViewer.ViewportHeight * 0.8, ScrollViewer.ZoomFactor);
            }
        }

        private async void Font_OnClick(object sender, RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.init(InstalledFont, FontColor, BgColor, FontSize, LineSize);
            ContentDialogResult result = await fontDialog.ShowAsync();
            if (fontDialog.FontName != null)
            {
                InstalledFont = fontDialog.FontName;
            }
            FontColor = fontDialog.FontColor;
            BgColor = fontDialog.BgColor;
            if (fontDialog.SFontSize > 0)
            {
                FontSize = fontDialog.SFontSize;
            }
            if (fontDialog.SLineSize > 0)
            {
                LineSize = fontDialog.SLineSize;
            }
            localSettings.Values["InstalledFont"] = InstalledFont.ToJson();
            localSettings.Values["FontColor"] = FontColor.ToString();
            localSettings.Values["BgColor"] = BgColor.ToString();
            localSettings.Values["SLineSize"] = LineSize;
            localSettings.Values["SFontSize"] = FontSize;
            applySetting();
        }

        private void applySetting()
        {
            RichTextBlock.FontFamily = new FontFamily(InstalledFont.Name);
            RichTextBlock.Foreground = new SolidColorBrush(FontColor);
            ScrollViewer.Background = new SolidColorBrush(BgColor);
            RichTextBlock.FontSize = FontSize;
            RichTextBlock.LineHeight = LineSize;
        }

        private async void More_OnClick(object sender, RoutedEventArgs e)
        {
            ChapterListDialog chapterListDialog = new ChapterListDialog();
            chapterListDialog.init(chapter.ChapterList, chapter.ChapterIndex);
            ContentDialogResult result = await chapterListDialog.ShowAsync();
            if (chapterListDialog.SelectedChapter != null)
            {
                chapter.ChapterIndex = chapterListDialog.SelectedChapter.index;
                render();
            }
        }

        private async void ChangeToc_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeTocDialog changeTocDialog = new ChangeTocDialog();
            changeTocDialog.BookId = chapter.BookId;
            ContentDialogResult result = await changeTocDialog.ShowAsync();
            if (changeTocDialog.TocChapterList != null)
            {
                chapter.ChapterList = changeTocDialog.TocChapterList.chapters;
                chapter.TocId = changeTocDialog.BookToc._id;

                //前后10个内取相似度最高的作为index
                int startI = Math.Max(0, chapter.ChapterIndex - 10);
                int endI = Math.Min(chapter.ChapterList.Count, chapter.ChapterIndex + 10);
                int maxIndex = chapter.ChapterIndex;
                decimal maxDistance = 0;
                decimal tempDistance = 0;
                for (int i = startI; i < endI; i++)
                {
                    tempDistance = LevenshteinDistance.Instance.LevenshteinDistancePercent(PageTitle.Text, chapter.ChapterList[i].title);
                    if (tempDistance > maxDistance)
                    {
                        maxDistance = tempDistance;
                        maxIndex = i;
                    }
                }
                chapter.ChapterIndex = maxIndex;

                ChangeBookSource = changeTocDialog.ChangeAll;
                ChangeChapterSource = !ChangeBookSource;
                More_OnClick(null, null);
            }
        }
    }
}