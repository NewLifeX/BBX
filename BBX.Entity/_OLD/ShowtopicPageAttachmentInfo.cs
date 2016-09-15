namespace BBX.Entity
{
    public class ShowtopicPageAttachmentInfo : AttachmentInfo
    {
        
        private int m_getattachperm;
        public int Getattachperm { get { return m_getattachperm; } set { m_getattachperm = value; } }

        private int m_attachimgpost;
        public int Attachimgpost { get { return m_attachimgpost; } set { m_attachimgpost = value; } }

        private int m_allowread;
        public int Allowread { get { return m_allowread; } set { m_allowread = value; } }

        private string m_preview = string.Empty;
        public string Preview { get { return m_preview; } set { m_preview = value; } }

        private int m_isbought;
        public int Isbought { get { return m_isbought; } set { m_isbought = value; } }

        private int m_inserted;
        public int Inserted { get { return m_inserted; } set { m_inserted = value; } }
    }
}