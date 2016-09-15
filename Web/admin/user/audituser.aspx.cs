using System;
using NewLife;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using XCode;
using XUser = BBX.Entity.User;

namespace BBX.Web.Admin
{
    public partial class auditnewuser : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "审核用户列表";
            this.DataGrid1.DataKeyField = "uid";
            //DataTable userListByGroupid = Users.GetUserListByGroupid(8);
            //this.DataGrid1.BindData(userListByGroupid);
            //this.AllDelete.Enabled = (userListByGroupid.Rows.Count > 0);
            //this.AllPass.Enabled = (userListByGroupid.Rows.Count > 0);
            EntityList<XUser> list = XUser.FindAllByGroupID(8);
            this.DataGrid1.BindData(list.ToDataTable(false));
            this.AllDelete.Enabled = list.Count > 0;
            this.AllPass.Enabled = list.Count > 0;
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void SelectPass_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string uid = Request["uid"];
                if (uid != "")
                {
                    if (CreditsFacade.GetCreditsUserGroupId(0f) != null)
                    {
                        int groupid = CreditsFacade.GetCreditsUserGroupId(0f).ID;
                        //Users.UpdateUserGroupByUidList(groupid, uid);
                        //IUser user = XUser.FindByID(uid.ToInt());
                        string[] array = uid.Split(',');
                        for (int i = 0; i < array.Length; i++)
                        {
                            string value = array[i];
                            Int32 id = Convert.ToInt32(value);
                            XUser user = XUser.FindByID(id);
                            user.GroupID = groupid;
                            user.Save();
                            CreditsFacade.UpdateUserCredits(id);
                        }
                        //Users.ClearUsersAuthstr(uid);
                        XUser.ClearUsersAuthstr(uid);
                    }
                    if (this.sendemail.Checked)
                    {
                        Users.SendEmailForAccountCreateSucceed(uid);
                    }
                    base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location='forum_audituser.aspx';</script>");
            }
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string uid = Request["uid"];
                if (uid != "")
                {
                    //Users.DeleteUsers(uid);
                    BBX.Entity.User.FindAllByIDs(uid).Delete();
                    base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location='forum_audituser.aspx';</script>");
            }
        }

        private void AllPass_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (CreditsFacade.GetCreditsUserGroupId(0f) != null)
                {
                    int groupid = CreditsFacade.GetCreditsUserGroupId(0f).ID;
                    UserGroup.ChangeAllUserGroupId(8, groupid);
                    //foreach (DataRow dataRow in Users.GetUserListByGroupid(8).Rows)
                    foreach (XUser user in XUser.FindAllByGroupID(8))
                    {
                        //CreditsFacade.UpdateUserCredits(Convert.ToInt32(dataRow["uid"].ToString()));
                        CreditsFacade.UpdateUserCredits(user.ID);
                    }
                    //Users.ClearUsersAuthstrByUncheckedUserGroup();
                    XUser.ClearUsersAuthstr(XUser.GetUidListByUserGroupId(8));
                }
                if (this.sendemail.Checked)
                {
                    Users.SendEmailForUncheckedUserGroup();
                }
                base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
            }
        }

        private void AllDelete_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                XUser.DeleteAuditUser();
                base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
            }
        }

        protected void searchuser_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //this.DataGrid1.BindData(Users.AuditNewUserClear(this.searchusername.Text, this.regbefore.Text, this.regip.Text));
                this.DataGrid1.BindData(XUser.AuditNewUserClear(searchusername.Text, Int32.Parse(regbefore.Text), regip.Text));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
            this.AllPass.Click += new EventHandler(this.AllPass_Click);
            this.AllDelete.Click += new EventHandler(this.AllDelete_Click);
            this.DataGrid1.DataKeyField = "uid";
            this.DataGrid1.TableHeaderName = "审核用户列表";
            this.DataGrid1.ColumnSpan = 8;
        }
    }
}