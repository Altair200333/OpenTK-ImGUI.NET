using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using rescuePars.GUI;

namespace rescuePars.ECS
{
    abstract class RendererExtension: IGuiDrawable
    {
        public abstract void init(MeshRenderer renderer);
        public abstract void onRender();

        public abstract void drawGui();
    }

    /// <summary>Component responsible for drawing mesh component attached to object</summary>
    class MeshRenderer : Component, IGuiDrawable
    {
        public Shader.Shader shader;

        Matrix4 model;
        int VBO, VAO;

        private string fragment = "Shader/fragment.fs";
        private string vertex = "Shader/vertex.vs";
        public RendererExtension extension = null;

        public MeshRenderer()
        {

        }
        
     
        public MeshRenderer init(string fragment = "Shader/fragment.fs", string vertex = "Shader/vertex.vs")
        {
            this.fragment = fragment;
            this.vertex = vertex;

            Mesh mesh = owner.getComponent<Mesh>();
            if (mesh == null)
            {
                //TODO implement logger 
                //Logger::log("Mesh component required");
                return this;
            }
            model = Matrix4.Identity;
            shader = new Shader.Shader(vertex, fragment);
            shader.use();
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * mesh.vertexCount * 6, mesh.vertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(VAO);

            // position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            // normal attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            // note that we update the lamp's position attribute's stride to reflect the updated buffer data
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            extension?.init(this);
            return this;
        }
        public void render(Camera camera)
        {
            if(owner.getComponent<Mesh>() == null)
            {
                return;
            }
            shader.use();
            shader.setVec3("viewPos", camera.owner.getComponent<Transform>().position);

            Material mat = owner.getComponent<Material>();
            // material properties
            shader.setVec3("diffuse", mat?.color ?? new Vector3(1, 0.5f, 0.3f));

            // view/projection transformations
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(Camera.Radians(camera.zoom), camera.aspectRatio, 0.1f, 1000.0f);
            Matrix4 view = camera.getViewMatrix();
            shader.setMat4("projection", projection);
            shader.setMat4("view", view);

            // world transformation
            Matrix4 worldModel = model*Matrix4.CreateTranslation(owner.getComponent<Transform>().position);
            shader.setMat4("model", worldModel);

            extension?.onRender();

            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, owner.getComponent<Mesh>().vertexCount);
        }

        public void drawGui()
        {
            extension?.drawGui();
        }
    }
}