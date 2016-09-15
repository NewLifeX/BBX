using Discuz.Cache;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Plugin.Mall;

namespace Discuz.Aggregation
{
    public class GoodsAggregationData : AggregationData
    {
        public GoodsinfoCollection GetGoodsList(string condition, string orderBy, int categoryId, int topNumber)
        {
            if (Utils.StrIsNullOrEmpty(orderBy))
            {
                orderBy = "goodsid";
            }
            condition = (Utils.StrIsNullOrEmpty(orderBy) ? "" : condition.ToLower().Trim());
            string xpath = "/Aggregation/Goods/Goods_" + categoryId + "_" + condition + "_" + orderBy + "_List";
            var cacheService = XCache.Current;
            GoodsinfoCollection goodsinfoCollection = cacheService.RetrieveObject(xpath) as GoodsinfoCollection;
            if (goodsinfoCollection == null)
            {
                string a;
                if ((a = condition) != null)
                {
                    if (!(a == "recommend"))
                    {
                        if (!(a == "quality_new"))
                        {
                            if (a == "quality_old")
                            {
                                condition = MallPluginProvider.GetInstance().GetGoodsQualityCondition(4, 2);
                            }
                        }
                        else
                        {
                            condition = MallPluginProvider.GetInstance().GetGoodsQualityCondition(4, 1);
                        }
                    }
                    else
                    {
                        condition = MallPluginProvider.GetInstance().GetGoodsRecommendCondition(4, 1);
                    }
                }
                string a2;
                if ((a2 = orderBy) != null)
                {
                    if (!(a2 == "newgoods"))
                    {
                        if (!(a2 == "viewcount"))
                        {
                            if (!(a2 == "hotgoods"))
                            {
                            }
                        }
                        else
                        {
                            orderBy = "viewcount";
                        }
                    }
                    else
                    {
                        orderBy = "goodsid";
                    }
                }
                if (orderBy == "hotgoods")
                {
                    condition = MallPluginProvider.GetInstance().GetGoodsCloseCondition(1, 0);
                    condition += MallPluginProvider.GetInstance().GetGoodsExpirationCondition(6, 0);
                    condition += MallPluginProvider.GetInstance().GetGoodsDateLineCondition(4, 0);
                    condition += MallPluginProvider.GetInstance().GetGoodsRemainCondition(3, 0);
                    condition += MallPluginProvider.GetInstance().GetGoodsDisplayCondition(4, 0);
                    goodsinfoCollection = MallPluginProvider.GetInstance().GetHotGoods(360, categoryId, topNumber, condition);
                }
                else
                {
                    if (categoryId > 0)
                    {
                        goodsinfoCollection = MallPluginProvider.GetInstance().GetGoodsInfoList(categoryId, topNumber, 1, condition, orderBy, 1);
                    }
                    else
                    {
                        goodsinfoCollection = MallPluginProvider.GetInstance().GetGoodsInfoList(topNumber, 1, condition, orderBy, 1);
                    }
                }
                //cacheService.LoadCacheStrategy(new DefaultCacheStrategy
                //{
                //    TimeOut = 300
                //});
                XCache.Add(xpath, goodsinfoCollection);
                //cacheService.LoadDefaultCacheStrategy();
            }
            return goodsinfoCollection;
        }
    }
}