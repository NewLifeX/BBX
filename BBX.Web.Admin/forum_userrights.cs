using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class forum_userrights : AdminPage
    {
        private GeneralConfigInfo configInfo = GeneralConfigInfo.Current;
        protected HtmlForm form1;
        protected BBX.Control.RadioButtonList dupkarmarate;
        protected BBX.Control.TextBox edittimelimit;
        protected BBX.Control.TextBox deletetimelimit;
        protected BBX.Control.TextBox maxattachments;
        protected RegularExpressionValidator RegularExpressionValidator2;
        protected BBX.Control.TextBox karmaratelimit;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected BBX.Control.TextBox maxfavorites;
        protected RegularExpressionValidator RegularExpressionValidator4;
        protected BBX.Control.TextBox maxpolloptions;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected BBX.Control.TextBox minpostsize;
        protected BBX.Control.TextBox maxpostsize;
        protected BBX.Control.RadioButtonList moderactions;
        protected Hint Hint1;
        protected BBX.Control.Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            this.dupkarmarate.SelectedValue = this.configInfo.Dupkarmarate.ToString();
            this.minpostsize.Text = this.configInfo.Minpostsize.ToString();
            this.maxpostsize.Text = this.configInfo.Maxpostsize.ToString();
            this.maxfavorites.Text = this.configInfo.Maxfavorites.ToString();
            this.maxpolloptions.Text = this.configInfo.Maxpolloptions.ToString();
            this.maxattachments.Text = this.configInfo.Maxattachments.ToString();
            this.karmaratelimit.Text = this.configInfo.Karmaratelimit.ToString();
            this.moderactions.SelectedValue = this.configInfo.Moderactions.ToString();
            this.edittimelimit.Text = this.configInfo.Edittimelimit.ToString();
            this.deletetimelimit.Text = this.configInfo.Deletetimelimit.ToString();
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.edittimelimit.Text.ToInt() > 9999999 || this.edittimelimit.Text.ToInt() < -1)
                {
                    base.RegisterStartupScript("", "<script>alert('编辑帖子时间限制只能在-1-9999999之间');window.location.href='forum_option.aspx';</script>");
                    return;
                }
                if (this.deletetimelimit.Text.ToInt() > 9999999 || this.deletetimelimit.Text.ToInt() < -1)
                {
                    base.RegisterStartupScript("", "<script>alert('删除帖子时间限制只能在-1-9999999之间');window.location.href='forum_option.aspx';</script>");
                    return;
                }
                if (this.minpostsize.Text.ToInt() > 9999999 || this.minpostsize.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('帖子最小字数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (this.maxpostsize.Text.ToInt() > 9999999 || this.maxpostsize.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('帖子最大字数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (this.maxfavorites.Text.ToInt() > 9999999 || this.maxfavorites.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('收藏夹容量只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (this.maxpolloptions.Text.ToInt() > 9999999 || this.maxpolloptions.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('最大签名高度只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (this.maxattachments.Text.ToInt() > 9999999 || this.maxattachments.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('投票最大选项数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (this.karmaratelimit.Text.ToInt() > 9999 || this.karmaratelimit.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('评分时间限制只能在0-9999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                config.Dupkarmarate = (int)Convert.ToInt16(this.dupkarmarate.SelectedValue);
                config.Minpostsize = this.minpostsize.Text.ToInt();
                config.Maxpostsize = this.maxpostsize.Text.ToInt();
                config.Maxfavorites = this.maxfavorites.Text.ToInt();
                config.Maxpolloptions = this.maxpolloptions.Text.ToInt();
                config.Maxattachments = this.maxattachments.Text.ToInt();
                config.Karmaratelimit = (int)Convert.ToInt16(this.karmaratelimit.Text);
                config.Moderactions = (int)Convert.ToInt16(this.moderactions.SelectedValue);
                config.Edittimelimit = this.edittimelimit.Text.ToInt();
                config.Deletetimelimit = this.deletetimelimit.Text.ToInt();
                config.Save();

                //config.Save();;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "用户权限设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='forum_userrights.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }
    }
}