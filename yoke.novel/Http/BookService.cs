using System;
using System.Threading.Tasks;
using yoke.novel.Model;
using ServiceStack;

namespace yoke.novel.Http
{
    class BookService
    {
        static string _urlBookDetail = "/book/{0}";
        static string _urlBookReviews = "/post/review/best-by-book?book={0}";
        static string _urlBookToc = "/toc?view=summary&book={0}";
        static string _urlBookTocChapters = "/toc/{0}?view=chapters";

        /// <summary>
        /// 小说详情
        /// </summary>
        /// <param name="id">书id</param>
        /// <returns></returns>
        public static async Task<BookDetail> GetDetail(string id)
        {
            try
            {
                string url = string.Format(_urlBookDetail, id.UrlEncode().ToUpper());
                return await GetJson<BookDetail>(url);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="id">书id</param>
        /// <returns></returns>
        public static async Task<ReviewResult> GetReviewResult(string id)
        {
            try
            {
                string url = string.Format(_urlBookReviews, id.UrlEncode().ToUpper());
                return await GetJson<ReviewResult>(url);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 书籍来源列表
        /// </summary>
        /// <param name="id">书id</param>
        /// <returns></returns>
        public static async Task<BookToc[]> GetBookTocArray(string id)
        {
            try
            {
                string url = string.Format(_urlBookToc, id.UrlEncode().ToUpper());
                return await GetJson<BookToc[]>(url);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 书籍来源的章节列表
        /// </summary>
        /// <param name="tocId">书来源id</param>
        /// <returns></returns>
        public static async Task<TocChapterList> GetBookTocChapters(string tocId)
        {
            try
            {
                string url = string.Format(_urlBookTocChapters, tocId.UrlEncode().ToUpper());
                return await GetJson<TocChapterList>(url);
            }
            catch
            {
                return null;
            }
        }

        public static string BaseUrl = "http://api.zhuishushenqi.com";

        public static async Task<T> GetJson<T>(string url)
        {
            try
            {
                string allUrl = BaseUrl + url;
                var json = await allUrl.GetJsonFromUrlAsync();
                T t = json.FromJson<T>();
                return t;
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        public static string GetCover(string cover)
        {
            if (cover != null && cover.IndexOf("http") > 0)
            {
                cover = cover.Replace("/agent/", "");
            }
            else
            {
                cover = "Assets/NewStoreLogo.scale-150.png";
            }
            return cover;
        }
    }
}
