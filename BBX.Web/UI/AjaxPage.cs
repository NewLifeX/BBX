using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Log;
using NewLife.Web;

namespace BBX.Web.UI
{
    public class AjaxPage //: Page
    {
        HttpRequest Request { get { return HttpContext.Current.Request; } }

        HttpResponse Response { get { return HttpContext.Current.Response; } }

        public static void Process()
        {
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetUrlReferrer()))
            {
                string[] array = DecodeUid(DNTRequest.GetString("input")).Split(',');
                var userInfo = Users.GetUserInfo(array[0].ToInt());
                if (userInfo == null || DNTRequest.GetString("appid") != Utils.MD5(userInfo.Name + userInfo.Password + userInfo.ID + array[1])) return;
            }
            else
            {
                if (ForumUtils.IsCrossSitePost()) return;
            }
            string t = DNTRequest.GetString("t");
            try
            {
                if (Utils.InArray(t, "deleteattach,getattachlist,deletepostsbyuidanddays,deletepost,ignorepost,passpost,deletetopic,ignoretopic,passtopic,getimagelist,getblocklist,getpagelist,forumtree,topictree,quickreply,report,getdebatepostpage,confirmbuyattach,getnewpms,getnewnotifications,getajaxforums,checkuserextcredit,diggdebates,imagelist,debatevote"))
                {
                    //HttpContext.Current.Server.Transfer("sessionajax.aspx?t=" + t + "&reason=" + DNTRequest.GetString("reason"));
                    //return;
                    new SessionAjaxPage();
                }
                else
                    new AjaxPage();
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }
        }

        public AjaxPage()
        {
            //this.config = GeneralConfigInfo.Current;
            string t = DNTRequest.GetString("t");
            switch (t)
            {
                case "checkusername":
                    this.CheckUserName();
                    break;

                case "checkrewritename":
                    //this.CheckRewriteName();
                    break;

                case "ratelist":
                    this.GetRateLogList();
                    break;

                case "smilies":
                    this.GetSmilies();
                    break;

                case "relatekw":
                    this.GetRelateKeyword();
                    break;

                case "gettopictags":
                    this.GetTopicTags();
                    break;

                case "topicswithsametag":
                    this.GetTopicsWithSameTag();
                    break;

                case "getforumhottags":
                    this.GetForumHotTags();
                    break;

                case "gethotdebatetopic":
                    this.Getdebatesjsonlist("gethotdebatetopic", DNTRequest.GetString("tidlist", true));
                    break;

                case "recommenddebates":
                    this.Getdebatesjsonlist("recommenddebates", DNTRequest.GetString("tidlist", true));
                    break;

                case "addcommentdebates":
                    this.ResponseXML(Debates.CommentDabetas(DNTRequest.GetInt("tid", 0), DNTRequest.GetString("commentdebates", true), DNTRequest.IsPost()));
                    break;

                case "getpostinfo":
                    this.GetPostInfo();
                    break;

                case "getattachpaymentlog":
                    this.GetAttachPaymentLogByAid(DNTRequest.GetInt("aid", 0));
                    break;

                case "getiplist":
                    this.GetIpList();
                    break;

                case "getforumtopictypelist":
                    this.GetForumTopicTypeList();
                    break;

                case "image":
                    this.GetImage();
                    break;

                case "resetemail":
                    this.ResetEmail();
                    break;

                case "colorfulltags":
                    ColorFullTags();
                    break;

                case "closedtags":
                    ClosedTags();
                    break;
            }
            if (DNTRequest.GetString("Filename") != "" && DNTRequest.GetString("Upload") != "")
            {
                var uid = DecodeUid(DNTRequest.GetString("input")).Split(',')[0].ToInt(-1);
                this.ResponseText(this.UploadTempAvatar(uid));
                return;
            }
            if (DNTRequest.GetString("avatar1") != "" && DNTRequest.GetString("avatar2") != "" && DNTRequest.GetString("avatar3") != "")
            {
                var uid = DecodeUid(DNTRequest.GetString("input")).Split(',')[0];
                //this.CreateDir(uid);
                if (!this.SaveAvatar("avatar1", uid) || !this.SaveAvatar("avatar2", uid) || !this.SaveAvatar("avatar3", uid))
                {
                    //File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                    this.ResponseText("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
                    return;
                }
                //File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\temp\\avatar_" + uid + ".jpg"));
                this.ResponseText("<?xml version=\"1.0\" ?><root><face success=\"1\"/></root>");
            }
        }

        static string DecodeUid(string encodeUid)
        {
            return DES.Decode(encodeUid.Replace(' ', '+'), GeneralConfigInfo.Current.Passwordkey);
        }

        private void GetIpList()
        {
            try
            {
                string[] array = Utils.SplitString(DNTRequest.GetString("iplist"), ",");
                string[] array2 = Utils.SplitString(DNTRequest.GetString("pidlist"), ",");
                var sb = new StringBuilder();
                sb.Append("[");
                for (int i = 0; i < array.Length; i++)
                {
                    sb.Append("'");
                    sb.Append(array2[i]);
                    sb.Append("|");
                    sb.Append(IPAddress.Parse(array[i].Replace("*", "1")).GetAddress());
                    sb.Append("'");
                    sb.Append(",");
                }
                this.ResponseJSON(sb.ToString().TrimEnd(',') + "]");
            }
            catch
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = 0;
                HttpContext.Current.Response.Cache.SetNoStore();
                HttpContext.Current.Response.End();
            }
        }

        //private void CreateDir(string uid)
        //{
        //    uid = Avatars.FormatUid(uid);
        //    string strPath = string.Format("{0}avatars/upload/{1}/{2}/{3}", new object[]
        //    {
        //        BaseConfigs.GetForumPath,
        //        uid.Substring(0, 3),
        //        uid.Substring(3, 2),
        //        uid.Substring(5, 2)
        //    });
        //    if (!Directory.Exists(Utils.GetMapPath(strPath)))
        //    {
        //        Directory.CreateDirectory(Utils.GetMapPath(strPath));
        //    }
        //}

        private bool SaveAvatar(string avatar, string uid)
        {
            var buf = this.FlashDataDecode(DNTRequest.GetString(avatar));
            if (buf.Length == 0) return false;

            //uid = Avatars.FormatUid(uid);
            string size;
            if (avatar == "avatar1")
                size = "large";
            else if (avatar == "avatar2")
                size = "medium";
            else
                size = "small";

            //string strPath = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_{5}.jpg", new object[]
            //{
            //    BaseConfigs.GetForumPath,
            //    uid.Substring(0, 3),
            //    uid.Substring(3, 2),
            //    uid.Substring(5, 2),
            //    uid.Substring(7, 2),
            //    size
            //});
            var strPath = Avatars.FormatPathPrefix(uid) + size + ".jpg";
            File.WriteAllBytes(strPath.EnsureDirectory(), buf);
            //FileStream fileStream = new FileStream(Utils.GetMapPath(strPath), FileMode.Create);
            //fileStream.Write(buf, 0, buf.Length);
            //fileStream.Close();
            return true;
        }

        private byte[] FlashDataDecode(string s)
        {
            byte[] array = new byte[s.Length / 2];
            int length = s.Length;
            for (int i = 0; i < length; i += 2)
            {
                int num = (int)(s[i] - '0');
                num -= ((num > 9) ? 7 : 0);
                int num2 = (int)(s[i + 1] - '0');
                num2 -= ((num2 > 9) ? 7 : 0);
                array[i / 2] = (byte)(num << 4 | num2);
            }
            return array;
        }

        private string UploadTempAvatar(Int32 uid)
        {
            if (uid <= 0) return "unvaliable request!";

            string str = "avatar_" + uid + ".jpg";
            string str2 = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "upload/";
            string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload\\");
            if (!Directory.Exists(mapPath + "temp\\"))
            {
                Utils.CreateDir(mapPath + "temp\\");
            }
            str = "temp/" + str;
            HttpContext.Current.Request.Files[0].SaveAs(mapPath + str);
            return str2 + str;
        }

        private void GetPostInfo()
        {
            var tid = DNTRequest.GetInt("tid", 0);
            //var postInfo = Posts.GetPostInfo(tid, Post.FindByTid(tid).ID);
            // 直接就可以找到主贴了，为何还要兜兜转转
            var postInfo = Post.FindByTid(tid);
            var sb = this.IsValidGetPostInfo(postInfo);
            if (!sb.ToString().Contains("<error>"))
            {
                sb.Append("<post>\r\n\t");
                sb.AppendFormat("<message>{0}</message>\r\n", postInfo.Message);
                sb.AppendFormat("<tid>{0}</tid>\r\n", postInfo.Tid);
                sb.Append("</post>\r\n\t");
            }
            this.ResponseXML(sb);
        }

        private StringBuilder IsValidGetPostInfo(Post info)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                sb.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return sb;
            }
            if (info == null)
            {
                sb.Append("<error>读取帖子失败</error>");
                return sb;
            }
            return sb;
        }

        private void ResponseText(string text)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }

        private void ResponseText(StringBuilder builder)
        {
            this.ResponseText(builder.ToString());
        }

        private void GetForumTopicTypeList()
        {
            int fid = DNTRequest.GetInt("fid", 0);
            if (fid <= 0)
            {
                this.ResponseText("[]");
            }
            var forumInfo = Forums.GetForumInfo(fid);
            if (forumInfo == null)
            {
                this.ResponseText("[]");
            }
            if (string.IsNullOrEmpty(forumInfo.TopicTypes))
            {
                this.ResponseText("[]");
            }
            var sb = new StringBuilder("[{'typeid':'0','typename':'分类'}");
            string[] array = forumInfo.TopicTypes.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (!Utils.StrIsNullOrEmpty(text.Trim()))
                {
                    sb.Append(",{");
                    sb.AppendFormat("'typeid':'{0}','typename':'{1}'", text.Split(',')[0], text.Split(',')[1]);
                    sb.Append("}");
                }
            }
            sb.Append("]");
            this.ResponseText(sb);
        }

        private void GetForumHotTags()
        {
            //string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "cache\\tag\\hottags_forum_cache_jsonp.txt");
            //if (!File.Exists(mapPath))
            //{
            //    ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);
            //}
            //this.WriteFile(mapPath);
            var json = Tag.GetHotTagsListForForumJSONP(60);
            HttpContext.Current.Response.Write(json);
        }

        private void GetTopicsWithSameTag()
        {
            if (DNTRequest.GetInt("tagid", 0) > 0)
            {
                var tag = Tag.FindByID(DNTRequest.GetInt("tagid", 0));
                if (tag != null)
                {
                    var topicsWithSameTag = Topic.GetTopicListByTagId(DNTRequest.GetInt("tagid", 0), 1, GeneralConfigInfo.Current.Tpp);
                    var stringBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    stringBuilder.Append("<root><![CDATA[ \r\n");
                    stringBuilder.Append("<div class=\"tagthread\" style=\"width:300px\">\r\n                                <a class=\"close\" href=\"javascript:;hideMenu()\" title=\"关闭\"><img src=\"images/common/close.gif\" alt=\"关闭\" /></a>\r\n                                <h4>标签: ");
                    stringBuilder.Append(string.Format("<font color='{1}'>{0}</font>", tag.Name, tag.Color));
                    stringBuilder.Append("</h4>\r\n<ul>\r\n");
                    foreach (var item in topicsWithSameTag)
                    {
                        stringBuilder.Append(string.Format("<li><a href=\"{0}\" target=\"_blank\">{1}</a></li>", Urls.ShowTopicAspxRewrite(item.ID, 1), item.Title));
                    }
                    stringBuilder.Append(string.Format("<li class=\"more\"><a href=\"tags.aspx?tagid={0}\" target=\"_blank\">查看更多</a></li>", tag.ID));
                    stringBuilder.Append("</ul>\r\n");
                    stringBuilder.Append("</div>\r\n                                ]]></root>");
                    this.ResponseXML(stringBuilder);
                }
            }
        }

        private void GetTopicTags()
        {
            var topicid = WebHelper.RequestInt("topicid");
            if (topicid > 0)
            {
                //StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.Append(BaseConfigs.GetForumPath);
                //stringBuilder.Append("cache/topic/magic/");
                //stringBuilder.Append((DNTRequest.GetInt("topicid", 0) / 1000 + 1).ToString());
                //stringBuilder.Append("/");
                //string mapPath = Utils.GetMapPath(stringBuilder.ToString() + DNTRequest.GetInt("topicid", 0) + "_tags.config");
                //if (!File.Exists(mapPath))
                //{
                //    ForumTags.WriteTopicTagsCacheFile(DNTRequest.GetInt("topicid", 0));
                //}
                //this.WriteFile(mapPath);

                var str = Tag.GetTopicTags(topicid);
                HttpContext.Current.Response.Write(str);
            }
        }

        private void GetRelateKeyword()
        {
            string arg = Utils.UrlEncode(Utils.RemoveHtml(Utils.ClearUBB(DNTRequest.GetString("titleenc").Trim())));
            string text = Utils.RemoveHtml(Utils.ClearUBB(DNTRequest.GetString("contentenc").Trim()));
            text = text.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\u3000", "");
            text = Utils.GetUnicodeSubString(text, 500, string.Empty);
            text = Utils.UrlEncode(text);
            string sourceTextByUrl = Utils.GetSourceTextByUrl(string.Format("http://keyword.discuz.com/related_kw.html?title={0}&content={1}&ics=utf-8&ocs=utf-8", arg, text));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sourceTextByUrl);
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("kw");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (XmlNode xmlNode in elementsByTagName)
            {
                stringBuilder.AppendFormat("{0} ", xmlNode.InnerText);
            }
            StringBuilder xmlnode = new StringBuilder(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n                                            <root><![CDATA[\r\n                                            <script type=\"text/javascript\">\r\n                                            var tagsplit = $('tags').value.split(' ');\r\n                                            var inssplit = '{0}';\r\n                                            var returnsplit = inssplit.split(' ');\r\n                                            var result = '';\r\n                                            for(i in tagsplit) {{\r\n                                                for(j in returnsplit) {{\r\n                                                    if(tagsplit[i] == returnsplit[j]) {{\r\n                                                        tagsplit[i] = '';break;\r\n                                                    }}\r\n                                                }}\r\n                                            }}\r\n\r\n                                            for(i in tagsplit) {{\r\n                                                if(tagsplit[i] != '') {{\r\n                                                    result += tagsplit[i] + ' ';\r\n                                                }}\r\n                                            }}\r\n                                            $('tags').value = result + '{0}';\r\n                                            </script>\r\n                                            ]]></root>", stringBuilder.ToString()));
            this.ResponseXML(xmlnode);
        }

        private void GetSmilies()
        {
            if (ForumUtils.IsCrossSitePost()) return;

            Response.Clear();
            ResponseText("{" + Smilie.GetSmiliesCache() + "}", true);
        }

        public void CheckUserName()
        {
            if (String.IsNullOrEmpty(DNTRequest.GetString("username").Trim()))
            {
                return;
            }
            string arg = "0";
            string text = DNTRequest.GetString("username").Trim();
            if (text.IndexOf("\u3000") != -1)
            {
                arg = "1";
            }
            else
            {
                if (text.IndexOf(" ") != -1)
                {
                    arg = "2";
                }
                else
                {
                    if (text.IndexOf(":") != -1)
                    {
                        arg = "3";
                    }
                    else
                    {
                        if (Users.GetUserId(text) > 0)
                        {
                            arg = "4";
                        }
                        else
                        {
                            if (!Utils.IsSafeSqlString(text))
                            {
                                arg = "5";
                            }
                            else
                            {
                                if (!Utils.IsSafeUserInfoString(text))
                                {
                                    arg = "6";
                                }
                                else
                                {
                                    if (text.Trim() == "系统" || ForumUtils.IsBanUsername(text, GeneralConfigInfo.Current.Censoruser))
                                    {
                                        arg = "7";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            StringBuilder stringBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            this.ResponseXML(stringBuilder.AppendFormat("<result>{0}</result>", arg));
        }

        public void GetRateLogList()
        {
            var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                sb.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                this.ResponseXML(sb);
                return;
            }
            try
            {
                //List<RateLogInfo> postRateLogList = Posts.GetPostRateLogList(DNTRequest.GetFormInt("pid", 0));
                var postRateLogList = RateLog.Search(0, WebHelper.RequestInt("pid"));
                if (postRateLogList == null || postRateLogList.Count == 0)
                {
                    sb.Append("<error>该帖没有评分记录</error>");
                    this.ResponseXML(sb);
                }
                else
                {
                    sb.Append("<data>\r\n");
                    var list = new List<RateLog>();
                    foreach (var info in postRateLogList)
                    {
                        //Predicate<RateLogInfo> match = (RateLogInfo rateLog) => rateLog.Uid == info.Uid && rateLog.ExtCredits == info.ExtCredits;
                        //RateLogInfo rateLogInfo = list.Find(match);
                        var rateLogInfo = list.Find(e => e.Uid == info.Uid && e.ExtCredits == info.ExtCredits);
                        if (rateLogInfo == null)
                        {
                            list.Add(info);
                        }
                        else
                        {
                            rateLogInfo.Score += info.Score;
                            rateLogInfo.Reason = (string.IsNullOrEmpty(rateLogInfo.Reason) ? info.Reason : rateLogInfo.Reason);
                            if (rateLogInfo.Reason.IsNullOrWhiteSpace()) rateLogInfo.Reason = info.Reason;
                        }
                    }
                    string[] validScoreName = Scoresets.GetValidScoreName();
                    string[] validScoreUnit = Scoresets.GetValidScoreUnit();
                    int num = 0;
                    int num2 = 0;
                    foreach (var item in list)
                    {
                        if (num2 != item.Uid) num++;

                        sb.Append("<ratelog>");
                        sb.AppendFormat("\r\n\t<rateid>{0}</rateid>", item.ID);
                        sb.AppendFormat("\r\n\t<uid>{0}</uid>", item.Uid);
                        sb.AppendFormat("\r\n\t<username>{0}</username>", item.UserName.Trim());
                        sb.AppendFormat("\r\n\t<extcredits>{0}</extcredits>", item.ExtCredits);
                        sb.AppendFormat("\r\n\t<extcreditsname>{0}</extcreditsname>", validScoreName[item.ExtCredits]);
                        sb.AppendFormat("\r\n\t<extcreditsunit>{0}</extcreditsunit>", validScoreUnit[item.ExtCredits]);
                        sb.AppendFormat("\r\n\t<postdatetime>{0}</postdatetime>", ForumUtils.ConvertDateTime(item.PostDateTime));
                        sb.AppendFormat("\r\n\t<score>{0}</score>", (item.Score > 0) ? ("+" + item.Score.ToString()) : item.Score.ToString());
                        sb.AppendFormat("\r\n\t<reason>{0}</reason>", item.Reason.Trim());
                        sb.Append("\r\n</ratelog>\r\n");
                        num2 = item.Uid;
                    }
                    sb.Append("</data>");
                    this.ResponseXML(sb);
                    if (DNTRequest.GetFormInt("ratetimes", 0) != num)
                    {
                        Posts.UpdatePostRateTimes(DNTRequest.GetFormInt("tid", 0), DNTRequest.GetFormInt("pid", 0).ToString());
                    }
                }
            }
            catch
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = 0;
                HttpContext.Current.Response.Cache.SetNoStore();
                HttpContext.Current.Response.End();
            }
        }

        private void Getdebatesjsonlist(string callback, string tidllist)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(Debates.GetDebatesJsonList(callback, tidllist));
            HttpContext.Current.Response.End();
        }

        private void GetAttachPaymentLogByAid(int aid)
        {
            if (aid > 0)
            {
                HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
                HttpContext.Current.Response.Expires = -1;
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(AttachPaymentLog.GetAttachPaymentLogJsonByAid(aid));
                HttpContext.Current.Response.End();
            }
        }

        private void GetImage()
        {
            var aid = WebHelper.RequestInt("aid");
            var size = WebHelper.Request["size"];
            var key = WebHelper.Request["key"];
            var nocache = WebHelper.Request["nocache"].EqualIgnoreCase("yes");
            //if (String.IsNullOrEmpty(DNTRequest.GetString("aid")) || String.IsNullOrEmpty(DNTRequest.GetString("size")) || String.IsNullOrEmpty(DNTRequest.GetString("key")))
            if (aid == 0 || size.IsNullOrWhiteSpace() || key.IsNullOrWhiteSpace())
            {
                HttpContext.Current.Response.Redirect("images/common/none.gif");
                return;
            }
            string forumpath = BaseConfigInfo.Current.Forumpath;
            //bool nocache = DNTRequest.GetString("nocache") == "yes";
            //int aid = DNTRequest.GetInt("aid", 0);
            //if (DNTRequest.GetString("type") != "")
            //{
            //    DNTRequest.GetString("type");
            //}
            var array = size.SplitAsInt("x");
            int width = array[0];
            int height = array[1];
            string str = string.Format("{0}_{1}_{2}.jpg'", aid, width, height);
            string text = forumpath + "cache/thumbnail/";
            var file = text + str;
            var fullfile = file.GetFullPath();
            if (!nocache && File.Exists(fullfile))
            {
                HttpContext.Current.Response.Redirect(file);
                return;
            }
            var a = DES.Encode(aid + "," + width + "," + height, Utils.MD5(aid.ToString())).Replace("+", "[");
            if (a != key)
            {
                HttpContext.Current.Response.Redirect("images/common/none.gif");
                return;
            }
            var att = Attachment.FindByID(aid);
            HttpContext.Current.Response.Expires = 60;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddMinutes(60.0);
            fullfile.EnsureDirectory();
            if (!att.IsLocal)
                Thumbnail.MakeRemoteThumbnailImage(att.FileName, fullfile, width, height);
            else
                Thumbnail.MakeThumbnailImage(att.FullFileName, fullfile, width, height);

            if (nocache)
            {
                HttpContext.Current.Response.ContentType = "image/jpg";
                HttpContext.Current.Response.BinaryWrite(File.ReadAllBytes(fullfile));
                try
                {
                    File.Delete(fullfile);
                    return;
                }
                catch { return; }
            }
            HttpContext.Current.Response.Redirect(file);
        }

        private void ResetEmail()
        {
            int uid = DNTRequest.GetInt("uid", -1);
            if (uid <= 0)
            {
                this.ResponseText("{'text':'非法请求','code':0}");
                return;
            }
            string newemail = DNTRequest.GetString("newemail");
            if (!Utils.IsValidEmail(newemail))
            {
                this.ResponseText("{'text':'E-mail格式不正确','code':0}");
                return;
            }
            var userInfo = User.FindByID(uid);
            if (Utils.MD5(userInfo.Password + GeneralConfigInfo.Current.Passwordkey + DNTRequest.GetString("ts")) != DNTRequest.GetString("auth"))
            {
                this.ResponseText("{'text':'非法请求','code':0}");
                return;
            }
            if (long.Parse(DNTRequest.GetString("ts")) < DateTime.Now.AddMinutes(-2.0).Ticks)
            {
                this.ResponseText("{'text':'该操作已经超过了时限,无法执行','code':0}");
                return;
            }
            if (userInfo.GroupID != 8)
            {
                this.ResponseText("{'text':'该用户不是等待验证的用户','code':0}");
                return;
            }
            if (userInfo.Email != newemail)
            {
                if (!Users.ValidateEmail(newemail, uid))
                {
                    this.ResponseText("{'text':'Email: \"" + newemail + "\" 已经被其它用户注册使用','code':0}");
                    return;
                }
                userInfo.Email = newemail;
                //Users.UpdateUserProfile(userInfo);
                //throw new NotImplementedException("UpdateUserProfile");
                userInfo.Save();
            }
            Emails.SendRegMail(userInfo.Name, newemail, string.Empty, userInfo.Field.Authstr);
            this.ResponseText("{'text':'验证邮件已经重新发送到您指定的E-mail地址当中','code':1}");
        }

        private void ResponseXML(StringBuilder sb)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "Text/XML";
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void ResponseJSON(String json, Boolean usecache = false)
        {
            Response.Clear();
            Response.ContentType = "application/json";

            if (!usecache)
            {
                Response.Expires = 0;
                Response.Cache.SetNoStore();
            }

            ResponseText(json, usecache);
        }

        private void ResponseText(String html, Boolean usecache = false)
        {
            if (usecache)
            {
                var cache = Response.Cache;
                cache.SetCacheability(HttpCacheability.Public);
                cache.SetLastModified(DateTime.Now);
                cache.SetMaxAge(new TimeSpan(1, 0, 0));
            }

            Response.Write(html);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        //private void ResponseJSON<T>(T jsonobj)
        //{
        //    this.ResponseJSON(JavaScriptConvert.SerializeObject(jsonobj));
        //}

        #region 标签
        String _colortags;
        String _closedtags;
        DateTime _expiredTime;

        void CheckTags()
        {
            if (_expiredTime > DateTime.Now) return;
            _expiredTime = DateTime.Now.AddMinutes(1);

            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            foreach (var tag in Tag.GetForumTags("", 0))
            {
                if (tag.OrderID == -1) sb.Separate(",").AppendFormat("'{0}'", tag.ID);
                if (!String.IsNullOrEmpty(tag.Color)) sb2.Separate(",").AppendFormat("'{0}':{{'tagid' : '{0}', 'color' : '{1}'}}", tag.ID, (tag.Color + "").Trim());
            }
            _colortags = sb2.ToString();
            _closedtags = sb.ToString();
        }

        public void ColorFullTags()
        {
            CheckTags();

            ResponseJSON(_colortags, true);
        }

        public void ClosedTags()
        {
            CheckTags();

            ResponseJSON(_closedtags, true);
        }
        #endregion
    }
}