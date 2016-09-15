using System.Collections;

namespace Discuz.Entity
{
    public class GoodsrateinfoCollection : CollectionBase
    {
        public class GoodsratesinfoinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Goodsrateinfo Current { get { return (Goodsrateinfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public GoodsratesinfoinfoCollectionEnumerator(GoodsrateinfoCollection mappings)
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

        public Goodsrateinfo this[int index] { get { return (Goodsrateinfo)base.List[index]; } } 

        public GoodsrateinfoCollection()
        {
        }

        public GoodsrateinfoCollection(GoodsrateinfoCollection value)
        {
            this.AddRange(value);
        }

        public GoodsrateinfoCollection(Goodsrateinfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Goodsrateinfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Goodsrateinfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(GoodsrateinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Goodsrateinfo)value.List[i]);
            }
        }

        public bool Contains(Goodsrateinfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Goodsrateinfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Goodsrateinfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Goodsrateinfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Goodsrateinfo value)
        {
            base.List.Remove(value);
        }

        public new GoodsrateinfoCollection.GoodsratesinfoinfoCollectionEnumerator GetEnumerator()
        {
            return new GoodsrateinfoCollection.GoodsratesinfoinfoCollectionEnumerator(this);
        }
    }
}