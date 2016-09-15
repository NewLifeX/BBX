using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class help : PageBase
    {
        public List<Help> helplist = (DNTRequest.GetInt("hid", 0) > 0) ? Help.GetHelpList(DNTRequest.GetInt("hid", 0)) : Help.GetHelpList(0);
        public string dbtype;
        public string assemblyproductname = Utils.ProductName;
        public string Assemblycopyright = Utils.GetAssemblyCopyright();
        public int showversion = DNTRequest.GetInt("version", 0);
        //public string dllver_bbxaggregation = "";
        //public string dllver_bbxcache = "";
        public string dllver_bbxcommon = "";
        //public string dllver_bbxconfig = "";
        public string dllver_bbxcontrol = "";
        //public string dllver_bbxdata = "";
        //public string dllver_bbxdatasqlserver = "";
        //public string dllver_bbxdataaccess = "";
        //public string dllver_bbxdatamysql = "";
        public string dllver_bbxentity = "";
        //public string dllver_bbxevent = "";
        public string dllver_bbxforum = "";
        public string dllver_bbxplugin = "";
        //public string dllver_bbxpluginmailsysmail = "";
        //public string dllver_bbxpluginpasswordmode = "";
        //public string dllver_bbxpluginpreviewjpg = "";
        //public string dllver_bbxpluginspread = "";
        //public string dllver_bbxspace = "";
        public string dllver_bbxwebadmin = "";
        public string dllver_bbxweb = "";
        //public string dllver_bbxwebservice = "";

        protected override void ShowPage()
        {
            dbtype = XForum.Meta.Session.Dal.DbType.ToString();
            this.pagetitle = "帮助";
            if (this.helplist == null)
            {
                base.AddErrLine("没有信息可读取！");
                return;
            }
            if (this.showversion == 1)
            {
                //this.dllver_bbxaggregation = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Aggregation.dll");
                //this.dllver_bbxcache = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Cache.dll");
                this.dllver_bbxcommon = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Common.dll");
                //this.dllver_bbxconfig = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Config.dll");
                this.dllver_bbxcontrol = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Control.dll");
                //this.dllver_bbxdata = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Data.dll");
                //this.dllver_bbxdatasqlserver = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Data.SqlServer.dll");
                //this.dllver_bbxdataaccess = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Data.Access.dll");
                //this.dllver_bbxdatamysql = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Data.MySql.dll");
                this.dllver_bbxentity = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Entity.dll");
                //this.dllver_bbxevent = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Event.dll");
                this.dllver_bbxforum = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Forum.dll");
                this.dllver_bbxplugin = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Plugin.dll");
                //this.dllver_bbxpluginmailsysmail = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Plugin.Mail.SysMail.dll");
                //this.dllver_bbxpluginpasswordmode = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Plugin.PasswordMode.dll");
                //this.dllver_bbxpluginpreviewjpg = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Plugin.Preview.Jpg.dll");
                //this.dllver_bbxpluginspread = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Plugin.Spread.dll");
                //this.dllver_bbxspace = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Space.dll");
                this.dllver_bbxwebadmin = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Web.Admin.dll");
                this.dllver_bbxweb = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Web.dll");
                //this.dllver_bbxwebservice = this.LoadDllVersion(HttpRuntime.BinDirectory + "BBX.Web.Services.dll");
            }
        }

        private string LoadDllVersion(string fullfilename)
        {
            string result;
            try
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fullfilename);
                result = string.Format("{0}.{1}.{2}", versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart);
            }
            catch
            {
                result = "未能加载dll或该dll文件不存在!";
            }
            return result;
        }
    }
}