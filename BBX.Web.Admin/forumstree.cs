using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using XCode;

namespace BBX.Web.Admin
{
    public class forumstree : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected Label ShowTreeLabel;
        private string T_rootpic = "<img src=../images/lines/tplus.gif align=absmiddle>";
        private string L_rootpic = "<img src=../images/lines/lplus.gif align=absmiddle>";
        private string L_TOP_rootpic = "<img src=../images/lines/rplus.gif align=absmiddle>";
        private string I_rootpic = "<img src=../images/lines/dashplus.gif align=absmiddle>";
        private string T_nodepic = "<img src=../images/lines/tminus.gif align=absmiddle>";
        private string L_nodepic = "<img src=../images/lines/lminus.gif align=absmiddle>";
        private string I_nodepic = "<img src=../images/lines/i.gif align=absmiddle>";
        private string No_nodepic = "<img src=../images/lines/noexpand.gif align=absmiddle>";
        public string str = "";
        public int noPicCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            var fid = Request["currentfid"].ToInt();

            if (fid > 0)
            {
                var tfid = Request["targetfid"].ToInt();
                if (!AdminForums.MovingForumsPos(fid, tfid, Request["isaschildnode"].ToInt() == 1))
                {
                    base.RegisterStartupScript("", "<script>alert('当前版块下面有子版块,因此无法移动!');window.location.href='forum_forumsTree.aspx';</script>");
                }
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "移动论坛版块", "移动论坛版块ID:" + fid + "到ID:" + tfid);
            }
            if (!this.Page.IsPostBack)
            {
                var list = XForum.Root.Childs;
                if (list.Count == 0)
                {
                    base.Server.Transfer("forum_AddFirstForum.aspx");
                }
                else
                {
                    this.AddTree(0, list, "");
                    this.str = "<script type=\"text/javascript\">\r\n  var obj = [" + this.str;
                    this.str = this.str.Substring(0, this.str.Length - 3);
                    this.str += "];\r\n var newtree = new tree(\"newtree\",obj,\"reSetTree\");";
                    this.str += "</script>";
                }
                this.ShowTreeLabel.Text = this.str;
            }
        }

        private void AddTree(int layer, EntityList<XForum> list, string currentnodestr)
        {
            if (layer == 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    string text = "";
                    if (list.Count == 1)
                    {
                        text += this.I_rootpic;
                        currentnodestr = this.No_nodepic;
                    }
                    else if (i == 0)
                    {
                        text += this.L_TOP_rootpic;
                        currentnodestr = this.I_nodepic;
                    }
                    else if (i > 0 && i < list.Count - 1)
                    {
                        text += this.T_rootpic;
                        currentnodestr = this.I_nodepic;
                    }
                    else
                    {
                        text += this.L_rootpic;
                        currentnodestr = this.No_nodepic;
                    }

                    var fi = list[i];
                    var name = Utils.HtmlEncode((fi.Name + "").Replace("\\", "\\\\ "));
                    this.str += "{fid:" + fi.ID + ",name:\"" + name + "\",subject:\" " + text + " <img src=../images/folders.gif align=\\\"absmiddle\\\" > <a href=\\\"../../showforum.aspx?forumid=" + fi.ID + "\\\" target=\\\"_blank\\\">" + name + "</a>\",linetitle:\"" + text + "\",parentidlist:0,layer:" + fi.Layer + ",subforumcount:" + fi.Childs.Count + ",istrade:" + 0 + "},\r\n";
                    if (fi.Childs.Count > 0) AddTree(fi.Layer + 1, fi.Childs, currentnodestr);
                    //{
                    //    int num = fi.Layer;
                    //    string filterExpression = "layer=" + ++num + " AND parentid=" + drs[i]["fid"].ToString();
                    //    this.AddTree(num, dataTable.Select(filterExpression), currentnodestr);
                    //}
                }
                return;
            }
            for (int j = 0; j < list.Count; j++)
            {
                string text2 = "" + currentnodestr;
                string text3 = currentnodestr;
                if (j >= 0 && j < list.Count - 1)
                {
                    text2 += this.T_nodepic;
                    text3 += this.I_nodepic;
                }
                else
                {
                    text2 += this.L_nodepic;
                    this.noPicCount++;
                    text3 += this.No_nodepic;
                }
                var fi = list[j];
                var name = Utils.HtmlEncode((fi.Name + "").Replace("\\", "\\\\ "));
                this.str += "{fid:" + fi.ID + ",name:\"" + name + "\",subject:\" " + text2 + " <img src=../images/folder.gif align=\\\"absmiddle\\\" > <a href=\\\"../../showforum.aspx?forumid=" + fi.ID + "\\\" target=\\\"_blank\\\">" + name + "</a>\",linetitle:\"" + text2 + "\",parentidlist:\"" + fi.GetFullPath(false, ",") + "\",layer:" + fi.Layer + ",subforumcount:" + fi.Childs.Count + ",istrade:" + 0 + "},\r\n";
                if (fi.Childs.Count > 0) AddTree(fi.Layer + 1, fi.Childs, text3);
                //if (Convert.ToInt32(drs[j]["subforumcount"].ToString()) > 0)
                //{
                //    int num2 = Convert.ToInt32(drs[j]["layer"].ToString());
                //    string filterExpression2 = "layer=" + ++num2 + " AND parentid=" + fi.ID;
                //    this.AddTree(num2, dataTable.Select(filterExpression2), text3);
                //}
            }
        }
    }
}