using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class global_passportsetting : AdminPage
    {
        protected HtmlForm form1;
        protected TextBox appname;
        protected TextBox callbackurl;
        protected TextBox appurl;
        protected TextareaResize ipaddresses;
        protected HtmlInputHidden apikeyhidd;
        protected Button savepassportinfo;
        protected Hint hint1;
        protected RadioButtonList applicationtype;
        protected RadioButtonList asyncmode;
        protected TextBox asyncurl;
        protected TextBox asynclist;
        protected HtmlTable showurl;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.applicationtype.Items[0].Attributes.Add("onclick", "$('showurl').style.display='';");
            this.applicationtype.Items[1].Attributes.Add("onclick", "$('showurl').style.display='none';");
            this.asyncmode.Items[0].Attributes.Add("onclick", "$('tr_asyncurl').style.display='';$('tr_asynclist').style.display='none';");
            this.asyncmode.Items[1].Attributes.Add("onclick", "$('tr_asyncurl').style.display='none';$('tr_asynclist').style.display='none';");
            this.asyncmode.Items[2].Attributes.Add("onclick", "$('tr_asyncurl').style.display='';$('tr_asynclist').style.display='';");

            if (!base.IsPostBack)
            {
                string apikey = Request["apikey"];
                if (apikey != "")
                {
                    foreach (var item in APIConfigInfo.Current.AppCollection)
                    {
                        if (item.APIKey == apikey)
                        {
                            this.appname.Text = item.AppName;
                            this.applicationtype.SelectedValue = item.ApplicationType.ToString();
                            if (this.applicationtype.SelectedIndex == 1)
                            {
                                base.RegisterStartupScript("applicationtype", "<script>$('showurl').style.display='none';</script>");
                            }
                            this.appurl.Text = item.AppUrl;
                            this.callbackurl.Text = item.CallbackUrl;
                            this.ipaddresses.Text = item.IPAddresses;
                            this.asyncmode.SelectedValue = item.SyncMode.ToString();

                            if (this.asyncmode.SelectedIndex == 1)
                                base.RegisterStartupScript("asyncmode", "<script>$('tr_asyncurl').style.display='none';$('tr_asynclist').style.display='none';</script>");
                            else if (this.asyncmode.SelectedIndex == 2)
                                base.RegisterStartupScript("asyncmode", "<script>$('tr_asyncurl').style.display='';$('tr_asynclist').style.display='';</script>");

                            this.asyncurl.Text = item.SyncUrl;
                            this.asynclist.Text = item.SyncList;
                            break;
                        }
                    }
                }
                this.apikeyhidd.Value = apikey;
            }
        }

        protected void savepassportinfo_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.appname.Text.Trim()))
            {
                base.RegisterStartupScript("PAGE", "alert('整合程序名称不能为空!');");
                return;
            }
            if (this.applicationtype.SelectedValue != "2")
            {
                if (String.IsNullOrEmpty(this.appurl.Text.Trim()))
                {
                    base.RegisterStartupScript("PAGE", "alert('整合程序 Url 地址不能为空!');");
                    return;
                }
                if (this.applicationtype.SelectedValue == "1" && String.IsNullOrEmpty(this.callbackurl.Text.Trim()))
                {
                    base.RegisterStartupScript("PAGE", "alert('登录完成后返回地址不能为空!');");
                    return;
                }
            }
            if (!this.ipaddresses.Text.IsNullOrEmpty())
            {
                string[] array = this.ipaddresses.Text.Replace("\r\n", "").Replace(" ", "").Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string ip = array[i];
                    if (!Utils.IsIP(ip))
                    {
                        base.RegisterStartupScript("PAGE", "alert('IP地址格式错误!');");
                        return;
                    }
                }
            }

            var config = APIConfigInfo.Current;
            if (String.IsNullOrEmpty(this.apikeyhidd.Value))
            {
                var applicationInfo = new ApplicationInfo();
                applicationInfo.AppName = this.appname.Text;
                applicationInfo.AppUrl = this.appurl.Text;
                applicationInfo.APIKey = Utils.MD5(Guid.NewGuid().ToString());
                applicationInfo.Secret = Utils.MD5(Guid.NewGuid().ToString());
                applicationInfo.ApplicationType = this.applicationtype.SelectedValue.ToInt();

                if (applicationInfo.ApplicationType == 1)
                    applicationInfo.CallbackUrl = this.callbackurl.Text;
                else
                    applicationInfo.CallbackUrl = "";

                applicationInfo.CallbackUrl = this.callbackurl.Text;
                applicationInfo.IPAddresses = this.ipaddresses.Text.Replace("\r\n", "").Replace(" ", "");
                applicationInfo.SyncMode = this.asyncmode.SelectedValue.ToInt();
                applicationInfo.SyncUrl = this.asyncurl.Text;
                applicationInfo.SyncList = this.asynclist.Text;

                if (config.AppCollection == null) config.AppCollection = new List<ApplicationInfo>();

                config.AppCollection.Add(applicationInfo);

                config.Save();
            }
            else
            {
                foreach (var item in config.AppCollection)
                {
                    if (item.APIKey == this.apikeyhidd.Value)
                    {
                        item.AppName = this.appname.Text;
                        item.AppUrl = this.appurl.Text;
                        item.ApplicationType = this.applicationtype.SelectedValue.ToInt();

                        if (item.ApplicationType == 1)
                            item.CallbackUrl = this.callbackurl.Text;
                        else
                            item.CallbackUrl = "";

                        item.CallbackUrl = this.callbackurl.Text;
                        item.IPAddresses = this.ipaddresses.Text.Replace("\r\n", "").Replace(" ", "");
                        item.SyncMode = this.asyncmode.SelectedValue.ToInt();
                        item.SyncUrl = this.asyncurl.Text;
                        item.SyncList = this.asynclist.Text;
                        break;
                    }
                }

                config.Save();
            }
            base.Response.Redirect("global_passportmanage.aspx");
        }
    }
}