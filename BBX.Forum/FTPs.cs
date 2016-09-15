using Discuz.Common;
using Discuz.Config;
using System;
using System.IO;

namespace Discuz.Forum
{
    public class FTPs
    {
        public enum FTPUploadEnum
        {
            ForumAttach = 1,
            SpaceAttach,
            AlbumAttach,
            MallAttach,
            ForumAvatar
        }
        private delegate bool delegateUpLoadFile(string path, string file, FTPUploadEnum ftpuploadname);
        private delegateUpLoadFile upload_aysncallback;
        public static FTPConfigInfo GetForumAttachInfo { get { return FTPConfigs.GetForumAttachInfo; } }
        public static FTPConfigInfo GetSpaceAttachInfo { get { return FTPConfigs.GetSpaceAttachInfo; } }
        public static FTPConfigInfo GetAlbumAttachInfo { get { return FTPConfigs.GetAlbumAttachInfo; } }
        public static FTPConfigInfo GetMallAttachInfo { get { return FTPConfigs.GetMallAttachInfo; } }
        public static FTPConfigInfo GetForumAvatarInfo { get { return FTPConfigs.GetForumAvatarInfo; } }
        public void AsyncUpLoadFile(string path, string file, FTPUploadEnum ftpuploadname)
        {
            this.upload_aysncallback = new delegateUpLoadFile(this.UpLoadFile);
            this.upload_aysncallback.BeginInvoke(path, file, ftpuploadname, null, null);
        }
        public bool UpLoadFile(string path, string file, FTPUploadEnum ftpuploadname)
        {
            var fTP = new FTP();
            path = path.Replace("\\", "/");
            path = (path.StartsWith("/") ? path : ("/" + path));
            FTPConfigInfo config = null;
            bool flag = true;
            switch (ftpuploadname)
            {
                case FTPUploadEnum.ForumAttach:
                    config = GetForumAttachInfo;
                    break;
                case FTPUploadEnum.SpaceAttach:
                    config = GetSpaceAttachInfo;
                    break;
                case FTPUploadEnum.AlbumAttach:
                    config = GetAlbumAttachInfo;
                    break;
                case FTPUploadEnum.MallAttach:
                    config = GetMallAttachInfo;
                    break;
                case FTPUploadEnum.ForumAvatar:
                    config = GetForumAvatarInfo;
                    break;
            }
            fTP = new FTP(config.Serveraddress, config.Serverport, config.Username, config.Password, 1, config.Timeout);
            path = config.Uploadpath + path;
            flag = (config.Reservelocalattach != 1);

            if (!fTP.ChangeDir(path))
            {
                string[] array = path.Split('/');
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    if (text.Trim() != "")
                    {
                        fTP.MakeDir(text);
                        fTP.ChangeDir(text);
                    }
                }
            }
            fTP.Connect();
            if (!fTP.IsConnected)
            {
                return false;
            }
            int num = 0;
            if (!fTP.OpenUpload(file, Path.GetFileName(file)))
            {
                fTP.Disconnect();
                return false;
            }
            while (fTP.DoUpload() > 0L)
            {
                num = (int)(fTP.BytesTotal * 100L / fTP.FileSize);
            }
            fTP.Disconnect();
            if (flag && Utils.FileExists(file))
            {
                File.Delete(file);
            }
            return num >= 100;
        }
        public bool TestConnect(string Serveraddress, int Serverport, string Username, string Password, int Timeout, string uploadpath, ref string message)
        {
            FTP fTP = new FTP(Serveraddress, Serverport, Username, Password, 1, Timeout);
            bool flag = fTP.Connect();
            if (!flag)
            {
                message = fTP.errormessage;
                return flag;
            }
            if (!fTP.ChangeDir(uploadpath))
            {
                fTP.MakeDir(uploadpath);
                if (!fTP.ChangeDir(uploadpath))
                {
                    message += fTP.errormessage;
                    flag = false;
                }
            }
            return flag;
        }
    }
}