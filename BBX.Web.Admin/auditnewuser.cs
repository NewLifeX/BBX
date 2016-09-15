using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class auditnewuser : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected Discuz.Control.TextBox searchusername;
        protected Discuz.Control.TextBox regbefore;
        protected Discuz.Control.TextBox regip;
        protected Discuz.Control.Button searchuser;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.Button SelectPass;
        protected Discuz.Control.Button SelectDelete;
        protected Discuz.Control.Button AllPass;
        protected Discuz.Control.Button AllDelete;
        protected HtmlInputCheckBox sendemail;

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
            DataTable userListByGroupid = Users.GetUserListByGroupid(8);
            this.DataGrid1.BindData(userListByGroupid);
            this.AllDelete.Enabled = (userListByGroupid.Rows.Count > 0);
            this.AllPass.Enabled = (userListByGroupid.Rows.Count > 0);
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
                string @string = Request["uid"];
                if (@string != "")
                {
                    if (CreditsFacade.GetCreditsUserGroupId(0f) != null)
                    {
                        int groupid = CreditsFacade.GetCreditsUserGroupId(0f).ID;
                        Users.UpdateUserGroupByUidList(groupid, @string);
                        string[] array = @string.Split(',');
                        for (int i = 0; i < array.Length; i++)
                        {
                            string value = array[i];
                            CreditsFacade.UpdateUserCredits(Convert.ToInt32(value));
                        }
                        Users.ClearUsersAuthstr(@string);
                    }
                    if (this.sendemail.Checked)
                    {
                        Users.SendEmailForAccountCreateSucceed(@string);
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
                string @string = Request["uid"];
                if (@string != "")
                {
                    Users.DeleteUsers(@string);
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
                    UserGroups.ChangeAllUserGroupId(8, groupid);
                    foreach (DataRow dataRow in Users.GetUserListByGroupid(8).Rows)
                    {
                        CreditsFacade.UpdateUserCredits(Convert.ToInt32(dataRow["uid"].ToString()));
                    }
                    Users.ClearUsersAuthstrByUncheckedUserGroup();
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
                Users.DeleteAuditUser();
                base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
            }
        }

        protected void searchuser_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.DataGrid1.BindData(Users.AuditNewUserClear(this.searchusername.Text, this.regbefore.Text, this.regip.Text));
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