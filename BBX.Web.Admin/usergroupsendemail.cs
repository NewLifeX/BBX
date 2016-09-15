using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using XUser = BBX.Entity.User;

namespace BBX.Web.Admin
{
    public class usergroupsendemail : AdminPage
    {
        public string groupidlist = "";
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected BBX.Control.TextBox usernamelist;
        protected BBX.Control.CheckBoxList Usergroups;
        protected BBX.Control.TextBox subject;
        protected TextareaResize body;
        protected BBX.Control.Button BatchSendEmail;
        protected Label lblClientSideCheck;
        protected Label lblCheckedNodes;
        protected Label lblServerSideCheck;
        protected CheckBox selectall;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                EmailConfigInfo config = EmailConfigInfo.Current;
                GeneralConfigInfo config2 = GeneralConfigInfo.Current;
                string text = config.Emailcontent.Replace("{forumtitle}", config2.Forumtitle);
                text = text.Replace("{forumurl}", "<a href=" + config2.Forumurl + ">" + config2.Forumurl + "</a>");
                text = text.Replace("{webtitle}", config2.Webtitle);
                text = text.Replace("{weburl}", "<a href=" + config2.Forumurl + ">" + config2.Weburl + "</a>");
                this.body.Text = text;
            }
            if (Request["flag"] == "1")
            {
                this.ExportUserEmails();
            }
        }

        private void BatchSendEmail_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.groupidlist = this.Usergroups.GetSelectString(",");
                if (String.IsNullOrEmpty(this.groupidlist) && String.IsNullOrEmpty(this.usernamelist.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('您需要输入接收邮件用户名称或选取相关的用户组,因此邮件无法发送');</script>");
                    return;
                }
                int num = 5;
                if (!this.usernamelist.Text.IsNullOrEmpty())
                {
                    //DataTable list = Users.GetEmailListByUserNameList(this.usernamelist.Text);
                    var list = XUser.FindAllByName(usernamelist.Text);
                    if (list.Count <= 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('您输入的接收邮件用户名未能查找到相关用户,因此邮件无法发送');</script>");
                        return;
                    }
                    //Thread[] array = new Thread[list.Count];
                    int num2 = 0;
                    foreach (var item in list)
                    {
                        //EmailMultiThread em = new EmailMultiThread(dataRow["UserName"].ToString(), dataRow["Email"].ToString(), this.subject.Text, this.body.Text);
                        //array[num2] = new Thread(new ThreadStart(em.Send));
                        //array[num2].Start();
                        Emails.SendAsync(item.Name, item.Email, this.subject.Text, this.body.Text);
                        if (num2 >= num)
                        {
                            Thread.Sleep(5000);
                            num2 = 0;
                        }
                        num2++;
                    }
                }
                if (String.IsNullOrEmpty(this.groupidlist))
                {
                    base.LoadRegisterStartupScript("PAGE", "window.location.href='global_usergroupsendemail.aspx';");
                    return;
                }
                base.ClientScript.RegisterStartupScript(base.GetType(), "Page", "<script>submit_Click();</script>");
            }
        }

        private void ExportUserEmails()
        {
            string gid = "";
            if (base.CheckCookie())
            {
                gid = this.Usergroups.GetSelectString(",");
            }
            if (String.IsNullOrEmpty(gid)) return;

            //DataTable emailListByGroupidList = Users.GetEmailListByGroupidList(gid);
            //string text2 = "";
            //if (emailListByGroupidList.Rows.Count > 0)
            //{
            //    for (int i = 0; i < emailListByGroupidList.Rows.Count; i++)
            //    {
            //        text2 = text2 + emailListByGroupidList.Rows[i][1].ToString().Trim() + "; ";
            //    }
            //}
            var sb = new StringBuilder();
            foreach (var item in XUser.GetEmailListByGroupidList(gid.SplitAsInt()))
            {
                if (item.Email.IsNullOrWhiteSpace()) continue;

                if (sb.Length > 0) sb.Append("; ");
                sb.Append(item.Email);
            }
            string file = "Useremail.txt";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = false;
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + base.Server.UrlEncode(file));
            HttpContext.Current.Response.ContentType = "text/plain";
            this.EnableViewState = false;
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.End();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.BatchSendEmail.Click += new EventHandler(this.BatchSendEmail_Click);
            //DataTable userGroupWithOutGuestTitle = UserGroups.GetUserGroupWithOutGuestTitle();
            var list = UserGroup.GetUserGroupExceptGroupid((Int32)UserGroupKinds.游客);
            //var dt = list.ToDataTable(false);
            //foreach (DataRow dataRow in dt.Rows)
            //{
            //    dataRow["grouptitle"] = "<img src=../images/usergroup.GIF border=0  style=\"position:relative;top:2 ;height:18 ;\">" + dataRow["grouptitle"];
            //}
            //this.Usergroups.AddTableData(dt, "grouptitle", "ID");
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