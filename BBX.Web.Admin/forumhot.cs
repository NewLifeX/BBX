using System;
using BBX.Cache;
using BBX.Common;
using BBX.Config;

namespace BBX.Web.Admin
{
    public class forumhot : AdminPage
    {
        public string action;
        public int id = DNTRequest.GetInt("id", -1);
        protected pageinfo info1;
        public ForumHotConfigInfo forumHotConfigInfo = ForumHotConfigInfo.Current;
        public ForumHotItemInfo forumHotItem = new ForumHotItemInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            action = DNTRequest.GetString("action");
            if (action != null)
            {
                var cfg = ForumHotConfigInfo.Current;

                if (action == "setenabled")
                {
                    cfg.Enable = (DNTRequest.GetInt("enabled", 0) == 1);

                    //ForumHotConfigs.SaveConfig(cfg);
                    cfg.Save();
                    base.Response.Redirect("forum_forumhot.aspx");
                    return;
                }
                if (action == "edit")
                {
                    this.forumHotItem = cfg.ForumHotCollection[this.id - 1];
                    return;
                }
                if (action != "editsave") return;

                string forumlist = Request["forumlist"];
                int forumnamelength = DNTRequest.GetInt("forumnamelength", 0);
                int topictitlelength = DNTRequest.GetInt("topictitlelength", 0);
                forumnamelength = ((forumnamelength < 0) ? 0 : forumnamelength);
                topictitlelength = ((topictitlelength < 0) ? 0 : topictitlelength);
                string datatimetype = Request["datatimetype"];
                string datatype = Request["datatype"];
                string sorttype = Request["sorttype"];
                string forumhotitemname = Request["forumhotitemname"];
                int itemenabled = DNTRequest.GetInt("itemenabled", 0);
                int datacount = DNTRequest.GetInt("datacount", 0);
                int cachetime = DNTRequest.GetInt("cachetime", 0);
                itemenabled = ((itemenabled < 0) ? 0 : ((itemenabled > 1) ? 1 : itemenabled));
                cachetime = ((cachetime < 0) ? 1 : cachetime);

                switch (datatype)
                {
                    case "topics":
                        //forumlist = string.Empty;
                        //datatimetype = string.Empty;
                        //topictitlelength = 0;
                        break;
                    case "forums":
                        break;
                    case "users":
                        //forumlist = string.Empty;
                        //forumnamelength = 0;
                        //topictitlelength = 0;
                        sorttype = ((sorttype == "posts") ? datatimetype : sorttype);
                        break;
                    case "pictures":
                        //forumnamelength = 0;
                        break;
                    default:
                        break;
                }
                var hot = cfg.ForumHotCollection[this.id - 1];
                hot.Name = forumhotitemname;
                hot.Enabled = itemenabled;
                hot.Datatype = datatype;
                hot.Sorttype = sorttype;
                hot.Forumlist = forumlist;
                hot.Dataitemcount = datacount;
                hot.Datatimetype = datatimetype;
                hot.Cachetimeout = cachetime;
                hot.Forumnamelength = forumnamelength;
                hot.Topictitlelength = topictitlelength;
                cfg.Save();

                XCache.Remove("/Forum/ForumHostList-" + this.id);
                XCache.Remove("/Aggregation/HotForumList" + this.id);
                XCache.Remove("/Aggregation/Users_" + this.id + "List");
                XCache.Remove("/Aggregation/HotImages_" + this.id + "List");
                base.Response.Redirect("forum_forumhot.aspx");
            }
        }
    }
}