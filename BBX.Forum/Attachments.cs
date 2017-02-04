using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Plugin.Preview;

namespace BBX.Forum
{
    public enum ThumbnailType
    {
        Square = 1,
        Thumbnail
    }
    public class Attachments
    {
        private const string ATTACH_TIP_IMAGE = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_{0}\"><img border=\"0\" src=\"{1}images/attachicons/attachimg.gif\" /></span>";
        private const string IMAGE_ATTACH = "{0}<img imageid=\"{2}\" src=\"{1}\" onmouseover=\"attachimginfo(this, 'attach_{2}', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_{2}', 0, event)\" {7} /><div id=\"attach_{2}_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"{3}images/attachicons/image.gif\" /><a target=\"_blank\"  href=\"{1}\"><strong>{4}</strong></a>({5})<br/><div class=\"t_smallfont\">{6}</div></div>";
        private const string IMAGE_ATTACH_MTDOWNLOAD = "{0}<img alt=\"点击加载图片\" imageid=\"{2}\" inpost=\"1\" src=\"/images/common/imgloading.png\" newsrc=\"{1}\" onload=\"{8}attachimg(this, 'load');\" onclick=\"loadImg(this);\" onmouseout=\"attachimginfo(this, 'attach_{2}', 0, event)\" {7} /><div id=\"attach_{2}_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"{3}images/attachicons/image.gif\" /><a target=\"_blank\" onclick=\"return ShowDownloadTip({9});\"  href=\"{1}\"><strong>{4}</strong></a>({5})<br/><div class=\"t_smallfont\">{6}</div></div>";
        private const string PAID_ATTACH = "{0}<strong>收费附件:{1}</strong>";
        private const string PAID_ATTACH_LINK = "<p>售价({0}):<strong>{1} </strong>[<a onclick=\"loadattachpaymentlog({2});\" href=\"javascript:void(0);\">记录</a>] [<a onclick=\"loadbuyattach({2});\" href=\"javascript:void(0);\">购买</a>]</p>";
        private const string ATTACH_INFO_DL = "<dl class=\"t_attachlist attachimg cl\">{0}</dl>";
        private const string ATTACH_INFO_DTIMG = "<dt><img src=\"{0}images/attachicons/{1}\" alt=\"\"></dt>";
        private const string ATTACH_INFO_DD = "<dd>{0}</dd>";
        private const string ATTACH_INFO_EM = "<em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr({0});</script>, 下载次数:{1})</em>";
        //private static bool appSquidAttachment = EntLibConfigInfo.Current != null && !EntLibConfigInfo.Current.Attachmentdir.IsNullOrEmpty();

        public static int[] CreateAttachments(Attachment[] atts)
        {
            int num2 = 0;
            int tid = 0;
            int pid = 0;
            int[] array = new int[atts.Length];
            int attType = 1;
            for (int i = 0; i < atts.Length; i++)
            {
                if (atts[i] != null)
                {
                    //array[i] = BBX.Data.Attachments.CreateAttachments(attachmentinfo[i]);
                    atts[i].Insert();
                    array[i] = atts[i].ID;
                    num2++;
                    tid = atts[i].Tid;
                    pid = atts[i].Pid;
                    //attachmentinfo[i].ID = array[i];
                    if (atts[i].FileType.StartsWithIgnoreCase("image"))
                    {
                        attType = 2;
                    }
                }
            }
            if (num2 > 0)
            {
                UpdateTopicAndPostAttachmentType(tid, pid, attType);
            }
            return array;
        }

        public static void UpdateTopicAndPostAttachmentType(int tid, int pid, int attType)
        {
            //BBX.Data.Topics.UpdateTopicAttachmentType(tid, attType);
            var tp = Topic.FindByID(tid);
            if (tp != null)
            {
                tp.Attachment = attType;
                tp.Save();
            }

            //BBX.Data.Posts.UpdatePostAttachmentType(tid, pid, attType);
            var pi = Post.FindByID(pid);
            if (pi != null)
            {
                pi.Attachment = attType;
                pi.Save();
            }
        }

        //public static AttachmentInfo GetAttachmentInfo(int aid)
        //{
        //	if (aid <= 0)
        //	{
        //		return null;
        //	}
        //	return BBX.Data.Attachments.GetAttachmentInfo(aid);
        //}

        //public static int GetAttachmentCountByPid(int pid)
        //{
        //	return BBX.Data.Attachments.GetAttachmentCountByPid(pid);
        //}

        //public static DataTable GetAttachmentListByPid(int pid)
        //{
        //	if (pid <= 0)
        //	{
        //		return null;
        //	}
        //	return BBX.Data.Attachments.GetAttachmentListByPid(pid);
        //}

        //public static DataTable GetAttachmentType()
        //{
        //	var cacheService = XCache.Current;
        //	DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE) as DataTable;
        //	if (dataTable == null)
        //	{
        //		dataTable = BBX.Data.Attachments.GetAttachmentType();
        //		XCache.Add(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE, dataTable);
        //	}
        //	return dataTable;
        //}

        //public static string GetAttachmentTypeArray(string filterexpression)
        //{
        //	DataTable attachmentType = Attachments.GetAttachmentType();
        //	StringBuilder stringBuilder = new StringBuilder();
        //	DataRow[] array = attachmentType.Select(filterexpression);
        //	for (int i = 0; i < array.Length; i++)
        //	{
        //		DataRow dataRow = array[i];
        //		stringBuilder.Append(dataRow["extension"]);
        //		stringBuilder.Append(",");
        //		stringBuilder.Append(dataRow["maxsize"]);
        //		stringBuilder.Append("|");
        //	}
        //	return stringBuilder.ToString().Trim();
        //}

        //public static string GetAttachmentTypeString(string filterexpression)
        //{
        //	DataTable attachmentType = Attachments.GetAttachmentType();
        //	StringBuilder stringBuilder = new StringBuilder();
        //	DataRow[] array = attachmentType.Select(filterexpression);
        //	for (int i = 0; i < array.Length; i++)
        //	{
        //		DataRow dataRow = array[i];
        //		if (!Utils.StrIsNullOrEmpty(stringBuilder.ToString()))
        //		{
        //			stringBuilder.Append(",");
        //		}
        //		stringBuilder.Append(dataRow["extension"]);
        //	}
        //	return stringBuilder.ToString().Trim();
        //}

        //public static string GetAllowAttachmentType(UserGroup usergroupinfo, IXForum forum)
        //{
        //	var sb = new StringBuilder();
        //	if (!usergroupinfo.AttachExtensions.IsNullOrWhiteSpace())
        //	{
        //		sb.Append("[id] in (");
        //		sb.Append(usergroupinfo.AttachExtensions);
        //		sb.Append(")");
        //	}
        //	if (!String.IsNullOrEmpty(forum.Attachextensions))
        //	{
        //		if (sb.Length > 0)
        //		{
        //			sb.Append(" AND ");
        //		}
        //		sb.Append("[id] in (");
        //		sb.Append(forum.Attachextensions);
        //		sb.Append(")");
        //	}
        //	return sb.ToString();
        //}

        //public static void UpdateAttachmentDownloads(int aid)
        //{
        //    if (aid > 0)
        //    {
        //        BBX.Data.Attachments.UpdateAttachmentDownloads(aid);
        //    }
        //}

        public static void UpdateTopicAttachment(string tidlist)
        {
            string[] array = Utils.SplitString(tidlist, ",");
            for (int i = 0; i < array.Length; i++)
            {
                //BBX.Data.Attachments.UpdateTopicAttachment(array[i].Trim().ToInt(-1));
                var tid = array[i].ToInt();
                var tp = Topic.FindByID(tid);
                if (tp != null)
                {
                    tp.Attachment = Attachment.FindCountByTid(tid);
                    tp.Save();
                }
            }
        }

        //public static int DeleteAttachmentByTid(int tid)
        //{
        //	if (tid <= 0)
        //	{
        //		return 0;
        //	}
        //	return BBX.Data.Attachments.DeleteAttachmentByTid(tid);
        //}

        //public static int DeleteAttachmentByTid(string tidlist)
        //{
        //	if (!Utils.IsNumericList(tidlist))
        //	{
        //		return -1;
        //	}
        //	return BBX.Data.Attachments.DeleteAttachmentByTid(tidlist);
        //}

        //public static int DeleteAttachment(int aid)
        //{
        //	if (aid <= 0)
        //	{
        //		return 0;
        //	}
        //	return BBX.Data.Attachments.DeleteAttachment(aid);
        //}

        //public static int UpdateAttachment(AttachmentInfo attachmentInfo)
        //{
        //    if (attachmentInfo == null || attachmentInfo.Aid <= 0)
        //    {
        //        return 0;
        //    }
        //    return BBX.Data.Attachments.UpdateAttachment(attachmentInfo);
        //}

        //public static int DeleteAttachment(string aidList)
        //{
        //	if (!Utils.IsNumericArray(aidList.Split(',')))
        //	{
        //		return -1;
        //	}
        //	return BBX.Data.Attachments.DeleteAttachment(aidList);
        //}

        //public static int GetUploadFileSizeByuserid(int uid)
        //{
        //    return BBX.Data.Attachments.GetUploadFileSizeByUserId(uid);
        //}

        /// <summary>过滤本地图片标记，替换为附件标签</summary>
        /// <param name="aid"></param>
        /// <param name="atts"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FilterLocalTags(int[] aid, Attachment[] atts, String msg)
        {
            for (int i = 0; i < aid.Length; i++)
            {
                if (aid[i] > 0)
                {
                    Regex regex = new Regex("\\[localimg=(\\d{1,}),(\\d{1,})\\]" + atts[i].Sys_index + "\\[\\/localimg\\]", RegexOptions.IgnoreCase);
                    Match match = regex.Match(msg);
                    while (match.Success)
                    {
                        msg = msg.Replace(match.Groups[0].ToString(), "[attachimg]" + aid[i] + "[/attachimg]");
                        match = match.NextMatch();
                    }
                    regex = new Regex("\\[local\\]" + atts[i].Sys_index + "\\[\\/local\\]", RegexOptions.IgnoreCase);
                    match = regex.Match(msg);
                    while (match.Success)
                    {
                        msg = msg.Replace(match.Groups[0].ToString(), "[attach]" + aid[i] + "[/attach]");
                        match = match.NextMatch();
                    }
                }
            }
            msg = Regex.Replace(msg, "\\[localimg=(\\d{1,}),\\s*(\\d{1,})\\][\\s\\S]+?\\[/localimg\\]", string.Empty, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "\\[local\\][\\s\\S]+?\\[/local\\]", string.Empty, RegexOptions.IgnoreCase);
            return msg;
        }

        public static int BindAttachment(Attachment[] atts, int topicId, int postId, int userId, UserGroup userGroupInfo)
        {
            int num = 0;
            for (int i = 0; i < atts.Length; i++)
            {
                if (atts[i] != null)
                {
                    if (atts[i].Pid == 0) num++;

                    var str = atts[i].ID.ToString();
                    atts[i].Uid = userId;
                    atts[i].Tid = topicId;
                    atts[i].Pid = postId;
                    atts[i].PostDateTime = DateTime.Now;
                    atts[i].ReadPerm = 0;
                    int num2 = DNTRequest.GetString("attachprice_" + str).ToInt(0);
                    atts[i].AttachPrice = num2 == 0 ? 0 : userGroupInfo.CheckMaxPrice(num2);
                    int num3 = DNTRequest.GetString("readperm_" + str).ToInt(0);
                    if (num3 != 0)
                    {
                        int num4 = num3;
                        num4 = ((num4 > 255) ? 255 : num4);
                        atts[i].ReadPerm = num4;
                    }
                    atts[i].Description = DNTRequest.GetHtmlEncodeString("attachdesc_" + str);
                }
            }
            return num;
        }

        public static string GetThumbnailByTid(int tid, int maxsize, ThumbnailType type)
        {
            int num = (maxsize < 300) ? maxsize : 300;
            string text = string.Format("{0}cache/thumbnail/t_{1}_{2}_{3}.jpg", new object[]
            {
                BaseConfigs.GetForumPath,
                tid,
                num,
                (int)type
            });
            //AttachmentInfo firstImageAttachByTid = BBX.Data.Attachments.GetFirstImageAttachByTid(tid);
            var att = Attachment.FindAllByTid(tid).ToList().FirstOrDefault(e => e.IsImage);
            if (att == null) return null;

            att.Name = text;
            string mapPath = Utils.GetMapPath(text);
            if (!File.Exists(mapPath))
            {
                Attachments.CreateTopicAttThumbnail(mapPath, type, att.FileName, num);
            }
            return text;
        }

        private static void CreateTopicAttThumbnail(string attPhyCachePath, ThumbnailType type, string attPhyPath, int theMaxsize)
        {
            if (!attPhyPath.StartsWith("http://"))
            {
                attPhyPath = Utils.GetMapPath(attPhyPath);
            }
            if (!attPhyPath.StartsWith("http://") && !File.Exists(attPhyPath))
            {
                return;
            }
            string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/thumbnail/");
            if (!Directory.Exists(mapPath))
            {
                try
                {
                    Utils.CreateDir(mapPath);
                }
                catch
                {
                    throw new Exception("请检查程序目录下cache文件夹的用户权限！");
                }
            }
            FileInfo[] files = new DirectoryInfo(mapPath).GetFiles();
            if (files.Length > 1500)
            {
                Attachments.QuickSort(files, 0, files.Length - 1);
                for (int i = files.Length - 1; i >= 1400; i--)
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
            try
            {
                switch (type)
                {
                    case ThumbnailType.Square:
                        if (attPhyPath.StartsWith("http://"))
                        {
                            Thumbnail.MakeRemoteSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        }
                        else
                        {
                            Thumbnail.MakeSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        }
                        break;

                    case ThumbnailType.Thumbnail:
                        if (attPhyPath.StartsWith("http://"))
                        {
                            Thumbnail.MakeRemoteThumbnailImage(attPhyPath, attPhyCachePath, theMaxsize, theMaxsize);
                        }
                        else
                        {
                            Thumbnail.MakeThumbnailImage(attPhyPath, attPhyCachePath, theMaxsize, theMaxsize);
                        }
                        break;

                    default:
                        if (attPhyPath.StartsWith("http://"))
                        {
                            Thumbnail.MakeRemoteSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        }
                        else
                        {
                            Thumbnail.MakeSquareImage(attPhyPath, attPhyCachePath, theMaxsize);
                        }
                        break;
                }
            }
            catch
            {
            }
        }

        private static int Partition(FileInfo[] arr, int low, int high)
        {
            FileInfo fileInfo = arr[low];
            while (low < high)
            {
                while (low < high && arr[high].CreationTime <= fileInfo.CreationTime)
                {
                    high--;
                }
                Attachments.Swap(ref arr[high], ref arr[low]);
                while (low < high && arr[low].CreationTime >= fileInfo.CreationTime)
                {
                    low++;
                }
                Attachments.Swap(ref arr[high], ref arr[low]);
            }
            arr[low] = fileInfo;
            return low;
        }

        private static void Swap(ref FileInfo i, ref FileInfo j)
        {
            FileInfo fileInfo = i;
            i = j;
            j = fileInfo;
        }

        public static void QuickSort(FileInfo[] arr, int low, int high)
        {
            if (low <= high - 1)
            {
                int num = Attachments.Partition(arr, low, high);
                Attachments.QuickSort(arr, low, num - 1);
                Attachments.QuickSort(arr, num + 1, high);
            }
        }

        //public static void DeleteAttachmentByPid(int pid)
        //{
        //	DataTable attachmentListByPid = Attachments.GetAttachmentListByPid(pid);
        //	if (attachmentListByPid != null || attachmentListByPid.Rows.Count != 0)
        //	{
        //		foreach (DataRow dataRow in attachmentListByPid.Rows)
        //		{
        //			string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + dataRow["filename"]);
        //			if (dataRow["filename"].ToString().Trim().ToLower().IndexOf("http") < 0 && File.Exists(mapPath))
        //			{
        //				File.Delete(mapPath);
        //			}
        //		}
        //	}
        //	BBX.Data.Attachments.DeleteAttachmentByPid(pid);
        //}

        //public static int GetUserAttachmentCount(int uid)
        //{
        //    if (uid >= 0)
        //    {
        //        return BBX.Data.Attachments.GetUserAttachmentCount(uid);
        //    }
        //    return 0;
        //}

        //public static int GetUserAttachmentCount(int uid, int typeid)
        //{
        //    return BBX.Data.Attachments.GetUserAttachmentCount(uid, Attachments.SetExtNamelist(typeid));
        //}

        //public static string SetExtNamelist(int typeid)
        //{
        //    string text = "";
        //    string[] array = Attachments.GetExtName(typeid).Split(',');
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        string str = array[i];
        //        text = text + "'" + str + "',";
        //    }
        //    return text.Remove(text.Length - 1, 1);
        //}

        //public static List<MyAttachmentInfo> GetAttachmentByUid(int uid, int typeid, int pageIndex, int pageSize)
        //{
        //    List<MyAttachmentInfo> result = new List<MyAttachmentInfo>();
        //    if (pageIndex <= 0)
        //    {
        //        return result;
        //    }
        //    return BBX.Data.Attachments.GetAttachmentByUid(uid, typeid, pageIndex, pageSize, Attachments.SetExtNamelist(typeid));
        //}

        //public static List<AttachmentType> AttachTypeList()
        //{
        //    return BBX.Data.Attachments.AttachTypeList();
        //}

        //public static string GetExtName(int typeid)
        //{
        //    var ts = MyAttachmentsTypeConfigInfo.Current.AttachmentType;
        //    for (int i = 0; i < ts.Length; i++)
        //    {
        //        var ti = ts[i];
        //        if (ti.TypeId == typeid)
        //        {
        //            return "." + ti.ExtName.Replace(",", ",.");
        //        }
        //    }
        //    return "";
        //}

        //public static void UpdateSLUploadAttachInfo(int topicid, int postid, UserGroup usergroupinfo)
        //{
        //	if (GeneralConfigInfo.Current.Silverlight != 1 || Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("attachid")))
        //	{
        //		return;
        //	}
        //	string[] array = DNTRequest.GetFormString("attachid").Split(',');
        //	if (String.IsNullOrEmpty(array[0]))
        //	{
        //		array[0] = "0";
        //	}
        //	if (Utils.IsNumericArray(array))
        //	{
        //		string[] descs = DNTRequest.GetFormString("sl_attachdesc").Split(',');
        //		string[] readpeams = DNTRequest.GetFormString("sl_readperm").Split(',');
        //		string[] prices = (DNTRequest.GetString("sl_attachprice") == null) ? null : DNTRequest.GetString("sl_attachprice").Split(',');
        //		int num = 0;
        //		int num2 = 0;
        //		while (num2 < array.Length && num2 <= GeneralConfigInfo.Current.Maxattachments)
        //		{
        //			int aid = array[num2].ToInt(0);
        //			if (aid > 0 && Utils.IsSafeSqlString(descs[num2]))
        //			{
        //				//var att = Attachments.GetAttachmentInfo(aid);
        //				var att = Attachment.FindByID(aid);
        //				att.Description = descs[num2];
        //				att.ReadPerm = ((readpeams.Length == array.Length) ? readpeams[num2].ToInt(0) : 0);
        //				att.Tid = topicid;
        //				att.Pid = postid;
        //				att.AttachPrice = ((prices.Length == array.Length) ? UserGroups.CheckUserGroupMaxPrice(usergroupinfo, prices[num2].ToInt(0)) : 0);
        //				//Attachments.UpdateAttachment(att);
        //				att.Save();
        //				num = (att.FileType.ToLower().StartsWith("image") ? 2 : 1);
        //			}
        //			num2++;
        //		}
        //		if (num > 0)
        //		{
        //			Attachments.UpdateTopicAndPostAttachmentType(topicid, postid, num);
        //		}
        //	}
        //}

        //public static void DeleteNoUsedForumAttachment()
        //{
        //    BBX.Data.Attachments.DeleteNoUsedForumAttachment();
        //}

        public static List<Attachment> GetAttachmentList(PostpramsInfo postpramsInfo, string pidList)
        {
            var list = new List<Attachment>();
            if (!pidList.IsNullOrEmpty())
            {
                //list = BBX.Data.Attachments.GetAttachmentListByPidList(pidList.ToString());
                var atts = Attachment.FindAllByPids(pidList);
                foreach (var item in atts)
                {
                    list.Add(item);
                }
                Attachments.CheckPurchasedAttachments(list, postpramsInfo.CurrentUserid);
            }
            return list;
        }

        public static void CheckPurchasedAttachments(List<Attachment> attachList, int uid)
        {
            //string ids = "";
            //foreach (var item in attachList)
            //{
            //    //if (current.Filename.IndexOf("http") < 0 && Attachments.appSquidAttachment)
            //    //{
            //    //    current.Filename = EntLibConfigInfo.Current.Attachmentdir.TrimEnd('/') + "/" + current.Filename;
            //    //}
            //    ids = Utils.MergeString(item.Aid.ToString(), ids);
            //}
            var ids = attachList.Select(e => e.ID).ToArray();
            if (ids.Length > 0 && uid > 0)
            {
                //ids = BBX.Data.Attachments.GetPurchasedAttachmentIdList(ids, uid);
                ids = AttachPaymentLog.GetPurchasedAttachmentIdList(ids, uid);
                foreach (var att in attachList)
                {
                    //if (att.Attachprice > 0 && Utils.InArray(att.Aid.ToString(), ids))
                    if (att.AttachPrice > 0 && ids.Contains(att.ID))
                    {
                        att.IsBought = true;
                    }
                }
            }
        }

        private static string GetImageShowWidth(int templateWidth, int attachWidth)
        {
            if (attachWidth < 0 || templateWidth < attachWidth)
            {
                return string.Format("width={0}", templateWidth);
            }
            return "";
        }

        private static string GetAutoImageWidthScript(int templateWidth, int attachWidth, bool needOnLoad)
        {
            string text = string.Empty;
            if (attachWidth == 0)
            {
                text = string.Format("if(this.width > {0}) this.width = {0};", templateWidth);
            }
            if (needOnLoad)
            {
                text = string.Format("onload=\"{0}\"", text);
            }
            return text;
        }

        private static string ParseImageAttachContent(PostpramsInfo postPramsInfo, Attachment att, string fileSize)
        {
            string empty = string.Empty;
            string forumpath = BaseConfigInfo.Current.Forumpath;
            string text = string.Format("<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_{0}\"><img border=\"0\" src=\"{1}images/attachicons/attachimg.gif\" /></span>", att.ID, forumpath);
            string str = string.Format("<dt><img src=\"{0}images/attachicons/{1}\" alt=\"\"></dt>", forumpath, "image.gif");
            string str2 = string.Format("<em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr({0});</script>, 下载次数:{1})</em>", att.FileSize, att.Downloads);
            bool flag = GeneralConfigInfo.Current.Showimgattachmode == 0;
            string format = flag ? "{0}<img imageid=\"{2}\" src=\"{1}\" onmouseover=\"attachimginfo(this, 'attach_{2}', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_{2}', 0, event)\" {7} /><div id=\"attach_{2}_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"{3}images/attachicons/image.gif\" /><a target=\"_blank\"  href=\"{1}\"><strong>{4}</strong></a>({5})<br/><div class=\"t_smallfont\">{6}</div></div>" : "{0}<img alt=\"点击加载图片\" imageid=\"{2}\" inpost=\"1\" src=\"/images/common/imgloading.png\" newsrc=\"{1}\" onload=\"{8}attachimg(this, 'load');\" onclick=\"loadImg(this);\" onmouseout=\"attachimginfo(this, 'attach_{2}', 0, event)\" {7} /><div id=\"attach_{2}_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"{3}images/attachicons/image.gif\" /><a target=\"_blank\" onclick=\"return ShowDownloadTip({9});\"  href=\"{1}\"><strong>{4}</strong></a>({5})<br/><div class=\"t_smallfont\">{6}</div></div>";
            string text2 = flag ? Attachments.GetImageShowWidth(postPramsInfo.TemplateWidth, att.Width) : "";
            //att.Attachimgpost = 1;
            if (postPramsInfo.Showattachmentpath == 1)
            {
                if (postPramsInfo.Isforspace == 1)
                {
                    if (att.IsLocal)
                    {
                        return string.Format("<img src=\"{0}\" {1} {2}/>", att.FileName, Attachments.GetImageShowWidth(postPramsInfo.TemplateWidth, att.Width), Attachments.GetAutoImageWidthScript(postPramsInfo.TemplateWidth, att.Width, true));
                    }
                    return string.Format("<img src=\"{0}upload/{1}\" {2} {3}/>", new object[]
                    {
                        forumpath,
                        att.FileName,
                        Attachments.GetImageShowWidth(postPramsInfo.TemplateWidth, att.Width),
                        Attachments.GetAutoImageWidthScript(postPramsInfo.TemplateWidth, att.Width, true)
                    });
                }
                else
                {
                    if (postPramsInfo.CurrentUserGroup.Is管理团队 || postPramsInfo.CurrentUserid == att.Uid || att.AttachPrice <= 0 || att.IsBought)
                    {
                        if (att.IsLocal)
                        {
                            return string.Format(format, new object[]
                            {
                                text,
                                att.FileName,
                                att.ID,
                                forumpath,
                                att.Name.Trim(),
                                fileSize,
                                att.PostDateTime,
                                text2,
                                Attachments.GetAutoImageWidthScript(postPramsInfo.TemplateWidth, att.Width, false),
                                att.Uid
                            });
                        }
                        return string.Format(format, new object[]
                        {
                            text,
                            forumpath + "upload/" + att.FileName,
                            att.ID,
                            forumpath,
                            att.Name.Trim(),
                            fileSize,
                            att.PostDateTime,
                            text2,
                            Attachments.GetAutoImageWidthScript(postPramsInfo.TemplateWidth, att.Width, false),
                            att.Uid
                        });
                    }
                    else
                    {
                        string.Format("<dl class=\"t_attachlist attachimg cl\">{0}</dl>", str + string.Format("<dd>{0}</dd>", att.Name + str2 + string.Format("<p>售价({0}):<strong>{1} </strong>[<a onclick=\"loadattachpaymentlog({2});\" href=\"javascript:void(0);\">记录</a>] [<a onclick=\"loadbuyattach({2});\" href=\"javascript:void(0);\">购买</a>]</p>", Scoresets.GetTopicAttachCreditsTransName(), att.AttachPrice, att.ID)));
                    }
                }
            }
            if (postPramsInfo.Isforspace == 1)
            {
                return string.Format("<img src=\"{0}attachment.aspx?attachmentid={1}\" {2} {3}/>", new object[]
                {
                    forumpath,
                    att.ID,
                    Attachments.GetImageShowWidth(postPramsInfo.TemplateWidth, att.Width),
                    Attachments.GetAutoImageWidthScript(postPramsInfo.TemplateWidth, att.Width, true)
                });
            }
            if (postPramsInfo.CurrentUserGroup.Is管理团队 || postPramsInfo.CurrentUserid == att.Uid || att.AttachPrice <= 0 || att.IsBought)
            {
                return string.Format(format, new object[]
                {
                    text,
                    forumpath + "attachment.aspx?attachmentid=" + att.ID,
                    att.ID,
                    forumpath,
                    att.Name.Trim(),
                    fileSize,
                    att.PostDateTime,
                    text2,
                    Attachments.GetAutoImageWidthScript(postPramsInfo.TemplateWidth, att.Width, false),
                    att.Uid
                });
            }
            return string.Format("<dl class=\"t_attachlist attachimg cl\">{0}</dl>", str + string.Format("<dd>{0}</dd>", att.Name + str2 + string.Format("<p>售价({0}):<strong>{1} </strong>[<a onclick=\"loadattachpaymentlog({2});\" href=\"javascript:void(0);\">记录</a>] [<a onclick=\"loadbuyattach({2});\" href=\"javascript:void(0);\">购买</a>]</p>", Scoresets.GetTopicAttachCreditsTransName(), att.AttachPrice, att.ID)));
        }

        private static string GetAttachReplacement(PostpramsInfo postPramsInfo, int allowGetAttach, Attachment att)
        {
            string forumpath = BaseConfigInfo.Current.Forumpath;
            string arg = "attachment.gif";
            if (Utils.InArray(Utils.GetFileExtName(att.FileName), ".rar,.zip"))
            {
                arg = "rar.gif";
            }
            string str = string.Format("<dt><img src=\"{0}images/attachicons/{1}\" alt=\"\"></dt>", forumpath, arg);
            string result;
            if (allowGetAttach != 1)
            {
                result = string.Format("<dl class=\"t_attachlist attachimg cl\">{0}</dl>", str + string.Format("<dd>{0}</dd>", "<div class=\"hide\"><span class=\"attachnotdown\">您所在的用户组无法下载或查看附件</span></div>"));
            }
            else
            {
                if (att.AllowRead)
                {
                    string fileSize;
                    if (att.FileSize > 1024L)
                    {
                        fileSize = Convert.ToString(Math.Round(Convert.ToDecimal(att.FileSize) / 1024m, 2)) + " K";
                    }
                    else
                    {
                        fileSize = att.FileSize + " B";
                    }
                    string str2 = string.Format("<em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr({0});</script>, 下载次数:{1})</em>", att.FileSize, att.Downloads);
                    if (att.ImgPost)
                    {
                        result = ParseImageAttachContent(postPramsInfo, att, fileSize);
                    }
                    else
                    {
                        //att.Attachimgpost = 0;
                        if (postPramsInfo.CurrentUserGroup.Is管理团队 || postPramsInfo.CurrentUserid == att.Uid || att.AttachPrice <= 0 || att.IsBought)
                        {
                            result = string.Format("<dl class=\"t_attachlist attachimg cl\">{0}</dl>", str + string.Format("<dd>{0}</dd>", string.Format("<a href=\"{0}attachment.aspx?attachmentid={1}\" onclick=\"return ShowDownloadTip({2});\" target=\"_blank\">{3}</a>", new object[]
                            {
                                forumpath,
                                att.ID,
                                att.Uid,
                                att.Name
                            }) + str2));
                        }
                        else
                        {
                            result = string.Format("<dl class=\"t_attachlist attachimg cl\">{0}</dl>", str + string.Format("<dd>{0}</dd>", att.Name + str2 + string.Format("<p>售价({0}):<strong>{1} </strong>[<a onclick=\"loadattachpaymentlog({2});\" href=\"javascript:void(0);\">记录</a>] [<a onclick=\"loadbuyattach({2});\" href=\"javascript:void(0);\">购买</a>]</p>", Scoresets.GetTopicAttachCreditsTransName(), att.AttachPrice, att.ID)));
                        }
                    }
                }
                else
                {
                    if (postPramsInfo.CurrentUserid > 0)
                    {
                        result = string.Format("<div class=\"hide\"><span class=\"attachnotdown\">你的下载权限 {0} 低于此附件所需权限 {1}, 你无权查看此附件</span></div>", postPramsInfo.Usergroupreadaccess, att.ReadPerm);
                    }
                    else
                    {
                        result = string.Format("<div class=\"hide\">附件: <em><span class=\"attachnotdown\">你需要<a href=\"{0}login.aspx\" onclick=\"hideWindow('register');showWindow('login', this.href);\">登录</a>才可以下载或查看附件。没有帐号? <a href=\"{0}register.aspx\" onclick=\"hideWindow('login');showWindow('register', this.href);\" title=\"注册帐号\">注册</a></span></em></div>", BaseConfigs.GetForumPath);
                    }
                }
            }
            return result;
        }

        public static string GetMessageWithAttachInfo(PostpramsInfo postPramsInfo, int allowGetAttach, string[] hideAttachIdArray, Int32 pid, Int32 posterid, Attachment attachInfo, string message)
        {
            if (Utils.InArray(attachInfo.ID.ToString(), hideAttachIdArray)) return message;

            if ((attachInfo.ReadPerm <= postPramsInfo.Usergroupreadaccess || posterid == postPramsInfo.CurrentUserid) && allowGetAttach == 1)
                attachInfo.AllowRead = true;
            else
                attachInfo.AllowRead = false;

            attachInfo.Getattachperm = allowGetAttach;
            attachInfo.FileName = attachInfo.FileName.ToString().Replace("\\", "/");
            if (message.IndexOf("[attach]" + attachInfo.ID + "[/attach]") != -1 || 
                message.IndexOf("[attachimg]" + attachInfo.ID + "[/attachimg]") != -1)
            {
                string attachReplacement = Attachments.GetAttachReplacement(postPramsInfo, allowGetAttach, attachInfo);
                Regex regex = new Regex(string.Format("\\[attach\\]{0}\\[/attach\\]|\\[attachimg\\]{0}\\[/attachimg\\]", attachInfo.ID));
                message = regex.Replace(message, attachReplacement, 1);
                message = message.Replace("[attach]" + attachInfo.ID + "[/attach]", string.Empty);
                message = message.Replace("[attachimg]" + attachInfo.ID + "[/attachimg]", string.Empty);
                if (attachInfo.Pid == pid)
                {
                    attachInfo.Inserted = true;
                }
            }
            else
            {
                if (attachInfo.Pid == pid)
                {
                    //attachInfo.Attachimgpost = (Attachment.IsImgFilename(attachInfo.Name) ? 1 : 0);
                    var instance = PreviewProvider.GetInstance(Path.GetExtension(attachInfo.FileName).Remove(0, 1).Trim());
                    if (instance != null)
                    {
                        if (attachInfo.FileName.StartsWith("http://") || attachInfo.FileName.StartsWith("ftp://"))
                        {
                            instance.UseFTP = true;
                            attachInfo.Preview = instance.GetPreview(attachInfo.FileName, attachInfo);
                        }
                        else
                        {
                            instance.UseFTP = false;
                            string fileName = "";
                            if (!attachInfo.FileName.Contains("://"))
                            {
                                fileName = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/" + attachInfo.FileName);
                            }
                            attachInfo.Preview = instance.GetPreview(fileName, attachInfo);
                        }
                    }
                }
            }
            return message;
        }

        //public static void DeleteAttchType(string idList)
        //{
        //	if (Utils.IsNumericList(idList))
        //	{
        //		BBX.Data.Attachments.DeleteAttchType(idList);
        //		//XCache.Remove(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
        //	}
        //}

        //public static void AddAttchType(string extension, string maxSize)
        //{
        //	BBX.Data.Attachments.AddAttchType(extension, maxSize);
        //	//XCache.Remove(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
        //	//Attachments.GetAttachmentType();
        //}

        //public static void UpdateAttchType(string extension, string maxSize, int id)
        //{
        //	if (id > 0)
        //	{
        //		BBX.Data.Attachments.UpdateAttchType(extension, maxSize, id);
        //	}
        //}

        //public static DataTable GetAttachList(string condition, string postName)
        //{
        //	return BBX.Data.Attachments.GetAttachList(condition, postName);
        //}

        //public static string SearchAttachment(int forumid, string posttablename, string filesizemin, string filesizemax, string downloadsmin, string downloadsmax, string postdatetime, string filename, string description, string poster)
        //{
        //    return BBX.Data.Attachments.SearchAttachment(forumid, posttablename, filesizemin, filesizemax, downloadsmin, downloadsmax, postdatetime, filename, description, poster);
        //}

        //public static bool UpdateAttachment(Attachment[] attachs, int topicId, int postId, PostInfo postInfo, ref StringBuilder returnMsg, int userId, GeneralConfigInfo config, UserGroup userGroupInfo)
        //{
        //	if (attachs == null) return false;

        //	if (attachs.Length > config.Maxattachments)
        //	{
        //		returnMsg = new StringBuilder();
        //		returnMsg.AppendFormat("您添加了{0}个图片/附件,多于系统设置的{1}个.<br/>请重新编辑该帖并删除多余图片/附件.", attachs.Length, config.Maxattachments);
        //		return false;
        //	}

        //	int num = Attachments.BindAttachment(attachs, topicId, postId, userId, userGroupInfo);
        //	int[] array = new int[attachs.Length];
        //	int num2 = 0;
        //	for (int i = 0; i < attachs.Length; i++)
        //	{
        //		//Attachments.UpdateAttachment(attachs[i]);
        //		attachs[i].Save();
        //		array[i] = attachs[i].ID;
        //		num2 = ((num2 != 3 && attachs[i].FileType.ToLower().StartsWith("image")) ? ((num2 == 1) ? 3 : 2) : ((num2 == 2) ? 3 : 1));
        //	}
        //	string text = Attachments.FilterLocalTags(array, attachs, postInfo.Message);
        //	if (text != postInfo.Message)
        //	{
        //		postInfo.Message = text;
        //		postInfo.Pid = postId;
        //		Posts.UpdatePost(postInfo);
        //	}
        //	if (num > 0)
        //	{
        //		CreditsFacade.UploadAttachments(userId, num);
        //	}
        //	Attachments.UpdateTopicAndPostAttachmentType(topicId, postId, num2);
        //	return true;
        //}

        public static bool UpdateAttachment(Attachment[] attachs, int topicId, int postId, Post postInfo, ref StringBuilder returnMsg, int userId, GeneralConfigInfo config, UserGroup userGroupInfo)
        {
            if (attachs == null) return false;

            if (attachs.Length > config.Maxattachments)
            {
                returnMsg = new StringBuilder();
                returnMsg.AppendFormat("您添加了{0}个图片/附件,多于系统设置的{1}个.<br/>请重新编辑该帖并删除多余图片/附件.", attachs.Length, config.Maxattachments);
                return false;
            }

            int num = Attachments.BindAttachment(attachs, topicId, postId, userId, userGroupInfo);
            int[] array = new int[attachs.Length];
            int num2 = 0;
            for (int i = 0; i < attachs.Length; i++)
            {
                //Attachments.UpdateAttachment(attachs[i]);
                attachs[i].Save();
                array[i] = attachs[i].ID;
                num2 = ((num2 != 3 && attachs[i].FileType.ToLower().StartsWith("image")) ? ((num2 == 1) ? 3 : 2) : ((num2 == 2) ? 3 : 1));
            }
            string text = Attachments.FilterLocalTags(array, attachs, postInfo.Message);
            if (text != postInfo.Message)
            {
                postInfo.Message = text;
                //postInfo.Pid = postId;
                //Posts.UpdatePost(postInfo);
                postInfo.Save();
            }
            if (num > 0)
            {
                CreditsFacade.UploadAttachments(userId, num);
            }
            UpdateTopicAndPostAttachmentType(topicId, postId, num2);
            return true;
        }

        public static Attachment[] GetNoUsedAttachmentArray(int userid, string aidList)
        {
            var list = Attachment.FindAllNoUsed(userid, DateTime.MinValue);
            if (list.Count <= 0) return null;

            var aids = aidList.SplitAsInt();
            return list.ToList().Where(e => aids.Contains(e.ID)).ToArray();
        }

        //public static AttachmentInfo[] GetEditPostAttachArray(int userid, string attachId)
        //{
        //    return BBX.Data.Attachments.GetEditPostAttachList(userid, attachId).ToArray();
        //}

        public static string GetImageAttachmentTypeString(string allowFormats)
        {
            if (String.IsNullOrEmpty(allowFormats))
            {
                allowFormats = "jpg,gif,png,jpeg,bmp";
            }
            else
            {
                string[] array = allowFormats.Split(',');
                allowFormats = "";
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    if (Utils.InArray(text, "jpg,gif,png,jpeg,bmp"))
                    {
                        allowFormats = allowFormats + text + ",";
                    }
                }
            }
            return allowFormats.TrimEnd(',');
        }
    }
}