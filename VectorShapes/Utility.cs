using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace VectorShapes
{
    public class Utility
    {
        public static int FloodFill(RgbRasterImage imago, int x, int y, Color a)
        {
            int mod_count = 0;
            var q = new Queue<Point>();
            q.Enqueue(new Point(x, y));

            var north = new Size(0, 1);
            var south = new Size(0, -1);
            var east = new Size(-1, 0);
            var west = new Size(1, 0);
            var replacementcolor = imago[x, y];
            while (q.Count > 0)
            {
                var w = q.Dequeue();
                imago[w] = a;

                try
                {
                    var ps = new Point[] { Point.Add(w, north), Point.Add(w, south), Point.Add(w, west), Point.Add(w, east) };
                    foreach (var i in ps)
                    {
                        if (imago[i] == replacementcolor)
                        {
                            imago[i] = a;
                            q.Enqueue(i);

                        }
                    }
                }
                catch (IndexOutOfRangeException f)
                {
                    Console.WriteLine(w); ;
                }

            }
            return mod_count;
        }

        /// <summary>
        /// Linear Interpolation
        /// </summary>
        /// <param name="t">The parametric.</param>
        /// <param name="a">The first value(which is what happens at t = 0)</param>
        /// <param name="b">The second value(which it tends to as t approaches 1)</param>
        /// <returns>The interpolated value</returns>
        public static float lerp(float t, float a, float b)
        {
            return t * b + (1 - t) * a;
        }

        public static PointF lerpP(float t, PointF a, PointF b)
        {
            return new PointF(lerp(t, a.X, b.X), lerp(t, a.Y, b.Y));
        }

        public static PointF EvaluateBezier(float t, List<PointF> ctrl)
        {
            while (true)
            {
                if (ctrl.Count == 1)
                    return ctrl[0];
                var reduced = ctrl.GetRange(1, ctrl.Count - 1);
                for (int i = 0; i < reduced.Count; i++)
                {
                    reduced[i] = lerpP(t, ctrl[i], reduced[i]);
                }
                ctrl = reduced;
            }
        }

        public static PointF GetCentroid(List<PointF> a)
        {
            var sum = new PointF(0, 0);
            foreach (var i in a)
            {
                sum.X += i.X;
                sum.Y += i.Y;

            }
            sum.X /= a.Count;
            sum.Y /= a.Count;
            return sum;

        }

        public static void DrawLine(RgbRasterImage img, Point a, Point b, Color lc)
        {
            DrawLine(img, a.X, a.Y, b.X, b.Y, lc);
        }

        private static void Swap(ref int a, ref int b)
        {
            var r = a;
            a = b;
            b = r;
        }

        public static int Distance(Point a, Point b)
        {
            return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y))/2;
        }

        public static float LegitDistance(Point a, Point b)
        {
            float s = a.X - b.X, z = a.Y - b.Y;
            return (float) Math.Sqrt(s*s + z*z);
        }

        public static float LegitDistance(int x0, int y0, int x1, int y1)
        {
            float s = x0 - x1, z = y0 - y1;
            return (float) Math.Sqrt(s*s + z*z);
        }
        public static Point ScalePoint(PointF a, float xs, float ys)
        {
            return new Point(){X=(int)Math.Round(a.X*xs),Y=(int)Math.Round(a.Y*ys)};
        }
        
        public static void DrawLine(RgbRasterImage img,int x0, int y0, int x1, int y1,Color foreground,bool alternate=false)
        {
            Func<float, Point> line = t =>
            {
                var x = (1.0f - t)*x0 + t*x1;
                var y = (1.0f - t)*y0 + t*y1;
                return new Point((int)Math.Round(x),(int)Math.Round(y));
            };
            bool alternated = false;
            var c = LegitDistance(x0, y0, x1, y1);
            Point old = Point.Empty;
            for (float z = 0.0f; z <= 1.0; z += 1.0f/c)
            {
                var p = line.Invoke(z);
                if (old != Point.Empty && LegitDistance(old, p) > 1)
                    c *= 2;
                if (alternate&&img[p] != Color.Transparent)
                {
                    alternated = !alternated;
                    //skip to the next pixel
                    continue;
                }
                img[p] = alternated?Color.Transparent:foreground;
                old = p;
            }
        }

    }
}
