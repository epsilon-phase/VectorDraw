using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorShapes
{
    public class Beziergon :IVectorShape
    {
        private List<BezierCurve> curves; 
        public Beziergon(int maxdegree=3)
        {
            curves=new List<BezierCurve>(); 
            DeltaDiff = 0.1f;
        }

        public void AddCurve(BezierCurve curve)
        {
            if (curves.Count > 0)
            {
                curves.Last().ControlPoints[curves.Last().ControlPoints.Count - 1] = curve.ControlPoints.First();
                curve.ControlPoints.Add(curves[0].ControlPoints[0]);

            }
            else
            {
                //else loop it around to the start of itself
                curve.ControlPoints.Add(curve.ControlPoints.First());
            }
            curve.DeltaDiff = DeltaDiff;
            curve.OutlineColor = OutlineColor;
            curves.Add(curve);
        }

        public void AddMany(IEnumerable<PointF> points,int degree=3)
        {
            var p=points.ToArray();
            var c=new BezierCurve();
            for (int i = 0; i < p.Length; i++)
            {
                if (c.ControlPoints.Count%degree == 0)
                {
                    if(c.ControlPoints.Count>0)
                    AddCurve(c);
                    c = new BezierCurve();
                }
                c.AddPoint(p[i]);
            }
        }
        public bool LogCrossover { get; set; }
        public void Draw(RgbRasterImage img, float xscale, float yscale)
        {
            //create a new blank image to allow the crossovers some degree of certainty that it does in fact originate with the shape itself.
            RgbRasterImage g=new RgbRasterImage(img.Width,img.Height);
            foreach (var i in curves)
            {
                i.LogCrossover = LogCrossover;
                i.Draw(g,xscale,yscale);
            }
        }

        private Color _outlinecolor;
        public Color OutlineColor { get { return _outlinecolor; } 
            set
        {
            _outlinecolor = value;
            foreach (var i in curves) i.OutlineColor = value;
        } 
        }
        public List<PointF> ProjectPoints()
        {
            var q=new List<PointF>();
            foreach (var z in curves)
            {
                q.AddRange(z.ProjectPoints());
            }
            return q;
        }

        public Color FillColor { get; set; }
        public void Fill(RgbRasterImage img, float xscale, float yscale)
        {
            var a=new RgbRasterImage(img.Pixels.GetLength(0),img.Pixels.GetLength( 1));
            Draw(a,xscale,yscale);
            var f = Utility.GetCentroid(ProjectPoints());
            var t=new Point((int)(xscale*f.X),(int)(f.Y*yscale));
            Utility.FloodFill(a,t.X,t.Y,FillColor);
            img.MergeImages(a);
        }

        public bool Intersects(IVectorShape other)
        {
            var c = ProjectPoints();
            var d = other.ProjectPoints();
            return (from i in c from j in d where Math.Abs(i.X - j.X) + Math.Abs(i.Y - j.Y) < 0.1 select i).Any();
        }

        private float _delDiff;
        public float DeltaDiff { get { return _delDiff; } set {
            foreach (var c in curves) c.DeltaDiff = value;
            _delDiff = value;
        } }
    }
}
