using System.Net;
using NewLife.Net;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using XUser = BBX.Entity.User;
using System;

namespace BBX.Web
{
    public class getip : PageBase
    {
        public string forumname = "";
        public string forumnav = "";
        public string posttitle = "";
        public int postid = DNTRequest.GetInt("pid", 0);
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public string ip = "";
        public string iplocation = "";

        protected override void ShowPage()
        {
            if (this.postid == 0)
            {
                base.AddErrLine("指定的主题不存在或已被删除或正在被审核,请返回.");
                return;
            }
            var postInfo = Post.FindByID(this.postid);
            if (postInfo == null)
            {
                base.AddErrLine("指定的主题不存在或已被删除或正在被审核,请返回.");
                return;
            }
            this.ip = postInfo.IP;
            this.iplocation = IPAddress.Parse(this.ip).GetAddress();
            if (this.iplocation == null)
            {
                this.iplocation = "(IP数据库文件不存在,无法查询)";
            }
            else
            {
                if (string.IsNullOrEmpty(this.iplocation))
                {
                    this.iplocation = "没有查询到该用户的地理所在地";
                }
            }
            var topicInfo = Topic.FindByID(postInfo.Tid);
            if (topicInfo == null)
            {
                base.AddErrLine("不存在的主题ID");
                return;
            }
            var forumInfo = Forums.GetForumInfo(postInfo.Fid);
            this.forumname = forumInfo.Name;
            this.pagetitle = topicInfo.Title;
            this.forumnav = ForumUtils.UpdatePathListExtname(forumInfo.Pathlist.Trim(), this.config.Extname);
            var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
            if (adminGroupInfo == null || !adminGroupInfo.AllowViewIP)
            {
                base.AddErrLine("你没有查看IP的权限");
                return;
            }
            if (DNTRequest.GetString("action") == "ipban")
            {
                if (!adminGroupInfo.AllowBanIP)
                {
                    base.AddErrLine("你无权禁止用户IP,请返回");
                    return;
                }
                if (Utils.InIPArray(DNTRequest.GetString("ip"), Utils.SplitString(this.config.Ipdenyaccess, "\n")))
                {
                    //Users.UpdateUserGroup(postInfo.Posterid, 6);
                    XUser user = XUser.FindByID(postInfo.PosterID);
                    user.GroupID = 6;
                    user.Save();
                    base.AddErrLine("IP已在列表中存在,无需重复添加");
                    return;
                }
                var cfg = GeneralConfigInfo.Current;
                cfg.Ipdenyaccess += "\n" + DNTRequest.GetString("ip");
                cfg.Save();
                //if (GeneralConfigs.SetIpDenyAccess(DNTRequest.GetString("ip")))
                {
                    //Users.UpdateUserGroup(postInfo.Posterid, 6);
                    XUser user = XUser.FindByID(postInfo.PosterID);
                    user.GroupID = 6;
                    user.Save();
                    base.SetUrl(base.ShowTopicAspxRewrite(topicInfo.ID, 0));
                    base.SetMetaRefresh();
                    base.SetShowBackLink(false);
                    base.MsgForward("getip_succeed");
                    base.AddMsgLine("IP已加入到用户禁止列表中");
                    this.ispost = true;
                    return;
                }
                //base.AddErrLine("未知原因,IP无法加到禁止列表中");
            }
        }
    }
}