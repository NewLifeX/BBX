using System;
using NewLife;
using System.Data;
using System.Web.UI.WebControls;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using XCode;
using XUser = BBX.Entity.User;

namespace BBX.Web.Admin
{
    public partial class usergrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.joindateStart.SelectedDate = DateTime.Now.AddDays(-30.0);
                this.joindateEnd.SelectedDate = DateTime.Now;
                this.UserGroup.AddTableData(BBX.Entity.UserGroup.GetAll(), "grouptitle", "id");
                String username = Request["username"];
                String where = ViewState["condition"] + "";
                if (!String.IsNullOrEmpty(username))
                {
                    where = String.Format("name='{0}'", username);
                    this.searchtable.Visible = false;
                    this.ResetSearchTable.Visible = true;
                }
                if (!String.IsNullOrEmpty(where))
                {
                    this.searchtable.Visible = false;
                    this.ResetSearchTable.Visible = true;
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request["condition"]))
                    {
                        where = Request["condition"].Replace("~^", "'").Replace("~$", "%");
                        this.searchtable.Visible = false;
                        this.ResetSearchTable.Visible = true;
                    }
                }
                ViewState["condition"] = where;
                this.BindData();
            }
        }

        public void BindData()
        {
            String condition = ViewState["condition"] + "";
            Int32 count = 0;
            if (String.IsNullOrEmpty(condition))
                count = XUser.Meta.Count;
            else
                count = XUser.FindCount(condition, null, null, 0, 0);

            this.DataGrid1.AllowCustomPaging = true;
            this.DataGrid1.VirtualItemCount = count;

            this.DataGrid1.DataSource = this.buildGridData();
            this.DataGrid1.DataBind();
        }

        private EntityList<XUser> buildGridData()
        {
            EntityList<XUser> list = XUser.FindAll(this.ViewState["condition"] + "", null, null, (this.DataGrid1.CurrentPageIndex) * this.DataGrid1.PageSize, this.DataGrid1.PageSize);
            if (list.Count == 1 && Request["username"] != null && Request["username"] != "")
            {
                base.Response.Redirect("edituser.aspx?uid=" + list[0].ID);
            }
            return list;
        }

        public void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void StopTalk_Click(object sender, EventArgs e)
        {
            if (Request["uid"] != "")
            {
                string text = "0" + Request["uid"];
                string[] array = text.Split(',');
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string s = array2[i];
                    int num = int.Parse(s);
                    if (num != 0)
                    {
                        Online.DeleteUserByUid(num);
                    }
                }
                Users.UpdateUserToStopTalkGroup(text);
                base.RegisterStartupScript("PAGE", "window.location.href='usergrid.aspx';");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location.href='usergrid.aspx';</script>");
        }

        private bool CheckSponser(int uid)
        {
            return BaseConfigInfo.Current.Founderuid != uid || BaseConfigInfo.Current.Founderuid == this.userid;
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string text = Request["uid"].Trim(',');
                if (text != "")
                {
                    bool delposts = this.deltype.SelectedValue.IndexOf("1") < 0;
                    bool delpms = this.deltype.SelectedValue.IndexOf("2") < 0;
                    string[] array = text.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text2 = array[i];
                        if (text2 != "" && this.CheckSponser(Utility.ToInt(text2, 0)) && Utility.ToInt(text2, 0) > 1)
                        {
                            int userid = Utility.ToInt(text2, 0);
                            User user = XUser.FindByID(userid);
                            if (user.Delete(delposts, delpms))
                            {
                                Sync.DeleteUsers(text2, "");
                                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户", "用户名:批量用户删除");
                                base.RegisterStartupScript("PAGE", "window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';");
                            }
                        }
                    }
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';</script>");
            }
        }

        public string GetAvatarUrl(string uid)
        {
            return Avatars.GetAvatarUrl(Int32.Parse(uid), AvatarSize.Small);
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string usersSearchCondition = Users.GetUsersSearchCondition(this.islike.Checked, this.ispostdatetime.Checked, this.Username.Text, this.nickname.Text, this.UserGroup.SelectedValue, this.email.Text, this.credits_start.Text, this.credits_end.Text, this.lastip.Text, this.posts.Text, this.digestposts.Text, this.uid.Text, this.joindateStart.SelectedDate.ToString(), this.joindateEnd.SelectedDate.AddDays(1.0).ToString());
                string where = XUser.SearchWhere(this.islike.Checked, this.ispostdatetime.Checked, this.Username.Text, this.nickname.Text, Utility.ToInt(this.UserGroup.SelectedValue, 0), this.email.Text, Utility.ToInt(this.credits_start.Text, 0), Utility.ToInt(this.credits_end.Text, 0), this.lastip.Text, Utility.ToInt(this.posts.Text, 0), Utility.ToInt(this.digestposts.Text, 0), this.uid.Text, this.joindateStart.SelectedDate, this.joindateEnd.SelectedDate);
                this.ViewState["condition"] = where;
                this.searchtable.Visible = false;
                this.ResetSearchTable.Visible = true;
                //DataTable usersByCondition = Users.GetUsersByCondition(usersSearchCondition);
                DataTable dt = XUser.FindAll(where, null, null, 0, 1).ToDataTable(false);
                if (dt.Rows.Count == 1)
                {
                    base.Response.Redirect("edituser.aspx?uid=" + dt.Rows[0][0].ToString() + "&condition=" + where.Replace("'", "~^").Replace("%", "~$"));
                    return;
                }
                this.DataGrid1.CurrentPageIndex = 0;
                this.BindData();
            }
        }

        private void ResetSearchTable_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("usergrid.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Search.Click += new EventHandler(this.Search_Click);
            this.StopTalk.Click += new EventHandler(this.StopTalk_Click);
            this.DeleteUser.Click += new EventHandler(this.DeleteUser_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(this.GoToPagerButton_Click);
            this.ResetSearchTable.Click += new EventHandler(this.ResetSearchTable_Click);
            this.DataGrid1.TableHeaderName = "用户列表";
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.ColumnSpan = 12;
        }
    }
}