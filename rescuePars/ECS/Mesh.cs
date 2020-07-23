using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.ECS
{
    class Mesh : Component
    {
        
        public float[] vertices;
        public int vertexCount;

        public Mesh()
        {
        }
        public Mesh(float[] _vertices, int vCount)
        {
            vertices = _vertices;
            vertexCount = vCount;
        }
    }
}