using System;
using System.Data;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class global_passportmanage : AdminPage
    {
        protected HtmlForm form1;
        protected RadioButtonList allowpassport;
        protected HtmlGenericControl passportbody;
        protected DataGrid DataGrid1;
        protected Button DelRec;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                APIConfigInfo config = APIConfigInfo.Current;
                this.allowpassport.SelectedValue = (config.Enable ? "1" : "0");
                this.passportbody.Attributes.Add("style", "display:" + (config.Enable ? "block" : "none"));
                this.allowpassport.Items[0].Attributes.Add("onclick", "setAllowPassport(1)");
                this.allowpassport.Items[1].Attributes.Add("onclick", "setAllowPassport(0)");
                var appCollection = config.AppCollection;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("appname");
                dataTable.Columns.Add("apptype");
                dataTable.Columns.Add("callbackurl");
                dataTable.Columns.Add("apikey");
                dataTable.Columns.Add("secret");
                foreach (ApplicationInfo current in appCollection)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["appname"] = current.AppName;
                    dataRow["apptype"] = ((current.ApplicationType == 1) ? "Web" : "桌面");
                    dataRow["callbackurl"] = current.CallbackUrl;
                    dataRow["apikey"] = current.APIKey;
                    dataRow["secret"] = current.Secret;
                    dataTable.Rows.Add(dataRow);
                }
                this.DataGrid1.TableHeaderName = "整合程序列表";
                this.DataGrid1.DataKeyField = "apikey";
                this.DataGrid1.DataSource = dataTable;
                this.DataGrid1.DataBind();
            }
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            string @string = Request["apikey"];
            if (String.IsNullOrEmpty(@string))
            {
                return;
            }
            string[] array = @string.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string b = array[i];
                var config = APIConfigInfo.Current;
                var appCollection = config.AppCollection;
                foreach (var current in appCollection)
                {
                    if (current.APIKey == b)
                    {
                        config.AppCollection.Remove(current);
                        break;
                    }
                }

                //APIConfigs.SaveConfig(config);
                config.Save();
            }
            base.Response.Redirect("global_passportmanage.aspx");
        }
    }
}