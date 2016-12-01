using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace yoke.novel.Model
{
    public class BookDetail
    {
        public string _id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string longIntro { get; set; }
        public string cover { get; set; }
        public string cat { get; set; }
        public string creater { get; set; }
        public string majorCate { get; set; }
        public string minorCate { get; set; }
        public bool _le { get; set; }
        public bool allowMonthly { get; set; }
        public bool allowVoucher { get; set; }
        public bool allowBeanVoucher { get; set; }
        public bool hasCp { get; set; }
        public int postCount { get; set; }
        public int latelyFollower { get; set; }
        public int latelyFollowerBase { get; set; }
        public int followerCount { get; set; }
        public int wordCount { get; set; }
        public int serializeWordCount { get; set; }
        public int minRetentionRatio { get; set; }
        public string retentionRatio { get; set; }
        public DateTime updated { get; set; }
        public bool isSerial { get; set; }
        public int chaptersCount { get; set; }
        public string lastChapter { get; set; }
        public List<string> gender { get; set; }
        public List<string> tags { get; set; }
        public bool donate { get; set; }
        public string copyright { get; set; }
    }

    #region 书籍信息
    public class Author
    {
        public string _id { get; set; }
        public string avatar { get; set; }
        public string nickname { get; set; }
        public string type { get; set; }
        public int lv { get; set; }
        public string gender { get; set; }
    }

    public class Helpful
    {
        public int yes { get; set; }
        public int total { get; set; }
        public int no { get; set; }
    }

    public class Review
    {
        public string _id { get; set; }
        public int rating { get; set; }
        public string content { get; set; }
        public string title { get; set; }
        public Author author { get; set; }
        public Helpful helpful { get; set; }
        public int likeCount { get; set; }
        public string state { get; set; }
        public DateTime updated { get; set; }
        public string created { get; set; }
        public int commentCount { get; set; }
    }

    public class ReviewResult
    {
        public List<Review> reviews { get; set; }
        public bool ok { get; set; }
    }
    #endregion


    #region 书籍来源

    public class BookToc
    {
        public string _id { get; set; }
        public string source { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public string lastChapter { get; set; }
        public bool isCharge { get; set; }
        public int chaptersCount { get; set; }
        public DateTime updated { get; set; }
        public bool starting { get; set; }
        public string host { get; set; }
    }
    #endregion


    #region 书籍来源章节
    public class TocChapterList
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
        public List<TocChapterItem> chapters { get; set; }
        public DateTime updated { get; set; }
        public string host { get; set; }
    }

    public class TocChapterItem
    {
        public string id { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public int currency { get; set; }
        public bool unreadble { get; set; }
        public bool isVip { get; set; }
        public int index { get; set; }
    }
    #endregion

    #region 章节内容
    public class Chapter
    {
        public string title { get; set; }
        public string body { get; set; }
        public bool isVip { get; set; }
        public string cpContent { get; set; }
        public int currency { get; set; }
        public string id { get; set; }
    }

    public class ChapterResult
    {
        public bool ok { get; set; }
        public Chapter chapter { get; set; }
    } 
    #endregion
}
