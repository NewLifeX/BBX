using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class helplist : AdminPage
    {
        protected HtmlForm form1;
        protected Button Orderby;
        protected Button DelRec;
        protected DataGrid DataGrid1;
        public IDataReader ddr;
        public List<Help> helpInfoList;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.helpInfoList = Help.GetHelpList(0);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            string formString = DNTRequest.GetFormString("id");
            if (base.CheckCookie())
            {
                if (formString != "")
                {
                    this.del(formString);
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_helplist.aspx';</script>");
            }
        }

        protected void del(string idlist)
        {
            Help.DelHelp(idlist);
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除帮助", "删除帮助,帮助ID为: " + Request["id"]);
            base.Response.Redirect("global_helplist.aspx");
        }

        private void Orderby_Click(object sender, EventArgs e)
        {
            string[] orderlist = DNTRequest.GetFormString("orderbyid").Split(',');
            string[] idlist = DNTRequest.GetFormString("hidid").Split(',');
            if (!Help.UpOrder(orderlist, idlist))
            {
                base.RegisterStartupScript("", "<script>alert('输入错误,排序号只能是数字');window.location.href='global_helplist.aspx';</script>");
                return;
            }
            base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.Orderby.Click += new EventHandler(this.Orderby_Click);
        }
    }
}