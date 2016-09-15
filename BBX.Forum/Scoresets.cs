using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class Scoresets
    {
        private static object lockHelper = new object();
        private static string scoreFilePath = Utils.GetMapPath(BaseConfigs.GetForumPath + "config/scoreset.config");
        public static DataTable GetScoreSet()
        {
            DataTable result;
            lock (lockHelper)
            {
                var cacheService = XCache.Current;
                DataTable dataTable = cacheService.RetrieveObject("/Forum/ScoreSet") as DataTable;
                if (dataTable == null)
                {
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(scoreFilePath);
                    dataTable = dataSet.Tables[0];
                    XCache.Add("/Forum/ScoreSet", dataTable, scoreFilePath);
                }
                result = dataTable;
            }
            return result;
        }
        public static UserExtcreditsInfo GetScoreSet(int extcredits)
        {
            var userExtcreditsInfo = new UserExtcreditsInfo();
            string columnName = "extcredits" + extcredits;
            DataTable scoreSet = GetScoreSet();
            if (extcredits > 0)
            {
                userExtcreditsInfo.Name = scoreSet.Rows[0][columnName].ToString();
                userExtcreditsInfo.Unit = scoreSet.Rows[1][columnName].ToString();
                userExtcreditsInfo.Rate = float.Parse(scoreSet.Rows[2][columnName].ToString());
                userExtcreditsInfo.Init = float.Parse(scoreSet.Rows[3][columnName].ToString());
                userExtcreditsInfo.Topic = float.Parse(scoreSet.Rows[4][columnName].ToString());
                userExtcreditsInfo.Reply = float.Parse(scoreSet.Rows[5][columnName].ToString());
                userExtcreditsInfo.Digest = float.Parse(scoreSet.Rows[6][columnName].ToString());
                userExtcreditsInfo.Upload = float.Parse(scoreSet.Rows[7][columnName].ToString());
                userExtcreditsInfo.Download = float.Parse(scoreSet.Rows[8][columnName].ToString());
                userExtcreditsInfo.Pm = float.Parse(scoreSet.Rows[9][columnName].ToString());
                userExtcreditsInfo.Search = float.Parse(scoreSet.Rows[10][columnName].ToString());
                userExtcreditsInfo.Pay = float.Parse(scoreSet.Rows[11][columnName].ToString());
                userExtcreditsInfo.Vote = float.Parse(scoreSet.Rows[12][columnName].ToString());
            }
            return userExtcreditsInfo;
        }
        public static DataTable GetScorePaySet(int type)
        {
            var cacheService = XCache.Current;
            DataTable dataTable = (type == 0) ? (cacheService.RetrieveObject(CacheKeys.FORUM_SCORE_PAY_SET) as DataTable) : (cacheService.RetrieveObject("/Forum/ScorePaySet1") as DataTable);
            if (dataTable == null)
            {
                DataTable scoreSet = GetScoreSet();
                DataTable dataTable2 = new DataTable();
                dataTable2.Columns.Add("id", typeof(Int32));
                dataTable2.Columns.Add("name", typeof(String));
                dataTable2.Columns.Add("rate", typeof(Single));
                for (int i = 1; i <= 8; i++)
                {
                    bool flag = !Utils.StrIsNullOrEmpty(scoreSet.Rows[0]["extcredits" + i].ToString());
                    if (type == 0)
                    {
                        flag = (flag && scoreSet.Rows[2]["extcredits" + i].ToString() != "0");
                    }
                    if (flag)
                    {
                        DataRow dataRow = dataTable2.NewRow();
                        dataRow["id"] = i;
                        dataRow["name"] = scoreSet.Rows[0]["extcredits" + i.ToString()].ToString().Trim();
                        dataRow["rate"] = (Single)scoreSet.Rows[2]["extcredits" + i].ToDouble();
                        dataTable2.Rows.Add(dataRow);
                    }
                }
                if (type == 0)
                {
                    XCache.Add(CacheKeys.FORUM_SCORE_PAY_SET, dataTable2);
                }
                else
                {
                    XCache.Add("/Forum/ScorePaySet1", dataTable2);
                }
                dataTable = dataTable2;
            }
            return dataTable;
        }
        //public static DataTable GetRateScoreSet()
        //{
        //	var cacheService = XCache.Current;
        //	var dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_RATESCORESET) as DataTable;
        //	if (dataTable == null)
        //	{
        //		var scoreSet = GetScoreSet();
        //		var dataTable2 = new DataTable();
        //		dataTable2.Columns.Add("id", typeof(Int32));
        //		dataTable2.Columns.Add("name", typeof(String));
        //		dataTable2.Columns.Add("rate", typeof(Single));
        //		for (int i = 1; i <= 8; i++)
        //		{
        //			var dataRow = dataTable2.NewRow();
        //			dataRow["id"] = i;
        //			dataRow["name"] = scoreSet.Rows[0]["extcredits" + i].ToString().Trim();
        //			dataRow["rate"] = TypeConverter.ObjectToFloat(scoreSet.Rows[2]["extcredits" + i]);
        //			dataTable2.Rows.Add(dataRow);
        //		}
        //		dataTable = dataTable2;
        //		XCache.Add(CacheKeys.FORUM_RATESCORESET, dataTable);
        //	}
        //	return dataTable;
        //}
        public static string[] GetValidScoreUnit()
        {
            var cacheService = XCache.Current;
            string[] array = cacheService.RetrieveObject(CacheKeys.FORUM_VALID_SCORE_UNIT) as string[];
            if (array == null)
            {
                array = GetValidScore(1);
                XCache.Add(CacheKeys.FORUM_VALID_SCORE_UNIT, array);
            }
            return array;
        }
        public static bool IsSetDownLoadAttachScore()
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/IsSetDownLoadAttachScore") as string;
            if (Utils.StrIsNullOrEmpty(text))
            {
                float[] userExtCredits = GetUserExtCredits(CreditsOperationType.DownloadAttachment);
                float[] array = userExtCredits;
                for (int i = 0; i < array.Length; i++)
                {
                    float num = array[i];
                    if ((double)num < 0.0)
                    {
                        XCache.Add("/Forum/IsSetDownLoadAttachScore", "true");
                        return true;
                    }
                }
            }
            return text.ToBoolean();
        }
        public static float[] GetUserExtCredits(CreditsOperationType creditsOperationType)
        {
            DataRow dataRow = GetScoreSet().Rows[(int)creditsOperationType];
            float[] array = new float[8];
            for (int i = 0; i < 8; i++)
            {
                array[i] = (Single)dataRow["extcredits" + (i + 1)].ToDouble();
            }
            return array;
        }
        private static string[] GetValidScore(int validid)
        {
            string[] array = new string[9];
            array[0] = "";
            DataTable scoreSet = GetScoreSet();
            for (int i = 1; i < 9; i++)
            {
                if (Utils.StrIsNullOrEmpty(scoreSet.Rows[validid]["extcredits" + i].ToString()))
                {
                    array[i] = "";
                }
                else
                {
                    array[i] = scoreSet.Rows[validid]["extcredits" + i].ToString();
                }
            }
            scoreSet.Dispose();
            return array;
        }
        public static string[] GetValidScoreName()
        {
            var cacheService = XCache.Current;
            string[] array = cacheService.RetrieveObject(CacheKeys.FORUM_VALID_SCORE_NAME) as string[];
            if (array == null)
            {
                array = GetValidScore(0);
                XCache.Add(CacheKeys.FORUM_VALID_SCORE_NAME, array);
            }
            return array;
        }
        private static string GetScoresCache(string cacheKey)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/Scoreset/" + cacheKey) as string;
            if (text == null)
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(scoreFilePath);
                text = dataSet.Tables["formula"].Rows[0][cacheKey.ToLower()].ToString();
                XCache.Add("/Forum/Scoreset/" + cacheKey, text);
            }
            return text;
        }
        public static string GetScoreCalFormula()
        {
            return GetScoresCache("FormulaContext");
        }
        public static int GetCreditsTrans()
        {
            return GetScoresCache("CreditsTrans").ToInt();
        }
        public static float GetCreditsTax()
        {
            return (Single)GetScoresCache("CreditsTax").ToDouble();
        }
        public static int GetTransferMinCredits()
        {
            return GetScoresCache("TransferMinCredits").ToInt();
        }
        public static int GetExchangeMinCredits()
        {
            return GetScoresCache("ExchangeMinCredits").ToInt();
        }
        public static int GetMaxIncPerTopic()
        {
            return GetScoresCache("MaxIncPerThread").ToInt();
        }
        public static int GetMaxChargeSpan()
        {
            return GetScoresCache("MaxChargeSpan").ToInt();
        }
        public static bool BetweenTime(string timelist, out string vtime)
        {
            if (!Utils.StrIsNullOrEmpty(timelist))
            {
                string[] array = Utils.SplitString(timelist, "\n");
                if (array.Length > 0)
                {
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text = array2[i];
                        if (Regex.IsMatch(text, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])-(([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9]))$"))
                        {
                            string text2 = text.Substring(0, text.IndexOf("-"));
                            //int num = Utils.StrDateDiffMinutes(text2, 0);
                            string text3 = Utils.CutString(text, text.IndexOf("-") + 1, text.Length - (text.IndexOf("-") + 1));
                            //int num2 = Utils.StrDateDiffMinutes(text3, 0);
                            var dtf = DateTime.Parse("1900-01-01");
                            var dt2 = text2.ToDateTime(dtf);
                            var dt3 = text3.ToDateTime(dtf);
                            var num = (DateTime.Now - dt2).TotalMinutes;
                            var num2 = (DateTime.Now - dt3).TotalMinutes;
                            bool result;
                            if (dt2 < dt3)
                            {
                                if (num <= 0 || num2 >= 0)
                                {
                                    goto IL_EB;
                                }
                                vtime = text;
                                result = true;
                            }
                            else
                            {
                                if ((num >= 0 || num2 >= 0) && (num <= 0 || num2 <= 0 || num2 <= num))
                                {
                                    goto IL_EB;
                                }
                                vtime = text;
                                result = true;
                            }
                            return result;
                        }
                    IL_EB: ;
                    }
                }
            }
            vtime = "";
            return false;
        }
        public static bool BetweenTime(string timelist)
        {
            string text = "";
            return BetweenTime(timelist, out text);
        }
        public static int GetTopicAttachCreditsTrans()
        {
            if (GetCreditsTrans() == 0)
            {
                return 0;
            }
            //DNTCache.Current;
            int num = GetScoresCache("TopicAttachCreditsTrans").ToInt();
            if (num < 1 || num > 8)
            {
                num = GetCreditsTrans();
            }
            return num;
        }
        public static string GetTopicAttachCreditsTransName()
        {
            return GetValidScoreName()[GetTopicAttachCreditsTrans()];
        }
        public static int GetBonusCreditsTrans()
        {
            if (GetCreditsTrans() == 0)
            {
                return 0;
            }
            //DNTCache.Current;
            int num = GetScoresCache("BonusCreditsTrans").ToInt();
            if (num == 0)
            {
                num = GetCreditsTrans();
            }
            return num;
        }
        public static string GetValidScoreNameAndId()
        {
            var names = GetValidScoreName();
            var sb = new StringBuilder();
            for (int i = 1; i < 9; i++)
            {
                if (!String.IsNullOrEmpty(names[i]))
                {
                    sb.Append(i);
                    sb.Append("|");
                    sb.Append(names[i]);
                    sb.Append("|");
                    sb.Append(",");
                }
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
