using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class auditpost : AdminPage
    {
        public int pageid = DNTRequest.GetInt("pageid", 1);
        public int pageCount;
        public int postCount;
        public List<Post> auditPostList = new List<Post>();
        //public int postTableId = DNTRequest.GetInt("table", Posts.GetMaxPostTableId());
        protected HtmlForm Form1;
        //protected BBX.Control.DropDownList postlist;
        protected BBX.Control.Button SelectPass;
        protected BBX.Control.Button SelectDelete;
        protected ajaxpostinfo AjaxPostInfo1;
        protected Literal msg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                //if (TableList.GetPostsCount(Int32.Parse(this.postlist.SelectedValue)) == 0)
                if (Post.Meta.Count == 0)
                {
                    this.msg.Visible = true;
                }
                //this.postlist.SelectedValue = this.postTableId.ToString();
                this.BindData();
            }
        }

        public void BindData()
        {
            this.postCount = Posts.GetUnauditNewPostCount("0", 1);
            this.pageCount = (this.postCount - 1) / 20 + 1;
            this.pageid = ((this.pageid > this.pageCount) ? this.pageCount : this.pageid);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.auditPostList = Post.GetUnauditPost(null, 1, this.pageid, 20);
        }

        private void postslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        //public void initPostTable()
        //{
        //    this.postlist.AutoPostBack = true;
        //    //DataTable allPostTableName = Posts.GetAllPostTableName();
        //    this.postlist.Items.Clear();
        //    foreach (var item in TableList.GetAllPostTable())
        //    {
        //        this.postlist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + item.ID, item.ID.ToString()));
        //    }
        //    this.postlist.DataBind();
        //    this.postlist.SelectedValue = TableList.GetPostTableId();
        //}

        private void SelectPass_Click(object sender, EventArgs e)
        {
            string @string = Request["pid"];
            string text = "";
            string text2 = "";
            string[] array = @string.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text3 = array[i];
                string[] array2 = text3.Split('|');
                text = text + array2[0] + ",";
                text2 = text2 + array2[1] + ",";
            }
            text = text.TrimEnd(',');
            text2 = text2.TrimEnd(',');
            if (base.CheckCookie())
            {
                if (text != "")
                {
                    this.UpdateUserCredits(text2, text);
                    //Posts.PassPost(int.Parse(this.postlist.SelectedValue), text);
                    Post.Pass(text);
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_auditpost.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_auditpost.aspx';</script>");
            }
        }

        private void UpdateUserCredits(string tidlist, string pidlist)
        {
            string[] array = tidlist.Split(',');
            string[] array2 = pidlist.Split(',');
            for (int i = 0; i < array2.Length; i++)
            {
                var postInfo = Post.FindByID(array2[i].ToInt());
                CreditsFacade.PostReply(postInfo.PosterID, Forums.GetForumInfo(postInfo.Fid).Replyperm);
            }
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["pid"] != "")
                {
                    GetPostLayer();
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_auditpost.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_auditpost.aspx';</script>");
            }
        }

        public void GetPostLayer()
        {
            //DataTable dataTable = new DataTable();
            string[] array = DNTRequest.GetString("pid").Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                string text2 = text.Split('|')[0];
                if (!text2.IsNullOrEmpty())
                {
                    //dataTable = BBX.Data.Posts.GetPostLayer(currentPostTableId, int.Parse(text2));
                    //if (dataTable.Rows.Count > 0)
                    var pi = Post.FindByID(text2.ToInt());
                    if (pi != null)
                    {
                        if (pi.Layer == 0)
                            TopicAdmins.DeleteTopics(pi.Tid + "", false);
                        else
                            Posts.DeletePost(pi, false, false);
                    }
                }
            }
        }

        protected string GetPostStatus(string invisible)
        {
            if (invisible == "1")
            {
                return "未审核";
            }
            if (invisible == "-3")
            {
                return "忽略";
            }
            return invisible;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            //this.postlist.SelectedIndexChanged += new EventHandler(this.postslist_SelectedIndexChanged);
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
            //this.initPostTable();
        }

        public string ShowPageIndex()
        {
            string text = "";
            int num = (this.pageid - 5 > 0) ? (this.pageid - 5 - ((this.pageid + 5 < this.pageCount) ? 0 : (this.pageid + 5 - this.pageCount))) : 1;
            int num2 = (this.pageid + 5 < this.pageCount) ? (this.pageid + 5 + ((this.pageid - 5 > 0) ? 0 : ((this.pageid - 5) * -1 + 1))) : this.pageCount;
            for (int i = num; i <= num2; i++)
            {
                if (i != this.pageid)
                {
                    text += string.Format("<a style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;\" href=\"forum_auditpost.aspx?pageid={0}\">{0}</a>", i);
                }
                else
                {
                    text += string.Format("<span style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;background:#09C;color:#FFF\" >{0}</span> ", i);
                }
            }
            return text;
        }
    }
}