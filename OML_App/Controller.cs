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
    public class Controller : Activity
    {
        private ViewFlipper flipper;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Controller);

            flipper = FindViewById<ViewFlipper>(Resource.Id.flipper);


            //Setup Connect Button
            Button btnOpenNewActivity1 = FindViewById<Button>(Resource.Id.flipButton);
            btnOpenNewActivity1.Click += new EventHandler(FlipButton);
            
        }

        public void FlipButton(object sender, EventArgs e)
        {
            flipper.ShowNext();
        }
    }
}