using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class templatesgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
        protected DataGrid DataGrid1;
        protected Button IntoDB;
        protected Button DelRec;
        protected Button DelTemplates;
        public string path;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.path = BaseConfigs.GetForumPath.CombinePath("templates").GetFullPath();

            if (!this.Page.IsPostBack)
            {
                this.LoadTemplateData();
                //这里千万不能用 Request["createtemplate"] != "" 
                if (!string.IsNullOrEmpty(Request["createtemplate"]))
                {
                    this.CreateTemplateByDirectory(Request["createtemplate"]);
                    base.RegisterStartupScript("PAGE", "window.location.href='global_templatesgrid.aspx';");
                }
            }
        }

        public void LoadTemplateData()
        {
            string msg = "由于目录 : ";
            var ids = new List<Int32>();
            var list = Template.GetValids();
            foreach (var tmp in list)
            {
                var dir = tmp.Directory;
                if (!dir.IsNullOrEmpty() && !dir.EqualIgnoreCase("default") && !Directory.Exists(path.CombinePath(dir)))
                {
                    msg = msg + dir + " ,";
                    ids.Add(tmp.ID);
                }
            }
            if (ids.Count > 0)
            {
                base.RegisterStartupScript("", "<script>alert('" + msg.Substring(0, msg.Length - 1) + "已被删除, 因此系统将自动更新模板列表!')</script>");
                //AdminTemplates.DeleteTemplateItem(ids);
                Template.Delete(ids);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "从数据库中删除模板文件", "ID为:" + ids.Join());
                XCache.Remove(CacheKeys.FORUM_TEMPLATE_ID_LIST);
                //Templates.GetValidTemplateIDList();
            }
            else
            {
                try
                {
                    var tmps = Template.Import(path);
                    foreach (var item in tmps)
                    {
                        AdminVisitLog.InsertLog(userid, username, usergroupid, grouptitle, ip, "模板文件入库", item.Name);
                        this.CreateTemplateByDirectory(item.Directory);
                    }
                }
                catch (Exception ex)
                {
                    base.RegisterStartupScript("", "<script>alert('" + ex.Message + "');window.location.href='global_templatesgrid.aspx';</script>");
                }
            }
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.DataSource = Template.FindAllWithCache();
            this.DataGrid1.DataBind();
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (!base.CheckCookie()) return;

            var tids = Request["templateid"].SplitAsInt();
            if (tids.Length == 0)
            {
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项'); window.location.href='global_templatesgrid.aspx';</script>");
            }

            if (tids.Contains(1))
            {
                base.RegisterStartupScript("", "<script>alert('选中的模板中含有系统初始化模板,此次提交无法执行');window.location.href='global_templatesgrid.aspx'</script>");
                return;
            }

            for (int i = 0; i < tids.Length; i++)
            {
                try
                {
                    var tmp = Template.FindByID(tids[i]);
                    tmp.Enable = false;
                    tmp.Save();
                }
                catch (Exception ex)
                {
                    base.RegisterStartupScript("", "<script>alert('" + ex.Message + "');window.location.href='global_templatesgrid.aspx';</script>");
                }
            }
            base.Response.Redirect("global_templatesgrid.aspx");
            return;
        }

        private void IntoDB_Click(object sender, EventArgs e)
        {
            if (!base.CheckCookie()) return;

            var tids = Request["templateid"].SplitAsInt();
            if (tids.Length == 0)
            {
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项'); window.location.href='global_templatesgrid.aspx';</script>");
            }
            //if (tids.Contains(1))
            //{
            //    base.RegisterStartupScript("", "<script>alert('选中的模板中含有系统初始化模板,此次提交无法执行');window.location.href='global_templatesgrid.aspx'</script>");
            //    return;
            //}
            var list = Template.GetValids();
            // 这里可以遍历templates目录来入库模版，AboutInfo可加载基本信息
            //throw new NotImplementedException("未实现模版入库！");
            for (int i = 0; i < tids.Length; i++)
            {
                try
                {
                    var tmp = Template.FindByID(tids[i]);
                    tmp.Import(null, tmp.Directory);
                    tmp.Enable = true;
                    tmp.Save();
                }
                catch (Exception ex)
                {
                    base.RegisterStartupScript("", "<script>alert('" + ex.Message + "');window.location.href='global_templatesgrid.aspx';</script>");
                }
            }

            XCache.Remove("/Forum/TemplateList");
            XCache.Remove(CacheKeys.FORUM_TEMPLATE_ID_LIST);
            XCache.Remove(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX);
            XCache.Remove(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS);
            base.RegisterStartupScript("PAGE", "window.location.href='global_templatesgrid.aspx';");
        }

        protected void DelTemplates_Click(object sender, EventArgs e)
        {
            if (!base.CheckCookie()) return;

            var tids = Request["templateid"].SplitAsInt();
            if (tids.Length == 0)
            {
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项'); window.location.href='global_templatesgrid.aspx';</script>");
            }
            if (tids.Contains(1))
            {
                base.RegisterStartupScript("", "<script>alert('您选中的数据项中含有系统初始化模板,此次提交无法执行');window.location.href='global_templatesgrid.aspx'</script>");
                return;
            }
            for (int i = 0; i < tids.Length; i++)
            {
                try
                {
                    var tmp = Template.FindByID(tids[i]);
                    tmp.Delete();
                }
                catch (Exception ex)
                {
                    base.RegisterStartupScript("", "<script>alert('" + ex.Message + "');window.location.href='global_templatesgrid.aspx';</script>");
                }
            }
            base.Response.Redirect("global_templatesgrid.aspx");
            return;
        }

        public string DirectoryStr(string path)
        {
            if (path.ToLower().IndexOf("http") >= 0)
            {
                return "<a href=" + path + ">点击浏览</a>";
            }
            if (path.ToLower().IndexOf("templates/default") >= 0)
            {
                return "<a href=../../" + path + ".htm >点击浏览</a>";
            }
            return "<a href=../../templates/default/" + path + ".htm >点击浏览</a>";
        }

        private void CreateTemplateByDirectory(string directorypath)
        {
            if (base.CheckCookie())
            {
                ForumPageTemplate.BuildTemplate(directorypath);
            }
        }

        public string CreateStr(string name, string path, string templateid)
        {
            string str = "<a href=global_templatetree.aspx?path=" + path.Trim().Replace(" ", "%20") + "&templateid=" + templateid.Trim() + "&templatename=" + name.Trim().Replace(" ", "%20") + ">管理</a>&nbsp;&nbsp;";
            return str + "<a href=\"javascript:CreateTemplate('" + path + "')\">生成</a>";
        }

        public string Valid(string valid)
        {
            if (valid == "1")
                return "<div align=center><img src=../images/state2.gif /></div>";
            else
                return "<div align=center><img src=../images/state3.gif /></div>";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.IntoDB.Click += new EventHandler(this.IntoDB_Click);
            this.DataGrid1.SaveDSViewState = true;
            this.DataGrid1.TableHeaderName = "模板列表";
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.AllowPaging = false;
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.ShowFooter = false;
        }
    }
}