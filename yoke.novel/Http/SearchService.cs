using System.Threading.Tasks;
using yoke.novel.Model;
using ServiceStack;

namespace yoke.novel.Http
{
    class SearchService
    {
        static string _urlSearchFuzzy = "/book/fuzzy-search?query={0}";
        static string _urlSearchAutoComplete = "/book/auto-complete?query={0}";

        /// <summary>
        /// 搜索小说
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public async static Task<SearchResult> Search(string keywords)
        {
            try
            {
                string url = string.Format(_urlSearchFuzzy, keywords.UrlEncode().ToUpper());
                SearchResult searchResult = await BookService.GetJson<SearchResult>(url);
                return searchResult;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 搜索自动提示
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public async static Task<SearchAutoComplete> AutoComplete(string keywords)
        {
            try
            {
                string url = string.Format(_urlSearchAutoComplete, keywords.UrlEncode().ToUpper());
                SearchAutoComplete searchResult = await BookService.GetJson<SearchAutoComplete>(url);
                return searchResult;
            }
            catch
            {
                return null;
            }
        }
    }
}
