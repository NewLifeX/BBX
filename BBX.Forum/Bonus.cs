using System.Collections.Generic;
using BBX.Common;

using BBX.Entity;

namespace BBX.Forum
{
    public class Bonus
    {
        public static void CloseBonus(TopicInfo topicinfo, int userid, int[] postIdArray, int[] winerIdArray, string[] winnerNameArray, string[] costBonusArray, string[] valuableAnswerArray, int bestAnswer)
        {
            int isbest = 0;
            topicinfo.Special = 3;
            Topics.UpdateTopic(topicinfo);
            for (int i = 0; i < winerIdArray.Length; i++)
            {
                int num = TypeConverter.StrToInt(costBonusArray[i]);
                if (winerIdArray[i] > 0 && num > 0)
                {
                    Users.UpdateUserExtCredits(winerIdArray[i], Scoresets.GetBonusCreditsTrans(), (float)num);
                }
                if (Utils.InArray(postIdArray[i].ToString(), valuableAnswerArray))
                {
                    isbest = 1;
                }
                if (postIdArray[i] == bestAnswer)
                {
                    isbest = 2;
                }
                BBX.Data.Bonus.AddLog(topicinfo.Tid, topicinfo.Posterid, winerIdArray[i], winnerNameArray[i], postIdArray[i], num, Scoresets.GetBonusCreditsTrans(), isbest);
            }
        }

        public static List<BonusLogInfo> GetLogs(TopicInfo topic)
        {
            if (topic.Tid <= 0 || topic.Special != 3)
            {
                return null;
            }
            return BBX.Data.Bonus.GetLogs(topic.Tid, TableList.GetPostTableId(topic.Tid));
        }
    }
}