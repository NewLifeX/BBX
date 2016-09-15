using System;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class QzoneConnectContext
    {
        #region 常量
        const String CONNECT_URL = "https://graph.qq.com/";
        const String AUTHORIZE_URL = CONNECT_URL + "oauth2.0/authorize?response_type=code&client_id={0}&state={1}&redirect_uri={2}";
        const String ACCESS_URL = CONNECT_URL + "oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&state={3}&redirect_uri={4}";
        const String OPEN_ID_URL = CONNECT_URL + "oauth2.0/me?access_token={0}";
        const String GET_USER_INFO = CONNECT_URL + "user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}&format=xml";
        #endregion

        #region 属性
        String _state = null;
        String _redirect = null;

        private QzoneConnectConfigInfo Config { get; set; }

        /// <summary>当前用户令牌对象</summary>
        public QzoneConnectToken Token { get; private set; }

        /// <summary>回调地址</summary>
        public String Callback { get; private set; }

        /// <summary>是否在线用户绑定</summary>
        public bool IsOnlineUserBindConnect { get { return Token != null; } }
        #endregion

        #region 构造
        const String QZONE_CONNECT_CONTEXT_SESSION_KEY = "QzoneConnectContext";
        /// <summary>当前QQ令牌上下文</summary>
        public static QzoneConnectContext Current
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null || context.Session == null) return null;

                var connect = context.Session[QZONE_CONNECT_CONTEXT_SESSION_KEY] as QzoneConnectContext;
                if (connect == null)
                {
                    connect = new QzoneConnectContext();
                    //var uid = Utils.StrToInt(ForumUtils.GetCookie("userid"), -1);
                    //connect.Token = QzoneConnectToken.FindByUid(ForumUtils.GetCookie("userid").ToInt());
                }

                return connect;
            }
            set
            {
                var context = HttpContext.Current;
                if (context == null || context.Session == null) return;

                context.Session[QZONE_CONNECT_CONTEXT_SESSION_KEY] = value;
            }
        }

        public QzoneConnectContext(String callback = null)
        {
            var cfg = BaseConfigInfo.Current;
            if (String.IsNullOrEmpty(callback)) callback = cfg.Forumpath;
            Callback = callback;

            Config = QzoneConnectConfigInfo.Current;

            var context = HttpContext.Current;
            var req = context.Request;
            _redirect = String.Format("{0}://{1}{2}", req.Url.Scheme, req.Url.Host, cfg.Forumpath + Config.QzoneConnectPage + HttpUtility.UrlEncode("?act=access&callback=" + callback));

            var _rnd = new Random();
            _state = _rnd.Next(100000, 999999).ToString();

            Current = this;

            //var uid = Utils.StrToInt(ForumUtils.GetCookie("userid"), -1);
            Token = QzoneConnectToken.FindByUid(ForumUtils.GetCookie("userid").ToInt());
        }
        #endregion

        #region 授权动作
        /// <summary>第一步获取QQ验证Url跳过去登录</summary>
        /// <returns></returns>
        public String GetAuthorizationUrl()
        {
            return String.Format(AUTHORIZE_URL, Config.AppKey, _state, _redirect);
        }

        /// <summary>第二步经过QQ验证后得到授权码code</summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public QzoneConnectToken GetAccessToken(String code)
        {
            if (String.IsNullOrEmpty(code)) throw new ArgumentNullException("code");

            // 根据授权码code直接向服务器请求得到访问令牌
            var url = String.Format(ACCESS_URL, Config.AppKey, Config.AppSecret, code, _state, _redirect);
            var tokenSeg = WebRequest(url).Split(new[] { '=', '&' });

            // 同时获取OpenID，作为用户唯一标识
            url = String.Format(OPEN_ID_URL, tokenSeg[1]);
            var openidSeg = WebRequest(url).Split('"');

            var token = QzoneConnectToken.FindByOpenId(openidSeg[7]);
            if (token == null) token = new QzoneConnectToken();
            token.OpenId = openidSeg[7];
            token.AccessToken = tokenSeg[1];
            token.ExpiresAt = DateTime.Now.AddSeconds(int.Parse(tokenSeg[3]));

            Token = token;

            return token;
        }

        /// <summary>取消绑定</summary>
        public void UnbindToken()
        {
            Token = null;
            Current = null;
        }
        #endregion

        #region 获取用户信息
        /// <summary>获取昵称</summary>
        /// <returns></returns>
        public String GetQqNickname()
        {
            try
            {
                var xdoc = GetUserInfo();

                var node = xdoc.SelectSingleNode("/data/nickname");
                if (node != null) return node.InnerText;
            }
            catch { }

            return String.Empty;
        }

        /// <summary>获取性别</summary>
        /// <returns>male return 1, female return 2, otherwise return 0</returns>
        public int GetQqSex()
        {
            try
            {
                var xdoc = GetUserInfo();

                var node = xdoc.SelectSingleNode("/data/gender");
                if (node != null)
                {
                    if (node.InnerText == "男")
                        return 1;
                    else if (node.InnerText == "女")
                        return 2;
                    else
                        return 0;
                }
            }
            catch { }

            return 0;
        }

        /// <summary>获取头像地址</summary>
        /// <returns></returns>
        public String GetAvatarUrl()
        {
            try
            {
                var xdoc = GetUserInfo();

                var node = xdoc.SelectSingleNode("/data/figureurl_2");
                if (node != null) return node.InnerText;
            }
            catch { }

            return String.Empty;
        }

        XmlDocument _xUserInfo = null;
        /// <summary>获取用户XML</summary>
        /// <returns></returns>
        private XmlDocument GetUserInfo()
        {
            if (_xUserInfo == null)
            {
                try
                {
                    var url = String.Format(GET_USER_INFO, Token.AccessToken, Config.AppKey, Token.OpenId);
                    var xml = WebRequest(url);
                    _xUserInfo = new XmlDocument();
                    _xUserInfo.LoadXml(xml);
                }
                catch { }
            }

            return _xUserInfo;
        }
        #endregion

        #region 辅助
        WebClient _wc = null;
        private String WebRequest(String url)
        {
            if (_wc == null)
            {
                _wc = new WebClient();
                _wc.Encoding = Encoding.UTF8;
            }
            return _wc.DownloadString(url);
        }
        #endregion
    }
}