using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Uri url = Request.Url;
        if (url.Host.ToLower() == "j.newlifex.com")
            Response.Redirect(url.Scheme + "://j.nnhy.org" + url.PathAndQuery);
        else if (url.Host.ToLower() == "s.newlifex.com")
            Response.Redirect(url.Scheme + "://s.nnhy.org" + url.PathAndQuery);
    }
}