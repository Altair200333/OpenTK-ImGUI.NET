using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace rescuePars.ECS
{
    class Transform : Component
    {
        public Vector3 position;
        public override int getId()
        {
            return 2;
        }
        
        public Transform(Vector3 _position)
        {
            position = _position;
        }
        public Transform() :this(new Vector3( 0,0,0)) 
        {}
    }
}
