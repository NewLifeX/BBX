using System;

namespace BBX.Entity
{
    public class BonusLogInfo
    {
        
        private int _tid;
        public int Tid { get { return _tid; } set { _tid = value; } }

        private int _authorid;
        public int Authorid { get { return _authorid; } set { _authorid = value; } }

        private int _answerid;
        public int Answerid { get { return _answerid; } set { _answerid = value; } }

        private string _answername;
        public string Answername { get { return _answername; } set { _answername = value; } }

        private int _pid;
        public int Pid { get { return _pid; } set { _pid = value; } }

        private DateTime _dateline;
        public DateTime Dateline { get { return _dateline; } set { _dateline = value; } }

        private int _bonus;
        public int Bonus { get { return _bonus; } set { _bonus = value; } }

        private byte _extid;
        public byte Extid { get { return _extid; } set { _extid = value; } }

        private int _isbest;
        public int Isbest { get { return _isbest; } set { _isbest = value; } }

        private string _message;
        public string Message { get { return _message; } set { _message = value; } }
    }
}