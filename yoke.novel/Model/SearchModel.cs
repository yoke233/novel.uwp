using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yoke.novel.Model
{

    public class SearchResult
    {
        public List<SearchBook> books { get; set; }
        public bool ok { get; set; }
    }

    public class SearchBook
    {
        public string _id { get; set; }
        public bool hasCp { get; set; }
        public string title { get; set; }
        public string cat { get; set; }
        public string author { get; set; }
        public string site { get; set; }
        public string cover { get; set; }
        public string shortIntro { get; set; }
        public string lastChapter { get; set; }
        public double? retentionRatio { get; set; }
        public int latelyFollower { get; set; }
        public int wordCount { get; set; }
    }

    public class SearchAutoComplete
    {
        public List<string> keywords { get; set; }
        public bool ok { get; set; }
    }
}
