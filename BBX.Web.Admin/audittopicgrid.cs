using System;
using NewLife;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class audittopicgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button AudioSelectTopic;
        protected BBX.Control.Button AllAudioPass;
        protected BBX.Control.Button DeleteSelectTopic;
        protected BBX.Control.Button AllDelete;
        protected ajaxpostinfo AjaxPostInfo1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.Session["audittopicswhere"] == null)
                {
                    base.Response.Redirect("forum_auditingtopic.aspx");
                    return;
                }
                this.ViewState["condition"] = this.Session["audittopicswhere"].ToString();
                this.BindData();
                if (Post.GetPostListByCondition(this.ViewState["condition"].ToString()).Count > 0)
                {
                    this.AllAudioPass.Enabled = false;
                    this.AllDelete.Enabled = false;
                    this.DeleteSelectTopic.Enabled = false;
                    this.AudioSelectTopic.Enabled = false;
                }
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            //this.DataGrid1.BindData(Topics.GetAuditTopicList(this.ViewState["condition"].ToString()));
            var list = Topic.FindAll(ViewState["condition"] + "", null, null, 0, 0);
            this.DataGrid1.BindData(list);
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void AudioSelectTopic_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var tid = Request["tid"].ToInt();
                if (tid > 0)
                {
                    //TopicAdmins.RestoreTopics(Request["tid"]);
                    var tp = Topic.FindByID(tid);
                    tp.Deleted = false;
                    tp.Save();

                    base.RegisterStartupScript("", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选择任何主题!');window.location.href='forum_auditingtopic.aspx';</script>");
            }
        }

        private void AllAudioPass_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string text = "";
                //DataTable auditTopicList = Topics.GetAuditTopicList(this.ViewState["condition"].ToString());
                //if (auditTopicList.Rows.Count > 0)
                //{
                //    foreach (DataRow dataRow in auditTopicList.Rows)
                //    {
                //        text = text + dataRow["tid"].ToString() + ",";
                //    }
                //    //TopicAdmins.RestoreTopics(text.TrimEnd(','));
                //    var list = Topic.FindAllByIDs(text);
                //    list.ForEach(tp => tp.Deleted = false);
                //    list.Save();
                //}
                var list = Topic.FindAll(ViewState["condition"] + "", null, null, 0, 0);
                list.ForEach(tp => tp.Deleted = false);
                list.Save();
                base.RegisterStartupScript("", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
            }
        }

        private void DeleteSelectTopic_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["tid"] != "")
                {
                    TopicAdmins.DeleteTopics(Request["tid"], false);
                    base.RegisterStartupScript("", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选择任何主题!');window.location.href='forum_auditingtopic.aspx';</script>");
            }
        }

        private void AllDelete_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string text = "";
                //DataTable auditTopicList = Topics.GetAuditTopicList(this.ViewState["condition"].ToString());
                //if (auditTopicList.Rows.Count > 0)
                //{
                //    foreach (DataRow dataRow in auditTopicList.Rows)
                //    {
                //        text = text + dataRow["tid"].ToString() + ",";
                //    }
                //    TopicAdmins.DeleteTopics(text.TrimEnd(','), false);
                //}
                var list = Topic.FindAll(ViewState["condition"] + "", null, null, 0, 0);
                list.Delete();
                base.RegisterStartupScript("", "<script>alert('提交成功');window.location.href='forum_auditingtopic.aspx';</script>");
            }
        }

        public string BoolStr(string closed)
        {
            if (closed == "1")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            return "<div align=center><img src=../images/Cancel.gif /></div>";
        }

        public string GetPostLink(string tid, string replies)
        {
            return "<a href=forum/forum_postgrid.aspx?tid=" + tid + ">" + replies + "</a>";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AudioSelectTopic.Click += new EventHandler(this.AudioSelectTopic_Click);
            this.AllAudioPass.Click += new EventHandler(this.AllAudioPass_Click);
            this.DeleteSelectTopic.Click += new EventHandler(this.DeleteSelectTopic_Click);
            this.AllDelete.Click += new EventHandler(this.AllDelete_Click);
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.TableHeaderName = "主题列表";
            this.DataGrid1.ColumnSpan = 11;
        }
    }
}