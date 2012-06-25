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
        Button extra;
        Button stopR;
        Button stopL;

        //battery buttons
        Button volt0;
        Button amp0;
        Button temp0;
        Button volt1;
        Button amp1;
        Button temp1;

        //audio buttons
        Button ab1;
        Button ab2;
        Button ab3;
        Button ab4;
        Button ab5;
        Button ab6;
        Button ab7;
        Button ab8;
        Button ab9;

        //LED button
        Button LEDButton;

        //audio textviews
        TextView atv1;
        TextView atv2;
        TextView atv3;
        TextView atv4;
        TextView atv5;
        TextView atv6;
        TextView atv7;
        TextView atv8;
        TextView atv9;

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

        //Random for our color value
        Random rnd = new Random();

        //bools to keep track of our stopbuttons
        private bool rStopped = false;
        private bool lStopped = false;

        private int stopValue = 16;

        //ActiveIndex for Batteryview
        public static int activeIndex { get; set; }

        //bool to set disco mode for our LEDs
        public static bool discoInferno { get; set; }

        //webview and path to our camera feed
        private WebView wView;
        string path = "http://192.168.209.88:8090/webcam.mjpeg";

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

            //set webview and client
            wView = FindViewById<WebView>(Resource.Id.vidview);

            //midbox viewflipper
            flipper = FindViewById<ViewFlipper>(Resource.Id.flipper);

            //graphview
            graphview = FindViewById<RelativeLayout>(Resource.Id.graphview);

            #region Eventhandlers control buttons

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

            stopR = FindViewById<Button>(Resource.Id.stopbuttonR);
            stopR.Click += new EventHandler(stopRclick);

            stopL = FindViewById<Button>(Resource.Id.stopbuttonL);
            stopL.Click += new EventHandler(stopLclick);

            #endregion

            #region Eventhandlers battery buttons

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

            #endregion

            #region Eventhandlers + Textviews audio buttons

            extra = FindViewById<Button>(Resource.Id.extrabutton);
            extra.Click += new EventHandler(extraClick);

            atv1 = FindViewById<TextView>(Resource.Id.aTxt1);
            atv1.Text = Receive_Singleton.Instance.Track1;

            ab1 = FindViewById<Button>(Resource.Id.aButton1);
            ab1.Click += delegate { playTrack(1); };

            atv2 = FindViewById<TextView>(Resource.Id.aTxt2);
            atv2.Text = Receive_Singleton.Instance.Track2;
            
            ab2 = FindViewById<Button>(Resource.Id.aButton2);
            ab2.Click += delegate { playTrack(2); };

            atv3 = FindViewById<TextView>(Resource.Id.aTxt3);
            atv3.Text = Receive_Singleton.Instance.Track3;

            ab3 = FindViewById<Button>(Resource.Id.aButton3);
            ab3.Click += delegate { playTrack(3); };

            atv4 = FindViewById<TextView>(Resource.Id.aTxt4);
            atv4.Text = Receive_Singleton.Instance.Track4;

            ab4 = FindViewById<Button>(Resource.Id.aButton4);
            ab4.Click += delegate { playTrack(4); };

            atv5 = FindViewById<TextView>(Resource.Id.aTxt5);
            atv5.Text = Receive_Singleton.Instance.Track5;

            ab5 = FindViewById<Button>(Resource.Id.aButton5);
            ab5.Click += delegate { playTrack(5); };

            atv6 = FindViewById<TextView>(Resource.Id.aTxt6);
            atv6.Text = Receive_Singleton.Instance.Track6;

            ab6 = FindViewById<Button>(Resource.Id.aButton6);
            ab6.Click += delegate { playTrack(6); };

            atv7 = FindViewById<TextView>(Resource.Id.aTxt7);
            atv7.Text = Receive_Singleton.Instance.Track7;

            ab7 = FindViewById<Button>(Resource.Id.aButton7);
            ab7.Click += delegate { playTrack(7); };

            atv8 = FindViewById<TextView>(Resource.Id.aTxt8);
            atv8.Text = Receive_Singleton.Instance.Track8;

            ab8 = FindViewById<Button>(Resource.Id.aButton8);
            ab8.Click += delegate { playTrack(8); };

            atv9 = FindViewById<TextView>(Resource.Id.aTxt9);
            atv9.Text = Receive_Singleton.Instance.Track9;

            ab9 = FindViewById<Button>(Resource.Id.aButton9);
            ab9.Click += delegate { playTrack(9); };
            
            #endregion

            LEDButton = FindViewById<Button>(Resource.Id.discoButton);
            LEDButton.Click += new EventHandler(LEDClick);

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
                    //Close the Viewer TCP
                    Settings_Singleton.Instance.CloseViewerTCP();
                    
                    
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

        #region Flippers and sub-events (Eventhandlers)

        #region Control flippers
        /// <summary>
        /// Flips the Current View to First
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToOverView(object sender, EventArgs e)
        {
            //reset the button drawable and flip to the correct child
            btnDrawableReset();
            flipper.DisplayedChild = 1;

            //change the background on click
            overview.SetBackgroundResource(Resource.Drawable.overviewbutton_pressed);
        }//end method FlipToOverView

        /// <summary>
        /// Flips the Current View to Second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToBattery(object sender, EventArgs e)
        {
            //reset the button drawable and flip to the correct child
            btnDrawableReset();
            flipper.DisplayedChild = 2;

            //change the background on click
            battery.SetBackgroundResource(Resource.Drawable.batterybutton_pressed);
        }//end method FlipToBattery

        /// <summary>
        /// Flips the Current View to Third
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToCamera(object sender, EventArgs e)
        {
            //reset the button drawable and flip to the correct child
            btnDrawableReset();
            flipper.DisplayedChild = 3;

            //change the background on click
            camera.SetBackgroundResource(Resource.Drawable.camerabutton_pressed);

            wView.Settings.JavaScriptEnabled = true;
            wView.LoadUrl(path);
            //wView.LoadUrl(path);
            //wView.Invalidate();
            //wView.RequestFocus();
        }//end method FlipToCamera

        /// <summary>
        /// Flips the Current View to Fourth
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToPitch(object sender, EventArgs e)
        {
            //reset the button drawable and flip to the correct child
            btnDrawableReset();
            flipper.DisplayedChild = 4;

            //change the background on click
            orient.SetBackgroundResource(Resource.Drawable.orientbutton_pressed);
        }//end method FlipToPitch

        /// <summary>
        /// Opens a dialog window with the choice to release the object we picked up (ring)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void releaseClick(object sender, EventArgs e)
        {
            //reset the button drawable
            btnDrawableReset();
            //set the background
            release.SetBackgroundResource(Resource.Drawable.releasebutton_pressed);

            updateThread.Abort();

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

        /// <summary>
        /// Flips the current view to Fifth
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void extraClick(object sender, EventArgs e)
        {
            //reset the button drawable and flip to the correct child
            btnDrawableReset();
            flipper.DisplayedChild = 5;

            //change the background on click
            extra.SetBackgroundResource(Resource.Drawable.extrabutton_pressed);
        }//end method audioClick

        /// <summary>
        /// Puts the vehicle to an instant standstill when pressed with the left stop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stopRclick(object sender, EventArgs e)
        {
            if (!rStopped)
            {
                stopR.SetBackgroundResource(Resource.Drawable.stopbutton_pressed);
                rStopped = true;
            }//end if

            else
            {
                stopR.SetBackgroundResource(Resource.Drawable.stopbutton);
                rStopped = false;
            }//end else
        }//end method stopRclick

        /// <summary>
        /// Puts the vehicle to an instant standstill when pressed with the right stop button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void stopLclick(object sender, EventArgs e)
        {
            if (!lStopped)
            {
                stopL.SetBackgroundResource(Resource.Drawable.stopbutton_pressed);
                lStopped = true;
            }//end if

            else
            {
                stopL.SetBackgroundResource(Resource.Drawable.stopbutton);
                lStopped = false;
            }//end else
        }//end method stopLclick

        #endregion

        #region Sub-Flips Battery (Eventhandlers)

        public void FlipToVolt0(object sender, EventArgs e)
        {
            activeIndex = 1;

            //reset the battery button background
            batteryBtnDrawableReset();

            //change background on click
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton_pressed);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToVolt0

        public void FlipToVolt1(object sender, EventArgs e)
        {
            activeIndex = 2;

            //reset the battery button background
            batteryBtnDrawableReset();

            //change background on click
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton_pressed);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method

        public void FlipToAmp0(object sender, EventArgs e)
        {
            activeIndex = 3;

            //reset the battery button background
            batteryBtnDrawableReset();

            //change background on click
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton_pressed);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToAmp0

        public void FlipToAmp1(object sender, EventArgs e)
        {
            activeIndex = 4;

            //reset the battery button background
            batteryBtnDrawableReset();

            //change background on click
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton_pressed);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToAmp1

        public void FlipToTemp0(object sender, EventArgs e)
        {
            activeIndex = 5;

            //reset the battery button background
            batteryBtnDrawableReset();

            //change background on click
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton_pressed);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToTemp0

        public void FlipToTemp1(object sender, EventArgs e)
        {
            activeIndex = 6;

            //reset the battery button background
            batteryBtnDrawableReset();

            //change background on click
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton_pressed);

            //change graphview backgroud
            graphview.SetBackgroundResource(Resource.Drawable.basegraph);
        }//end method FlipToTemp1

        public void batteryBtnDrawableReset()
        {
            temp1.SetBackgroundResource(Resource.Drawable.tempbutton);
            volt0.SetBackgroundResource(Resource.Drawable.voltbutton);
            volt1.SetBackgroundResource(Resource.Drawable.voltbutton);
            amp0.SetBackgroundResource(Resource.Drawable.ampbutton);
            amp1.SetBackgroundResource(Resource.Drawable.ampbutton);
            temp0.SetBackgroundResource(Resource.Drawable.tempbutton);
        }//end method batteryBtnDrawableReset

        #endregion

        #region play track (audio)

        /// <summary>
        /// Plays the track depending on the pressed button (#1-9)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="btn">number #1-9</param>
        public void playTrack(int btnIndex)
        {
            //switch the button index, to check which 
            //button were dealing with and what to do..
            switch (btnIndex)
            {
                case 1:
                    //change the background drawables
                    resetAudioDrawable();
                    ab1.SetBackgroundResource(Resource.Drawable.audiobutton1_pressed);
                    //play 1st track
                    Send_Singleton.Instance.sound = 1;
                    break;
                case 2:
                    //change the background drawables
                    resetAudioDrawable();
                    ab2.SetBackgroundResource(Resource.Drawable.audiobutton2_pressed);
                    //play 2nd track
                    Send_Singleton.Instance.sound = 2;
                    break;
                case 3:
                    //change the background drawables
                    resetAudioDrawable();
                    ab3.SetBackgroundResource(Resource.Drawable.audiobutton3_pressed);
                    //play 3rd track
                    Send_Singleton.Instance.sound = 3;
                    break;
                case 4:
                    //change the background drawables
                    resetAudioDrawable();
                    ab4.SetBackgroundResource(Resource.Drawable.audiobutton4_pressed);
                    //play 4th track
                    Send_Singleton.Instance.sound = 4;
                    break;
                case 5:
                    //change the background drawables
                    resetAudioDrawable();
                    ab5.SetBackgroundResource(Resource.Drawable.audiobutton5_pressed);
                    //play 5th track
                    Send_Singleton.Instance.sound = 5;
                    break;
                case 6:
                    //change the background drawables
                    resetAudioDrawable();
                    ab6.SetBackgroundResource(Resource.Drawable.audiobutton6_pressed);
                    //play 6th track
                    Send_Singleton.Instance.sound = 6;
                    break;
                case 7:
                    //change the background drawables
                    resetAudioDrawable();
                    ab7.SetBackgroundResource(Resource.Drawable.audiobutton7_pressed);
                    //play 7th track
                    Send_Singleton.Instance.sound = 7;
                    break;
                case 8:
                    //change the background drawables
                    resetAudioDrawable();
                    ab8.SetBackgroundResource(Resource.Drawable.audiobutton8_pressed);
                    //play 8th track
                    Send_Singleton.Instance.sound = 8;
                    break;
                case 9:
                    //change the background drawables
                    resetAudioDrawable();
                    ab9.SetBackgroundResource(Resource.Drawable.audiobutton9_pressed);
                    //play 9th track
                    Send_Singleton.Instance.sound = 9;
                    break;
            }//end switch
        }//end method playTrack

        /// <summary>
        /// method to reset the backgrounds of the audio track buttons (half height buttons)
        /// </summary>
        public void resetAudioDrawable()
        {
            //reset the background drawables
            ab1.SetBackgroundResource(Resource.Drawable.audiobutton1);
            ab2.SetBackgroundResource(Resource.Drawable.audiobutton2);
            ab3.SetBackgroundResource(Resource.Drawable.audiobutton3);
            ab4.SetBackgroundResource(Resource.Drawable.audiobutton4);
            ab5.SetBackgroundResource(Resource.Drawable.audiobutton5);
            ab6.SetBackgroundResource(Resource.Drawable.audiobutton6);
            ab7.SetBackgroundResource(Resource.Drawable.audiobutton7);
            ab8.SetBackgroundResource(Resource.Drawable.audiobutton8);
            ab9.SetBackgroundResource(Resource.Drawable.audiobutton9);
        }//end method resetAudioDrawable

        #endregion

        #region LED Click

        public void LEDClick(object sender, EventArgs e)
        {
            //check wether we are in disco mode or not
            //and act accordingly
            if (!discoInferno)
            {
                //set the bool true, so we can read it in our ledControls class
                discoInferno = true;
                LEDButton.SetBackgroundResource(Resource.Drawable.minibutton_pressed);
            }//end if

            else
            {
                //set the bool false, so we can read it in our ledControls class
                discoInferno = false;
                LEDButton.SetBackgroundResource(Resource.Drawable.minibutton);
            }//end else
        }//end method LEDClick

        #endregion

        /// <summary>
        /// Method to reset the drawables of our buttons
        /// </summary>
        public void btnDrawableReset()
        {
            //change the background on click
            orient.SetBackgroundResource(Resource.Drawable.orientbutton);
            battery.SetBackgroundResource(Resource.Drawable.batterybutton);
            overview.SetBackgroundResource(Resource.Drawable.overviewbutton);
            camera.SetBackgroundResource(Resource.Drawable.camerabutton);
            extra.SetBackgroundResource(Resource.Drawable.extrabutton);
        }//end method btnDrawableReset

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
                    case 5:
                        UpdateLED();
                        break;
                }//end switch

                UpdateStopSequence();

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

        #region UpdateLED

        public void UpdateLED()
        {
            if (discoInferno)
            {
                LedControls.colorValue = (rnd.Next(1, 7) * 2);
                Send_Singleton.Instance.releaseRing = (int)LedControls.colorValue;
            }//end if
        }//end method UpdateLED
        #endregion

        #region UpdateStopSequence

        /// <summary>
        /// Update method to activate our emergency stop
        /// </summary>
        public void UpdateStopSequence()
        {
            if (rStopped && lStopped)
                Send_Singleton.Instance.releaseRing = stopValue;
        }//end method UpdateStopSequence

        #endregion

        #endregion
    }//end class Controller
}//end namepsace OML_App