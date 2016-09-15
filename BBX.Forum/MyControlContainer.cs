using System;
using System.ComponentModel;

namespace BBX.Forum
{
    public class MyControlContainer
    {
        private static MyControlContainer instance = null;
        private static object syncRoot = new Object();

        private Container _container = new Container();
        public Container CurrentContainer { get { return _container; } set { _container = value; } }

        private MyControlContainer() { }

        public static MyControlContainer GetContainer()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new MyControlContainer();
                    }
                }
            }
            return instance;
        }

        public bool AddNormalComponent(string componentName, object componentDataObject)
        {
            bool result;
            try
            {
                instance = GetContainer();
                var normalComponent = new NormalComponent(componentName);
                if (instance.CurrentContainer.Components[componentName] != null)
                    instance.RemoveComponentByName(componentName);
                else
                    instance.RemoveComponentByInnerName(normalComponent.InnerName);

                normalComponent.ComponentDataObject = componentDataObject;
                instance.CurrentContainer.Add(normalComponent, normalComponent.InnerName);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public object GetNormalComponentDataObject(string componentName)
        {
            instance = GetContainer();
            var normalComponent = new NormalComponent(componentName);
            if (instance.CurrentContainer.Components[normalComponent.InnerName] == null) return null;

            return ((NormalComponent)instance.CurrentContainer.Components[normalComponent.InnerName]).ComponentDataObject;
        }

        public void RemoveComponentByName(string componentName)
        {
            foreach (IBBXComponent item in GetContainer().CurrentContainer.Components)
            {
                if (item != null && item.ComponentName == componentName)
                {
                    this.CurrentContainer.Remove(item);
                }
            }
        }

        public void RemoveComponentByInnerName(string componentName)
        {
            var normalComponent = new NormalComponent(componentName);
            foreach (IBBXComponent item in GetContainer().CurrentContainer.Components)
            {
                if (item != null && item.InnerName == normalComponent.ComponentName)
                {
                    this.CurrentContainer.Remove(item);
                }
            }
        }
    }
}