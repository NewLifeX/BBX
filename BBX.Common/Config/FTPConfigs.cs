using System;
using System.Collections.Generic;

namespace BBX.Config
{
    public class FTPConfigs
    {
        public static List<FTPConfigInfo> Infos { get { return FTPConfigInfos.Current.Infos; } }

        public static FTPConfigInfo GetForumAttachInfo { get { return FTPConfigInfos.Current["ForumAttach"]; } }

        public static FTPConfigInfo GetSpaceAttachInfo { get { return FTPConfigInfos.Current["SpaceAttach"]; } }

        public static FTPConfigInfo GetAlbumAttachInfo { get { return FTPConfigInfos.Current["AlbumAttach"]; } }

        public static FTPConfigInfo GetMallAttachInfo { get { return FTPConfigInfos.Current["MallAttach"]; } }

        public static FTPConfigInfo GetForumAvatarInfo { get { return FTPConfigInfos.Current["ForumAvatar"]; } }

        public static void Add(FTPConfigInfo info)
        {
            if (info == null) return;

            var list = FTPConfigInfos.Current.Infos;
            list.RemoveAll(e => e.Name.EqualIgnoreCase(info.Name));
            list.Add(info);
        }

        public static void Save()
        {
            FTPConfigInfos.Current.Save();
        }
    }
}