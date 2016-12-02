using System;
using System.Threading.Tasks;
using ServiceStack;
using yoke.novel.Model;

namespace yoke.novel.Http
{

    /// <summary>
    /// 访问HTTP服务器基础服务
    /// </summary>
    static class Chapter2Service
    {
        public static string BaseUrl = "http://chapter2.zhuishushenqi.com";
        static string _urlBookChapter = "/chapter/{0}";

        /// <summary>
        /// 章节信息
        /// </summary>
        /// <param name="chapterUrl"></param>
        /// <returns></returns>
        public static async Task<ChapterResult> GetDetail(string chapterUrl)
        {
            try
            {
                string url = string.Format(_urlBookChapter, chapterUrl.UrlEncode());
                return await GetJson<ChapterResult>(url);
            }
            catch
            {
                return null;
            }
        }


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
    }
}
