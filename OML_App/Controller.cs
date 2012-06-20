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
using Android.Graphics.Drawables;
using OML_App.Data;
using OML_App.Setting;
using System.Threading;
using Android.Webkit;

namespace OML_App
{
    [Activity(Label = "My Activity", ScreenOrientation = ScreenOrientation.Landscape, Icon = "@drawable/icon")]
    public class Controller : Activity
    {

        #region Variables
        private ViewFlipper flipper;
        private Thread updateThread;

        //control buttons
        Button overview;
        Button battery;
        Button camera;
        Button orient;
        Button release;
        Button volt0;
        Button amp0;
        Button temp0;
        Button volt1;
        Button amp1;
        Button temp1;

        //dialog for exit message
        Dialog dialog;

        //dialog buttons
        Button okButton;
        Button cancelButton;
        
        //dialog textview
        TextView dialogTxt;

        TextView Eng_Curr;
        TextView Eng_Volt;
        TextView Eng_Temp;

        TextView Accu_Curr;
        TextView Accu_Volt;
        TextView Accu_Temp;

        TextView Throttle0;
        TextView Throttle1;
        TextView Throttle2;
        TextView Throttle3;

        //graphview layout
        RelativeLayout graphview;

        //Bundle to be able to return from Dialog
        Bundle bundle;

        //ActiveIndex for Batteryview
        public static int activeIndex { get; set; }
        //webview and path to our camera feed
        private WebView wView;
        string path = "http://192.168.1.107:8090/webcam.mjpeg";

        #endregion

        #region OnCreate
        /// <summary>
        /// Method thats called when the activity is first created
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.bundle = bundle;

            //set the activeIndex to 0
            activeIndex = 0;

            //set grabber off (also reinitialisation of the grabber)
            Send_Singleton.Instance.releaseRing = 0;

            // Create your application here
            SetContentView(Resource.Layout.Controller);

            //set webview
            wView = FindViewById<WebView>(Resource.Id.vidview);

            //midbox viewflipper
            flipper = FindViewById<ViewFlipper>(Resource.Id.flipper);

            //graphview
            graphview = FindViewById<RelativeLayout>(Resource.Id.graphview);

            //set buttons
            overview = FindViewById<Button>(Resource.Id.overviewButton);
            overview.Click += new EventHandler(FlipToOverView);

            battery = FindViewById<Button>(Resource.Id.batteryButton);
            battery.Click += new EventHandler(FlipToBattery);

            camera = FindViewById<Button>(Resource.Id.cameraButton);
            camera.Click += new EventHandler(FlipToCamera);

            orient = FindViewById<Button>(Resource.Id.orientbutton);
            orient.Click += new EventHandler(FlipToPitch);

            release = FindViewById<Button>(Resource.Id.releasebutton);
            release.Click += new EventHandler(releaseClick);

            volt0 = FindViewById<Button>(Resource.Id.voltbutton0);
            volt0.Click += new EventHandler(FlipToVolt0);

            volt1 = FindViewById<Button>(Resource.Id.voltbutton1);
            volt1.Click += new EventHandler(FlipToVolt1);

            amp0 = FindViewById<Button>(Resource.Id.ampbutton0);
            amp0.Click += new EventHandler(FlipToAmp0);

            amp1 = FindViewById<Button>(Resource.Id.ampbutton1);
            amp1.Click += new EventHandler(FlipToAmp1);

            temp0 = FindViewById<Button>(Resource.Id.tempbutton0);
            temp0.Click += new EventHandler(FlipToTemp0);

            temp1 = FindViewById<Button>(Resource.Id.tempbutton1);
            temp1.Click += new EventHandler(FlipToTemp1);

            //set throttle textviews
            Throttle0 = FindViewById<TextView>(Resource.Id.Throttle0);
            Throttle1 = FindViewById<TextView>(Resource.Id.Throttle1);
            Throttle2 = FindViewById<TextView>(Resource.Id.Throttle2);
            Throttle3 = FindViewById<TextView>(Resource.Id.Throttle3);

            //Start Update Thread
            updateThread = new Thread(new ThreadStart(Update));
            updateThread.Start();
        }//end overrided method OnCreate

        #endregion

        #region OnKeyDown (Back key)
        /// <summary>
        /// overrided bool to show a dialog window 
        /// when trying to exit the controller
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            //if the back key is pressed
            if (keyCode == Keycode.Back)
            {
                updateThread.Abort();
                //set the content view to the dialogwindow content
                //outside this contentview we are unable to find our resources
                SetContentView(Resource.Layout.DialogWindow);

                //create a new dialog
                dialog = new Dialog(this);
                dialog.SetContentView(Resource.Layout.DialogWindow);
                dialog.SetTitle("OML");
                dialog.SetCancelable(true);

                //set the dialog text
                dialogTxt = FindViewById<TextView>(Resource.Id.dialogText);
                dialogTxt.Text = "By leaving the controller interface you will exit the current session. \n\n Do you wish to continue?";

                //set the buttons
                okButton = FindViewById<Button>(Resource.Id.okButton);
                cancelButton = FindViewById<Button>(Resource.Id.cancelButton);

                //handle click events
                okButton.Click += delegate
                {
                    //change the background and finish the current activity (controller)
                    okButton.SetBackgroundResource(Resource.Drawable.okbutton_pressed);                    
                    //Close the Activity
                    Finish();
                    //Ends the Current Session!
                    Settings_Singleton.Instance.CloseCurrentSession();                  
                    
                    
                };//end delegate
                cancelButton.Click += delegate
                {
                    //change the background, cancel the dialog and set the contentview back to the controller
                    //so we can continue with our activity
                    cancelButton.SetBackgroundResource(Resource.Drawable.cancelbutton_pressed);
                    dialog.Cancel();
                    this.OnCreate(bundle);                    
                };//end delegate

                return true;
            }//end if
 	        return base.OnKeyDown(keyCode, e);
        }//end overrided method OnKeyDown

        #endregion

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

            wView.StopLoading();
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

            wView.StopLoading();
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

            wView.LoadUrl(path);
            wView.RequestFocus();
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

            wView.StopLoading();
        }//end method FlipToPitch

        public void releaseClick(object sender, EventArgs e)
        {
            updateThread.Abort();
            wView.StopLoading();

            //set the content view to the dialogwindow content
            //outside this contentview we are unable to find our resources
            SetContentView(Resource.Layout.DialogWindow);

            //create a new dialog
            dialog = new Dialog(this);
            dialog.SetContentView(Resource.Layout.DialogWindow);
            dialog.SetTitle("Release ring");
            dialog.SetCancelable(true);

            //set the dialog text
            dialogTxt = FindViewById<TextView>(Resource.Id.dialogText);
            dialogTxt.Text = "Are you sure you want to release the ring?";

            //set the buttons
            okButton = FindViewById<Button>(Resource.Id.okButton);
            cancelButton = FindViewById<Button>(Resource.Id.cancelButton);

            //handle click events
            okButton.Click += delegate
            {
                //change the background and finish the current activity (controller)
                okButton.SetBackgroundResource(Resource.Drawable.okbutton_pressed);
                
                //set bool true to get carmen to release the ring
                Send_Singleton.Instance.releaseRing = 1;

                SetContentView(Resource.Layout.Controller);
                this.OnCreate(bundle);
                //Close the dialog
                dialog.Cancel();
            };//end delegate
            cancelButton.Click += delegate
            {
                //change the background, cancel the dialog and set the contentview back to the controller
                //so we can continue with our activity
                cancelButton.SetBackgroundResource(Resource.Drawable.cancelbutton_pressed);
                dialog.Cancel();
                this.OnCreate(bundle);
            };//end delegate
        }//end method releaseClick

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

        #region Update
        /// <summary>
        /// Call Update to update The Views
        /// </summary>
        public void Update()
        {
            while (true)
            {
                switch (flipper.DisplayedChild)
                {
                    case 1:
                        UpdateOverView();
                        break;
                    case 2:
                        UpdateBattery();
                        break;
                }//end switch
                Thread.Sleep(Settings_Singleton.Instance.Controller_UpdateRate);
            }            

        }//end method Update

        #region UpdateOverView
        /// <summary>
        /// Call UpdateOverView to only update the OverView GUI
        /// </summary>
        public void UpdateOverView()
        {
            UpdateEngineLF();
            UpdateEningeLR();
            UpdateEningeRF();
            UpdateEningeRR();
            UpdateBattery1();
            UpdateBattery2();
        }//end method UpdateOverView


        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEngineLF()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0T].Values.Length - 1].Value; //C

            Eng_Curr = FindViewById<TextView>(Resource.Id.EngLF1);
            Eng_Volt = FindViewById<TextView>(Resource.Id.EngLF2);
            Eng_Temp = FindViewById<TextView>(Resource.Id.EngLF3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0A].Unity);
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0V].Unity);
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0T].Unity);

            RunOnUiThread(() => Throttle0.Text = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0Th].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0Th].Values.Length - 1].Value.ToString());
        }//end method UpdateEngineLF

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeRF()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1T].Values.Length - 1].Value; //C

            Eng_Curr = FindViewById<TextView>(Resource.Id.EngRF1);
            Eng_Volt = FindViewById<TextView>(Resource.Id.EngRF2);
            Eng_Temp = FindViewById<TextView>(Resource.Id.EngRF3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1A].Unity);
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1V].Unity);
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1T].Unity);

            RunOnUiThread(() => Throttle1.Text = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1Th].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1Th].Values.Length - 1].Value.ToString());
        }//end method UpdateEngineRF

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeLR()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2T].Values.Length - 1].Value; //C

            Eng_Curr = FindViewById<TextView>(Resource.Id.EngLR1);
            Eng_Volt = FindViewById<TextView>(Resource.Id.EngLR2);
            Eng_Temp = FindViewById<TextView>(Resource.Id.EngLR3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2A].Unity);
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2V].Unity);
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2T].Unity);

            RunOnUiThread(() => Throttle2.Text = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2Th].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2Th].Values.Length - 1].Value.ToString());
        }//end method UpdateEngineLR

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeRR()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3T].Values.Length - 1].Value; //C
            
            Eng_Curr = FindViewById<TextView>(Resource.Id.EngRR1);
            Eng_Volt = FindViewById<TextView>(Resource.Id.EngRR2);
            Eng_Temp = FindViewById<TextView>(Resource.Id.EngRR3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3A].Unity);
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3V].Unity);
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3T].Unity);

            RunOnUiThread(() => Throttle3.Text = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3Th].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M3Th].Values.Length - 1].Value.ToString());
        }//end method UpdateEngineRR

        /// <summary>
        /// Call to update Battery X on the Overview GUI
        /// </summary>
        public void UpdateBattery1()
        {
            float _Accu_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length - 1].Value; //A
            float _Accu_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length - 1].Value; //A
            float _Accu_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length - 1].Value; //A
            
            Accu_Curr = FindViewById<TextView>(Resource.Id.Accu1Curr);
            Accu_Volt = FindViewById<TextView>(Resource.Id.Accu1Voltage);
            Accu_Temp = FindViewById<TextView>(Resource.Id.Accu1Temp);
            RunOnUiThread(() => Accu_Curr.Text = _Accu_Curr + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Unity);
            RunOnUiThread(() => Accu_Volt.Text = _Accu_Volt + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Unity);
            RunOnUiThread(() => Accu_Temp.Text = _Accu_Temp + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Unity);
        }//end method UpdateBattery1

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateBattery2()
        {
            float _Accu_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length - 1].Value; //A
            float _Accu_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length - 1].Value; //A
            float _Accu_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length - 1].Value; //A
            
            Accu_Curr = FindViewById<TextView>(Resource.Id.Accu2Curr);
            Accu_Volt = FindViewById<TextView>(Resource.Id.Accu2Voltage);
            Accu_Temp = FindViewById<TextView>(Resource.Id.Accu2Temp);
            RunOnUiThread(() => Accu_Curr.Text = _Accu_Curr + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Unity);
            RunOnUiThread(() => Accu_Volt.Text = _Accu_Volt + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Unity);
            RunOnUiThread(() => Accu_Temp.Text = _Accu_Temp + Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Unity);
        }//end method UpdateBattery2

        #endregion

        #region UpdateBattery
        /// <summary>
        /// Call to update The Battery GUI
        /// </summary>
        public void UpdateBattery()
        {

        }//end method UpdateBattery

        #endregion

        #endregion
    }//end class Controller
}//end namepsace OML_App