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
using Android.Content.PM;
using OML_App.Setting;
using Android.Graphics;

namespace OML_App
{
    [Activity(Label = "My Activity", ScreenOrientation = ScreenOrientation.Landscape, Icon = "@drawable/icon")]
    class Viewer : Activity
    {
        //Buttons
        Button spectate;

        //TextView
        TextView textonoff;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Set our content view
            SetContentView(Resource.Layout.Viewer);

            spectate = FindViewById<Button>(Resource.Id.spectateButton);
            spectate.Click += new EventHandler(spectateClick);

            textonoff = FindViewById<TextView>(Resource.Id.onofflinetxt);
        }//end overrided method OnCreate

        protected override void OnResume()
        {
            base.OnResume();
            //reset the background on resume
            spectate.SetBackgroundResource(Resource.Drawable.spectatebutton);
            //Update Status

        }//end overrided method OnResume

        private void UpdateStatus()
        {
            if (Settings_Singleton.Instance.TCP_View_State)
            {
                //graphview
                RunOnUiThread(() =>textonoff.Text = "Online");
                RunOnUiThread(() =>textonoff.SetTextColor(Color.Green));

                RunOnUiThread(() =>spectate.Visibility = ViewStates.Visible);
            }
            else
            {
                //graphview
                RunOnUiThread(() =>textonoff.Text = "Offline");
                RunOnUiThread(() =>textonoff.SetTextColor(Color.Red));

                RunOnUiThread(() =>spectate.Visibility = ViewStates.Invisible);
            }
        }

        /// <summary>
        /// eventhandler for the spectate button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void spectateClick(object sender, EventArgs e)
        {
            spectate.SetBackgroundResource(Resource.Drawable.spectatebutton_pressed);
            loadSpectate();
        }//end method spectateClick

        /// <summary>
        /// method to load the viewer's view
        /// </summary>
        private void loadSpectate()
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Spectate));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }//end method loadViewer
    }//end class Viewer
}//end namespace OML_App