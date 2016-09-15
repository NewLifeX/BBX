using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class global_navigationmanage : AdminPage
    {
        private DataTable navMenuTable = Nav.GetNavigation(true).ToDataTable();
        protected HtmlForm form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected HtmlButton returnbutton;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataGrid1.DataKeyField = "id";
            string menuid = DNTRequest.GetString("menuid");
            string mode = DNTRequest.GetString("mode");
            if (mode != "")
            {
                if (mode == "del")
                {
                    Nav.Delete(Nav._.ID == DNTRequest.GetQueryInt("id", 0));
                    Response.Redirect(Request.Path + (DNTRequest.GetString("parentid") != "" ? "?parentid=" + DNTRequest.GetString("parentid") : ""), true);
                }
                else
                {
                    if (String.IsNullOrEmpty(DNTRequest.GetFormString("name").Trim()) ||
                        String.IsNullOrEmpty(DNTRequest.GetFormString("displayorder").Trim()) || 
                        DNTRequest.GetFormInt("displayorder", 0) > Int16.MaxValue)
                    {
                        this.RegisterStartupScript("", "<script type='text/javascript'>alert('名称或序号输入不合法。');window.location=window.location;</script>");
                        return;
                    }
                    if (menuid == "0")
                    {
                        Nav nav = new Nav();
                        nav.ParentID = DNTRequest.GetQueryInt("parentid", 0);
                        GetFromData(nav);
                        //Nav.Insert(nav);
                        nav.Insert();
                    }
                    else
                    {
                        Nav nav = new Nav();
                        nav = Nav.Find(Nav._.ID == DNTRequest.GetFormInt("menuid", 0));
                        GetFromData(nav);
                        Nav.Update(nav);
                    }
                    Response.Redirect(Request.RawUrl, true);
                }
            }
            else
            {
                BindDataGrid(DNTRequest.GetQueryInt("parentid", 0));
                if (String.IsNullOrEmpty(DNTRequest.GetString("parentid")))
                {
                    returnbutton.Visible = false;
                }
            }
        }

        private void GetFromData(Nav nav)
        {
            nav.Name = this.GetMaxlengthString(DNTRequest.GetFormString("name"), 50);
            nav.Title = this.GetMaxlengthString(DNTRequest.GetFormString("title"), 255);
            nav.Url = this.GetMaxlengthString(DNTRequest.GetFormString("url"), 255);
            nav.Target = DNTRequest.GetFormInt("target", 0);
            nav.Available = DNTRequest.GetFormInt("available", 0);
            nav.DisplayOrder = DNTRequest.GetFormInt("displayorder", 0);
            nav.Level = DNTRequest.GetFormInt("level", 0);
        }

        private string GetMaxlengthString(string str, int len)
        {
            if (str.Length > len)
            {
                return str.Substring(0, len);
            }
            return str;
        }

        private void BindDataGrid(int parentid)
        {
            this.DataGrid1.TableHeaderName = ((parentid != 0) ? "子" : "") + "导航菜单管理";
            this.DataGrid1.AllowCustomPaging = false;
            DataTable dataTable = this.navMenuTable.Clone();
            DataRow[] array = this.navMenuTable.Select("parentid=" + parentid);
            for (int i = 0; i < array.Length; i++)
            {
                DataRow row = array[i];
                dataTable.ImportRow(row);
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataBind();
            string text = "\r\n<script type='text/javascript'>\r\nnav = [";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                text += string.Format("\r\n{{id:'{0}',parentid:'{1}',name:'{2}',title:'{3}',url:'{4}',target:'{5}',type:'{6}',available:'{7}',displayorder:'{8}',level:'{9}'}},", new object[]
                {
                    dataRow["id"],
                    dataRow["parentid"],
                    dataRow["name"],
                    dataRow["title"],
                    dataRow["url"],
                    dataRow["target"],
                    dataRow["type"],
                    dataRow["available"],
                    dataRow["displayorder"],
                    dataRow["level"]
                });
            }
            text = text.TrimEnd(',') + "]\r\n</script>";
            base.RegisterStartupScript("", text);
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[0].Controls[0];
                textBox.Attributes.Add("size", "4");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                textBox.Attributes.Add("size", "20");
            }
        }

        protected void saveNav_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(current.ToString());
                string text = this.DataGrid1.GetControlValue(num, "displayorder").Trim();
                string text2 = this.DataGrid1.GetControlValue(num, "url").Trim();

                Nav navigation = (Nav)Nav.Meta.Cache.Find(Nav._.ID, id);
                if (navigation != null)
                {
                    if (!Utils.IsNumeric(text) || String.IsNullOrEmpty(text2))
                    {
                        num++;
                    }
                    else
                    {
                        if (navigation.DisplayOrder != int.Parse(text) || navigation.Url != text2)
                        {
                            navigation.DisplayOrder = int.Parse(text);
                            navigation.Url = text2;
                            navigation.Update();
                        }
                        num++;
                    }
                }
            }
            base.Response.Redirect(base.Request.RawUrl, true);
        }

        protected string GetSubNavMenuManage(string id, string type)
        {
            if ((this.navMenuTable.Select("parentid=" + id).Length != 0 || type == "1") && String.IsNullOrEmpty(Request["parentid"]))
            {
                return string.Format("<a href=\"?parentid={0}\">管理子菜单</a>", id);
            }
            return "";
        }

        protected string GetDeleteLink(string id, string type)
        {
            if (type == "1" && this.navMenuTable.Select("parentid=" + id).Length == 0)
            {
                return string.Format("<a href=\"?{0}mode=del&id={1}\" onclick=\"return confirm('确认要将该菜单项删除吗?');\">删除</a>", (Request["parentid"] != "") ? ("parentid=" + Request["parentid"] + "&") : "", id);
            }
            return "";
        }

        protected string GetLink(string url)
        {
            if (url.ToLower().StartsWith("http://"))
            {
                return url;
            }
            return string.Format("../../{0}", url);
        }

        protected string GetLevel(string level)
        {
            if (level != null)
            {
                if (level == "0")
                {
                    return "游客";
                }
                if (level == "1")
                {
                    return "会员";
                }
                if (level == "2")
                {
                    return "版主";
                }
                if (level == "3")
                {
                    return "管理员";
                }
            }
            return "";
        }
    }
}