using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class global_schedulesetting : AdminPage
    {
        protected HtmlForm form1;
        protected pageinfo PageInfo1;
        protected BBX.Control.TextBox key;
        protected HtmlInputHidden oldkey;
        protected BBX.Control.TextBox scheduletype;
        protected RadioButton type1;
        protected BBX.Control.DropDownList hour;
        protected BBX.Control.DropDownList minute;
        protected RadioButton type2;
        protected BBX.Control.TextBox timeserval;
        protected HtmlTableRow eventenabletr;
        protected BBX.Control.RadioButtonList eventenable;
        protected HtmlInputHidden apikeyhidd;
        protected BBX.Control.Button savepassportinfo;
        protected Hint hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.type1.Attributes.Add("onclick", "changetimespan(this.value)");
                this.type2.Attributes.Add("onclick", "changetimespan(this.value)");
                for (int i = 0; i < 24; i++)
                {
                    this.hour.Items.Add(new ListItem(i.ToString("00"), i.ToString()));
                }
                for (int j = 0; j < 60; j++)
                {
                    this.minute.Items.Add(new ListItem(j.ToString("00"), j.ToString()));
                }
                if (Request["keyid"] != "")
                {
                    var config = ScheduleConfigInfo.Current;
                    var events = config.Events;
                    for (int k = 0; k < events.Length; k++)
                    {
                        var ev = events[k];
                        if (ev.Key == Request["keyid"])
                        {
                            this.oldkey.Value = ev.Key;
                            this.key.Text = ev.Key;
                            this.key.Enabled = !ev.IsSystemEvent;
                            this.scheduletype.Text = ev.ScheduleType;
                            this.scheduletype.Enabled = !ev.IsSystemEvent;
                            this.timeserval.HintInfo = "设置执行时间间隔,最小值:" + config.TimerMinutesInterval + "分钟.如果设置值小于最小值,将取最小值";
                            if (ev.TimeOfDay != -1)
                            {
                                int num = ev.TimeOfDay / 60;
                                int num2 = ev.TimeOfDay % 60;
                                this.type1.Checked = true;
                                this.hour.SelectedValue = num.ToString();
                                this.minute.SelectedValue = num2.ToString();
                                this.hour.Enabled = true;
                                this.minute.Enabled = true;
                                this.timeserval.Enabled = false;
                            }
                            else
                            {
                                this.type2.Checked = true;
                                this.timeserval.Text = ev.Minutes.ToString();
                                this.hour.Enabled = false;
                                this.minute.Enabled = false;
                                this.timeserval.Enabled = true;
                            }
                            if (!ev.IsSystemEvent)
                            {
                                if (ev.Enabled)
                                {
                                    this.eventenable.Items[0].Selected = true;
                                }
                                else
                                {
                                    this.eventenable.Items[1].Selected = true;
                                }
                            }
                            else
                            {
                                this.eventenable.Items[0].Selected = true;
                                this.eventenable.Enabled = false;
                            }
                        }
                    }
                }
            }
        }

        protected void savepassportinfo_Click(object sender, EventArgs e)
        {
            var config = ScheduleConfigInfo.Current;
            if (String.IsNullOrEmpty(this.key.Text.Trim()))
            {
                base.RegisterStartupScript("PAGE", "alert('计划任务名称不能为空!');");
                return;
            }
            if (String.IsNullOrEmpty(this.scheduletype.Text.Trim()))
            {
                base.RegisterStartupScript("PAGE", "alert('计划任务类型不能为空!');");
                return;
            }
            if (this.type2.Checked && (String.IsNullOrEmpty(this.timeserval.Text) || !Utils.IsNumeric(this.timeserval.Text)))
            {
                base.RegisterStartupScript("PAGE", "alert('周期执行时间必须为数值!');");
                return;
            }
            if (String.IsNullOrEmpty(Request["keyid"]))
            {
                var events = config.Events;
                for (int i = 0; i < events.Length; i++)
                {
                    BBX.Config.Event ev = events[i];
                    if (ev.Key == this.key.Text.Trim())
                    {
                        base.RegisterStartupScript("PAGE", "alert('计划任务名称已经存在!');");
                        return;
                    }
                }
                var event2 = new BBX.Config.Event();
                event2.Key = this.key.Text;
                event2.Enabled = true;
                event2.IsSystemEvent = false;
                event2.ScheduleType = this.scheduletype.Text.Trim();
                if (this.type1.Checked)
                {
                    event2.TimeOfDay = int.Parse(this.hour.Text) * 60 + int.Parse(this.minute.Text);
                    event2.Minutes = config.TimerMinutesInterval;
                }
                else
                {
                    event2.Minutes = int.Parse(this.timeserval.Text.Trim());
                    event2.TimeOfDay = -1;
                }
                BBX.Config.Event[] array = new BBX.Config.Event[config.Events.Length + 1];
                for (int j = 0; j < config.Events.Length; j++)
                {
                    array[j] = config.Events[j];
                }
                array[array.Length - 1] = event2;
                config.Events = array;
            }
            else
            {
                BBX.Config.Event[] events2 = config.Events;
                for (int k = 0; k < events2.Length; k++)
                {
                    BBX.Config.Event event3 = events2[k];
                    if (this.key.Text.Trim() != this.oldkey.Value && event3.Key == this.key.Text.Trim())
                    {
                        base.RegisterStartupScript("PAGE", "alert('计划任务名称已经存在!');");
                        return;
                    }
                }
                BBX.Config.Event[] events3 = config.Events;
                int l = 0;
                while (l < events3.Length)
                {
                    BBX.Config.Event event4 = events3[l];
                    if (event4.Key == this.oldkey.Value)
                    {
                        event4.Key = this.key.Text.Trim();
                        event4.ScheduleType = this.scheduletype.Text.Trim();
                        if (this.type1.Checked)
                        {
                            event4.TimeOfDay = int.Parse(this.hour.Text) * 60 + int.Parse(this.minute.Text);
                            event4.Minutes = config.TimerMinutesInterval;
                        }
                        else
                        {
                            if (int.Parse(this.timeserval.Text.Trim()) < config.TimerMinutesInterval)
                            {
                                event4.Minutes = config.TimerMinutesInterval;
                            }
                            else
                            {
                                event4.Minutes = int.Parse(this.timeserval.Text.Trim());
                            }
                            event4.TimeOfDay = -1;
                        }
                        if (event4.IsSystemEvent)
                        {
                            break;
                        }
                        if (this.eventenable.Items[0].Selected)
                        {
                            event4.Enabled = true;
                            break;
                        }
                        event4.Enabled = false;
                        break;
                    }
                    else
                    {
                        l++;
                    }
                }
            }

            //ScheduleConfigs.SaveConfig(config);
            config.Save();
            base.Response.Redirect("global_schedulemanage.aspx");
        }
    }
}