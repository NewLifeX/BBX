using System.Collections.Generic;

namespace Discuz.Entity
{
    public class UserPrefCollection<T> : List<T> where T : UserPref, new()
    {
        
        private bool _showRequired;
        public bool ShowRequired { get { return _showRequired; } set { _showRequired = value; } }

        private int _visibleItemCount;
        public int VisibleItemCount { get { return _visibleItemCount; } } 

        public UserPrefCollection()
        {
        }

        public UserPrefCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public UserPrefCollection(int capacity)
            : base(capacity)
        {
        }

        public new int Add(T value)
        {
            if (value.DataType != UserPrefDataType.HiddenType)
            {
                this._visibleItemCount++;
            }
            if (value.Required)
            {
                this._showRequired = true;
            }
            base.Add(value);
            return base.Count;
        }

        public void AddRange(T[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].DataType != UserPrefDataType.HiddenType)
                {
                    this._visibleItemCount++;
                }
                if (value[i].Required)
                {
                    this._showRequired = true;
                }
                base.Add(value[i]);
            }
        }

        public void AddRange(UserPrefCollection<T> value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                T t = value[i];
                if (t.DataType != UserPrefDataType.HiddenType)
                {
                    this._visibleItemCount++;
                }
                T t2 = value[i];
                if (t2.Required)
                {
                    this._showRequired = true;
                }
                base.Add(value[i]);
            }
        }

        public new void Insert(int index, T value)
        {
            if (value.DataType != UserPrefDataType.HiddenType)
            {
                this._visibleItemCount++;
            }
            if (value.Required)
            {
                this._showRequired = true;
            }
            base.Insert(index, value);
        }

        public new void Remove(T value)
        {
            if (value.DataType != UserPrefDataType.HiddenType)
            {
                this._visibleItemCount--;
            }
            base.Remove(value);
            this._showRequired = false;
            foreach (T current in this)
            {
                if (current.Required)
                {
                    this._showRequired = true;
                    break;
                }
            }
        }
    }
}