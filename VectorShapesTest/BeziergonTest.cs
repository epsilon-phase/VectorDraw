using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VectorShapes;
namespace VectorShapesTest
{
    [TestClass]
    public class BeziergonTest
    {
        [TestMethod]
        public void TestCircularity()
        {
            var b_gon=new Beziergon();
            var bc=new BezierCurve();
            bc.AddPoint(0.5f, 0.5f);
            bc.AddPoint(0.5f,1f);
            bc.AddPoint(1,2);
            var bd=new BezierCurve();
            bd.AddPoint(2,2);
            bd.AddPoint(.5f,2);
            b_gon.AddCurve(bc);
            b_gon.AddCurve(bd);
            var c=new BezierCurve();
            c.AddPoint(1.5f,1.5f);
            c.AddPoint(2f,0.5f);
            b_gon.AddCurve(c);
            RgbRasterImage image=new RgbRasterImage(1050,1050);
            b_gon.OutlineColor=Color.White;
            b_gon.Draw(image,500f,500f);
            image.SaveAsAnotherFormat("circularity.bmp");
        }

        [TestMethod]
        public void TestAddMany()
        {
            var r=new Random();
            var bgon=new Beziergon();
            List<PointF> q=new List<PointF>();
            for (int i = 0; i < 13; i++)
            {
                q.Add(new PointF((float)r.NextDouble()*2,(float)r.NextDouble()*2));
            }
            bgon.AddMany(q);
            RgbRasterImage img=new RgbRasterImage(1050,1050);
            bgon.OutlineColor = Color.Red;
            bgon.Draw(img,500f,500f);
            img.SaveAsAnotherFormat("randomSpline.bmp");
        }
    }
}
