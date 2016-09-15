using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class ftpsetting : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList Allowupload;
        protected HtmlGenericControl FtpLayout;
        protected TextBox Serveraddress;
        protected TextBox Username;
        protected TextBox Timeout;
        protected TextBox Serverport;
        protected TextBox Password;
        protected HtmlInputHidden hiddpassword;
        protected TextBox Uploadpath;
        protected TextBox Remoteurl;
        protected RadioButtonList Reservelocalattach;
        protected RadioButtonList Reserveremoteattach;
        protected Button SaveFtpInfo;
        protected Hint Hint1;
        protected RadioButtonList Mode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                string param = this.GetParam(Request["ftptype"]);

                //FTPConfigInfoCollection fTPConfigInfoCollection = (FTPConfigInfoCollection)SerializationHelper.Load(typeof(FTPConfigInfoCollection), base.Server.MapPath("../../config/ftp.config"));
                this.Allowupload.Items[0].Attributes.Add("onclick", "ShowFtpLayout(true)");
                this.Allowupload.Items[1].Attributes.Add("onclick", "ShowFtpLayout(false)");
                foreach (var current in FTPConfigs.Infos)
                {
                    if (current.Name == param)
                    {
                        this.Serveraddress.Text = current.Serveraddress;
                        this.Serverport.Text = current.Serverport.ToString();
                        this.Username.Text = current.Username;
                        this.Password.Text = current.Password;
                        this.hiddpassword.Value = current.Password;
                        this.Mode.SelectedValue = current.Mode.ToString();
                        this.Uploadpath.Text = current.Uploadpath;
                        this.Timeout.Text = current.Timeout.ToString();
                        this.Allowupload.SelectedValue = current.Allowupload.ToString();
                        this.Remoteurl.Text = current.Remoteurl;
                        this.Reservelocalattach.SelectedValue = current.Reservelocalattach.ToString();
                        this.Reserveremoteattach.SelectedValue = current.Reserveremoteattach.ToString();
                        if (current.Allowupload == 0)
                        {
                            this.FtpLayout.Attributes.Add("style", "display:none");
                        }
                        else
                        {
                            this.FtpLayout.Attributes.Add("style", "display:block");
                        }
                    }
                }
                if (this.Serveraddress.Text != "")
                {
                    return;
                }
                this.FtpLayout.Attributes.Add("style", "display:none");
                foreach (var current2 in FTPConfigs.Infos)
                {
                    if (current2.Serveraddress != "")
                    {
                        this.Serveraddress.Text = current2.Serveraddress;
                        this.Serverport.Text = current2.Serverport.ToString();
                        this.Username.Text = current2.Username;
                        this.Password.Text = current2.Password;
                        this.Mode.SelectedValue = current2.Mode.ToString();
                        this.Uploadpath.Text = param.ToLower();
                        this.Timeout.Text = current2.Timeout.ToString();
                        this.Remoteurl.Text = current2.Remoteurl.Replace(current2.Name.ToLower(), param.ToLower());
                        this.Reservelocalattach.SelectedValue = current2.Reservelocalattach.ToString();
                        this.Reserveremoteattach.SelectedValue = current2.Reserveremoteattach.ToString();
                    }
                }
            }
        }

        protected void SaveFtpInfo_Click(object sender, EventArgs e)
        {
            string param = this.GetParam(Request["ftptype"]);
            if (String.IsNullOrEmpty(this.Serveraddress.Text.Trim()) ||
                String.IsNullOrEmpty(this.Serverport.Text.Trim()) ||
                String.IsNullOrEmpty(this.Username.Text.Trim()) ||
                String.IsNullOrEmpty(this.Password.Text.Trim()) || 
                String.IsNullOrEmpty(this.Timeout.Text.Trim()) || 
                String.IsNullOrEmpty(this.Remoteurl.Text.Trim()))
            {
                base.RegisterStartupScript("", "<script>alert('远程附件设置各项不允许为空');window.location.href='global_ftpsetting.aspx?ftptype=" + param + "';</script>");
                return;
            }
            if (this.Uploadpath.Text.EndsWith("/"))
            {
                base.RegisterStartupScript("", "<script>alert('附件保存路径不允许以“/”结尾');window.location.href='global_ftpsetting.aspx?ftptype=" + param + "';</script>");
                return;
            }
            if (this.Remoteurl.Text.EndsWith("/"))
            {
                base.RegisterStartupScript("", "<script>alert('远程访问 URL 不允许以“/”结尾');window.location.href='global_ftpsetting.aspx?ftptype=" + param + "';</script>");
                return;
            }
            if (!Utils.IsNumeric(this.Serverport.Text) || int.Parse(this.Serverport.Text) < 1)
            {
                base.RegisterStartupScript("", "<script>alert('远程访问端口必须为数字并且大于1');window.location.href='global_ftpsetting.aspx?ftptype=" + param + "';</script>");
                return;
            }
            if (!Utils.IsNumeric(this.Timeout.Text) || int.Parse(this.Timeout.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('超时时间必须为数字并且大于1');window.location.href='global_ftpsetting.aspx?ftptype=" + param + "';</script>");
                return;
            }

            //FTPConfigInfoCollection fTPConfigInfoCollection = (FTPConfigInfoCollection)SerializationHelper.Load(typeof(FTPConfigInfoCollection), base.Server.MapPath("../../config/ftp.config"));
            bool flag = false;
            foreach (var current in FTPConfigs.Infos)
            {
                if (current.Name == param)
                {
                    current.Serveraddress = this.Serveraddress.Text;
                    current.Serverport = int.Parse(this.Serverport.Text);
                    current.Username = this.Username.Text;
                    current.Password = this.Password.Text;
                    current.Mode = int.Parse(this.Mode.SelectedValue);
                    current.Uploadpath = this.Uploadpath.Text;
                    current.Timeout = int.Parse(this.Timeout.Text);
                    current.Allowupload = int.Parse(this.Allowupload.SelectedValue);
                    current.Remoteurl = this.Remoteurl.Text;
                    current.Reservelocalattach = int.Parse(this.Reservelocalattach.SelectedValue);
                    current.Reserveremoteattach = int.Parse(this.Reserveremoteattach.SelectedValue);
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                FTPConfigs.Add(new FTPConfigInfo
                {
                    Name = param,
                    Serveraddress = this.Serveraddress.Text,
                    Serverport = int.Parse(this.Serverport.Text),
                    Username = this.Username.Text,
                    Password = this.Password.Text,
                    Mode = int.Parse(this.Mode.SelectedValue),
                    Uploadpath = param.ToLower(),
                    Timeout = int.Parse(this.Timeout.Text),
                    Allowupload = int.Parse(this.Allowupload.SelectedValue),
                    Remoteurl = this.Remoteurl.Text,
                    Reservelocalattach = int.Parse(this.Reservelocalattach.SelectedValue),
                    Reserveremoteattach = int.Parse(this.Reserveremoteattach.SelectedValue)
                });
            }

            //SerializationHelper.Save(fTPConfigInfoCollection, base.Server.MapPath("../../config/ftp.config"));
            FTPConfigs.Save();
            base.Response.Redirect("global_ftpsetting.aspx?ftptype=" + param);
        }

        private string GetParam(string param)
        {
            if (param != null)
            {
                if (param == "forumattach")
                {
                    return "ForumAttach";
                }
                if (param == "spaceattach")
                {
                    return "SpaceAttach";
                }
                if (param == "albumattach")
                {
                    return "AlbumAttach";
                }
            }
            return param;
        }
    }
}