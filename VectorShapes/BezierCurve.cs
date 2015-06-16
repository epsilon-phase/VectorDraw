using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorShapes
{
    public class BezierCurve :IDrawable
    {
        public float DeltaDiff = 0.1f;
        public BezierCurve(List<PointF> ctrl)
        {
            ControlPoints=new List<PointF>(ctrl);
        }

        public BezierCurve()
        {
            ControlPoints=new List<PointF>();
            Crossovers=new List<Point>();
        }
        public List<Point> Crossovers { get; set; }  
        public List<PointF> ControlPoints { get; set; }
        public bool LogCrossover=false;
        public Color OutlineColor { get; set; }
        public void Draw(RgbRasterImage img, float xscale, float yscale)
        {
            //reset each drawing
            if(LogCrossover)
                Crossovers.Clear();
            var t = ProjectPoints();            
            var incr = 1/(Math.Max(xscale, yscale)+3.0f);
            Point z = Point.Empty;
            for (float f = 0.0f; f <= 1; f+=incr)
            {
                //Get the scaled point for use in actually addressing positions in the image
                var q=Utility.ScalePoint(Utility.EvaluateBezier(f, ControlPoints),xscale,yscale);
                //If the evaluation has resulted in a gap of more than one pixel, then reduce the step and restart from where the issue occurred(woo, artificial numeric stability)
                if (z!=Point.Empty
                    &&Utility.LegitDistance(q, z) > 1)
                {
                    
                    f -= incr;
                    //Halve the velocity(as I like to think of it).
                    incr /= 2.0f;
                    //skip to the next iteration
                    continue;
                }
                //Store information about potential self-intersections
                if (LogCrossover&&img[q] == OutlineColor)
                {
                    Crossovers.Add(q);
                }
                //finally get around to setting the pixel
                img[q] = OutlineColor;
                //Store the previous result allowing distance to be calculated and stabilized.
                z = q;
            }
        }

        public void AddPoint(PointF a)
        {
            ControlPoints.Add(a);
        }

        public void AddPoint(float x, float y)
        {
            ControlPoints.Add(new PointF(x,y));
        }
        public List<PointF> ProjectPoints(float totalDifferenceAllowable)
        {
            var p = new List<PointF>();
            for (var q = 0.0f; q <= 1; q += DeltaDiff)
            {
                if(p.Count==0)
                    p.Add(Utility.EvaluateBezier(q, ControlPoints));
                else
                {
                    var c = Utility.EvaluateBezier(q, ControlPoints);
                    var l = p.Last();
                    var distance = Math.Abs(c.X - l.X) + Math.Abs(c.Y - l.Y);
                    if (distance > totalDifferenceAllowable)
                    {       
                        p.Add(Utility.EvaluateBezier(q - DeltaDiff/2f, ControlPoints));
                    }
                    p.Add(c);
                }
            }
            return p;
        }
        public List<PointF> ProjectPoints()
        {
            var p=new List<PointF>();
            for (var q = 0.0f; q <= 1; q += DeltaDiff)
            {
                p.Add(Utility.EvaluateBezier(q,ControlPoints));
            }
            return p;
        }
    }
}
