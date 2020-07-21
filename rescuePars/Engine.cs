using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using rescuePars.ECS;

namespace rescuePars
{
    class Engine
    {
        Window window;
        private Object obj;
        private Object cam;

        public Engine()
        {
            window = new Window(1800, 900, "LearnOpenTK");

            window.bindCallback(onUpdate);
            obj = new Object();
            cam = new Object();
            cam.addComponent(new Transform(new Vector3(0, 0, 1)));
            cam.addComponent(new Camera((float) window.Width / window.Height));

            obj.addComponent(new Transform(new Vector3(0, 0, -2)));
            obj.addComponent(new Mesh(new float[]
            {
                0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
                -0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
                0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
                0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
                -0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f
            }, 6));
            obj.addComponent(new MeshRenderer());
            obj.getComponent<MeshRenderer>().init();

        }

        public void run()
        {
            window.Run(60.0);
        }

        void onUpdate()
        {
            obj.getComponent<MeshRenderer>().render(cam.getComponent<Camera>());
            if(window.input.IsKeyDown(Key.A))
                cam.getComponent<Transform>().position-=Vector3.UnitX*0.1f;
            else if (window.input.IsKeyDown(Key.D))
                cam.getComponent<Transform>().position += Vector3.UnitX * 0.1f;

            //System.Console.WriteLine("update has been called");
        }
    }
}