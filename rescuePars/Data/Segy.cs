using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rescuePars.Data
{
    public struct SegyPillar
    {
        public List<float> values;
    }
    public class SegyData
    {
        public float min;
        public float max;
        public int depth;

        public int w;
        public int h;
        public List<SegyPillar> data;

        public SegyData()
        {
            data = new List<SegyPillar>();
        }

        public float getNormValue(int i, int j, int k)
        {
            return (data[i * h + j].values[k] - min) / (max - min);
        }
    }
    public class SegyBinReader
    {
        public static SegyData read(string path)
        {
            SegyData data = new SegyData();
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                int w = reader.ReadInt32();
                int h = reader.ReadInt32();
                float min = float.MaxValue;
                float max = float.MinValue;
                for (int i = 0; i < w * h; i++)
                {
                    int valuesCount = reader.ReadInt32();
                    List<float> values = new List<float>();

                    for (int j = 0; j < valuesCount; j++)
                    {
                        var value = reader.ReadSingle();
                        values.Add(value);
                        min = Math.Min(min, value);
                        max = Math.Max(max, value);
                    }

                    SegyPillar pillar = new SegyPillar()
                    {
                        values = values,
                    };
                    data.data.Add(pillar);
                }

                data.w = w;
                data.h = h;

                data.min = min;
                data.max = max;
                data.depth = data.data[0].values.Count;
            }
            return data;
        }
    }
}
