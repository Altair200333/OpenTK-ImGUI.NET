using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using rescuePars.ECS;

namespace rescuePars
{
    /// <summary>This is Entity, container for components. Some components rely on other and insertion order matters.
    /// So if you create a camera you first need to add Transform and then Camera components.
    /// MeshRenderer wants to see Mesh already attached</summary>
    class Object
    {
        public ComponentManager componentManager;
        public string name = "Object";
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
