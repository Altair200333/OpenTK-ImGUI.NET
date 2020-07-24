using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace rescuePars.ECS
{
    class CameraController : Component
    {
        public Vector3 pivot;
        public Vector3 targetPivot;
        public Vector3 shift;
        public float mouseSensitivity = 0.1f;

        public CameraController()
        {
            shift = new Vector3(0, 0, 0);
        }

        public void pan(Vector3 pan)
        {
            shift += pan;
            targetPivot += pan;
        }
        public void cameraMouseLook(Vector2 offset, bool constrainPitch)
        {
            Camera camera = owner.getComponent<Camera>();
            offset.X *= mouseSensitivity;
            offset.Y *= mouseSensitivity;

            camera.yaw += offset.X;
            camera.pitch -= offset.Y;

            if (constrainPitch)
            {
                if (camera.pitch > 89.0f)
                    camera.pitch = 89.0f;
                if (camera.pitch < -89.0f)
                    camera.pitch = -89.0f;
            }

            camera.updateCameraVectors();
        }

        public void rotateAroundPivot(Vector2 offset)
        {
            Camera camera = owner.getComponent<Camera>();

            var pos = owner.getComponent<Transform>().position;

            float distance = (pos - (pivot + shift)).Length;
            cameraMouseLook(offset, true);

            pos = pivot + shift - camera.front * distance;
            owner.getComponent<Transform>().position = pos;
        }
    }
}