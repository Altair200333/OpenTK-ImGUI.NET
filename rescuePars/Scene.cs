using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using rescuePars.Data;
using rescuePars.ECS;
using rescuePars.Loaders;
using rescuePars.Volume;
using Transform = rescuePars.ECS.Transform;

namespace rescuePars
{
    
    class Scene
    {
        public List<Object> objects;
        public Object camera;

        public Scene()
        {
            objects = new List<Object>();

            Object obj = new Object();
            Object cam = new Object();
            obj = new Object();
            
            obj.addComponent(new Transform(new Vector3(0, 0, -5)));
            obj.addComponent(new STLMeshLoader().load("cube.stl"));
            obj.addComponent(new MeshRenderer());
            obj.addComponent(new Material(new Vector3(0.9f, 0.5f, 0.3f)));

            obj.getComponent<MeshRenderer>().init();
            objects.Add(obj);

            obj = new Object();

           obj.addComponent(new Transform(new Vector3(0, 0, 0)));
           obj.addComponent(new STLMeshLoader().load("cube.stl"));
           obj.addComponent(new Material(new Vector3(0, 0.5f, 0.3f)));
           obj.addComponent(new MeshRenderer());
           obj.getComponent<MeshRenderer>().extension = new VolumeRenderer();
           obj.getComponent<MeshRenderer>().init("Shader/Projection/fragment.fs", "Shader/Projection/vertex.vs");
           objects.Add(obj);
        }
    }

    
}
