using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        int[] int_Line = new int[30];
        int cc = 0;
        int[] int_Line2 = new int[30]; int[] int_Line3 = new int[30];
        int framesRendered = 0, fps = 0;
        DateTime lastTime;
        string[] _st_pos = new string[3];
        // 
        int framesRendered2 = 0, fps2 = 0;
        DateTime lastTime2;
        int framesRendered3 = 0, fps3 = 0;
        DateTime lastTime3;
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

        void Create_Vector(int[] _ar)
        {
            int[] _pos = new int[3];
            for(int i = 0; i < 10; i++)
            {
                _pos[0] += _ar[i];
            }
            for (int i = 10; i < 20; i++)
            {
                _pos[1] += _ar[i];
            }
            for (int i = 20; i < 30; i++)
            {
                _pos[2] += _ar[i];
            }
            _pos[0] = (int)(_pos[0] / 48.33);
            _pos[1] = (int)(_pos[1] / 48.33);
            _pos[2] = (int)(_pos[2] / 48.33);
            for(int j = 0; j < 3; j ++)
            {
                _st_pos[j] = "";
                for (int i = 0; i < 7; i++)
                {
                    if (_pos[j] == i)
                    {
                        _st_pos[j] += "1";
                    }
                    else
                    {
                        _st_pos[j] += "0";
                    }

                    if(i != 6)
                    {
                        _st_pos[j] += " ";
                    }
                    else
                    {
                        _st_pos[j] += "";
                    }
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        ReadFrame(int_Line3);
                        Create_Vector(int_Line3);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lb_l1.Text = _st_pos[0];
                            lb_l2.Text = _st_pos[1];
                            lb_l3.Text = _st_pos[2];
                            Update_FPS3();                       
                        }); 
                    }
                    catch
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lb_fps3.Text = "0";
                        });
                    }

                }
            });
        }


        void Update_FPS()
        {
            framesRendered++;

            if ((DateTime.Now - lastTime).TotalSeconds >= 1)
            {
                // one second has elapsed 

                fps = framesRendered;
                framesRendered = 0;
                lastTime = DateTime.Now;
            }

            // draw FPS on screen here using current value of _fps          
            lb_fps1.Text = fps.ToString() ;
        }
        private async void bt_simulate_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                while(true)
                {
                    try
                    {
                        ReadFrame(int_Line);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Create_Box(gr_Simulate, int_Line);
                            Update_FPS();
                        });
                        Console.WriteLine("Red Pont Done");
                    }
                    catch
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            lb_fps1.Text = "0";
                        });
                    }

                }      
            });
        }
        void ReadFrame(int[] _int_line)
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
                    _int_line[i] = vt;
                }
            }
            //im_gray.Source = ImageSource.FromStream(() => new MemoryStream(arImg));
            Console.WriteLine("Read done!");

        }

        void ShowRedPoint()
        {

        }

        // Test

        void Update_FPS2()
        {
            framesRendered2++;

            if ((DateTime.Now - lastTime2).TotalSeconds >= 1)
            {
                // one second has elapsed 

                fps2 = framesRendered2;
                framesRendered2 = 0;
                lastTime2 = DateTime.Now;
            }

            // draw FPS on screen here using current value of _fps          
            lb_fps2.Text = fps2.ToString();
        }
        void Update_FPS3()
        {
            framesRendered3++;

            if ((DateTime.Now - lastTime3).TotalSeconds >= 1)
            {
                // one second has elapsed 

                fps3 = framesRendered3;
                framesRendered3 = 0;
                lastTime3 = DateTime.Now;
            }

            // draw FPS on screen here using current value of _fps          
            lb_fps3.Text = fps3.ToString();
        }
       
    }
}
