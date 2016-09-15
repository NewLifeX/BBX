using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using XCode;

namespace BBX.Forum
{
    public class ForumHots
    {
        /// <summary>获取帖子列表，带缓存</summary>
        /// <param name="inf"></param>
        /// <returns></returns>
        public static EntityList<Topic> GetTopicList(ForumHotItemInfo inf)
        {
            if (inf.Cachetimeout < 60) inf.Cachetimeout = 60;
            if (inf.Dataitemcount > 50)
                inf.Dataitemcount = 50;
            else if (inf.Dataitemcount < 1)
                inf.Dataitemcount = 1;

            var list = XCache.Current.RetrieveObject("/Forum/ForumHostList-" + inf.Id) as EntityList<Topic>;
            if (list == null)
            {
                string visibleFidList = string.IsNullOrEmpty(inf.Forumlist) ? Forums.GetVisibleForum() : inf.Forumlist;
                //string fieldName = Focuses.GetFieldName((TopicOrderType)Enum.Parse(typeof(TopicOrderType), inf.Sorttype));
                var type = Focuses.GetStartDate((TopicTimeType)Enum.Parse(typeof(TopicTimeType), inf.Datatimetype));
                list = Topic.GetFocusTopicList(inf.Dataitemcount, -1, 0, "", type, inf.Sorttype, visibleFidList, inf.Sorttype == "digest", false);
                if (inf.Cachetimeout > 0) XCache.Add("/Forum/ForumHostList-" + inf.Id, list, inf.Cachetimeout);
            }
            return list;
        }

        public static List<Post> GetFirstPostInfo(int tid, int cachetime)
        {
            var cacheService = XCache.Current;
            var list = cacheService.RetrieveObject("/Forum/HotForumFirst_" + tid) as List<Post>;
            if (list == null || list.Count == 0)
            {
                //list = Posts.GetPostList(tid.ToString());
                list = Post.FindAllByTid(tid);
                XCache.Add("/Forum/HotForumFirst_" + tid, list, cachetime * 60);
            }
            return list;
        }

        public static List<IXForum> GetHotForumList(int topNumber, string orderby, int cachetime, int tabid)
        {
            var cacheService = XCache.Current;
            var list = cacheService.RetrieveObject("/Aggregation/HotForumList_" + tabid) as List<IXForum>;
            if (list == null)
            {
                list = Stats.GetForumArray(orderby).Cast<IXForum>().ToList();
                if (list.Count > topNumber)
                {
                    var list2 = new List<IXForum>();
                    for (int i = 0; i < topNumber; i++)
                    {
                        list2.Add(list[i]);
                    }
                    list = list2;
                }
                XCache.Add("/Aggregation/HotForumList" + tabid, list, cachetime * 60);
            }
            return list;
        }

        public static IUser[] GetUserList(int topNumber, string orderBy, int cachetime, int tabid)
        {
            var cacheService = XCache.Current;
            IUser[] array = cacheService.RetrieveObject("/Aggregation/Users_" + tabid + "List") as IUser[];
            if (array == null)
            {
                if (Utils.InArray(orderBy, "lastactivity,joindate"))
                {
                    //List<IUser> list = new List<IUser>();
                    //DataTable userList = Users.GetUserList(topNumber, 1, orderBy, "desc");
                    //foreach (DataRow dataRow in userList.Rows)
                    //{
                    //    list.Add(new IUser
                    //    {
                    //        Uid = TypeConverter.ObjectToInt(dataRow["uid"]),
                    //        Username = dataRow["username"].ToString(),
                    //        Lastactivity = dataRow["lastactivity"].ToString(),
                    //        JoinDate = dataRow["joindate"].ToString()
                    //    });
                    //}
                    //array = list.ToArray();
                    array = User.FindAll(null, orderBy, null, 0, topNumber).ToArray();
                }
                else
                {
                    array = Stats.GetUserArray(orderBy);
                    if (array.Length > topNumber)
                    {
                        var list2 = new List<IUser>();
                        for (int i = 0; i < topNumber; i++)
                        {
                            list2.Add(array[i]);
                        }
                        array = list2.ToArray();
                    }
                }
                XCache.Add("/Aggregation/Users_" + tabid + "List", array, cachetime * 60);
            }
            return array;
        }

        private static EntityList<Attachment> HotImages(int count, int cachetime, string orderby, int tabid, string fidlist, int continuous)
        {
            var cacheService = XCache.Current;
            var list = cacheService.RetrieveObject("/Aggregation/HotImages_" + tabid + "List") as EntityList<Attachment>;
            if (list == null)
            {
                //dataTable = DatabaseProvider.GetInstance().GetWebSiteAggHotImages(count, orderby, fidlist, continuous);
                list = Attachment.GetWebSiteAggHotImages(count, orderby, fidlist, continuous);
                XCache.Add("/Aggregation/HotImages_" + tabid + "List", list, cachetime * 60);
            }
            return list;
        }

        public static string HotImagesArray(ForumHotItemInfo fi)
        {
            //string format = "<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"/></a></li>";
            //string format2 = "<a href=\"#\" rel=\"{0}\">{0}</a>";
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            var thum = (BaseConfigs.GetForumPath + "cache/rotatethumbnail/").EnsureDirectory();

            //string fidlist = string.IsNullOrEmpty(fi.Forumlist) ? Forums.GetVisibleForum() : fi.Forumlist;
            var fidlist = fi.Forumlist;
            if (fidlist.IsNullOrWhiteSpace()) fidlist = Forums.GetVisibleForum();
            int num = 1;
            // 热点图片
            var list = ForumHots.HotImages(fi.Dataitemcount, fi.Cachetimeout, fi.Sorttype, fi.Id, fidlist, fi.Enabled);
            foreach (var att in list)
            {
                //int topicid = att["tid"].ToInt();
                string filename = (att.FileName + "").Trim();
                string title = (att.Title + "").Trim();
                title = Utils.JsonCharFilter(title).Replace("'", "\\'");

                string format = "<li><a href=\"{0}\" target=\"_blank\"><img src=\"{1}\" alt=\"{2}\"/></a></li>";
                if (!att.IsLocal)
                {
                    ForumHots.DeleteCacheImageFile();
                    var file = Path.GetFileName(filename);
                    Thumbnail.MakeRemoteThumbnailImage(filename, thum.CombinePath("r_" + file), 360, 240);
                    sb.AppendFormat(format, Urls.ShowTopicAspxRewrite(att.Tid, 0), "cache/rotatethumbnail/r_" + file, title);
                }
                else
                {
                    string file = att.FullFileName.Replace('\\', '/').Trim();
                    string str = "cache/rotatethumbnail/r_" + Utils.GetFilename(file);
                    var newFile = Utils.GetMapPath(BaseConfigs.GetForumPath + str);
                    if (!File.Exists(newFile) && File.Exists(file))
                    {
                        ForumHots.DeleteCacheImageFile();
                        Thumbnail.MakeThumbnailImage(file, newFile, 360, 240);
                    }
                    sb.AppendFormat(format, Urls.ShowTopicAspxRewrite(att.Tid, 0), str, title);
                }
                sb2.AppendFormat("<a href=\"#\" rel=\"{0}\">{0}</a>", num);
                num++;
            }
            return "<div class=\"image_reel\"><ul>" + sb.ToString() + "</ul></div><div class=\"paging\"><span></span>" + sb2.ToString() + "</div>";
        }

        public static string RemoveUbb(string message, int length)
        {
            message = Regex.Replace(message, "\\[attachimg\\](\\d+)(\\[/attachimg\\])*", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, "\\[img\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/img\\]", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, "\\[img=(\\d{1,4})[x|\\,](\\d{1,4})\\]\\s*([^\\[\\<\\r\\n]+?)\\s*\\[\\/img\\]", "{图片}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, "\\[attach\\](\\d+)(\\[/attach\\])*", "{附件}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, "\\s*\\[hide\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", "{隐藏内容}", RegexOptions.IgnoreCase);
            message = Regex.Replace(message, "\\s*\\[hide=(\\d+?)\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", "{隐藏内容}", RegexOptions.IgnoreCase);
            if (message.IndexOf("[free]") > -1)
            {
                Match match = Regex.Match(message, "\\s*\\[free\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/free\\]\\s*", RegexOptions.IgnoreCase);
                message = ((match.Groups[0] != null && match.Groups[0].Value != "") ? match.Groups[0].Value : message);
            }
            return Utils.GetSubString(Utils.ClearUBB(Utils.RemoveHtml(message)).Replace("{", "[").Replace("}", "]"), length, "......");
        }

        private static void DeleteCacheImageFile()
        {
            FileInfo[] files = new DirectoryInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/rotatethumbnail/")).GetFiles();
            if (files.Length > 100)
            {
                Attachments.QuickSort(files, 0, files.Length - 1);
                for (int i = files.Length - 1; i >= 50; i--)
                {
                    try
                    {
                        files[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}