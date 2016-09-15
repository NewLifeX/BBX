using System;
using System.Collections;
using System.Web.UI;

namespace BBX.Control
{
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public sealed class TabPageCollection : CollectionBase, IList, ICollection, IEnumerable
    {
        private TabControl _tabControl;

        public TabPage this[int index] { get { return (TabPage)base.List[index]; } set { base.List[index] = value; } }

        internal TabPageCollection(TabControl i_tabControl)
        {
            this._tabControl = i_tabControl;
        }

        public void Add(TabPage pTab)
        {
            base.List.Add(pTab);
        }

        public bool Contains(TabPage pTab)
        {
            return base.List.Contains(pTab);
        }

        public void CopyTo(TabPage[] pArray, int pIndex)
        {
            base.List.CopyTo(pArray, pIndex);
        }

        public int IndexOf(TabPage pItem)
        {
            return base.List.IndexOf(pItem);
        }

        public void Insert(int pIndex, TabPage pItem)
        {
            base.List.Insert(pIndex, pItem);
        }

        protected override void OnInsert(int index, object value)
        {
            if (value.GetType() != typeof(TabPage))
            {
                throw new ArgumentException("插入的子项类型必须为 [TabPage]");
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            if (value.GetType() == typeof(TabPage))
            {
                ((TabPage)value).SetTabControl(this._tabControl);
            }
        }

        protected override void OnRemove(int index, object value)
        {
            if (value.GetType() != typeof(TabPage))
            {
                throw new ArgumentException("移除的子项类型必须为 [TabPage]");
            }
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            if (newValue.GetType() != typeof(TabPage))
            {
                throw new ArgumentException("类型必须为 [TabPage]");
            }
        }

        protected override void OnValidate(object value)
        {
            if (value.GetType() != typeof(TabPage))
            {
                throw new ArgumentException("类型必须为 [TabPage]");
            }
        }

        public void Remove(TabPage pTab)
        {
            base.List.Remove(pTab);
        }
    }
}