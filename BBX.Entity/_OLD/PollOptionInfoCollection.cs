using System.Collections;

namespace Discuz.Entity
{
    public class PollOptionInfoCollection : CollectionBase
    {
        public class PollOptionInfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public PollOption Current { get { return (PollOption)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public PollOptionInfoCollectionEnumerator(PollOptionInfoCollection mappings)
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

        public PollOption this[int index] { get { return (PollOption)base.List[index]; } } 

        public object SyncRoot { get { return this; } } 

        public PollOptionInfoCollection()
        {
        }

        public PollOptionInfoCollection(PollOptionInfoCollection value)
        {
            this.AddRange(value);
        }

        public PollOptionInfoCollection(PollOption[] value)
        {
            this.AddRange(value);
        }

        public int Add(PollOption value)
        {
            return base.List.Add(value);
        }

        public void AddRange(PollOption[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(PollOptionInfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((PollOption)value.List[i]);
            }
        }

        public bool Contains(PollOption value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(PollOption[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(PollOption value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, PollOption value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(PollOption value)
        {
            base.List.Remove(value);
        }

        public new PollOptionInfoCollection.PollOptionInfoCollectionEnumerator GetEnumerator()
        {
            return new PollOptionInfoCollection.PollOptionInfoCollectionEnumerator(this);
        }
    }
}