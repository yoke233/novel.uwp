using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace yoke.novel.Db
{
    class BookDb
    {

        [PrimaryKey]
        public string id { get; set; }

        public string title { get; set; }

        public string author { get; set; }

        public string lastChapter { get; set; }

        public string cover { get; set; }

        public DateTime updated { get; set; }

        public string toc_id { get; set; }

        public int lastChapterIndex { get; set; }

        public DateTime lastOpenTime { get; set; }

        public string chapters { get; set; }

    }


    public class ChapterDb
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string book_id { get; set; }
        public string toc_id { get; set; }
        public int index { get; set; }
        public string title { get; set; }
        public string body { get; set; }
    }
}
