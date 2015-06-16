using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorShapes;

namespace VectorShapesTest
{
    [TestClass]
    public class BezierTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<PointF> f=new List<PointF>();

            BezierCurve q=new BezierCurve();
            q.ControlPoints.Add(new PointF(0,0));
            q.ControlPoints.Add(new PointF(1,1));
            q.ControlPoints.Add(new PointF(2,0));
            q.ControlPoints.Add(new PointF(3,2));
            var z=q.ProjectPoints();
            
            foreach(var i in z)
                Trace.Write(i);
        }
    }
}
