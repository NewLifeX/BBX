using System;

namespace BBX.Entity
{
    public class DebateInfo
    {
        
        private int _tid;
        public int Tid { get { return _tid; } set { _tid = value; } }

        private string _positiveopinion;
        public string Positiveopinion { get { return _positiveopinion; } set { _positiveopinion = value; } }

        private string _negativeopinion;
        public string Negativeopinion { get { return _negativeopinion; } set { _negativeopinion = value; } }

        private DateTime _terminaltime;
        public DateTime Terminaltime { get { return _terminaltime; } set { _terminaltime = value; } }

        private int _positivediggs;
        public int Positivediggs { get { return _positivediggs; } set { _positivediggs = value; } }

        private int _negativediggs;
        public int Negativediggs { get { return _negativediggs; } set { _negativediggs = value; } }

        private int _positivevote;
        public int Positivevote { get { return _positivevote; } set { _positivevote = value; } }

        private string _positivevoterids;
        public string Positivevoterids { get { return _positivevoterids; } set { _positivevoterids = value; } }

        private int _negativevote;
        public int Negativevote { get { return _negativevote; } set { _negativevote = value; } }

        private string _negativevoterids;
        public string Negativevoterids { get { return _negativevoterids; } set { _negativevoterids = value; } }
    }
}