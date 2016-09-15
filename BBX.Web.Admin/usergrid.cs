using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;

namespace Discuz.Web.Admin
{
    public class usergrid : AdminPage
    {
        protected HtmlForm Form1;
        protected Panel searchtable;
        protected Discuz.Control.TextBox Username;
        protected HtmlInputCheckBox islike;
        protected Discuz.Control.DropDownList UserGroup;
        protected Discuz.Control.TextBox uid;
        protected RegularExpressionValidator homephone;
        protected Discuz.Control.TextBox credits_start;
        protected Discuz.Control.TextBox credits_end;
        protected Discuz.Control.TextBox posts;
        protected Discuz.Control.TextBox nickname;
        protected Discuz.Control.TextBox email;
        protected Discuz.Control.Calendar joindateStart;
        protected Discuz.Control.Calendar joindateEnd;
        protected HtmlInputCheckBox ispostdatetime;
        protected Discuz.Control.TextBox lastip;
        protected Discuz.Control.TextBox digestposts;
        protected Discuz.Control.Button Search;
        protected Discuz.Control.Button ResetSearchTable;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.Button StopTalk;
        protected Discuz.Control.Button DeleteUser;
        protected Discuz.Control.CheckBoxList deltype;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.joindateStart.SelectedDate = DateTime.Now.AddDays(-30.0);
                this.joindateEnd.SelectedDate = DateTime.Now;
                this.UserGroup.AddTableData(UserGroups.GetUserGroupForDataTable(), "grouptitle", "groupid");
                if (Request["username"] != null && Request["username"] != "")
                {
                    this.ViewState["condition"] = Users.GetUserListCondition(Request["username"]);
                    this.searchtable.Visible = false;
                    this.ResetSearchTable.Visible = true;
                }
                if (this.ViewState["condition"] != null)
                {
                    this.searchtable.Visible = false;
                    this.ResetSearchTable.Visible = true;
                }
                else
                {
                    if (!String.IsNullOrEmpty(Request["condition"]))
                    {
                        this.ViewState["condition"] = Request["condition"].Replace("~^", "'").Replace("~$", "%");
                        this.searchtable.Visible = false;
                        this.ResetSearchTable.Visible = true;
                    }
                }
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = true;
            this.DataGrid1.VirtualItemCount = this.GetRecordCount();
            this.DataGrid1.DataSource = this.buildGridData();
            this.DataGrid1.DataBind();
        }

        private DataTable buildGridData()
        {
            DataTable dataTable = new DataTable();
            if (this.ViewState["condition"] == null)
            {
                dataTable = Users.GetUserListByCurrentPage(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1);
            }
            else
            {
                dataTable = Users.GetUserList(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1, this.ViewState["condition"].ToString());
            }
            if (dataTable.Rows.Count == 1 && Request["username"] != null && Request["username"] != "")
            {
                base.Response.Redirect("global_edituser.aspx?uid=" + dataTable.Rows[0][0].ToString());
            }
            return dataTable;
        }

        public void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        private int GetRecordCount()
        {
            if (this.ViewState["condition"] == null)
            {
                return Users.GetUserCount("");
            }
            return Users.GetUserCount(this.ViewState["condition"].ToString());
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
                        if (SpacePluginProvider.GetInstance() != null)
                        {
                            SpacePluginProvider.GetInstance().Ban(num);
                        }
                        if (AlbumPluginProvider.GetInstance() != null)
                        {
                            AlbumPluginProvider.GetInstance().Ban(num);
                        }
                        OnlineUsers.DeleteUserByUid(num);
                    }
                }
                Users.UpdateUserToStopTalkGroup(text);
                base.RegisterStartupScript("PAGE", "window.location.href='global_usergrid.aspx';");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location.href='global_usergrid.aspx';</script>");
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
                        if (text2 != "" && this.CheckSponser(Convert.ToInt32(text2)) && Convert.ToInt32(text2) > 1)
                        {
                            int userid = Convert.ToInt32(text2);
                            if (AlbumPluginProvider.GetInstance() != null)
                            {
                                AlbumPluginProvider.GetInstance().Delete(userid);
                            }
                            if (SpacePluginProvider.GetInstance() != null)
                            {
                                SpacePluginProvider.GetInstance().Delete(userid);
                            }
                            if (AdminUsers.DelUserAllInf(userid, delposts, delpms))
                            {
                                Sync.DeleteUsers(text2, "");
                                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户", "用户名:批量用户删除");
                                base.RegisterStartupScript("PAGE", "window.location.href='global_usergrid.aspx?condition=" + Request["condition"] + "';");
                            }
                        }
                    }
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location.href='global_usergrid.aspx?condition=" + Request["condition"] + "';</script>");
            }
        }

        public string GetAvatarUrl(string uid)
        {
            return Avatars.GetAvatarUrl(uid, AvatarSize.Small);
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string usersSearchCondition = Users.GetUsersSearchCondition(this.islike.Checked, this.ispostdatetime.Checked, this.Username.Text, this.nickname.Text, this.UserGroup.SelectedValue, this.email.Text, this.credits_start.Text, this.credits_end.Text, this.lastip.Text, this.posts.Text, this.digestposts.Text, this.uid.Text, this.joindateStart.SelectedDate.ToString(), this.joindateEnd.SelectedDate.AddDays(1.0).ToString());
                this.ViewState["condition"] = usersSearchCondition;
                this.searchtable.Visible = false;
                this.ResetSearchTable.Visible = true;
                DataTable usersByCondition = Users.GetUsersByCondition(usersSearchCondition);
                if (usersByCondition.Rows.Count == 1)
                {
                    base.Response.Redirect("global_edituser.aspx?uid=" + usersByCondition.Rows[0][0].ToString() + "&condition=" + this.ViewState["condition"].ToString().Replace("'", "~^").Replace("%", "~$"));
                    return;
                }
                this.DataGrid1.CurrentPageIndex = 0;
                this.BindData();
            }
        }

        private void ResetSearchTable_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("global_usergrid.aspx");
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
            this.DataGrid1.DataKeyField = "uid";
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.ColumnSpan = 12;
        }
    }
}