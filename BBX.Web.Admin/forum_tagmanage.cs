using System;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class forum_tagmanage : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.TextBox tagname;
        protected BBX.Control.TextBox txtfrom;
        protected BBX.Control.TextBox txtend;
        protected BBX.Control.RadioButtonList radstatus;
        protected BBX.Control.Button search;
        protected BBX.Control.Button searchtag;
        protected BBX.Control.DataGrid DataGrid1;
        protected Hint Hint1;
        protected BBX.Control.Button savetags;
        protected BBX.Control.Button DisableRec;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "标签列表";
            this.DataGrid1.DataKeyField = "ID";
            //this.DataGrid1.BindData(Tags.GetForumTags("", this.radstatus.SelectedValue.ToInt()));
            DataGrid1.BindData<Tag>(Tag.GetForumTags("", radstatus.SelectedValue.ToInt()));
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void savetags_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int tagid = int.Parse(current.ToString());
                string s = this.DataGrid1.GetControlValue(num, "orderid").Trim();
                string color = this.DataGrid1.GetControlValue(num, "color").Trim().ToUpper();
                if (!Tag.UpdateForumTags(tagid, int.Parse(s), color))
                    flag = true;
                else
                    num++;
            }
            Topics.NeatenRelateTopics();
            //this.WriteTagsStatus();
            if (flag)
            {
                base.RegisterStartupScript("PAGE", "alert('某些记录输入错误，未能被更新！');window.location.href='aspx';");
                return;
            }
            base.RegisterStartupScript("PAGE", "window.location.href='aspx';");
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string uid = Request["uid"];
                if (uid != "")
                {
                    //Users.DeleteUsers(uid);
                    BBX.Entity.User.FindByID(uid.ToInt()).Delete();
                    base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location='forum_audituser.aspx';</script>");
            }
        }

        //private static void WriteTextFile(string filename, string content)
        //{
        //    //FileStream fileStream = new FileStream(Utils.GetMapPath("../../cache/tag/" + filename), FileMode.Create);
        //    //byte[] bytes = Encoding.UTF8.GetBytes(content);
        //    //fileStream.Write(bytes, 0, bytes.Length);
        //    //fileStream.Close();

        //    File.WriteAllText("../../cache/tag/".CombinePath(filename), content);
        //}

        //private void WriteTagsStatus()
        //{
        //    string text = "";
        //    string text2 = "";
        //    //DataTable forumTags = Tags.GetForumTags("", this.radstatus.SelectedValue.ToInt());
        //    //foreach (DataRow dataRow in forumTags.Rows)
        //    foreach (var tag in Tag.GetForumTags("", radstatus.SelectedValue.ToInt()))
        //    {
        //        if (tag.OrderID == -1)
        //        {
        //            text = text + "'" + tag.ID + "',";
        //        }
        //        if (!String.IsNullOrEmpty(tag.Color))
        //        {
        //            string text3 = text2;
        //            text2 = text3 + "'" + tag.ID + "':{'tagid' : '" + tag.ID + "', 'color' : '" + tag.Color + "'},";
        //        }
        //    }
        //    text = "var closedtags = [" + text.TrimEnd(',') + "];";
        //    WriteTextFile("closedtags.txt", text);
        //    text2 = "var colorfultags = {" + text2.TrimEnd(',') + "};";
        //    WriteTextFile("colorfultags.txt", text2);
        //}

        protected void searchtag_Click(object sender, EventArgs e)
        {
            this.BindData();
            string name = this.tagname.Text.Trim();
            int start = txtfrom.Text.ToInt(101);
            int end = txtend.Text.ToInt(102);
            if ((start == 101 && end == 102 && String.IsNullOrEmpty(name)) ||
                (start == 101 && end != 102) ||
                (start != 101 && end == 102))
            {
                return;
            }
            int type = radstatus.SelectedValue.ToInt();
            if (name != "" && start == 101 && end == 102)
            {
                if (base.CheckCookie())
                {
                    //this.DataGrid1.BindData(Tags.GetForumTags(text, type));
                    DataGrid1.BindData<Tag>(Tag.GetForumTags(name, type));
                    return;
                }
            }
            else
            {
                //this.DataGrid1.BindData(Topics.GetTopicNumber(name, start, end, type));
                DataGrid1.BindData<Tag>(Tag.GetForumTags(name, type, start, end));
            }
        }

        protected void DisableRec_Click(object sender, EventArgs e)
        {
            int num = 0;
            string tagid = Request["tagid"];
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(current.ToString());
                if (("," + tagid + ",").IndexOf("," + id + ",") != -1)
                {
                    string color = this.DataGrid1.GetControlValue(num, "color").Trim().ToUpper();
                    Tag.UpdateForumTags(id, -1, color);
                    num++;
                }
            }
            Topics.NeatenRelateTopics();
            //this.WriteTagsStatus();
            base.RegisterStartupScript("PAGE", "window.location.href='aspx';");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                textBox.Attributes.Add("maxlength", "3");
                textBox.Attributes.Add("size", "3");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                textBox.Attributes.Add("maxlength", "6");
                textBox.Attributes.Add("size", "6");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.TableHeaderName = "标签列表";
            this.DataGrid1.ColumnSpan = 8;
        }
    }
}