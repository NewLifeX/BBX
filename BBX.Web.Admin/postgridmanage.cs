using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class postgridmanage : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SetPostInfo;
        public string condition;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.Session["postswhere"] == null)
                {
                    base.Response.Redirect("forum_searchpost.aspx");
                    return;
                }
                this.ViewState["condition"] = this.Session["postswhere"].ToString();
                this.ViewState["fid"] = this.Session["seachpost_fid"].ToString();
                //if (this.Session["posttablename"] != null)
                //{
                //    this.ViewState["posttablename"] = this.Session["posttablename"].ToString();
                //}
                //else
                //{
                //    this.ViewState["posttablename"] = TableList.CurrentTableName;
                //}
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "发帖列表";
            this.DataGrid1.BindData(Post.GetPostListByCondition(this.ViewState["condition"].ToString()));
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            this.BindData();
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[2].Text.ToString().Length > 15)
            {
                e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 15) + "…";
            }
        }

        private void SetPostInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["pid"] != "")
                {
                    string pid = Request["pid"];
                    //string text = this.ViewState["posttablename"].ToString().Trim().Replace(BaseConfigs.GetTablePrefix + "posts", "");
                    //text = ((String.IsNullOrEmpty(text)) ? "1" : text);
                    string[] array = Request["pid"].Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text2 = array[i];
                        int num = int.Parse(text2.Split('|')[0]);
                        int tid = int.Parse(text2.Split('|')[1]);
                        var pi = Post.FindByID(num);
                        if (pi != null)
                        {
                            var forumInfo = Forums.GetForumInfo(pi.Fid);
                            if (forumInfo != null)
                            {
                                //Utils.StrDateDiffHours(postInfo.Postdatetime, this.config.Losslessdel * 24);
                                if (pi.Layer == 0)
                                {
                                    TopicAdmins.DeleteTopics(tid.ToString(), forumInfo.Recyclebin != 0, false);
                                    XForum.SetRealCurrentTopics(forumInfo.ID);
                                }
                                else
                                {
                                    Posts.DeletePost(pi, false, true);
                                    ForumUtils.DeleteTopicCacheFile(tid.ToString());
                                    Topics.UpdateTopicReplyCount(tid);
                                }
                            }
                        }
                    }
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量删帖", "帖子ID:" + pid);
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_searchpost.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_searchpost.aspx';</script>");
            }
        }

        public string Invisible(string invisible)
        {
            if (invisible == "0")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            return "<div align=center><img src=../images/Cancel.gif /></div>";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetPostInfo.Click += new EventHandler(this.SetPostInfo_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.TableHeaderName = "发帖列表";
            this.DataGrid1.ColumnSpan = 8;
            this.DataGrid1.DataKeyField = "ID";
        }
    }
}