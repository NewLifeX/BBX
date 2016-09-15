using System.Web.UI;
using BBX.Config;

namespace BBX.Web.UI
{
    public class HelpPage : Page
    {
        public string forumtitle;
        public string forumurl;

        public HelpPage()
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            this.forumtitle = config.Forumtitle;
            this.forumurl = config.Forumurl;
        }
    }
}