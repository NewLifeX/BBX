using System;
using System.Linq;
using NewLife;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class topicsgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected DropDownTreeList forumid;
        protected HtmlInputCheckBox nodeletepostnum;
        protected BBX.Control.Button SetTopicInfo;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.DropDownList typeid;
        protected CheckBox chkConfirmInsert;
        protected CheckBox chkConfirmUpdate;
        protected CheckBox chkConfirmDelete;
        //public string condition;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.Session["topicswhere"] == null)
                {
                    base.Response.Redirect("forum_seachtopic.aspx");
                    return;
                }
                //this.condition = this.Session["topicswhere"].ToString();
                this.BindData();
            }
            this.forumid.BuildTree(XForum.Root, "Name", "ID");
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            //this.DataGrid1.BindData(Topics.GetTopicsByCondition(this.condition));
            var condition = this.Session["topicswhere"].ToString();
            var list = Topic.FindAll(condition, null, null, 0, 0);
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

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[2].Text.ToString().Length > 15)
            {
                e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 15) + "…";
            }
        }

        private void SetTopicInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["tid"] != "")
                {
                    string tid = Request["tid"];
                    string string2;
                    if ((string2 = Request["operation"]) != null)
                    {
                        if (!(string2 == "moveforum"))
                        {
                            if (!(string2 == "movetype"))
                            {
                                if (!(string2 == "delete"))
                                {
                                    if (!(string2 == "displayorder"))
                                    {
                                        if (!(string2 == "adddigest"))
                                        {
                                            if (string2 == "deleteattach")
                                            {
                                                AdminTopics.BatchDeleteTopicAttachs(tid, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                                            }
                                        }
                                        else
                                        {
                                            AdminTopics.BatchChangeTopicsDigest(tid, Request["digest_level"].ToInt(), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                                        }
                                    }
                                    else
                                    {
                                        AdminTopics.BatchChangeTopicsDisplayOrderLevel(tid, Request["displayorder_level"].ToInt(), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                                    }
                                }
                                else
                                {
                                    AdminTopics.BatchDeleteTopics(tid, !this.nodeletepostnum.Checked, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                                }
                            }
                            else
                            {
                                if (this.typeid.SelectedValue != "0")
                                {
                                    //AdminTopics.SetTypeid(tid, (int)Convert.ToInt16(this.typeid.SelectedValue));
                                    var list = Topic.FindAllByIDs(tid);
                                    list.ForEach(tp => tp.TypeID = typeid.SelectedValue.ToInt());
                                    list.Save();
                                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量修改主题类型", "主题ID:" + tid + " <br />类型tid:" + this.typeid.SelectedValue);
                                }
                            }
                        }
                        else
                        {
                            if (this.forumid.SelectedValue != "0")
                            {
                                AdminTopics.BatchMoveTopics(tid, (int)Convert.ToInt16(this.forumid.SelectedValue), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                            }
                        }
                    }
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_topicsgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请选择相应的主题!');window.location.href='forum_topicsgrid.aspx';</script>");
            }
        }

        public string BoolStr(string closed)
        {
            if (!(closed == "1"))
            {
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            }
            return "<div align=center><img src=../images/OK.gif /></div>";
        }

        public string GetPostLink(string tid, string replies)
        {
            return "<a href=forum_postgrid.aspx?tid=" + tid + ">" + replies + "</a>";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetTopicInfo.Click += new EventHandler(this.SetTopicInfo_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.TableHeaderName = "主题列表";
            this.DataGrid1.ColumnSpan = 11;
            this.DataGrid1.DataKeyField = "ID";
        }
    }
}