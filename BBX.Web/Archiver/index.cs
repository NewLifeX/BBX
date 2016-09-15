using System.Collections.Generic;
using System.Linq;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web.Archiver
{
    public class index : ArchiverPage
    {
        private string FORUM_LINK = "<a href=\"showforum-{0}{1}\">{2}</a>";
        private List<IXForum> archiverForumList;
        private string extName = "";

        public index()
        {
            if (this.config.Aspxrewrite != 1)
            {
                this.FORUM_LINK = "<a href=\"showforum{1}?forumid={0}\">{2}</a>";
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            base.ShowTitle(this.config.Forumtitle + " ");
            base.ShowBody();
            Response.Write("<h1>" + this.config.Forumtitle + "</h1>");
            Response.Write("<div id=\"wrap\">");
            //this.archiverForumList = Forums.GetArchiverForumIndexList(this.config.Hideprivate, this.usergroupinfo.ID);
            // 为了构造的论坛树刚好就是实体树的结构，只需要过滤一下不可见的论坛
            var list = XForum.Root.AllChilds.ToList().Cast<IXForum>().ToList();
            list = list.Where(f => f.Visible)
                .Where(f => config.Hideprivate == 0 || f.AllowView(usergroupinfo.ID))
                .ToList();
            this.archiverForumList = list;
            this.extName = ((this.config.Aspxrewrite == 1) ? this.config.Extname : ".aspx");
            this.WriteSubForumLayer(0);
            Response.Write("</div>");
            Response.Write("<div class=\"fullversion\">查看完整版本: <a href=\"../index.aspx\">" + this.config.Forumtitle + "</a></div>\r\n");
            base.ShowFooter();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void WriteSubForumLayer(int parentFid)
        {
            foreach (var item in this.archiverForumList)
            {
                if (item.ParentID == parentFid)
                {
                    Response.Write((item.Layer == 0) ? "<div class=\"cateitem\"><h2>" : "<div class=\"forumitem\"><h3>");
                    if (item.Layer != 0)
                    {
                        Response.Write(Utils.GetSpacesString(item.Layer));
                    }
                    Response.Write(string.Format(this.FORUM_LINK, item.Fid, this.extName, Utils.HtmlDecode(item.Name)));
                    Response.Write(string.Format("{0}</div>\r\n", (item.Layer == 0) ? "</h2>" : "</h3>"));
                    this.WriteSubForumLayer(item.Fid);
                }
            }
        }
    }
}