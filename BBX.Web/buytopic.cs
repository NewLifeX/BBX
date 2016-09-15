using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;
using XCode;

namespace BBX.Web
{
	public class buytopic : TopicPage
	{
        public EntityList<Post> lastpostlist;
		public EntityList<PaymentLog> paymentloglist;
		public UserExtcreditsInfo userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
		public int buyers;
		public int showpayments = DNTRequest.GetInt("showpayments", 0);
		public int buyit = DNTRequest.GetInt("buyit", 0);
		public int topicprice;
		public float netamount;
		public int maxincpertopic = Scoresets.GetMaxIncPerTopic();
		public int maxchargespan = Scoresets.GetMaxChargeSpan();
		public float creditstax = Scoresets.GetCreditsTax() * 100f;
		public int price;
		public float userlastprice;
		public int expirehours;
		public Post postinfo;
		public string postmessage = "";
		public static Regex r = new Regex("\\s*\\[free\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/free\\]\\s*", RegexOptions.IgnoreCase);
		private int pageSize = GeneralConfigInfo.Current.Tpp;

		protected override void OnInit(EventArgs e)
		{
			if (!this.SetTopicInfo())
			{
				this.topic = new Topic();
				this.forum = new XForum();
				//return;
			}
			base.OnInit(e);
		}

		protected override void ShowPage()
		{
			if (topic.ID == 0) return;

			this.pagetitle = this.topic.Title.Trim();
			if (!String.IsNullOrEmpty(forum.Password) && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + this.forum.Fid + "password"))
			{
				base.AddErrLine("本版块被管理员设置了密码");
				base.Response.Redirect(string.Format("{0}showforum-{1}{2}", BaseConfigs.GetForumPath, this.forum.Fid, this.config.Extname), true);
				return;
			}
			if (!UserAuthority.VisitAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
			{
				base.AddErrLine(this.msg);
				return;
			}
			this.postinfo = Post.FindByTid(this.topicid);
            var msg = postinfo.Message;
			if (msg.ToLower().Contains("[free]") || msg.ToLower().Contains("[/free]"))
			{
				Match match = buytopic.r.Match(msg);
				while (match.Success)
				{
					object obj = this.postmessage;
					this.postmessage = obj + "<br /><div class=\"msgheader\">免费内容:</div><div class=\"msgborder\">" + match.Groups[1] + "</div><br />";
					match = match.NextMatch();
				}
			}
			this.topicprice = this.topic.Price;
			if (this.topic.Price > 0)
			{
				this.price = this.topic.Price;
				this.expirehours = (Int32)(DateTime.Now - topic.PostDateTime.AddHours(maxchargespan)).TotalHours;
				if (PaymentLog.IsBuyer(this.topicid, this.userid) || (this.expirehours > 0 && this.maxchargespan != 0))
				{
					this.price = -1;
				}
				else
				{
					this.expirehours = Math.Abs(this.expirehours);
				}
			}
			this.netamount = (float)this.topicprice - (float)this.topicprice * this.creditstax / 100f;
			if (this.topicprice > this.maxincpertopic)
			{
				this.netamount = (float)this.maxincpertopic - (float)this.maxincpertopic * this.creditstax / 100f;
			}
			if (this.price != -1)
			{
				var userInfo = Users.GetUserInfo(this.userid);
				if (this.buyit == 1 && !this.CheckUserExtCredit(userInfo))
				{
					return;
				}
				this.userlastprice = Users.GetUserExtCredit(userInfo, Scoresets.GetTopicAttachCreditsTrans()) - (float)this.topic.Price;
			}
			if (!this.ispost)
			{
				this.buyers = PaymentLog.GetPaymentLogByTidCount(this.topic.ID);
				if (this.showpayments == 1)
				{
					this.pagecount = ((this.buyers % this.pageSize == 0) ? (this.buyers / this.pageSize) : (this.buyers / this.pageSize + 1));
					this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
					this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
					this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
					this.paymentloglist = PaymentLog.GetPaymentLogByTid(this.pageSize, this.pageid, this.topic.ID);
				}
				int hide = (this.topic.Hide == 1) ? this.topic.Hide : 0;
				if (Post.IsReplier(this.topicid, this.userid))
				{
					hide = -1;
				}
				this.lastpostlist = Posts.GetPagedLastPost(this.GetPostPramsInfo(hide));
				return;
			}
			int num = PaymentLog.BuyTopic(this.userid, this.topic.ID, this.topic.PosterID, this.topic.Price, this.netamount);
			if (num > 0)
			{
				base.SetUrl(base.ShowTopicAspxRewrite(this.topic.ID, 0));
				base.SetMetaRefresh();
				base.SetShowBackLink(false);
				base.MsgForward("buytopic_succeed");
				base.AddMsgLine("购买主题成功,返回该主题");
				return;
			}
			base.SetBackLink(base.ShowForumAspxRewrite(this.topic.Fid, 0));
			if (num == -1)
			{
				base.AddErrLine("对不起,您的账户余额少于交易额,无法进行交易");
				return;
			}
			if (num == -2)
			{
				base.AddErrLine("您无权购买本主题");
				return;
			}
			base.AddErrLine("未知原因,交易无法进行,给您带来的不方便我们很抱歉");
		}

		private bool SetTopicInfo()
		{
			if (this.userid < 0)
			{
				base.AddErrLine("您还没有登录，请登录后再操作");
				this.needlogin = true;
				return false;
			}
			if (this.topicid == -1)
			{
				base.AddErrLine("无效的主题ID");
				return false;
			}
			//this.topic = Topics.GetTopicInfo(this.topicid);
			this.topic = Topic.FindByID(this.topicid);
			if (this.topic == null)
			{
				base.AddErrLine("不存在的主题ID");
				return false;
			}
			if (this.topic.DisplayOrder == -1 || this.topic.DisplayOrder == -2)
			{
				base.AddErrLine("此主题已被删除或未经审核！");
				return false;
			}
			this.forum = Forums.GetForumInfo(this.topic.Fid);
			if (this.forum == null)
			{
				base.AddErrLine("主题对应版块不存在！");
				return false;
			}
			if (((this.topic.PosterID == this.userid || this.ismoder == 1) && String.IsNullOrEmpty(DNTRequest.GetString("showpayments"))) || this.topic.Price <= 0)
			{
				HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + base.ShowTopicAspxRewrite(this.topic.ID, 0));
				return false;
			}
			if (this.topic.ReadPerm > this.usergroupinfo.Readaccess && this.topic.PosterID != this.userid && this.useradminid != 1 && this.ismoder != 1)
			{
				base.AddErrLine(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", this.topic.ReadPerm.ToString(), this.usergroupinfo.GroupTitle));
				return false;
			}
			return true;
		}

		private PostpramsInfo GetPostPramsInfo(int hide)
		{
			PostpramsInfo postpramsInfo = new PostpramsInfo();
			postpramsInfo.Fid = this.forum.Fid;
			postpramsInfo.Tid = this.topicid;
			postpramsInfo.Jammer = this.forum.Jammer;
			postpramsInfo.Pagesize = 5;
			postpramsInfo.Pageindex = 1;
			postpramsInfo.Getattachperm = this.forum.Getattachperm;
			postpramsInfo.Usergroupid = this.usergroupid;
			postpramsInfo.Attachimgpost = this.config.Attachimgpost;
			postpramsInfo.Showattachmentpath = this.config.Showattachmentpath;
			postpramsInfo.Hide = hide;
			postpramsInfo.Price = this.price;
			postpramsInfo.Ubbmode = false;
			postpramsInfo.Showimages = this.forum.Allowimgcode;
			postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
			postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
			postpramsInfo.Smiliesmax = this.config.Smiliesmax;
			postpramsInfo.Bbcodemode = this.config.Bbcodemode;
			postpramsInfo.CurrentUserid = this.userid;
			User userInfo = Users.GetUserInfo(postpramsInfo.CurrentUserid);
			postpramsInfo.Usercredits = ((userInfo == null) ? 0 : userInfo.Credits);
			return postpramsInfo;
		}

		public bool CheckUserExtCredit(User userInfo)
		{
			if (userInfo == null)
			{
				base.AddErrLine("您无权购买本主题");
				this.needlogin = true;
				return false;
			}
			if (Users.GetUserExtCredit(userInfo, Scoresets.GetTopicAttachCreditsTrans()) < (float)this.topic.Price)
			{
				string text = "";
				if (EPayments.IsOpenEPayments())
				{
					text = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
				}
				base.AddErrLine(string.Format("对不起,您的账户余额 <span class=\"bold\">{0} {1}{2}</span> 交易额为 {3}{2} ,无法进行交易.{4}", new object[]
                {
                    Scoresets.GetValidScoreName()[Scoresets.GetTopicAttachCreditsTrans()],
                    Users.GetUserExtCredit(userInfo, Scoresets.GetTopicAttachCreditsTrans()),
                    Scoresets.GetValidScoreUnit()[Scoresets.GetTopicAttachCreditsTrans()],
                    this.topic.Price,
                    text
                }));
				return false;
			}
			return true;
		}
	}
}