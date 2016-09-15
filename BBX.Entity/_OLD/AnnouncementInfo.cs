using System;

namespace Discuz.Entity
{
    public class AnnouncementInfo
    {
        
        private int m_id;
        public int Id { get { return m_id; } set { m_id = value; } }

        private string m_poster;
        public string Poster { get { return m_poster; } set { m_poster = value; } }

        private int m_posterid;
        public int Posterid { get { return m_posterid; } set { m_posterid = value; } }

        private string m_title;
        public string Title { get { return m_title; } set { m_title = value; } }

        private int m_displayorder;
        public int Displayorder { get { return m_displayorder; } set { m_displayorder = value; } }

        private DateTime m_starttime;
        public DateTime Starttime { get { return m_starttime; } set { m_starttime = value; } }

        private DateTime m_endtime;
        public DateTime Endtime { get { return m_endtime; } set { m_endtime = value; } }

        private string m_message;
        public string Message { get { return m_message; } set { m_message = value; } }
    }
}