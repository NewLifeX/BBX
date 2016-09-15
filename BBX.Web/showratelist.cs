using System.Collections.Generic;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
	public class showratelist : PageBase
	{
		public int postid = DNTRequest.GetInt("pid", 0);
		public List<RateLog> rateloglist = new List<RateLog>();
		public string[] scorename = Scoresets.GetValidScoreName();
		public string[] scoreunit = Scoresets.GetValidScoreUnit();
		public int ratecount;
		public int pagecount;
		public int pagesize = 10;
		public int pageid = DNTRequest.GetInt("page", 0);
		public string pagenumbers = "";

		protected override void ShowPage()
		{
			if (this.postid > 0)
			{
				this.pagetitle = "帖子ID为" + this.postid + "的评分列表";
				//this.ratecount = Posts.GetPostRateLogCount(this.postid);
				//this.rateloglist = Posts.GetPostRateLogList(this.postid, this.pageid, this.pagesize);
				this.ratecount = RateLog.SearchCount(0, postid);
				this.rateloglist = RateLog.Search(0, postid, (pageid - 1) * pagesize, pagesize);
				this.pagecount = ((this.ratecount % this.pagesize == 0) ? (this.ratecount / this.pagesize) : (this.ratecount / this.pagesize + 1));
				this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
				this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
				this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
				this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, "showratelist.aspx?pid=" + this.postid, 8);
			}
		}

		public string GetExtCreditName(int extCredit)
		{
			return this.scorename[extCredit];
		}

		public string GetExtCreditUnit(int extCredit)
		{
			return this.scoreunit[extCredit];
		}

		public string GetAvatarUrl(int uid)
		{
			return Urls.UserInfoAspxRewrite(uid);
		}

		public string GetScoreMark(int value)
		{
			if (value <= 0)
			{
				return "";
			}
			return "+";
		}
	}
}