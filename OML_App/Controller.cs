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

        //graphview layout
        RelativeLayout graphview;

        //Bundle to be able to return from Dialog
        Bundle bundle;

        //ActiveIndex for Batteryview
        public static int activeIndex { get; set; }

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

            // Create your application here
            SetContentView(Resource.Layout.Controller);

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
        }//end method FlipToCamera

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

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngLF1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngLF2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngLF3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }//end method UpdateEngineLF

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeRF()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M1T].Values.Length - 1].Value; //C

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngRF1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngRF2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngRF3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }//end method UpdateEngineRF

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeLR()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M2T].Values.Length - 1].Value; //C

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngLR1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngLR2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngLR3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }//end method UpdateEngineLR

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeRR()
        {
            float _Eng_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0A].Values.Length - 1].Value; //A
            float _Eng_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0V].Values.Length - 1].Value; //V
            float _Eng_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.M0T].Values.Length - 1].Value; //C
            
            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngRR1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngRR2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngRR3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }//end method UpdateEngineRR

        /// <summary>
        /// Call to update Battery X on the Overview GUI
        /// </summary>
        public void UpdateBattery1()
        {
            float _Accu_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length - 1].Value; //A
            float _Accu_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length - 1].Value; //A
            float _Accu_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length - 1].Value; //A
            
            TextView Accu_Curr = FindViewById<TextView>(Resource.Id.Accu1Curr);
            TextView Accu_Volt = FindViewById<TextView>(Resource.Id.Accu1Voltage);
            TextView Accu_Temp = FindViewById<TextView>(Resource.Id.Accu1Temp);
            RunOnUiThread(() => Accu_Curr.Text = _Accu_Curr + "A");
            RunOnUiThread(() => Accu_Volt.Text = _Accu_Volt + "V");
            RunOnUiThread(() => Accu_Temp.Text = _Accu_Temp + "C");
        }//end method UpdateBattery1

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateBattery2()
        {
            float _Accu_Curr = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length - 1].Value; //A
            float _Accu_Volt = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length - 1].Value; //A
            float _Accu_Temp = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length - 1].Value; //A
            
            TextView Accu_Curr = FindViewById<TextView>(Resource.Id.Accu2Curr);
            TextView Accu_Volt = FindViewById<TextView>(Resource.Id.Accu2Voltage);
            TextView Accu_Temp = FindViewById<TextView>(Resource.Id.Accu2Temp);
            RunOnUiThread(() => Accu_Curr.Text = _Accu_Curr + "A");
            RunOnUiThread(() => Accu_Volt.Text = _Accu_Volt + "V");
            RunOnUiThread(() => Accu_Temp.Text = _Accu_Temp + "C");
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