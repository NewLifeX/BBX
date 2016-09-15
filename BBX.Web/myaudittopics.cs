using System.Collections.Generic;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class myaudittopics : PageBase
    {
        public List<Topic> topics;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int pagecount;
        public int topiccount;
        public string pagenumbers;
        public IUser user;

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (this.userid == -1)
            {
                base.AddErrLine("你尚未登录");
                return;
            }
            this.user = Users.GetUserInfo(this.userid);
            //this.topiccount = Topics.GetMyUnauditTopicCount(this.user.ID, -2);
            this.topiccount = Topic.GetMyUnauditTopicCount(this.user.ID, -2);
            this.pagecount = ((this.topiccount % 16 == 0) ? (this.topiccount / 16) : (this.topiccount / 16 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
            this.topics = Topic.GetMyUnauditTopic(this.user.ID, 16, this.pageid, -2);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, "myaudittopics.aspx", 8);
        }
    }
}