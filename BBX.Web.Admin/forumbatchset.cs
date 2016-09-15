using System;
using System.Data;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class forumbatchset : AdminPage
    {
        public IXForum forumInfo;
        protected HtmlForm Form1;
        protected forumtree Forumtree1;
        protected HtmlInputCheckBox setpassword;
        protected TextBox password;
        protected HtmlInputCheckBox setpostcredits;
        protected HtmlInputCheckBox setattachextensions;
        protected CheckBoxList attachextensions;
        protected HtmlInputCheckBox setreplycredits;
        protected HtmlInputCheckBox setsetting;
        protected CheckBoxList setting;
        protected HtmlInputCheckBox setviewperm;
        protected CheckBoxList viewperm;
        protected HtmlInputCheckBox setpostperm;
        protected CheckBoxList postperm;
        protected HtmlInputCheckBox setreplyperm;
        protected CheckBoxList replyperm;
        protected HtmlInputCheckBox setgetattachperm;
        protected CheckBoxList getattachperm;
        protected HtmlInputCheckBox setpostattachperm;
        protected CheckBoxList postattachperm;
        protected Hint Hint1;
        protected Button SubmitBatchSet;

        public void LoadCurrentForumInfo(int fid)
        {
            if (fid > 0)
            {
                this.forumInfo = Forums.GetForumInfo(fid);
                if (this.forumInfo.AllowSmilies)
                {
                    this.setting.Items[0].Selected = true;
                }
                if (this.forumInfo.AllowRss)
                {
                    this.setting.Items[1].Selected = true;
                }
                if (this.forumInfo.AllowBbCode)
                {
                    this.setting.Items[2].Selected = true;
                }
                if (this.forumInfo.AllowImgCode)
                {
                    this.setting.Items[3].Selected = true;
                }
                if (this.forumInfo.Recyclebin == 1)
                {
                    this.setting.Items[4].Selected = true;
                }
                if (this.forumInfo.Modnewposts == 1)
                {
                    this.setting.Items[5].Selected = true;
                }
                if (this.forumInfo.Modnewtopics == 1)
                {
                    this.setting.Items[6].Selected = true;
                }
                if (this.forumInfo.Jammer == 1)
                {
                    this.setting.Items[7].Selected = true;
                }
                if (this.forumInfo.DisableWatermark)
                {
                    this.setting.Items[8].Selected = true;
                }
                if (this.forumInfo.Inheritedmod == 1)
                {
                    this.setting.Items[9].Selected = true;
                }
                if (this.forumInfo.AllowThumbnail)
                {
                    this.setting.Items[10].Selected = true;
                }
                if (this.forumInfo.AllowTag)
                {
                    this.setting.Items[11].Selected = true;
                }
                if ((this.forumInfo.AllowPostSpecial & 1) != 0)
                {
                    this.setting.Items[12].Selected = true;
                }
                if ((this.forumInfo.AllowPostSpecial & 16) != 0)
                {
                    this.setting.Items[13].Selected = true;
                }
                if ((this.forumInfo.AllowPostSpecial & 4) != 0)
                {
                    this.setting.Items[14].Selected = true;
                }
                if (this.forumInfo.AllowEditRules)
                {
                    this.setting.Items[15].Selected = true;
                }
                this.viewperm.SetSelectByID(this.forumInfo.ViewPerm.Trim());
                this.postperm.SetSelectByID(this.forumInfo.PostPerm.Trim());
                this.replyperm.SetSelectByID(this.forumInfo.ReplyPerm.Trim());
                this.getattachperm.SetSelectByID(this.forumInfo.GetattachPerm.Trim());
                this.postattachperm.SetSelectByID(this.forumInfo.PostattachPerm.Trim());
                this.attachextensions.SetSelectByID(this.forumInfo.Attachextensions.Trim());
                return;
            }
        }

        public int BoolToInt(bool a)
        {
            if (!a)
            {
                return 0;
            }
            return 1;
        }

        private void SubmitBatchSet_Click(object sender, EventArgs e)
        {
            string fidlist = Request["Forumtree1"];
            if (String.IsNullOrEmpty(fidlist) || fidlist == "," || fidlist == "0")
            {
                base.RegisterStartupScript("", "<script>alert('您未选中任何版块, 系统无法提交! ');</script>");
                return;
            }
            this.forumInfo = Forums.GetForumInfo(DNTRequest.GetInt("fid", -1));
            //this.forumInfo.AllowHtml = 0;
            //this.forumInfo.AllowBlog = 0;
            //this.forumInfo.IsTrade = 0;
            //this.forumInfo.AllowEditRules = 0;
            this.forumInfo.AllowSmilies = this.setting.Items[0].Selected;
            this.forumInfo.AllowRss = this.setting.Items[1].Selected;
            this.forumInfo.AllowBbCode = this.setting.Items[2].Selected;
            this.forumInfo.AllowImgCode = this.setting.Items[3].Selected;
            this.forumInfo.Recyclebin = this.BoolToInt(this.setting.Items[4].Selected);
            this.forumInfo.Modnewposts = this.BoolToInt(this.setting.Items[5].Selected);
            this.forumInfo.Jammer = this.BoolToInt(this.setting.Items[6].Selected);
            this.forumInfo.DisableWatermark = this.setting.Items[7].Selected;
            this.forumInfo.Inheritedmod = this.BoolToInt(this.setting.Items[8].Selected);
            this.forumInfo.AllowThumbnail = this.setting.Items[9].Selected;
            this.forumInfo.AllowTag = this.setting.Items[10].Selected;
            int num = 0;
            num = (this.setting.Items[11].Selected ? (num | 1) : (num & -2));
            num = (this.setting.Items[12].Selected ? (num | 16) : (num & -17));
            num = (this.setting.Items[13].Selected ? (num | 4) : (num & -5));
            this.forumInfo.AllowPostSpecial = num;
            this.forumInfo.AllowEditRules = this.setting.Items[14].Selected;
            this.forumInfo.Password = this.password.Text;
            this.forumInfo.Attachextensions = this.attachextensions.GetSelectString(",");
            this.forumInfo.ViewPerm = this.viewperm.GetSelectString(",");
            this.forumInfo.PostPerm = this.postperm.GetSelectString(",");
            this.forumInfo.ReplyPerm = this.replyperm.GetSelectString(",");
            this.forumInfo.GetattachPerm = this.getattachperm.GetSelectString(",");
            this.forumInfo.PostattachPerm = this.postattachperm.GetSelectString(",");
            BatchSetParams bsp = default(BatchSetParams);
            bsp.SetPassWord = this.setpassword.Checked;
            bsp.SetAttachExtensions = this.setattachextensions.Checked;
            bsp.SetPostCredits = this.setpostcredits.Checked;
            bsp.SetReplyCredits = this.setreplycredits.Checked;
            bsp.SetSetting = this.setsetting.Checked;
            bsp.SetViewperm = this.setviewperm.Checked;
            bsp.SetPostperm = this.setpostperm.Checked;
            bsp.SetReplyperm = this.setreplyperm.Checked;
            bsp.SetGetattachperm = this.setgetattachperm.Checked;
            bsp.SetPostattachperm = this.setpostattachperm.Checked;
            if (XForum.BatchSet(this.forumInfo, bsp, fidlist))
            {
                ForumOperator.RefreshForumCache();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "复制版块设置", "编辑论坛版块列表为:" + fidlist.Trim());
                base.RegisterStartupScript("PAGE", "window.location.href='forum_ForumsTree.aspx';");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('提交不成功!');window.location.href='forum_ForumsTree.aspx';</script>");
        }

        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    base.MySavePageState(viewState);
        //}

        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    return base.MyLoadPageState();
        //}

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SubmitBatchSet.Click += new EventHandler(this.SubmitBatchSet_Click);
            //DataTable userGroupForDataTable = UserGroups.GetUserGroupForDataTable();
            var userGroupForDataTable = UserGroup.GetAll();
            this.viewperm.AddTableData(userGroupForDataTable, "grouptitle", "id");
            this.postperm.AddTableData(userGroupForDataTable, "grouptitle", "id");
            this.replyperm.AddTableData(userGroupForDataTable, "grouptitle", "id");
            this.getattachperm.AddTableData(userGroupForDataTable, "grouptitle", "id");
            this.postattachperm.AddTableData(userGroupForDataTable, "grouptitle", "id");
			this.attachextensions.AddTableData(AttachType.FindAllWithCache());
            this.LoadCurrentForumInfo(DNTRequest.GetInt("fid", -1));
        }
    }
}