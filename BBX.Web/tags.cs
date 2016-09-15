using System.Collections.Generic;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class tags : PageBase
    {
        public List<Topic> topiclist;
        //public List<SpacePostInfo> spacepostlist;
        //public List<PhotoInfo> photolist;
        public int tagid = DNTRequest.GetInt("tagid", 0);
        public Tag tag;
        public int pageid = (DNTRequest.GetInt("page", 1) < 1) ? 1 : DNTRequest.GetInt("page", 1);
        public int topiccount;
        //public int spacepostcount;
        //public int photocount;
        public int pagecount = 1;
        public string pagenumbers;
        public string listtype = DNTRequest.GetString("t");
        //public GoodsinfoCollection goodslist;
        //public int goodscount;
        public Tag[] taglist;

        protected override void ShowPage()
        {
            if (!config.Enabletag)
            {
                base.AddErrLine("没有启用Tag功能");
                return;
            }
            if (this.tagid <= 0)
            {
                this.pagetitle = "标签";
                this.taglist = Tag.GetHotForumTags(100).ToArray();
                return;
            }
            this.tag = Tag.FindByID(this.tagid);
            if (this.tag == null || this.tag.OrderID < 0)
            {
                base.AddErrLine("指定的标签不存在或已关闭");
                return;
            }
            this.pagetitle = this.tag.Name;
            if (Utils.StrIsNullOrEmpty(this.listtype))
            {
                this.listtype = "topic";
            }
            if (base.IsErr())
            {
                return;
            }
            this.BindItem();
        }

        private void BindItem()
        {
            string type = listtype;
            if (type == null) return;
            if (type == "topic")
            {
                //this.topiccount = Topics.GetTopicCountByTagId(this.tagid);
                this.topiccount = TopicTag.FindTidCountByTagID(tagid);
                this.SetPage(this.topiccount);
                if (this.topiccount > 0)
                {
                    this.topiclist = Topics.GetTopicListByTagId(this.tagid, this.pageid, this.config.Tpp);
                    this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, "tags.aspx?t=topic&tagid=" + this.tagid, 8);
                    return;
                }
                base.AddErrLine("该标签下暂无主题");
            }
        }

        private void SetPage(int count)
        {
            this.pagecount = ((count % this.config.Tpp == 0) ? (count / this.config.Tpp) : (count / this.config.Tpp + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
        }
    }
}