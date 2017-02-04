using System;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class addforums : AdminPage
    {
        public IXForum forumInfo;
        protected string root = Utils.GetRootUrl(BaseConfigInfo.Current.Forumpath);
        protected HtmlForm Form1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected TextBox name;
        protected RadioButtonList colcount;
        protected HtmlGenericControl showcolnum;
        protected TextBox colcountnumber;
        protected RadioButtonList status;
        protected RadioButtonList addtype;
        protected HtmlGenericControl showtargetforum;
        protected DropDownTreeList targetforumid;
        protected TextareaResize moderators;
        protected TextBox rewritename;
        protected HtmlInputHidden oldrewritename;
        protected TextBox description;
        protected TextBox seokeywords;
        protected TextBox seodescription;
        protected TabPage tabPage22;
        protected TextBox icon;
        protected TextBox password;
        protected TextBox redirect;
        protected TextBox rules;
        protected CheckBoxList attachextensions;
        protected RadioButtonList autocloseoption;
        protected HtmlGenericControl showclose;
        protected TextBox autocloseday;
        protected RadioButtonList allowspecialonly;
        protected CheckBoxList setting;
        protected TabPage tabPage33;
        protected pageinfo info1;
        protected HtmlTable powerset;
        protected TextBox topictypes;
        protected DropDownList templateid;
        protected Button SubmitAdd;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && Forums.GetForumList().Count == 0)
            {
                base.Server.Transfer("forum_AddFirstForum.aspx");
            }
        }

        public void InitInfo()
        {
            this.targetforumid.BuildTree(XForum.Root, "name", "fid");
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
                htmlTableCell.Controls.Add(new LiteralControl("<input type='checkbox' id='r" + num + "' onclick='selectRow(" + num + ",this.checked)'><label for='r" + num + "'>" + ug.GroupTitle + "</lable>"));
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
            if (Request["fid"] != "")
            {
                this.targetforumid.SelectedValue = Request["fid"];
                this.addtype.SelectedValue = "1";
                this.targetforumid.Visible = true;
            }
            this.showcolnum.Attributes.Add("style", "display:none");
            this.colcount.SelectedIndex = 0;
            this.colcount.Attributes.Add("onclick", "javascript:document.getElementById('" + this.showcolnum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage51_colcount_0').checked ? 'none' : 'block');");
            this.showclose.Attributes.Add("style", "display:none");
            this.autocloseoption.SelectedIndex = 0;
            this.showtargetforum.Attributes.Add("style", "display:block");
            this.addtype.Attributes.Add("onclick", "javascript:document.getElementById('" + this.showtargetforum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage51_addtype_0').checked ? 'none' : 'block');setColDisplayer(document.getElementById('TabControl1_tabPage51_addtype_0').checked);");
            this.autocloseoption.Attributes.Add("onclick", "javascript:document.getElementById('" + this.showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage22_autocloseoption_0').checked ? 'none' : 'block');");
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

        private void SubmitSameAfter()
        {
            if (base.CheckCookie())
            {
                this.InsertForum(0, XForum.Root.AllChilds.ToList().Max(e => e.DisplayOrder));
            }
        }

        public Int32 SetAfterDisplayOrder(int currentdisplayorder)
        {
            // 大于该顺序的论坛全部递加
            //Forums.UpdateFourmsDisplayOrder(currentdisplayorder);
            foreach (var item in XForum.Root.AllChilds)
            {
                if (item.DisplayOrder > currentdisplayorder)
                {
                    item.DisplayOrder++;
                    item.Save();
                }
            }
            return currentdisplayorder + 1;
        }

        private void SubmitAddChild()
        {
            if (base.CheckCookie())
            {
                if (String.IsNullOrEmpty(this.name.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('论坛名称不能为空');</script>");
                    return;
                }
                if (!this.rewritename.Text.IsNullOrEmpty() && XForum.CheckRewriteNameInvalid(this.rewritename.Text.Trim()))
                {
                    this.rewritename.Text = "";
                    base.RegisterStartupScript("", "<script>alert('URL重写非法!');</script>");
                    return;
                }
                if (Convert.ToInt16(this.colcountnumber.Text) < 1 || Convert.ToInt16(this.colcountnumber.Text) > 9)
                {
                    base.RegisterStartupScript("", "<script>alert('列值必须在2~9范围内');</script>");
                    return;
                }
                if (this.targetforumid.SelectedValue != "0")
                {
                    var f = XForum.FindByID(this.targetforumid.SelectedValue.ToInt());
                    //var forumInfo = Forums.GetForumInfo(this.targetforumid.SelectedValue.ToInt(0));
                    var forumInfo = f as IXForum;
                    //string parentidlist;
                    //if (forumInfo.Parentidlist == "0")
                    //    parentidlist = forumInfo.ID.ToString();
                    //else
                    //    parentidlist = (forumInfo.Parentidlist + "").Trim() + "," + forumInfo.ID;

                    //DataTable forumList = Forums.GetForumList(this.targetforumid.SelectedValue.ToInt(0));
                    var forumList = f.AllChilds;
                    int afterDisplayOrder;
                    //if (forumList.Rows.Count > 0)
                    //	afterDisplayOrder = TypeConverter.ObjectToInt(forumList.Compute("Max(displayorder)", ""));
                    if (forumList.Count > 0)
                        afterDisplayOrder = forumList.ToList().Max(e => e.DisplayOrder);
                    else
                        afterDisplayOrder = forumInfo.DisplayOrder;

                    this.InsertForum(forumInfo.ID, this.SetAfterDisplayOrder(afterDisplayOrder));
                    //Forums.UpdateSubForumCount(forumInfo.ID);
                    //f.SubforumCount++;
                    f.Update();
                    return;
                }
                this.InsertForum(0, XForum.Root.AllChilds.ToList().Max(e => e.DisplayOrder) + 1);
            }
        }

        private void SubmitAdd_Click(object sender, EventArgs e)
        {
            if (this.addtype.SelectedValue == "0")
            {
                this.SubmitSameAfter();
                return;
            }
            if (this.targetforumid.SelectedValue == "0")
            {
                base.RegisterStartupScript("", "<script>alert('请选择所属论坛版块');</script>");
                return;
            }
            this.SubmitAddChild();
        }

        public void InsertForum(Int32 parentid, Int32 systemdisplayorder)
        {
            //需要初始化 forumInfo
            forumInfo = new XForum();
            this.forumInfo.ParentID = parentid;
            //this.forumInfo.Layer = layer.ToInt();
            //this.forumInfo.Parentidlist = parentidlist;
            //this.forumInfo.SubforumCount = subforumcount.ToInt();
            this.forumInfo.Name = this.name.Text.Trim();
            this.forumInfo.Status = this.status.SelectedValue.ToInt();
            this.forumInfo.DisplayOrder = systemdisplayorder;
            this.forumInfo.TemplateID = this.templateid.SelectedValue.ToInt();
            this.forumInfo.AllowSmilies = this.setting.Items[0].Selected;
            this.forumInfo.AllowRss = this.setting.Items[1].Selected;
            //this.forumInfo.AllowHtml = 0;
            this.forumInfo.AllowBbCode = this.setting.Items[2].Selected;
            this.forumInfo.AllowImgCode = this.setting.Items[3].Selected;
            //this.forumInfo.AllowBlog = 0;
            //this.forumInfo.IsTrade = 0;
            //this.forumInfo.AllowEditRules = 0;
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
            //this.forumInfo.PostcrEdits = "";
            //this.forumInfo.ReplycrEdits = "";
            this.forumInfo.Redirect = this.redirect.Text;
            this.forumInfo.Attachextensions = this.attachextensions.GetSelectString(",");
            this.forumInfo.Moderators = this.moderators.Text;
            this.forumInfo.Rules = this.rules.Text;
            this.forumInfo.Seokeywords = this.seokeywords.Text.Trim();
            this.forumInfo.Seodescription = this.seodescription.Text.Trim();
            this.forumInfo.RewriteName = this.rewritename.Text.Trim();
            this.forumInfo.TopicTypes = this.topictypes.Text;
            this.forumInfo.ColCount = colcount.SelectedValue == "1" ? 1 : colcountnumber.Text.ToInt();
            this.forumInfo.ViewPerm = base.Request.Form["viewperm"];
            this.forumInfo.PostPerm = base.Request.Form["postperm"];
            this.forumInfo.ReplyPerm = base.Request.Form["replyperm"];
            this.forumInfo.GetattachPerm = base.Request.Form["getattachperm"];
            this.forumInfo.PostattachPerm = base.Request.Form["postattachperm"];
            string text;
            int fid = AdminForums.CreateForums(this.forumInfo, out text, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
            if (HttpContext.Current.Request.Files.Count > 0 && !string.IsNullOrEmpty(HttpContext.Current.Request.Files[0].FileName))
            {
                this.forumInfo = Forums.GetForumInfo(fid);
                this.forumInfo.Icon = AdminForums.UploadForumIcon(this.forumInfo.ID);
                AdminForums.UpdateForumInfo(this.forumInfo).Replace("'", "’");
                ForumOperator.RefreshForumCache();
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                config.Specifytemplate = ((Forums.GetSpecifyForumTemplateCount() > 0) ? 1 : 0);
                config.Save();

                //config.Save();;
            }
            if (string.IsNullOrEmpty(text))
            {
                base.RegisterStartupScript("PAGE", "self.location.href='forum_ForumsTree.aspx';");
                return;
            }
            base.RegisterStartupScript("PAGE", "alert('用户:" + text + "不存在,因为无法设为版主');self.location.href='forum_ForumsTree.aspx';");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.SubmitAdd.Click += new EventHandler(this.SubmitAdd_Click);
            this.InitInfo();
        }
    }
}