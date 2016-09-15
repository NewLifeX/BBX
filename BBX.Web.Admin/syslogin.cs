using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class syslogin : Page
    {
        protected HtmlForm Form1;
        protected Literal Msg;
        protected BBX.Control.TextBox UserName;
        protected BBX.Control.TextBox PassWord;
        public int olid;
        protected internal GeneralConfigInfo config;
        public string footer = "";

        public syslogin()
        {
            footer = "<div align=\"center\" style=\" padding-top:60px;font-size:11px; font-family: Arial\">";
            footer += "<hr style=\"height:1; width:600; height:1; color:#CCCCCC\" />Powered by ";
            footer += "<a style=\"COLOR: #000000\" href=\"http://www.newlifex.com\" target=\"_blank\">";
            footer += Utils.ProductName;
            footer += "</a> &nbsp;&copy; 2001-";
            footer += Utils.GetAssemblyCopyright().Split(',')[0];
            footer += ", <a style=\"COLOR: #000000;font-weight:bold\" href=\"" + Utils.CompanyUrl + "\" target=\"_blank\">" + Utils.CompanyName + "</a></div>";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserName.Attributes.Remove("class");
            PassWord.Attributes.Remove("class");
            UserName.AddAttributes("style", "width:200px");
            PassWord.AddAttributes("style", "width:200px");
            config = GeneralConfigInfo.Current;

            var online = Online.UpdateInfo();
            this.olid = online.ID;
            if (!this.Page.IsPostBack)
            {
                if (!this.config.Adminipaccess.IsNullOrEmpty())
                {
                    string[] iparray = Utils.SplitString(this.config.Adminipaccess, "\n");
                    if (!Utils.InIPArray(WebHelper.UserHost, iparray))
                    {
                        var stringBuilder = new StringBuilder();
                        stringBuilder.Append("<br /><br /><div style=\"width:100%\" align=\"center\"><div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\">");
                        stringBuilder.Append("<img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" />&nbsp; 您的IP地址不在系统允许的范围之内</div></div>");
                        base.Response.Write(stringBuilder.ToString());
                        base.Response.End();
                        return;
                    }
                }
                //var userGroupInfo = UserGroup.FindByID((int)online.GroupID);
                if (online.UserID <= 0 || !online.Group.Is管理员)
                {
                    string text = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                    text += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>无法确认您的身份</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                    text += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><script type=\"text/javascript\">if(top.location!=self.location){top.location.href = \"syslogin.aspx\";}</script><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                    text += "<div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                    text += "无法确认您的身份, 请<a href=\"../login.aspx\">登录</a></div></div></body></html>";
                    base.Response.Write(text);
                    base.Response.End();
                    return;
                }
                if (this.Context.Request.Cookies["bbx_admin"] == null || this.Context.Request.Cookies["bbx_admin"]["key"] == null || ForumUtils.GetCookiePassword(this.Context.Request.Cookies["bbx_admin"]["key"].ToString(), this.config.Passwordkey) != online.Password + Users.GetUserInfo(online.UserID).Secques + online.UserID.ToString())
                {
                    this.Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\">请重新进行管理员登录";
                }
                if (online.UserID > 0 && online.Group.Is管理团队 && !online.UserName.IsNullOrEmpty())
                {
                    this.UserName.Text = online.UserName;
                    this.UserName.AddAttributes("readonly", "true");
                    this.UserName.CssClass = "nofocus";
                    this.UserName.Attributes.Add("onfocus", "this.className='nofocus';");
                    this.UserName.Attributes.Add("onblur", "this.className='nofocus';");
                }
                if (Request["result"] == "1")
                {
                    this.Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">用户不存在或密码错误</font>";
                    return;
                }
                if (Request["result"] == "2")
                {
                    this.Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">用户不是管理员身分,因此无法登陆后台</font>";
                    return;
                }
                if (Request["result"] == "3")
                {
                    this.Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">验证码错误,请重新输入</font>";
                    return;
                }
                if (Request["result"] == "4")
                {
                    this.Msg.Text = "";
                    return;
                }
            }
            if (this.Page.IsPostBack)
            {
                this.VerifyLoginInf();
                return;
            }
            base.Response.Redirect("syslogin.aspx?result=4");
        }

        public void VerifyLoginInf()
        {
            if (!Online.CheckUserVerifyCode(this.olid, Request["vcode"]))
            {
                base.Response.Redirect("syslogin.aspx?result=3");
                return;
            }
            User userInfo;
            //if (this.config.Passwordmode == 1)
            //{
            //    userInfo = Users.GetUserInfo(Users.CheckDvBbsPassword(Request["username"], Request["password"]));
            //}
            //else
            //{
            //    if (this.config.Passwordmode == 0)
            //    {
            //userInfo = Users.GetUserInfo(Users.CheckPassword(Request["username"], Utils.MD5(Request["password"]), false));
            userInfo = BBX.Entity.User.Login(Request["username"], Request["password"]);
            //    }
            //    else
            //    {
            //        userInfo = Users.CheckThirdPartPassword(Request["username"], Request["password"], -1, null);
            //    }
            //}
            if (userInfo == null)
            {
                base.Response.Redirect("syslogin.aspx?result=1");
                return;
            }
            var userGroupInfo = UserGroup.FindByID(userInfo.GroupID);
            if (userGroupInfo.Is管理团队)
            {
                ForumUtils.WriteUserCookie(userInfo.ID, 1440, GeneralConfigInfo.Current.Passwordkey);
                var userGroupInfo2 = UserGroup.FindByID(userInfo.GroupID);
                HttpCookie httpCookie = new HttpCookie("bbx_admin");
                httpCookie.Values["key"] = ForumUtils.SetCookiePassword(userInfo.Password + userInfo.Secques + userInfo.ID, this.config.Passwordkey);
                httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
                HttpContext.Current.Response.AppendCookie(httpCookie);
                AdminVisitLog.InsertLog(userInfo.ID, userInfo.Name, userInfo.GroupID, userGroupInfo2.GroupTitle, WebHelper.UserHost, "后台管理员登陆", "");

                //try
                //{
                //    SoftInfo.LoadSoftInfo();
                //}
                //catch
                //{
                //    base.Response.Write("<script type=\"text/javascript\">top.location.href='index.aspx';</script>");
                //    base.Response.End();
                //}
                try
                {
                    //GeneralConfigs.Serialiaze(GeneralConfigInfo.Current, base.Server.MapPath("../config/general.config"));
                    GeneralConfigInfo.Current.Save();
                }
                catch
                {
                }
                base.Response.Write("<script type=\"text/javascript\">top.location.href='index.aspx';</script>");
                base.Response.End();
                return;
            }
            base.Response.Redirect("syslogin.aspx?result=2");
        }

        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    base.SavePageStateToPersistenceMedium(viewState);
        //}

        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    object result = new object();
        //    try
        //    {
        //        result = base.LoadPageStateFromPersistenceMedium();
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
    }
}