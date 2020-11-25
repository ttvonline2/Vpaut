using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Vpaut.Droid
{
    public class ScreenshotService : IScreenshotService
    {
        int[] ar_pixels;
        static Activity currentActivityy;
        public void SetActivity(Activity activity)
        {

            currentActivityy = activity;
            if (currentActivityy != null)
            {
                string a = "hello";
            }
            else
            {
                string a = "???";
            }
        }

        public byte[] Capture()
        {

            if (currentActivityy != null)
            {
                string a = "hello";
            }
            else
            {
                string a = "???";
            }
            var view = currentActivityy.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;
            
            using (var stream = new MemoryStream())
            {


                //bitmap.GetPixels(ar_pixels, 0, 1000,0,0, 600, 400);
                Color _cl = new Color();
                _cl.R = 0;
                _cl.G = 0;
                _cl.B = 0;
                for(int i = 0; i < bitmap.Width; i++)
                {
                    for(int j = 0; j < bitmap.Height; j++)
                    {
                        int color =  bitmap.GetPixel(i, j);
                        int A = (color >> 24) & 0xff; // or color >>> 24
                        int R = (color >> 16) & 0xff;
                        int G = (color >> 8) & 0xff;
                        int B = (color) & 0xff;
                    }
                }
                bitmap.Compress(Bitmap.CompressFormat.Png, 5, stream);

                return stream.ToArray();
            }

            

        }

    }
}