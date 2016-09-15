using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class smilegrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid smilesgrid;
        protected BBX.Control.Button EditSmile;
        protected BBX.Control.Button DelRec;
        protected Literal fileinfoList;
        protected BBX.Control.Button SubmitButton;
        private ArrayList fileList = new ArrayList();

        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            this.BindFilesList();
        }

        public void BindData()
        {
            this.smilesgrid.AllowCustomPaging = false;
            this.smilesgrid.TableHeaderName = "论坛表情列表";
            //this.smilesgrid.BindData(Smilies.GetSmilieByType(DNTRequest.GetInt("typeid", 0)));
            this.smilesgrid.BindData<Smilie>(Smilie.FindAllByType(DNTRequest.GetInt("typeid", 0)));
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.smilesgrid.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.smilesgrid.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                textBox.Attributes.Add("maxlength", "25");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Attributes.Add("maxlength", "4");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0];
                textBox.Attributes.Add("maxlength", "30");
                textBox.ReadOnly = true;
            }
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    string @string = Request["id"];
                    AdminForums.DeleteSmilies(@string, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                    base.Response.Redirect("forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0));
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';</script>");
            }
        }

        private void EditSmile_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            foreach (object current in this.smilesgrid.GetKeyIDArray())
            {
                if (!Utils.IsNumeric(this.smilesgrid.GetControlValue(num, "displayorder")))
                {
                    flag = true;
                }
                else
                {
                    AdminForums.UpdateSmilies(int.Parse(current.ToString()), int.Parse(this.smilesgrid.GetControlValue(num, "displayorder")), DNTRequest.GetInt("typeid", 0), this.smilesgrid.GetControlValue(num, "code"), this.smilesgrid.GetControlValue(num, "url"), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                    num++;
                }
            }
            if (flag)
            {
                base.RegisterStartupScript("", "<script>alert('批量更新出现输入错误，某些记录未能更新');window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';</script>");
                return;
            }
            base.RegisterStartupScript("", "<script>window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';</script>");
        }

        public string PicStr(string filename)
        {
            return "<img src=../../editor/images/smilies/" + filename + " height=20px width=20px border=0 />";
        }

        private ArrayList GetSmilesFileList(string smilesPath)
        {
            string strPath = BaseConfigs.GetForumPath + "editor/images/smilies/" + smilesPath;
            DirectoryInfo directoryInfo = new DirectoryInfo(Utils.GetMapPath(strPath));
            if (!directoryInfo.Exists)
            {
                throw new IOException("分类文件夹不存在!");
            }
            FileInfo[] files = directoryInfo.GetFiles();
            ArrayList arrayList = new ArrayList();
            FileInfo[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                FileInfo fileInfo = array[i];
                arrayList.Add(fileInfo.Name);
            }
            return arrayList;
        }

        private void BindFilesList()
        {
            try
            {
                this.fileinfoList.Text = "";
                var typeid = WebHelper.RequestInt("typeid");
                var sm = Smilie.FindByID(typeid);
                if (sm != null)
                {
                    this.fileList = this.GetSmilesFileList(sm.Url);
                    string url = sm.Url;
                    //var smilieByType = Smilies.GetSmilieByType(typeid);
                    var list = Smilie.FindAllByType(sm.ID);
                    foreach (var sm2 in list)
                    {
                        var viewState = this.ViewState;
                        viewState["code"] = viewState["code"] + "" + sm2.Code + ",";
                        this.fileList.Remove(sm2.Url.Replace(url + "/", ""));
                    }
                    this.fileList.Remove("Thumbs.db");
                    int num = 1;
                    foreach (string text in this.fileList)
                    {
                        Literal expr_12C = this.fileinfoList;
                        expr_12C.Text += "<tr class='mouseoutstyle' onmouseover='this.className=\"mouseoverstyle\"' onmouseout='this.className=\"mouseoutstyle\"'>\n";
                        Literal expr_147 = this.fileinfoList;
                        object text2 = expr_147.Text;
                        expr_147.Text = text2 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='checkbox' id='id" + num + "' name='id" + num + "' value='" + num + "'/></td>\n";
                        Literal expr_1B4 = this.fileinfoList;
                        object text3 = expr_1B4.Text;
                        expr_1B4.Text = text3 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='code" + num + "' name='code" + num + "' value=':" + url + (list.Count + num) + ":' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                        Literal expr_233 = this.fileinfoList;
                        object text4 = expr_233.Text;
                        expr_233.Text = text4 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='order" + num + "' name='order" + num + "' value='" + num + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" size='4' /></td>\n";
                        Literal expr_2A0 = this.fileinfoList;
                        object text5 = expr_2A0.Text;
                        expr_2A0.Text = text5 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='hidden' name='url" + num + "' value='" + url + "/" + text + "' />" + url + "/" + text + "</td>\n";
                        Literal expr_323 = this.fileinfoList;
                        expr_323.Text = expr_323.Text + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'>" + this.PicStr(sm.Url + "/" + text) + "</td>\n";
                        Literal expr_35B = this.fileinfoList;
                        expr_35B.Text += "</tr>\n";
                        num++;
                    }
                    if (this.fileList.Count == 0)
                    {
                        this.SubmitButton.Visible = false;
                    }
                }
            }
            catch (IOException ex)
            {
                base.RegisterStartupScript("", "<script>alert('" + ex.Message + "');window.location.href='forum_smiliemanage.aspx';</script>");
            }
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            bool flag = false;
            for (int i = 1; i <= this.fileList.Count; i++)
            {
                if (DNTRequest.GetFormString("id" + i) != "")
                {
                    try
                    {
                        if (!Utils.IsNumeric(DNTRequest.GetInt("typeid", 0)))
                        {
                            flag = true;
                        }
                        else
                        {
                            AdminForums.CreateSmilies(DNTRequest.GetFormInt("order" + i, 0), DNTRequest.GetInt("typeid", 0), DNTRequest.GetFormString("code" + i), DNTRequest.GetFormString("url" + i), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                        }
                    }
                    catch
                    {
                        base.RegisterStartupScript("", "<script>alert('出现错误，可能文件超出长度！');window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';</script>");
                    }
                }
            }
            base.RegisterStartupScript("", "<script>" + (flag ? "alert('增加的记录中某个显示顺序是非数字,该记录未能增加!');" : "") + "window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';</script>");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.smilesgrid.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.EditSmile.Click += new EventHandler(this.EditSmile_Click);
            this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
            this.SubmitButton.Attributes.Add("onclick", "return validate()");
            this.smilesgrid.ColumnSpan = 7;
        }
    }
}