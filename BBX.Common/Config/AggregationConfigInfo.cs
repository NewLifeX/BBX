using System;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    /// <summary>æ€∫œ≈‰÷√</summary>
    [Description("æ€∫œ≈‰÷√")]
    [XmlConfigFile("config/aggregationconfiginfo.config", 15000)]
    [Serializable]
    public class AggregationConfigInfo : XmlConfig2<AggregationConfigInfo>
    {
        private int m_spacetopnewcommentscount = 10;
        public int SpaceTopNewCommentsCount { get { return m_spacetopnewcommentscount; } set { m_spacetopnewcommentscount = value; } }

        private int m_spacetopnewcommentstimeout = 20;
        public int SpaceTopNewCommentsTimeout { get { return m_spacetopnewcommentstimeout; } set { m_spacetopnewcommentstimeout = value; } }

        private int m_topcommentcountpostlistcount = 10;
        public int TopcommentcountPostListCount { get { return m_topcommentcountpostlistcount; } set { m_topcommentcountpostlistcount = value; } }

        private int m_topcommentcountpostlisttimeout = 20;
        public int TopcommentcountPostListTimeout { get { return m_topcommentcountpostlisttimeout; } set { m_topcommentcountpostlisttimeout = value; } }

        private int m_topviewspostlistcount = 10;
        public int TopviewsPostListCount { get { return m_topviewspostlistcount; } set { m_topviewspostlistcount = value; } }

        private int m_topviewspostlisttimeout = 20;
        public int TopviewsPostListTimeout { get { return m_topviewspostlisttimeout; } set { m_topviewspostlisttimeout = value; } }

        private int m_topcommentcountspacelistcount = 10;
        public int TopcommentcountSpaceListCount { get { return m_topcommentcountspacelistcount; } set { m_topcommentcountspacelistcount = value; } }

        private int m_topcommentcountspacelisttimeout = 20;
        public int TopcommentcountSpaceListTimeout { get { return m_topcommentcountspacelisttimeout; } set { m_topcommentcountspacelisttimeout = value; } }

        private int m_topvisitedtimesspacelistcount = 10;
        public int TopvisitedtimesSpaceListCount { get { return m_topvisitedtimesspacelistcount; } set { m_topvisitedtimesspacelistcount = value; } }

        private int m_topvisitedtimesspacelisttimeout = 20;
        public int TopvisitedtimesSpaceListTimeout { get { return m_topvisitedtimesspacelisttimeout; } set { m_topvisitedtimesspacelisttimeout = value; } }

        private int m_toppostcountspacelistcount = 10;
        public int ToppostcountSpaceListCount { get { return m_toppostcountspacelistcount; } set { m_toppostcountspacelistcount = value; } }

        private int m_toppostcountspacelisttimeout = 20;
        public int ToppostcountSpaceListTimeout { get { return m_toppostcountspacelisttimeout; } set { m_toppostcountspacelisttimeout = value; } }

        private int m_recentupdatespaceaggregationlistcount = 10;
        public int RecentUpdateSpaceAggregationListCount { get { return m_recentupdatespaceaggregationlistcount; } set { m_recentupdatespaceaggregationlistcount = value; } }

        private int m_recentupdatespaceaggregationlisttimeout = 20;
        public int RecentUpdateSpaceAggregationListTimeout { get { return m_recentupdatespaceaggregationlisttimeout; } set { m_recentupdatespaceaggregationlisttimeout = value; } }
    }
}