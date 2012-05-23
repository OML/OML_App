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
    [Activity(Label = "OML_App", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape))]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            /**
             * Create start
             */
            Button btnOpenNewActivity1 = FindViewById<Button>(Resource.Id.liveButton);
            btnOpenNewActivity1.Click += new EventHandler(LiveClick);

            /**
            * Create info sometimes
            */
            Button btnOpenNewActivity = FindViewById<Button>(Resource.Id.recordedButton);
            btnOpenNewActivity.Click += new EventHandler(RecordedClick);

            /**
             * Create settings
             */
            Button btnOpenNewActivity2 = FindViewById<Button>(Resource.Id.settingsButton);
            btnOpenNewActivity2.Click += new EventHandler(SettingsClick);
        }

        void LiveClick(object sender, EventArgs e)
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Live));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

        void RecordedClick(object sender, EventArgs e)
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Recorded));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

        void SettingsClick(object sender, EventArgs e)
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Settings));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }
    }
}

