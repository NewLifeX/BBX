using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class wordgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info2;
        protected pageinfo PageInfo1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveWord;
        protected BBX.Control.Button DelRec;
        protected BBX.Control.TextBox find;
        protected BBX.Control.TextBox replacement;
        protected BBX.Control.Button AddNewRec;
        protected BBX.Control.Button addbadwords;
        protected BBX.Control.RadioButtonList radfilter;
        protected BBX.Control.TextBox badwords;
        protected BBX.Control.TextBox antipamreplacement;
        protected BBX.Control.Button saveantipamreplacement;

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
            this.DataGrid1.TableHeaderName = "过滤词列表";
            //this.DataGrid1.BindData(BanWords.GetBanWordList());
            this.DataGrid1.BindData(Word.FindAllWithCache());
            this.antipamreplacement.Text = this.config.Antispamreplacement;
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression;
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void SaveWord_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(current.ToString());
                string find = this.DataGrid1.GetControlValue(num, "find").Trim();
                string replacement = this.DataGrid1.GetControlValue(num, "replacement").Trim();
                if (String.IsNullOrEmpty(find) || String.IsNullOrEmpty(replacement))
                {
                    flag = true;
                }
                else
                {
                    //BanWords.UpdateBanWord(id, find, replacement);
                    var w = Word.FindByID(id);
                    w.Key = find;
                    w.Replacement = replacement;
                    w.Update();

                    num++;
                }
            }
            //XCache.Remove(CacheKeys.FORUM_BAN_WORD_LIST);
            Word.GetBanWordList();
            if (flag)
            {
                base.RegisterStartupScript("PAGE", "alert('某些信息不完整，未能更新！');window.location.href='global_wordgrid.aspx';");
            }
            base.RegisterStartupScript("PAGE", "window.location.href='global_wordgrid.aspx';");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Attributes.Add("maxlength", "254");
                textBox.Attributes.Add("size", "30");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0];
                textBox.Attributes.Add("maxlength", "254");
                textBox.Attributes.Add("size", "30");
            }
        }

        private void SaveAntiPamReplacement_Click(object sender, EventArgs e)
        {
            this.config.Antispamreplacement = this.antipamreplacement.Text;
            this.config.Save();

            //GeneralConfigs.Serialiaze(this.config, base.Server.MapPath("../../config/general.config"));
            Caches.ReSetConfig();
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "屏蔽特殊字符", "");
            base.RegisterStartupScript("PAGE", "window.location.href='global_wordgrid.aspx';");
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    Word.DeleteBanWords(Request["id"]);
                    base.Response.Redirect("global_wordgrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_wordgrid.aspx';</script>");
            }
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.find.Text))
            {
                base.RegisterStartupScript("", "<script>alert('要添加的过滤内容不能为空');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }
            if (String.IsNullOrEmpty(this.replacement.Text))
            {
                base.RegisterStartupScript("", "<script>alert('要添加的替换内容不能为空');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }
            if (Word.IsExistBanWord(this.find.Text))
            {
                base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的过滤内容');window.location.href='global_wordgrid.aspx';</script>");
                return;
            }
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "加入字符过滤", "字符为:" + this.find.Text);
            try
            {
                Word.CreateBanWord(this.username, this.find.Text, this.replacement.Text);
                this.BindData();
                base.RegisterStartupScript("PAGE", "window.location.href='global_wordgrid.aspx';");
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_wordgrid.aspx';</script>");
            }
        }

        private string[] getwords()
        {
            return this.badwords.Text.Split('\n');
        }

        private void addbadwords_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.badwords.Text))
            {
                this.BindData();
                return;
            }
            string[] array = this.getwords();
            if (this.radfilter.SelectedValue == "0")
            {
                string text = "";
                string text2 = "";
                //string ids = "";
                //foreach (DataRow dataRow in BanWords.GetBanWordList().Rows)
                //{
                //    ids = ids + dataRow["id"].ToString() + ",";
                //}
                //if (ids != "")
                //{
                //    BanWords.DeleteBanWords(ids.TrimEnd(','));
                //}
                Word.FindAllWithCache().Delete();
                for (int i = 0; i < array.Length; i++)
                {
                    string[] array2 = array[i].Split('=');
                    text = array2[0].ToString().Replace("\r", "").Trim();
                    if (wordgrid.GetReplacement(array, array2, ref text, ref text2))
                    {
                        //BanWords.CreateBanWord(this.username, text, text2);
                        Word.CreateBanWord(this.username, text, text2);
                    }
                }
            }
            if (this.radfilter.SelectedValue == "1")
            {
                string text4 = "";
                string text5 = "";
                for (int j = 0; j < array.Length; j++)
                {
                    string[] array2 = array[j].Split('=');
                    text4 = array2[0].ToString().Replace("\r", "").Trim();
                    if (GetReplacement(array, array2, ref text4, ref text5))
                    {
                        Word.UpdateBadWords(text4, text5);
                    }
                }
            }
            if (this.radfilter.SelectedValue == "2")
            {
                string str = "";
                string text6 = "";
                //DataTable banWordList = BanWords.GetBanWordList();
                for (int k = 0; k < array.Length; k++)
                {
                    string[] array2 = array[k].Split('=');
                    str = array2[0].ToString().Replace("\r", "").Trim();
                    if (GetReplacement(array, array2, ref str, ref text6))
                    {
                        //DataRow[] array3 = banWordList.Select("find='" + str + "'");
                        //if (array3.Length == 0)
                        //{
                        //    BanWords.CreateBanWord(this.username, str, text6);
                        //}
                        var w = Word.FindByKeyWord(str);
                        if (w == null) Word.CreateBanWord(this.username, str, text6);
                    }
                }
            }
            this.BindData();
            this.badwords.Text = "";
        }

        private static bool GetReplacement(string[] badwords, string[] filterwords, ref string find, ref string replacement)
        {
            if (String.IsNullOrEmpty(find))
            {
                return false;
            }
            if (filterwords.Length == 2)
            {
                for (int i = 0; i < badwords.Length; i++)
                {
                    if (filterwords[1].ToString() != "")
                    {
                        replacement = filterwords[1].ToString();
                    }
                }
            }
            else
            {
                if (filterwords.Length < 2)
                {
                    for (int j = 0; j < badwords.Length; j++)
                    {
                        replacement = "**";
                    }
                }
                else
                {
                    replacement = filterwords[filterwords.Length - 1];
                    filterwords.SetValue("", filterwords.Length - 1);
                    find = string.Join("=", filterwords);
                    find = find.Remove(find.Length - 2);
                }
            }
            if (replacement == string.Empty)
            {
                replacement = "**";
            }
            return true;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.SaveWord.Click += new EventHandler(this.SaveWord_Click);
            this.addbadwords.Click += new EventHandler(this.addbadwords_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.saveantipamreplacement.Click += new EventHandler(this.SaveAntiPamReplacement_Click);
            this.DataGrid1.ColumnSpan = 5;
        }
    }
}