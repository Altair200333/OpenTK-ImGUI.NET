using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace rescuePars.ECS
{
    class Material : Component
    {
        public Vector3 color;

        public Material(Vector3 _color)
        {
            color = _color;
        }

        public Material()
        {
        }

    }
}