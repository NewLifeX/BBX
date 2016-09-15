using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Discuz.Common;

using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Polls
    {
        public static bool CreatePoll(int tid, int multiple, int itemcount, string itemnamelist, string itemvaluelist, string enddatetime, int userid, int maxchoices, int visible, int allowview)
        {
            string[] array = Utils.SplitString(itemnamelist, "\r\n");
            if (array.Length != itemcount || Utils.SplitString(itemvaluelist, "\r\n").Length != itemcount)
            {
                return false;
            }
            int num = Discuz.Data.Polls.CreatePoll(new PollInfo
            {
                Displayorder = 0,
                Expiration = Utils.GetStandardDateTime(enddatetime),
                Maxchoices = maxchoices,
                Multiple = multiple,
                Tid = tid,
                Uid = userid,
                Visible = visible,
                Allowview = allowview
            });
            if (num > 0)
            {
                for (int i = 0; i < itemcount; i++)
                {
                    Discuz.Data.Polls.CreatePollOption(new PollOptionInfo
                    {
                        Displayorder = i + 1,
                        Pollid = num,
                        Polloption = Utils.GetSubString(array[i], 80, ""),
                        Tid = tid,
                        Voternames = "",
                        Votes = 0
                    });
                }
                return true;
            }
            return false;
        }

        public static bool UpdatePoll(int tid, int multiple, int itemcount, string polloptionidlist, string itemnamelist, string itemdisplayorderlist, string enddatetime, int maxchoices, int visible, int allowview)
        {
            string[] array = Utils.SplitString(itemnamelist, "\r\n");
            string[] array2 = Utils.SplitString(itemdisplayorderlist, "\r\n");
            string[] array3 = Utils.SplitString(polloptionidlist, "\r\n");
            if (array.Length != itemcount || array2.Length != itemcount)
            {
                return false;
            }
            PollInfo pollInfo = Discuz.Data.Polls.GetPollInfo(tid);
            pollInfo.Expiration = Utils.GetStandardDateTime(enddatetime);
            pollInfo.Maxchoices = maxchoices;
            pollInfo.Multiple = multiple;
            pollInfo.Tid = tid;
            pollInfo.Visible = visible;
            pollInfo.Allowview = allowview;
            bool flag = false;
            if (pollInfo.Pollid > 0)
            {
                flag = Discuz.Data.Polls.UpdatePoll(pollInfo);
            }
            if (flag)
            {
                List<PollOptionInfo> pollOptionInfoCollection = Discuz.Data.Polls.GetPollOptionInfoCollection(pollInfo.Tid);
                int num = 0;
                string[] array4 = array3;
                for (int i = 0; i < array4.Length; i++)
                {
                    string a = array4[i];
                    bool flag2 = false;
                    foreach (PollOptionInfo current in pollOptionInfoCollection)
                    {
                        if (a == current.Polloptionid.ToString())
                        {
                            current.Pollid = pollInfo.Pollid;
                            current.Polloption = Utils.GetSubString(array[num], 80, "");
                            current.Displayorder = (Utils.StrIsNullOrEmpty(array2[num]) ? (num + 1) : Utils.StrToInt(array2[num], 0));
                            Discuz.Data.Polls.UpdatePollOption(current);
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        Discuz.Data.Polls.CreatePollOption(new PollOptionInfo
                        {
                            Displayorder = Utils.StrIsNullOrEmpty(array2[num]) ? (num + 1) : Utils.StrToInt(array2[num], 0),
                            Pollid = pollInfo.Pollid,
                            Polloption = Utils.GetSubString(array[num], 80, ""),
                            Tid = tid,
                            Voternames = "",
                            Votes = 0
                        });
                    }
                    num++;
                }
                foreach (PollOptionInfo current2 in pollOptionInfoCollection)
                {
                    if (("\r\n" + polloptionidlist + "\r\n").IndexOf("\r\n" + current2.Polloptionid + "\r\n") < 0)
                    {
                        Discuz.Data.Polls.DeletePollOption(current2);
                    }
                }
                return true;
            }
            return false;
        }

        public static int UpdatePoll(int tid, string selitemidlist, string username)
        {
            if (Utils.StrIsNullOrEmpty(username))
            {
                return -1;
            }
            string[] array = Utils.SplitString(selitemidlist, ",");
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string expression = array2[i];
                if (Utils.StrToInt(expression, -1) == -1)
                {
                    return -1;
                }
            }
            PollInfo pollInfo = Discuz.Data.Polls.GetPollInfo(tid);
            if (pollInfo.Pollid < 1)
            {
                return -3;
            }
            if (Utils.StrIsNullOrEmpty(pollInfo.Voternames))
            {
                pollInfo.Voternames = username;
            }
            else
            {
                pollInfo.Voternames = pollInfo.Voternames + "\r\n" + username;
            }
            Discuz.Data.Polls.UpdatePoll(pollInfo);
            List<PollOptionInfo> pollOptionInfoCollection = Discuz.Data.Polls.GetPollOptionInfoCollection(pollInfo.Tid);
            string[] array3 = array;
            for (int j = 0; j < array3.Length; j++)
            {
                string a = array3[j];
                foreach (PollOptionInfo current in pollOptionInfoCollection)
                {
                    if (a == current.Polloptionid.ToString())
                    {
                        if (Utils.StrIsNullOrEmpty(current.Voternames))
                        {
                            current.Voternames = username;
                        }
                        else
                        {
                            current.Voternames = current.Voternames + "\r\n" + username;
                        }
                        current.Votes++;
                        DatabaseProvider.GetInstance().UpdatePollOption(current);
                    }
                }
            }
            return 0;
        }

        public static DataTable GetPollOptionList(int tid)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("name", typeof(String));
            dataTable.Columns.Add("value", typeof(String));
            dataTable.Columns.Add("barid", typeof(Int32));
            dataTable.Columns.Add("barwidth", typeof(Double));
            dataTable.Columns.Add("percent", typeof(String));
            dataTable.Columns.Add("multiple", typeof(String));
            dataTable.Columns.Add("polloptionid", typeof(Int32));
            dataTable.Columns.Add("displayorder", typeof(Int32));
            dataTable.Columns.Add("votername", typeof(String));
            dataTable.Columns.Add("percentwidth", typeof(String));
            List<PollOptionInfo> pollOptionInfoCollection = Discuz.Data.Polls.GetPollOptionInfoCollection(tid);
            int num = 0;
            int num2 = 0;
            int multiple = Discuz.Data.Polls.GetPollInfo(tid).Multiple;
            foreach (PollOptionInfo current in pollOptionInfoCollection)
            {
                num += current.Votes;
                num2 = ((current.Votes > num2) ? current.Votes : num2);
            }
            if (num == 0)
            {
                num = 1;
            }
            int num3 = 0;
            string text = "";
            foreach (PollOptionInfo current2 in pollOptionInfoCollection)
            {
                object[] array = new object[10];
                array[0] = current2.Polloption;
                array[1] = current2.Votes;
                array[2] = num3 % 10;
                array[3] = ((double)(Utils.StrToFloat(current2.Votes, 0f) * 100f / (float)num) / 100.0 * 200.0 + 3.0).ToString("0.00");
                array[4] = ((double)(Utils.StrToFloat(current2.Votes, 0f) * 100f / (float)num) / 100.0).ToString("0.00%");
                array[5] = multiple;
                array[6] = current2.Polloptionid;
                array[7] = current2.Displayorder;
                if (!Utils.StrIsNullOrEmpty(current2.Voternames))
                {
                    string[] array2 = Utils.SplitString(current2.Voternames, "\r\n");
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text2 = array2[i];
                        string text3 = text;
                        text = text3 + "<a href=\"userinfo.aspx?username=" + Utils.UrlEncode(text2.Trim()) + "\">" + text2.Trim() + "</a> ";
                    }
                    array[8] = text;
                    text = "";
                }
                else
                {
                    array[8] = "";
                }
                array[9] = (420.0 * (double)(Utils.StrToFloat(current2.Votes, 0f) / (float)num2)).ToString();
                dataTable.Rows.Add(array);
                num3++;
            }
            return dataTable;
        }

        public static bool AllowVote(int tid, string username)
        {
            if (Utils.StrIsNullOrEmpty(username))
            {
                return false;
            }
            string pollUserNameList = Discuz.Data.Polls.GetPollUserNameList(tid);
            if (Utils.StrIsNullOrEmpty(pollUserNameList))
            {
                return true;
            }
            string[] array = Utils.SplitString(pollUserNameList.Trim(), "\r\n");
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (text.Trim() == username)
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetVoters(int tid, int userid, string username, out bool allowvote)
        {
            string pollUserNameList = Discuz.Data.Polls.GetPollUserNameList(tid);
            allowvote = true;
            if (Utils.StrIsNullOrEmpty(pollUserNameList))
            {
                return "<li>暂无人投票</li>";
            }
            string[] array = Utils.SplitString(pollUserNameList.Trim(), "\r\n");
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Trim() == username)
                {
                    allowvote = false;
                }
                if (userid == -1 && Utils.InArray(tid.ToString(), ForumUtils.GetCookie("dnt_polled")))
                {
                    allowvote = false;
                }
                if (array[i].IndexOf(' ') == -1)
                {
                    stringBuilder.AppendFormat("<li><a href=\"userinfo.aspx?username={0}\">{1}</a></li>", Utils.UrlEncode(array[i].Trim()), array[i]);
                }
                else
                {
                    stringBuilder.Append(array[i].Substring(0, array[i].LastIndexOf(".") + 1).Trim().Replace(" ", string.Empty) + "]");
                }
                stringBuilder.Append("&nbsp; ");
            }
            return stringBuilder.ToString();
        }

        public static string GetPollEnddatetime(int tid)
        {
            return Discuz.Data.Polls.GetPollEnddatetime(tid);
        }

        public static PollInfo GetPollInfo(int tid)
        {
            return Discuz.Data.Polls.GetPollInfo(tid);
        }

        public static string CreatePoll(string pollItemName, int multiple, int maxchoices, int visiblepoll, int allowview, string enddatetime, int tid, string[] pollitem, int userid)
        {
            string result = null;
            StringBuilder stringBuilder = new StringBuilder("");
            for (int i = 0; i < pollitem.Length; i++)
            {
                stringBuilder.Append("0\r\n");
            }
            string text = Utils.HtmlEncode(pollItemName);
            if (text != "")
            {
                if (multiple <= 0)
                {
                    multiple = 0;
                }
                if (multiple == 1 && maxchoices > pollitem.Length)
                {
                    maxchoices = pollitem.Length;
                }
                if (!Polls.CreatePoll(tid, multiple, pollitem.Length, text.Trim(), stringBuilder.ToString().Trim(), enddatetime, userid, maxchoices, visiblepoll, allowview))
                {
                    result = "投票错误";
                }
            }
            else
            {
                result = "投票项为空";
            }
            return result;
        }
    }
}