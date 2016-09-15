using System;
using System.ComponentModel;
namespace BBX.Forum
{
	internal class NormalSite : ISite, IServiceProvider
	{
		private static IComponent _curComponent;
		private static IContainer _curContainer;
		private bool _bDesignMode;
		private string _normalCmpName;
		public virtual IComponent Component { get { return NormalSite._curComponent; } } 
		public virtual IContainer Container { get { return NormalSite._curContainer; } } 
		public virtual bool DesignMode { get { return _bDesignMode; } } 
		public virtual string Name { get { return _normalCmpName; } set { _normalCmpName = value; } }
		public NormalSite(IContainer actvCntr, IComponent prntCmpnt)
		{
			NormalSite._curComponent = prntCmpnt;
			NormalSite._curContainer = actvCntr;
			this._bDesignMode = false;
			this._normalCmpName = null;
		}
		public virtual object GetService(Type service)
		{
			if (service == typeof(ISite))
			{
				return this;
			}
			if (service != typeof(IContainer))
			{
				return null;
			}
			return this;
		}
	}
}