using System;
using System.Web.UI.HtmlControls;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class searchsm : AdminPage
    {
        protected HtmlForm Form1;
        protected HtmlInputCheckBox isnew;
        protected TextBox postdatetime;
        protected TextBox subject;
        protected TextBox msgfromlist;
        protected HtmlInputCheckBox lowerupper;
        protected TextBox message;
        protected Hint Hint1;
        protected HtmlInputCheckBox isupdateusernewpm;

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
    }
}