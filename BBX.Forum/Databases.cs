using System;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class Databases
    {
		//private delegate bool delegateCreateOrFillText(string DbName, string postidlist);

		//private delegate void delegateCreateFillIndex(string DbName);

		//private Databases.delegateCreateOrFillText aysncallback;
		//private Databases.delegateCreateFillIndex aysncallbackFillIndex;

        //public static string RestoreDatabase(string backupPath, string serverName, string userName, string password, string dbName, string fileName)
        //{
        //    return BBX.Data.Databases.RestoreDatabase(backupPath, serverName, userName, password, dbName, fileName.Replace(" ", "_"));
        //}

        //public static string BackUpDatabase(string backupPath, string serverName, string userName, string password, string dbName, string fileName)
        //{
        //    return BBX.Data.Databases.BackUpDatabase(backupPath, serverName, userName, password, dbName, fileName.Replace(" ", "_"));
        //}

        //public static bool IsBackupDatabase()
        //{
        //    return BBX.Data.Databases.IsBackupDatabase();
        //}

        public static int TestFullTextIndex(ref string msg)
        {
            foreach (var item in TableList.GetAllPostTable())
            {
                try
                {
                    BBX.Data.Databases.TestFullTextIndex(item.ID);
                }
                catch
                {
                    msg = "<script>alert('您的数据库帖子表[" + BaseConfigs.GetTablePrefix + "posts" + item.ID + "]中暂未进行全文索引设置,因此使用数据库全文搜索无效');</script>";
                    return 0;
                }
            }
            return 1;
        }

        public static bool IsFullTextSearchEnabled()
        {
            return BBX.Data.Databases.IsFullTextSearchEnabled();
        }

        //public static string GetDbName()
        //{
        //    return BBX.Data.Databases.GetDbName();
        //}

		//public void CallBack(IAsyncResult e)
		//{
		//	this.aysncallback.EndInvoke(e);
		//}

        //public bool StarFillIndexWithPostid(string DbName, string postidlist)
        //{
        //    bool result;
        //    try
        //    {
        //        string[] array = postidlist.Split(',');
        //        for (int i = 0; i < array.Length; i++)
        //        {
        //            string postid = array[i];
        //            Posts.CreateORFillIndex(DbName, postid);
        //        }
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Databases.FormatMessage(ex.Message);
        //        result = false;
        //    }
        //    return result;
        //}

        private static string FormatMessage(string message)
        {
            return message.Replace("'", " ").Replace("\\", "/").Replace("\r\n", "\\r\\n").Replace("\r", "\\r").Replace("\n", "\\n");
        }

        //public string StartFullIndex(string id, string dbname, string userName)
        //{
        //    string dbName = Databases.GetDbName();
        //    if (id != "")
        //    {
        //        try
        //        {
        //            BBX.Data.Databases.StartFullIndex(dbName);
        //            this.aysncallback = new Databases.delegateCreateOrFillText(this.StarFillIndexWithPostid);
        //            AsyncCallback callback = new AsyncCallback(this.CallBack);
        //            this.aysncallback.BeginInvoke(dbName, id, callback, userName);
        //            string result = "window.location.href='global_detachtable.aspx';";
        //            return result;
        //        }
        //        catch (Exception ex)
        //        {
        //            string result = "<script>alert('" + Databases.FormatMessage(ex.Message) + "');</script>";
        //            return result;
        //        }
        //    }
        //    return "<script>alert('您未选中任何选项');window.location.href='global_detachtable.aspx';</script>";
        //}

        //public static void CreatePostTableAndIndex(string tablename)
        //{
        //    BBX.Data.Databases.CreatePostTableAndIndex(tablename);
        //}

        public static bool IsStoreProc()
        {
            return BBX.Data.Databases.IsStoreProc();
        }

        public static bool IsShrinkData()
        {
            return BBX.Data.Databases.IsShrinkData();
        }

        public static string ShrinkDataBase(string strDbName, string size)
        {
            string result;
            try
            {
                string shrinksize = (!Utils.StrIsNullOrEmpty(size)) ? size : "0";
                BBX.Data.Databases.ShrinkDataBase(shrinksize, strDbName);
                result = "window.location.href='global_logandshrinkdb.aspx';";
            }
            catch (Exception ex)
            {
                result = "<script language=\"javascript\">alert('" + Databases.FormatMessage(ex.Message) + "!');window.location.href='global_logandshrinkdb.aspx';</script>";
            }
            return result;
        }

        public static string ClearDBLog(string dbname)
        {
            string result;
            try
            {
                BBX.Data.Databases.ClearDBLog(dbname);
                result = "window.location.href='global_logandshrinkdb.aspx';";
            }
            catch (Exception ex)
            {
                result = "<script language=\"javascript\">alert('" + Databases.FormatMessage(ex.Message) + "!');window.location.href='global_logandshrinkdb.aspx';</script>";
            }
            return result;
        }

        public static string RunSql(string sql)
        {
            return BBX.Data.Databases.RunSql(sql);
        }

		//public void CallBackFillIndex(IAsyncResult e)
		//{
		//	this.aysncallbackFillIndex.EndInvoke(e);
		//}

        //public void CreateFullText(string DbName)
        //{
        //    foreach (var item in TableList.GetAllPostTable())
        //    {
        //        PostTables.CreateORFillIndex(DbName, item.ID.ToString());
        //    }
        //}

        //public string CreateFullTextIndex(string tableName, string userName)
        //{
        //    string result;
        //    try
        //    {
        //        BBX.Data.Databases.CreateFullTextIndex(tableName);
        //        this.aysncallbackFillIndex = new Databases.delegateCreateFillIndex(this.CreateFullText);
        //        AsyncCallback callback = new AsyncCallback(this.CallBackFillIndex);
        //        this.aysncallbackFillIndex.BeginInvoke(tableName, callback, userName);
        //        result = "window.location.href='forum_updateforumstatic.aspx';";
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "<script>alert('" + Databases.FormatMessage(ex.Message) + "');</script>";
        //    }
        //    return result;
        //}

        //public static void UpdatePostSP()
        //{
        //}

        //public static string GetDataBaseVersion()
        //{
        //    return BBX.Data.Databases.GetDataBaseVersion();
        //}

        //public static bool IsExistTable(string tableName)
        //{
        //    return BBX.Data.Databases.IsExistTable(tableName);
        //}
    }
}