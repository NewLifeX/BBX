using BBX.Common;
using System;
using NewLife;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
	public class usercpsubscribe : UserCpPage
	{
		public int typeid = DNTRequest.GetInt("typeid", 0);
		public FavoriteType type;
		public int favoriteCount;

		protected override void ShowPage()
		{
			this.pagetitle = "用户控制面板";
			if (!base.IsLogin())
			{
				return;
			}
			switch (this.typeid)
			{
				case 3:
					this.type = FavoriteType.Goods;
					break;
				default:
					this.type = FavoriteType.ForumTopic;
					break;
			}
			if (!DNTRequest.IsPost())
			{
				this.favoriteCount = Favorite.SearchCount(this.userid, null, this.type);
				base.BindItems(this.favoriteCount, string.Format("usercpsubscribe.aspx?typeid={0}", this.typeid));
				return;
			}
			if (ForumUtils.IsCrossSitePost())
			{
				base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
				return;
			}
			string formString = DNTRequest.GetFormString("titemid");
			if (formString.IsNullOrEmpty() || !Utils.IsNumericList(formString))
			{
				base.AddErrLine("您未选中任何数据信息，当前操作失败！");
				return;
			}
			//if (Favorites.DeleteFavorites(this.userid, Utils.SplitString(formString, ","), this.type) == -1)
			var list = Favorite.Search(userid, formString.SplitAsInt(","), type);
			if (list.Delete() == 0)
			{
				base.AddErrLine("参数无效");
				return;
			}
			base.SetShowBackLink(false);
			base.SetUrl("usercpsubscribe.aspx");
			base.SetMetaRefresh();
			base.AddMsgLine("删除完毕");
		}

		public string GetForumName(Int32 fid)
		{
			var forumInfo = Forums.GetForumInfo(fid);
			if (forumInfo != null)
			{
				return Utils.RemoveHtml(forumInfo.Name);
			}
			return string.Empty;
		}
	}
}