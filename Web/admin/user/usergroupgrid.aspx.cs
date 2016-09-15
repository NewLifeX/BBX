using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class usergroupgrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "用户组列表";
            this.DataGrid1.Attributes.Add("borderStyle", "2");
            this.DataGrid1.DataKeyField = "id";
            //this.DataGrid1.BindData(UserGroups.GetCreditUserGroup());
            this.DataGrid1.BindData<UserGroup>(UserGroup.FindAll积分组());
            this.DataGrid1.Sort = "creditshigher";
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void EditUserGroup_Click(object sender, EventArgs e)
        {
            try
            {
                int num = 0;
                ArrayList arrayList = new ArrayList();
                ArrayList arrayList2 = new ArrayList();
                List<UserGroup2> ugs = new List<UserGroup2>();
                foreach (object current in this.DataGrid1.GetKeyIDArray())
                {
                    int id = int.Parse(current.ToString());
                    string controlValue = this.DataGrid1.GetControlValue(num, "grouptitle");
                    if (controlValue.Trim() == "")
                    {
                        base.RegisterStartupScript("", "<script>alert('组标题未输入,请检查!');window.location.href='usergroupgrid.aspx';</script>");
                        return;
                    }
                    int num2 = int.Parse(this.DataGrid1.GetControlValue(num, "creditshigher"));
                    int num3 = int.Parse(this.DataGrid1.GetControlValue(num, "creditslower"));
                    if (num2 >= num3)
                    {
                        base.RegisterStartupScript("", "<script>alert('" + controlValue + "组的积分下限超过上限,请检查!');window.location.href='usergroupgrid.aspx';</script>");
                        return;
                    }
                    arrayList.Add(num2);
                    arrayList2.Add(num3);
                    ugs.Add(new UserGroup2(id, controlValue, num2, num3));
                    num++;
                }
                arrayList.Sort();
                arrayList2.Sort();
                for (int i = 1; i < arrayList.Count; i++)
                {
                    if (arrayList[i].ToString() != arrayList2[i - 1].ToString())
                    {
                        base.RegisterStartupScript("", "<script>alert('积分下限与上限取值不连续,请检查!');window.location.href='usergroupgrid.aspx';</script>");
                        return;
                    }
                }
                for (int j = 0; j < ugs.Count; j++)
                {
                    UserGroup2 userGroup = (UserGroup2)ugs[j];
                    UserGroup userGroupInfo = UserGroup.FindByID(userGroup.id);
                    userGroupInfo.GroupTitle = userGroup.grouptitle;
                    userGroupInfo.Creditslower = userGroup.creditslower;
                    userGroupInfo.Creditshigher = userGroup.creditshigher;
                    //UserGroups.UpdateUserGroup(userGroupInfo);
                    userGroupInfo.Save();
                }
                //Caches.ReSetUserGroupList();
                base.RegisterStartupScript("", "<script>window.location.href='usergroupgrid.aspx';</script>");
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('积分下限或是上限输入的数值不合法,请检查!');window.location.href='usergroupgrid.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.ColumnSpan = 12;
        }
    }
}