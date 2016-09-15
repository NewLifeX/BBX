using System.Collections;

namespace BBX.Entity
{
    public class GoodsinfoCollection : CollectionBase
    {
        public class GoodsinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Goodsinfo Current { get { return (Goodsinfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public GoodsinfoCollectionEnumerator(GoodsinfoCollection mappings)
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

        public Goodsinfo this[int index] { get { return (Goodsinfo)base.List[index]; } } 

        public GoodsinfoCollection()
        {
        }

        public GoodsinfoCollection(GoodsinfoCollection value)
        {
            this.AddRange(value);
        }

        public GoodsinfoCollection(Goodsinfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Goodsinfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Goodsinfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(GoodsinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Goodsinfo)value.List[i]);
            }
        }

        public bool Contains(Goodsinfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Goodsinfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Goodsinfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Goodsinfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Goodsinfo value)
        {
            base.List.Remove(value);
        }

        public new GoodsinfoCollection.GoodsinfoCollectionEnumerator GetEnumerator()
        {
            return new GoodsinfoCollection.GoodsinfoCollectionEnumerator(this);
        }
    }
}