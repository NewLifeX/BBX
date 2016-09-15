using System.Collections;

namespace Discuz.Entity
{
    public class GoodstradeloginfoCollection : CollectionBase
    {
        public class GoodstradeloginfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Goodstradeloginfo Current { get { return (Goodstradeloginfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public GoodstradeloginfoCollectionEnumerator(GoodstradeloginfoCollection mappings)
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

        public Goodstradeloginfo this[int index] { get { return (Goodstradeloginfo)base.List[index]; } } 

        public GoodstradeloginfoCollection()
        {
        }

        public GoodstradeloginfoCollection(GoodstradeloginfoCollection value)
        {
            this.AddRange(value);
        }

        public GoodstradeloginfoCollection(Goodstradeloginfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Goodstradeloginfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Goodstradeloginfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(GoodstradeloginfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Goodstradeloginfo)value.List[i]);
            }
        }

        public bool Contains(Goodstradeloginfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Goodstradeloginfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Goodstradeloginfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Goodstradeloginfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Goodstradeloginfo value)
        {
            base.List.Remove(value);
        }

        public new GoodstradeloginfoCollection.GoodstradeloginfoCollectionEnumerator GetEnumerator()
        {
            return new GoodstradeloginfoCollection.GoodstradeloginfoCollectionEnumerator(this);
        }
    }
}