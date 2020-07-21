using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using  OpenTK.Input;
namespace rescuePars
{
    public delegate void onUpdateCallback();

    class Window : GameWindow
    {
        private Shader.Shader shader;
        public KeyboardState input;
        private onUpdateCallback onUpdate;

        public static Vector4 clearColor = new Vector4(0.5f, 0.3f, 0.3f, 1.0f);
        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }

        public void bindCallback(onUpdateCallback c)
        {
            onUpdate = c;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            input = Keyboard.GetState();
            
            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);

            //Code goes here
            shader = new Shader.Shader("Shader/vertex.vs", "Shader/fragment.fs");

            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (onUpdate != null)
                onUpdate();
                
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteProgram(shader.ID);
            base.OnUnload(e);
        }
    }
}
