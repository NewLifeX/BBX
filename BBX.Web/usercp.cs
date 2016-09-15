using System.Text;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercp : UserCpPage
    {
        public string avatarurl = "";
        public int avatartype = 1;
        public int avatarwidth;
        public int ismoder;
        public int avatarheight;
        public string usergroupattachtype;
        public AdminGroup admingroupinfo;
        public string score1 = "";
        public string score2 = "";
        public string score3 = "";
        public string score4 = "";
        public string score5 = "";
        public string score6 = "";
        public string score7 = "";
        public string score8 = "";

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (!base.IsLogin())
            {
                return;
            }
            this.score1 = ((decimal)this.user.ExtCredits1).ToString();
            this.score2 = ((decimal)this.user.ExtCredits2).ToString();
            this.score3 = ((decimal)this.user.ExtCredits3).ToString();
            this.score4 = ((decimal)this.user.ExtCredits4).ToString();
            this.score5 = ((decimal)this.user.ExtCredits5).ToString();
            this.score6 = ((decimal)this.user.ExtCredits6).ToString();
            this.score7 = ((decimal)this.user.ExtCredits7).ToString();
            this.score8 = ((decimal)this.user.ExtCredits8).ToString();
            if (!base.IsErr() && this.useradminid > 0)
            {
                this.admingroupinfo = AdminGroup.FindByID(this.usergroupid);
            }
			//var stringBuilder = new StringBuilder();
			//if (!Utils.StrIsNullOrEmpty(this.usergroupinfo.AttachExtensions))
			//{
			//	stringBuilder.AppendFormat("[id] in ({0})", this.usergroupinfo.AttachExtensions);
			//}
			//this.usergroupattachtype = Attachments.GetAttachmentTypeString(stringBuilder.ToString());
			this.usergroupattachtype = AttachType.GetAttachmentTypeString(usergroupinfo, null);
            this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
        }
    }
}