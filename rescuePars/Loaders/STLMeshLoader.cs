using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using rescuePars.ECS;

namespace rescuePars.Loaders
{
    class STLMeshLoader : MeshLoader
    {
        public override Mesh load(string path)
        {
            List<float> meshList = new List<float>();
            int numOfTriangles = 0;
            int i = 0;
            int byteIndex = 0;
            byte[] fileBytes = File.ReadAllBytes(path);

            byte[] temp = new byte[4];

            /* 80 bytes title + 4 byte num of triangles + 50 bytes (1 of triangular mesh)  */
            if (fileBytes.Length > 120)
            {
                temp[0] = fileBytes[80];
                temp[1] = fileBytes[81];
                temp[2] = fileBytes[82];
                temp[3] = fileBytes[83];

                numOfTriangles = System.BitConverter.ToInt32(temp, 0);

                byteIndex = 84;
                for (i = 0; i < numOfTriangles; i++)
                {
                    Vector3 n = fromBuff(fileBytes, byteIndex);
                    byteIndex += 12;
                    Vector3 v1 = fromBuff(fileBytes, byteIndex);
                    byteIndex += 12;
                    Vector3 v2 = fromBuff(fileBytes, byteIndex);
                    byteIndex += 12;
                    Vector3 v3 = fromBuff(fileBytes, byteIndex);
                    byteIndex += 12;

                    byteIndex += 2;

                    meshList.AddRange(new float[]
                    {
                        v1.X, v1.Y, v1.Z, n.X, n.Y, n.Z,
                        v2.X, v2.Y, v2.Z, n.X, n.Y, n.Z,
                        v3.X, v3.Y, v3.Z, n.X, n.Y, n.Z
                    });
                }
            }

            Mesh mesh = new Mesh(meshList.ToArray(), numOfTriangles * 3);
            return mesh;
        }

        Vector3 fromBuff(byte[] fileBytes, int byteIndex)
        {
            Vector3 v;
            v.X = System.BitConverter.ToSingle(
                new byte[]
                {
                    fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3]
                }, 0);
            byteIndex += 4;
            v.Y = System.BitConverter.ToSingle(
                new byte[]
                {
                    fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3]
                }, 0);
            byteIndex += 4;
            v.Z = System.BitConverter.ToSingle(
                new byte[]
                {
                    fileBytes[byteIndex], fileBytes[byteIndex + 1], fileBytes[byteIndex + 2], fileBytes[byteIndex + 3]
                }, 0);
            byteIndex += 4;
            return v;
        }
    }
}