using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class forumlist : PageBase
    {
        public IUser userinfo;
        public int totalonline;
        public int totalonlineuser = Online.Meta.Count;
        public string[] score = Scoresets.GetValidScoreName();

        protected override void ShowPage()
        {
            this.pagetitle = "版块列表";
            if (this.config.Rssstatus == 1)
            {
                base.AddLinkRss("tools/rss.aspx", this.config.Forumtitle + "最新主题");
            }
            if (this.userid != -1)
            {
                this.userinfo = BBX.Entity.User.FindByID(this.userid);
                this.newpmcount = (!this.userinfo.Newpm ? 0 : this.newpmcount);
            }
            Online.UpdateAction(this.olid, UserAction.IndexShow, 0, this.config.Onlinetimeout);
            this.totalonline = this.onlineusercount;
        }
    }
}