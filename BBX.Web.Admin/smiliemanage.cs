using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class smiliemanage : AdminPage
    {
        private ArrayList dirList = new ArrayList();
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid smilesgrid;
        protected BBX.Control.Button SaveSmiles;
        protected BBX.Control.Button DelRec;
        protected Literal dirinfoList;
        protected BBX.Control.Button SubmitButton;

        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.SmilesGridBind();
            }
            this.SmilesListBind();
        }

        private void SmilesGridBind()
        {
            string text = Smilie.ClearEmptySmiliesType();
            var smilesDirList = this.GetSmilesDirList();
            var array = smilesDirList;
            for (int i = 0; i < array.Length; i++)
            {
                var directoryInfo = array[i];
                this.dirList.Add(directoryInfo.Name);
            }
            string text2 = "";
            foreach (var sm in Smilie.GetSmiliesTypes())
            {
                this.dirList.Remove(sm.Url);
                text2 = text2 + sm.Code + ",";
            }
            this.smilesgrid.TableHeaderName = "论坛表情列表";
            //this.smilesgrid.BindData(Smilies.GetSmilies());
            smilesgrid.BindData<Smilie>(Smilie.FindAllWithCache());
            this.ViewState["dir"] = text2;
            this.ViewState["dirList"] = this.dirList;
            if (text != "")
            {
                base.RegisterStartupScript("", "<script>alert('" + text + " 为空,已经被移除!');</script>");
            }
        }

        private DirectoryInfo[] GetSmilesDirList()
        {
            string strPath = BaseConfigs.GetForumPath + "editor/images/smilies";
            DirectoryInfo directoryInfo = new DirectoryInfo(Utils.GetMapPath(strPath));
            return directoryInfo.GetDirectories();
        }

        private void SmilesListBind()
        {
            this.dirinfoList.Text = "";
            this.dirList = (ArrayList)this.ViewState["dirList"];
            if (this.dirList.Count == 0)
            {
                this.SubmitButton.Visible = false;
                return;
            }
            int num = 1;
            foreach (string text in this.dirList)
            {
                Literal expr_6A = this.dirinfoList;
                expr_6A.Text += "<tr class='mouseoutstyle' onmouseover='this.className=\"mouseoverstyle\"' onmouseout='this.className=\"mouseoutstyle\"' >\n";
                Literal expr_85 = this.dirinfoList;
                object text2 = expr_85.Text;
                expr_85.Text = text2 + "<td nowrap='nowrap' style='border: 1px solid rgb(234, 233, 225); width: 20px;'><input type='checkbox' id='id" + num + "' name='id" + num + "' value='" + num + "'/></td>\n";
                Literal expr_ED = this.dirinfoList;
                object text3 = expr_ED.Text;
                expr_ED.Text = text3 + "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='group" + num + "' name='group" + num + "' value='" + text + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                Literal expr_152 = this.dirinfoList;
                object text4 = expr_152.Text;
                expr_152.Text = text4 + "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='text' id='order" + num + "' name='order" + num + "' value='" + num + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" /></td>\n";
                Literal expr_1BC = this.dirinfoList;
                object text5 = expr_1BC.Text;
                expr_1BC.Text = text5 + "<td align='center' nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;'><input type='hidden' name='url" + num + "' value='" + text + "' />" + text + "</td>\n";
                Literal expr_21C = this.dirinfoList;
                expr_21C.Text += "</tr>\n";
                num++;
            }
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= this.dirList.Count; i++)
            {
                if (DNTRequest.GetFormString("id" + i) != null && DNTRequest.GetFormString("id" + i) != "")
                {
                    AdminForums.CreateSmilies(DNTRequest.GetInt("order" + i, 0), 0, DNTRequest.GetFormString("group" + i), DNTRequest.GetFormString("url" + i), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                    int type = Smilie.GetMaxSmiliesId() - 1;
                    int num = 1;
                    string formString = DNTRequest.GetFormString("url" + i);
                    ArrayList smilesFileList = this.GetSmilesFileList(DNTRequest.GetFormString("url" + i));
                    foreach (string text in smilesFileList)
                    {
                        if (!(text.ToLower() == "thumbs.db"))
                        {
                            AdminForums.CreateSmilies(num, type, ":" + formString + num + ":", formString + "/" + text, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                            num++;
                        }
                    }
                }
            }
            base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
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

        private void SaveSmiles_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int num = -1;
                bool flag = false;
                foreach (object current in this.smilesgrid.GetKeyIDArray())
                {
                    string s = current.ToString();
                    string controlValue = this.smilesgrid.GetControlValue(num, "code");
                    string controlValue2 = this.smilesgrid.GetControlValue(num, "displayorder");
                    string controlValue3 = this.smilesgrid.GetControlValue(num, "type");
                    string controlValue4 = this.smilesgrid.GetControlValue(num, "url");
                    num++;
                    if (String.IsNullOrEmpty(controlValue) || !Utils.IsNumeric(controlValue2) || Smilies.IsExistSameSmilieCode(controlValue, int.Parse(s)))
                    {
                        flag = true;
                    }
                    else
                    {
                        AdminForums.UpdateSmilies(int.Parse(s), int.Parse(controlValue2), Utils.StrToInt(controlValue3, 0), controlValue, controlValue4, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                    }
                }
                if (flag)
                {
                    base.RegisterStartupScript("", "<script>alert('某些记录输入不完整或数据库中已存在相同的表情组名称');window.location.href='forum_smiliemanage.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string[] array = Request["id"].Split(',');
                if (this.smilesgrid.Items.Count == array.Length)
                {
                    base.RegisterStartupScript("", "<script>alert('请至少保留一组默认表情，或者添加一组新表情后，再删除本组表情！');window.location.href='forum_smiliemanage.aspx';</script>");
                    return;
                }
                string[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    AdminForums.DeleteSmilies(text, this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                    this.smilesgrid.EditItemIndex = -1;
                    this.SmilesGridBind();
                    Smilie.FindAllByType(int.Parse(text)).Delete();
                }
                base.RegisterStartupScript("", "<script>window.location.href='forum_smiliemanage.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
            this.SubmitButton.Attributes.Add("onclick", "return validate()");
            this.SaveSmiles.Click += new EventHandler(this.SaveSmiles_Click);
            this.smilesgrid.ColumnSpan = 5;
            this.smilesgrid.AllowPaging = false;
        }
    }
}