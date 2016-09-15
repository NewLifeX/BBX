using System;
using System.Web.UI.HtmlControls;
using BBX.Entity;
using NewLife;

namespace BBX.Web.Admin
{
	public class delforums : AdminPage
	{
		protected HtmlForm Form1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				//var mpp = MallPluginProvider.GetInstance();
				//if (Request["istrade"] == "1" && mpp != null)
				//{
				//	mpp.EmptyGoodsCategoryFid(DNTRequest.GetInt("fid", 0));
				//	mpp.StaticWriteJsonFile();
				//	XCache.Remove("/Mall/MallSetting/GoodsCategories");
				//}
				//if (Forums.DeleteForum(Request["fid"]))
				var f = XForum.FindByID(Request["fid"].ToInt());
				if (f != null)
				{
					f.Delete();

					ForumOperator.RefreshForumCache();
					AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除论坛版块", "删除论坛版块,fid为:" + Request["fid"]);
					base.RegisterStartupScript("", "<script>window.location.href='forum_ForumsTree.aspx';</script>");
					return;
				}
				base.RegisterStartupScript("", "<script>alert('对不起,当前节点下面还有子结点,因此不能删除！');window.location.href='forum_ForumsTree.aspx';</script>");
			}
		}
	}
}