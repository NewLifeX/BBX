using System.Collections;

namespace Discuz.Entity
{
    public class ShoplinkinfoCollection : CollectionBase
    {
        public class ShoplinkinfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public Shoplinkinfo Current { get { return (Shoplinkinfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public ShoplinkinfoCollectionEnumerator(ShoplinkinfoCollection mappings)
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

        public Shoplinkinfo this[int index] { get { return (Shoplinkinfo)base.List[index]; } } 

        public ShoplinkinfoCollection()
        {
        }

        public ShoplinkinfoCollection(ShoplinkinfoCollection value)
        {
            this.AddRange(value);
        }

        public ShoplinkinfoCollection(Shoplinkinfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(Shoplinkinfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(Shoplinkinfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(ShoplinkinfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((Shoplinkinfo)value.List[i]);
            }
        }

        public bool Contains(Shoplinkinfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(Shoplinkinfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(Shoplinkinfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, Shoplinkinfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(Shoplinkinfo value)
        {
            base.List.Remove(value);
        }

        public new ShoplinkinfoCollection.ShoplinkinfoCollectionEnumerator GetEnumerator()
        {
            return new ShoplinkinfoCollection.ShoplinkinfoCollectionEnumerator(this);
        }
    }
}