namespace BBX.Entity
{
    public class ShowbonusPagePostInfo : ShowtopicPagePostInfo
    {
        
        private int m_bonus;
        public int Bonus { get { return m_bonus; } set { m_bonus = value; } }

        private int m_isbest;
        public int Isbest { get { return m_isbest; } set { m_isbest = value; } }

        private int m_bonusextid;
        public int Bonusextid { get { return m_bonusextid; } set { m_bonusextid = value; } }
    }
}