using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class advsgrid : AdminPage
    {
        public int type = DNTRequest.GetInt("type", -1);

        public string[] advtypes = new string[]
        {
            "头部横幅广告",
            "尾部横幅广告",
            "页内文字广告",
            "帖内广告",
            "浮动广告",
            "对联广告",
            "Silverlight媒体广告",
            "帖间通栏广告",
            "分类间广告",
            "快速发帖栏上方广告",
            "快速编辑器背景广告",
            "聚合首页头部广告",
            "聚合首页热贴下方广告",
            "聚合首页发帖排行上方广告",
            "聚合首页推荐版块上方广告",
            "聚合首页推荐版块下方广告",
            "聚合首页推荐相册下方广告",
            "聚合首页底部广告",
            "页内横幅广告"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.InitAdvertisementAvailable();
                this.BindData(this.type);
            }
        }

        private void InitAdvertisementAvailable()
        {
            Advertisement.SetAvailable(type, 0);
        }

        public void BindData(int type)
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.BindData(Advertisement.FindAllByType(type).ToDataTable(false));
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelAds_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                String ads = Request["advid"];
                if (ads != "")
                {
                    //Advertisements.DeleteAdvertisementList(Request["advid"]);
                    Advertisement.DeleteAll(ads);

                    XCache.Remove(CacheKeys.FORUM_ADVERTISEMENTS);
                    base.RegisterStartupScript("PAGE", "window.location.href='advsgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='advsgrid.aspx';</script>");
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

        public string ParameterType(string parameters)
        {
            return parameters.Split('|')[0];
        }

        public string TargetsType(string targets)
        {
            string text = "";
            if (targets.IndexOf("全部") >= 0)
            {
                return "全部";
            }
            if (targets.IndexOf("首页") >= 0)
            {
                text = "首页,";
                targets = targets.Replace("首页,", "");
            }
            if (targets.Trim() != "首页")
            {
                foreach (IXForum current in Forums.GetForumList(targets))
                {
                    text = text + current.Name + ",";
                }
            }
            if (text.Length <= 0)
            {
                return "";
            }
            return text.Substring(0, text.Length - 1);
        }

        private void SetUnAvailable_Click(object sender, EventArgs e)
        {
            this.UpdateAdvertisementAvailable(0);
        }

        private void SetAvailable_Click(object sender, EventArgs e)
        {
            this.UpdateAdvertisementAvailable(1);
        }

        private void UpdateAdvertisementAvailable(int available)
        {
            if (base.CheckCookie())
            {
                if (Request["advid"] != "")
                {
                    //Advertisements.UpdateAdvertisementAvailable(Request["advid"], available);
                    Advertisement.SetAvailable(Request["advid"], available);
                    XCache.Remove(CacheKeys.FORUM_ADVERTISEMENTS);
                    base.RegisterStartupScript("PAGE", "window.location.href='advsgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='advsgrid.aspx';</script>");
            }
        }

        public string GetAdType(string adtype)
        {
            return this.advtypes[Utility.ToInt(adtype, 0)];
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetAvailable.Click += new EventHandler(this.SetAvailable_Click);
            this.SetUnAvailable.Click += new EventHandler(this.SetUnAvailable_Click);
            this.DelAds.Click += new EventHandler(this.DelAds_Click);
            this.DataGrid1.TableHeaderName = "广告列表";
            this.DataGrid1.DataKeyField = "id";
            this.DataGrid1.ColumnSpan = 12;
        }
    }
}