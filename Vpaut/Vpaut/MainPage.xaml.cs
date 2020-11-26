using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Image = Xamarin.Forms.Image;

namespace Vpaut
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        int[,] Ar_Pixel = new int[20, 20];
        int[] int_Line = new int[30];

        public MainPage()
        {
            InitializeComponent();

        }
        void Create_Box(AbsoluteLayout abLayout, int[] _ar)
        {
            abLayout.Children.Clear();
            for (int i = 0; i < 30; i ++)
            {
                abLayout.Children.Add(new BoxView
                {
                    Color = Color.Red,
                }, new Xamarin.Forms.Rectangle(0, 3*i,_ar[i]*3, 3));
                abLayout.Children.Add(new BoxView
                {
                    Color = Color.Red,
                }, new Xamarin.Forms.Rectangle(_ar[i] * 3+3, 3*i, (39-_ar[i]) * 3, 3));

            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ReadFrame();
            Create_Box(gr_Simulate, int_Line);
            Console.WriteLine("Done");
        }

        private async void bt_simulate_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                while(true)
                {
                    ReadFrame();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Create_Box(gr_Simulate, int_Line);
                    });
                    Console.WriteLine("Red Pont Done");
                }      
            });
        }
        void ReadFrame()
        {
            Console.WriteLine("Start");
            var arImg = DependencyService.Get<IScreenshotService>().Capture();

            for (int i = 0; i < 30; i++)
            {
                int max = 0; int vt = 0;
                for (int j = 0; j < 40; j++)
                {
                    // int A = (arImg[j, i] >> 24) & 0xff; // or color >>> 24
                    int R = (arImg[j, i] >> 16) & 0xff;
                    int G = (arImg[j, i] >> 8) & 0xff;
                    int B = (arImg[j, i]) & 0xff;
                    if (max <= (R + G + B))
                    {
                        max = R + G + B;
                        vt = j;
                    }
                    int_Line[i] = vt;
                }
            }


            //im_gray.Source = ImageSource.FromStream(() => new MemoryStream(arImg));
            Console.WriteLine("Read done!");

        }

        void ShowRedPoint()
        {

        }
    }
}
