using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Windows.Media.Color;

namespace rescuePars.Data
{
    struct GradientStep
    {
        public Color color;
        public float time;
    }

    class Gradient
    {
        public List<GradientStep> colors;

        public Gradient()
        {
            colors = new List<GradientStep>();
        }

        public void addStep(float value, Color color)
        {
            colors.Add(new GradientStep()
            {
                time = value,
                color = color
            });
        }

        public void init()
        {
            colors.Sort((x, y) => x.time.CompareTo(y.time));
        }

        Color lerp(in Color c1, in Color c2, float t)
        {
            Color c = new Color();
            c.R = (byte)(c1.R * (1 - t) + c2.R * t);
            c.G = (byte)(c1.G * (1 - t) + c2.G * t);
            c.B = (byte)(c1.B * (1 - t) + c2.B * t);
            c.A = (byte)(c1.A * (1 - t) + c2.A * t);
            return c;
        }

        public Color evaluate(float time)
        {
            if (time < 0)
                return colors[0].color;

            for (int i = 0; i < colors.Count - 1; i++)
            {
                if (time > colors[i].time && time <= colors[i + 1].time)
                {
                    return lerp(colors[i].color, colors[i + 1].color,
                        (time - colors[i].time) / (colors[i + 1].time - colors[i].time));
                }
            }

            return colors[colors.Count - 1].color;
        }
    }
}
