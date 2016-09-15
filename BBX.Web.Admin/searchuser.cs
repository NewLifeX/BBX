using System;
using BBX.Common;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class searchuser : UserControlsPageBase
    {
        public string userListTable = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string @string = Request["searchinf"];
            if (@string != "")
            {
                this.userListTable = Users.GetSearchUserList(@string);
                return;
            }
            this.userListTable = "您未输入任何搜索关键字";
        }
    }
}