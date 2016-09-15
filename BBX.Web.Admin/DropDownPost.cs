using System;
using System.Data;
using System.Web.UI.WebControls;
using BBX.Config;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class DropDownPost : UserControlsPageBase
    {
        protected BBX.Control.DropDownList postslist;

        public string SelectedValue { get { return postslist.SelectedValue; } set { postslist.SelectedValue = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.postslist.Items.Clear();
                //foreach (DataRow dataRow in Posts.GetPostTableList().Rows)
                foreach (var item in TableList.GetAllPostTable())
                {
                    this.postslist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + item.ID, item.ID.ToString()));
                }
                this.postslist.DataBind();
                this.postslist.SelectedValue = TableList.GetPostTableId();
            }
        }
    }
}