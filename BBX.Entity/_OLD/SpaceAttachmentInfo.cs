using System;

namespace Discuz.Entity
{
    public class SpaceAttachmentInfo
    {
        
        private int _aid;
        public int AID { get { return _aid; } set { _aid = value; } }

        private int _uid;
        public int UID { get { return _uid; } set { _uid = value; } }

        private int _spacePostID;
        public int SpacePostID { get { return _spacePostID; } set { _spacePostID = value; } }

        private DateTime _postDateTime;
        public DateTime PostDateTime { get { return _postDateTime; } set { _postDateTime = value; } }

        private string _fileName;
        public string FileName { get { return _fileName; } set { _fileName = value; } }

        private string _fileType;
        public string FileType { get { return _fileType; } set { _fileType = value; } }

        private int _fileSize;
        public int FileSize { get { return _fileSize; } set { _fileSize = value; } }

        private string _attachment;
        public string Attachment { get { return _attachment; } set { _attachment = value; } }

        private int _downloads;
        public int Downloads { get { return _downloads; } set { _downloads = value; } }
    }
}