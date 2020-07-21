using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace rescuePars.ECS
{
    class MeshRenderer : Component
    {
        Shader.Shader shader;

        Matrix4 model;
        int VBO, VAO;

        public override int getId()
        {
            return 4;
        }

        public MeshRenderer()
        {
        }
        public MeshRenderer init()
        {
            Mesh mesh = owner.getComponent<Mesh>();
            if (mesh == null)
            {
                //TODO implement logger 
                //Logger::log("Mesh component required");
                return this;
            }
            model = Matrix4.Identity;
            shader = new Shader.Shader("Shader/vertex.vs", "Shader/fragment.fs");

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

            // material properties
            shader.setVec3("diffuse", new Vector3(1, 0.5f, 0.3f));

            // view/projection transformations
            Matrix4 projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(Camera.Radians(camera.zoom), camera.aspectRatio, 0.1f, 1000.0f);
            Matrix4 view = camera.getViewMatrix();
            shader.setMat4("projection", projection);
            shader.setMat4("view", view);

            // world transformation
            Matrix4 worldModel = model*Matrix4.CreateTranslation(owner.getComponent<Transform>().position);
            shader.setMat4("model", worldModel);


            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, owner.getComponent<Mesh>().vertexCount);
        }
	}
}