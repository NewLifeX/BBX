using System;
using System.Drawing;
using System.IO;
using BBX.Common;
using BBX.Config;

namespace BBX.Forum
{
    public class QZoneAvatar
    {
        private delegate bool GetUserQZoneAvatar(QzoneConnectContext userConnectInfo);
        private QZoneAvatar.GetUserQZoneAvatar getAvatar_asyncCallback;
        public void AsyncGetAvatar(QzoneConnectContext userConnectInfo)
        {
            this.getAvatar_asyncCallback = new QZoneAvatar.GetUserQZoneAvatar(this.SaveUserAvatar);
            this.getAvatar_asyncCallback.BeginInvoke(userConnectInfo, null, null);
        }
        private bool SaveUserAvatar(QzoneConnectContext userConnectInfo)
        {
            //string text = Avatars.FormatUid(userConnectInfo.Token.Uid.ToString());
            //string fi = string.Format("{0}avatars/upload/{1}/{2}/{3}/{4}_avatar_", new object[]
            //{
            //    BaseConfigs.GetForumPath,
            //    text.Substring(0, 3),
            //    text.Substring(3, 2),
            //    text.Substring(5, 2),
            //    text.Substring(7, 2)
            //});
            //fi = Utils.GetMapPath(fi);
            var pf = Avatars.FormatPathPrefix(userConnectInfo.Token.Uid.ToString());
            var fi = Utils.GetMapPath(BaseConfigs.GetForumPath.CombinePath(pf));
            //string url = string.Format("http://avatar.connect.discuz.qq.com/{0}/{1}", DiscuzCloudConfigInfo.Current.Connectappid, userConnectInfo.OpenId);
            var url = "";
            if (String.IsNullOrEmpty(url)) return false;

            if (!Directory.Exists(fi))
            {
                Utils.CreateDir(fi);
            }
            if (!Thumbnail.MakeRemoteThumbnailImage(url, fi + "large.jpg", 200, 200)) return false;

            var image = Image.FromFile(fi + "large.jpg");
            if ((double)image.Width * 0.8 <= 130.0)
            {
                Thumbnail.MakeThumbnailImage(fi + "large.jpg", fi + "medium.jpg", (int)((double)image.Width * 0.8), (int)((double)image.Height * 0.8));
                Thumbnail.MakeThumbnailImage(fi + "large.jpg", fi + "small.jpg", (int)((double)image.Width * 0.6), (int)((double)image.Height * 0.6));
            }
            else
            {
                Thumbnail.MakeThumbnailImage(fi + "large.jpg", fi + "medium.jpg", (int)((double)image.Width * 0.5), (int)((double)image.Height * 0.5));
                Thumbnail.MakeThumbnailImage(fi + "large.jpg", fi + "small.jpg", (int)((double)image.Width * 0.3), (int)((double)image.Height * 0.3));
            }
            try
            {
                image.Dispose();
            }
            catch { }
            //if (FTPs.GetForumAvatarInfo.Allowupload == 1)
            //{
            //    var fTPs = new FTPs();
            //    //string path = string.Format("/avatars/upload/{0}/{1}/{2}/", text.Substring(0, 3), text.Substring(3, 2), text.Substring(5, 2));
            //    var path = "/" + Avatars.FormatPathPrefix(pf);
            //    fTPs.UpLoadFile(path, fi + "large.jpg", FTPs.FTPUploadEnum.ForumAvatar);
            //    fTPs.UpLoadFile(path, fi + "medium.jpg", FTPs.FTPUploadEnum.ForumAvatar);
            //    fTPs.UpLoadFile(path, fi + "small.jpg", FTPs.FTPUploadEnum.ForumAvatar);
            //}
            return true;
        }
    }
}
