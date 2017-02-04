using System;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addusergroup : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected pageinfo info1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected BBX.Control.TextBox groupTitle;
        protected BBX.Control.TextBox creditshigher;
        protected BBX.Control.TextBox stars;
        protected RegularExpressionValidator RegularExpressionValidator2;
        protected BBX.Control.TextBox maxprice;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected BBX.Control.TextBox maxsigsize;
        protected RegularExpressionValidator RegularExpressionValidator5;
        protected BBX.Control.TextBox maxsizeperday;
        protected BBX.Control.TextBox maxspacephotosize;
        protected ColorPicker color;
        protected BBX.Control.TextBox creditslower;
        protected BBX.Control.TextBox readaccess;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected BBX.Control.TextBox maxpmnum;
        protected RegularExpressionValidator RegularExpressionValidator4;
        protected BBX.Control.TextBox maxattachsize;
        protected BBX.Control.TextBox maxspaceattachsize;
        protected BBX.Control.CheckBoxList attachextensions;
        protected TabPage tabPage22;
        protected usergrouppowersetting usergrouppowersetting;
        protected BBX.Control.Button AddUserGroupInf;
        protected BBX.Control.TextBox groupavatar;
        protected BBX.Control.TextBox maxfriendscount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.usergrouppowersetting.Bind();
                if (Request["groupid"] != "")
                {
                    this.SetGroupRights(DNTRequest.GetInt("groupid", 0));
                }
            }
        }

        public void SetGroupRights(int groupid)
        {
            var userGroupInfo = UserGroup.FindByKeyForEdit(groupid);
            this.creditshigher.Text = userGroupInfo.Creditshigher.ToString();
            this.creditslower.Text = userGroupInfo.Creditslower.ToString();
            this.stars.Text = userGroupInfo.Stars.ToString();
            this.color.Text = userGroupInfo.Color;
            this.groupavatar.Text = userGroupInfo.Groupavatar;
            this.readaccess.Text = userGroupInfo.Readaccess.ToString();
            this.maxprice.Text = userGroupInfo.MaxPrice.ToString();
            this.maxpmnum.Text = userGroupInfo.MaxPmNum.ToString();
            this.maxsigsize.Text = userGroupInfo.MaxSigSize.ToString();
            this.maxattachsize.Text = userGroupInfo.MaxAttachSize.ToString();
            this.maxsizeperday.Text = userGroupInfo.MaxSizeperday.ToString();
        }

        private void AddUserGroupInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                foreach (DictionaryEntry item in new Hashtable
                {
                    {
                        "附件最大尺寸",
                        this.maxattachsize.Text
                    },

                    {
                        "每天最大附件总尺寸",
                        this.maxsizeperday.Text
                    },

                    {
                        "个人空间附件总尺寸",
                        this.maxspaceattachsize.Text
                    },

                    {
                        "相册空间总尺寸",
                        this.maxspacephotosize.Text
                    }
                })
                {
                    if (item.Value.ToInt(-1) < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误," + item.Key + "只能是0或者正整数');window.location.href='global_editusergroup.aspx';</script>");
                        return;
                    }
                }
                var userGroupInfo = new UserGroup();
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = Convert.ToInt32((String.IsNullOrEmpty(this.readaccess.Text)) ? "0" : this.readaccess.Text);
                userGroupInfo.RadminID = 0;
                userGroupInfo.GroupTitle = this.groupTitle.Text;
                userGroupInfo.Creditshigher = this.creditshigher.Text.ToInt();
                userGroupInfo.Creditslower = this.creditslower.Text.ToInt();
                this.usergrouppowersetting.GetSetting(ref userGroupInfo);
                if (userGroupInfo.Creditshigher >= userGroupInfo.Creditslower)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败, 积分下限必须小于积分上限');</script>");
                }
                else
                {
                    if (userGroupInfo.AllowBonus && userGroupInfo.MinBonusprice >= userGroupInfo.MaxBonusprice)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败, 最低悬赏价格必须小于最高悬赏价格');</script>");
                        return;
                    }
                    userGroupInfo.Stars = this.stars.Text.ToInt();
                    userGroupInfo.Color = this.color.Text;
                    userGroupInfo.Groupavatar = this.groupavatar.Text;
                    userGroupInfo.MaxPrice = this.maxprice.Text.ToInt();
                    userGroupInfo.MaxPmNum = this.maxpmnum.Text.ToInt();
                    userGroupInfo.MaxSigSize = this.maxsigsize.Text.ToInt();
                    userGroupInfo.MaxAttachSize = this.maxattachsize.Text.ToInt();
                    userGroupInfo.MaxSizeperday = this.maxsizeperday.Text.ToInt();
                    userGroupInfo.MaxSpaceattachSize = this.maxspaceattachsize.Text.ToInt();
                    userGroupInfo.MaxSpacephotoSize = this.maxspacephotosize.Text.ToInt();
                    userGroupInfo.AttachExtensions = this.attachextensions.GetSelectString(",");
                    userGroupInfo.Raterange = "";
                    //if (AdminUserGroups.AddUserGroupInfo(userGroupInfo))
                    try
                    {
                        userGroupInfo.InsertWithCredits();
                        //XCache.Remove(CacheKeys.FORUM_USER_GROUP_LIST);
                        UserGroup.FindAllWithCache();
                        AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加用户组", "组名:" + this.groupTitle.Text);
                        base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
                        return;
                    }
                    //if (!String.IsNullOrEmpty(AdminUserGroups.opresult))
                    catch (Exception ex)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + ex.Message + "');window.location.href='global_usergroupgrid.aspx';</script>");
                        return;
                    }
                    //base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergroupgrid.aspx';</script>");
                    //return;
                }
                return;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.AddUserGroupInf.Click += new EventHandler(this.AddUserGroupInf_Click);
            //DataTable attachmentType = Attachments.GetAttachmentType();
            this.attachextensions.AddTableData(AttachType.FindAllWithCache());
        }
    }
}