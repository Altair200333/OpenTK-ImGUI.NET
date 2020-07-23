using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace rescuePars.Shader
{
    class Shader
    {
        public int ID;

        protected string getShaderCode(string path)
        {
            return File.ReadAllText(path);
        }
        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexCode = getShaderCode(vertexPath);
            string fragmentCode = getShaderCode(fragmentPath);

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexCode);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentCode);

            GL.CompileShader(vertexShader);

            string infoLogVert = GL.GetShaderInfoLog(vertexShader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            GL.CompileShader(fragmentShader);

            string infoLogFrag = GL.GetShaderInfoLog(fragmentShader);
            if (infoLogFrag != System.String.Empty)
                System.Console.WriteLine(infoLogFrag);

            ID = GL.CreateProgram();

            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);

            GL.LinkProgram(ID);
            //Dispose useless data
            GL.DetachShader(ID, vertexShader);
            GL.DetachShader(ID, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        ~Shader()
        {
        }
        //call before setting uniform variables or using shader
        public void use()
        {
            GL.UseProgram(ID);
        }
        
        public void setBool(string name, bool value)
        {
            GL.Uniform1(GL.GetUniformLocation(ID, name), Convert.ToInt32(value));
        }

        public void setInt(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(ID, name), value);
        }

        public void setFloat(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(ID, name), value);
        }
        public void setVec2(string name, Vector2 value)
        {
            GL.Uniform2(GL.GetUniformLocation(ID, name), value);
        }
        public void setVec3(string name, Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(ID, name), value);
        }
        public void setVec4(string name, Vector4 value)
        {
            GL.Uniform4(GL.GetUniformLocation(ID, name), value);
        }
        public void setMat4(string name, Matrix4 value)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(ID, name), false, ref value);
        }

        
    }
}