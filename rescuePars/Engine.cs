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

            window.bindUpdateCallback(onUpdate);
            window.bindRenderCallback(onRender);
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
            var camera = cam.getComponent<Camera>();
            if (Input.keyDown(Key.A))
                cam.getComponent<Transform>().position -= camera.Right * 0.1f;
            if (Input.keyDown(Key.D))
                cam.getComponent<Transform>().position += camera.Right * 0.1f;
            if (Input.keyDown(Key.W))
                cam.getComponent<Transform>().position += camera.Front * 0.1f;
            if (Input.keyDown(Key.S))
                cam.getComponent<Transform>().position -= camera.Front * 0.1f;
            if (Input.keyDown(Key.Q))
                cam.getComponent<Transform>().position -= camera.Up * 0.1f;
            if (Input.keyDown(Key.E))
                cam.getComponent<Transform>().position += camera.Up * 0.1f;

            camera.cameraMouseLook(Input.mouseDeltaPos(), false);
            if (Input.keyDown(Key.Escape))
            {
                window.close();
            }

            //System.Console.WriteLine("update has been called");
        }

        void onRender()
        {
            obj.getComponent<MeshRenderer>().render(cam.getComponent<Camera>());
        }
    }
}