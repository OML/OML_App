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

namespace OML_App
{
    [Activity(Label = "My Activity")]
    public class Live : Activity
    {
        public EditText ipaddress;
        public EditText port;
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
            string portnr = port.Text;
            if (ip == "1234")
                return true;
            else
                //TODO Make Test!
                return false;

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