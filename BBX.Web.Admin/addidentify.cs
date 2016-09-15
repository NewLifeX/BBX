using System;
using System.IO;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
	public class addidentify : AdminPage
	{
		protected HtmlForm Form1;
		protected TextBox name;
		protected UpFile uploadfile;
		protected UpFile uploadfilesmall;
		protected Hint Hint1;
		protected Button AddIdentifyInfo;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.uploadfile.UpFilePath = (this.uploadfilesmall.UpFilePath = base.Server.MapPath("../../images/identify/"));
			}
		}

		private void AddIdentifyInfo_Click(object sender, EventArgs e)
		{
			if (base.CheckCookie())
			{
				if (String.IsNullOrEmpty(this.uploadfile.FileName.Trim()) || String.IsNullOrEmpty(this.uploadfilesmall.FileName.Trim()))
				{
					base.RegisterStartupScript("PAGE", "alert('没有选择鉴定图片');window.location.href='forum_addidentify.aspx';");
					return;
				}
				string text = this.uploadfile.UpdateFile();
				string text2 = this.uploadfilesmall.UpdateFile();
				if (String.IsNullOrEmpty(text.Trim()))
				{
					base.RegisterStartupScript("PAGE", "alert('没有选择鉴定大图片');window.location.href='forum_addidentify.aspx';");
					return;
				}
				if (String.IsNullOrEmpty(text2.Trim()))
				{
					base.RegisterStartupScript("PAGE", "alert('没有选择鉴定小图片');window.location.href='forum_addidentify.aspx';");
					return;
				}
				if (TopicIdentify.Add(this.name.Text, text))
				{
					string[] array = text.Split('.');
					string str = string.Format("{0}_small.{1}", array[0], array[1]);
					Directory.Move(base.Server.MapPath("../../images/identify/") + text2, base.Server.MapPath("../../images/identify/") + str);
					XCache.Remove("/Forum/TopicIdentifys");
					XCache.Remove("/Forum/TopicIndentifysJsArray");
					AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件添加", this.name.Text);
					base.RegisterStartupScript("PAGE", "window.location.href='forum_identifymanage.aspx';");
					return;
				}
				base.RegisterStartupScript("PAGE", "alert('插入失败，可能名称与原有的相同');window.location.href='forum_identifymanage.aspx';");
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.AddIdentifyInfo.Click += new EventHandler(this.AddIdentifyInfo_Click);
		}
	}
}