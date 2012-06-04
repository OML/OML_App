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

namespace OML_App
{
    [Activity(Label = "My Activity", ScreenOrientation = ScreenOrientation.Landscape, Icon = "@drawable/icon")]
    class Viewer : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Set our content view
            SetContentView(Resource.Layout.Viewer);


        }//end overrided method OnCreate
    }//end class Viewer
}//end namespace OML_App