using System;
using BBX.Common;

namespace BBX.Forum
{
    /// <summary>页面元数据管理</summary>
    public class PageHelper
    {
        public static String AddMetaInfo(string Seokeywords, string Seodescription, string Seohead)
        {
            var meta = "";
            if (Seokeywords != "")
            {
                meta += "<meta name=\"keywords\" content=\"" + Utils.RemoveHtml(Seokeywords).Replace("\"", " ") + "\" />\r\n";
            }
            if (Seodescription != "")
            {
                meta += "<meta name=\"description\" content=\"" + Utils.RemoveHtml(Seodescription).Replace("\"", " ") + "\" />\r\n";
            }
            meta += Seohead;

            return meta;
        }

        public static String SetMetaRefresh(int sec = 2, string url = null)
        {
            return "\r\n<meta http-equiv=\"refresh\" content=\"" + sec + "; url=" + url + "\" />";
        }

        //public static String AddMeta(string metastr)
        //{
        //	return "\r\n<meta " + metastr + " />";
        //}

        public static String UpdateMetaInfo(String meta, string Seokeywords, string Seodescription, string Seohead)
        {
            var array = Utils.SplitString(meta, "\r\n");

            meta = "";
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (text.ToLower().IndexOf("name=\"keywords\"") > 0 && Seokeywords != null && !Seokeywords.IsNullOrEmpty())
                {
                    meta += "<meta name=\"keywords\" content=\"" + Utils.RemoveHtml(Seokeywords).Replace("\"", " ") + "\" />\r\n";
                }
                else
                {
                    if (text.ToLower().IndexOf("name=\"description\"") > 0 && Seodescription != null && !Seodescription.IsNullOrEmpty())
                        meta += "<meta name=\"description\" content=\"" + Utils.RemoveHtml(Seodescription).Replace("\"", " ") + "\" />\r\n";
                    else
                        meta += text + "\r\n";
                }
            }

            return meta;
        }

        //public static void ShowMessage(byte mode)
        //{
        //    this.ShowMessage("", mode);
        //}

        //public static void ShowMessage(string hint, byte mode)
        //{
        //    var config = GeneralConfigInfo.Current;
        //    var Response = HttpContext.Current.Response;

        //    Response.Clear();
        //    Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head><title>");
        //    string s;
        //    string s2;
        //    switch (mode)
        //    {
        //        case 1:
        //            s = "论坛已关闭";
        //            s2 = config.Closedreason;
        //            break;

        //        case 2:
        //            s = "禁止访问";
        //            s2 = hint;
        //            break;

        //        default:
        //            s = "提示";
        //            s2 = hint;
        //            break;
        //    }
        //    Response.Write(s);
        //    Response.Write(" - ");
        //    Response.Write(config.Forumtitle);
        //    Response.Write(" - Powered by " + Utils.ProductName + "</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
        //    Response.Write(meta);
        //    Response.Write("<style type=\"text/css\"><!-- body { margin: 20px; font-family: Tahoma, Verdana; font-size: 14px; color: #333333; background-color: #FFFFFF; }a {color: #1F4881;text-decoration: none;}--></style></head><body><div style=\"border: #cccccc solid 1px; padding: 20px; width: 500px; margin:auto\" align=\"center\">");
        //    Response.Write(s2);
        //    Response.Write("</div><br /><br /><br /><div style=\"border: 0px; padding: 0px; width: 500px; margin:auto\"><strong>当前服务器时间:</strong> ");
        //    Response.Write(Utils.GetDateTime());
        //    Response.Write("<br /><strong>当前页面</strong> ");
        //    Response.Write(pagename);
        //    Response.Write("<br /><strong>可选择操作:</strong> ");
        //    if (String.IsNullOrEmpty(userkey))
        //    {
        //        Response.Write("<a href=");
        //        Response.Write(forumpath);
        //        Response.Write("login.aspx>登录</a> | <a href=");
        //        Response.Write(forumpath);
        //        Response.Write("register.aspx>注册</a>");
        //    }
        //    else
        //    {
        //        Response.Write("<a href=\"logout.aspx?userkey=" + userkey + "\">退出</a>");
        //        if (useradminid == 1)
        //        {
        //            Response.Write(" | <a href=\"logout.aspx?userkey=" + userkey + "\">系统管理</a>");
        //        }
        //    }
        //    Response.Write("</div></body></html>");
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}
    }
}