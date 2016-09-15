using System;

namespace Discuz.Entity
{
    [Serializable]
    public class NavInfo
    {
        
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }

        private int _jsmenu;
        public int Jsmenu { get { return _jsmenu; } set { _jsmenu = value; } }

        private int _level;
        public int Level { get { return _level; } set { _level = value; } }

        private int _parentid;
        public int Parentid { get { return _parentid; } set { _parentid = value; } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private string _title;
        public string Title { get { return _title; } set { _title = value; } }

        private string _url;
        public string Url { get { return _url; } set { _url = value; } }

        private int _target;
        public int Target { get { return _target; } set { _target = value; } }

        private int _type;
        public int Type { get { return _type; } set { _type = value; } }

        private int _available;
        public int Available { get { return _available; } set { _available = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }
    }
}