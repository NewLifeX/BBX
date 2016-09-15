using System;
using System.Data;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addmedal : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox name;
        protected RadioButtonList available;
        protected UpFile image;
        protected Button AddMedalInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                //DataTable medal = Medals.GetMedal();
                //if (medal.Rows.Count >= 100)
                if (Medal.Meta.Count >= 100)
                {
                    base.RegisterStartupScript("", "<script>alert('勋章列表记录已经达到99枚,因此系统不再允许添加勋章');window.location.href='global_medalgrid.aspx';</script>");
                    return;
                }
                this.image.UpFilePath = Utils.GetMapPath("../../images/medals");
            }
        }

        public void AddMedalInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (String.IsNullOrEmpty(this.image.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('上传图片不能为空');</script>");
                    return;
                }
                Medal.Add(this.name.Text, int.Parse(this.available.SelectedValue) != 0, this.image.Text);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件添加", this.name.Text);
                base.RegisterStartupScript("PAGE", "window.location.href='global_medalgrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}