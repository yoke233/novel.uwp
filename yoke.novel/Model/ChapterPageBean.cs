using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yoke.novel.Db;

namespace yoke.novel.Model
{
    class ChapterPageBean
    {
        /// <summary>
        /// 书籍id
        /// </summary>
        public string BookId;

        /// <summary>
        /// 来源id
        /// </summary>
        public string TocId;

        /// <summary>
        /// 数据库章节
        /// </summary>
        public ChapterDb ChapterDb { get; set; }

        /// <summary>
        /// 章节列表
        /// </summary>
        public List<TocChapterItem> ChapterList { get; set; }

        /// <summary>
        /// 章节在目录的索引
        /// </summary>
        public int ChapterIndex;
    }
}
