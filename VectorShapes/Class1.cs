using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VectorShapes
{
    public interface IDrawable
    {
        Color OutlineColor { get; set; }
        void Draw(RgbRasterImage img, float xscale, float yscale);
        List<PointF> ProjectPoints();
        
    }
    public interface IVectorShape:IDrawable
    {
        Color FillColor { get; set; }
        void Fill(RgbRasterImage img, float xscale, float yscale); 
        bool Intersects(IVectorShape other);
    }
}
