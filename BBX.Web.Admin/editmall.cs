using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class editmall : AdminPage
    {
        protected HtmlForm Form1;
        protected Literal forumname;
        protected TabControl TabControl1;
        protected TabPage tabPage1;
        protected BBX.Control.TextBox name;
        protected BBX.Control.RadioButtonList status;
        protected BBX.Control.RadioButtonList allowspecialonly;
        protected TextareaResize moderators;
        protected Literal inheritmoderators;
        protected BBX.Control.RadioButtonList colcount;
        protected HtmlGenericControl showcolnum;
        protected BBX.Control.TextBox colcountnumber;
        protected TextareaResize description;
        protected HtmlGenericControl templatestyle;
        protected BBX.Control.DropDownList templateid;
        protected TabPage tabPage2;
        protected BBX.Control.TextBox password;
        protected BBX.Control.TextBox redirect;
        protected BBX.Control.TextBox rules;
        protected BBX.Control.TextBox icon;
        protected BBX.Control.CheckBoxList attachextensions;
        protected BBX.Control.RadioButtonList autocloseoption;
        protected HtmlGenericControl showclose;
        protected BBX.Control.TextBox autocloseday;
        protected BBX.Control.CheckBoxList setting;
        protected TabPage tabPage3;
        protected pageinfo PageInfo1;
        protected HtmlTable powerset;
        protected TabPage tabPage4;
        protected BBX.Control.Button DelButton;
        protected BBX.Control.DataGrid SpecialUserList;
        protected pageinfo info1;
        protected TextareaResize UserList;
        protected BBX.Control.Button BindPower;
        protected TabPage tabPage5;
        protected BBX.Control.RadioButtonList applytopictype;
        protected BBX.Control.RadioButtonList viewbytopictype;
        protected BBX.Control.RadioButtonList postbytopictype;
        protected BBX.Control.RadioButtonList topictypeprefix;
        protected BBX.Control.DataGrid TopicTypeDataGrid;
        protected TabPage tabPage6;
        protected Label forumsstatic;
        protected BBX.Control.Button RunForumStatic;
        protected BBX.Control.TextBox displayorder;
        protected BBX.Control.TextBox topictypes;
        protected BBX.Control.Button SubmitInfo;
        protected Hint Hint1;
        public string runforumsstatic;
        public DataRow dr;
        public IXForum forumInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.TabControl1.Items.Remove(this.TabControl1.Items[5]);
                this.TabControl1.Items.Remove(this.TabControl1.Items[4]);
                if (String.IsNullOrEmpty(Request["fid"]))
                {
                    return;
                }
                this.BindTopicType();
                this.DataGridBind("");
            }
        }

        public void LoadCurrentForumInfo(int fid)
        {
            if (fid <= 0)
            {
                return;
            }
            this.forumInfo = Forums.GetForumInfo(fid);
            if (this.forumInfo.Layer > 0)
            {
                this.tabPage2.Visible = true;
                this.tabPage6.Visible = true;
            }
            else
            {
                this.TabControl1.Items.Remove(this.tabPage2);
                this.tabPage2.Visible = false;
                this.TabControl1.Items.Remove(this.tabPage4);
                this.tabPage4.Visible = false;
                this.TabControl1.Items.Remove(this.tabPage5);
                this.tabPage5.Visible = false;
                this.TabControl1.Items.Remove(this.tabPage6);
                this.tabPage6.Visible = false;
                this.templatestyle.Visible = false;
            }
            this.forumname.Text = this.forumInfo.Name.Trim();
            this.name.Text = this.forumInfo.Name.Trim();
            this.displayorder.Text = this.forumInfo.DisplayOrder.ToString();
            this.status.SelectedValue = this.forumInfo.Status.ToString();
            if (this.forumInfo.ColCount == 1)
            {
                this.showcolnum.Attributes.Add("style", "display:none");
                this.colcount.SelectedIndex = 0;
            }
            else
            {
                this.showcolnum.Attributes.Add("style", "display:block");
                this.colcount.SelectedIndex = 1;
            }
            this.colcount.Attributes.Add("onclick", "javascript:document.getElementById('" + this.showcolnum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage1_colcount_0').checked ? 'none' : 'block');");
            this.colcountnumber.Text = this.forumInfo.ColCount.ToString();
            this.templateid.SelectedValue = this.forumInfo.TemplateID.ToString();
            this.forumsstatic.Text = string.Format("主题总数:{0}<br />帖子总数:{1}<br />今日回帖数总数:{2}<br />最后提交日期:{3}", new object[]
            {
                this.forumInfo.Topics.ToString(),
                this.forumInfo.Posts.ToString(),
                this.forumInfo.TodayPosts.ToString(),
                this.forumInfo.LastPost.ToString()
            });
            this.ViewState["forumsstatic"] = this.forumsstatic.Text;
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
            if (this.forumInfo.DisableWatermark)
            {
                this.setting.Items[6].Selected = true;
            }
            if (this.forumInfo.Inheritedmod == 1)
            {
                this.setting.Items[7].Selected = true;
            }
            if (this.forumInfo.AllowThumbnail)
            {
                this.setting.Items[8].Selected = true;
            }
            if (this.forumInfo.AllowTag)
            {
                this.setting.Items[9].Selected = true;
            }
            this.allowspecialonly.SelectedValue = this.forumInfo.AllowSpecialOnly.ToString();
            if (this.forumInfo.AutoClose == 0)
            {
                this.showclose.Attributes.Add("style", "display:none");
                this.autocloseoption.SelectedIndex = 0;
            }
            else
            {
                this.autocloseoption.SelectedIndex = 1;
            }
            this.autocloseoption.Attributes.Add("onclick", "javascript:document.getElementById('" + this.showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage2_autocloseoption_0').checked ? 'none' : 'block');");
            this.autocloseday.Text = this.forumInfo.AutoClose.ToString();
            this.description.Text = this.forumInfo.Description.Trim();
            this.password.Text = this.forumInfo.Password.Trim();
            this.icon.Text = this.forumInfo.Icon.Trim();
            this.redirect.Text = this.forumInfo.Redirect.Trim();
            this.moderators.Text = this.forumInfo.Moderators.Trim();
            this.inheritmoderators.Text = Users.GetModerators(fid);
            this.rules.Text = this.forumInfo.Rules.Trim();
            this.topictypes.Text = this.forumInfo.TopicTypes.Trim();
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
                htmlTableRow.Cells.Add(this.GetTD("viewperm", this.forumInfo.ViewPerm.Trim(), ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("postperm", this.forumInfo.PostPerm.Trim(), ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("replyperm", this.forumInfo.ReplyPerm.Trim(), ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("getattachperm", this.forumInfo.GetattachPerm.Trim(), ug.ID, num));
                htmlTableRow.Cells.Add(this.GetTD("postattachperm", this.forumInfo.PostattachPerm.Trim(), ug.ID, num));
                this.powerset.Rows.Add(htmlTableRow);
                num++;
            }
            //var dataTable = Attachments.GetAttachmentType();
            this.attachextensions.SetSelectByID(this.forumInfo.Attachextensions.Trim());
            if (fid > 0)
            {
                this.forumInfo = Forums.GetForumInfo(fid);
                this.applytopictype.SelectedValue = this.forumInfo.ApplytopicType.ToString();
                this.postbytopictype.SelectedValue = this.forumInfo.PostbytopicType.ToString();
                this.viewbytopictype.SelectedValue = this.forumInfo.ViewbytopicType.ToString();
                this.topictypeprefix.SelectedValue = this.forumInfo.Topictypeprefix.ToString();
                return;
            }
        }

        private void BindTopicType()
        {
            this.TopicTypeDataGrid.BindData(TopicType.FindAllWithCache().ToDataTable());
            this.TopicTypeDataGrid.TableHeaderName = "当前版块:  " + this.forumInfo.Name;
        }

        public void TopicTypeDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            string[] array = this.forumInfo.TopicTypes.Split('|');
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                string text = e.Item.Cells[0].Text;
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text2 = array2[i];
                    if (text2.Split(',')[0] == text)
                    {
                        e.Item.Cells[3].Text = "<input type='hidden' name='oldtopictype" + e.Item.ItemIndex + "' value='" + text2 + "|' /><input type='radio' name='type" + e.Item.ItemIndex + "' value='-1' />";
                        if ((text2 + "&").IndexOf(",0&") < 0)
                        {
                            e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + text + "," + e.Item.Cells[1].Text + ",0|' />";
                        }
                        else
                        {
                            e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' checked value='" + text + "," + e.Item.Cells[1].Text + ",0|' />";
                        }
                        if ((text2 + "&").IndexOf(",1&") < 0)
                        {
                            e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + text + "," + e.Item.Cells[1].Text + ",1|' />";
                        }
                        else
                        {
                            e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' checked value='" + text + "," + e.Item.Cells[1].Text + ",1|' />";
                        }
                        return;
                    }
                }
                e.Item.Cells[3].Text = "<input type='hidden' name='oldtopictype" + e.Item.ItemIndex + "' value='' /><input type='radio' name='type" + e.Item.ItemIndex + "' checked value='-1' />";
                e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + text + "," + e.Item.Cells[1].Text + ",0|' />";
                e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + text + "," + e.Item.Cells[1].Text + ",1|' />";
            }
        }

        public void TopicTypeDataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.TopicTypeDataGrid.LoadCurrentPageIndex(e.NewPageIndex);
            this.BindTopicType();
            this.DataGridBind("");
            this.TabControl1.SelectedIndex = 4;
        }

        private HtmlTableCell GetTD(string strPerfix, string groupList, Int32 groupId, int ctlId)
        {
            groupList = "," + groupList + ",";
            string text = "<input type='checkbox' name='" + strPerfix + "' id='" + strPerfix + ctlId + "' value='" + groupId + "' " + ((groupList.IndexOf("," + groupId + ",") == -1) ? "" : "checked='checked'") + ">";
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

        public void BindPower_Click(object sender, EventArgs e)
        {
            if (this.UserList.Text != "")
            {
                string text = this.forumInfo.Permuserlist;
                string[] array = new string[]
                {
                    ""
                };
                if (text != null)
                {
                    array = this.forumInfo.Permuserlist.Split('|');
                }
                string[] array2 = this.UserList.Text.Split(',');
                for (int i = 0; i < array2.Length; i++)
                {
                    string text2 = array2[i];
                    string text3 = Users.GetUserId(text2).ToString();
                    if (!(text3 == "-1"))
                    {
                        bool flag = false;
                        string[] array3 = array;
                        for (int j = 0; j < array3.Length; j++)
                        {
                            string text4 = array3[j];
                            if (text4.IndexOf(text2 + ",") == 0)
                            {
                                text = text.Replace(text4, text2 + "," + text3 + "," + 0);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            text = text2 + "," + text3 + "," + 0 + "|" + text;
                        }
                    }
                }
                if (text != "" && text.Substring(text.Length - 1, 1) == "|")
                {
                    text = text.Substring(0, text.Length - 1);
                }
                this.forumInfo.Permuserlist = text;
                AdminForums.UpdateForumInfo(this.forumInfo);
                this.UserList.Text = "";
            }
            this.DataGridBind("");
            this.BindTopicType();
            this.TabControl1.SelectedIndex = 3;
        }

        public void DelButton_Click(object sender, EventArgs e)
        {
            int num = 0;
            ArrayList arrayList = new ArrayList(this.forumInfo.Permuserlist.Split('|'));
            foreach (object current in this.SpecialUserList.GetKeyIDArray())
            {
                if (this.SpecialUserList.GetCheckBoxValue(num, "userid"))
                {
                    string str = current.ToString();
                    string[] array = this.forumInfo.Permuserlist.Split('|');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text = array[i];
                        if (text.IndexOf("," + str + ",") > 0)
                        {
                            arrayList.Remove(text);
                            break;
                        }
                    }
                }
                num++;
            }
            string text2 = "";
            foreach (string str2 in arrayList)
            {
                text2 = text2 + str2 + "|";
            }
            if (text2 != "")
            {
                text2 = text2.Substring(0, text2.Length - 1);
            }
            this.forumInfo.Permuserlist = text2;
            AdminForums.UpdateForumInfo(this.forumInfo);
            if (this.SpecialUserList.Items.Count == 1 && this.SpecialUserList.CurrentPageIndex > 0)
            {
                this.SpecialUserList.CurrentPageIndex--;
            }
            this.DataGridBind("");
            this.BindTopicType();
            this.TabControl1.SelectedIndex = 3;
        }

        private void DataGridBind(string userList)
        {
            this.SpecialUserList.TableHeaderName = "特殊用户权限设置";
            string permuserlist = this.forumInfo.Permuserlist;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id", typeof(Int32));
            dataTable.Columns.Add("uid", typeof(Int32));
            dataTable.Columns.Add("name", typeof(String));
            dataTable.Columns.Add("viewbyuser", typeof(Boolean));
            dataTable.Columns.Add("postbyuser", typeof(Boolean));
            dataTable.Columns.Add("replybyuser", typeof(Boolean));
            dataTable.Columns.Add("getattachbyuser", typeof(Boolean));
            dataTable.Columns.Add("postattachbyuser", typeof(Boolean));
            string[] array = userList.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (!(String.IsNullOrEmpty(text.Trim())))
                {
                    int userId = Users.GetUserId(text);
                    if (userId != -1)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["id"] = dataTable.Rows.Count + 1;
                        dataRow["uid"] = userId.ToString();
                        dataRow["name"] = text;
                        dataRow["viewbyuser"] = false;
                        dataRow["postbyuser"] = false;
                        dataRow["replybyuser"] = false;
                        dataRow["getattachbyuser"] = false;
                        dataRow["postattachbyuser"] = false;
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            if (permuserlist != null)
            {
                string[] array2 = permuserlist.Split('|');
                for (int j = 0; j < array2.Length; j++)
                {
                    string text2 = array2[j];
                    if (("," + userList + ",").IndexOf("," + text2.Split(',')[0] + ",") < 0)
                    {
                        int num = Convert.ToInt32(text2.Split(',')[2]);
                        DataRow dataRow2 = dataTable.NewRow();
                        dataRow2["id"] = dataTable.Rows.Count + 1;
                        dataRow2["uid"] = text2.Split(',')[1];
                        dataRow2["name"] = text2.Split(',')[0];
                        dataRow2["viewbyuser"] = (num & 1);
                        dataRow2["postbyuser"] = (num & 2);
                        dataRow2["replybyuser"] = (num & 4);
                        dataRow2["getattachbyuser"] = (num & 8);
                        dataRow2["postattachbyuser"] = (num & 16);
                        dataTable.Rows.Add(dataRow2);
                    }
                }
            }
            this.SpecialUserList.DataSource = dataTable;
            this.SpecialUserList.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.SpecialUserList.LoadCurrentPageIndex(e.NewPageIndex);
            this.DataGridBind("");
            this.BindTopicType();
            this.TabControl1.SelectedIndex = 3;
        }

        public int BoolToInt(bool a)
        {
            if (!a)
            {
                return 0;
            }
            return 1;
        }

        private void SubmitInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie() && Request["fid"] != "")
            {
                this.forumInfo = Forums.GetForumInfo(DNTRequest.GetInt("fid", 0));
                this.forumInfo.Name = this.name.Text.Trim();
                this.forumInfo.DisplayOrder = this.displayorder.Text.ToInt();
                this.forumInfo.Status = this.status.SelectedValue.ToInt();
                if (this.colcount.SelectedValue == "1")
                {
                    this.forumInfo.ColCount = 1;
                }
                else
                {
                    var count = colcountnumber.Text.ToInt();
                    if (count < 1 || count > 9)
                    {
                        base.RegisterStartupScript("", "<script>alert('列值必须在2~9范围内');</script>");
                        return;
                    }
                    this.forumInfo.ColCount = count;
                }
                this.forumInfo.TemplateID = ((this.templateid.SelectedValue.ToInt() == this.config.Templateid) ? 0 : this.templateid.SelectedValue.ToInt());
                //this.forumInfo.AllowHtml = 0;
                //this.forumInfo.AllowBlog = 0;
                //this.forumInfo.AllowEditRules = 0;
                this.forumInfo.AllowSmilies = this.setting.Items[0].Selected;
                this.forumInfo.AllowRss = this.setting.Items[1].Selected;
                this.forumInfo.AllowBbCode = this.setting.Items[2].Selected;
                this.forumInfo.AllowImgCode = this.setting.Items[3].Selected;
                this.forumInfo.Recyclebin = this.BoolToInt(this.setting.Items[4].Selected);
                this.forumInfo.Modnewposts = this.BoolToInt(this.setting.Items[5].Selected);
                this.forumInfo.DisableWatermark = this.setting.Items[6].Selected;
                this.forumInfo.Inheritedmod = this.BoolToInt(this.setting.Items[7].Selected);
                this.forumInfo.AllowThumbnail = this.setting.Items[8].Selected;
                this.forumInfo.AllowTag = this.setting.Items[9].Selected;
                int allowpostspecial = 0;
                this.forumInfo.AllowPostSpecial = allowpostspecial;
                this.forumInfo.AllowSpecialOnly = (int)Convert.ToInt16(this.allowspecialonly.SelectedValue) != 0;
                if (this.autocloseoption.SelectedValue == "0")
                {
                    this.forumInfo.AutoClose = 0;
                }
                else
                {
                    this.forumInfo.AutoClose = this.autocloseday.Text.ToInt();
                }
                this.forumInfo.Description = this.description.Text;
                this.forumInfo.Password = this.password.Text;
                this.forumInfo.Icon = this.icon.Text;
                this.forumInfo.Redirect = this.redirect.Text;
                this.forumInfo.Attachextensions = this.attachextensions.GetSelectString(",");
                AdminForums.CompareOldAndNewModerator(this.forumInfo.Moderators, this.moderators.Text.Replace("\r\n", ","), DNTRequest.GetInt("fid", 0));
                this.forumInfo.Moderators = this.moderators.Text.Replace("\r\n", ",");
                this.forumInfo.Rules = this.rules.Text;
                this.forumInfo.TopicTypes = this.topictypes.Text;
                this.forumInfo.ViewPerm = base.Request.Form["viewperm"];
                this.forumInfo.PostPerm = base.Request.Form["postperm"];
                this.forumInfo.ReplyPerm = base.Request.Form["replyperm"];
                this.forumInfo.GetattachPerm = base.Request.Form["getattachperm"];
                this.forumInfo.PostattachPerm = base.Request.Form["postattachperm"];
                this.forumInfo.ApplytopicType = Convert.ToSByte(this.applytopictype.SelectedValue);
                this.forumInfo.PostbytopicType = Convert.ToSByte(this.postbytopictype.SelectedValue);
                this.forumInfo.ViewbytopicType = Convert.ToSByte(this.viewbytopictype.SelectedValue);
                this.forumInfo.Topictypeprefix = Convert.ToSByte(this.topictypeprefix.SelectedValue);
                this.forumInfo.TopicTypes = this.GetTopicType();
                this.forumInfo.Permuserlist = this.GetPermuserlist();
                //AggregationFacade.ForumAggregation.ClearDataBind();
                string text = AdminForums.UpdateForumInfo(this.forumInfo).Replace("'", "’");
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑论坛版块", "编辑论坛版块,名称为:" + this.name.Text.Trim());
                var config = GeneralConfigInfo.Current;
                config.Specifytemplate = ((Forums.GetSpecifyForumTemplateCount() > 0) ? 1 : 0);
                config.Save();

                //config.Save();;
                if (String.IsNullOrEmpty(text))
                {
                    base.Response.Redirect("forum_ForumsTree.aspx");
                    return;
                }
                base.Response.Write("<script>alert('用户:" + text + "不存在或因为它们所属组为\"游客\",\"等待验证会员\",因为无法设为版主');window.location.href='forum_ForumsTree.aspx';</script>");
                base.Response.End();
            }
        }

        private string GetTopicType()
        {
            string text = this.forumInfo.TopicTypes;
            int num = 0;
            DataTable topicTypes = TopicType.FindAllWithCache().ToDataTable();
            while (!(String.IsNullOrEmpty(DNTRequest.GetFormString("type" + num))))
            {
                if (DNTRequest.GetFormString("type" + num) != "-1")
                {
                    string formString = DNTRequest.GetFormString("oldtopictype" + num);
                    string formString2 = DNTRequest.GetFormString("type" + num);
                    if (String.IsNullOrEmpty(formString))
                    {
                        int displayOrder = this.GetDisplayOrder(formString2.Split(',')[1], topicTypes);
                        ArrayList arrayList = new ArrayList();
                        string[] array = text.Split('|');
                        for (int i = 0; i < array.Length; i++)
                        {
                            string text2 = array[i];
                            if (text2 != "")
                            {
                                arrayList.Add(text2);
                            }
                        }
                        bool flag = false;
                        for (int j = 0; j < arrayList.Count; j++)
                        {
                            int displayOrder2 = this.GetDisplayOrder(arrayList[j].ToString().Split(',')[1], topicTypes);
                            if (displayOrder2 > displayOrder)
                            {
                                arrayList.Insert(j, formString2);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            arrayList.Add(formString2);
                        }
                        text = "";
                        IEnumerator enumerator = arrayList.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                object current = enumerator.Current;
                                text = text + current.ToString() + "|";
                            }
                            goto IL_215;
                        }
                        finally
                        {
                            IDisposable disposable = enumerator as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    text = text.Replace(formString, formString2);
                }
                else
                {
                    if (DNTRequest.GetFormString("oldtopictype" + num) != "")
                    {
                        text = text.Replace(DNTRequest.GetFormString("oldtopictype" + num), "");
                    }
                }
            IL_215:
                num++;
            }
            return text;
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            foreach (DataRow dataRow in topicTypes.Rows)
            {
                if (dataRow["name"].ToString().Trim() == topicTypeName.Trim())
                {
                    return int.Parse(dataRow["displayorder"].ToString());
                }
            }
            return -1;
        }

        private string GetTopicTypeString(string topicTypes, string topicName)
        {
            string[] array = topicTypes.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (text.IndexOf("," + topicName.Trim() + ",") != -1)
                {
                    return text;
                }
            }
            return "";
        }

        private string GetPermuserlist()
        {
            int num = 0;
            string text = this.forumInfo.Permuserlist;
            if (text == null)
            {
                return "";
            }
            foreach (object current in this.SpecialUserList.GetKeyIDArray())
            {
                string text2 = current.ToString();
                int num2 = 0;
                if (this.SpecialUserList.GetCheckBoxValue(num, "viewbyuser"))
                {
                    num2 |= 1;
                }
                if (this.SpecialUserList.GetCheckBoxValue(num, "postbyuser"))
                {
                    num2 |= 2;
                }
                if (this.SpecialUserList.GetCheckBoxValue(num, "replybyuser"))
                {
                    num2 |= 4;
                }
                if (this.SpecialUserList.GetCheckBoxValue(num, "getattachbyuser"))
                {
                    num2 |= 8;
                }
                if (this.SpecialUserList.GetCheckBoxValue(num, "postattachbyuser"))
                {
                    num2 |= 16;
                }
                string[] array = this.forumInfo.Permuserlist.Split('|');
                bool flag = false;
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text3 = array2[i];
                    if (text3.IndexOf("," + text2 + ",") > 0)
                    {
                        text = text.Replace(text3, text3.Split(',')[0] + "," + text2 + "," + num2);
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    IUser shortUserInfo = BBX.Entity.User.FindByID(text2.ToInt());
                    text = ((shortUserInfo != null) ? shortUserInfo.Name.Trim() : "") + "," + text2 + "," + num2 + "|" + text;
                }
                num++;
            }
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }
            if (text.Substring(text.Length - 1, 1) == "|")
            {
                return text.Substring(0, text.Length - 1);
            }
            return text;
        }

        private void RunForumStatic_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.forumsstatic.Text = this.ViewState["forumsstatic"].ToString();
                int fid = DNTRequest.GetInt("fid", -1);
                if (fid <= 0) return;

                var f = XForum.FindByID(fid);
                // 克隆一份，后面要比对
                this.forumInfo = f.CloneEntity();
                //int num = 0;
                //int num2 = 0;
                //int num3 = 0;
                //string text = "";
                //DateTime lastpost = DateTime.MinValue;
                //int num4 = 0;
                //string text3 = "";
                //int num5 = 0;
                //AdminForumStats.ReSetFourmTopicAPost(fid, out num, out num2, out num3, out text, out lastpost, out num4, out text3, out num5);
                // 重置最后发帖信息
                f.ResetLastPost();
                this.runforumsstatic = string.Format("<br /><br />运行结果<hr style=\"height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; \" align=\"left\" />主题总数:{0}<br />帖子总数:{1}<br />今日回帖数总数:{2}<br />最后提交日期:{3}", new object[]
                {
                    f.Topics,
                    f.Posts,
                    f.TodayPosts,
                    f.LastPost.ToFullString()
                });
                if (this.forumInfo.Topics == f.Topics &&
                    this.forumInfo.Posts == f.Posts &&
                    this.forumInfo.TodayPosts == f.TodayPosts &&
                    this.forumInfo.LastPost == f.LastPost)
                {
                    this.runforumsstatic += "<br /><br /><br />结果一致";
                }
                else
                {
                    this.runforumsstatic += "<br /><br /><br />比较<hr style=\"height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; \" align=\"left\" />";
                    if (this.forumInfo.Topics != f.Topics)
                    {
                        this.runforumsstatic += "主题总数有差异<br />";
                    }
                    if (this.forumInfo.Posts != f.Posts)
                    {
                        this.runforumsstatic += "帖子总数有差异<br />";
                    }
                    if (this.forumInfo.TodayPosts != f.TodayPosts)
                    {
                        this.runforumsstatic += "今日回帖数总数有差异<br />";
                    }
                    if (this.forumInfo.LastPost != f.LastPost)
                    {
                        this.runforumsstatic += "最后提交日期有差异<br />";
                    }
                }
            }
            this.TabControl1.SelectedIndex = 5;
            this.DataGridBind("");
            this.BindTopicType();
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.TopicTypeDataGrid.Sort = e.SortExpression.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.TabControl1.SelectedIndex = DNTRequest.GetInt("tabindex", 0);
            this.SpecialUserList.PageIndexChanged += new DataGridPageChangedEventHandler(this.DataGrid_PageIndexChanged);
            this.TopicTypeDataGrid.ItemDataBound += new DataGridItemEventHandler(this.TopicTypeDataGrid_ItemDataBound);
            this.TopicTypeDataGrid.SortCommand += new DataGridSortCommandEventHandler(this.Sort_Grid);
            this.TopicTypeDataGrid.PageIndexChanged += new DataGridPageChangedEventHandler(this.TopicTypeDataGrid_PageIndexChanged);
            this.TopicTypeDataGrid.AllowCustomPaging = false;
            this.TopicTypeDataGrid.DataKeyField = "id";
            this.TopicTypeDataGrid.ColumnSpan = 6;
            this.SubmitInfo.Click += new EventHandler(this.SubmitInfo_Click);
            this.RunForumStatic.Click += new EventHandler(this.RunForumStatic_Click);
            this.BindPower.Click += new EventHandler(this.BindPower_Click);
            this.DelButton.Click += new EventHandler(this.DelButton_Click);
            this.templateid.AddTableData(Template.GetValids(), "name", "id");
            this.attachextensions.AddTableData(AttachType.FindAllWithCache(), null, null);
            this.LoadCurrentForumInfo(DNTRequest.GetInt("fid", -1));
            this.SpecialUserList.AllowPaging = true;
            this.SpecialUserList.DataKeyField = "id";
        }
    }
}