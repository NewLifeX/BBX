using System;
using System.Threading;
using System.Web.UI.HtmlControls;
using BBX.Config;
using BBX.Control;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class logandshrinkdb : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox strDbName;
        protected TextBox size;
        protected Button ClearLog;
        protected Button ShrinkDB;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Databases.IsShrinkData() || this.Page.IsPostBack)
            //{
            base.Response.Write("<script>alert('您所使用的数据库不支持此功能');</script>");
            base.Response.Write("<script>history.go(-1)</script>");
            base.Response.End();
            return;
            //}
            //if (!base.IsFounderUid(this.userid))
            //{
            //    base.Response.Write(base.GetShowMessage());
            //    base.Response.End();
            //    return;
            //}
            //string getDBConnectString = BaseConfigInfo.Current.Dbconnectstring;
            //string[] array = getDBConnectString.Split(';');
            //for (int i = 0; i < array.Length; i++)
            //{
            //    string text = array[i];
            //    if (text.ToLower().IndexOf("initial catalog") >= 0 || text.ToLower().IndexOf("database") >= 0)
            //    {
            //        this.strDbName.Text = text.Split('=')[1].Trim();
            //        return;
            //    }
            //}
        }

        public void ShrinkDateBase()
        {
            //string text = Databases.ShrinkDataBase(this.strDbName.Text, this.size.Text);
            //base.RegisterStartupScript(text.StartsWith("window") ? "PAGE" : "", text);
        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (!base.IsFounderUid(this.userid))
                {
                    base.Response.Write(base.GetShowMessage());
                    base.Response.End();
                    return;
                }
                //string text = Databases.ClearDBLog(this.strDbName.Text);
                //if (text.StartsWith("window"))
                //{
                //    base.LoadRegisterStartupScript("PAGE", text);
                //    return;
                //}
                //base.RegisterStartupScript("", text);
            }
        }

        private void ShrinkDB_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (!base.IsFounderUid(this.userid))
                {
                    base.Response.Write(base.GetShowMessage());
                    base.Response.End();
                    return;
                }
                Thread thread = new Thread(new ThreadStart(this.ShrinkDateBase));
                thread.Start();
                base.LoadRegisterStartupScript("PAGE", "window.location.href='global_logandshrinkdb.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ClearLog.Click += new EventHandler(this.ClearLog_Click);
            this.ShrinkDB.Click += new EventHandler(this.ShrinkDB_Click);
            this.strDbName.IsReplaceInvertedComma = false;
        }
    }
}