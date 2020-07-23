using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES30;

namespace rescuePars.ECS
{
    /// <summary>
    /// Contains information about orientation of camera in space. Also handles mouse look input 
    /// </summary>
    class Camera : Component
    {
        const float YAW = -90.0f;
        const float PITCH = 0.0f;

        const float ZOOM = 60.0f;
        const float ASPECTR = 1.0f;

        public Vector3 front;
        public Vector3 up;
        public Vector3 right;
        public Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

        public float yaw = YAW;
        public float pitch = PITCH;

        // Camera options
        public float zoom = ZOOM;
        public float aspectRatio = ASPECTR;


        public Camera()
        {
        }

        public Camera(float aspect, Vector3 front)
        {
            aspectRatio = aspect;
            this.front = front;
            updateCameraVectors();
        }
        
        public Matrix4 getViewMatrix()
        {
            Vector3 pos = owner.getComponent<Transform>().position;
            return Matrix4.LookAt(pos, pos + front, up);
        }
        public static float Radians(double angle)
        {
            return (float) ((Math.PI / 180) * angle);
        }
        public void updateCameraVectors()
        {
            Vector3 front = new Vector3();
            front.X = (float) (Math.Cos(Radians(yaw)) * Math.Cos(Radians(pitch)));
            front.Y = (float) Math.Sin(Radians(pitch));
            front.Z = (float) (Math.Sin(Radians(yaw)) * Math.Cos(Radians(pitch)));
            this.front = Vector3.Normalize(front);

            right = Vector3.Normalize(Vector3.Cross(this.front,
                WorldUp)); 
            up = Vector3.Normalize(Vector3.Cross(right, this.front));
        }
    }
}