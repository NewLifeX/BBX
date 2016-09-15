using System.Data;
using System.Text;
using BBX.Entity;

namespace BBX.Plugin.Mall
{
    public abstract class MallPluginBase
    {
        public enum OperaCode
        {
            Equal = 1,
            NoEuqal,
            Morethan,
            MorethanOrEqual,
            Lessthan,
            LessthanOrEqual
        }

        public const string GoodsHotTagJSONPCacheFileName = "cache\\tag\\hottags_mall_cache_jsonp.txt";

        public abstract void WriteHotTagsListForGoodsJSONPCacheFile(int count);

        public abstract StringBuilder GetTradeLogJson(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc);

        public abstract StringBuilder GetLeaveWordJson(int leavewordid);

        public abstract StringBuilder GetLeaveWordJson(int goodsid, int pagesize, int pageindex, string orderby, int ascdesc);

        public abstract string GetGoodsRatesJson(int uid, int uidtype, int ratetype, string filter);

        public abstract string GetHotGoodsJsonData(int days, int categroyid, int count);

        public abstract string GetShopInfoJson(int shoptype);

        public abstract string GetGoodsRecommendCondition(int opcode, int recommend);

        public abstract string GetGoodsQualityCondition(int opcode, int quality);

        public abstract string GetGoodsCloseCondition(int opcode, int closed);

        public abstract string GetGoodsExpirationCondition(int opcode, int day);

        public abstract string GetGoodsDateLineCondition(int opcode, int day);

        public abstract string GetGoodsRemainCondition(int opcode, int amount);

        public abstract string GetGoodsDisplayCondition(int opcode, int displayorder);

        public abstract GoodsinfoCollection GetHotGoods(int days, int categoryid, int count, string condition);

        public abstract GoodsinfoCollection GetGoodsInfoList(int categoryid, int pagesize, int pageindex, string condition, string orderby, int ascdesc);

        public abstract GoodsinfoCollection GetGoodsInfoList(int pagesize, int pageindex, string condition, string orderby, int ascdesc);

        public abstract string GetGoodsListJsonData(int categroyid, int order, int topnumber);

        public abstract string GetGoodsCategoryWithFid();

        public abstract DataTable GetLocationsTable();

        public abstract Goodsinfo GetGoodsInfo(int goodsid);

        public abstract int GetGoodsCountWithSameTag(int tagid);

        public abstract int GetGoodsCategoryIdByFid(int forumid);

        public abstract GoodsinfoCollection GetGoodsWithSameTag(int tagid, int pageid, int pagesize);

		//public abstract Goodsattachmentinfo GetGoodsAttachmentsByAid(int aid);

        public abstract int GetCategoriesFid(int categoryid);

        public abstract void EmptyGoodsCategoryFid(int fid);

        public abstract void StaticWriteJsonFile();
    }
}