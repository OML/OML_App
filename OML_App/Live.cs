using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OML_App.Connection;
using OML_App.Data;


namespace OML_App
{
    [Activity(Label = "My Activity")]
    public class Live : Activity
    {
        public EditText ipaddress;
        public EditText port;
        public TCPClient connect;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Live);

            //Setup Connect Button
            Button btnOpenNewActivity1 = FindViewById<Button>(Resource.Id.connectbutton);

            btnOpenNewActivity1.Click += new EventHandler(ConnectButton);


            //Find Text Input
            ipaddress = FindViewById<EditText>(Resource.Id.ipaddress);

            port = FindViewById<EditText>(Resource.Id.port);
        }

        private string[] CreateSpinnerList()
        {
            string[] Array = { "Custom", "OML CARMEN" };
            return Array;
        }

        private void ConnectButton(object sender, EventArgs e)
        {
            //TODO!
            //Make Ip Check and more!
            if (CheckIP())
            {
                //Load the controller            
                LoadController();
            }
            else
            {
                //Show Error Widget
                //TODO
            }
        }

        private bool CheckIP()
        {
            string ip = ipaddress.Text;
            //if (port = null)
            //{
            //    int portnr = Convert.ToInt16(port.Text);
            //}
            if (ip == "1234")
                return true;
            else
            {
                //Dirty H@ck
                Receive_Singleton.Instance.init();

                //some hardcoded values

                Send_Singleton.Instance.speed = 1;
                Send_Singleton.Instance.sound = 0;
                Send_Singleton.Instance.right = 50;
                Send_Singleton.Instance.left = 50;
                Send_Singleton.Instance.engine0 = 10;
                Send_Singleton.Instance.engine1 = 10;
                Send_Singleton.Instance.engine2 = 10;
                Send_Singleton.Instance.engine3 = 10;
                Send_Singleton.Instance.Calibration_mode = 0;
                Send_Singleton.Instance.voltage = 1;
                Send_Singleton.Instance.Calibration_mode = 1;
                Send_Singleton.Instance.Gyro = 1;
                Send_Singleton.Instance.temperature = 1;
                Send_Singleton.Instance.throttle = 1;


                connect = new TCPClient(ip, 1337);

                if (connect.connected)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        private void LoadController()
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Controller));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

    }
}