using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.ECS
{
    /// <summary>Base component class, can be instantiated but you dont need it</summary>
    abstract class Component
    {
        /// <summary>The object current component is attached to. Can be null</summary>
        public Object owner = null;

        public abstract int getId();

    }
}
