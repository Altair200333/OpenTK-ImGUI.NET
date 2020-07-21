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
        public Scene scene;

        public Engine()
        {
            window = new Window(1800, 900, "LearnOpenTK");
            window.bindUpdateCallback(onUpdate);
            window.bindRenderCallback(onRender);
            scene = new Scene();

            Object cam = new Object();
            cam = new Object();
            cam.addComponent(new Transform(new Vector3(0, 0, 1)));
            cam.addComponent(new Camera((float)window.Width / window.Height, new Vector3(0.0f, 0.0f, -1.0f)));
            scene.camera = cam;
        }

        public void run()
        {
            window.Run(60.0);
        }
        /// <summary>onUpdate is called every frame. User input and game logic should be handled here. Drawing content here will likely do nothing </summary>
        void onUpdate()
        {
            var camera = scene.camera.getComponent<Camera>();
            if (Input.keyDown(Key.A))
                camera.owner.getComponent<Transform>().position -= camera.right * 0.1f;
            if (Input.keyDown(Key.D))
                camera.owner.getComponent<Transform>().position += camera.right * 0.1f;
            if (Input.keyDown(Key.W))
                camera.owner.getComponent<Transform>().position += camera.front * 0.1f;
            if (Input.keyDown(Key.S))
                camera.owner.getComponent<Transform>().position -= camera.front * 0.1f;
            if (Input.keyDown(Key.Q))
                camera.owner.getComponent<Transform>().position -= camera.up * 0.1f;
            if (Input.keyDown(Key.E))
                camera.owner.getComponent<Transform>().position += camera.up * 0.1f;

            camera.cameraMouseLook(Input.mouseDeltaPos(), true);
            if (Input.keyDown(Key.Escape))
            {
                window.close();
            }

            //System.Console.WriteLine("update has been called");
        }
        /// <summary>onUpdate is called when window draws new frame</summary>
        void onRender()
        {
            foreach (var obj in scene.objects)
            {
                obj.getComponent<MeshRenderer>().render(scene.camera.getComponent<Camera>());
            }
        }
    }
}