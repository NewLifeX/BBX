using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class userinfo : PageBase
    {
        public IUser user;
        public UserGroup group;
        public AdminGroup admininfo;
        public string[] score;
        public bool needlogin;
        public string score1;
        public string score2;
        public string score3;
        public string score4;
        public string score5;
        public string score6;
        public string score7;
        public string score8;
        public int id = DNTRequest.GetInt("userid", -1);

        protected override void ShowPage()
        {
            this.pagetitle = "查看用户信息";
            if (!this.usergroupinfo.AllowViewpro && this.userid != this.id)
            {
                base.AddErrLine(string.Format("您当前的身份 \"{0}\" 没有查看用户资料的权限", this.usergroupinfo.GroupTitle));
                if (this.userid < 1)
                {
                    this.needlogin = true;
                }
                return;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("username")) && Utils.StrIsNullOrEmpty(DNTRequest.GetString("userid")))
            {
                base.AddErrLine("错误的URL链接");
                return;
            }
            if (this.id == -1)
            {
                this.id = Users.GetUserId(Utils.UrlDecode(DNTRequest.GetString("username")));
            }
            if (this.id == -1)
            {
                base.AddErrLine("该用户不存在");
                return;
            }
            this.user = Users.GetUserInfo(this.id);
            if (this.user == null)
            {
                base.AddErrLine("该用户不存在");
                return;
            }
            if (!user.ShowEmail && this.id != this.userid)
            {
                //this.user.Email = "";
                // 不影响脏数据，避免不小心被保存了
                user["Email"] = "";
            }
            this.score = Scoresets.GetValidScoreName();
            this.group = UserGroup.FindByID(this.user.GroupID);
            //this.admininfo = AdminUserGroups.AdminGetAdminGroupInfo(this.usergroupid);
            admininfo = AdminGroup.FindByID(usergroupid);
            this.score1 = ((decimal)this.user.ExtCredits1).ToString();
            this.score2 = ((decimal)this.user.ExtCredits2).ToString();
            this.score3 = ((decimal)this.user.ExtCredits3).ToString();
            this.score4 = ((decimal)this.user.ExtCredits4).ToString();
            this.score5 = ((decimal)this.user.ExtCredits5).ToString();
            this.score6 = ((decimal)this.user.ExtCredits6).ToString();
            this.score7 = ((decimal)this.user.ExtCredits7).ToString();
            this.score8 = ((decimal)this.user.ExtCredits8).ToString();
        }
    }
}