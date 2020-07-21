using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace rescuePars.ECS
{
    class Camera : Component
    {
        const float YAW = -90.0f;
        const float PITCH = 0.0f;
        const float SPEED = 10.3f;
        const float SENSITIVITY = 0.1f;
        const float ZOOM = 60.0f;
        const float ASPECTR = 1.0f;

        public Vector3 Front;
        public Vector3 Up;
        public Vector3 Right;
        public Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

        float yaw = YAW;

        float pitch = PITCH;

        // Camera options
        float movementSpeed = SPEED;
        float mouseSensitivity = SENSITIVITY;
        public float zoom = ZOOM;
        public float aspectRatio;

        public override int getId()
        {
            return 1;
        }


        public Camera()
        {
        }

        public Camera(float aspect)
        {
            aspectRatio = aspect;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            updateCameraVectors();
        }
        public void cameraMouseLook(Vector2 offset, bool constrainPitch)
        {
            offset.X *= mouseSensitivity;
            offset.Y *= mouseSensitivity;

            yaw += offset.X;
            pitch -= offset.Y;

            // Make sure that when pitch is out of bounds, screen doesn't get flipped
            if (constrainPitch)
            {
                if (pitch > 89.0f)
                    pitch = 89.0f;
                if (pitch < -89.0f)
                    pitch = -89.0f;
            }

            // Update Front, Right and Up Vectors using the updated Euler angles
            updateCameraVectors();
        }
        public Matrix4 getViewMatrix()
        {
            Vector3 pos = owner.getComponent<Transform>().position;
            return Matrix4.LookAt(pos, pos + Front, Up);
        }
        public static float Radians(double angle)
        {
            return (float) ((Math.PI / 180) * angle);
        }
        void updateCameraVectors()
        {
            // Calculate the new Front vector
            Vector3 front = new Vector3();
            front.X = (float) (Math.Cos(Radians(yaw)) * Math.Cos(Radians(pitch)));
            front.Y = (float) Math.Sin(Radians(pitch));
            front.Z = (float) (Math.Sin(Radians(yaw)) * Math.Cos(Radians(pitch)));
            Front = Vector3.Normalize(front);
            // Also re-calculate the Right and Up vector
            Right = Vector3.Normalize(Vector3.Cross(Front,
                WorldUp)); // Normalize the vectors, because their length gets closer to 0 the more you look up or down which results in slower movement.
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
    }
}