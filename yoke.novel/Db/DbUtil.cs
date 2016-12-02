using System;
using System.IO;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace yoke.novel.Db
{
    class DbUtil
    {
        private static String DbName = "book.db";
        private static String DbFilePath;

        public static void Init()
        {
            DbFilePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DbName);
            using (var con = GetDbConnection())
            {
                con.CreateTable<BookDb>();
                con.CreateTable<ChapterDb>();
            }
        }

        public static SQLiteConnection GetDbConnection()
        {
            var con = new SQLiteConnection(new SQLitePlatformWinRT(), DbFilePath);
            return con;
        }

    }
}
