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
using OML_App.net.ukct.reintjan1;
using System.Collections;

namespace OML_App
{
    [Activity(Label = "Settings", ScreenOrientation = ScreenOrientation.Landscape, Icon = "@drawable/icon")]
    public class Settings : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "settings" layout resource
            SetContentView(Resource.Layout.Setting);

            TextView Text1 = FindViewById<TextView>(Resource.Id.textView1);

            WebService ws = new WebService();

            // Set security for the web service - may not be required in all environments
            ws.Credentials = System.Net.CredentialCache.DefaultCredentials;            
            
            RunOnUiThread(() => Text1.Text = "bla");
        }
    }   
}