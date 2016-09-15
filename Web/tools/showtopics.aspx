<%@ Page Language="C#" AutoEventWireup="true" Inherits="BBX.Web.UI.ShowTopicsPage" %>

<%@ Import Namespace="BBX.Forum" %>
<%@ Import Namespace="BBX.Common" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="BBX.Entity" %>
<%@ Import Namespace="XCode" %>
<script runat="server">

    /*  模板代码开始 在此调整样式 请注意备份.
	{0}代表帖子ID,
	{1}代表标题,
	{2}代表帖子所属论坛ID,
	{3}代表帖子所属论坛名称 ,
	{4}代表帖子未截字的完整标题,双引号用\"表示，建议使用单引号 ,
	{5}代表图片缩*unicef略图路径(从根路径开始输出，设置type参数为1(正方形)或2(原比例)或onlyimg参数为1时有效),
	{6}代表当前Space完整Url,
	{7}代表当前Space日志完整Url,
	{8}代表当前相册完整Url,
	{9}代表当前版块的Url重写名称
	{10}代表当前帖子的Url
    {11}代表站点根地址
    {12}代表主题分类
*/
    public string[] Templates = {
	/*模板0 奇数行模板*/			"<p>[<a href=\"{11}{9}\" title=\"{4}\">{3}</a>]{12} <a href=\"{11}{10}\" title=\"{4}\">{1}</a></p>",
	/*模板1 奇数行模板*/			"<li>·<a href=\"{11}{10}\" title=\"{4}\">{1}</a></li>",
	/*模板2 奇数行图片调用模板*/			"<a href =\"{11}{10}\"><img src=\"{5}\" title=\"{4}\" /></a>",
	/*模板3 奇数行个人空间模板*/		"<a href=\"{6}\" title=\"{4}\">{1}</a>",
	/*模板4 奇数行个人日志模板*/		"<a href=\"{7}\" title=\"{4}\">{1}</a>",
	/*模板5 奇数行相册模板*/		"<a href=\"{8}\" title=\"{4}\">{1}</a>",
	/*模板6 奇数行推荐日志模板*/	"<li>·<a href=\"{7}\" title=\"{4}\">{1}</a></li>"
							};
    public string[] AlternatingTemplates = {
	/*模板0 偶数行模板*/			"<p style=\"background-color: #ffffcc;\">[<a href=\"{11}{9}\" title=\"{4}\">{3}</a>]{12} <a href=\"{11}{10}\" title=\"{4}\">{1}</a></p>",
	/*模板1 偶数行模板*/			"<li>·<a href=\"{11}{10}\" title=\"{4}\"><b>{1}</b></a></li>",
	/*模板2 偶数行图片调用模板*/			"<a href =\"{11}{10}\"><img src=\"{5}\" title=\"{4}\" /></a>",
	/*模板3 偶数行个人空间模板*/		"<a href=\"{6}\" title=\"{4}\">{1}</a>",
	/*模板4 偶数行个人日志模板*/		"<a href=\"{7}\" title=\"{4}\">{1}</a>",
	/*模板5 偶数行相册模板*/		"<a href=\"{8}\" title=\"{4}\">{1}</a>",
	/*模板6 偶数行推荐日志模板*/	"<li>·<a href=\"{7}\" title=\"{4}\">{1}</a></li>"
							};
    public string TypeNameTemplate = "<a href=\"{2}showforum.aspx?forumid={3}&typeid={1}\">[{0}]</a>";
    /*在主题有主题分类的情况下，分类名称的显示样式模板 */

    public string AggregationTemplate = "<li><span><a href=\"{11}{9}\" target=\"_blank\">[{3}]</a></span><a href=\"{11}{10}\" target=\"_blank\" title=\"{4}\">{1}</a></li>";
    /* 模板代码结束 */

    override protected void OnInit(EventArgs e)
    {
        int count = DNTRequest.GetQueryInt("count", 10);
        int views = DNTRequest.GetQueryInt("views", -1);
        int fid = DNTRequest.GetQueryInt("fid", 0);
        int tType = DNTRequest.GetQueryInt("time", 0);
        int oType = DNTRequest.GetQueryInt("order", 0);
        bool isDigest = DNTRequest.GetQueryInt("digest", 0) == 1;
        int mid = DNTRequest.GetQueryInt("template", 0);
        int cachetime = DNTRequest.GetQueryInt("cachetime", 20);
        bool onlyimg = DNTRequest.GetQueryInt("onlyimg", 0) == 1;
        string agg = DNTRequest.GetQueryString("agg");
        string typeIdList = DNTRequest.GetQueryString("typeidlist");

        try
        {
            if (Request.QueryString["encoding"] != null)
                Response.ContentEncoding = System.Text.Encoding.GetEncoding(Request.QueryString["encoding"]);

            string template = Templates[mid];
            string alternatingTemplate = AlternatingTemplates[mid];

            TopicTimeType timeType = ConvertTimeType(tType);
            //TopicOrderType orderType = ConvertOrderType(oType);

            /*
            Focuss.GetTopicList(count, views, fid, timetype, ordertype, isdigest, timespan, onlyimg)方法说明
            <summary>
            获得帖子列表
            </summary>
            <param name="count">最大数量</param>
            <param name="views">最小浏览量, -1为不限制</param>
            <param name="fid">板块ID,0为全部板块</param>
            <param name="timetype">期限类型，1为一天(TopicTimeType.Day)、2为一周(TopicTimeType.Week)、3为一月(TopicTimeType.Month)、0为不限制(TopicTimeType.All)</param>
            <param name="ordertype">排序类型(倒序): 0为id(TopicOrderType.ID)、1为访问量倒序(TopicOrderType.Views)、2为最后回复排序(TopicOrderType.LastPost)、3为最新主题(PostDateTime)、5为回复数(Replies)、6为评分数</param>
            <param name="isdigest">是否精华</param>
            <param name="timespan">缓存时间</param>
            <param name="onlyimg">是否只为图片</param>
            <param name="agg">空间，日志或相册调用：1为更新的个人空间；2推荐的个人空间；3最新日志；4推荐日志；5推荐相册。</param>
            <returns></returns>
            */
            switch (agg)
            {
                case "1":
                case "updatedspace":
                    //OutPutUpdatedSpaces(template, alternatingTemplate);
                    break;
                case "2":
                case "recommendedspace":
                    //OutPutRecommendedSpaces(template, alternatingTemplate);
                    break;
                case "3":
                case "newspacepost":
                    //OutPutNewSpacePosts(template, alternatingTemplate);
                    break;
                case "4":
                case "recommendedspacepost":
                    //OutPutRecommendedSpacePosts(template, alternatingTemplate);
                    break;
                case "5":
                case "recommendedalbum":
                    //OutPutRecommendedAlbum(template, alternatingTemplate);
                    break;
                default:
                    EntityList<Topic> dt = Focuses.GetTopicList(count, views, fid, typeIdList, timeType, oType, isDigest, cachetime, onlyimg, null);
                    OutPut(dt, template, alternatingTemplate);
                    break;
            }
        }
        catch
        {
            Response.Write("document.write('参数错误，请检查！');");
        }
        finally
        {
            Response.End();
        }
        base.OnInit(e);
    }

    private void OutPut(EntityList<Topic> list, string template, string alternatingTemplate)
    {
        string result = "";
        int length = DNTRequest.GetQueryInt("length", -1);
        int contentType = DNTRequest.GetQueryInt("type", 0);
        int imgMaxSize = DNTRequest.GetQueryInt("imgsize", 80);
        bool onlyimg = DNTRequest.GetQueryInt("onlyimg", 0) == 1;
        IDictionary<int, string> topicTypeList = TopicType.GetTopicTypeArray();
        int aggregation = DNTRequest.GetQueryInt("aggregation", 0);

        int i = 0;
        foreach (Topic topic in list)
        {
            int tid = Utils.StrToInt(topic.ID, 0);
            IXForum forum = Forums.GetForumInfo(Convert.ToInt32(topic.Fid));
            string title = topic.Title.Trim();
            string thumbnail = "";

            string topicTypeName;
            topicTypeList.TryGetValue(topic.TypeID, out topicTypeName);
            if (!string.IsNullOrEmpty(topicTypeName))
                topicTypeName = string.Format(TypeNameTemplate, topicTypeName, topic.TypeID, rootUrl, topic.Fid);

            if (contentType == 1 || contentType == 2 || onlyimg)
            {
                thumbnail = Attachments.GetThumbnailByTid(tid, imgMaxSize, ConvertThumbnailType(contentType));
            }

            if (aggregation == 0)
                result += string.Format((i % 2 == 0 ? template : alternatingTemplate), topic.ID.ToString(), (length == -1 ? title : Utils.GetUnicodeSubString(title, length, "")), forum.Fid, forum.Name, title, thumbnail, "", "", "", Urls.ShowForumAspxRewrite(forum.Fid, 0, forum.Rewritename), Urls.ShowTopicAspxRewrite(tid, 0), rootUrl, topicTypeName);
            else
                result += string.Format(AggregationTemplate, topic.ID.ToString(), (length == -1 ? title : Utils.GetUnicodeSubString(title, length, "")), forum.Fid, forum.Name, title, thumbnail, "", "", "", Urls.ShowForumAspxRewrite(forum.Fid, 0, forum.Rewritename), Urls.ShowTopicAspxRewrite(tid, 0), rootUrl, topicTypeName);

            i++;
        }
        Response.Write("document.write('" + result.Replace("'", "\\'") + "');");
    }

    //private TopicOrderType ConvertOrderType(int t)
    //{
    //    switch (t)
    //    {
    //        case (int)TopicOrderType.Views:
    //            return TopicOrderType.Views;
    //        case (int)TopicOrderType.LastPost:
    //            return TopicOrderType.LastPost;
    //        case (int)TopicOrderType.PostDateTime:
    //            return TopicOrderType.PostDateTime;
    //        case (int)TopicOrderType.Replies:
    //            return TopicOrderType.Replies;
    //        case (int)TopicOrderType.Rate:
    //            return TopicOrderType.Rate;
    //        default: return TopicOrderType.ID;
    //    }
    //}

    private ThumbnailType ConvertThumbnailType(int t)
    {
        switch (t)
        {
            case (int)ThumbnailType.Square:
                return ThumbnailType.Square;
            case (int)ThumbnailType.Thumbnail:
                return ThumbnailType.Thumbnail;
            default: return ThumbnailType.Square;
        }
    }

    private TopicTimeType ConvertTimeType(int t)
    {
        return (TopicTimeType)(Enum.Parse(typeof(TopicTimeType), t.ToString()));
    }
</script>