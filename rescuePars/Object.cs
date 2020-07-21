using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rescuePars.ECS;

namespace rescuePars
{
    class Object
    {
        public ComponentManager componentManager;

        public Object()
        {
            componentManager = new ComponentManager();
        }
        public void addComponent(Component comp)
        {
            comp.owner = this;
            componentManager.addComponent(comp);
        }
        public T getComponent<T>() where T : Component, new()
        {
            return componentManager.getComponent<T>();
        }
    }
}
