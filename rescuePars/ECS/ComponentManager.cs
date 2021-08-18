using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.ECS
{
    /// <summary>Class responsible for handling components i.e. add, delete, get</summary>
    class ComponentManager : IEnumerable
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

        public T getComponent<T>() where T : Component
        {
            return components.TryGetValue(typeof(T), out var value) ? (T) value : null;
        }
        public void removeComponent<T>() where T : Component
        {
            components.Remove(typeof(T));
        }

        public IEnumerator<Component> GetEnumerator()
        {
            foreach (var component in components)
            {
                yield return component.Value;
            }
        }
        // The IEnumerable.GetEnumerator method is also required
        // because IEnumerable<T> derives from IEnumerable.
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // Invoke IEnumerator<string> GetEnumerator() above.
            return GetEnumerator();
        }
    }
}