using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.ECS
{
    class ComponentManager
    {
        private Dictionary<int, Component> components;

        public ComponentManager()
        {
            components = new Dictionary<int, Component>();
        }

        public void addComponent(Component comp)
        {
            components[comp.getId()] = comp;
        }

        public T getComponent<T>() where T : Component, new()
        {
            Component comp = new T();

            return components.TryGetValue(comp.getId(), out var value) ? (T) value : null;
        }
        public void removeComponent<T>() where T : Component, new()
        {
            Component comp = new T();
            components.Remove(comp.getId());
        }
    }
}