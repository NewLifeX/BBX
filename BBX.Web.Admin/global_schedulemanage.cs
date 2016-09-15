using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Config;
using BBX.Entity;
using BBX.Forum.ScheduledEvents;
using NewLife;

namespace BBX.Web.Admin
{
    public class global_schedulemanage : AdminPage
    {
        protected HtmlForm form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
        protected HtmlGenericControl passportbody;
        protected DataGrid DataGrid1;

        protected void Page_Load(object sender, EventArgs e)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("key");
            dataTable.Columns.Add("scheduletype");
            dataTable.Columns.Add("exetime");
            dataTable.Columns.Add("lastexecute");
            dataTable.Columns.Add("issystemevent");
            dataTable.Columns.Add("enable");
            var events = ScheduleConfigInfo.Current.Events;
            for (int i = 0; i < events.Length; i++)
            {
                var ev = events[i];
                DataRow dataRow = dataTable.NewRow();
                dataRow["key"] = ev.Key;
                dataRow["scheduletype"] = ev.ScheduleType;
                if (ev.TimeOfDay != -1)
                {
                    dataRow["exetime"] = "定时执行:" + ev.TimeOfDay / 60 + "时" + ev.TimeOfDay % 60 + "分";
                }
                else
                {
                    dataRow["exetime"] = "周期执行:" + ev.Minutes + "分钟";
                }
                //DateTime lastExecuteScheduledEventDateTime = BBX.Forum.ScheduledEvents.Event.GetLastExecuteScheduledEventDateTime(@event.Key, Environment.MachineName);
                DateTime lastExecuteScheduledEventDateTime = ScheduledEvent.GetLast(ev.Key, Environment.MachineName);
                if (lastExecuteScheduledEventDateTime == DateTime.MinValue)
                {
                    dataRow["lastexecute"] = "从未执行";
                }
                else
                {
                    dataRow["lastexecute"] = lastExecuteScheduledEventDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dataRow["issystemevent"] = (ev.IsSystemEvent ? "系统级" : "非系统级");
                dataRow["enable"] = (ev.Enabled ? "启用" : "禁用");
                dataTable.Rows.Add(dataRow);
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataKeyField = "key";
            this.DataGrid1.DataBind();
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "exec")
            {
                var events = ScheduleConfigInfo.Current.Events;
                for (int i = 0; i < events.Length; i++)
                {
                    var ev = events[i];
                    if (ev.Key == e.CommandArgument.ToString())
                    {
                        var type = Type.GetType(ev.ScheduleType);
                        if (type == null) throw new XException("无法找到计划任务类型[{0}]！", ev.ScheduleType);
                        ((IEvent)Activator.CreateInstance(type)).Execute(HttpContext.Current);
                        //BBX.Forum.ScheduledEvents.Event.SetLastExecuteScheduledEventDateTime(ev.Key, Environment.MachineName, DateTime.Now);
                        ScheduledEvent.SetLast(ev.Key, Environment.MachineName, DateTime.Now);
                        return;
                    }
                }
            }
        }

        public void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "this.className='mouseoverstyle'");
                e.Item.Attributes.Add("onmouseout", "this.className='mouseoutstyle'");
                e.Item.Style["cursor"] = "hand";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.CssClass = "datalist";
            this.DataGrid1.ShowHeader = true;
            this.DataGrid1.AutoGenerateColumns = false;
            this.DataGrid1.SelectedItemStyle.CssClass = "datagridSelectedItem";
            this.DataGrid1.HeaderStyle.CssClass = "category";
            this.DataGrid1.AutoGenerateColumns = false;
            this.DataGrid1.BorderWidth = 1;
            this.DataGrid1.BorderStyle = BorderStyle.Solid;
            this.DataGrid1.BorderColor = Color.FromArgb(234, 233, 225);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
        }
    }
}