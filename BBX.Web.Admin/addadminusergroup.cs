using System;
using NewLife;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addadminusergroup : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected BBX.Control.DropDownList radminid;
        protected ColorPicker color;
        protected BBX.Control.TextBox readaccess;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected BBX.Control.TextBox maxpmnum;
        protected RegularExpressionValidator homephone;
        protected BBX.Control.TextBox maxattachsize;
        protected BBX.Control.TextBox maxspaceattachsize;
        protected BBX.Control.CheckBoxList attachextensions;
        protected BBX.Control.TextBox groupTitle;
        protected BBX.Control.TextBox stars;
        protected RegularExpressionValidator RegularExpressionValidator2;
        protected BBX.Control.TextBox maxprice;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected BBX.Control.TextBox maxsigsize;
        protected RegularExpressionValidator RegularExpressionValidator4;
        protected BBX.Control.TextBox maxsizeperday;
        protected BBX.Control.TextBox maxspacephotosize;
        protected TabPage tabPage22;
        protected usergrouppowersetting usergrouppowersetting;
        protected TabPage tabPage33;
        protected BBX.Control.CheckBoxList admingroupright;
        protected BBX.Control.DropDownList allowstickthread;
        protected BBX.Control.Button AddUserGroupInf;
        protected BBX.Control.TextBox creditshigher;
        protected BBX.Control.TextBox creditslower;
        protected BBX.Control.TextBox groupavatar;
        protected BBX.Control.TextBox maxfriendscount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.usergrouppowersetting.Bind();
            }
        }

        public void SetGroupRights(string groupid)
        {
            var userGroupInfo = UserGroup.FindByID(groupid.ToInt());
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
            this.radminid.SelectedValue = userGroupInfo.RadminID.ToString();
            var attachmentType = AttachType.FindAllWithCache().ToDataTable(false);
            this.attachextensions.AddTableData(attachmentType, userGroupInfo.AttachExtensions);
            this.usergrouppowersetting.Bind(userGroupInfo);

            var adminGroupInfo = AdminGroup.FindByID(groupid.ToInt());
            if (adminGroupInfo != null)
            {
                this.admingroupright.Items[0].Selected = adminGroupInfo.AllowEditPost;
                this.admingroupright.Items[1].Selected = adminGroupInfo.AllowEditpoll;
                this.admingroupright.Items[2].Selected = adminGroupInfo.AllowDelPost;
                this.admingroupright.Items[3].Selected = adminGroupInfo.AllowMassprune;
                this.admingroupright.Items[4].Selected = adminGroupInfo.AllowViewIP;
                this.admingroupright.Items[5].Selected = adminGroupInfo.AllowEditUser;
                this.admingroupright.Items[6].Selected = adminGroupInfo.AllowViewLog;
                this.admingroupright.Items[7].Selected = adminGroupInfo.DisablePostctrl;
                this.admingroupright.Items[8].Selected = adminGroupInfo.AllowViewrealName;
                this.admingroupright.Items[9].Selected = adminGroupInfo.AllowBanUser;
                this.admingroupright.Items[10].Selected = adminGroupInfo.AllowBanIP;
                this.admingroupright.Items[11].Selected = adminGroupInfo.AllowModPost;
                this.admingroupright.Items[12].Selected = adminGroupInfo.AllowPostannounce;
                var config = GeneralConfigInfo.Current;
                this.admingroupright.Items[13].Selected = (("," + config.Reportusergroup + ",").IndexOf("," + groupid + ",") != -1);
                this.admingroupright.Items[14].Selected = (("," + config.Photomangegroups + ",").IndexOf("," + groupid + ",") != -1);
            }
            if (this.radminid.SelectedValue == "1")
            {
                this.allowstickthread.Enabled = false;
                this.allowstickthread.SelectedValue = "3";
                return;
            }
            this.allowstickthread.Enabled = true;
        }

        public byte BoolToByte(bool a)
        {
            return (Byte)(a ? 1 : 0);
        }

        private void AddUserGroupInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.radminid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,请您选择相应的管理组, 再点击提交按钮!');</script>");
                    return;
                }
                if (this.groupTitle.Text.Trim() == string.Empty)
                {
                    base.RegisterStartupScript("", "<script>alert('用户组名称不能为空!');</script>");
                    return;
                }
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
                        base.RegisterStartupScript("", "<script>alert('输入错误," + item.Key + "只能是0或者正整数');window.location.href='addadminusergroup.aspx';</script>");
                        return;
                    }
                }
                var userGroupInfo = new UserGroup();
                userGroupInfo.System = 0;
                userGroupInfo.Type = 0;
                userGroupInfo.Readaccess = String.IsNullOrEmpty(this.readaccess.Text) ? 0 : this.readaccess.Text.ToInt();
                userGroupInfo.AllowDirectPost = true;
                //userGroupInfo.AllowMultigroups = 0;
                //userGroupInfo.AllowCstatus = 0;
                //userGroupInfo.AllowUsebLog = 0;
                //userGroupInfo.AllowInvisible = 0;
                //userGroupInfo.AllowTransfer = 0;
                //userGroupInfo.AllowHtml = 0;
                //userGroupInfo.AllowNickName = 0;
                //userGroupInfo.AllowViewstats = 0;
                userGroupInfo.GroupTitle = this.groupTitle.Text;
                userGroupInfo.Creditshigher = this.creditshigher.Text.ToInt();
                userGroupInfo.Creditslower = this.creditslower.Text.ToInt();
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
                userGroupInfo.RadminID = this.radminid.SelectedValue.ToInt();
                this.usergrouppowersetting.GetSetting(ref userGroupInfo);
                //if (AdminUserGroups.AddUserGroupInfo(userGroupInfo))
                if (userGroupInfo.InsertWithCredits() > 0)
                {
                    var config = GeneralConfigInfo.Current;
                    int maxUserGroupId = UserGroup.GetMaxUserGroupId();
                    if (this.admingroupright.Items[13].Selected && ("," + config.Reportusergroup + ",").IndexOf("," + maxUserGroupId + ",") == -1)
                    {
                        if (String.IsNullOrEmpty(config.Reportusergroup))
                            config.Reportusergroup = maxUserGroupId.ToString();
                        else
                            config.Reportusergroup += "," + maxUserGroupId.ToString();
                    }
                    if (this.admingroupright.Items[14].Selected && ("," + config.Photomangegroups + ",").IndexOf("," + maxUserGroupId + ",") == -1)
                    {
                        if (String.IsNullOrEmpty(config.Photomangegroups))
                            config.Photomangegroups = maxUserGroupId.ToString();
                        else
                            config.Photomangegroups += "," + maxUserGroupId.ToString();
                    }

                    //config.Save();;
                    config.Save();
                    var adg = new AdminGroup
                    {
                        ID = UserGroup.GetMaxUserGroupId(),
                        AllowEditPost = admingroupright.Items[0].Selected,
                        AllowEditpoll = admingroupright.Items[1].Selected,
                        AllowStickthread = this.allowstickthread.SelectedValue.ToInt(),

                        //Allowmodpost = 0,
                        AllowDelPost = admingroupright.Items[2].Selected,
                        AllowMassprune = admingroupright.Items[3].Selected,
                        AllowRefund = false,
                        AllowCensorword = false,
                        AllowViewIP = admingroupright.Items[4].Selected,

                        //AllowBanIP = 0,
                        AllowEditUser = admingroupright.Items[5].Selected,
                        AllowModUser = false,

                        //AllowBanUser = 0,
                        //Allowpostannounce = 0,
                        AllowViewLog = admingroupright.Items[6].Selected,
                        DisablePostctrl = admingroupright.Items[7].Selected,
                        AllowViewrealName = admingroupright.Items[8].Selected,
                        AllowBanUser = admingroupright.Items[9].Selected,
                        AllowBanIP = admingroupright.Items[10].Selected,
                        AllowModPost = admingroupright.Items[11].Selected,
                        AllowPostannounce = admingroupright.Items[12].Selected
                    };
                    adg.Insert();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加管理组", "组名:" + this.groupTitle.Text);
                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
            }
        }

        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetGroupRights(this.radminid.SelectedValue);
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
            this.radminid.SelectedIndexChanged += new EventHandler(this.radminid_SelectedIndexChanged);
            //DataTable attachmentType = Attachments.GetAttachmentType();
            this.attachextensions.AddTableData(AttachType.FindAllWithCache());
            this.radminid.Items.Add(new ListItem("请选择", "0"));
            var adminUserGroup = UserGroup.FindAll管理组();
            foreach (var current in adminUserGroup)
            {
                if (current.ID > 0 && current.ID <= 3)
                {
                    this.radminid.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                }
            }
            if (Request["groupid"] != "")
            {
                this.SetGroupRights(Request["groupid"]);
            }
        }
    }
}