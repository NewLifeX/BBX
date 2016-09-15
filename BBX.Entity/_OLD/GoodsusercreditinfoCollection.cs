using System.Collections;

namespace Discuz.Entity
{
    public class GoodsusercreditinfoCollection : CollectionBase
    {
        public class GoodsusercreditinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Goodsusercreditinfo Current { get { return (Goodsusercreditinfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public GoodsusercreditinfoCollectionEnumerator(GoodsusercreditinfoCollection mappings)
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

        public Goodsusercreditinfo this[int index] { get { return (Goodsusercreditinfo)base.List[index]; } } 

        public GoodsusercreditinfoCollection()
        {
        }

        public GoodsusercreditinfoCollection(GoodsusercreditinfoCollection value)
        {
            this.AddRange(value);
        }

        public GoodsusercreditinfoCollection(Goodsusercreditinfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Goodsusercreditinfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Goodsusercreditinfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(GoodsusercreditinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Goodsusercreditinfo)value.List[i]);
            }
        }

        public bool Contains(Goodsusercreditinfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Goodsusercreditinfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Goodsusercreditinfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Goodsusercreditinfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Goodsusercreditinfo value)
        {
            base.List.Remove(value);
        }

        public new GoodsusercreditinfoCollection.GoodsusercreditinfoCollectionEnumerator GetEnumerator()
        {
            return new GoodsusercreditinfoCollection.GoodsusercreditinfoCollectionEnumerator(this);
        }
    }
}