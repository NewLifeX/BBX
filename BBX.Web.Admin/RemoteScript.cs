using System;
using System.IO;
using System.Net;
using System.Text;

namespace BBX.Web.Admin
{
    public class RemoteScript : AdminPage
    {
        private void Page_Load(object sender, EventArgs e)
        {
            base.Response.ContentType = "text/javascript";
            string text = base.Request.QueryString["output"];
            string text2 = base.Request.QueryString["callback"];
            string text3 = base.Request.QueryString["AjaxTemplate"];
            if (text3 != null)
            {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322)");
                    object[] args = new object[]
					{
						base.Request.ServerVariables["QUERY_STRING"],
						base.Request.ServerVariables["SERVER_NAME"],
						base.Request.ServerVariables["SERVER_PORT"],
						base.Request.ServerVariables["SCRIPT_NAME"].ToLower().Replace("remotescript.aspx", "")
					};
                    string address = string.Format("http://{1}:{2}{3}ajax.ashx?{0}", args);
                    StreamReader streamReader = new StreamReader(webClient.OpenRead(address), Encoding.UTF8);
                    string text4 = streamReader.ReadToEnd().Trim();
                    streamReader.Close();
                    string text5 = "<!--AjaxContent-->";
                    text4 = text4.Substring(text4.IndexOf(text5, 0) + text5.Length).Trim();
                    text4 = text4.Substring(0, text4.Length - 7);
                    string text6 = "";
                    if (text != null && text.Length > 0)
                    {
                        text6 += string.Format("if ($('{0}') != null) $('{0}').innerHTML = '{1}';", text, RemoteScript.ToJavaScriptString(text4));
                    }
                    if (text2 != null && text2.Length > 0)
                    {
                        text6 += string.Format(" if ({0}) {0}('{1}');", text2, RemoteScript.ToJavaScriptString(text4));
                    }
                    base.Response.Write(text6);
                }
                catch
                {
                }
            }
        }

        public static string ToJavaScriptString(string str)
        {
            return str.Replace("\n", "").Replace("\r", "").Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"").Replace("/", "\\/");
        }
    }
}