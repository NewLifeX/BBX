using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using BBX.Plugin.Payment;
using BBX.Plugin.Payment.Alipay;

namespace BBX.Web.Admin
{
    public class scoreset : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.TextBox formula;
        protected BBX.Control.CheckBoxList RefreshUserScore;
        protected BBX.Control.DropDownList creditstrans;
        protected BBX.Control.DropDownList topicattachcreditstrans;
        protected BBX.Control.DropDownList bonuscreditstrans;
        protected BBX.Control.TextBox transfermincredits;
        protected BBX.Control.TextBox maxincperthread;
        protected BBX.Control.TextBox creditstax;
        protected BBX.Control.TextBox exchangemincredits;
        protected BBX.Control.TextBox maxchargespan;
        protected BBX.Control.Button Save;
        protected Hint Hint1;
        protected HtmlTable creditstransLayer;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected TabPage tabPage22;
        protected BBX.Control.TextBox losslessdel;
        public DataSet dsSrc = new DataSet();
        public GeneralConfigInfo configInfo = GeneralConfigInfo.Current;
        public string[] scoreNames = Scoresets.GetValidScoreName();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["accout"]))
            {
                this.TestAccout(Request["accout"]);
            }
            if (!this.Page.IsPostBack)
            {
                for (int i = 1; i < 9; i++)
                {
                    if (!(this.scoreNames[i] == string.Empty))
                    {
                        this.creditstrans.Items.Add(new ListItem("extcredits" + i + "(" + this.scoreNames[i] + ")", i.ToString()));
                        this.topicattachcreditstrans.Items.Add(new ListItem("extcredits" + i + "(" + this.scoreNames[i] + ")", i.ToString()));
                        this.bonuscreditstrans.Items.Add(new ListItem("extcredits" + i + "(" + this.scoreNames[i] + ")", i.ToString()));
                    }
                }
                this.dsSrc.ReadXml(base.Server.MapPath("../../config/scoreset.config"));
                this.formula.Text = this.dsSrc.Tables["formula"].Rows[0]["formulacontext"].ToString();
                if (String.IsNullOrEmpty(this.dsSrc.Tables["formula"].Rows[0]["creditstrans"].ToString()))
                {
                    this.creditstrans.SelectedIndex = 0;
                    base.ClientScript.RegisterClientScriptBlock(base.GetType(), "credits", "creditsTransStatus(0);");
                }
                else
                {
                    try
                    {
                        this.creditstrans.SelectedValue = this.dsSrc.Tables["formula"].Rows[0]["creditstrans"].ToString();
                    }
                    catch
                    {
                    }
                }
                this.creditstrans.Attributes.Add("onchange", "creditsTransStatus(this.value);");
                if (String.IsNullOrEmpty(this.dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"].ToString()))
                {
                    this.topicattachcreditstrans.SelectedIndex = 0;
                }
                else
                {
                    try
                    {
                        this.topicattachcreditstrans.SelectedValue = this.dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"].ToString();
                    }
                    catch
                    {
                    }
                }
                if (String.IsNullOrEmpty(this.dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"].ToString()))
                {
                    this.bonuscreditstrans.SelectedIndex = 0;
                }
                else
                {
                    try
                    {
                        this.bonuscreditstrans.SelectedValue = this.dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"].ToString();
                    }
                    catch
                    {
                    }
                }
                this.creditstax.Text = this.dsSrc.Tables["formula"].Rows[0]["creditstax"].ToString();
                this.transfermincredits.Text = this.dsSrc.Tables["formula"].Rows[0]["transfermincredits"].ToString();
                this.exchangemincredits.Text = this.dsSrc.Tables["formula"].Rows[0]["exchangemincredits"].ToString();
                this.maxincperthread.Text = this.dsSrc.Tables["formula"].Rows[0]["maxincperthread"].ToString();
                this.maxchargespan.Text = this.dsSrc.Tables["formula"].Rows[0]["maxchargespan"].ToString();
                this.losslessdel.Text = this.configInfo.Losslessdel.ToString();
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "<img src='../images/icons/icon31.jpg'>积分设置";
            this.DataGrid1.DataSource = this.dsSrc.Tables[0];
            this.DataGrid1.DataBind();
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Header:
                    e.Item.Cells[0].ColumnSpan = 1;
                    e.Item.Cells[1].Visible = false;
                    return;
                case ListItemType.Footer:
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                case ListItemType.SelectedItem:
                    break;
                case ListItemType.EditItem:
                    for (int i = 0; i < this.DataGrid1.Columns.Count; i++)
                    {
                        if (e.Item.ItemType == ListItemType.EditItem && i >= 3)
                        {
                            System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[i].Controls[0];
                            textBox.Width = 60;
                        }
                    }
                    break;
                default:
                    return;
            }
        }

        public DataTable AbsScoreSet1(DataTable dt)
        {
            foreach (DataRow dataRow in dt.Rows)
            {
                if (Convert.ToInt16(dataRow["id"].ToString()) > 2 && (dataRow["id"].ToString() == "11" || dataRow["id"].ToString() == "12" || dataRow["id"].ToString() == "13"))
                {
                    dataRow["extcredits1"] = -1.0 * double.Parse(dataRow["extcredits1"].ToString());
                    dataRow["extcredits2"] = -1.0 * double.Parse(dataRow["extcredits2"].ToString());
                    dataRow["extcredits3"] = -1.0 * double.Parse(dataRow["extcredits3"].ToString());
                    dataRow["extcredits4"] = -1.0 * double.Parse(dataRow["extcredits4"].ToString());
                    dataRow["extcredits5"] = -1.0 * double.Parse(dataRow["extcredits5"].ToString());
                    dataRow["extcredits6"] = -1.0 * double.Parse(dataRow["extcredits6"].ToString());
                    dataRow["extcredits7"] = -1.0 * double.Parse(dataRow["extcredits7"].ToString());
                    dataRow["extcredits8"] = -1.0 * double.Parse(dataRow["extcredits8"].ToString());
                }
            }
            return dt;
        }

        public void SetUserGroupRaterange(int scoreid)
        {
            bool flag = true;
            foreach (DataRow dataRow in Scoresets.GetScoreSet().Rows)
            {
                if (dataRow["id"].ToString() != "1" && dataRow["id"].ToString() != "2" && dataRow[scoreid + 1].ToString().Trim() != "0")
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                //foreach (DataRow dataRow2 in UserGroups.GetRateRange(scoreid).Rows)
                //{
                //	UserGroups.UpdateRateRange(dataRow2["raterange"].ToString().Replace(scoreid + ",True,", scoreid + ",False,"), dataRow2["groupid"].ToInt(0));
                //}
                foreach (var item in UserGroup.FindAllWithCache())
                {
                    item.Raterange = item.Raterange.Replace(scoreid + ",True,", scoreid + ",False,");
                    item.Update();
                }
            }
        }

        public void DataGrid_Update(object sender, DataGridCommandEventArgs E)
        {
            string text = this.DataGrid1.DataKeys[E.Item.ItemIndex].ToString();
            string text2 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[3].Controls[0]).Text.Trim();
            string text3 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[4].Controls[0]).Text.Trim();
            string text4 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[5].Controls[0]).Text.Trim();
            string text5 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[6].Controls[0]).Text.Trim();
            string text6 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[7].Controls[0]).Text.Trim();
            string text7 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[8].Controls[0]).Text.Trim();
            string text8 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[9].Controls[0]).Text.Trim();
            string text9 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[10].Controls[0]).Text.Trim();
            int num = (int)Convert.ToInt16(text);
            bool flag = true;
            if (num <= 2)
            {
                if (num == 1)
                {
                    if (String.IsNullOrEmpty(text2))
                    {
                        this.SetUserGroupRaterange(1);
                    }
                    if (String.IsNullOrEmpty(text3))
                    {
                        this.SetUserGroupRaterange(2);
                    }
                    if (String.IsNullOrEmpty(text4))
                    {
                        this.SetUserGroupRaterange(3);
                    }
                    if (String.IsNullOrEmpty(text5))
                    {
                        this.SetUserGroupRaterange(4);
                    }
                    if (String.IsNullOrEmpty(text6))
                    {
                        this.SetUserGroupRaterange(5);
                    }
                    if (String.IsNullOrEmpty(text7))
                    {
                        this.SetUserGroupRaterange(6);
                    }
                    if (String.IsNullOrEmpty(text8))
                    {
                        this.SetUserGroupRaterange(7);
                    }
                    if (String.IsNullOrEmpty(text9))
                    {
                        this.SetUserGroupRaterange(8);
                    }
                }
                if (text2 != "" && Utils.IsNumeric(text2.Replace("-", "")))
                {
                    flag = false;
                }
                if (text3 != "" && Utils.IsNumeric(text3.Replace("-", "")))
                {
                    flag = false;
                }
                if (text4 != "" && Utils.IsNumeric(text4.Replace("-", "")))
                {
                    flag = false;
                }
                if (text5 != "" && Utils.IsNumeric(text5.Replace("-", "")))
                {
                    flag = false;
                }
                if (text6 != "" && Utils.IsNumeric(text6.Replace("-", "")))
                {
                    flag = false;
                }
                if (text7 != "" && Utils.IsNumeric(text7.Replace("-", "")))
                {
                    flag = false;
                }
                if (text8 != "" && Utils.IsNumeric(text8.Replace("-", "")))
                {
                    flag = false;
                }
                if (text9 != "" && Utils.IsNumeric(text9.Replace("-", "")))
                {
                    flag = false;
                }
                if (!flag)
                {
                    base.RegisterStartupScript("DataGrid1", "<script>alert('当前项中数据不能为数字');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(text2) || String.IsNullOrEmpty(text3) || String.IsNullOrEmpty(text4) ||
                    String.IsNullOrEmpty(text5) || String.IsNullOrEmpty(text6) || String.IsNullOrEmpty(text7) ||
                    String.IsNullOrEmpty(text8) || String.IsNullOrEmpty(text9))
                {
                    flag = false;
                }

                if (!flag)
                {
                    base.RegisterStartupScript("DataGrid1", "<script>alert('当前项中数据不能为空.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                flag = true;
                if (text2 != "" && !Utils.IsNumeric(text2.Replace("-", "")))
                {
                    flag = false;
                }
                if (text3 != "" && !Utils.IsNumeric(text3.Replace("-", "")))
                {
                    flag = false;
                }
                if (text4 != "" && !Utils.IsNumeric(text4.Replace("-", "")))
                {
                    flag = false;
                }
                if (text5 != "" && !Utils.IsNumeric(text5.Replace("-", "")))
                {
                    flag = false;
                }
                if (text6 != "" && !Utils.IsNumeric(text6.Replace("-", "")))
                {
                    flag = false;
                }
                if (text7 != "" && !Utils.IsNumeric(text7.Replace("-", "")))
                {
                    flag = false;
                }
                if (text8 != "" && !Utils.IsNumeric(text8.Replace("-", "")))
                {
                    flag = false;
                }
                if (text9 != "" && !Utils.IsNumeric(text9.Replace("-", "")))
                {
                    flag = false;
                }
                if (!flag)
                {
                    base.RegisterStartupScript("DataGrid1", "<script>alert('当前项中数据只能为数字.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                flag = true;
                if (Convert.ToDouble(text2) > 999.0 || Convert.ToDouble(text2) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text3) > 999.0 || Convert.ToDouble(text3) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text4) > 999.0 || Convert.ToDouble(text4) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text5) > 999.0 || Convert.ToDouble(text5) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text6) > 999.0 || Convert.ToDouble(text6) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text7) > 999.0 || Convert.ToDouble(text7) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text8) > 999.0 || Convert.ToDouble(text8) < -999.0)
                {
                    flag = false;
                }
                if (Convert.ToDouble(text9) > 999.0 || Convert.ToDouble(text9) < -999.0)
                {
                    flag = false;
                }
                if (!flag)
                {
                    base.RegisterStartupScript("DataGrid1", "<script>alert('各项积分增减允许的范围为-999～+999.');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
            }
            this.dsSrc = new DataSet();
            this.dsSrc.ReadXml(base.Server.MapPath("../../config/scoreset.config"));
            foreach (DataRow dataRow in this.dsSrc.Tables["record"].Rows)
            {
                int num2 = (int)Convert.ToInt16(dataRow["id"].ToString());
                if (text == num2.ToString())
                {
                    if (num <= 2)
                    {
                        dataRow["extcredits1"] = text2;
                        dataRow["extcredits2"] = text3;
                        dataRow["extcredits3"] = text4;
                        dataRow["extcredits4"] = text5;
                        dataRow["extcredits5"] = text6;
                        dataRow["extcredits6"] = text7;
                        dataRow["extcredits7"] = text8;
                        dataRow["extcredits8"] = text9;
                    }
                    else
                    {
                        dataRow["extcredits1"] = Math.Round(double.Parse(text2), 2);
                        dataRow["extcredits2"] = Math.Round(double.Parse(text3), 2);
                        dataRow["extcredits3"] = Math.Round(double.Parse(text4), 2);
                        dataRow["extcredits4"] = Math.Round(double.Parse(text5), 2);
                        dataRow["extcredits5"] = Math.Round(double.Parse(text6), 2);
                        dataRow["extcredits6"] = Math.Round(double.Parse(text7), 2);
                        dataRow["extcredits7"] = Math.Round(double.Parse(text8), 2);
                        dataRow["extcredits8"] = Math.Round(double.Parse(text9), 2);
                    }
                }
                if (num2 > 2)
                {
                    dataRow["extcredits1"] = Math.Round(double.Parse(dataRow["extcredits1"].ToString()), 2);
                    dataRow["extcredits2"] = Math.Round(double.Parse(dataRow["extcredits2"].ToString()), 2);
                    dataRow["extcredits3"] = Math.Round(double.Parse(dataRow["extcredits3"].ToString()), 2);
                    dataRow["extcredits4"] = Math.Round(double.Parse(dataRow["extcredits4"].ToString()), 2);
                    dataRow["extcredits5"] = Math.Round(double.Parse(dataRow["extcredits5"].ToString()), 2);
                    dataRow["extcredits6"] = Math.Round(double.Parse(dataRow["extcredits6"].ToString()), 2);
                    dataRow["extcredits7"] = Math.Round(double.Parse(dataRow["extcredits7"].ToString()), 2);
                    dataRow["extcredits8"] = Math.Round(double.Parse(dataRow["extcredits8"].ToString()), 2);
                }
            }
            try
            {
                this.dsSrc.WriteXml(base.Server.MapPath("../../config/scoreset.config"));
                this.dsSrc.Reset();
                this.dsSrc.Dispose();

                XCache.Remove(CacheKeys.FORUM_SCORE_PAY_SET);
                XCache.Remove("/Forum/ScoreSet");
                XCache.Remove(CacheKeys.FORUM_VALID_SCORE_NAME);
                XCache.Remove(CacheKeys.FORUM_VALID_SCORE_UNIT);
                XCache.Remove(CacheKeys.FORUM_RATESCORESET);
                XCache.Remove("/Forum/IsSetDownLoadAttachScore");
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "积分设置", "");
                this.DataGrid1.EditItemIndex = -1;
                this.dsSrc = new DataSet();
                this.dsSrc.ReadXml(base.Server.MapPath("../../config/scoreset.config"));
                this.DataGrid1.DataSource = this.dsSrc.Tables[0];
                this.DataGrid1.DataBind();
            }
            catch
            {
                base.RegisterStartupScript("DataGrid1", "<script>alert('无法更新数据库.');window.location.href='global_scoreset.aspx';</script>");
                return;
            }
            if (num > 2)
            {
                Regex regex = new Regex("^\\d+(\\.\\d{1,2})?$");
                if (!regex.IsMatch(text2.Replace("-", "")) || !regex.IsMatch(text3.Replace("-", "")) || !regex.IsMatch(text4.Replace("-", "")) || !regex.IsMatch(text5.Replace("-", "")) || !regex.IsMatch(text6.Replace("-", "")) || !regex.IsMatch(text7.Replace("-", "")) || !regex.IsMatch(text8.Replace("-", "")) || !regex.IsMatch(text9.Replace("-", "")))
                {
                    base.RegisterStartupScript("DataGrid1", "<script>alert('当前数据项最多只能为小位点后两位,系统将四舍五入该数据.');window.location.href='global_scoreset.aspx';</script>");
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Convert.ToDouble(this.creditstax.Text.Trim()) > 1.0 || Convert.ToDouble(this.creditstax.Text.Trim()) < 0.0)
                {
                    base.RegisterStartupScript("", "<script>alert('积分交易税必须是0--1之间的小数');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                if (Convert.ToDouble(this.transfermincredits.Text.Trim()) < 0.0)
                {
                    base.RegisterStartupScript("", "<script>alert('转账最低余额必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                if (Convert.ToDouble(this.exchangemincredits.Text.Trim()) < 0.0)
                {
                    base.RegisterStartupScript("", "<script>alert('兑换最低余额必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                if (Convert.ToDouble(this.maxincperthread.Text.Trim()) < 0.0)
                {
                    base.RegisterStartupScript("", "<script>alert('单主题最高收入必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                if (Convert.ToDouble(this.maxchargespan.Text.Trim()) < 0.0)
                {
                    base.RegisterStartupScript("", "<script>alert('单主题最高出售时限必须是大于或等于0');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                //if (String.IsNullOrEmpty(this.formula.Text.Trim()) || !AdminForums.CreateUpdateUserCreditsProcedure(this.formula.Text.Trim()))
                // 原来计算积分的公式作为存储过程写入，现在暂时固定编码到User.UpdateUserCredits
                if (String.IsNullOrEmpty(this.formula.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('总积分计算公式为空或不正确');window.location.href='global_scoreset.aspx';</script>");
                    return;
                }
                if (this.losslessdel.Text.ToInt() > 9999 || this.losslessdel.Text.ToInt() < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('删帖不减积分时间期限只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                    return;
                }
                this.dsSrc.ReadXml(base.Server.MapPath("../../config/scoreset.config"));
                this.dsSrc.Tables["formula"].Rows[0]["formulacontext"] = this.formula.Text.Trim();
                this.dsSrc.Tables["formula"].Rows[0]["creditstrans"] = this.creditstrans.SelectedValue;
                if (this.creditstrans.SelectedValue == "0")
                {
                    this.dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"] = this.creditstrans.SelectedValue;
                    this.dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"] = this.creditstrans.SelectedValue;
                }
                else
                {
                    this.dsSrc.Tables["formula"].Rows[0]["topicattachcreditstrans"] = this.topicattachcreditstrans.SelectedValue;
                    this.dsSrc.Tables["formula"].Rows[0]["bonuscreditstrans"] = this.bonuscreditstrans.SelectedValue;
                }
                this.dsSrc.Tables["formula"].Rows[0]["creditstax"] = Convert.ToDouble(this.creditstax.Text);
                this.dsSrc.Tables["formula"].Rows[0]["transfermincredits"] = Convert.ToDouble(this.transfermincredits.Text);
                this.dsSrc.Tables["formula"].Rows[0]["exchangemincredits"] = Convert.ToDouble(this.exchangemincredits.Text);
                this.dsSrc.Tables["formula"].Rows[0]["maxincperthread"] = Convert.ToDouble(this.maxincperthread.Text);
                this.dsSrc.Tables["formula"].Rows[0]["maxchargespan"] = Convert.ToDouble(this.maxchargespan.Text);
                this.dsSrc.WriteXml(base.Server.MapPath("../../config/scoreset.config"));

                XCache.Remove(CacheKeys.FORUM_SCORESET);
                XCache.Remove(CacheKeys.FORUM_SCORESET_CREDITS_TRANS);
                XCache.Remove("/Forum/Scoreset//Forum/Scoreset/TopicAttachCreditsTrans");
                XCache.Remove("/Forum/Scoreset/BonusCreditsTrans");
                XCache.Remove(CacheKeys.FORUM_SCORESET_CREDITS_TAX);
                XCache.Remove(CacheKeys.FORUM_SCORESET_TRANSFER_MIN_CREDITS);
                XCache.Remove(CacheKeys.FORUM_SCORESET_EXCHANGE_MIN_CREDITS);
                XCache.Remove(CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD);
                XCache.Remove(CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN);
                XCache.Remove("/Forum/IsSetDownLoadAttachScore");
                XCache.Remove(CacheKeys.FORUM_VALID_SCORE_UNIT);
                XCache.Remove(CacheKeys.FORUM_RATESCORESET);

                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "修改积分设置", "修改积分设置");
                this.configInfo.Alipayaccout = DNTRequest.GetFormString("alipayaccount");
                this.configInfo.Cashtocreditrate = DNTRequest.GetFormInt("cashtocreditsrate", 0);
                int num = DNTRequest.GetFormInt("mincreditstobuy", 0);
                if (this.configInfo.Cashtocreditrate > 0)
                {
                    while (num / this.configInfo.Cashtocreditrate < 0.10m)
                    {
                        num++;
                    }
                }
                this.configInfo.Mincreditstobuy = num;
                this.configInfo.Maxcreditstobuy = DNTRequest.GetFormInt("maxcreditstobuy", 0);
                this.configInfo.Userbuycreditscountperday = DNTRequest.GetFormInt("userbuycreditscountperday", 0);
                this.configInfo.Alipaypartnercheckkey = DNTRequest.GetFormString("alipaypartnercheckkey");
                this.configInfo.Alipaypartnerid = DNTRequest.GetFormString("alipaypartnerid");
                this.configInfo.Usealipaycustompartnerid = DNTRequest.GetFormInt("usealipaycustompartnerid", 1);
                this.configInfo.Usealipayinstantpay = DNTRequest.GetFormInt("usealipayinstantpay", 0);
                this.configInfo.Losslessdel = (int)Convert.ToInt16(this.losslessdel.Text);

                //GeneralConfigs.SaveConfig(this.configInfo);
                //GeneralConfigs.ResetConfig();
                configInfo.Save();
                GeneralConfigInfo.Current = null;
                if (this.RefreshUserScore.SelectedValue.IndexOf("1") == 0)
                {
                    base.ClientScript.RegisterStartupScript(base.GetType(), "Page", "<script>submit_Click();</script>");
                    return;
                }
                base.RegisterStartupScript("PAGE", "window.location.href='global_scoreset.aspx';");
            }
        }

        protected void DataGrid_Delete(object sender, DataGridCommandEventArgs E)
        {
            this.DataGrid1.DataKeys[E.Item.ItemIndex].ToString();
        }

        protected void DataGrid_Edit(object sender, DataGridCommandEventArgs E)
        {
            this.DataGrid1.EditItemIndex = E.Item.ItemIndex;
            this.dsSrc.Clear();
            this.dsSrc.ReadXml(base.Server.MapPath("../../config/scoreset.config"));
            this.DataGrid1.DataSource = this.dsSrc.Tables[0];
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_Cancel(object sender, DataGridCommandEventArgs E)
        {
            this.DataGrid1.EditItemIndex = -1;
            this.DataGrid1.DataSource = Scoresets.GetScoreSet();
            this.DataGrid1.DataBind();
        }

        public string GetImgLink(string img)
        {
            if (img != "")
            {
                return "<img src=../../images/groupicons/" + img + ">";
            }
            return "";
        }

        public void TestAccout(string accout)
        {
            int @int = DNTRequest.GetInt("openpartner", 0);
            string @string = Request["partnerid"];
            string string2 = Request["partnerKey"];
            DigitalTrade digitalTrade = new DigitalTrade();
            digitalTrade.Subject = "测试支付宝充值功能";
            if (Utils.IsValidEmail(accout))
            {
                digitalTrade.Seller_Email = accout;
            }
            else
            {
                digitalTrade.Seller_Id = accout;
            }
            digitalTrade.Return_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            digitalTrade.Notify_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            digitalTrade.Quantity = 1;
            digitalTrade.Price = 0.1m;
            digitalTrade.Payment_Type = 1;
            digitalTrade.PayMethod = "bankPay";
            string url;
            if (@int == 1)
            {
                digitalTrade.Partner = @string;
                digitalTrade.Sign = string2;
                url = StandardAliPayment.GetService().CreateDigitalGoodsTradeUrl(digitalTrade);
            }
            else
            {
                url = AliPayment.GetService().CreateDigitalGoodsTradeUrl(digitalTrade);
            }
            HttpContext.Current.Response.Redirect(url);
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(this.DataGrid_Cancel);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.Save.Click += new EventHandler(this.Save_Click);
            this.formula.IsReplaceInvertedComma = false;
            this.DataGrid1.LoadEditColumn();
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.AllowPaging = false;
            this.DataGrid1.ShowFooter = false;
        }
    }
}