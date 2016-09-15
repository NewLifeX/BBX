using System;
using System.Data;
using BBX.Config;
using BBX.Forum;
using BBX.Web.Admin;

namespace NewLife.BBX.Admin
{
    public partial class CreditStrategy : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadScoreInf();
            }
        }

        public void LoadScoreInf()
        {
            this.Literal1.Text = "转向地址和推广积分策略";
            this.txtTransferUrl.Text = SpreadConfig.Current.TransferUrl;
            string spreadCredits = SpreadConfig.Current.SpreadCredits;
            if (spreadCredits != "" && spreadCredits != "0")
            {
                string[] array = spreadCredits.Split(',');
                this.extcredits1.Text = array[0].Trim();
                this.extcredits2.Text = array[1].Trim();
                this.extcredits3.Text = array[2].Trim();
                this.extcredits4.Text = array[3].Trim();
                this.extcredits5.Text = array[4].Trim();
                this.extcredits6.Text = array[5].Trim();
                this.extcredits7.Text = array[6].Trim();
                this.extcredits8.Text = array[7].Trim();
            }
            else
            {
                this.extcredits1.Text = "";
                this.extcredits2.Text = "";
                this.extcredits3.Text = "";
                this.extcredits4.Text = "";
                this.extcredits5.Text = "";
                this.extcredits6.Text = "";
                this.extcredits7.Text = "";
                this.extcredits8.Text = "";
            }
            DataRow dataRow = Scoresets.GetScoreSet().Rows[0];
            if (dataRow[2].ToString().Trim() != "")
            {
                this.extcredits1name.Text = dataRow[2].ToString().Trim();
            }
            else
            {
                this.extcredits1.Enabled = false;
            }
            if (dataRow[3].ToString().Trim() != "")
            {
                this.extcredits2name.Text = dataRow[3].ToString().Trim();
            }
            else
            {
                this.extcredits2.Enabled = false;
            }
            if (dataRow[4].ToString().Trim() != "")
            {
                this.extcredits3name.Text = dataRow[4].ToString().Trim();
            }
            else
            {
                this.extcredits3.Enabled = false;
            }
            if (dataRow[5].ToString().Trim() != "")
            {
                this.extcredits4name.Text = dataRow[5].ToString().Trim();
            }
            else
            {
                this.extcredits4.Enabled = false;
            }
            if (dataRow[6].ToString().Trim() != "")
            {
                this.extcredits5name.Text = dataRow[6].ToString().Trim();
            }
            else
            {
                this.extcredits5.Enabled = false;
            }
            if (dataRow[7].ToString().Trim() != "")
            {
                this.extcredits6name.Text = dataRow[7].ToString().Trim();
            }
            else
            {
                this.extcredits6.Enabled = false;
            }
            if (dataRow[8].ToString().Trim() != "")
            {
                this.extcredits7name.Text = dataRow[8].ToString().Trim();
            }
            else
            {
                this.extcredits7.Enabled = false;
            }
            if (dataRow[9].ToString().Trim() != "")
            {
                this.extcredits8name.Text = dataRow[9].ToString().Trim();
                return;
            }
            this.extcredits8.Enabled = false;
        }

        private void DeleteSet_Click(object sender, EventArgs e)
        {
            SpreadConfig config = SpreadConfig.Current;
            config.SpreadCredits = "";
            config.TransferUrl = "";
            config.Save();
            this.LoadScoreInf();
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            string spreadCredits = this.extcredits1.Text + "," + this.extcredits2.Text + "," + this.extcredits3.Text + "," + this.extcredits4.Text + "," + this.extcredits5.Text + "," + this.extcredits6.Text + "," + this.extcredits7.Text + "," + this.extcredits8.Text;
            SpreadConfig config = SpreadConfig.Current;
            config.SpreadCredits = spreadCredits;
            config.TransferUrl = this.txtTransferUrl.Text;
            config.Save();
            this.LoadScoreInf();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.DeleteSet.Click += new EventHandler(this.DeleteSet_Click);
            base.Load += new EventHandler(this.Page_Load);
        }
    }
}