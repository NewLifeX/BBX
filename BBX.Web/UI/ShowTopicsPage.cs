using System.Web.UI;
using BBX.Common;
using BBX.Config;

namespace BBX.Web.UI
{
    public class ShowTopicsPage : Page
    {
        private int length = DNTRequest.GetQueryInt("length", -1);
        private int count = DNTRequest.GetQueryInt("count", 10);
        private int cachetime = DNTRequest.GetQueryInt("cachetime", 20);
        public string rootUrl = Utils.GetRootUrl(BaseConfigs.GetForumPath);
        private string spacerooturl = string.Empty;

        public ShowTopicsPage()
        {
            this.spacerooturl = this.rootUrl + "space/";
        }
    }
}