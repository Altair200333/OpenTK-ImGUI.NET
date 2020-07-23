using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.ECS
{
    /// <summary>Class responsible for handling components i.e. add, delete, get</summary>
    class ComponentManager
    {
        private Dictionary<Type, Component> components;

        public ComponentManager()
        {
            components = new Dictionary<Type, Component>();
        }

        public void addComponent(Component comp)
        {
            components[comp.GetType()] = comp;
        }

        public T getComponent<T>() where T : Component, new()
        {
            Component comp = new T();

            return components.TryGetValue(typeof(T), out var value) ? (T) value : null;
        }
        public void removeComponent<T>() where T : Component, new()
        {
            Component comp = new T();
            components.Remove(typeof(T));
        }
    }
}