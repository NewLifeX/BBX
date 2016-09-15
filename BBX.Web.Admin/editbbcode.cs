using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class editbbcode : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList available;
        protected UpFile icon;
        protected TextBox tag;
        protected TextareaResize replacement;
        protected TextareaResize example;
        protected TextareaResize explanation;
        protected TextBox param;
        protected TextareaResize paramsdescript;
        protected TextBox nest;
        protected TextareaResize paramsdefvalue;
        protected Hint Hint1;
        protected Button UpdateBBCodeInfo;
        protected Button DeleteBBCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request["id"]))
            {
                base.Response.Redirect("forum_bbcodegrid.aspx");
                return;
            }
            if (!base.IsPostBack)
            {
                this.icon.UpFilePath = base.Server.MapPath(this.icon.UpFilePath);
                this.LoadAnnounceInf(DNTRequest.GetInt("id", -1));
            }
        }

        public void LoadAnnounceInf(int id)
        {
            //DataTable bBCode = BBCodes.GetBBCode(id);
            var code = BbCode.FindByID(id);
            if (code != null)
            {
                this.available.SelectedValue = code.Available.ToString();
                this.tag.Text = code.Tag;
                this.replacement.Text = code.Replacement;
                this.example.Text = code.Example;
                this.explanation.Text = code.Explanation;
                this.paramsdescript.Text = code.ParamsDescript;
                this.paramsdefvalue.Text = code.ParamsDefValue;
                this.nest.Text = code.Nest.ToString();
                this.param.Text = code.Params.ToString();
                this.icon.Text = code.Icon;
                this.ViewState["inco"] = code.Icon;
            }
        }

        private void UpdateBBCodeInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                foreach (DictionaryEntry dictionaryEntry in new SortedList
                {
                    {
                        "参数个数",
                        this.param.Text
                    },

                    {
                        "嵌套次数",
                        this.nest.Text
                    }
                })
                {
                    if (!Utils.IsInt(dictionaryEntry.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + dictionaryEntry.Key.ToString() + ",只能是0或者正整数');window.location.href='forum_editbbcode.aspx';</script>");
                        return;
                    }
                }
                string a = this.icon.UpdateFile();
                if (String.IsNullOrEmpty(a))
                {
                    a = this.ViewState["inco"].ToString();
                }
                this.replacement.is_replace = (this.example.is_replace = (this.explanation.is_replace = (this.paramsdescript.is_replace = (this.paramsdefvalue.is_replace = false))));
                BbCode.UpdateBBCode(int.Parse(this.available.SelectedValue), Regex.Replace(this.tag.Text.Replace("<", "").Replace(">", ""), "^[\\>]|[\\{]|[\\}]|[\\[]|[\\]]|[\\']|[\\.]", ""), a, this.replacement.Text, this.example.Text, this.explanation.Text, this.param.Text, this.nest.Text, this.paramsdescript.Text, this.paramsdefvalue.Text, DNTRequest.GetInt("id", 0));
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "更新" + Utils.ProductName + "代码", "TAB为:" + this.tag.Text);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_bbcodegrid.aspx';");
            }
        }

        private void DeleteBBCode_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                BbCode.DeleteBBCode(Request["id"]);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除" + Utils.ProductName + "代码", "TAB为:" + this.tag.Text);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_bbcodegrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateBBCodeInfo.Click += new EventHandler(this.UpdateBBCodeInfo_Click);
            this.DeleteBBCode.Click += new EventHandler(this.DeleteBBCode_Click);
        }
    }
}