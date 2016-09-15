using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class announceprivatemessage : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info2;
        protected pageinfo PageInfo1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveWord;
        protected BBX.Control.Button DelRec;
        protected BBX.Control.TextBox subject;
        protected BBX.Control.TextBox message;
        protected BBX.Control.Button AddNewRec;

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
            this.DataGrid1.TableHeaderName = "公共消息列表";
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.BindData<ShortMessage>(ShortMessage.GetAnnouncePrivateMessageList(0, 0));
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    //PrivateMessages.DeletePrivateMessage(0, Utils.SplitString(Request["id"], ","));
                    ShortMessage.DeletePrivateMessage(0, Request["id"]);
                    //XCache.Remove("/Forum/AnnouncePrivateMessageCount");
                    base.Response.Redirect("global_announceprivatemessage.aspx");
                    return;
                }
                base.RegisterStartupScript("", this.GetMessageScript("您未选中任何选项"));
            }
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.subject.Text))
            {
                base.RegisterStartupScript("", this.GetMessageScript("公共消息标题不能为空"));
                return;
            }
            if (String.IsNullOrEmpty(this.message.Text))
            {
                base.RegisterStartupScript("", this.GetMessageScript("公共消息内容不能为空"));
                return;
            }
            try
            {
                var msg = new ShortMessage
                {
                    Message = this.message.Text,
                    Subject = this.subject.Text,
                    Msgto = "",
                    MsgtoID = 0,
                    Msgfrom = "",
                    MsgfromID = 0
                };
                msg.Create(false);
                this.BindData();
                //XCache.Remove("/Forum/AnnouncePrivateMessageCount");
                base.RegisterStartupScript("PAGE", "window.location.href='global_announceprivatemessage.aspx';");
            }
            catch
            {
                base.RegisterStartupScript("", this.GetMessageScript("无法更新数据库."));
            }
        }

        private string GetMessageScript(string message)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_announceprivatemessage.aspx';</script>", message);
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.ColumnSpan = 5;
        }
    }
}