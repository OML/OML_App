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
using OML_App.Front;
using Java.IO;
using Android.Media;

namespace OML_App
{
    [Activity(Label = "My Activity", ScreenOrientation = ScreenOrientation.Landscape, Icon = "@drawable/icon")]
    class Spectate : Activity
    {
        //control buttons
        Button overview;
        Button battery;
        Button camera;
        Button volt0;
        Button amp0;
        Button temp0;
        Button volt1;
        Button amp1;
        Button temp1;
        Button orient;

        //viewflipper
        private ViewFlipper flipper;

        //graphview layout
        RelativeLayout graphview;

        //ActiveIndex for Batteryview
        public static int activeIndex;

        private VideoView video;
        private MediaController ctlr;
        string path = "http://192.168.1.102:8090/webcam.asf";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Spectate);

            video = FindViewById<VideoView>(Resource.Id.vidview);
            ctlr = new MediaController(this);
            ctlr.SetMediaPlayer(video);
            video.SetMediaController(ctlr);
            video.SetVideoPath(path);

            //set the activeIndex to 0
            activeIndex = 0;

            //midbox viewflipper
            flipper = FindViewById<ViewFlipper>(Resource.Id.spectateflipper);

            //graphview
            graphview = FindViewById<RelativeLayout>(Resource.Id.spec_graphview);

            //set buttons
            overview = FindViewById<Button>(Resource.Id.spec_overviewButton);
            overview.Click += new EventHandler(FlipToOverView);

            battery = FindViewById<Button>(Resource.Id.spec_batteryButton);
            battery.Click += new EventHandler(FlipToBattery);

            camera = FindViewById<Button>(Resource.Id.spec_cameraButton);
            camera.Click += new EventHandler(FlipToCamera);

            orient = FindViewById<Button>(Resource.Id.spec_orientbutton);
            orient.Click += new EventHandler(FlipToPitch);

            volt0 = FindViewById<Button>(Resource.Id.spec_voltbutton0);
            volt0.Click += new EventHandler(FlipToVolt0);

            volt1 = FindViewById<Button>(Resource.Id.spec_voltbutton1);
            volt1.Click += new EventHandler(FlipToVolt1);

            amp0 = FindViewById<Button>(Resource.Id.spec_ampbutton0);
            amp0.Click += new EventHandler(FlipToAmp0);

            amp1 = FindViewById<Button>(Resource.Id.spec_ampbutton1);
            amp1.Click += new EventHandler(FlipToAmp1);

            temp0 = FindViewById<Button>(Resource.Id.spec_tempbutton0);
            temp0.Click += new EventHandler(FlipToTemp0);

            temp1 = FindViewById<Button>(Resource.Id.spec_tempbutton1);
            temp1.Click += new EventHandler(FlipToTemp1);
        }//end overrided method OnCreate

        #region Flippers
        /// <summary>
        /// Flips the Current View to First
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToOverView(object sender, EventArgs e)
        {
            flipper.DisplayedChild = 1;

            //change the backgrounds on click
            overview.SetBackgroundResource(Resource.Drawable.overviewbutton_pressed);
            battery.SetBackgroundResource(Resource.Drawable.batterybutton);
            camera.SetBackgroundResource(Resource.Drawable.camerabutton);
            orient.SetBackgroundResource(Resource.Drawable.orientbutton);
        }//end method FlipToOverView

        /// <summary>
        /// Flips the Current View to Second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToBattery(object sender, EventArgs e)
        {
            flipper.DisplayedChild = 2;

            //change the backgrounds on click
            battery.SetBackgroundResource(Resource.Drawable.batterybutton_pressed);
            overview.SetBackgroundResource(Resource.Drawable.overviewbutton);
            camera.SetBackgroundResource(Resource.Drawable.camerabutton);
            orient.SetBackgroundResource(Resource.Drawable.orientbutton);
        }//end method FlipToBattery

        /// <summary>
        /// Flips the Current View to Third
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToCamera(object sender, EventArgs e)
        {
            flipper.DisplayedChild = 3;

            //change the backgrounds on click
            battery.SetBackgroundResource(Resource.Drawable.batterybutton);
            overview.SetBackgroundResource(Resource.Drawable.overviewbutton);
            camera.SetBackgroundResource(Resource.Drawable.camerabutton_pressed);
            orient.SetBackgroundResource(Resource.Drawable.orientbutton);

            video.RequestFocus();
            video.Start();
        }//end method FlipToCamera

        /// <summary>
        /// Flips the Current View to Fourth
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToPitch(object sender, EventArgs e)
        {
            flipper.DisplayedChild = 4;

            //change the background on click
            orient.SetBackgroundResource(Resource.Drawable.orientbutton_pressed);
            battery.SetBackgroundResource(Resource.Drawable.batterybutton);
            overview.SetBackgroundResource(Resource.Drawable.overviewbutton);
            camera.SetBackgroundResource(Resource.Drawable.camerabutton);
        }//end method FlipToPitch

        #region Sub-Flips Battery
        public void FlipToVolt0(object sender, EventArgs e)
        {
            activeIndex = 1;

            //change background on click
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton_pressed);
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton);
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton);
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToVolt0

        public void FlipToVolt1(object sender, EventArgs e)
        {
            activeIndex = 2;

            //change background on click
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton_pressed);
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton);
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton);
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method

        public void FlipToAmp0(object sender, EventArgs e)
        {
            activeIndex = 3;

            //change background on click
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton_pressed);
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton);
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton);
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToAmp0

        public void FlipToAmp1(object sender, EventArgs e)
        {
            activeIndex = 4;

            //change background on click
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton_pressed);
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton);
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton);
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToAmp1

        public void FlipToTemp0(object sender, EventArgs e)
        {
            activeIndex = 5;

            //change background on click
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton_pressed);
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton);
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton);
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToTemp0

        public void FlipToTemp1(object sender, EventArgs e)
        {
            activeIndex = 6;

            //change background on click
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton_pressed);
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton);
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton);
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToTemp1

        #endregion
        #endregion
    }//end class Spectate
}//end namespace OML_App