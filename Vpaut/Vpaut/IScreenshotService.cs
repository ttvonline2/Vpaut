using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Forms;
using Image = Xamarin.Forms.Image;

namespace Vpaut
{
    public interface IScreenshotService
    {
        int[,] Capture();
        int[] offset();
    }
}
