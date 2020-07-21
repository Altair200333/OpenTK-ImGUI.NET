using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.ECS
{
    class Component
    {
        public Object owner = null;
        public virtual int getId()
        {
            return 0;
        }
       
    }
}
