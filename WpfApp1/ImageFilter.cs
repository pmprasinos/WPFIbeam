using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Media;

namespace WpfApp1
{
    public static class ImageFilter
    {
        public static void ReplaceColor(Bitmap bitmap, System.Drawing.Color originalColor, System.Drawing.Color replacementColor)
        {
            for (var y = 0; y < bitmap.Height; y++)
            {
                for (var x = 0; x < bitmap.Width; x++)
                {
                    System.Drawing.Color c = bitmap.GetPixel(x, y);
                    if (c.R + c.B + c.G >= 750)
                    {
                        bitmap.SetPixel(x, y, replacementColor);
                    }
                }
            }
        }

        public static ImageSource SetJoyStick( bool JoyStickActive, bool UpDown, bool LeftRight, bool Twist)
        {

            System.Drawing.Bitmap bm = (System.Drawing.Bitmap)Properties.Resources.EmptyJoyStick;

            //bm.MakeTransparent(System.Drawing.Color.Black);


            MemoryStream mem = new MemoryStream();
           System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
            Graphics g = Graphics.FromImage(bm);
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            if (UpDown)
            {
                g.DrawImage(Properties.Resources.Up, new PointF(123, 1));
                g.DrawImage(Properties.Resources.Down, new PointF(125, 210));
            }
            if(LeftRight)
            {
                g.DrawImage(Properties.Resources.Left, new PointF(10, 110));
                g.DrawImage(Properties.Resources.Right, new PointF(227, 110));
            }
            if (Twist)
            {
                g.DrawImage(Properties.Resources.Twist, new PointF(72, 75));
            }
            if (JoyStickActive) ReplaceColor(bm, System.Drawing.Color.White, System.Drawing.Color.LimeGreen);
            if (!JoyStickActive) ReplaceColor(bm, System.Drawing.Color.Wheat, System.Drawing.Color.DarkGray);
            bm.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
            mem.Position = 0;

            bi.BeginInit();
            bi.StreamSource = mem;
            bi.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            ImageSourceConverter IMSC = new ImageSourceConverter();

            bi.EndInit();

            return (ImageSource)bi;
           
        }
        //    private unsafe void ReplaceColorUnsafe(Bitmap bitmap, byte[] originalColor, byte[] replacementColor)
        //    {
        //        if (originalColor.Length != replacementColor.Length)
        //        {
        //            throw new ArgumentException("Original and Replacement arguments are in different pixel formats.");
        //        }

        //        if (originalColor.SequenceEqual(replacementColor))
        //        {
        //            return;
        //        }

        //        var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size),
        //                                ImageLockMode.ReadWrite,
        //                                   bitmap.PixelFormat);

        //        var bpp = Image.GetPixelFormatSize(data.PixelFormat);

        //        if (originalColor.Length != bpp)
        //        {
        //            throw new ArgumentException("Original and Replacement arguments and the bitmap are in different pixel format.");
        //        }

        //        var start = (byte*)data.Scan0;
        //        var end = start + data.Stride;

        //        for (var px = start; px < end; px += bpp)
        //        {
        //            var match = true;

        //            for (var bit = 0; bit < bpp; bit++)
        //            {
        //                if (px[bit] != originalColor[bit])
        //                {
        //                    match = false;
        //                    break;
        //                }
        //            }

        //            if (!match)
        //            {
        //                continue;
        //            }

        //            for (var bit = 0; bit < bpp; bit++)
        //            {
        //                px[bit] = replacementColor[bit];
        //            }
        //        }

        //        bitmap.UnlockBits(data);
        //    }

        //}
    }
}