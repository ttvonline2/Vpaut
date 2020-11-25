using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Image = Xamarin.Forms.Image;

namespace Vpaut
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        int int_count = 0;
        string st_filepath = "";
        int int_width = 20;
        int int_height = 20;
        int[,] Ar_Pixel = new int[20, 20];

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
           var arImg =   DependencyService.Get<IScreenshotService>().Capture();
            im_gray.Source = ImageSource.FromStream(() => new MemoryStream(arImg));
        }

    }
}
