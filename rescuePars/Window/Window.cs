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
using rescuePars.GUI;


namespace rescuePars
{
    public delegate void onEventCallback();

    public class Window : GameWindow
    {
        ImGuiController _controller;

        private onEventCallback onUpdate;
        private onEventCallback onRender;
        private onEventCallback onDrawGUI;

        public static Vector4 clearColor = new Vector4(0.5f, 0.3f, 0.3f, 1.0f);

        public Window(int width, int height, string title, GraphicsMode gMode) : base(width, height, gMode, title,
            GameWindowFlags.Default,
            DisplayDevice.Default,
            4, 5, GraphicsContextFlags.ForwardCompatible)
        {
        }

        public void bindUpdateCallback(onEventCallback call)
        {
            onUpdate = call;
        }

        public void bindRenderCallback(onEventCallback call)
        {
            onRender = call;
        }
        public void bindDrawGUICallback(onEventCallback call)
        {
            onDrawGUI = call;
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Input.onUpdateFrame();

            onUpdate?.Invoke();
        }

        public void close()
        {
            Exit();
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
            _controller = new ImGuiController(Width, Height);
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            base.OnRenderFrame(e);
            _controller.Update(this, (float)e.Time);

            clear();

            GL.Enable(EnableCap.DepthTest);

            onRender?.Invoke();
            onDrawGUI?.Invoke();
            
            _controller.Render();
            Context.SwapBuffers();
        }

        public void clear()
        {
            GL.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);
        }
        public void clear(Vector3 color)
        {
            GL.ClearColor(color.X,color.Y, color.Z, clearColor.W);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _controller.WindowResized(Width, Height);

            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

        }
    }
}