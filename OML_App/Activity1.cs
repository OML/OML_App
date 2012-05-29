using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace OML_App
{
    [Activity(Label = "OML_App", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity
    {
        //buttons
        Button live;
        Button recorded;
        Button settings;

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
            recorded = FindViewById<Button>(Resource.Id.recordedButton);
            recorded.Click += new EventHandler(RecordedClick);

            /**
             * Create settings
             */
            settings = FindViewById<Button>(Resource.Id.settingsButton);
            settings.Click += new EventHandler(SettingsClick);
        }

        protected override void OnResume()
        {
            base.OnResume();

            //reset backgrounds on resume
            live.SetBackgroundResource(Resource.Drawable.livebutton);
            recorded.SetBackgroundResource(Resource.Drawable.recordedbutton);
            settings.SetBackgroundResource(Resource.Drawable.settingsbutton);
        }

        void LiveClick(object sender, EventArgs e)
        {
            //set background to pressed
            live.SetBackgroundResource(Resource.Drawable.livebutton_pressed);
            Intent i = new Intent();
            i.SetClass(this, typeof(Live));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

        void RecordedClick(object sender, EventArgs e)
        {
            //set background to pressed
            recorded.SetBackgroundResource(Resource.Drawable.recordedbutton_pressed);
            Intent i = new Intent();
            i.SetClass(this, typeof(Recorded));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

        void SettingsClick(object sender, EventArgs e)
        {
            //set background to pressed
            settings.SetBackgroundResource(Resource.Drawable.settingsbutton_pressed);
            Intent i = new Intent();
            i.SetClass(this, typeof(Settings));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }
    }
}

