using System.Collections;

namespace Discuz.Entity
{
    public class AttachPaymentlogInfoCollection : CollectionBase
    {
        public class AttachPaymentlogInfoCollectionEnumerator : IEnumerator
        {
            private IEnumerator _enumerator;
            private IEnumerable _temp;

            public AttachPaymentlogInfo Current { get { return (AttachPaymentlogInfo)this._enumerator.Current; } } 

            object IEnumerator.Current { get { return _enumerator.Current; } } 

            public AttachPaymentlogInfoCollectionEnumerator(AttachPaymentlogInfoCollection mappings)
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

        public AttachPaymentlogInfo this[int index] { get { return (AttachPaymentlogInfo)base.List[index]; } } 

        public object SyncRoot { get { return this; } } 

        public AttachPaymentlogInfoCollection()
        {
        }

        public AttachPaymentlogInfoCollection(AttachPaymentlogInfoCollection value)
        {
            this.AddRange(value);
        }

        public AttachPaymentlogInfoCollection(AttachPaymentlogInfo[] value)
        {
            this.AddRange(value);
        }

        public int Add(AttachPaymentlogInfo value)
        {
            return base.List.Add(value);
        }

        public void AddRange(AttachPaymentlogInfo[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                this.Add(value[i]);
            }
        }

        public void AddRange(AttachPaymentlogInfoCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.Add((AttachPaymentlogInfo)value.List[i]);
            }
        }

        public bool Contains(AttachPaymentlogInfo value)
        {
            return base.List.Contains(value);
        }

        public void CopyTo(AttachPaymentlogInfo[] array, int index)
        {
            base.List.CopyTo(array, index);
        }

        public int IndexOf(AttachPaymentlogInfo value)
        {
            return base.List.IndexOf(value);
        }

        public void Insert(int index, AttachPaymentlogInfo value)
        {
            base.List.Insert(index, value);
        }

        public void Remove(AttachPaymentlogInfo value)
        {
            base.List.Remove(value);
        }

        public new AttachPaymentlogInfoCollection.AttachPaymentlogInfoCollectionEnumerator GetEnumerator()
        {
            return new AttachPaymentlogInfoCollection.AttachPaymentlogInfoCollectionEnumerator(this);
        }
    }
}