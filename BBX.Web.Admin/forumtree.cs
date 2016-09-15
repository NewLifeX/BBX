using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class forumtree : UserControlsPageBase
    {
        protected Literal TreeContent;
        private string T_rootpic = "<img src=../images/lines/tplus.gif align=absmiddle>";
        private string L_rootpic = "<img src=../images/lines/lplus.gif align=absmiddle>";
        private string L_TOP_rootpic = "<img src=../images/lines/rplus.gif align=absmiddle>";
        private string I_rootpic = "<img src=../images/lines/dashplus.gif align=absmiddle>";
        private string T_nodepic = "<img src=../images/lines/tminus.gif align=absmiddle>";
        private string L_nodepic = "<img src=../images/lines/lminus.gif align=absmiddle>";
        private string I_nodepic = "<img src=../images/lines/i.gif align=absmiddle>";
        private string No_nodepic = "<img src=../images/lines/noexpand.gif align=absmiddle>";
        private int noPicCount;
        public bool WithCheckBox = true;
        public string PageName = "forumbatchset";
        private string SelectForumStr = "";
        public StringBuilder sb = new StringBuilder();

        private string _hintTitle = "";

        public string HintTitle { get { return _hintTitle; } set { _hintTitle = value; } }

        private string _hintInfo = "";

        public string HintInfo { get { return _hintInfo; } set { _hintInfo = value; } }

        private int _hintLeftOffSet;

        public int HintLeftOffSet { get { return _hintLeftOffSet; } set { _hintLeftOffSet = value; } }

        private int _hintTopOffSet;

        public int HintTopOffSet { get { return _hintTopOffSet; } set { _hintTopOffSet = value; } }

        private string _hintShowType = "up";

        public string HintShowType { get { return _hintShowType; } set { _hintShowType = value; } }

        private int _hintHeight = 30;

        public int HintHeight { get { return _hintHeight; } set { _hintHeight = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadForumTree();
        }

        public void LoadForumTree()
        {
            DataTable forumListForDataTable = Forums.GetForumListForDataTable();
            if (forumListForDataTable.Rows.Count == 0)
            {
                base.Server.Transfer("../forum/forum_AddFirstForum.aspx");
            }
            this.ViewState["dt"] = forumListForDataTable;
            this.sb.Append("<table border=\"0\"  width=\"100%\" align=\"center\" cellspacing=\"0\" cellpadding=\"0\">");
            if (this.PageName.ToLower() != "advertisement")
            {
                if (this.WithCheckBox)
                {
                    this.sb.Append("<div style=\"height:30px\"><input class=\"input1\" title=\"选中/取消选中\" onclick=\"CheckAllTreeByName(this.form,'" + this.ClientID + "','null')\" type=\"checkbox\" name=\"" + this.ClientID + "_chkall\"\tid=\"" + this.ClientID + "_CheckAll\">全选/取消全选</div>");
                }
                this.AddTree(0, forumListForDataTable.Select("layer=0 AND [parentid]=0"), "");
            }
            else
            {
                var entity = Advertisement.FindByID(DNTRequest.GetInt("advid", 0));
                if (entity != null)
                {
                    this.SelectForumStr = "," + entity.Targets.Trim(',') + ",";
                }
                if (this.SelectForumStr.IndexOf("全部") >= 0)
                {
                    this.sb.Append("<tr><td class=treetd> " + this.L_TOP_rootpic + "<img class=treeimg src=../images/aspx.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"全部\"   checked> 全部</td></tr>");
                }
                else
                {
                    this.sb.Append("<tr><td class=treetd> " + this.L_TOP_rootpic + "<img class=treeimg src=../images/aspx.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"全部\"   > 全部</td></tr>");
                }
                if (this.SelectForumStr.IndexOf("首页") >= 0 && this.SelectForumStr.IndexOf("全部") < 0)
                {
                    this.sb.Append("<tr><td class=treetd> " + this.T_rootpic + "<img class=treeimg src=../images/htm.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"首页\"   checked> 首页</td></tr>");
                }
                else
                {
                    this.sb.Append("<tr><td class=treetd> " + this.T_rootpic + "<img class=treeimg src=../images/htm.gif > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"首页\"   > 首页</td></tr>");
                }
                this.AddAdsTree(0, forumListForDataTable.Select("layer=0 AND [parentid]=0"), "");
            }
            this.sb.Append("</table>");
            this.TreeContent.Text = this.sb.ToString();
        }

        private void AddTree(int layer, DataRow[] drs, string currentnodestr)
        {
            DataTable dataTable = (DataTable)this.ViewState["dt"];
            if (layer == 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    string text = "";
                    if (drs.Length == 1)
                    {
                        text += this.I_rootpic;
                        currentnodestr = this.No_nodepic;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            text += this.L_TOP_rootpic;
                            currentnodestr = this.I_nodepic;
                        }
                        else
                        {
                            if (i > 0 && i < drs.Length - 1)
                            {
                                text += this.T_rootpic;
                                currentnodestr = this.I_nodepic;
                            }
                            else
                            {
                                text += this.L_rootpic;
                                currentnodestr = this.No_nodepic;
                            }
                        }
                    }
                    if (this.WithCheckBox)
                    {
                        this.sb.Append("<tr><td class=treetd> " + text + "<img border=0 src=../images/folders.gif align=\\\"absmiddle\\\" > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[i]["fid"].ToString().Trim() + "\"  onclick=\"javascript:Tree_SelectOneNode(this)\" > <a href=\"../../showforum-" + drs[i]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[i]["name"].ToString().Trim() + "</a></td></tr>");
                    }
                    else
                    {
                        this.sb.Append("<tr><td class=treetd> " + text + " <img border=0 src=../images/folders.gif align=\\\"absmiddle\\\" >  <a href=\"../../showforum-" + drs[i]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[i]["name"].ToString().Trim() + "</a></td></tr>");
                    }
                    if (Convert.ToInt32(drs[i]["subforumcount"].ToString()) > 0)
                    {
                        int num = Convert.ToInt32(drs[i]["layer"].ToString());
                        string filterExpression = "layer=" + ++num + " AND parentid=" + drs[i]["fid"].ToString();
                        this.AddTree(num, dataTable.Select(filterExpression), currentnodestr);
                    }
                }
                return;
            }
            for (int j = 0; j < drs.Length; j++)
            {
                string text2 = "";
                text2 += currentnodestr;
                string text3 = currentnodestr;
                if (j >= 0 && j < drs.Length - 1)
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
                if (this.WithCheckBox)
                {
                    this.sb.Append("<tr><td class=treetd> " + text2 + " <img class=treeimg  src=../images/folder.gif align=\\\"absmiddle\\\" > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[j]["fid"].ToString().Trim() + "\" onclick=\"javascript:Tree_SelectOneNode(this)\" > <a href=\"../../showforum-" + drs[j]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[j]["name"].ToString().Trim() + "</a></td></tr>");
                }
                else
                {
                    this.sb.Append("<tr><td class=treetd> " + text2 + " <img class=treeimg  src=../images/folder.gif align=\\\"absmiddle\\\" > <a href=\"../../showforum-" + drs[j]["fid"].ToString().Trim() + ".aspx\" target=\"_blank\">" + drs[j]["name"].ToString().Trim() + "</a></td></tr>");
                }
                if (Convert.ToInt32(drs[j]["subforumcount"].ToString()) > 0)
                {
                    int num2 = Convert.ToInt32(drs[j]["layer"].ToString());
                    string filterExpression2 = "layer=" + ++num2 + " AND parentid=" + drs[j]["fid"].ToString();
                    this.AddTree(num2, dataTable.Select(filterExpression2), text3);
                }
            }
        }

        private void AddAdsTree(int layer, DataRow[] drs, string currentnodestr)
        {
            DataTable dataTable = (DataTable)this.ViewState["dt"];
            if (layer == 0)
            {
                for (int i = 0; i < drs.Length; i++)
                {
                    string text = "";
                    if (drs.Length == 1)
                    {
                        text += this.I_rootpic;
                        currentnodestr = this.No_nodepic;
                    }
                    else
                    {
                        if (i >= 0 && i < drs.Length - 1)
                        {
                            text += this.T_rootpic;
                            currentnodestr = this.I_nodepic;
                        }
                        else
                        {
                            text += this.L_rootpic;
                            currentnodestr = this.No_nodepic;
                        }
                    }
                    this.sb.Append("<tr><td class=treetd> " + text + "<img src=../images/folders.gif class=treeimg > " + drs[i]["name"].ToString().Trim() + "</td></tr>");
                    if (Convert.ToInt32(drs[i]["subforumcount"].ToString()) > 0)
                    {
                        int num = Convert.ToInt32(drs[i]["layer"].ToString());
                        string filterExpression = "layer=" + ++num + " AND parentid=" + drs[i]["fid"].ToString();
                        this.AddAdsTree(num, dataTable.Select(filterExpression), currentnodestr);
                    }
                }
                return;
            }
            for (int j = 0; j < drs.Length; j++)
            {
                string text2 = "";
                text2 += currentnodestr;
                string text3 = currentnodestr;
                if (j >= 0 && j < drs.Length - 1)
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
                if (this.SelectForumStr.IndexOf("," + drs[j]["fid"].ToString().Trim() + ",") >= 0 && this.SelectForumStr.IndexOf("全部") < 0)
                {
                    this.sb.Append("<tr><td class=treetd> " + text2 + " <img src=../images/folder.gif class=treeimg > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[j]["fid"].ToString().Trim() + "\"  checked> " + drs[j]["name"].ToString().Trim() + "</td></tr>");
                }
                else
                {
                    this.sb.Append("<tr><td class=treetd> " + text2 + " <img src=../images/folder.gif class=treeimg > <input class=\"input1\" type=checkbox id=\"" + this.ClientID + "\" name=\"" + this.ClientID + "\" value=\"" + drs[j]["fid"].ToString().Trim() + "\" > " + drs[j]["name"].ToString().Trim() + "</td></tr>");
                }
                if (Convert.ToInt32(drs[j]["subforumcount"].ToString()) > 0)
                {
                    int num2 = Convert.ToInt32(drs[j]["layer"].ToString());
                    string filterExpression2 = "layer=" + ++num2 + " AND parentid=" + drs[j]["fid"].ToString();
                    this.AddAdsTree(num2, dataTable.Select(filterExpression2), text3);
                }
            }
        }
    }
}