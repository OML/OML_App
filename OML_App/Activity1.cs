using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using OML_App.Connection.Viewer;
using System.Threading;
using OML_App.Setting;

namespace OML_App
{
    [Activity(Label = "OML_App", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity
    {
        //buttons
        Button live;
        Button viewer;

        //TCP Viewer
        private Thread viewerThread;
        TCPViewer tcpViewer;

        //bool to check wether were viewing or controlling
        public static bool controller;

        /// <summary>
        /// Android Function OnCreate
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            /**
             * Create start
             */
            live = FindViewById<Button>(Resource.Id.liveButton);
            live.Click += new EventHandler(LiveClick);            

            /**
            * Create info sometimes
            */
            viewer = FindViewById<Button>(Resource.Id.viewerButton);
            viewer.Click += new EventHandler(ViewClick);

            //Start the Connection in a different Thread! (so you can still control all the buttons)
            viewerThread = new Thread(new ThreadStart(ConnectViewer));
            viewerThread.Start(); 
        }
        
        /// <summary>
        /// Android Function OnResume
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            //reset backgrounds on resume
            live.SetBackgroundResource(Resource.Drawable.livebutton);
            viewer.SetBackgroundResource(Resource.Drawable.viewbutton);
        }        

        /// <summary>
        /// Change to Live activity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LiveClick(object sender, EventArgs e)
        {
            //set background to pressed
            live.SetBackgroundResource(Resource.Drawable.livebutton_pressed);
            controller = true;
            Intent i = new Intent();
            i.SetClass(this, typeof(Live));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

        /// <summary>
        /// Change to View(er) activity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ViewClick(object sender, EventArgs e)
        {
            //set background to pressed
            viewer.SetBackgroundResource(Resource.Drawable.viewbutton_pressed);
            controller = false;
            Intent i = new Intent();
            i.SetClass(this, typeof(Viewer));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

        /// <summary>
        /// Connect To the TCP Viewer Server
        /// </summary>
        private void ConnectViewer()
        {
            //Get new TCP Connection
            tcpViewer = new TCPViewer();
            //Set in Sinngleton
            Settings_Singleton.Instance.TCP_Viewer = tcpViewer;
        }
    }
}

