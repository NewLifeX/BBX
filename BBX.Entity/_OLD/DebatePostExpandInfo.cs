namespace BBX.Entity
{
    public class DebatePostExpandInfo
    {
        
        private int tid;
        public int Tid { get { return tid; } set { tid = value; } }

        private int pid;
        public int Pid { get { return pid; } set { pid = value; } }

        private int opinion;
        public int Opinion { get { return opinion; } set { opinion = value; } }

        private int diggs;
        public int Diggs { get { return diggs; } set { diggs = value; } }
    }
}