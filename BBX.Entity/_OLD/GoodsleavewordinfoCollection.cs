using System.Collections;

namespace Discuz.Entity
{
    public class GoodsleavewordinfoCollection : CollectionBase
    {
        public class GoodsleavewordinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Goodsleavewordinfo Current { get { return (Goodsleavewordinfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public GoodsleavewordinfoCollectionEnumerator(GoodsleavewordinfoCollection mappings)
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

        public Goodsleavewordinfo this[int index] { get { return (Goodsleavewordinfo)base.List[index]; } } 

        public GoodsleavewordinfoCollection()
        {
        }

        public GoodsleavewordinfoCollection(GoodsleavewordinfoCollection value)
        {
            this.AddRange(value);
        }

        public GoodsleavewordinfoCollection(Goodsleavewordinfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Goodsleavewordinfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Goodsleavewordinfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(GoodsleavewordinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Goodsleavewordinfo)value.List[i]);
            }
        }

        public bool Contains(Goodsleavewordinfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Goodsleavewordinfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Goodsleavewordinfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Goodsleavewordinfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Goodsleavewordinfo value)
        {
            base.List.Remove(value);
        }

        public new GoodsleavewordinfoCollection.GoodsleavewordinfoCollectionEnumerator GetEnumerator()
        {
            return new GoodsleavewordinfoCollection.GoodsleavewordinfoCollectionEnumerator(this);
        }
    }
}