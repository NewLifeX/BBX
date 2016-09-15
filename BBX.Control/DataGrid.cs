using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;

namespace BBX.Control
{
    [DefaultProperty("Text"), ToolboxData("<{0}:DataGrid runat=server></{0}:DataGrid>")]
    public class DataGrid : System.Web.UI.WebControls.DataGrid, INamingContainer
    {
        public Button GoToPagerButton = new Button();
        public HtmlInputText GoToPagerInputText = new HtmlInputText();
        private string sort;

        [Category("Appearance"), DefaultValue("ASC"), Description("表头的名称。")]
        public string DataGridSortType
        {
            get
            {
                object obj = this.ViewState["DataGridSortType"];
                string a = (obj == null) ? "ASC" : ((string)obj);
                if (a == "ASC")
                {
                    this.ViewState["DataGridSortType"] = "DESC";
                    return "DESC";
                }
                this.ViewState["DataGridSortType"] = "ASC";
                return "ASC";
            }
            set { ViewState["DataGridSortType"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Sort
        {
            get { return sort; }
            set
            {
                this.sort = value;
                this.SortTable(this.sort, null);
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public bool SaveDSViewState
        {
            get
            {
                object obj = this.ViewState["SaveDSViewState"];
                return obj != null && obj.ToString().ToLower() == "true";
            }
            set { ViewState["SaveDSViewState"] = value; }
        }

        public DataTable SourceDataTable { get { return (DataTable)this.ViewState["SourceDataTable"]; } set { ViewState["SourceDataTable"] = value; } }

        public string SqlText { get { return (string)this.ViewState["SqlText"]; } set { ViewState["SqlText"] = value; } }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public int ColumnSpan
        {
            get
            {
                object obj = this.ViewState["ColumnSpan"];
                if (obj != null)
                {
                    return (int)obj;
                }
                return 1;
            }
            set { ViewState["ColumnSpan"] = value; }
        }

        [Category("Appearance"), DefaultValue(""), Description("表名称。")]
        public string TableHeaderName
        {
            get
            {
                object obj = this.ViewState["TableHeaderName"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "1";
            }
            set { ViewState["TableHeaderName"] = value; }
        }

        [Category("Appearance"), DefaultValue(""), Description("表头的名称。")]
        public string ImagePath
        {
            get
            {
                object obj = this.ViewState["ImagePath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../images/";
            }
            set { ViewState["ImagePath"] = value; }
        }

        [Category("Appearance"), DefaultValue(false), Description("是否在列表页中载入列控件")]
        public bool IsFixConlumnControls
        {
            get
            {
                object obj = this.ViewState["IsFixConlumnControls"];
                return obj != null && obj.ToString().ToLower() == "true";
            }
            set { ViewState["IsFixConlumnControls"] = value; }
        }

        public override object DataSource
        {
            get { return base.DataSource; }
            set
            {
                base.DataSource = value;
                if (!this.AllowCustomPaging)
                {
                    if (value is DataTable)
                    {
                        this.VirtualItemCount = (value as DataTable).Rows.Count;
                    }
                    if (value is DataSet)
                    {
                        DataSet dataSet = value as DataSet;
                        if (dataSet.Tables.Count > 0)
                        {
                            this.VirtualItemCount = dataSet.Tables[0].Rows.Count;
                        }
                    }
                    if (value.GetType().Name.ToString().IndexOf("[]") > 0)
                    {
                        Array array = value as Array;
                        if (array.Length > 0)
                        {
                            this.VirtualItemCount = array.Length;
                        }
                    }
                }
            }
        }

        public DataGrid()
        {
            this.CssClass = "datalist";
            this.ShowHeader = true;
            this.AutoGenerateColumns = false;
            this.SelectedItemStyle.CssClass = "datagridSelectedItem";
            this.ItemStyle.CssClass = "";
            this.HeaderStyle.CssClass = "category";
            this.PageSize = 25;
            this.PagerStyle.Mode = PagerMode.NumericPages;
            this.AllowCustomPaging = false;
            this.AllowPaging = true;
            this.AllowSorting = true;
            this.DataKeyField = "ID";
            this.GoToPagerInputText.Attributes.Add("onfocus", "this.className='colorfocus';");
            this.GoToPagerInputText.Attributes.Add("onblur", "this.className='colorblur';");
            this.GoToPagerInputText.Attributes.Add("CssClass", "colorblur");
            base.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            base.ItemCreated += new DataGridItemEventHandler(this.DataGrid_ItemCreated);
            base.SortCommand += new DataGridSortCommandEventHandler(this.SortGrid);
            this.GoToPagerButton.Click += new EventHandler(this.GoToPagerButton_Click);
        }

        //public DataGrid(string sqlstring)
        //    : this()
        //{
        //    this.BindData(sqlstring);
        //}

        public void LoadDefaultColumn()
        {
            this.LoadEditColumn();
            this.LoadDeleteColumn();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            if (this.GoToPagerInputText.Value != "")
            {
                if (!this.AllowCustomPaging)
                {
                    if (!Regex.IsMatch(this.GoToPagerInputText.Value, "^\\d+?\\d*$"))
                    {
                        this.LoadCurrentPageIndex(0);
                        return;
                    }
                    int num = Utils.StrToInt(this.GoToPagerInputText.Value.Trim(), 1);
                    if (num < 0)
                    {
                        this.LoadCurrentPageIndex(0);
                        return;
                    }
                    if (num > base.PageCount)
                    {
                        this.LoadCurrentPageIndex(base.PageCount - 1);
                        return;
                    }
                    this.LoadCurrentPageIndex(num - 1);
                    return;
                }
                else
                {
                    this.SetCurrentPageIndexByGoToPager();
                }
            }
        }

        private void SetCurrentPageIndexByGoToPager()
        {
            if (this.GoToPagerInputText.Value != "")
            {
                if (!Regex.IsMatch(this.GoToPagerInputText.Value, "^\\d+?\\d*$"))
                {
                    base.CurrentPageIndex = 0;
                    return;
                }
                int num = Utils.StrToInt(this.GoToPagerInputText.Value.Trim(), 1);
                if (num < 0)
                {
                    base.CurrentPageIndex = 0;
                    return;
                }
                if (num > base.PageCount)
                {
                    base.CurrentPageIndex = base.PageCount - 1;
                    return;
                }
                base.CurrentPageIndex = num - 1;
            }
        }

        public void LoadEditColumn()
        {
            EditCommandColumn editCommandColumn = new EditCommandColumn();
            editCommandColumn.SortExpression = "desc";
            editCommandColumn.ButtonType = ButtonColumnType.LinkButton;
            editCommandColumn.EditText = "编辑";
            editCommandColumn.UpdateText = "更新";
            editCommandColumn.CancelText = "取消";
            editCommandColumn.ItemStyle.Width = 70;
            editCommandColumn.ItemStyle.BorderWidth = 1;
            editCommandColumn.ItemStyle.BorderStyle = BorderStyle.Solid;
            editCommandColumn.ItemStyle.BorderColor = Color.FromArgb(234, 233, 225);
            this.Columns.AddAt(0, editCommandColumn);
        }

        public void LoadDeleteColumn()
        {
            ButtonColumn buttonColumn = new ButtonColumn();
            buttonColumn.SortExpression = "desc";
            buttonColumn.CommandName = "Delete";
            buttonColumn.Text = "删除";
            buttonColumn.ItemStyle.Width = 70;
            this.Columns.AddAt(1, buttonColumn);
        }

        public void BindData()
        {
            if (this.SourceDataTable != null)
            {
                this.BindData(this.SourceDataTable);
            }
        }

        //public void BindData(string sqlstring)
        //{
        //    this.SqlText = sqlstring;
        //    DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        //    this.BindData(dt);
        //}

        public void BindData(DataTable dt)
        {
            this.SourceDataTable = dt;
            this.VirtualItemCount = dt.Rows.Count;
            this.DataSource = dt;
            this.DataBind();
        }

        public void BindData<T>(List<T> list)
        {
            this.VirtualItemCount = list.Count;
            this.DataSource = list;
            this.DataBind();
        }

        protected void SortGrid(object sender, DataGridSortCommandEventArgs e)
        {
            this.SortTable(e.SortExpression, null);
            foreach (DataGridColumn dataGridColumn in this.Columns)
            {
                if (dataGridColumn.SortExpression == e.SortExpression)
                {
                    if (dataGridColumn.HeaderText.IndexOf("<img src=") >= 0)
                    {
                        if (this.DataGridSortType == "ASC")
                        {
                            dataGridColumn.HeaderText = dataGridColumn.HeaderText.Replace("<img src=" + this.ImagePath + "asc.gif height=13>", "<img src=" + this.ImagePath + "desc.gif height=13>");
                        }
                        else
                        {
                            dataGridColumn.HeaderText = dataGridColumn.HeaderText.Replace("<img src=" + this.ImagePath + "desc.gif height=13>", "<img src=" + this.ImagePath + "asc.gif height=13>");
                        }
                    }
                    else
                    {
                        if (this.DataGridSortType == "ASC")
                        {
                            dataGridColumn.HeaderText = dataGridColumn.HeaderText + "<img src=" + this.ImagePath + "desc.gif height=13>";
                        }
                        else
                        {
                            dataGridColumn.HeaderText = dataGridColumn.HeaderText + "<img src=" + this.ImagePath + "asc.gif height=13>";
                        }
                    }
                }
                else
                {
                    dataGridColumn.HeaderText = dataGridColumn.HeaderText.Replace("<img", "~").Split('~')[0];
                }
            }
        }

        public void SortTable(string SortExpression, DataTable dt)
        {
            DataView dataView = new DataView();
            if (dt != null && dt.Rows.Count > 0)
            {
                dataView = new DataView(dt);
            }
            else
            {
                if (this.SourceDataTable == null)
                {
                    return;
                }
                dataView = new DataView(this.SourceDataTable);
            }
            dataView.Sort = SortExpression.Replace("<img", "~").Split('~')[0] + " " + this.DataGridSortType;
            this.DataSource = dataView;
            this.DataBind();
        }

        //public void SortTable2(string SortExpression, string sqlstring)
        //{
        //    DataView dataView = new DataView();
        //    if (sqlstring != null && sqlstring != "")
        //    {
        //        this.DataSource = new DataView(DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0])
        //        {
        //            Sort = SortExpression.Replace("<img", "~").Split('~')[0] + " " + this.DataGridSortType
        //        };
        //        this.DataBind();
        //        return;
        //    }
        //}

        public void SortTable<T>(string sortExpression, List<T> list)
        {
            this.DataSource = list;
            this.Sort = sortExpression + " " + this.DataGridSortType;
            this.DataBind();
        }

        //public void DeleteByString(string sqlstring)
        //{
        //    DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring);
        //    this.EditItemIndex = -1;
        //    this.BindData();
        //}

        public void EditByItemIndex(int itemindex)
        {
            this.EditItemIndex = itemindex;
            this.BindData();
        }

        public void Cancel()
        {
            this.EditItemIndex = -1;
            this.BindData();
        }

        private void PageIndex(int pageindex)
        {
            this.BindData();
        }

        public void LoadCurrentPageIndex(int value)
        {
            base.CurrentPageIndex = ((value < 0) ? 0 : value);
            this.BindData();
        }

        public void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "this.className='mouseoverstyle'");
                e.Item.Attributes.Add("onmouseout", "this.className='mouseoutstyle'");
                e.Item.Style["cursor"] = "hand";
            }
            if (!this.SaveDSViewState)
            {
                this.Controls[0].EnableViewState = false;
            }
            if (this.IsFixConlumnControls)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        if (!e.Item.Cells[i].HasControls())
                        {
                            if (this.GetBoundColumnFieldReadOnly()[i].ToString().ToLower() == "false")
                            {
                                System.Web.UI.WebControls.TextBox textBox = new System.Web.UI.WebControls.TextBox();
                                textBox.ID = this.GetBoundColumnField()[i].ToString();
                                textBox.Attributes.Add("onmouseover", "if(this.className != 'FormFocus') this.className='FormBase'");
                                textBox.Attributes.Add("onmouseout", "if(this.className != 'FormFocus') this.className='formnoborder'");
                                textBox.Attributes.Add("onfocus", "this.className='FormFocus';");
                                textBox.Attributes.Add("onblur", "this.className='formnoborder';");
                                textBox.Attributes.Add("class", "formnoborder");
                                textBox.Text = e.Item.Cells[i].Text.Trim().Replace("&nbsp;", "");
                                if (this.Columns[i].ItemStyle.Width.Value > 0.0)
                                {
                                    textBox.Width = (int)this.Columns[i].ItemStyle.Width.Value;
                                }
                                e.Item.Cells[i].Controls.Add(textBox);
                            }
                        }
                        else
                        {
                            foreach (var control in e.Item.Cells[i].Controls)
                            {
                                if (control is DropDownList)
                                {
                                    DropDownList dropDownList = (DropDownList)control;
                                    try
                                    {
                                        dropDownList.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, dropDownList.DataValueField));
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (control is System.Web.UI.WebControls.DropDownList)
                                {
                                    System.Web.UI.WebControls.DropDownList dropDownList2 = (System.Web.UI.WebControls.DropDownList)control;
                                    try
                                    {
                                        dropDownList2.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, dropDownList2.DataValueField));
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
            }
            else
            {
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    for (int j = 0; j < e.Item.Cells.Count; j++)
                    {
                        e.Item.Cells[j].BorderWidth = 1;
                        e.Item.Cells[j].BorderStyle = BorderStyle.Solid;
                        e.Item.Cells[j].BorderColor = Color.FromArgb(234, 233, 225);
                        if (e.Item.Cells[j].HasControls())
                        {
                            for (int k = 0; k < e.Item.Cells[j].Controls.Count; k++)
                            {
                                System.Web.UI.WebControls.TextBox textBox2 = e.Item.Cells[j].Controls[k] as System.Web.UI.WebControls.TextBox;
                                if (textBox2 != null)
                                {
                                    textBox2.Attributes.Add("onfocus", "this.className='FormFocus';");
                                    textBox2.Attributes.Add("onblur", "this.className='FormBase';");
                                    textBox2.Attributes.Add("class", "FormBase");
                                }
                            }
                        }
                    }
                }
            }
        }

        public string LoadSelectedCheckBox(string keyid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<INPUT id=\"keyid\"  type=\"checkbox\" value=\"" + keyid + "\"\tname=\"keyid\" checked  style=\"display:none\">");
            return stringBuilder.ToString();
        }

        public ArrayList GetKeyIDArray()
        {
            ArrayList arrayList = new ArrayList();
            if (DNTRequest.GetString("keyid") != "")
            {
                string[] array = DNTRequest.GetString("keyid").Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string value = array[i];
                    arrayList.Add(value);
                }
            }
            return arrayList;
        }

        public string GetControlValue(int controlnumber, string fieldname)
        {
            return DNTRequest.GetFormString(this.ClientID.Replace("_", ":") + ":_ctl" + (controlnumber + 3) + ":" + fieldname);
        }

        public bool GetCheckBoxValue(int controlnumber, string fieldname)
        {
            string controlValue = this.GetControlValue(controlnumber, fieldname);
            return controlValue == "on";
        }

        private ArrayList GetBoundColumnField()
        {
            ArrayList arrayList = new ArrayList();
            foreach (DataGridColumn dataGridColumn in this.Columns)
            {
                BoundColumn boundColumn = dataGridColumn as BoundColumn;
                if (boundColumn != null)
                {
                    arrayList.Add(boundColumn.DataField);
                }
                else
                {
                    TemplateColumn templateColumn = dataGridColumn as TemplateColumn;
                    arrayList.Add(templateColumn.HeaderText);
                }
            }
            return arrayList;
        }

        private ArrayList GetBoundColumnFieldReadOnly()
        {
            ArrayList arrayList = new ArrayList();
            foreach (DataGridColumn dataGridColumn in this.Columns)
            {
                BoundColumn boundColumn = dataGridColumn as BoundColumn;
                if (boundColumn != null)
                {
                    arrayList.Add(boundColumn.ReadOnly);
                }
                else
                {
                    arrayList.Add(true);
                }
            }
            return arrayList;
        }

        public void DataGrid_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            ListItemType itemType = e.Item.ItemType;
            if (itemType == ListItemType.Pager)
            {
                TableCell tableCell = (TableCell)e.Item.Controls[0];
                tableCell.HorizontalAlign = HorizontalAlign.Left;
                tableCell.VerticalAlign = VerticalAlign.Bottom;
                tableCell.CssClass = "datagridPager";
                LiteralControl literalControl = new LiteralControl("splittable");
                literalControl.Text = "</td></tr></table><table class=\"datagridpage\"><tr><td height=\"2\"></td></tr><tr><td>";
                tableCell.Controls.AddAt(0, literalControl);
                LiteralControl literalControl2 = new LiteralControl("PageNumber");
                literalControl2.Text = " ";
                if (base.PageCount <= 1)
                {
                    try
                    {
                        tableCell.Controls.RemoveAt(1);
                        goto IL_A1;
                    }
                    catch
                    {
                        goto IL_A1;
                    }
                }
                literalControl2.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            IL_A1:
                LiteralControl expr_A2 = literalControl2;
                object text = expr_A2.Text;
                expr_A2.Text = text + "<font color=black>共 " + base.PageCount + " 页, 当前第 " + (base.CurrentPageIndex + 1) + " 页";
                if (this.VirtualItemCount > 0)
                {
                    LiteralControl expr_109 = literalControl2;
                    object text2 = expr_109.Text;
                    expr_109.Text = text2 + ", 共 " + this.VirtualItemCount + " 条记录";
                }
                LiteralControl expr_14D = literalControl2;
                expr_14D.Text = expr_14D.Text + "    &nbsp;&nbsp;" + ((base.PageCount > 1) ? "跳转到:" : "");
                tableCell.Controls.Add(literalControl2);
                if (base.PageCount > 1)
                {
                    this.GoToPagerInputText.ID = "GoToPagerInputText";
                    this.GoToPagerInputText.Attributes.Add("runat", "server");
                    this.GoToPagerInputText.Attributes.Add("onkeydown", "if(event.keyCode==13) { var gotoPageID=this.name.replace('InputText','Button'); return(document.getElementById(gotoPageID)).focus();}");
                    this.GoToPagerInputText.Size = 6;
                    this.GoToPagerInputText.Value = ((base.CurrentPageIndex == 0) ? "1" : (base.CurrentPageIndex + 1).ToString());
                    tableCell.Controls.Add(this.GoToPagerInputText);
                    literalControl2 = new LiteralControl("PageNumber");
                    literalControl2.Text = "页&nbsp;&nbsp;";
                    tableCell.Controls.Add(literalControl2);
                    this.GoToPagerButton.ID = "GoToPagerButton";
                    this.GoToPagerButton.Text = " Go ";
                    tableCell.Controls.Add(this.GoToPagerButton);
                }
                e.Item.Controls.Add(tableCell);
                TableCell tableCell2 = (TableCell)e.Item.Controls[0];
                int i = 1;
                while (i < tableCell2.Controls.Count)
                {
                    object obj = tableCell2.Controls[i];
                    if (!(obj is LinkButton))
                    {
                        goto IL_310;
                    }
                    LinkButton linkButton = (LinkButton)obj;
                    if (linkButton.Text == "..." && i == 1)
                    {
                        linkButton.Text = "上一页";
                    }
                    else
                    {
                        if (i <= 1 || !(linkButton.Text == "..."))
                        {
                            goto IL_310;
                        }
                        linkButton.Text = "下一页";
                    }
                IL_36A:
                    i += 2;
                    continue;
                IL_310:
                    if (!(obj is Label))
                    {
                        goto IL_36A;
                    }
                    Label label = (Label)obj;
                    if (label.Text == "..." && i == 1)
                    {
                        label.Text = "上一页";
                    }
                    if (i > 1 && label.Text == "...")
                    {
                        label.Text = "下一页";
                        goto IL_36A;
                    }
                    goto IL_36A;
                }
                return;
            }
            if (itemType == ListItemType.AlternatingItem || itemType == ListItemType.Item || itemType == ListItemType.Header)
            {
                foreach (var control in e.Item.Controls)
                {
                    var tableCell3 = (TableCell)control;
                    tableCell3.BorderWidth = 1;
                    tableCell3.BorderColor = Color.FromArgb(234, 233, 225);
                }
            }
            for (int j = 0; j < e.Item.Cells.Count; j++)
            {
                if (itemType == ListItemType.Header)
                {
                    e.Item.Cells[j].Wrap = false;
                }
                else
                {
                    if (j >= 2)
                    {
                        e.Item.Cells[j].Wrap = true;
                    }
                    else
                    {
                        e.Item.Cells[j].Wrap = false;
                    }
                }
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.WriteLine("<table class=\"ntcplist\" >\r\n");
            output.WriteLine("<tr class=\"head\">\r\n");
            output.WriteLine("<td>" + this.TableHeaderName + "</td>\r\n");
            output.WriteLine("</tr>\r\n");
            output.WriteLine("<tr>\r\n");
            output.WriteLine("<td>\r\n");
            base.Render(output);
            output.WriteLine("</td></tr></TABLE>");
        }
    }
}