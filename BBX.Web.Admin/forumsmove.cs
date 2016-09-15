using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class forumsmove : AdminPage
    {
        protected HtmlForm Form1;
        protected DropDownTreeList sourceforumid;
        protected RadioButtonList movetype;
        protected ListBoxTreeList targetforumid;
        protected Button SaveMoveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request["currentfid"]))
            {
                base.Server.Transfer("forum_ForumsTree.aspx");
                return;
            }
            this.sourceforumid.BuildTree(XForum.Root, "Name", "ID");
            this.sourceforumid.TypeID.Items.RemoveAt(0);
            this.sourceforumid.SelectedValue = Request["currentfid"];
            this.targetforumid.BuildTree(XForum.Root, "Name", "ID");
            this.targetforumid.TypeID.Items.RemoveAt(0);
            this.targetforumid.TypeID.Height = 290;
            this.targetforumid.TypeID.SelectedIndex = 0;
        }

        private void SaveMoveInfo_Click(object sender, EventArgs e)
        {
            if (this.sourceforumid.SelectedValue == this.targetforumid.SelectedValue)
            {
                base.RegisterStartupScript("", "<script>alert('您所要移动的版块与目标版块相同, 因此无法提交!');</script>");
                return;
            }
            bool isaschildnode = this.movetype.SelectedValue == "1";
            if (!AdminForums.MovingForumsPos(this.sourceforumid.SelectedValue.ToInt(), this.targetforumid.SelectedValue.ToInt(), isaschildnode))
            {
                base.RegisterStartupScript("", "<script>alert('当前源版块移动失败!');</script>");
                return;
            }
            ForumOperator.RefreshForumCache();
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "移动论坛版块", "移动论坛版块ID:" + this.sourceforumid.SelectedValue + "到ID:" + this.targetforumid.SelectedValue);
            base.RegisterStartupScript("PAGE", "window.location.href='forum_forumsTree.aspx';");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveMoveInfo.Click += new EventHandler(this.SaveMoveInfo_Click);
        }
    }
}