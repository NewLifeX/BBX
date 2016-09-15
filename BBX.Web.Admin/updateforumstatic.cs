using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class updateforumstatic : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.TextBox pertask1;
        protected BBX.Control.TextBox pertask2;
        protected BBX.Control.TextBox pertask3;
        protected BBX.Control.TextBox pertask4;
        protected BBX.Control.TextBox startfid;
        protected BBX.Control.TextBox endfid;
        protected BBX.Control.TextBox startuid_digest;
        protected BBX.Control.TextBox enduid_digest;
        protected BBX.Control.TextBox startuid_post;
        protected BBX.Control.TextBox enduid_post;
        protected BBX.Control.TextBox starttid;
        protected BBX.Control.TextBox endtid;
        protected BBX.Control.Button SubmitClearFlag;
        protected BBX.Control.Button ReSetStatistic;
        protected BBX.Control.Button SysteAutoSet;
        protected Panel UpdateStoreProcPanel;
        protected BBX.Control.Button UpdatePostSP;
        //protected BBX.Control.Button UpdatePostMaxMinTid;
        protected BBX.Control.Button CreateFullTextIndex;
        protected BBX.Control.Button UpdateCurTopics;
        protected BBX.Control.Button UpdateForumLastPost;
        protected BBX.Control.Button UpdateMyTopic;
        protected BBX.Control.Button UpdateMyPost;
        protected BBX.Control.Button ResetTodayPosts;
        protected Hint Hint1;
        protected BBX.Control.TextBox startfid_id;
        protected BBX.Control.TextBox endfid_id;

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.UpdateStoreProcPanel.Visible = Databases.IsStoreProc();
            // 不支持存储过程
            this.UpdateStoreProcPanel.Visible = false;
            //this.endtid.Text = TableList.GetPostTableId();
            this.starttid.Text = (this.endtid.Text.ToInt() - 5).ToString();
        }

        private void SubmitClearFlag_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //AdminForumStats.ReSetClearMove();
                XForum.ReSetClearMove();
                base.RegisterStartupScript("", "<script language=javascript>clearflag();</script>");
            }
        }

        private void ReSetStatistic_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                AdminForumStats.ReSetStatistic();
                Caches.ReSetStatistics();
                base.RegisterStartupScript("", "<script language=javascript>clearflag();</script>");
            }
        }

        private void SysteAutoSet_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //AdminForums.SetForumslayer();
                AdminForums.SetForumsSubForumCountAndDispalyorder();
                //AdminForums.SetForumsPathList();
                //AdminForums.SetForumsStatus();

                // 修正论坛数据
                var list = XForum.Root.AllChilds;
                //foreach (var item in list)
                //{
                //    item.Layer = item.Deepth;
                //}
                foreach (var item in list)
                {
                    // 所有非顶级论坛，都让它的子孙跟随它隐藏
                    if (item.Layer > 0 && !item.Visible)
                    {
                        foreach (var elm in item.Childs)
                        {
                            elm.Visible = item.Visible;
                        }
                    }
                }
                // 统一保存
                list.Save();

                Caches.ReSetForumLinkList();
                Caches.ReSetForumList();
                Caches.ReSetForumListBoxOptions();
                base.RegisterStartupScript("", "<script language=javascript>clearflag();</script>");
            }
        }

        private void UpdatePostSP_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //this.UpdatePostStoreProc();
                base.RegisterStartupScript("", "<script language=javascript>clearflag();</script>");
            }
        }

        //public void UpdatePostStoreProc()
        //{
        //    Databases.UpdatePostSP();
        //}

        //private void UpdatePostMaxMinTid_Click(object sender, EventArgs e)
        //{
        //    if (base.CheckCookie())
        //    {
        //        TableList.UpdateMinMaxField();
        //        base.RegisterStartupScript("", "<script language=javascript>clearflag();</script>");
        //    }
        //}

        public void UpdateCurTopics_Click(object sender, EventArgs e)
        {
            //Forums.ResetForumsTopics();
            foreach (var item in XForum.Root.AllChilds)
            {
                item.ResetLastPost();
            }
        }

        public void UpdateForumLastPost_Click(object sender, EventArgs e)
        {
            //Forums.ResetLastPostInfo();
            foreach (var item in XForum.Root.AllChilds)
            {
                item.ResetLastPost();
            }
        }

        public void CreateFullTextIndex_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //if (!Databases.IsFullTextSearchEnabled())   return;

                //string text = new Databases().CreateFullTextIndex(Databases.GetDbName(), this.username);
                //if (text.StartsWith("window"))
                //{
                //    base.LoadRegisterStartupScript("PAGE", text);
                //    return;
                //}
                //base.RegisterStartupScript("", text);
            }
        }

        private void UpdateMyTopic_Click(object sender, EventArgs e)
        {
            //Topics.UpdateMyTopic();
            // 清空MyTopic表，然后通过select数据从Topic表重建MyTopic表，暂时不实现
        }

        //private void UpdateMyPost_Click(object sender, EventArgs e)
        //{
        //    Posts.UpdateMyPost();
        //}

        public void ResetTodayPosts_Click(object sender, EventArgs e)
        {
            //Forums.ResetTodayPosts();
            foreach (var item in XForum.Root.AllChilds)
            {
                item.ResetLastPost();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SysteAutoSet.Click += new EventHandler(this.SysteAutoSet_Click);
            this.SubmitClearFlag.Click += new EventHandler(this.SubmitClearFlag_Click);
            //this.UpdatePostMaxMinTid.Click += new EventHandler(this.UpdatePostMaxMinTid_Click);
            this.CreateFullTextIndex.Click += new EventHandler(this.CreateFullTextIndex_Click);
            this.ReSetStatistic.Click += new EventHandler(this.ReSetStatistic_Click);
            this.UpdateCurTopics.Click += new EventHandler(this.UpdateCurTopics_Click);
            this.UpdateMyTopic.Click += new EventHandler(this.UpdateMyTopic_Click);
            this.ResetTodayPosts.Click += new EventHandler(this.ResetTodayPosts_Click);
            this.UpdateForumLastPost.Click += new EventHandler(this.UpdateForumLastPost_Click);
        }
    }
}