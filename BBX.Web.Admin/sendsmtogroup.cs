using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class sendsmtogroup : AdminPage
    {
        public string groupidlist = "";
        protected HtmlForm Form1;
        protected BBX.Control.CheckBoxList Usergroups;
        protected BBX.Control.TextBox subject;
        protected BBX.Control.TextBox msgfrom;
        protected BBX.Control.DropDownList folder;
        protected BBX.Control.TextBox postdatetime;
        protected BBX.Control.TextBox postcountpercircle;
        protected TextareaResize message;
        protected BBX.Control.Button BatchSendSM;
        protected Label lblClientSideCheck;
        protected Label lblCheckedNodes;
        protected Label lblServerSideCheck;
        protected CheckBox selectall;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack && this.username != "")
            {
                this.msgfrom.Text = this.username;
                this.postdatetime.Text = DateTime.Now.ToShortDateString();
            }
        }

        private void BatchSendSM_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.groupidlist = this.Usergroups.GetSelectString(",");
                if (String.IsNullOrEmpty(this.groupidlist))
                {
                    base.RegisterStartupScript("", "<script>alert('请您先选取相关的用户组,再点击提交按钮');</script>");
                    return;
                }
                base.ClientScript.RegisterStartupScript(base.GetType(), "Page", "<script>submit_Click();</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.BatchSendSM.Click += new EventHandler(this.BatchSendSM_Click);
            this.message.is_replace = true;
            //DataTable userGroupWithOutGuestTitle = UserGroups.GetUserGroupWithOutGuestTitle();
            var list = UserGroup.GetUserGroupExceptGroupid((Int32)UserGroupKinds.游客);
            //var userGroupWithOutGuestTitle = list.ToDataTable(false);
            //foreach (DataRow dataRow in userGroupWithOutGuestTitle.Rows)
            //{
            //    dataRow["grouptitle"] = "<img src=../images/usergroup.gif border=0  style=\"position:relative;top:2 ;height:18 \">" + dataRow["grouptitle"];
            //}
            //this.Usergroups.AddTableData(userGroupWithOutGuestTitle);
            // 先克隆一份，避免修改了原来的数据
            list = list.Clone();
            foreach (var item in list)
            {
                item["GroupTitle2"] = "<img src=../images/usergroup.GIF border=0  style=\"position:relative;top:2 ;height:18 ;\">" + item.GroupTitle;
            }
            this.Usergroups.AddTableData(list, "GroupTitle2", "ID");
        }
    }
}