using System.Collections;

namespace Discuz.Entity
{
    public class GoodsattachmentinfoCollection : CollectionBase
    {
        public class GoodsattachmentinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Goodsattachmentinfo Current { get { return (Goodsattachmentinfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public GoodsattachmentinfoCollectionEnumerator(GoodsattachmentinfoCollection mappings)
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

        public Goodsattachmentinfo this[int index] { get { return (Goodsattachmentinfo)base.List[index]; } } 

        public GoodsattachmentinfoCollection()
        {
        }

        public GoodsattachmentinfoCollection(GoodsattachmentinfoCollection value)
        {
            this.AddRange(value);
        }

        public GoodsattachmentinfoCollection(Goodsattachmentinfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Goodsattachmentinfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Goodsattachmentinfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(GoodsattachmentinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Goodsattachmentinfo)value.List[i]);
            }
        }

        public bool Contains(Goodsattachmentinfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Goodsattachmentinfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Goodsattachmentinfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Goodsattachmentinfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Goodsattachmentinfo value)
        {
            base.List.Remove(value);
        }

        public new GoodsattachmentinfoCollection.GoodsattachmentinfoCollectionEnumerator GetEnumerator()
        {
            return new GoodsattachmentinfoCollection.GoodsattachmentinfoCollectionEnumerator(this);
        }
    }
}