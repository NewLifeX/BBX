using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using BBX.Common;
using BBX.Config;
using NewLife.Threading;

namespace BBX.Forum
{
    /// <summary>API同步</summary>
    public class Sync
    {
        private const string ASYNC_LOGIN = "login";
        private const string ASYNC_LOGOUT = "logout";
        private const string ASYNC_REGISTER = "register";
        private const string ASYNC_DELETE_USER = "deleteuser";
        private const string ASYNC_RENAME_USER = "renameuser";
        private const string ASYNC_UPDATE_PASSWORD = "updatepwd";
        private const string ASYNC_UPDATE_CREDITS = "updatecredits";
        private const string ASYNC_UPDATE_SIGNATURE = "updatesignature";
        private const string ASYNC_UPDATE_PROFILE = "updateprofile";
        private const string ASYNC_NEW_TOPIC = "newtopic";
        private const string ASYNC_REPLY = "reply";
        private const string ASYNC_TEST = "test";
        public static string GetLoginScript(int uId, string userName)
        {
            var stringBuilder = new StringBuilder();
            var list = new List<MyParam>();
            list.Add(MyParam.Create("uid", uId));
            list.Add(MyParam.Create("user_name", userName));
            foreach (var current in Sync.GetAsyncTarget("login"))
            {
                stringBuilder.AppendFormat("<script src=\"{0}\" reload=\"1\"></script>", Sync.GetUrl(current.SyncUrl, current.Secret, "login", list.ToArray()));
            }
            return stringBuilder.ToString();
        }
        public static string GetLogoutScript(int uId)
        {
            var stringBuilder = new StringBuilder();
            var list = new List<MyParam>();
            list.Add(MyParam.Create("uid", uId));
            foreach (var current in Sync.GetAsyncTarget("logout"))
            {
                stringBuilder.AppendFormat("<script src=\"{0}\" reload=\"1\"></script>", Sync.GetUrl(current.SyncUrl, current.Secret, "logout", list.ToArray()));
            }
            return stringBuilder.ToString();
        }
        //public static string Test(string asyncUrl)
        //{
        //	return Utils.GetHttpWebResponse(string.Format("{0}?action={1}", asyncUrl, "test"));
        //}
        public static void UserRegister(int uId, string userName, string password, string apiKey)
        {
            Sync.SendRequest("register", new List<MyParam>
            {
                MyParam.Create("uid", uId),
                MyParam.Create("user_name", userName),
                MyParam.Create("password", password)
            }.ToArray(), apiKey);
        }
        public static void DeleteUsers(string uIds, string apiKey)
        {
            Sync.SendRequest("deleteuser", new List<MyParam>
            {
                MyParam.Create("uids", uIds)
            }.ToArray(), apiKey);
        }
        public static void RenameUser(int uId, string oldUserName, string newUserName, string apiKey)
        {
            Sync.SendRequest("renameuser", new List<MyParam>
            {
                MyParam.Create("uid", uId),
                MyParam.Create("old_user_name", oldUserName),
                MyParam.Create("new_user_name", newUserName)
            }.ToArray(), apiKey);
        }
        public static void UpdatePassword(string userName, string password, string apiKey)
        {
            Sync.SendRequest("updatepwd", new List<MyParam>
            {
                MyParam.Create("user_name", userName),
                MyParam.Create("password", password)
            }.ToArray(), apiKey);
        }
        public static void UpdateCredits(int uId, int creditIndex, string amount, string apiKey)
        {
            Sync.SendRequest("updatecredits", new List<MyParam>
            {
                MyParam.Create("uid", uId),
                MyParam.Create("credit_index", creditIndex.ToString()),
                MyParam.Create("amount", amount)
            }.ToArray(), apiKey);
        }
        public static void UpdateSignature(int uId, string userName, string signature, string apiKey)
        {
            Sync.SendRequest("updatesignature", new List<MyParam>
            {
                MyParam.Create("uid", uId),
                MyParam.Create("user_name", userName),
                MyParam.Create("signature", signature)
            }.ToArray(), apiKey);
        }
        public static void UpdateProfile(int uId, string userName, string apiKey)
        {
            Sync.SendRequest("updateprofile", new List<MyParam>
            {
                MyParam.Create("uid", uId),
                MyParam.Create("user_name", userName)
            }.ToArray(), apiKey);
        }
        public static void NewTopic(string topicId, string title, string author, string authorId, string fid, string apiKey)
        {
            Sync.SendRequest("newtopic", new List<MyParam>
            {
                MyParam.Create("tid", topicId),
                MyParam.Create("title", title),
                MyParam.Create("author", author),
                MyParam.Create("author_id", authorId),
                MyParam.Create("fid", fid)
            }.ToArray(), apiKey);
        }
        public static void Reply(string postId, string topicId, string topicTitle, string poster, string posterId, string fid, string apiKey)
        {
            Sync.SendRequest("reply", new List<MyParam>
            {
                MyParam.Create("pid", postId),
                MyParam.Create("tid", topicId),
                MyParam.Create("topic_title", topicTitle),
                MyParam.Create("poster", poster),
                MyParam.Create("poster_id", posterId),
                MyParam.Create("fid", fid)
            }.ToArray(), apiKey);
        }
        public static bool NeedAsyncLogin()
        {
            return Sync.GetAsyncTarget("login").Count > 0;
        }
        public static bool NeedAsyncLogout()
        {
            return Sync.GetAsyncTarget("logout").Count > 0;
        }
        private static void SendRequest(string action, MyParam[] data, string apiKey)
        {
            foreach (var current in Sync.GetAsyncTarget(action))
            {
                if (current.APIKey != apiKey)
                {
                    ThreadPoolX.QueueUserWorkItem(s =>
                    {
                        var url = Sync.GetUrl(current.SyncUrl, current.Secret, action, data);
                        Utils.GetHttpWebResponse(url);
                    });
                }
            }
        }

        private static string GetUrl(string asyncUrl, string secret, string action, MyParam[] parameters)
        {
            var list = new List<MyParam>(parameters);
            list.Add(MyParam.Create("time", Time()));
            list.Add(MyParam.Create("action", action));
            list.Sort();
            var stringBuilder = new StringBuilder();
            foreach (var current in list)
            {
                if (!string.IsNullOrEmpty(current.Value))
                {
                    stringBuilder.Append(current.ToString());
                }
            }
            stringBuilder.Append(secret);
            byte[] array = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
            var stringBuilder2 = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder2.Append(b.ToString("x2"));
            }
            list.Add(MyParam.Create("sig", stringBuilder2.ToString()));
            var stringBuilder3 = new StringBuilder();
            for (int j = 0; j < list.Count; j++)
            {
                if (j > 0)
                {
                    stringBuilder3.Append("&");
                }
                stringBuilder3.Append(list[j].ToEncodedString());
            }
            return string.Format("{0}?{1}", asyncUrl, stringBuilder3.ToString());
        }

        public static long Time()
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow.Ticks - dateTime.Ticks) / 10000000L;
        }

        private static List<ApplicationInfo> GetAsyncTarget(string action)
        {
            var list = new List<ApplicationInfo>();
            var config = APIConfigInfo.Current;
            if (!config.Enable) return list;

            foreach (var current in config.AppCollection)
            {
                if ((current.SyncMode == 1 || (current.SyncMode == 2 && Utils.InArray(action, current.SyncList))) && !(current.SyncUrl.Trim() == string.Empty))
                {
                    list.Add(current);
                }
            }
            return list;
        }

        class MyParam : IComparable
        {
            private object value;

            private string name;
            public string Name { get { return name; } }

            public string Value
            {
                get
                {
                    if (this.value is Array)
                    {
                        return ConvertArrayToString(this.value as Array);
                    }
                    return this.value + "";
                }
            }

            public string EncodedValue
            {
                get
                {
                    if (this.value is Array)
                    {
                        return HttpUtility.UrlEncode(ConvertArrayToString(this.value as Array));
                    }
                    return HttpUtility.UrlEncode(this.value.ToString());
                }
            }

            protected MyParam(string name, object value)
            {
                this.name = name;
                this.value = value;
            }

            public override string ToString()
            {
                return string.Format("{0}={1}", this.Name, this.Value);
            }

            public string ToEncodedString()
            {
                return string.Format("{0}={1}", this.Name, this.EncodedValue);
            }

            public static MyParam Create(string name, object value)
            {
                return new MyParam(name, value);
            }

            public int CompareTo(object obj)
            {
                if (!(obj is MyParam))
                {
                    return -1;
                }
                return this.name.CompareTo((obj as MyParam).name);
            }

            private static string ConvertArrayToString(Array a)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < a.Length; i++)
                {
                    if (i > 0)
                    {
                        stringBuilder.Append(",");
                    }
                    stringBuilder.Append(a.GetValue(i).ToString());
                }
                return stringBuilder.ToString();
            }
        }
    }
}