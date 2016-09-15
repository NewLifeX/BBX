namespace BBX.Entity
{
    public class UserExtcreditsInfo
    {
        
        private string m_name;
        public string Name { get { return m_name; } set { m_name = value; } }

        private string m_unit;
        public string Unit { get { return m_unit; } set { m_unit = value; } }

        private float m_rate;
        public float Rate { get { return m_rate; } set { m_rate = value; } }

        private float m_init;
        public float Init { get { return m_init; } set { m_init = value; } }

        private float m_topic;
        public float Topic { get { return m_topic; } set { m_topic = value; } }

        private float m_reply;
        public float Reply { get { return m_reply; } set { m_reply = value; } }

        private float m_digest;
        public float Digest { get { return m_digest; } set { m_digest = value; } }

        private float m_upload;
        public float Upload { get { return m_upload; } set { m_upload = value; } }

        private float m_download;
        public float Download { get { return m_download; } set { m_download = value; } }

        private float m_pm;
        public float Pm { get { return m_pm; } set { m_pm = value; } }

        private float m_search;
        public float Search { get { return m_search; } set { m_search = value; } }

        private float m_pay;
        public float Pay { get { return m_pay; } set { m_pay = value; } }

        private float m_vote;
        public float Vote { get { return m_vote; } set { m_vote = value; } }

        public UserExtcreditsInfo()
        {
            this.m_name = "";
            this.m_unit = "";
            this.m_rate = 0f;
            this.m_init = 0f;
            this.m_topic = 0f;
            this.m_reply = 0f;
            this.m_digest = 0f;
            this.m_upload = 0f;
            this.m_download = 0f;
            this.m_pm = 0f;
            this.m_search = 0f;
            this.m_pay = 0f;
            this.m_vote = 0f;
        }
    }
}