using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using rescuePars.ECS;
using rescuePars.GUI;

namespace rescuePars
{
    class Engine
    {
        Window window;
        public Scene scene;
        private Object active = null;

        public Engine()
        {
            GraphicsMode mode = new GraphicsMode(new ColorFormat(24), 16, 8, 4, new ColorFormat(32), 2, false);

            window = new Window(1800, 900, "LearnOpenTK", mode);

            window.bindUpdateCallback(onUpdate);
            window.bindRenderCallback(onRender);
            window.bindDrawGUICallback(onDrawGUI);

            scene = new Scene();

            Object cam = new Object();
            cam = new Object();
            cam.addComponent(new Transform(new Vector3(0, 0, 1)));
            cam.addComponent(new CameraController());
            cam.addComponent(new Camera((float) window.Width / window.Height, new Vector3(0.0f, 0.0f, -1.0f)));
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
                    camera.owner.getComponent<CameraController>().pan(shift);
                }
                else
                {
                    camera.owner.getComponent<CameraController>().rotateAroundPivot(Input.mouseDeltaPos());
                }
            }

            camera.owner.getComponent<Transform>().position += Input.mouseWheel() * camera.front;

            if (Input.keyDown(Key.Escape))
            {
                window.close();
            }

            scene.camera.getComponent<CameraController>().shift = Vector3.Lerp(
                scene.camera.getComponent<CameraController>().shift,
                scene.camera.getComponent<CameraController>().targetPivot, 0.1f);
            if (active != null)
            {
                float distance = (scene.camera.getComponent<CameraController>().pivot +
                                  scene.camera.getComponent<CameraController>().shift -
                                  scene.camera.getComponent<Transform>().position).Length;

                scene.camera.getComponent<CameraController>().pivot = Vector3.Lerp(
                    scene.camera.getComponent<CameraController>().pivot,
                    active.getComponent<Transform>().position, 0.1f);

                scene.camera.getComponent<Transform>().position =
                    scene.camera.getComponent<CameraController>().pivot +
                    scene.camera.getComponent<CameraController>().shift - camera.front * distance;
            }
        }

        /// <summary>onUpdate is called when window draws new frame</summary>
        void onRender()
        {
            mousePick();
            foreach (var obj in scene.objects)
            {
                obj.getComponent<MeshRenderer>().render(scene.camera.getComponent<Camera>());
            }
        }

        private void mousePick()
        {
            if (Input.getKeyDown(MouseButton.Left))
            {
                window.clear(new Vector3(0, 0, 0));
                for (int i = 0; i < scene.objects.Count; i++)
                {
                    Vector3 color = scene.objects[i].getComponent<Material>().color;
                    int id = i + 1;
                    scene.objects[i].getComponent<Material>().color = new Vector3((float) id / 255, 0, 0);
                    scene.objects[i].getComponent<MeshRenderer>().shader.use();
                    scene.objects[i].getComponent<MeshRenderer>().shader.setInt("val", 0);

                    scene.objects[i].getComponent<MeshRenderer>().render(scene.camera.getComponent<Camera>());

                    scene.objects[i].getComponent<MeshRenderer>().shader.setInt("val", 1);

                    scene.objects[i].getComponent<Material>().color = color;
                }

                Byte[] Pixel = new Byte[4];
                var cursorPos = Mouse.GetCursorState();

                GL.ReadPixels(cursorPos.X - window.X, window.Height - (cursorPos.Y - window.Y), 1, 1, PixelFormat.Rgba,
                    PixelType.UnsignedByte, Pixel);
                int index = Pixel[0] - 1;

                if (index != -1)
                {
                    Console.WriteLine("selected " + index);
                    scene.camera.getComponent<CameraController>().targetPivot = new Vector3(0);
                    active = scene.objects[index];
                }

                window.clear();
            }
        }

        bool enabled = true;

        void onDrawGUI()
        {
            //ImGui.ShowDemoWindow();

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open", "CTRL+O"))
                    {
                    }

                    ImGui.Separator();
                    if (ImGui.MenuItem("Save", "CTRL+S"))
                    {
                    }

                    if (ImGui.MenuItem("Save As", "CTRL+Shift+S"))
                    {
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Settings"))
                {
                    ImGui.Checkbox("Enabled", ref enabled);

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            if (ImGui.Begin("Objects"))
            {
                if (ImGui.TreeNode("Objects"))
                {
                    foreach (var obj in scene.objects)
                    {
                        ImGui.PushID(obj.name + scene.objects.IndexOf(obj));
                        if (ImGui.TreeNode(obj.name))
                        {
                            ImGui.Text("Position");

                            Vector3 tmp = obj.getComponent<Transform>().position;
                            System.Numerics.Vector3 v = new System.Numerics.Vector3(tmp.X, tmp.Y, tmp.Z);
                            
                            ImGui.DragFloat3("Position", ref v, 0.1f);
                            obj.getComponent<Transform>().position = new Vector3(v.X, v.Y, v.Z);

                            if (obj.getComponent<Material>() != null)
                            {
                                tmp = obj.getComponent<Material>().color;
                                System.Numerics.Vector3 v2 = new System.Numerics.Vector3(tmp.X, tmp.Y, tmp.Z);

                                ImGui.ColorEdit3("lab", ref v2);
                                obj.getComponent<Material>().color = new Vector3(v2.X, v2.Y, v2.Z);
                            }

                            foreach (var component in obj.componentManager)
                            {
                                if (component is IGuiDrawable drawable)
                                {
                                    drawable.drawGui();
                                }
                            }

                            ImGui.TreePop();
                        }

                       
                        ImGui.PopID();
                    }

                    ImGui.TreePop();
                }

                ImGui.End();
            }
        }
    }
}