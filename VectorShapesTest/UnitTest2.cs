using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorShapes;
namespace VectorShapesTest
{
    [TestClass]
    public class RgbRasterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var q=new RgbRasterImage(10,10);
            q[0, 2] = Color.White;
            q[1, 2] = Color.White;
            q[2, 0] = Color.White;
            q[2, 1] = Color.White;
            q[2, 2] = Color.White;
            Utility.FloodFill(q,0,0,Color.Aqua);
            var aquacount = 0;
            foreach (var c in q.Pixels)
            {
                if (c == Color.Aqua)
                    aquacount++;
            }
            
            Assert.AreEqual(4,aquacount);
        }

        [TestMethod]
        public void TestSave()
        {
            var q=new RgbRasterImage(100,100);
            var bc1=new BezierCurve();
            bc1.ControlPoints.Add(new PointF(0,0));
            bc1.ControlPoints.Add(new PointF(1,0.5f));
            bc1.ControlPoints.Add(new PointF(2,2));
            var bgon=new Beziergon();
            bgon.AddCurve(bc1);
            
            bgon.DeltaDiff = 0.01f;
            bgon.OutlineColor = Color.Azure;
            bgon.FillColor = Color.Red;
            bgon.Draw(q, 50.0f, 50f);
            q.SaveAsAnotherFormat("draw.bmp");
            bgon.Fill(q,50.0f,50.0f);
            q.SaveAsAnotherFormat("ff.bmp");
            
        }
    }
}
