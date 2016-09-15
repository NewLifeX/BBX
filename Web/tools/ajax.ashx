<%@ WebHandler Language="C#" Class="ajax" %>

using System;
using System.Web;

public class ajax : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        BBX.Web.UI.AjaxPage.Process();
    }

    public bool IsReusable { get { return false; } }
}