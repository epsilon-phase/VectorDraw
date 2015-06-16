using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VectorShapes
{
    struct FloatColor
    {
        public float A, R, G, B;

        public FloatColor(float a, float r, float g, float b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public static FloatColor FromColor(Color a)
        {
            var A = a.A/255.0f;
            var R = a.R/255.0f;
            var G = a.G/255.0f;
            var B = a.B/255.0f;
            return new FloatColor(A,R,G,B);
        }
        /// <summary>
        /// Computes the color for alpha composition.   
        /// </summary>
        /// <param name="over"></param>
        /// <returns></returns>
        public FloatColor AlphaComposite(FloatColor over)
        {
            var n = new FloatColor
            {
                A = over.A + A*(1.0f - over.A),
                R = over.R*over.A+R*(1.0f-over.A),
                B=over.B*over.A+B*(1.0f-over.A),
                G=over.G*over.A+G*(1.0f-over.A)
            };
            return n;
        }

        public Color ToColor()
        {
            return Color.FromArgb((int) A*255, (int) R*255, (int) G*255, (int) B*255);
        }

    }
    public class RgbRasterImage
    {
        public Color[,] Pixels { get; private set; }

        
        public RgbRasterImage(int width, int height)
        {
            Pixels=new Color[width,height];
        }
        /// <summary>
        /// Merge another image into this one.
        /// </summary>
        /// <param name="img">The image to merge into this one</param>
        public void MergeImages(RgbRasterImage img)
        {
         
            for (var i = 0; i < Pixels.GetLength(0); i++)
            {
                for (var j = 0; j < Pixels.GetLength(1); j++)
                {
                    var a = FloatColor.FromColor(this[i, j]);
                    var b = FloatColor.FromColor(img[i, j]);
                    Pixels[i, j] = a.AlphaComposite(b).ToColor();
                }
            }
        }
        public Color this[int x, int y]
        {
            get { return Pixels[x, y]; }
            set { Pixels[x, y] = value; }
        }

        public Color this[Point a]
        {
            get { return Pixels[a.X, a.Y]; }
            set { Pixels[a.X, a.Y] = value; }
        }
        public Color this[Tuple<int, int> f]
        {
            get { return Pixels[f.Item1, f.Item2]; }
            set { Pixels[f.Item1, f.Item2] = value; }
        }

        public int Width
        {
            get { return Pixels.GetLength(0); }
        }

        public int Height
        {
            get
            {
                return Pixels.GetLength(1);
            }
        }

        public bool OutOfBounds(int x, int y)
        {
            return (x < 0 || x > Pixels.GetLength(0)) && (y < 0 || y > Pixels.GetLength(1));
        }
        public void SaveAsPPM(string filename)
        {
            
            var q=new StringBuilder(30+5*Pixels.Length);
            q.Append(string.Format("P3\n{0} {1}\n {2}\n",Width,Height,255 ));
                for (var i = 0; i < Pixels.GetLength(0); i++)
                {
                    for (var j = 0; j < Pixels.GetLength(1); j++)
                    {
                        var v = Pixels[i, j];
                        q.Append(string.Format("{0} {1} {2} ", v.R, v.G, v.B));
                    }
                    q.Append("\n");
                }
                File.WriteAllText(filename,q.ToString());
        }

        
        public void SaveAsAnotherFormat(string filename)
        {
            if (!filename.EndsWith(".bmp"))
                filename += ".bmp";
            Bitmap q=new Bitmap(Width,Height);
            for(int x=0;x<Width;x++)
                for (int y = 0; y < Height; y++)
                    q.SetPixel(x, y, this[x, y]);
            Stream z=new FileStream(filename,FileMode.OpenOrCreate);
            q.Save(z,ImageFormat.Bmp);
            z.Close();
        }
    }
}
