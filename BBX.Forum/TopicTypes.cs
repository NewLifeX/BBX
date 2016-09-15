using System;
using System.Collections.Generic;
using System.Data;
using Discuz.Common;

using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class TopicTypes
    {
        public static DataTable GetTopicTypes(string searthKeyWord)
        {
            return Discuz.Data.TopicTypes.GetTopicTypes(searthKeyWord);
        }

        public static DataTable GetTopicTypes()
        {
            return TopicTypes.GetTopicTypes("");
        }

        public static void UpdateTopicTypes(string name, int displayorder, string description, int typeid)
        {
            Discuz.Data.TopicTypes.UpdateTopicTypes(name, displayorder, description, typeid);
        }

        public static void UpdateForumTopicType(string topictypes, int fid)
        {
            Discuz.Data.TopicTypes.UpdateForumTopicType(topictypes, fid);
        }

        public static bool IsExistTopicType(string topicTypeName)
        {
            foreach (DataRow dataRow in TopicTypes.GetTopicTypes().Rows)
            {
                if (dataRow["name"].ToString() == topicTypeName)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsExistTopicType(string topicTypeName, int typeid)
        {
            foreach (DataRow dataRow in TopicTypes.GetTopicTypes().Rows)
            {
                if (dataRow["name"].ToString() == topicTypeName && dataRow["id"].ToString() != typeid.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        public static void CreateTopicTypes(string typeName, int displayorder, string description)
        {
            Discuz.Data.TopicTypes.CreateTopicTypes(typeName, displayorder, description);
        }

        public static void DeleteTopicTypes(string typeidList)
        {
            Discuz.Data.TopicTypes.DeleteTopicTypes(typeidList);
        }

        public static int GetMaxTopicTypesId()
        {
            DataTable topicTypes = TopicTypes.GetTopicTypes();
            if (topicTypes != null)
            {
                return TypeConverter.ObjectToInt(topicTypes.Compute("Max(id)", ""));
            }
            return 0;
        }

        public static void DeleteForumTopicTypes(string typeidlist)
        {
            string[] array = typeidlist.Split(',');
            SortedList<int, string> sortedList = new SortedList<int, string>();
            sortedList = Caches.GetTopicTypeArray();
            DataTable forumListForDataTable = Forums.GetForumListForDataTable();
            foreach (DataRow dataRow in forumListForDataTable.Rows)
            {
                if (!(dataRow["topictypes"].ToString() == ""))
                {
                    string text = dataRow["topictypes"].ToString();
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text2 = array2[i];
                        text = text.Replace(text2 + "," + sortedList[int.Parse(text2)].ToString() + ",0|", "");
                        text = text.Replace(text2 + "," + sortedList[int.Parse(text2)].ToString() + ",1|", "");
                        Discuz.Data.Topics.ClearTopicType(int.Parse(text2));
                    }
                    ForumInfo forumInfo = Forums.GetForumInfo(int.Parse(dataRow["fid"].ToString()));
                    forumInfo.Topictypes = text;
                    AdminForums.UpdateForumInfo(forumInfo);
                }
            }
        }
    }
}