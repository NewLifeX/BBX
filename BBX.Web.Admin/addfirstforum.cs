using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addfirstforum : AdminPage
    {
        protected HtmlForm Form1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected TextBox name;
        protected RadioButtonList status;
        protected TextareaResize description;
        protected TextareaResize moderators;
        protected TabPage tabPage22;
        protected TextBox password;
        protected TextBox icon;
        protected TextBox redirect;
        protected CheckBoxList attachextensions;
        protected TextBox rules;
        protected TextBox seokeywords;
        protected TextBox seodescription;
        protected TextBox rewritename;
        protected RadioButtonList autocloseoption;
        protected HtmlGenericControl showclose;
        protected TextBox autocloseday;
        protected RadioButtonList allowspecialonly;
        protected CheckBoxList setting;
        protected TabPage tabPage33;
        protected pageinfo PageInfo1;
        protected HtmlTable powerset;
        protected TextBox topictypes;
        protected DropDownList templateid;
        protected Hint Hint1;
        protected Button Submit;
        public IXForum forumInfo;

        public void InitInfo()
        {
            this.templateid.AddTableData(Template.GetValids(), "name", "id");
            //DataTable dataTable = UserGroups.GetUserGroupForDataTable();
            int num = 1;
            foreach (var ug in UserGroup.GetAll())
            {
                HtmlTableRow htmlTableRow = new HtmlTableRow();
                HtmlTableCell htmlTableCell = new HtmlTableCell("td");
                if (num % 2 == 1)
                {
                    htmlTableCell.Attributes.Add("class", "td_alternating_item1");
                }
                else
                {
                    htmlTableCell.Attributes.Add("class", "td_alternating_item2");
                }
                htmlTableCell.Controls.Add(new LiteralControl("<input type='checkbox' id='r" + num + "' onclick='selectRow(" + num + ",this.checked)'>"));
                htmlTableRow.Cells.Add(htmlTableCell);
                htmlTableCell = new HtmlTableCell("td");
                if (num % 2 == 1)
                {
                    htmlTableCell.Attributes.Add("class", "td_alternating_item1");
                }
                else
                {
                    htmlTableCell.Attributes.Add("class", "td_alternating_item2");
                }
                htmlTableCell.Controls.Add(new LiteralControl("<label for='r" + num + "'>" + ug.GroupTitle + "</lable>"));
                htmlTableRow.Cells.Add(htmlTableCell);
                htmlTableRow.Cells.Add(this.GetTD("viewperm", ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("postperm", ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("replyperm", ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("getattachperm", ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("postattachperm", ug.ID, num));
                this.powerset.Rows.Add(htmlTableRow);
                num++;
            }
            //var dataTable = Attachments.GetAttachmentType();
            this.attachextensions.AddTableData(AttachType.FindAllWithCache());
            this.showclose.Attributes.Add("style", "display:none");
            this.autocloseoption.SelectedIndex = 0;
            this.autocloseoption.Attributes.Add("onclick", "javascript:document.getElementById('" + this.showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage2_autocloseoption_0').checked ? 'none' : 'block');");
        }

        private HtmlTableCell GetTD(string strPerfix, Int32 groupId, int ctlId)
        {
            string text = "<input type='checkbox' name='" + strPerfix + "' id='" + strPerfix + ctlId + "' value='" + groupId + "'>";
            HtmlTableCell htmlTableCell = new HtmlTableCell("td");
            if (ctlId % 2 == 1)
            {
                htmlTableCell.Attributes.Add("class", "td_alternating_item1");
            }
            else
            {
                htmlTableCell.Attributes.Add("class", "td_alternating_item2");
            }
            htmlTableCell.Controls.Add(new LiteralControl(text));
            return htmlTableCell;
        }

        public int BoolToInt(bool a)
        {
            if (!a)
            {
                return 0;
            }
            return 1;
        }

        private void SubmitSame_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.InsertForum("0", "1");
            }
        }

        public void InsertForum(string parentid, string systemdisplayorder)
        {
            if (!this.rewritename.Text.IsNullOrEmpty() && XForum.CheckRewriteNameInvalid(this.rewritename.Text.Trim()))
            {
                this.rewritename.Text = "";
                base.RegisterStartupScript("", "<script>alert('URL重写非法!');</script>");
                return;
            }
            this.forumInfo.ParentID = parentid.ToInt();
            //this.forumInfo.Layer = layer.ToInt();
            //this.forumInfo.Parentidlist = parentidlist;
            //this.forumInfo.SubforumCount = subforumcount.ToInt();
            this.forumInfo.Name = this.name.Text.Trim();
            this.forumInfo.Status = this.status.SelectedValue.ToInt();
            this.forumInfo.ColCount = 1;
            this.forumInfo.DisplayOrder = systemdisplayorder.ToInt();
            this.forumInfo.TemplateID = this.templateid.SelectedValue.ToInt();
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
            this.forumInfo.Modnewtopics = this.BoolToInt(this.setting.Items[6].Selected);
            this.forumInfo.Jammer = this.BoolToInt(this.setting.Items[7].Selected);
            this.forumInfo.DisableWatermark = this.setting.Items[8].Selected;
            this.forumInfo.Inheritedmod = this.BoolToInt(this.setting.Items[9].Selected);
            this.forumInfo.AllowThumbnail = this.setting.Items[10].Selected;
            this.forumInfo.AllowTag = this.setting.Items[11].Selected;
            //this.forumInfo.IsTrade = 0;
            int num = 0;
            num = (this.setting.Items[12].Selected ? (num | 1) : (num & -2));
            num = (this.setting.Items[13].Selected ? (num | 16) : (num & -17));
            num = (this.setting.Items[14].Selected ? (num | 4) : (num & -5));
            this.forumInfo.AllowPostSpecial = num;
            this.forumInfo.AllowEditRules = this.setting.Items[15].Selected;
            this.forumInfo.AllowSpecialOnly = (int)Convert.ToInt16(this.allowspecialonly.SelectedValue) != 0;
            this.forumInfo.AutoClose = ((this.autocloseoption.SelectedValue == "0") ? 0 : this.autocloseday.Text.ToInt());
            this.forumInfo.Description = this.description.Text;
            this.forumInfo.Password = this.password.Text;
            this.forumInfo.Icon = this.icon.Text;
            this.forumInfo.PostcrEdits = "";
            this.forumInfo.ReplycrEdits = "";
            this.forumInfo.Redirect = this.redirect.Text;
            this.forumInfo.Attachextensions = this.attachextensions.GetSelectString(",");
            this.forumInfo.Moderators = this.moderators.Text;
            this.forumInfo.Rules = this.rules.Text;
            this.forumInfo.Seokeywords = this.seokeywords.Text.Trim();
            this.forumInfo.Seodescription = this.seodescription.Text.Trim();
            this.forumInfo.RewriteName = this.rewritename.Text.Trim();
            this.forumInfo.TopicTypes = this.topictypes.Text;
            this.forumInfo.ViewPerm = base.Request.Form["viewperm"];
            this.forumInfo.PostPerm = base.Request.Form["postperm"];
            this.forumInfo.ReplyPerm = base.Request.Form["replyperm"];
            this.forumInfo.GetattachPerm = base.Request.Form["getattachperm"];
            this.forumInfo.PostattachPerm = base.Request.Form["postattachperm"];
            string text;
            AdminForums.CreateForums(this.forumInfo, out text, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
            if (String.IsNullOrEmpty(text))
            {
                base.RegisterStartupScript("PAGE", "window.location.href='forum_ForumsTree.aspx';");
                return;
            }
            base.RegisterStartupScript("PAGE", "alert('用户:" + text + "不存在,因为无法设为版主');window.location.href='forum_ForumsTree.aspx';");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.TabControl1.Items.Remove(this.tabPage22);
            this.tabPage22.Visible = false;
            this.Submit.Click += new EventHandler(this.SubmitSame_Click);
            this.InitInfo();
        }
    }
}