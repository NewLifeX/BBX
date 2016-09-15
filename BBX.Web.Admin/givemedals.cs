using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class givemedals : AdminPage
    {
        protected HtmlForm Form1;
        protected Literal givenusername;
        protected Repeater medallist;
        protected BBX.Control.TextBox reason;
        protected Hint Hint1;
        protected BBX.Control.Button GivenMedal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (String.IsNullOrEmpty(Request["uid"]))
                {
                    return;
                }
                int @int = DNTRequest.GetInt("uid", -1);
                User userInfo = Users.GetUserInfo(@int);
                this.givenusername.Text = userInfo.Name;
                if (String.IsNullOrEmpty(userInfo.Field.Medals.Trim()))
                {
                    userInfo.Field.Medals = "0";
                }
                this.LoadDataInfo("," + userInfo.Field.Medals + ",");
            }
        }

        public void LoadDataInfo(string begivenmedal)
        {
            //DataTable availableMedal = Medals.GetAvailableMedal();
            var availableMedal = Medal.GetAvailable().ToDataTable(false);
            if (availableMedal != null)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = "isgiven";
                dataColumn.DataType = typeof(Boolean);
                dataColumn.DefaultValue = false;
                dataColumn.AllowDBNull = false;
                availableMedal.Columns.Add(dataColumn);
                foreach (DataRow dataRow in availableMedal.Rows)
                {
                    if (begivenmedal.IndexOf("," + dataRow["medalid"].ToString() + ",") >= 0)
                    {
                        dataRow["isgiven"] = true;
                    }
                }
                this.medallist.DataSource = availableMedal;
                this.medallist.DataBind();
            }
        }

        public string BeGivenMedal(string isgiven, string medalid)
        {
            if (isgiven == "True")
            {
                return "<INPUT id=\"medalid\" type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\" checked>";
            }
            return "<INPUT id=\"medalid\" type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\">";
        }

        private void GivenMedal_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int @int = DNTRequest.GetInt("uid", -1);
                Users.UpdateMedals(@int, Request["medalid"], this.userid, this.username, WebHelper.UserHost, this.reason.Text.Trim());
                if (String.IsNullOrEmpty(Request["codition"]))
                {
                    this.Session["codition"] = null;
                }
                else
                {
                    this.Session["codition"] = Request["codition"].Replace("^", "'");
                }
                base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + @int + "&condition=" + Request["condition"] + "';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.GivenMedal.Click += new EventHandler(this.GivenMedal_Click);
        }
    }
}