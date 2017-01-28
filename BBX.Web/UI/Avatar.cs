using System;
using System.IO;
using System.Web.UI;
using BBX.Common;
using BBX.Config;
using BBX.Forum;
using NewLife.Collections;
using NewLife.Web;

namespace BBX.Web.UI
{
    public class Avatar : Page
    {
        public Avatar() { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            AvatarSize avatarSize;
            switch (DNTRequest.GetString("size").ToLower())
            {
                case "large":
                    avatarSize = AvatarSize.Large;
                    break;
                case "medium":
                    avatarSize = AvatarSize.Medium;
                    break;
                case "small":
                    avatarSize = AvatarSize.Small;
                    break;
                default:
                    avatarSize = AvatarSize.Medium;
                    break;
            }

            var uid = DNTRequest.GetInt("uid");

            // 如果物理文件存在，直接返回
            var fi = Avatars.GetPhysicsAvatarPath(uid + "", avatarSize);
            // 如果没有缓存，文件也不在，那么直接返回默认
            if (!File.Exists(fi)) fi = Utils.GetMapPath(BaseConfigs.GetForumPath.CombinePath("avatars/avatar_" + avatarSize.ToString().ToLower() + ".jpg"));
            if (File.Exists(fi))
            {
                var ci = GetData(fi);

                // 检查缓存是否有效
                var wd = new WebDownload();
                wd.BrowserCache = true;
                wd.ModifyTime = ci.ModifyTime;
                if (wd.CheckCache()) return;

                wd.Stream = new MemoryStream(ci.Data);
                //wd.Mode = WebDownload.DispositionMode.Inline;
                wd.ContentType = "image/" + Path.GetExtension(fi).TrimStart('.');

                // 启用浏览器缓存
                wd.BrowserCache = true;

                wd.Speed = 0;
                wd.Render();

                return;
            }

            //var avatarUrl = Avatars.GetAvatarUrl(uid + "", avatarSize);
            //XTrace.WriteLine("头像{0}未取到，跳转地址 {1}", fi, avatarUrl);
            //HttpContext.Current.Response.Redirect(avatarUrl);
        }

        static DictionaryCache<String, CacheItem> _cache = new DictionaryCache<String, CacheItem>(StringComparer.OrdinalIgnoreCase)
        {
            Expire = 10 * 60
        };
        /// <summary>带缓存的获取头像</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        static CacheItem GetData(String file)
        {
            return _cache.GetItem(file, f =>
            {
                var ci = new CacheItem();
                ci.Data = File.ReadAllBytes(f);
                ci.ModifyTime = new FileInfo(f).LastWriteTime;

                return ci;
            });
        }

        class CacheItem
        {
            public Byte[] Data;
            public DateTime ModifyTime;
        }
    }
}