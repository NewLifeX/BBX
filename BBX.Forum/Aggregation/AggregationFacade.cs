namespace BBX.Aggregation
{
    /// <summary>聚合接口</summary>
    public class AggregationFacade
    {
        private static AggregationData baseAggregationData;
        /// <summary>基础聚合</summary>
        public static AggregationData BaseAggregation { get { return baseAggregationData; } }

        private static ForumAggregationData forumAggregationData;
        /// <summary>论坛聚合</summary>
        public static ForumAggregationData ForumAggregation { get { return forumAggregationData; } }

        static AggregationFacade()
        {
            baseAggregationData = new AggregationData();
            forumAggregationData = new ForumAggregationData();
            AggregationDataSubject.Attach(baseAggregationData);
            AggregationDataSubject.Attach(forumAggregationData);
        }
    }
}