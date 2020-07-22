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
            GraphicsMode mode = new GraphicsMode(new ColorFormat(24), 16, 8, 4, new ColorFormat(32), 2, false);

            window = new Window(1800, 900, "LearnOpenTK", mode);
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

            if (Input.keyDown(MouseButton.Middle))
            {
                
                if (Input.keyDown(Key.ShiftLeft))
                {
                    Vector2 mouseShift = Input.mouseDeltaPos();
                    Vector3 shift = -camera.right * mouseShift.X * 0.01f + camera.up * mouseShift.Y * 0.01f;
                    camera.owner.getComponent<Transform>().position += shift;
                    camera.Pivot += shift;
                }
                else
                {
                    camera.rotateAroundPivot(Input.mouseDeltaPos());
                }

            }
            camera.owner.getComponent<Transform>().position += Input.mouseWheel() * camera.front;

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