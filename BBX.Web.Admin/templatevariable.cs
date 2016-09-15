using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class templatevariable : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveVar;
        protected BBX.Control.Button DelRec;
        protected BBX.Control.TextBox variablename;
        protected BBX.Control.TextBox variablevalue;
        protected BBX.Control.Button AddNewRec;
        public DataSet dsSrc = new DataSet();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && Request["path"] != null && Request["path"] != "")
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.DataSource = this.LoadDataTable();
            this.DataGrid1.DataBind();
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void SaveVar_Click(object sender, EventArgs e)
        {
            this.dsSrc = this.LoadDataTable();
            int num = 0;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int num2 = int.Parse(current.ToString());
                string text = this.DataGrid1.GetControlValue(num, "variablename").Trim();
                string text2 = this.DataGrid1.GetControlValue(num, "variablevalue").Trim();
                if (!(String.IsNullOrEmpty(text)) && !(String.IsNullOrEmpty(text2)))
                {
                    foreach (DataRow dataRow in this.dsSrc.Tables["TemplateVariable"].Rows)
                    {
                        if (num2.ToString() == dataRow["id"].ToString())
                        {
                            dataRow["variablename"] = text;
                            dataRow["variablevalue"] = text2;
                            break;
                        }
                    }
                    try
                    {
                        if (this.dsSrc.Tables[0].Rows.Count == 0)
                        {
                            File.Delete(Utils.GetMapPath("../../templates/" + Request["path"] + "/templatevariable.xml"));
                            this.dsSrc.Reset();
                            this.dsSrc.Dispose();
                        }
                        else
                        {
                            string fileName = base.Server.MapPath("../../templates/" + Request["path"] + "/templatevariable.xml");
                            this.dsSrc.WriteXml(fileName);
                            this.dsSrc.Reset();
                            this.dsSrc.Dispose();

                            XCache.Remove("/Forum/" + Request["path"] + "/TemplateVariable");
                            base.RegisterStartupScript("PAGE", "window.location.href='global_templatevariable.aspx?templateid=" + Request["templateid"] + "&path=" + Request["path"] + "&templatename=" + Request["templatename"] + "';");
                        }
                    }
                    catch
                    {
                        base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_templatevariable.aspx?templateid=" + Request["templateid"] + "&path=" + Request["path"] + "&templatename=" + Request["templatename"] + "';</script>");
                        break;
                    }
                    num++;
                }
            }
        }

        protected void DelRec_Click(object sender, EventArgs e)
        {
            this.dsSrc = this.LoadDataTable();
            string @string = Request["delid"];
            string[] array = @string.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string a = array[i];
                foreach (DataRow dataRow in this.dsSrc.Tables["TemplateVariable"].Rows)
                {
                    if (a == dataRow["id"].ToString())
                    {
                        this.dsSrc.Tables[0].Rows.Remove(dataRow);
                        break;
                    }
                }
            }
            try
            {
                if (this.dsSrc.Tables[0].Rows.Count == 0)
                {
                    File.Delete(Utils.GetMapPath("../../templates/" + Request["path"] + "/templatevariable.xml"));
                    this.dsSrc.Reset();
                    this.dsSrc.Dispose();
                }
                else
                {
                    string fileName = base.Server.MapPath("../../templates/" + Request["path"] + "/templatevariable.xml");
                    this.dsSrc.WriteXml(fileName);
                    this.dsSrc.Reset();
                    this.dsSrc.Dispose();
                    //var cacheService = XCache.Current;
                    //cacheService.RetrieveObject("/Forum/" + Request["path"] + "/TemplateVariable");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_templatevariable.aspx?templateid=" + Request["templateid"] + "&path=" + Request["path"] + "&templatename=" + Request["templatename"] + "';");
                }
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_templatevariable.aspx?templateid=" + Request["templateid"] + "&path=" + Request["path"] + "&templatename=" + Request["templatename"] + "';</script>");
            }
        }

        public DataSet LoadDataTable()
        {
            string @string = Request["path"];
            XCache.Remove("/Forum/" + @string + "/TemplateVariable");
            DataSet dataSet = new DataSet();
            DataTable table = ForumPageTemplate.GetTemplateVarList(BaseConfigs.GetForumPath, @string).Copy();
            dataSet.Tables.Add(table);
            return dataSet;
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            this.dsSrc = this.LoadDataTable();
            DataRow dataRow = this.dsSrc.Tables[0].NewRow();
            if (this.dsSrc.Tables[0].Rows.Count == 0)
            {
                dataRow["id"] = 1;
            }
            else
            {
                dataRow["id"] = Convert.ToInt32(this.dsSrc.Tables[0].Rows[this.dsSrc.Tables[0].Rows.Count - 1][0].ToString()) + 1;
            }
            dataRow["variablename"] = this.variablename.Text;
            dataRow["variablevalue"] = this.variablevalue.Text;
            this.dsSrc.Tables[0].Rows.Add(dataRow);
            try
            {
                if (this.dsSrc.Tables[0].Rows.Count == 0)
                {
                    File.Delete(Utils.GetMapPath("../../templates/" + Request["path"] + "/templatevariable.xml"));
                    this.dsSrc.Reset();
                    this.dsSrc.Dispose();
                }
                else
                {
                    string fileName = base.Server.MapPath("../../templates/" + Request["path"] + "/templatevariable.xml");
                    this.dsSrc.WriteXml(fileName);
                    this.dsSrc.Reset();
                    this.dsSrc.Dispose();
                    //var cacheService = XCache.Current;
                    //cacheService.RetrieveObject("/Forum/" + Request["path"] + "/TemplateVariable");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_templatevariable.aspx?templateid=" + Request["templateid"] + "&path=" + Request["path"] + "&templatename=" + Request["templatename"] + "';");
                }
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_templatevariable.aspx?templateid=" + Request["templateid"] + "&path=" + Request["path"] + "&templatename=" + Request["templatename"] + "';</script>");
            }
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.SaveVar.Click += new EventHandler(this.SaveVar_Click);
            this.DataGrid1.DataKeyField = "id";
            this.DataGrid1.TableHeaderName = "模板变量列表";
            this.DataGrid1.ColumnSpan = 4;
        }
    }
}