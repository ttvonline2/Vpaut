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
        VisualElement view;
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
            Console.WriteLine("Start");
            //var arImg =   DependencyService.Get<IScreenshotService>().Capture();
            var arImg = DependencyService.Get<IScreenshotService>().Capture();
            for(int i = 0; i < 40; i ++)
            {
                for(int j=0;j<30; j++)
                {
                    int A = (arImg[j, i] >> 24) & 0xff; // or color >>> 24
                    int R = (arImg[j, i] >> 16) & 0xff;
                    int G = (arImg[j, i] >> 8) & 0xff;
                    int B = (arImg[j, i]) & 0xff;
                 }
            }
            Console.WriteLine("Read done!");

        }
       
    }
}
