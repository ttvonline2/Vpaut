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
        static Activity currentActivityy;
        static View view;
        public void SetActivity(Activity activity)
        {

            currentActivityy = activity;

            view = currentActivityy.Window.DecorView;
        }

        [Obsolete]
        public int[] offset()
        {
            int[] int_offset = { -1,-1 };
            var view = currentActivityy.Window.DecorView;
            view.DrawingCacheEnabled = true;
            Bitmap bitmap = view.GetDrawingCache(true);
            for(int i = 0; i < bitmap.Height; i++)
            {
                for(int j = 0; j < bitmap.Width; j++)
                {
                    var color = bitmap.GetPixel(j, i);
                    int A = (color >> 24) & 0xff; // or color >>> 24
                    int R = (color >> 16) & 0xff;
                    int G = (color >> 8) & 0xff;
                    int B = (color) & 0xff;
                    if (R == 255 & G == 0 & B == 0)
                    {
                        var color2 = bitmap.GetPixel(j+1, i);
                        int A2 = (color2 >> 24) & 0xff; // or color >>> 24
                        int R2 = (color2 >> 16) & 0xff;
                        int G2 = (color2 >> 8) & 0xff;
                        int B2 = (color2) & 0xff;
                        if (R2 == 0 & G2 == 255 & B2 == 0)
                        {
                            int_offset[0] = i;
                            int_offset[1] = j;
                            return int_offset;
                        }
                    }
                }
            }

            return int_offset;
        }

        [Obsolete]
        public byte[] fullscreen()
        {
            var view = currentActivityy.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png,100, stream);
                return stream.ToArray();
            }
        }

        [Obsolete]
        public int[,] Capture()
        {

            int[,] Ar_Pixel = new int[40, 30];

            var rootView = currentActivityy.Window.DecorView.RootView;

            using (var bitmap = Bitmap.CreateBitmap(
                                    rootView.Width,
                                    rootView.Height,
                                    Bitmap.Config.Argb8888))
            {
                var canvas = new Canvas(bitmap);
                rootView.Draw(canvas);
                for (int i = 24; i < 64; i++)
                {
                    for (int j = 24; j < 54; j++)
                    {
                        Ar_Pixel[i - 24, j - 24] = bitmap.GetPixel(i, j);
                        //int A = (color >> 24) & 0xff; // or color >>> 24
                        //int R = (color >> 16) & 0xff;
                        //int G = (color >> 8) & 0xff;
                        //int B = (color) & 0xff;
                    }
                }
                return Ar_Pixel;
            }
            //  view.DrawingCacheEnabled = true;

            // Bitmap bitmap = view.GetDrawingCache(true);


            
        }

    }
}