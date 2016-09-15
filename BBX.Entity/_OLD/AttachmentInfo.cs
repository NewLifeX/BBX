namespace BBX.Entity
{
    public class AttachmentInfo
    {
        private string m_filename = string.Empty;
        private string m_filetype = string.Empty;
        
        private int m_aid;
        public int Aid { get { return m_aid; } set { m_aid = value; } }

        private int m_uid;
        public int Uid { get { return m_uid; } set { m_uid = value; } }

        private int m_tid;
        public int Tid { get { return m_tid; } set { m_tid = value; } }

        private int m_pid;
        public int Pid { get { return m_pid; } set { m_pid = value; } }

        private string m_postdatetime = string.Empty;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }

        private int m_readperm;
        public int Readperm { get { return m_readperm; } set { m_readperm = value; } }

        public string Filename { get { return m_filename.Trim(); } set { m_filename = value; } }

        private string m_description = string.Empty;
        public string Description { get { return m_description; } set { m_description = value; } }

        public string Filetype { get { return m_filetype.Trim(); } set { m_filetype = value; } }

        private long m_filesize;
        public long Filesize { get { return m_filesize; } set { m_filesize = value; } }

        private string m_attachment = string.Empty;
        public string Attachment { get { return m_attachment; } set { m_attachment = value; } }

        private int m_downloads;
        public int Downloads { get { return m_downloads; } set { m_downloads = value; } }

        private int m_attachprice;
        public int Attachprice { get { return m_attachprice; } set { m_attachprice = value; } }

        private int m_width;
        public int Width { get { return m_width; } set { m_width = value; } }

        private int m_height;
        public int Height { get { return m_height; } set { m_height = value; } }

        private int m_sys_index;
        public int Sys_index { get { return m_sys_index; } set { m_sys_index = value; } }

        private string m_sys_noupload = string.Empty;
        public string Sys_noupload { get { return m_sys_noupload; } set { m_sys_noupload = value; } }

        private int m_isimage;
        public int Isimage { get { return m_isimage; } set { m_isimage = value; } }
    }
}