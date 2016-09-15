using System;
using System.Text;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class sharelistgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected DataGrid DataGrid1;
        protected pageinfo info1;
        protected Button UpdateShare;
        public string[] list;

        protected void Page_Load(object sender, EventArgs e)
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            this.list = Utils.SplitString(config.Sharelist.ToString(), ",");
        }

        private void UpdateShare_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var array = DNTRequest.GetFormString("newdisplayorder").SplitAsInt(",");
            string[] array2 = DNTRequest.GetFormString("title").Split(',');
            string[] array3 = DNTRequest.GetFormString("site").Split(',');
            var array4 = DNTRequest.GetFormString("newdisplayorder").SplitAsInt(",");
            string[] stringarray = DNTRequest.GetFormString("sharedisable").Split(',');
            if (array.Length > 0)
            {
                array4 = InsertionSort(array4);
                for (int i = 0; i < array4.Length; i++)
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (array4[i] == array[j])
                        {
                            if (array[j].ToInt() < 0)
                            {
                                sb.Append("0");
                            }
                            else
                            {
                                sb.Append(array[j]);
                            }
                            sb.Append("|");
                            sb.Append(array3[j]);
                            sb.Append("|");
                            sb.Append(array2[j]);
                            sb.Append("|");
                            if (Utils.InArray(array3[j], stringarray))
                            {
                                sb.Append("1");
                            }
                            else
                            {
                                sb.Append("0");
                            }
                            sb.Append(",");
                            break;
                        }
                    }
                }
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                config.Sharelist = sb.ToString().TrimEnd(',');
                config.Save(); ;
            }
            base.RegisterStartupScript("PAGE", "window.location.href='global_sharelistgrid.aspx';");
        }

        public static Int32[] InsertionSort(Int32[] list)
        {
            for (int i = 1; i < list.Length; i++)
            {
                int num = list[i].ToInt();
                int num2 = i;
                while (num2 > 0 && list[num2 - 1].ToInt() > num)
                {
                    list[num2] = list[num2 - 1];
                    num2--;
                }
                list[num2] = num;
            }
            return list;
        }

        protected override void OnInit(EventArgs e)
        {
            this.UpdateShare.Click += new EventHandler(this.UpdateShare_Click);
            base.OnInit(e);
        }
    }
}