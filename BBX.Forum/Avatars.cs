using System;
using System.IO;
using BBX.Common;
using BBX.Config;

namespace BBX.Forum
{
    public enum AvatarSize
    {
        Large,
        Medium,
        Small
    }

    public class Avatars
    {
        private static String forumPath = BaseConfigs.GetForumPath;

        static String GetDefault(AvatarSize avatarSize = AvatarSize.Medium)
        {
            return Utils.GetMapPath(forumPath.CombinePath("avatars/avatar_" + avatarSize.ToString().ToLower() + ".jpg"));
        }

        /// <summary>获取头像静态地址</summary>
        /// <param name="uid"></param>
        /// <param name="avatarSize"></param>
        /// <returns></returns>
        static String GetStaticUrl(String uid, AvatarSize avatarSize = AvatarSize.Medium)
        {
            uid = uid.PadLeft(9, '0');
            var text = "";
            switch (avatarSize)
            {
                case AvatarSize.Large:
                    text = "large";
                    break;
                case AvatarSize.Medium:
                    text = "medium";
                    break;
                case AvatarSize.Small:
                    text = "small";
                    break;
            }

            var str = FormatPathPrefix(uid) + text + ".jpg";

            var config = GeneralConfigInfo.Current;
            if (config.ImageServer.IsNullOrEmpty())
            {
                // 如果本地头像不存在，则使用默认头像图片
                var fi = Utils.GetRootUrl(forumPath) + str;
                if (!File.Exists(fi)) fi = GetDefault(avatarSize);
                return fi;
            }
            else
                return config.ImageServer.EnsureEnd("/") + str;
        }

        /// <summary>获取用户头像URL，根据AvatarStatic配置可能是动态或者静态</summary>
        /// <param name="uid"></param>
        /// <param name="avatarSize"></param>
        /// <returns></returns>
        public static String GetAvatarUrl(int uid, AvatarSize avatarSize = AvatarSize.Medium)
        {
            var config = GeneralConfigInfo.Current;
            if (config.AvatarStatic) return GetStaticUrl(uid.ToString(), avatarSize);

            var root = Utils.GetRootUrl(BaseConfigs.GetForumPath);
            if (!config.ImageServer.IsNullOrEmpty()) root = config.ImageServer.EnsureEnd("/");

            return String.Format("{0}tools/avatar.aspx?uid={1}&size={2}", root, uid, avatarSize.ToString().ToLower());
        }

        /// <summary>获取用户头像URL，根据AvatarStatic配置可能是动态或者静态</summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static String GetAvatarUrl(int uid) { return GetAvatarUrl(uid, AvatarSize.Medium); }

        /// <summary>格式化头像路径前缀</summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static String FormatPathPrefix(String uid)
        {
            var us = uid.PadLeft(9, '0');
            return "avatars/" + String.Format("upload/{0}/{1}/{2}/{3}_avatar_", us.Substring(0, 3), us.Substring(3, 2), us.Substring(5, 2), us.Substring(7, 2));
        }

        public static bool ExistAvatar(String uid)
        {
            uid = uid.PadLeft(9, '0');
            return File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Large))
                && File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Medium))
                && File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Small));
        }

        public static String GetPhysicsAvatarPath(String uid, AvatarSize size)
        {
            return Utils.GetMapPath(forumPath + FormatPathPrefix(uid) + size.ToString().ToLower() + ".jpg");
        }

        public static void DeleteAvatar(String uid)
        {
            uid = uid.PadLeft(9, '0');
            if (File.Exists(GetPhysicsAvatarPath(uid, AvatarSize.Large)))
            {
                File.Delete(GetPhysicsAvatarPath(uid, AvatarSize.Large));
                File.Delete(GetPhysicsAvatarPath(uid, AvatarSize.Medium));
                File.Delete(GetPhysicsAvatarPath(uid, AvatarSize.Small));
            }
        }
    }
}