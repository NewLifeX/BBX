using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using NewLife.Log;
using Newtonsoft.Json;

namespace Discuz.Forum
{
    public class DiscuzCloud
    {
        private const string CLOUD_URL = API_CONNECT_URL + "site.php";
        private const string CLOUD_CP_URL = "http://cp.discuz.qq.com/";
        private const string CONNECT_URL = "http://connect.discuz.qq.com/";
        private const string API_CONNECT_URL = "http://api.discuz.qq.com/";
        private const string PRODUCT_RELEASE = "20110701";
        private const string REQUEST_TOKEN_URL = API_CONNECT_URL + "oauth/requestToken";
        private const string AUTHORIZE_URL = CONNECT_URL + "oauth/authorize";
        private const string ACCESS_TOKEN_URL = API_CONNECT_URL + "oauth/accessToken";
        private const string UNBIND_URL = API_CONNECT_URL + "connect/user/unbind";
        private const string FORMAT = "json";
        private const string CHARSET = "utf-8";
        private static string productType = Utils.ProductName;
        private static string productVersion = Utils.Version;
        private static string oauthCallback = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "connect.aspx?action=access";

        public static string RegisterSite()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            DiscuzCloudMethodParameter discuzCloudMethodParameter = new DiscuzCloudMethodParameter();
            discuzCloudMethodParameter.Add("sName", GeneralConfigInfo.Current.Forumtitle);
            discuzCloudMethodParameter.Add("sSiteKey", config.Sitekey);
            discuzCloudMethodParameter.Add("sCharset", CHARSET);
            discuzCloudMethodParameter.Add("sTimeZone", "8");
            discuzCloudMethodParameter.Add("sLanguage", "zh_CN");
            discuzCloudMethodParameter.Add("sProductType", DiscuzCloud.productType);
            discuzCloudMethodParameter.Add("sProductVersion", DiscuzCloud.productVersion);
            discuzCloudMethodParameter.Add("sTimestamp", Utils.ConvertToUnixTimestamp(DateTime.Now).ToString());
            discuzCloudMethodParameter.Add("sApiVersion", "0.4");
            discuzCloudMethodParameter.Add("sSiteUid", BaseConfigs.GetFounderUid.ToString());
            discuzCloudMethodParameter.Add("sProductRelease", PRODUCT_RELEASE);
            discuzCloudMethodParameter.Add("sUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            discuzCloudMethodParameter.Add("sUCenterUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            BaseCloudResponse<RegisterCloud> cloudResponse = DiscuzCloud.GetCloudResponse<RegisterCloud>("site.register", discuzCloudMethodParameter);
            if (cloudResponse.ErrCode == 0)
            {
                config.Cloudsiteid = cloudResponse.Result.CloudSiteId;
                config.Cloudsitekey = cloudResponse.Result.CloudSiteKey;

                //DiscuzCloudConfigs.SaveConfig(config);
                //DiscuzCloudConfigs.ResetConfig();
                config.Save();
                DiscuzCloudConfigInfo.Current = null;
            }
            return cloudResponse.ErrMessage;
        }

        public static string GetCloudAppListIFrame(int userId)
        {
            DiscuzCloudMethodParameter discuzCloudMethodParameter = new DiscuzCloudMethodParameter(false);
            discuzCloudMethodParameter.Add("refer", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            discuzCloudMethodParameter.Add("s_id", DiscuzCloudConfigInfo.Current.Cloudsiteid);
            discuzCloudMethodParameter.Add("s_site_uid", userId.ToString());
            return DiscuzCloud.GetCloudCpUrl("cloud/appList/", discuzCloudMethodParameter);
        }

        public static string GetCloudBindUrl(int userId)
        {
            DiscuzCloudMethodParameter discuzCloudMethodParameter = new DiscuzCloudMethodParameter(false);
            discuzCloudMethodParameter.Add("s_id", DiscuzCloudConfigInfo.Current.Cloudsiteid);
            discuzCloudMethodParameter.Add("s_site_uid", userId.ToString());
            return DiscuzCloud.GetCloudCpUrl("bind/index", discuzCloudMethodParameter);
        }

        public static string GetCloudUploadLogoIFrame(int userId)
        {
            DiscuzCloudMethodParameter discuzCloudMethodParameter = new DiscuzCloudMethodParameter(false);
            discuzCloudMethodParameter.Add("s_id", DiscuzCloudConfigInfo.Current.Cloudsiteid);
            discuzCloudMethodParameter.Add("s_site_uid", userId.ToString());
            discuzCloudMethodParameter.Add("link_url", "admin/global/global_connectset.aspx");
            discuzCloudMethodParameter.Add("self_url", "admin/global/global_connectset.aspx?upload=1");
            return DiscuzCloud.GetCloudCpUrl("connect/service", discuzCloudMethodParameter);
        }

        public static string SyncSite()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            DiscuzCloudMethodParameter discuzCloudMethodParameter = new DiscuzCloudMethodParameter();
            discuzCloudMethodParameter.Add("sId", config.Cloudsiteid);
            discuzCloudMethodParameter.Add("sName", GeneralConfigInfo.Current.Forumtitle);
            discuzCloudMethodParameter.Add("sSiteKey", config.Sitekey);
            discuzCloudMethodParameter.Add("sCharset", CHARSET);
            discuzCloudMethodParameter.Add("sTimeZone", "8");
            discuzCloudMethodParameter.Add("sLanguage", "zh_CN");
            discuzCloudMethodParameter.Add("sProductType", DiscuzCloud.productType);
            discuzCloudMethodParameter.Add("sProductVersion", DiscuzCloud.productVersion);
            discuzCloudMethodParameter.Add("sApiVersion", "0.4");
            discuzCloudMethodParameter.Add("sSiteUid", BaseConfigs.GetFounderUid.ToString());
            discuzCloudMethodParameter.Add("sProductRelease", PRODUCT_RELEASE);
            discuzCloudMethodParameter.Add("sTimestamp", Utils.ConvertToUnixTimestamp(DateTime.Now).ToString());
            discuzCloudMethodParameter.Add("sUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            discuzCloudMethodParameter.Add("sUCenterUrl", Utils.GetRootUrl(BaseConfigs.GetForumPath));
            BaseCloudResponse<bool> cloudResponse = DiscuzCloud.GetCloudResponse<bool>("site.sync", discuzCloudMethodParameter);
            return cloudResponse.ErrMessage;
        }

        public static string ResetSiteKey()
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            DiscuzCloudMethodParameter discuzCloudMethodParameter = new DiscuzCloudMethodParameter();
            discuzCloudMethodParameter.Add("sId", config.Cloudsiteid);
            BaseCloudResponse<RegisterCloud> cloudResponse = DiscuzCloud.GetCloudResponse<RegisterCloud>("site.resetKey", discuzCloudMethodParameter);
            if (cloudResponse.ErrCode == 0)
            {
                config.Cloudsitekey = cloudResponse.Result.CloudSiteKey;

                //DiscuzCloudConfigs.SaveConfig(config);
                //DiscuzCloudConfigs.ResetConfig();
                config.Save();
                DiscuzCloudConfigInfo.Current = null;
            }
            return cloudResponse.ErrMessage;
        }

        public static bool GetCloudServiceEnableStatus(string serviceName)
        {
            var config = DiscuzCloudConfigInfo.Current;
            if (config.Cloudenabled == 0) return false;

            if (serviceName != null)
            {
                if (serviceName == "connect") return config.Connectenabled == 1;

                if (serviceName == "connect_reg") return config.Allowconnectregister == 1;
            }
            return false;
        }

        public static string GetConnectLoginPageUrl(int userId)
        {
            var config = DiscuzCloudConfigInfo.Current;
            var list = new List<DiscuzOAuthParameter>();
            list.Add(new DiscuzOAuthParameter("client_ip", DNTRequest.GetIP()));
            list.Add(new DiscuzOAuthParameter("type", (userId > 0) ? "loginbind" : "login"));
            DiscuzOAuth discuzOAuth = new DiscuzOAuth();
            string text = "";
            string callback = DiscuzCloud.oauthCallback;
            var request = HttpContext.Current.Request;
            var referrer = request.UrlReferrer;
            if (referrer != null && !referrer.PathAndQuery.StartsWith("/logout.aspx", StringComparison.OrdinalIgnoreCase)
                && !referrer.PathAndQuery.StartsWith("/login.aspx", StringComparison.OrdinalIgnoreCase)
                && !referrer.PathAndQuery.StartsWith("/register.aspx", StringComparison.OrdinalIgnoreCase)
                && !referrer.PathAndQuery.StartsWith("/connect.aspx", StringComparison.OrdinalIgnoreCase))
                callback += "&url=" + referrer.PathAndQuery;
            XTrace.WriteLine(callback);
            string oAuthUrl = discuzOAuth.GetOAuthUrl(REQUEST_TOKEN_URL, "POST", config.Connectappid, config.Connectappkey, "", "", "", callback, list, out text);
            string httpWebResponse = Utils.GetHttpWebResponse(oAuthUrl, text);
            string result;
            try
            {
                var connectResponse = JavaScriptConvert.DeserializeObject<ConnectResponse<OAuthTokenInfo>>(httpWebResponse);

                //Utils.WriteCookie("connect", "token", connectResponse.Result.Token);
                //Utils.WriteCookie("connect", "secret", connectResponse.Result.Secret);
                var Session = HttpContext.Current.Session;
                Session["connect_token"] = connectResponse.Result.Token;
                Session["connect_secret"] = connectResponse.Result.Secret;
                XTrace.WriteLine("IP {0}, token {1}, secret {2}, verifier {3}, sid {4}, url {5}", DNTRequest.GetIP(), Session["connect_token"], Session["connect_secret"], DNTRequest.GetString("con_oauth_verifier"), Session.SessionID, request.Url);

                string oAuthUrl2 = discuzOAuth.GetOAuthUrl(AUTHORIZE_URL, "GET", config.Connectappid, config.Connectappkey, connectResponse.Result.Token, connectResponse.Result.Secret, "", callback, new List<DiscuzOAuthParameter>(), out text);
                result = oAuthUrl2 + "?" + text;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                result = "?Failed to get tmptoken";
            }
            return result;
        }

        public static OAuthAccessTokenInfo GetConnectAccessTokenInfo()
        {
            var config = DiscuzCloudConfigInfo.Current;
            var list = new List<DiscuzOAuthParameter>();
            list.Add(new DiscuzOAuthParameter("client_ip", DNTRequest.GetIP()));
            var discuzOAuth = new DiscuzOAuth();
            string postData = "";

            //var oAuthUrl = discuzOAuth.GetOAuthUrl(ACCESS_TOKEN_URL, "POST", config.Connectappid, config.Connectappkey, Utils.GetCookie("connect", "token"), Utils.GetCookie("connect", "secret"), DNTRequest.GetString("con_oauth_verifier"), "", list, out postData);
            var request = HttpContext.Current.Request;
            var Session = HttpContext.Current.Session;
            var oAuthUrl = discuzOAuth.GetOAuthUrl(ACCESS_TOKEN_URL, "POST", config.Connectappid, config.Connectappkey, Session["connect_token"] + "", Session["connect_secret"] + "", DNTRequest.GetString("con_oauth_verifier"), "", list, out postData);
            var httpWebResponse = Utils.GetHttpWebResponse(oAuthUrl, postData);
            OAuthAccessTokenInfo result;
            try
            {
                var connectResponse = JavaScriptConvert.DeserializeObject<ConnectResponse<OAuthAccessTokenInfo>>(httpWebResponse);
                result = connectResponse.Result;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);

                //XTrace.WriteLine(oAuthUrl);
                XTrace.WriteLine(httpWebResponse);
                XTrace.WriteLine("IP {0}, token {1}, secret {2}, verifier {3}, sid {4}, url {5}", DNTRequest.GetIP(), Session["connect_token"], Session["connect_secret"], DNTRequest.GetString("con_oauth_verifier"), Session.SessionID, request.Url);

                result = null;
            }
            return result;
        }

        public static string GetBindUserNotifyUrl(UserConnect connectInfo, string userName, string birthday, int gender, string email, int isPublicEmail, int isUsedQQAvatar, string type)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            List<DiscuzOAuthParameter> list = new List<DiscuzOAuthParameter>();
            list.Add(new DiscuzOAuthParameter("s_id", string.Empty));
            list.Add(new DiscuzOAuthParameter("openid", connectInfo.OpenId));
            list.Add(new DiscuzOAuthParameter("oauth_consumer_key", config.Connectappid));
            list.Add(new DiscuzOAuthParameter("u_id", connectInfo.Uid.ToString()));
            list.Add(new DiscuzOAuthParameter("username", userName));
            list.Add(new DiscuzOAuthParameter("birthday", birthday));
            string text = "unknown";
            text = ((gender == 1) ? "male" : text);
            text = ((gender == 2) ? "female" : text);
            list.Add(new DiscuzOAuthParameter("sex", text));
            list.Add(new DiscuzOAuthParameter("email", email));
            list.Add(new DiscuzOAuthParameter("is_public_email", isPublicEmail.ToString()));
            list.Add(new DiscuzOAuthParameter("is_use_qq_avatar", isUsedQQAvatar.ToString()));
            list.Add(new DiscuzOAuthParameter("statreferer", "forum"));
            list.Add(new DiscuzOAuthParameter("avatar_input", "234"));
            list.Add(new DiscuzOAuthParameter("avatar_agent", "23432"));
            list.Add(new DiscuzOAuthParameter("type", type));
            list.Add(new DiscuzOAuthParameter("site_ucenter_id", config.Sitekey));
            string str = "";
            string str2 = DiscuzCloud.GenerateNotifySignature(list, config.Connectappid + "|" + config.Connectappkey, out str);
            return CONNECT_URL + "notify/user/bind?" + str + "sig=" + str2;
        }

        private static string GenerateNotifySignature(List<DiscuzOAuthParameter> parms, string secret, out string queryStr)
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            parms.Sort(new ParameterComparer());
            foreach (DiscuzOAuthParameter current in parms)
            {
                stringBuilder2.AppendFormat("{0}={1}&", current.Name, Utils.PHPUrlEncode(current.Value));
                stringBuilder.AppendFormat("{0}={1}", current.Name, current.Value);
            }
            stringBuilder.Append(secret);
            queryStr = stringBuilder2.ToString();
            return Utils.MD5(stringBuilder.ToString());
        }

        public static int UnbindUserConnectInfo(string openId)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            UserConnect userConnectInfo = DiscuzCloud.GetUserConnectInfo(openId);
            if (userConnectInfo == null)
            {
                return -1;
            }
            List<DiscuzOAuthParameter> list = new List<DiscuzOAuthParameter>();
            list.Add(new DiscuzOAuthParameter("client_ip", DNTRequest.GetIP()));
            DiscuzOAuth discuzOAuth = new DiscuzOAuth();
            string postData = "";
            string oAuthUrl = discuzOAuth.GetOAuthUrl(UNBIND_URL, "POST", config.Connectappid, config.Connectappkey, userConnectInfo.Token, userConnectInfo.Secret, "", "", list, out postData);
            Utils.GetHttpWebResponse(oAuthUrl, postData);
            DiscuzCloud.DeleteUserConnectInfo(openId);
            Utils.WriteCookie("bindconnect", "");
            return 1;
        }

        //public static int CreateUserConnectInfo(UserConnect userConnectInfo)
        //{
        //    return UserConnect.Insert(userConnectInfo);
        //}

        public static int UpdateUserConnectInfo(UserConnect userConnectInfo)
        {
            if (userConnectInfo.OpenId.Length < 32 || userConnectInfo.Token.Length < 16 || userConnectInfo.Secret.Length < 16)
            {
                return -1;
            }
            return UserConnect.Update(userConnectInfo);
        }

        public static UserConnect GetUserConnectInfo(string openId)
        {
            if (openId.Length < 32)
            {
                return null;
            }
            return UserConnect.FindByOpenId(openId);
        }

        public static UserConnect GetUserConnectInfo(int userId)
        {
            if (userId < 0)
            {
                return null;
            }
            return UserConnect.FindByUid(userId);
        }

        public static int DeleteUserConnectInfo(string openId)
        {
            if (openId.Length < 32)
            {
                return -1;
            }
            return UserConnect.Delete(UserConnect._.OpenId == openId);
        }

        public static int DeleteUserConnectInfo(int userId)
        {
            if (userId < 1)
            {
                return -1;
            }
            return UserConnect.Delete(UserConnect._.Uid == userId);
        }

        public static bool OnlineUserIsBindConnect(int userId)
        {
            var flag = TypeConverter.IntStringToBoolean(Utils.GetCookie("bindconnect"));
            if (flag && userId > 0)
            {
                flag = DiscuzCloud.IsBindConnect(userId);
                Utils.WriteCookie("bindconnect", TypeConverter.BooleanToIntString(flag));
            }
            if (userId < 1 && flag)
            {
                Utils.WriteCookie("bindconnect", "");
            }
            return flag;
        }

        public static bool IsBindConnect(int userId)
        {
            return DiscuzCloudConfigInfo.Current.Connectenabled != 0 && userId >= 1 && DiscuzCloud.GetUserConnectInfo(userId) != null;
        }

        public static int CreateTopicPushFeedLog(PushfeedLog feedInfo)
        {
            //todo 这里的验证代码应该移到实体类的Valid方法里面
            if (feedInfo == null || feedInfo.ID < 0 || feedInfo.Uid < 0 || feedInfo.AuthorToken.Length < 16 || feedInfo.AuthorSecret.Length < 16)
            {
                return -1;
            }
            return PushfeedLog.Insert(feedInfo);
        }

        public static PushfeedLog GetTopicPushFeedLog(int tid)
        {
            if (tid < 0)
            {
                return null;
            }
            return PushfeedLog.FindByID(tid);
        }

        public static int DeleteTopicPushFeedLog(int tid)
        {
            if (tid < 0)
            {
                return -1;
            }
            //return Discuz.Data.DiscuzCloud.DeleteTopicPushFeedLog(tid);
            return PushfeedLog.Delete(PushfeedLog._.ID == tid);
        }

        public static bool PushFeedToDiscuzCloud(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, UserConnect connectInfo, string ip, string rootUrl)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            List<DiscuzOAuthParameter> list = new List<DiscuzOAuthParameter>();
            list.Add(new DiscuzOAuthParameter("client_ip", ip));
            list.Add(new DiscuzOAuthParameter("thread_id", topic.Tid.ToString()));
            list.Add(new DiscuzOAuthParameter("author_id", topic.Posterid.ToString()));
            list.Add(new DiscuzOAuthParameter("author", topic.Poster));
            list.Add(new DiscuzOAuthParameter("forum_id", topic.Fid.ToString()));
            list.Add(new DiscuzOAuthParameter("p_id", post.Pid.ToString()));
            list.Add(new DiscuzOAuthParameter("subject", topic.Title));
            GeneralConfigInfo config2 = GeneralConfigInfo.Current;
            list.Add(new DiscuzOAuthParameter("html_content", UBB.UBBToHTML(new PostpramsInfo
            {
                Sdetail = post.Message,
                Smiliesinfo = Smilies.GetSmiliesListWithInfo(),
                Bbcodemode = config2.Bbcodemode,
                Parseurloff = post.Parseurloff,
                BBCode = post.Bbcodeoff < 1,
                Signature = 0,
                Allowhtml = post.Htmlon,
                Pid = post.Pid,
                Showimages = 1 - post.Smileyoff,
                Smileyoff = post.Smileyoff,
                Smiliesmax = config2.Smiliesmax,
                Hide = 0
            })));
            list.Add(new DiscuzOAuthParameter("bbcode_content", post.Message));
            list.Add(new DiscuzOAuthParameter("read_permission", "0"));
            list.Add(new DiscuzOAuthParameter("u_id", topic.Posterid.ToString()));
            list.Add(new DiscuzOAuthParameter("f_type", TypeConverter.BooleanToIntString(connectInfo.AllowPushFeed)));
            StringBuilder stringBuilder = new StringBuilder();
            int num = 0;
            if (attachments != null)
            {
                for (int i = 0; i < attachments.Length; i++)
                {
                    AttachmentInfo attachmentInfo = attachments[i];
                    if (num < 3 && attachmentInfo.Filetype.IndexOf("image") > -1 && attachmentInfo.Attachprice <= 0)
                    {
                        stringBuilder.AppendFormat("|{0}upload/{1}", rootUrl, attachmentInfo.Filename.Replace("\\", "/"));
                        num++;
                    }
                }
            }
            list.Add(new DiscuzOAuthParameter("attach_images", stringBuilder.ToString().TrimStart('|')));
            DiscuzOAuth discuzOAuth = new DiscuzOAuth();
            string postData = "";
            string oAuthUrl = discuzOAuth.GetOAuthUrl(API_CONNECT_URL + "connect/feed/new", "POST", config.Connectappid, config.Connectappkey, connectInfo.Token, connectInfo.Secret, "", "", list, out postData);
            Utils.GetHttpWebResponse(oAuthUrl, postData);
            return true;
        }

        public static bool DeletePushedFeedInDiscuzCloud(PushfeedLog feedInfo, string ip)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            List<DiscuzOAuthParameter> list = new List<DiscuzOAuthParameter>();
            list.Add(new DiscuzOAuthParameter("client_ip", ip));
            list.Add(new DiscuzOAuthParameter("thread_id", feedInfo.ID.ToString()));
            DiscuzOAuth discuzOAuth = new DiscuzOAuth();
            string postData = "";
            string oAuthUrl = discuzOAuth.GetOAuthUrl(API_CONNECT_URL + "connect/feed/remove", "POST", config.Connectappid, config.Connectappkey, feedInfo.AuthorToken, feedInfo.AuthorSecret, "", "", list, out postData);
            Utils.GetHttpWebResponse(oAuthUrl, postData);
            return true;
        }

        public static bool GetOnlineUserCloudFeedStatus(int userId)
        {
            string cookie = Utils.GetCookie("cloud_feed_status");
            if (!string.IsNullOrEmpty(cookie))
            {
                string[] array = cookie.Split('|');
                if (array.Length == 2 && userId == TypeConverter.StrToInt(array[0], -1))
                {
                    return TypeConverter.IntStringToBoolean(array[1]);
                }
            }
            var userConnectInfo = DiscuzCloud.GetUserConnectInfo(userId);
            if (userConnectInfo != null)
            {
                Utils.WriteCookie("cloud_feed_status", string.Format("{0}|{1}", userId, Convert.ToInt32(userConnectInfo.AllowPushFeed)));
                return userConnectInfo.AllowPushFeed;
            }
            return false;
        }

        public static int CreateUserConnectBindLog(ConnectbindLog bindLog)
        {
            //todo 外部应该直接调用Insert
            return ConnectbindLog.Insert(bindLog);
        }

        public static int UpdateUserConnectBindLog(ConnectbindLog bindLog)
        {
            //todo 外部应该直接调用Insert
            return ConnectbindLog.Update(bindLog);
        }

        public static ConnectbindLog GetUserConnectBindLog(string openId)
        {
            return ConnectbindLog.FindByOpenID(openId);
        }

        private static BaseCloudResponse<T> GetCloudResponse<T>(string method, DiscuzCloudMethodParameter mParams)
        {
            string text = mParams.Find("sTimestamp");
            text = (string.IsNullOrEmpty(text) ? Utils.ConvertToUnixTimestamp(DateTime.Now).ToString() : text);
            string postData = string.Format("format={0}&method={1}&sId={2}&sig={3}&ts={4}{5}", new object[]
            {
                FORMAT,
                method,
                DiscuzCloudConfigInfo.Current.Cloudsiteid,
                DiscuzCloud.GetCloudMethodSignature(method, text, mParams),
                text,
                mParams.GetPostData()
            });
            string httpWebResponse = Utils.GetHttpWebResponse(CLOUD_URL, postData);
            BaseCloudResponse<T> result;
            try
            {
                result = JavaScriptConvert.DeserializeObject<BaseCloudResponse<T>>(httpWebResponse);
            }
            catch
            {
                BaseCloudResponse<string> baseCloudResponse = JavaScriptConvert.DeserializeObject<BaseCloudResponse<string>>(httpWebResponse);
                result = new BaseCloudResponse<T>
                {
                    ErrCode = baseCloudResponse.ErrCode,
                    ErrMessage = baseCloudResponse.ErrMessage
                };
            }
            return result;
        }

        private static string GetCloudMethodSignature(string method, string timeStamp, DiscuzCloudMethodParameter mParams)
        {
            DiscuzCloudConfigInfo config = DiscuzCloudConfigInfo.Current;
            return Utils.MD5(string.Format("format={0}&method={1}&sId={2}{3}|{4}|{5}", new object[]
            {
                FORMAT,
                method,
                config.Cloudsiteid,
                mParams.GetPostData(),
                config.Cloudsitekey,
                timeStamp
            }));
        }

        private static string GetCloudIframeSignature(DiscuzCloudMethodParameter mParams, string timeStamp)
        {
            return Utils.MD5(string.Format("{0}|{1}|{2}", mParams.GetPostData().TrimStart('&'), DiscuzCloudConfigInfo.Current.Cloudsitekey, timeStamp));
        }

        private static string GetCloudCpUrl(string target, DiscuzCloudMethodParameter mParams)
        {
            string text = Utils.ConvertToUnixTimestamp(DateTime.Now).ToString();
            return CLOUD_CP_URL + target + "?ts=" + text + mParams.GetPostData() + "&sig=" + DiscuzCloud.GetCloudIframeSignature(mParams, text);
        }
    }
}