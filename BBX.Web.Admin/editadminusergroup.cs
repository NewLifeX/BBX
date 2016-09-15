using System;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Cache;
using Discuz.Common;
using Discuz.Config;
using Discuz.Control;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;

namespace Discuz.Web.Admin
{
    public class editadminusergroup : AdminPage
    {
        public AdminGroup adminGroupInfo = new AdminGroup();
        public UserGroup userGroupInfo = new UserGroup();
        protected bool haveAlbum;
        protected bool haveSpace;
        protected HtmlForm Form1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected Discuz.Control.DropDownList radminid;
        protected ColorPicker color;
        protected Discuz.Control.TextBox readaccess;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected Discuz.Control.TextBox maxpmnum;
        protected RegularExpressionValidator homephone;
        protected Discuz.Control.TextBox maxattachsize;
        protected Discuz.Control.TextBox maxspaceattachsize;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected Discuz.Control.TextBox groupTitle;
        protected Discuz.Control.TextBox stars;
        protected RegularExpressionValidator RegularExpressionValidator2;
        protected Discuz.Control.TextBox maxprice;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected Discuz.Control.TextBox maxsigsize;
        protected RegularExpressionValidator RegularExpressionValidator4;
        protected Discuz.Control.TextBox maxsizeperday;
        protected Discuz.Control.TextBox maxspacephotosize;
        protected TabPage tabPage22;
        protected usergrouppowersetting usergrouppowersetting;
        protected TabPage tabPage33;
        protected Discuz.Control.CheckBoxList admingroupright;
        protected Discuz.Control.DropDownList allowstickthread;
        protected Hint Hint1;
        protected Discuz.Control.Button UpdateUserGroupInf;
        protected Discuz.Control.Button DeleteUserGroupInf;
        protected Discuz.Control.TextBox creditshigher;
        protected Discuz.Control.TextBox creditslower;
        protected Discuz.Control.TextBox groupavatar;
        protected Discuz.Control.TextBox maxfriendscount;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.haveAlbum = (AlbumPluginProvider.GetInstance() != null);
            this.haveSpace = (SpacePluginProvider.GetInstance() != null);
            if (!base.IsPostBack)
            {
                if (!(Request["groupid"] != ""))
                {
                    base.Response.Redirect("global_adminusergroupgrid.aspx");
                    return;
                }
                this.LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                if (AlbumPluginProvider.GetInstance() == null)
                {
                    this.admingroupright.Items.RemoveAt(this.admingroupright.Items.Count - 1);
                }
            }
        }

        public void LoadUserGroupInf(int groupid)
        {
            this.userGroupInfo = UserGroup.FindByID(groupid);
            this.groupTitle.Text = Utils.RemoveFontTag(this.userGroupInfo.GroupTitle);
            this.creditshigher.Text = this.userGroupInfo.Creditshigher.ToString();
            this.creditslower.Text = this.userGroupInfo.Creditslower.ToString();
            this.stars.Text = this.userGroupInfo.Stars.ToString();
            this.color.Text = this.userGroupInfo.Color;
            this.groupavatar.Text = this.userGroupInfo.Groupavatar;
            this.readaccess.Text = this.userGroupInfo.Readaccess.ToString();
            this.maxprice.Text = this.userGroupInfo.MaxPrice.ToString();
            this.maxpmnum.Text = this.userGroupInfo.MaxPmNum.ToString();
            this.maxsigsize.Text = this.userGroupInfo.MaxSigSize.ToString();
            this.maxattachsize.Text = this.userGroupInfo.MaxAttachSize.ToString();
            this.maxsizeperday.Text = this.userGroupInfo.MaxSizeperday.ToString();
            this.maxspaceattachsize.Text = this.userGroupInfo.MaxSpaceattachSize.ToString();
            this.maxspacephotosize.Text = this.userGroupInfo.MaxSpacephotoSize.ToString();
            if (groupid > 0 && groupid <= 3)
            {
                this.radminid.Enabled = false;
            }
            this.radminid.SelectedValue = this.userGroupInfo.RadminID.ToString();
            this.attachextensions.SetSelectByID(this.userGroupInfo.AttachExtensions.Trim());

            //this.adminGroupInfo = AdminUserGroups.AdminGetAdminGroupInfo(this.userGroupInfo.ID);
            adminGroupInfo = AdminGroup.FindByID(userGroupInfo.ID);
            this.usergrouppowersetting.Bind(this.userGroupInfo);
            if (this.adminGroupInfo != null)
            {
                admingroupright.SelectedIndex = -1;
                admingroupright.Items[0].Selected = adminGroupInfo.AllowEditPost;
                admingroupright.Items[1].Selected = adminGroupInfo.AllowEditpoll;
                admingroupright.Items[2].Selected = adminGroupInfo.AllowDelPost;
                admingroupright.Items[3].Selected = adminGroupInfo.AllowMassprune;
                admingroupright.Items[4].Selected = adminGroupInfo.AllowViewIP;
                admingroupright.Items[5].Selected = adminGroupInfo.AllowEditUser;
                admingroupright.Items[6].Selected = adminGroupInfo.AllowViewLog;
                admingroupright.Items[7].Selected = adminGroupInfo.DisablePostctrl;
                admingroupright.Items[8].Selected = adminGroupInfo.AllowViewrealName;
                admingroupright.Items[9].Selected = adminGroupInfo.AllowBanUser;
                admingroupright.Items[10].Selected = adminGroupInfo.AllowBanIP;
                admingroupright.Items[11].Selected = adminGroupInfo.AllowModPost;
                admingroupright.Items[12].Selected = adminGroupInfo.AllowPostannounce;
                var config = GeneralConfigInfo.Current;
                this.admingroupright.Items[13].Selected = (("," + config.Reportusergroup + ",").IndexOf("," + groupid + ",") != -1);
                this.admingroupright.Items[this.admingroupright.Items.Count - 1].Selected = (("," + config.Photomangegroups + ",").IndexOf("," + groupid + ",") != -1);
                if (this.adminGroupInfo.AllowStickthread.ToString() != "")
                {
                    this.allowstickthread.SelectedValue = this.adminGroupInfo.AllowStickthread.ToString();
                }
            }
            if (this.radminid.SelectedValue == "1")
            {
                this.allowstickthread.Enabled = false;
                this.allowstickthread.SelectedValue = "3";
            }
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (AdminUserGroups.DeleteUserGroupInfo(DNTRequest.GetInt("groupid", -1)))
                {
                    var config = GeneralConfigInfo.Current;
                    string text = "";
                    string[] array = config.Reportusergroup.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text2 = array[i];
                        if (text2 != this.userGroupInfo.ID.ToString())
                        {
                            if (text == "")
                            {
                                text = text2;
                            }
                            else
                            {
                                text = text + "," + text2;
                            }
                        }
                    }
                    config.Reportusergroup = text;
                    text = "";
                    string[] array2 = config.Photomangegroups.Split(',');
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text3 = array2[j];
                        if (text3 != this.userGroupInfo.ID.ToString())
                        {
                            if (text == "")
                            {
                                text = text3;
                            }
                            else
                            {
                                text = text + "," + text3;
                            }
                        }
                    }
                    config.Photomangegroups = text;
                    config.Save();

                    //config.Save();;
                    //DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
                    //AdminGroups.GetAdminGroupList();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除管理组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
            }
        }

        public int BoolToInt(bool a)
        {
            if (!a)
            {
                return 0;
            }
            return 1;
        }

        public byte BoolToByte(bool a)
        {
            return (Byte)(a ? 1 : 0);
        }

        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
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
                        base.RegisterStartupScript("", "<script>alert('输入错误," + dictionaryEntry.Key.ToString() + "只能是0或者正整数');window.location.href='global_editadminusergroup.aspx';</script>");
                        return;
                    }
                }
                this.userGroupInfo = UserGroup.FindByID(DNTRequest.GetInt("groupid", -1));
                this.userGroupInfo.System = 0;
                this.userGroupInfo.Type = 0;
                this.userGroupInfo.Readaccess = Convert.ToInt32(this.readaccess.Text);
                int num = Convert.ToInt32(this.radminid.SelectedValue);
                if (num > 0 && num <= 3)
                {
                    adminGroupInfo = new AdminGroup();
                    adminGroupInfo.AdmingID = (short)this.userGroupInfo.ID;
                    adminGroupInfo.AllowEditPost = admingroupright.Items[0].Selected;
                    adminGroupInfo.AllowEditpoll = admingroupright.Items[1].Selected;
                    adminGroupInfo.AllowStickthread = Convert.ToInt16(this.allowstickthread.SelectedValue) > 0;
                    adminGroupInfo.AllowModPost = false;
                    adminGroupInfo.AllowDelPost = admingroupright.Items[2].Selected;
                    adminGroupInfo.AllowMassprune = admingroupright.Items[3].Selected;
                    adminGroupInfo.AllowRefund = false;
                    adminGroupInfo.AllowCensorword = false;
                    adminGroupInfo.AllowViewIP = admingroupright.Items[4].Selected;
                    adminGroupInfo.AllowBanIP = false;
                    adminGroupInfo.AllowEditUser = admingroupright.Items[5].Selected;
                    adminGroupInfo.AllowModUser = false;
                    adminGroupInfo.AllowBanUser = false;
                    adminGroupInfo.AllowPostannounce = false;
                    adminGroupInfo.AllowViewLog = admingroupright.Items[6].Selected;
                    adminGroupInfo.DisablePostctrl = admingroupright.Items[7].Selected;
                    adminGroupInfo.AllowViewrealName = admingroupright.Items[8].Selected;
                    adminGroupInfo.AllowBanUser = admingroupright.Items[9].Selected;
                    adminGroupInfo.AllowBanIP = admingroupright.Items[10].Selected;
                    adminGroupInfo.AllowModPost = admingroupright.Items[11].Selected;
                    adminGroupInfo.AllowPostannounce = admingroupright.Items[12].Selected;

                    //AdminGroups.SetAdminGroupInfo(this.adminGroupInfo, this.userGroupInfo.ID);
                    adminGroupInfo.Save();
                    this.userGroupInfo.RadminID = num;
                }
                else
                {
                    this.userGroupInfo.RadminID = 0;
                }

                //AdminGroups.ChangeUserAdminidByGroupid(this.userGroupInfo.RadminID, this.userGroupInfo.ID);
                AdminGroup.ChangeGroup(userGroupInfo.RadminID, userGroupInfo.ID);
                this.userGroupInfo.GroupTitle = this.groupTitle.Text;
                this.userGroupInfo.Creditshigher = Convert.ToInt32(this.creditshigher.Text);
                this.userGroupInfo.Creditslower = Convert.ToInt32(this.creditslower.Text);
                this.userGroupInfo.Stars = Convert.ToInt32(this.stars.Text);
                this.userGroupInfo.Color = this.color.Text;
                this.userGroupInfo.Groupavatar = this.groupavatar.Text;
                this.userGroupInfo.MaxPrice = Convert.ToInt32(this.maxprice.Text);
                this.userGroupInfo.MaxPmNum = Convert.ToInt32(this.maxpmnum.Text);
                this.userGroupInfo.MaxSigSize = Convert.ToInt32(this.maxsigsize.Text);
                this.userGroupInfo.MaxAttachSize = Convert.ToInt32(this.maxattachsize.Text);
                this.userGroupInfo.MaxSizeperday = Convert.ToInt32(this.maxsizeperday.Text);
                this.userGroupInfo.MaxSpaceattachSize = Convert.ToInt32(this.maxspaceattachsize.Text);
                this.userGroupInfo.MaxSpacephotoSize = Convert.ToInt32(this.maxspacephotosize.Text);
                this.userGroupInfo.AttachExtensions = this.attachextensions.GetSelectString(",");
                this.usergrouppowersetting.GetSetting(ref this.userGroupInfo);
                //if (AdminUserGroups.UpdateUserGroupInfo(this.userGroupInfo))
                if (userGroupInfo.Save() > 0)
                {
                    var config = GeneralConfigInfo.Current;
                    int groupid = this.userGroupInfo.ID;
                    if (this.admingroupright.Items[13].Selected)
                    {
                        if (("," + config.Reportusergroup + ",").IndexOf("," + groupid + ",") == -1)
                        {
                            if (config.Reportusergroup == "")
                            {
                                config.Reportusergroup = groupid.ToString();
                            }
                            else
                            {
                                GeneralConfigInfo expr_636 = config;
                                expr_636.Reportusergroup = expr_636.Reportusergroup + "," + groupid.ToString();
                            }
                        }
                    }
                    else
                    {
                        string text = "";
                        string[] array = config.Reportusergroup.Split(',');
                        for (int i = 0; i < array.Length; i++)
                        {
                            string text2 = array[i];
                            if (text2 != groupid.ToString())
                            {
                                if (text == "")
                                {
                                    text = text2;
                                }
                                else
                                {
                                    text = text + "," + text2;
                                }
                            }
                        }
                        config.Reportusergroup = text;
                    }
                    if (AlbumPluginProvider.GetInstance() != null)
                    {
                        if (this.admingroupright.Items[this.admingroupright.Items.Count - 1].Selected)
                        {
                            if (("," + config.Photomangegroups + ",").IndexOf("," + groupid + ",") == -1)
                            {
                                if (config.Photomangegroups == "")
                                {
                                    config.Photomangegroups = groupid.ToString();
                                }
                                else
                                {
                                    GeneralConfigInfo expr_75C = config;
                                    expr_75C.Photomangegroups = expr_75C.Photomangegroups + "," + groupid.ToString();
                                }
                            }
                        }
                        else
                        {
                            string text3 = "";
                            string[] array2 = config.Photomangegroups.Split(',');
                            for (int j = 0; j < array2.Length; j++)
                            {
                                string text4 = array2[j];
                                if (text4 != groupid.ToString())
                                {
                                    if (text3 == "")
                                    {
                                        text3 = text4;
                                    }
                                    else
                                    {
                                        text3 = text3 + "," + text4;
                                    }
                                }
                            }
                            config.Photomangegroups = text3;
                        }
                    }
                    config.Save();

                    //config.Save();;
                    DNTCache.Current.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新管理组", "组名:" + this.groupTitle.Text);
                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
            }
        }

        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            var userGroupInfo = UserGroup.FindByID(int.Parse(this.radminid.SelectedValue));
            if (userGroupInfo != null)
            {
                this.creditshigher.Text = userGroupInfo.Creditslower.ToString();
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
                DataTable attachmentType = Attachments.GetAttachmentType();
                this.attachextensions.AddTableData(attachmentType, userGroupInfo.AttachExtensions);
            }
            var adminGroupInfo = AdminGroup.FindByID(int.Parse(this.radminid.SelectedValue));
            if (adminGroupInfo != null)
            {
                admingroupright.SelectedIndex = -1;
                admingroupright.Items[0].Selected = adminGroupInfo.AllowEditPost;
                admingroupright.Items[1].Selected = adminGroupInfo.AllowEditpoll;
                admingroupright.Items[2].Selected = adminGroupInfo.AllowDelPost;
                admingroupright.Items[3].Selected = adminGroupInfo.AllowMassprune;
                admingroupright.Items[4].Selected = adminGroupInfo.AllowViewIP;
                admingroupright.Items[5].Selected = adminGroupInfo.AllowEditUser;
                admingroupright.Items[6].Selected = adminGroupInfo.AllowViewLog;
                admingroupright.Items[7].Selected = adminGroupInfo.DisablePostctrl;
                admingroupright.Items[8].Selected = adminGroupInfo.AllowViewrealName;
            }
            if (this.radminid.SelectedValue == "1")
            {
                this.allowstickthread.Enabled = false;
                this.allowstickthread.SelectedValue = "3";
                return;
            }
            this.allowstickthread.Enabled = true;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.radminid.SelectedIndexChanged += new EventHandler(this.radminid_SelectedIndexChanged);
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);
            this.radminid.Items.Add(new ListItem("请选择     ", "0"));
            foreach (var current in UserGroup.GetAdminUserGroup())
            {
                if (current.ID > 0 && current.ID <= 3)
                {
                    this.radminid.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                }
            }
            var attachmentType = Attachments.GetAttachmentType();
            this.attachextensions.AddTableData(attachmentType);
        }
    }
}