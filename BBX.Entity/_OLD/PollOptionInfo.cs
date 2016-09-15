namespace Discuz.Entity
{
    public class PollOptionInfo
    {
        private string _voternames;

        private int _polloptionid;
        public int Polloptionid { get { return _polloptionid; } set { _polloptionid = value; } }

        private int _tid;
        public int Tid { get { return _tid; } set { _tid = value; } }

        private int _pollid;
        public int Pollid { get { return _pollid; } set { _pollid = value; } }

        private int _votes;
        public int Votes { get { return _votes; } set { _votes = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }

        private string _polloption;
        public string Polloption { get { return _polloption; } set { _polloption = value; } }

        public string Voternames
        {
            get
            {
                if (this._voternames != null)
                {
                    return this._voternames;
                }
                return "";
            }
            set { _voternames = value; }
        }
    }
}