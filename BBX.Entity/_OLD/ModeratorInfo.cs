using System;

namespace BBX.Entity
{
    [Serializable]
    public class ModeratorInfo
    {
        
        private int m_uid;
        public int Uid { get { return m_uid; } set { m_uid = value; } }

        private int m_fid;
        public int Fid { get { return m_fid; } set { m_fid = value; } }

        private int m_displayorder;
        public int Displayorder { get { return m_displayorder; } set { m_displayorder = value; } }

        private int m_inherited;
        public int Inherited { get { return m_inherited; } set { m_inherited = value; } }
    }
}