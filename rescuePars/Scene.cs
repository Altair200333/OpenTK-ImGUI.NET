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
            obj.addComponent(new STLMeshLoader().load("mod.stl"));
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

    class VolumeRenderer: RendererExtension
    {
        private MeshRenderer renderer;
        int texname;
        int id;
        private float[] buff;

        private int w = 10;
        private int h = 10;
        private int d = 10;
        public override void init(MeshRenderer renderer)
        {
            var gradient = new Gradient();
            gradient.addStep(0, Colors.Yellow);
            gradient.addStep(0.25f, Colors.OrangeRed);
            gradient.addStep(0.33f, Colors.Brown);
            gradient.addStep(0.5f, Colors.Wheat);
            gradient.addStep(0.75f, Colors.Black);
            gradient.addStep(1.0f, Colors.Blue);
            gradient.init();

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
                        buff[id * 4 + 0] = color.R / 255.0f;// (float)i / w;
                        buff[id * 4 + 1] = color.G / 255.0f;//(float)j / h;
                        buff[id * 4 + 2] = color.B / 255.0f;//(float)k / d;
                        buff[id * 4 + 3] = value * 0.03f;//(float)k / d * 0.1f;
                    }
                }
            }
            this.renderer = renderer;
            Console.WriteLine("init");

            renderer.shader.use();
            GL.GenBuffers(1, out texname);
            GL.BindBuffer(BufferTarget.TextureBuffer, texname);
            GL.BufferData(BufferTarget.TextureBuffer, (IntPtr)(buff.Length * 4), buff, BufferUsageHint.StaticDraw);

            GL.GenTextures(1, out id);
            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            GL.TexBuffer(TextureBufferTarget.TextureBuffer, SizedInternalFormat.Rgba32f, texname);

            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
            GL.BindTexture(TextureTarget.TextureBuffer, 0);
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
            //GL.TexImage3D();
        }
    }
}
