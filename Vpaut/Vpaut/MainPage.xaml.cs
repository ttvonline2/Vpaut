using Android.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms.Shapes;
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
        #region Variable
        bool b_Auto = false;
        double X1, X2, Y1, Y2;
        int[] int_Line = new int[30];
        int framesRendered = 0, int_fps = 0;
        DateTime lastTime;
        string[] _st_pos = new string[3];
        string[] st_Command = { "http://192.168.4.1/sp",
            "http://192.168.4.1/left", "http://192.168.4.1/go", "http://192.168.4.1/right",
            "http://192.168.4.1/rl", "http://192.168.4.1/stop", "http://192.168.4.1/rr", 
            "http://192.168.4.1/bl", "http://192.168.4.1/back", "http://192.168.4.1/br"};
        public MainPage()
        {
            InitializeComponent();

        }
        #endregion

        #region page Swap

        private async void bt_page1_Clicked(object sender, EventArgs e) //PAGE MANUAL
        {
            wv_stream.Source = new Uri("http://192.168.4.1/stop");
            wv_Manual.Source = new Uri("http://192.168.4.1/");
            await gr_page2.FadeTo(0, 250);
            gr_page2.IsVisible = false;
            gr_page1.Opacity = 0;
            gr_page1.IsVisible = true;
            await gr_page1.FadeTo(1, 250);

        }

        private async void bt_page2_Clicked(object sender, EventArgs e)//PAGE Auto
        {
            wv_Manual.Source = new Uri("http://192.168.4.1/stop");
            wv_stream.Source = new Uri("http://192.168.4.1/");
            gr_page1.Opacity = 1;
            await gr_page1.FadeTo(0, 250);
            gr_page1.IsVisible = false;
            gr_page2.Opacity = 0;
            gr_page2.IsVisible = true;
            await gr_page2.FadeTo(1, 250);
        }
        #endregion

        void Create_Box(AbsoluteLayout abLayout, int[] _ar)
        {
            abLayout.Children.Clear();
            for (int i = 0; i < 30; i ++)
            {
                abLayout.Children.Add(new BoxView
                {
                    Color = Color.Red,
                }, new Xamarin.Forms.Rectangle(0, 3 * i, _ar[i] * 3, 3));
                abLayout.Children.Add(new BoxView
                {
                    Color = Color.Red,
                }, new Xamarin.Forms.Rectangle(_ar[i] * 3+3, 3*i, (39-_ar[i]) * 3, 3));

            }

        }
        void Create_Line(AbsoluteLayout abLayout, int[] _ar)
        {
            abLayout.Children.Clear();
            for (int i = 0; i < 1; i++)
            {
                abLayout.Children.Add(new BoxView
                {
                    Color = Color.Red,
                    Rotation = 50
                }, new Xamarin.Forms.Rectangle(0, 3 * i, _ar[i] * 3, 3)) ;
                abLayout.Children.Add(new BoxView
                {
                    Color = Color.Red,
                    Rotation = 50
                }, new Xamarin.Forms.Rectangle(_ar[i] * 3 + 3, 3 * i, (39 - _ar[i]) * 3, 3));

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
            _pos[0] = (int)(_pos[0] / 48.33);  // Convert 29 -> 6. average / 10. 48.33  = 10*29/6
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

        void Update_FPS()
        {
            framesRendered++;
            if ((DateTime.Now - lastTime).TotalSeconds >= 1)
            {
                int_fps = framesRendered;
                framesRendered = 0;
                lastTime = DateTime.Now;
            }
            lb_FPS.Text = int_fps.ToString() ;
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

                        });
                    }

                }      
            });
        }

        #region Auto Button
        private void bt_Stop_Clicked(object sender, EventArgs e)
        {
            // Cancel auto mod
            b_Auto = false;
            int_fps = 0;
            lb_FPS.Text = "Stopped";
            //Stop command
            wv_command.Source = new Uri(st_Command[5]);
        }

        private async void bt_Go_Clicked(object sender, EventArgs e)
        {
            b_Auto = true;
            await Task.Run(() =>
            {
                while (b_Auto)
                {
                    try
                    {
                        ReadFrame(int_Line);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DetectLine(int_Line);
                            lb_td.Text = ((int)X1).ToString() + " - " + ((int)X2).ToString() + " - " + ((int)((X1 + X2) / 2)).ToString();
                            Update_FPS();
                            GoGo();
                        });
                    }
                    catch
                    {
                        Console.WriteLine("Drop");
                    }

                }
            });

        }

        private void bt_Back_Clicked(object sender, EventArgs e)
        {
            VCommand(100, 10, 4);
        }
        #endregion

        async void GoGo()
        {
            int _Va1 =(int) (X1 - X2);
            int _N1 = 3,_N2 = 4;

            int _Va2 = (int)((X1 + X2) / 2)-19;
            if(_Va1>_N1)
            {
                if(_Va2>_N2)
                {
                    VCommand(100, 1, 2);
                }
                else if(_Va2<_N2)
                {
                    VCommand(100, 1, 6);
                }
                else
                {
                    VCommand(100, 1, 6);
                }
            }
            else if (_Va1 <-_N1)
            {
                if (_Va2 > _N2)
                {
                    VCommand(100, 1, 4);
                }
                else if (_Va2 < -_N2)
                {
                    VCommand(100, 1, 2);
                }
                else
                {
                    VCommand(100, 1, 4);
                }
            }
            else
            {
                if (_Va2 > _N2)
                {
                    VCommand(100, 1, 4);
                }
                else if (_Va2 < -_N2)
                {
                    VCommand(100, 1, 6);
                }
                else
                {
                    VCommand(100, 1, 2);
                }
            }
            await Task.Delay(100);
            wv_command.Source = new Uri(st_Command[5]);
        }
        void DetectLine(int[] _int_line)
        {
            gr_Simulate.Children.Clear();
            for (int i = 0; i < 15; i ++)
            {
                X1 += _int_line[i];
            }
            X1 = X1 / 15;

            for (int i = 15; i < 30; i++)
            {
                X2 += _int_line[i];
            }
            X2 = X2 / 15;

            Line _ln = new Line();
            _ln.X1 = X1*3;
            _ln.Y1 = 0;
            _ln.X2 = X2*3;
            _ln.Y2 = 90;
            _ln.StrokeThickness = 7;
            _ln.Stroke = Xamarin.Forms.Brush.Red;
            gr_Simulate.Children.Add(_ln);
        }
        //Return the max value of color each a width line; 30 values
        void ReadFrame(int[] _int_line)
        {
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
           // Console.Write("Frame read...!");

        }
        
        void VCommand(int _interuptPeriod, int _step, int _Command)
        {
            for(int i = 0; i < _step; i ++)
            {
                wv_command.Source = new Uri(st_Command[_Command]);
            }
        }
    }
}
/* Tomorrow: 
1. Light adjust -->
2. Get the best value from stream view
3. reject the error value. if value j - value j-1 > 2 ==> error. 
4. making a line --> selected Direction
5. Step by step each command;

*/