namespace Discuz.Entity
{
    public class PhotoAggregationInfo
    {
        
        private int _focusphotoshowtype;
        public int Focusphotoshowtype { get { return _focusphotoshowtype; } set { _focusphotoshowtype = value; } }

        private int _focusphotodays = 7;
        public int Focusphotodays { get { return _focusphotodays; } set { _focusphotodays = value; } }

        private int _focusphotocount = 8;
        public int Focusphotocount { get { return _focusphotocount; } set { _focusphotocount = value; } }

        private int _focusalbumshowtype;
        public int Focusalbumshowtype { get { return _focusalbumshowtype; } set { _focusalbumshowtype = value; } }

        private int _focusalbumdays = 7;
        public int Focusalbumdays { get { return _focusalbumdays; } set { _focusalbumdays = value; } }

        private int _focusalbumcount = 8;
        public int Focusalbumcount { get { return _focusalbumcount; } set { _focusalbumcount = value; } }

        private int _weekhot = 10;
        public int Weekhot { get { return _weekhot; } set { _weekhot = value; } }
    }
}