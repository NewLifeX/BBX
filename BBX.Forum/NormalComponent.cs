using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BBX.Forum
{
    public class NormalComponent : Component, IBBXComponent, IComponent, IDisposable
    {
        private ISite _NormalComponent_Site;
        public new event EventHandler Disposed;
        private string _componentName = "";
        public string ComponentName { get { return _componentName; } set { _componentName = value; } }
        private string _innerName = "";
        public string InnerName { get { return _innerName; } set { _innerName = value; } }
        private object _componentDataObject;
        public object ComponentDataObject { get { return _componentDataObject; } set { _componentDataObject = value; } }
        public new virtual ISite Site { get { return _NormalComponent_Site; } set { _NormalComponent_Site = value; } }
        public NormalComponent(string componentName)
        {
            this._NormalComponent_Site = null;
            this._componentName = componentName;
            this._innerName = componentName;
            this.Disposed = null;
        }
        public NormalComponent(string componentName, object componentDataObject) : this(componentName)
        {
            this._componentDataObject = componentDataObject;
        }
        public new virtual void Dispose()
        {
            if (this.Disposed != null)
            {
                this.Disposed(this, EventArgs.Empty);
            }
        }
        public override bool Equals(object cmp)
        {
            NormalComponent normalComponent = (NormalComponent)cmp;
            return this.ComponentName.Equals(normalComponent.ComponentName);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}