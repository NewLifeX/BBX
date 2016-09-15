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
    public class addusergroupspecial : AdminPage
    {
        public UserGroup userGroupInfo = new UserGroup();
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected BBX.Control.TextBox groupTitle;
        protected ColorPicker color;
        protected BBX.Control.TextBox maxprice;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected BBX.Control.TextBox maxsigsize;
        protected RegularExpressionValidator RegularExpressionValidator5;
        protected BBX.Control.TextBox maxsizeperday;
        protected BBX.Control.TextBox maxspacephotosize;
        protected BBX.Control.TextBox stars;
        protected RegularExpressionValidator RegularExpressionValidator2;
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
            var userGroupInfo = UserGroup.FindByID(groupid);
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
                foreach (DictionaryEntry dictionaryEntry in new Hashtable
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
                    if (!Utils.IsInt(dictionaryEntry.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误," + dictionaryEntry.Key.ToString() + "只能是0或者正整数');window.location.href='addusergroupspecial.aspx';</script>");
                        return;
                    }
                }
                this.LoadUserGroupInfo();
                //if (this.AddUserGroupInfo())
                try
                {
                    userGroupInfo.InsertWithCredits();

                    //XCache.Remove(CacheKeys.FORUM_USER_GROUP_LIST);
                    //UserGroup.FindAllWithCache();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加特殊用户组", "组名:" + this.groupTitle.Text);
                    //return true;

                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupspecialgrid.aspx';");
                    //return;
                }
                //if (!String.IsNullOrEmpty(AdminUserGroups.opresult))
                catch (Exception ex)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + ex.Message + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    //return;
                }
                //base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
            }
        }

        private void LoadUserGroupInfo()
        {
            this.userGroupInfo.System = 0;
            this.userGroupInfo.Type = 0;
            this.userGroupInfo.Readaccess = Convert.ToInt32((String.IsNullOrEmpty(this.readaccess.Text)) ? "0" : this.readaccess.Text);
            this.userGroupInfo.AllowDirectPost = true;
            this.userGroupInfo.AllowMultigroups = false;
            this.userGroupInfo.AllowCstatus = false;
            this.userGroupInfo.AllowUsebLog = false;
            this.userGroupInfo.AllowInvisible = false;
            this.userGroupInfo.AllowTransfer = false;
            this.userGroupInfo.AllowHtml = false;
            this.userGroupInfo.AllowNickName = false;
            this.userGroupInfo.AllowViewstats = false;
            this.userGroupInfo.RadminID = -1;
            this.userGroupInfo.GroupTitle = this.groupTitle.Text;
            this.userGroupInfo.Creditshigher = 0;
            this.userGroupInfo.Creditslower = 0;
            this.userGroupInfo.Stars = this.stars.Text.ToInt();
            this.userGroupInfo.Color = this.color.Text;
            this.userGroupInfo.Groupavatar = this.groupavatar.Text;
            this.userGroupInfo.MaxPrice = this.maxprice.Text.ToInt();
            this.userGroupInfo.MaxPmNum = this.maxpmnum.Text.ToInt();
            this.userGroupInfo.MaxSigSize = this.maxsigsize.Text.ToInt();
            this.userGroupInfo.MaxAttachSize = this.maxattachsize.Text.ToInt();
            this.userGroupInfo.MaxSizeperday = this.maxsizeperday.Text.ToInt();
            this.userGroupInfo.MaxSpaceattachSize = this.maxspaceattachsize.Text.ToInt();
            this.userGroupInfo.MaxSpacephotoSize = this.maxspacephotosize.Text.ToInt();
            this.userGroupInfo.AttachExtensions = this.attachextensions.GetSelectString(",");
            this.userGroupInfo.Raterange = "";
            this.usergrouppowersetting.GetSetting(ref this.userGroupInfo);
        }

        //private bool AddUserGroupInfo()
        //{
        //    if (AdminUserGroups.AddUserGroupInfo(this.userGroupInfo))
        //    {
        //        //XCache.Remove(CacheKeys.FORUM_USER_GROUP_LIST);
        //        UserGroup.FindAllWithCache();
        //        AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加特殊用户组", "组名:" + this.groupTitle.Text);
        //        return true;
        //    }
        //    return false;
        //}

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