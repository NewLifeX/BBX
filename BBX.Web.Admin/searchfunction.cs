using System;
using System.Data;
using System.Text;
using BBX.Common;
using BBX.Config;

namespace BBX.Web.Admin
{
    public class searchfunction : UserControlsPageBase
    {
        public StringBuilder sb = new StringBuilder();
        public int menucount;

        protected void Page_Load(object sender, EventArgs e)
        {
            string @string = Request["searchinf"];
            if (@string != "")
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config"));
                this.menucount = dataSet.Tables["toptabmenu"].Rows.Count;
                int num = 0;
                bool flag = false;
                this.sb.Append("<table width=\"98%\" align=\"center\"><tr>");
                foreach (DataRow dataRow in dataSet.Tables["submain"].Rows)
                {
                    if (dataRow["menutitle"].ToString().IndexOf(@string) >= 0)
                    {
                        flag = true;
                        if (num >= 3)
                        {
                            num = 0;
                            this.sb.Append("</tr><tr>");
                        }
                        this.sb.Append("<td align=\"left\" width=\"33%\">");
                        try
                        {
                            this.sb.Append("[" + dataSet.Tables["mainmenu"].Select("menuid=" + dataRow["menuparentid"].ToString().ToString().Trim())[0]["menutitle"].ToString().Trim() + "]");
                        }
                        catch
                        {
                        }
                        this.sb.Append("<a href=\"#\" onclick=\"javascript:resetindexmenu('showmainmenu','toptabmenuid','mainmenulist','" + dataRow["link"] + "');\">" + dataRow["menutitle"].ToString().ToString() + "</a></td>");
                        foreach (DataRow dataRow2 in dataSet.Tables["toptabmenu"].Rows)
                        {
                            if (("," + dataRow2["mainmenuidlist"].ToString() + ",").IndexOf("," + dataRow["menuparentid"] + ",") >= 0)
                            {
                                this.sb.Replace("toptabmenuid", dataRow2["id"].ToString());
                                this.sb.Replace("mainmenulist", dataRow2["mainmenulist"].ToString());
                                string[] array = dataRow2["mainmenuidlist"].ToString().Split(',');
                                for (int i = 0; i < array.Length; i++)
                                {
                                    if (array[i] == dataRow["menuparentid"].ToString())
                                    {
                                        this.sb.Replace("showmainmenu", dataRow2["mainmenulist"].ToString().Split(',')[i]);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        num++;
                    }
                }
                if (!flag)
                {
                    this.sb.Append("没有找到相匹配的结果");
                }
                this.sb.Append("</tr></table>");
                dataSet.Dispose();
                return;
            }
            this.sb.Append("您未输入任何搜索关键字");
        }
    }
}