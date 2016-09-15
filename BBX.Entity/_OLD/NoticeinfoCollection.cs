using System.Collections;

namespace Discuz.Entity
{
    public class NoticeinfoCollection : CollectionBase
    {
        public class NoticeinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public NoticeInfo Current { get { return (NoticeInfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public NoticeinfoCollectionEnumerator(NoticeinfoCollection mappings)
            {
                this._temp = mappings;
                this._enumerator = this._temp.GetEnumerator();
            }

            public bool MoveNext()
            {
                return this._enumerator.MoveNext();
            }

            bool IEnumerator.MoveNext()
            {
                return this._enumerator.MoveNext();
            }

            public void Reset()
            {
                this._enumerator.Reset();
            }

            void IEnumerator.Reset()
            {
                this._enumerator.Reset();
            }
        }

        public NoticeInfo this[int index] { get { return (NoticeInfo)base.List[index]; } } 

        public NoticeinfoCollection()
        {
        }

        public NoticeinfoCollection(NoticeinfoCollection value)
        {
            this.AddRange(value);
        }

        public NoticeinfoCollection(NoticeInfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(NoticeInfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(NoticeInfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(NoticeinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((NoticeInfo)value.List[i]);
            }
        }

        public bool Contains(NoticeInfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(NoticeInfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(NoticeInfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, NoticeInfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(NoticeInfo value)
        {
            base.List.Remove(value);
        }

        public new NoticeinfoCollection.NoticeinfoCollectionEnumerator GetEnumerator()
        {
            return new NoticeinfoCollection.NoticeinfoCollectionEnumerator(this);
        }
    }
}