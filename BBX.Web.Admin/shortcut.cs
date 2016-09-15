using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Discuz.Common;
using Discuz.Common.Xml;
using Discuz.Config;
using Discuz.Control;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Web.Admin.AutoUpdateManager;

namespace Discuz.Web.Admin
{
    public class shortcut : AdminPage
    {
        protected HtmlForm Form1;
        protected Discuz.Control.TextBox Username;
        protected Discuz.Control.Button EditUser;
        protected DropDownTreeList forumid;
        protected Discuz.Control.Button EditForum;
        protected Discuz.Control.DropDownList Usergroupid;
        protected Discuz.Control.Button EditUserGroup;
        protected Discuz.Control.DropDownList Templatepath;
        protected Discuz.Control.Button CreateTemplate;
        protected LinkButton UpdateCache;
        protected LinkButton UpdateForumStatistics;
        public string filenamelist = "";
        protected string upgradeInfo = "";
        protected bool isNew;
        protected bool isHotFix;

        protected void Page_Load(object sender, EventArgs e)
        {
            string text = this.Page.Server.MapPath("../xml/user_" + this.userid + ".config");
            bool flag;
            if (File.Exists(text))
            {
                XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                xmlDocumentExtender.Load(text);
                flag = (xmlDocumentExtender.SelectSingleNode("/UserConfig/ShowUpgrade") != null && xmlDocumentExtender.SelectSingleNode("/UserConfig/ShowUpgrade").InnerText == "1");
            }
            else
            {
                flag = true;
            }
            if (flag)
            {
                this.LoadUpgradeInfo();
            }
            this.LoadTemplateInfo();
            this.GetStatInfo();
            this.forumid.BuildTree(Forums.GetForumListForDataTable(), "name", "fid");
            this.LinkDiscuzVersionPage();
        }

        private void GetStatInfo()
        {
            //StringBuilder stringBuilder = new StringBuilder();
            //string forumtitle = GeneralConfigInfo.Current.Forumtitle;
            //int num = Convert.ToInt32(Statistics.GetStatisticsRowItem("totalusers"));
            //int num2 = Convert.ToInt32(Statistics.GetStatisticsRowItem("totaltopic"));
            //int num3 = Convert.ToInt32(Statistics.GetStatisticsRowItem("totalpost"));
            //string text = HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
            //int major = Environment.Version.Major;
            //int minor = Environment.Version.Minor;
            //int build = Environment.Version.Build;
            //int num4 = 0;
            //string a;
            //if ((a = BaseConfigs.GetDbType.ToLower()) != null)
            //{
            //    if (!(a == "sqlserver"))
            //    {
            //        if (!(a == "access"))
            //        {
            //            if (a == "mysql")
            //            {
            //                num4 = 201;
            //            }
            //        }
            //        else
            //        {
            //            num4 = 101;
            //        }
            //    }
            //    else
            //    {
            //        num4 = 0;
            //    }
            //}
            //string text2 = string.Empty;
            //string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config");
            //if (File.Exists(mapPath))
            //{
            //    XmlDocument xmlDocument = new XmlDocument();
            //    xmlDocument.Load(mapPath);
            //    text2 = xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText;
            //    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/localupgrade/optionalupgrade/dnt" + Utils.Version + "/item");
            //    if (xmlNodeList != null)
            //    {
            //        foreach (XmlNode xmlNode in xmlNodeList)
            //        {
            //            if (this.StrToDateTime(xmlNode.InnerText) > this.StrToDateTime(text2))
            //            {
            //                text2 = xmlNode.InnerText;
            //            }
            //        }
            //    }
            //}
            //string text3 = Environment.OSVersion.ToString();
            //string arg_1C6_0 = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            //string text4 = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            //string text5 = "";
            //if (num4 == 0)
            //{
            //    Regex regex = new Regex("\\d{4}", RegexOptions.None);
            //    Match match = regex.Match(Databases.GetDataBaseVersion());
            //    if (match.Length != 0)
            //    {
            //        text5 = match.Value;
            //    }
            //}
            //string text6 = this.config.Passwordmode.ToString();
            //string text7 = this.config.Enablealbum.ToString();
            //string text8 = this.config.Enablespace.ToString();
            //string text9 = this.config.Enablemall.ToString();
            //string rootUrl = Utils.GetRootUrl(BaseConfigs.GetForumPath);
            //stringBuilder.Append(string.Concat(new object[]
            //{
            //    base.Server.UrlEncode(forumtitle),
            //    ",",
            //    num,
            //    ",",
            //    num2,
            //    ",",
            //    num3,
            //    ",",
            //    text,
            //    ",",
            //    Utils.AssemblyFileVersion.FileMajorPart,
            //    ",",
            //    Utils.AssemblyFileVersion.FileMinorPart,
            //    ",",
            //    Utils.AssemblyFileVersion.FileBuildPart,
            //    ","
            //}));
            //stringBuilder.Append(string.Concat(new object[]
            //{
            //    major,
            //    ",",
            //    minor,
            //    ",",
            //    build,
            //    ",",
            //    num4,
            //    ",",
            //    text2,
            //    ",",
            //    text3,
            //    ",",
            //    rootUrl,
            //    ",",
            //    text4,
            //    ","
            //}));
            //stringBuilder.Append(string.Concat(new string[]
            //{
            //    text5,
            //    ",",
            //    text6,
            //    ",",
            //    text7,
            //    ",",
            //    text8,
            //    ",",
            //    text9,
            //    ",",
            //    this.config.Passwordkey
            //}));
            //base.RegisterStartupScript("", string.Format("<script type='text/javascript' src='http://service.nt.discuz.net/news.aspx?update={0}'></script>", Convert.ToBase64String(Encoding.Default.GetBytes(stringBuilder.ToString()))));
        }

        private void LoadUpgradeInfo()
        {
            if (!base.IsPostBack)
            {
                try
                {
                    this.MergeUpgradeInfo();
                    AutoUpdate autoUpdate = new AutoUpdate();
                    string versionList = autoUpdate.GetVersionList();
                    StreamWriter streamWriter = new StreamWriter(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/upgradeini.config"));
                    streamWriter.Write(versionList.Replace("\n", "\r\n"));
                    streamWriter.Close();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
                    DateTime t = this.StrToDateTime(xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText);
                    XmlDocument xmlDocument2 = new XmlDocument();
                    xmlDocument2.LoadXml(versionList);
                    XmlNodeList xmlNodeList = xmlDocument2.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/requiredupgrade/item");
                    XmlNode xmlNode = xmlNodeList.Item(xmlNodeList.Count - 1);
                    DateTime t2 = this.StrToDateTime(xmlNode["version"].InnerText);
                    this.isNew = (t2 > t);
                    if (this.isNew)
                    {
                        this.upgradeInfo = "<span style='font-size:16px;'>检测到最新版本：" + xmlNode["versiondescription"].InnerText + "</span><br /><span style='padding:3px 0px 3px 10px;display:block;'>" + xmlNode["description"].InnerText + "</span>";
                    }
                    XmlNodeList xmlNodeList2 = xmlDocument.SelectNodes("/localupgrade/optionalupgrade/dnt" + Utils.Version + "/item");
                    XmlNodeList xmlNodeList3 = xmlDocument2.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/optionalupgrade/dnt" + Utils.Version + "/item");
                    string text = "";
                    foreach (XmlNode xmlNode2 in xmlNodeList3)
                    {
                        bool flag = false;
                        foreach (XmlNode xmlNode3 in xmlNodeList2)
                        {
                            if (xmlNode2.FirstChild.InnerText == xmlNode3.InnerText)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this.isHotFix = true;
                            text = text + xmlNode2["versiondescription"].InnerText + "<br />";
                        }
                    }
                    if (text != "")
                    {
                        this.upgradeInfo = this.upgradeInfo + "<span style='font-size:16px;'>检测到最新补丁：</span><br /><span style='padding:3px 0px 3px 10px;display:block;'>" + text + "</span>";
                    }
                    if (this.isNew || this.isHotFix)
                    {
                        base.RegisterStartupScript("", "<script type='text/javascript'>\r\nshowInfo();\r\n</script>\r\n");
                    }
                }
                catch
                {
                }
            }
        }

        private void MergeUpgradeInfo()
        {
            if (!File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config")))
            {
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            string text = "";
            string text2 = "";
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));
            if (xmlDocument.SelectSingleNode("/requiredupgrade") != null)
            {
                text = xmlDocument.SelectSingleNode("/requiredupgrade").InnerText;
            }
            else
            {
                text2 = xmlDocument.SelectSingleNode("/optionalupgrade").InnerText;
            }
            File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (text != "")
            {
                if (this.StrToDateTime(xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText) >= this.StrToDateTime(text))
                {
                    return;
                }
                xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText = text;
            }
            else
            {
                XmlNode xmlNode = xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade/dnt" + Utils.Version);
                if (xmlNode == null)
                {
                    xmlNode = xmlDocument.CreateElement("dnt" + Utils.Version);
                }
                else
                {
                    foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                    {
                        if (xmlNode2.InnerText == text2)
                        {
                            return;
                        }
                    }
                }
                XmlElement xmlElement = xmlDocument.CreateElement("item");
                xmlElement.InnerText = text2;
                xmlNode.AppendChild(xmlElement);
                if (xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade") == null)
                {
                    xmlDocument.SelectSingleNode("/localupgrade").AppendChild(xmlDocument.CreateElement("optionalupgrade"));
                }
                xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade").AppendChild(xmlNode);
            }
            xmlDocument.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
        }

        private DateTime StrToDateTime(string str)
        {
            string text = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            if (str.Length == 8)
            {
                text += " 00:00:00";
            }
            else
            {
                string text2 = text;
                text = text2 + " " + str.Substring(8, 2) + ":" + str.Substring(10, 2) + ":" + str.Substring(12, 2);
            }
            return Convert.ToDateTime(text);
        }

        public void LoadTemplateInfo()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(base.Server.MapPath("../../templates/" + this.Templatepath.SelectedValue + "/"));
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            for (int i = 0; i < fileSystemInfos.Length; i++)
            {
                FileSystemInfo fileSystemInfo = fileSystemInfos[i];
                if (fileSystemInfo != null)
                {
                    string text = fileSystemInfo.Extension.ToLower();
                    if (text.Equals(".htm") && fileSystemInfo.Name.IndexOf("_") != 0)
                    {
                        this.filenamelist = this.filenamelist + fileSystemInfo.Name.Split('.')[0] + "|";
                    }
                }
            }
        }

        public void LinkDiscuzVersionPage()
        {
            //var text = "http://nt.discuz.net/update/index.aspx?ver=" + Utils.Version + "&dbtype=" + BaseConfigs.GetDbType;
            //var httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
            //try
            //{
            //    httpWebRequest.Method = "GET";
            //    httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //    httpWebRequest.AllowAutoRedirect = false;
            //    httpWebRequest.Timeout = 1500;
            //    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //    if (httpWebResponse.StatusCode.ToString() != "OK")
            //    {
            //        base.RegisterStartupScript("form1", "<script language=javascript>document.getElementById('checkveriframe').parentNode.style.height=0+'px';document.getElementById('checkveriframe').src='about:blank';</script>");
            //    }
            //    else
            //    {
            //        base.RegisterStartupScript("form1", "<script language=javascript>document.getElementById('checkveriframe').parentNode.style.height=0+'px';document.getElementById('checkveriframe').src='" + text + "';</script>");
            //    }
            //}
            //catch
            //{
            //    base.RegisterStartupScript("form1", "<script language=javascript> if(navigator.appName.indexOf('Explorer') > -1){document.getElementById('checkveriframe').parentElement.innerHTML='&nbsp;&nbsp;&nbsp;  <img src=../images/hint.gif> 无法链接到" + Utils.ProductName + "官方网站,因此无法得到最新官方信息';}else{document.getElementById('checkveriframe').parentNode.innerHTML='&nbsp;&nbsp;&nbsp;  <img src=../images/hint.gif> 无法链接到" + Utils.ProductName + "官方网站,因此无法得到最新官方信息';}</script>");
            //}
        }

        private void EditForum_Click(object sender, EventArgs e)
        {
            if (this.forumid.SelectedValue != "0")
            {
                base.Response.Redirect("../forum/forum_EditForums.aspx?fid=" + this.forumid.SelectedValue);
                return;
            }
            base.RegisterStartupScript("", "<script>alert('请您选择有效的论坛版块!');</script>");
        }

        private void EditUserGroup_Click(object sender, EventArgs e)
        {
            if (this.Usergroupid.SelectedValue != "0")
            {
                int num = Convert.ToInt32(this.Usergroupid.SelectedValue);
                if (num >= 1 && num <= 3)
                {
                    base.Response.Redirect("../global/global_editadminusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                if (num >= 4 && num <= 8)
                {
                    base.Response.Redirect("../global/global_editsysadminusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                int radminid = UserGroup.FindByID(Utils.StrToInt(this.Usergroupid.SelectedValue, 0)).RadminID;
                if (radminid == 0)
                {
                    base.Response.Redirect("../global/global_editusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                if (radminid > 0)
                {
                    base.Response.Redirect("../global/global_editadminusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                if (radminid < 0)
                {
                    base.Response.Redirect("../global/global_editusergroupspecial.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('请您选择有效的用户组!');</script>");
            }
        }

        private void UpdateForumStatistics_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetStatistics();
                base.RegisterStartupScript("PAGE", "window.location.href='shortcut.aspx';");
            }
        }

        private void UpdateCache_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetAllCache();
                base.RegisterStartupScript("PAGE", "window.location.href='shortcut.aspx';");
            }
        }

        private void CreateTemplate_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Globals.BuildTemplate(this.Templatepath.SelectedValue);
                base.RegisterStartupScript("PAGE", "window.location.href='shortcut.aspx';");
            }
        }

        private void EditUser_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("../global/global_usergrid.aspx?username=" + this.Username.Text.Trim());
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.EditUser.Click += new EventHandler(this.EditUser_Click);
            this.EditForum.Click += new EventHandler(this.EditForum_Click);
            this.EditUserGroup.Click += new EventHandler(this.EditUserGroup_Click);
            this.UpdateCache.Click += new EventHandler(this.UpdateCache_Click);
            this.CreateTemplate.Click += new EventHandler(this.CreateTemplate_Click);
            this.UpdateForumStatistics.Click += new EventHandler(this.UpdateForumStatistics_Click);
            foreach (DataRow dataRow in AdminTemplates.GetAllTemplateList(Utils.GetMapPath("..\\..\\templates\\")).Rows)
            {
                if (dataRow["valid"].ToString() == "1")
                {
                    this.Templatepath.Items.Add(new ListItem(dataRow["name"].ToString(), dataRow["directory"].ToString()));
                }
            }
            this.Username.AddAttributes("onkeydown", "if(event.keyCode==13) return(document.forms(0).EditUser.focus());");
            this.Usergroupid.AddTableData(UserGroups.GetUserGroupForDataTable(), "grouptitle", "groupid");
        }
    }
}