using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using rescuePars.Data;
using rescuePars.ECS;

namespace rescuePars.Volume
{
    class VolumeRenderer : RendererExtension
    {
        Guid guid = new Guid();

        private MeshRenderer renderer;
        int texname;
        int id;
        private float[] buff;

        private int w = 10;
        private int h = 10;
        private int d = 10;

        private float low = 0;
        private float high = 1;
        private float density = 0.5f;
        public override void init(MeshRenderer renderer)
        {
            var gradient = createGradient();

            readData(gradient);
            //createData();

            this.renderer = renderer;
            Console.WriteLine("init");

            bindAndUpload(renderer);
        }

        private void bindAndUpload(MeshRenderer renderer)
        {
            renderer.shader.use();
            GL.GenBuffers(1, out texname);
            GL.BindBuffer(BufferTarget.TextureBuffer, texname);
            GL.BufferData(BufferTarget.TextureBuffer, (IntPtr) (buff.Length * 4), buff, BufferUsageHint.StaticDraw);

            GL.GenTextures(1, out id);
            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, texname);

            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
            GL.BindTexture(TextureTarget.TextureBuffer, 0);
        }

        void createData()
        {
            w = 5;
            h = 5;
            d = 5;
            buff = new float[w * h * d * 4];
            for (int k = 0; k < d; k++)
            {
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        int id = i + j * w + k * w * h;
                     
                        buff[id * 4 + 0] = 0;//float)i / w;
                        buff[id * 4 + 1] = 0;//(float)j / h;
                        buff[id * 4 + 2] = (float)k / d;
                        buff[id * 4 + 3] = 1;//(float)k / d;
                    }
                }
            }
        }
        private void readData(Gradient gradient)
        {
            var data = SegyBinReader.read("out_2.bin");
            w = data.w;
            h = data.h;
            d = data.depth;

            buff = new float[w * h * d * 4];
            for (int k = 0; k < d; k++)
            {
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        int id = i + j * w + k * w * h;
                        var value = data.getNormValue(i, j, k);
                        var color = gradient.evaluate(value);
                        buff[id * 4 + 0] = color.R / 255.0f; // (float)i / w;
                        buff[id * 4 + 1] = color.G / 255.0f; //(float)j / h;
                        buff[id * 4 + 2] = color.B / 255.0f; //(float)k / d;
                        buff[id * 4 + 3] = value; //(float)k / d * 0.1f;
                    }
                }
            }
        }

        private static Gradient createGradient()
        {
            var gradient = new Gradient();
            gradient.addStep(0, Colors.Yellow);
            gradient.addStep(0.25f, Colors.OrangeRed);
            gradient.addStep(0.33f, Colors.Brown);
            gradient.addStep(0.5f, Colors.Wheat);
            gradient.addStep(0.75f, Colors.Black);
            gradient.addStep(1.0f, Colors.Blue);
            gradient.init();
            return gradient;
        }

        public override void onRender()
        {
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Texture3DExt);

            GL.BindTexture(TextureTarget.Texture3D, texname);

            renderer.shader.setInt("w", w);
            renderer.shader.setInt("h", h);
            renderer.shader.setInt("d", d);

            renderer.shader.setFloat("low", low);
            renderer.shader.setFloat("high", high);
            renderer.shader.setFloat("density", density);
        }

        public override void drawGui()
        {
            float dragSpeed = 0.0003f;
            ImGui.PushID(guid.ToString());
            if (ImGui.TreeNode("Volume"))
            {
                ImGui.DragFloat("low", ref low, dragSpeed, 0, high);
                ImGui.DragFloat("high", ref high, dragSpeed, low, 1);
                ImGui.DragFloat("density", ref density, dragSpeed, 0, 30);

                ImGui.TreePop();
            }
        }
    }
}
