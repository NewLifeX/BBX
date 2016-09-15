using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class forumcombination : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
        protected DropDownTreeList sourceforumid;
        protected DropDownTreeList targetforumid;
        protected Button SaveCombinationInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.sourceforumid.BuildTree(XForum.Root, "Name", "ID");
            this.targetforumid.BuildTree(XForum.Root, "Name", "ID");
            if (!this.Page.IsPostBack && Request["fid"] != "")
            {
                this.sourceforumid.SelectedValue = Request["fid"];
            }
        }

        private void SaveCombinationInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.sourceforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('请选择相应的源论坛!');</script>");
                    return;
                }
                if (this.targetforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('请选择相应的目标论坛!');</script>");
                    return;
                }
                var forumInfo = Forums.GetForumInfo(Utils.StrToInt(this.targetforumid.SelectedValue, 0));
                if (forumInfo != null && forumInfo.ParentID == 0 && forumInfo.Layer == 0)
                {
                    base.RegisterStartupScript("", "<script>alert('您所选择的目标论坛是\"论坛分类\"而不是\"论坛版块\",因此合并无效!');</script>");
                    return;
                }
				if (!XForum.Combination(this.sourceforumid.SelectedValue.ToInt(), this.targetforumid.SelectedValue.ToInt()))
                {
                    string scriptstr = "<script>alert('当前节点下面有子结点,因此合并无效!');window.location.href='forum_forumcombination.aspx';</script>";
                    base.RegisterStartupScript("", scriptstr);
                    return;
                }
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并论坛版块", "合并论坛版块" + this.sourceforumid.SelectedValue + "到" + this.targetforumid.SelectedValue);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_forumstree.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveCombinationInfo.Click += new EventHandler(this.SaveCombinationInfo_Click);
        }
    }
}