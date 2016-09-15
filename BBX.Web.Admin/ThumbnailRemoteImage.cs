using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class ThumbnailRemoteImage : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox imageUrl;
        protected TextBox maxWidth;
        protected TextBox maxHeight;
        protected HtmlTableCell reImgUrl;
        protected Hint Hint1;
        protected Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.AllowShowNavigation = false;
            if (!this.Page.IsPostBack)
            {
                int num = Request["w"].ToInt();
                int num2 = Request["h"].ToInt();
                if (num > 0)
                {
                    this.maxWidth.Text = num.ToString();
                }
                if (num2 > 0)
                {
                    this.maxHeight.Text = num2.ToString();
                }
            }
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int num = this.maxWidth.Text.ToInt();
                int num2 = this.maxHeight.Text.ToInt();
                if (num <= 0 || num2 <= 0 || !this.IsImage(this.imageUrl.Text.Trim()))
                {
                    this.reImgUrl.InnerHtml = "<font color='red'>请填入正确的图片地址和宽度高度</font>";
                    return;
                }
                string remoteThumbnail = this.GetRemoteThumbnail(this.imageUrl.Text.Trim(), num, num2);
                if (remoteThumbnail == string.Empty)
                {
                    this.reImgUrl.InnerHtml = "<font color='red'>生成缩略图失败，请确认图片地址正确后重试</font>";
                }
                this.reImgUrl.InnerHtml = "<img src='" + remoteThumbnail + "' /><br /><br />生成缩略图成功，地址如下:<br /><br /><textarea cols='50' rows='2'>" + remoteThumbnail + "</textarea>";
            }
        }

        public string GetRemoteThumbnail(string url, int maxWidth, int maxHeight)
        {
            string text = Utils.CutString(url, url.LastIndexOf("/") + 1).ToLower();
            string text2 = Utils.CutString(text, text.LastIndexOf(".") + 1).ToLower();
            if (text2 == string.Empty || !Utils.InArray(text2, "jpg,jpeg,gif"))
            {
                text2 = "jpg";
            }
            string text3 = string.Format("{0}_{1}_{2}.{3}", new object[]
			{
				Utils.MD5(url.ToLower()),
				maxWidth,
				maxHeight,
				text2
			});
            string text4 = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "upload/temp/");
            string text5 = text4 + text3;
            string text6 = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "cache/thumbnail/");
            if (!Directory.Exists(text4))
            {
                Utils.CreateDir(text4);
            }
            if (!Directory.Exists(text6))
            {
                Utils.CreateDir(text6);
            }
            string newFileName = text6 + text3;
            try
            {
                this.DownloadImage(url, text5);
                Thumbnail.MakeThumbnailImage(text5, newFileName, maxWidth, maxHeight);
                this.DeleteImage(text5);
            }
            catch
            {
                return string.Empty;
            }
            return Utils.GetRootUrl(BaseConfigs.GetForumPath) + "cache/thumbnail/" + text3;
        }

        public void DownloadImage(string remotePath, string filePath)
        {
            WebClient webClient = new WebClient();
            try
            {
                webClient.DownloadFile(remotePath, filePath);
            }
            finally
            {
                webClient.Dispose();
            }
        }

        public void DeleteImage(string fileName)
        {
            File.Delete(fileName);
        }

        public bool IsImage(string remotePath)
        {
            if (!Uri.IsWellFormedUriString(remotePath, UriKind.RelativeOrAbsolute))
            {
                return false;
            }
            WebRequest webRequest = WebRequest.Create(remotePath);
            WebResponse response = webRequest.GetResponse();
            if (response.ContentType.IndexOf("image/") > -1)
            {
                response.Close();
                return true;
            }
            response.Close();
            return false;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }
    }
}