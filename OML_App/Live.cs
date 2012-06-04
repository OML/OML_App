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
using OML_App.Connection;
using OML_App.Data;
using OML_App.Setting;
using System.Threading;
using System.Diagnostics;
using Android.Views.InputMethods;


namespace OML_App
{
    [Activity(Label = "My Activity")]
    public class Live : Activity
    {
        //TextBox to input ipaddress / port
        EditText ipaddress;
        EditText port;       
        
        //Tcp Connection
        TCPClient connect;

        //dialog for exit message
        Dialog dialog;

        //dialog buttons
        Button okButton;
        Button cancelButton;

        //dialog textview
        TextView dialogTxt;

        //connect button
        Button connectB;

        //To return to Activity
        Bundle bundle;

        //Thread to take care off TimeOut and Starts the connection!
        Thread connectThread;
        Thread updateDialog;

        //Starts a timer to clock how long it takes to connect.
        Stopwatch connectStopwatch;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Live);

            //Setup Connect Button
            connectB = FindViewById<Button>(Resource.Id.connectbutton);

            connectB.Click += new EventHandler(ConnectButton);


            //Find Text Input
            ipaddress = FindViewById<EditText>(Resource.Id.ipaddress);
            ipaddress.Text = Settings_Singleton.Instance.IpAdress; //Default IP Address

            port = FindViewById<EditText>(Resource.Id.port);
            port.Text = Settings_Singleton.Instance.Port; //Default Port
        }

        protected override void OnResume()
        {
            base.OnResume();

            //reset the background on resume
            connectB.SetBackgroundResource(Resource.Drawable.connectbutton);
        }

        private void ConnectButton(object sender, EventArgs e)
        {
            //set background to pressed
            connectB.SetBackgroundResource(Resource.Drawable.connectbutton_pressed);
            
            //Make Ip Check and more!
            CheckIP();           
        }       

        /// <summary>
        /// CheckIP will start up the TCP connection , or its will go "Hard coded" (ip=1234) in to the Controller
        /// </summary>
        /// <returns></returns>
        private void CheckIP()
        {
            //Check for the Hard coded IP to debug
            if (ipaddress.Text == "1234")
            {
                //Create Session and Sensors
                Receive_Singleton.Instance.init();
                //Load the Controller
                LoadController();
            }
            else
            {
                //Create Session and Sensors
                Receive_Singleton.Instance.init();

                //some hardcoded values To send in the first Packet
                Send_Singleton.Instance.speed = 1;
                Send_Singleton.Instance.sound = 0;
                Send_Singleton.Instance.right = 0;
                Send_Singleton.Instance.left = 0;
                Send_Singleton.Instance.engine0 = 10;
                Send_Singleton.Instance.engine1 = 10;
                Send_Singleton.Instance.engine2 = 10;
                Send_Singleton.Instance.engine3 = 10;
                Send_Singleton.Instance.Calibration_mode = 0;
                Send_Singleton.Instance.voltage = 1;
                Send_Singleton.Instance.Calibration_mode = 1;
                Send_Singleton.Instance.Gyro = 1;
                Send_Singleton.Instance.temperature = 1;
                Send_Singleton.Instance.throttle = 1;


                //Get Ipadress and Port                
                if (port.Text == "")
                {
                    //Display Diaglog box , Needs a valid port number!                    
                    //Display Dialog
                    DisplayDialogOkey();
                    //Okey button only!
                    dialogTxt.Text = "In-valid port number";
                }
                else
                {
                    //Start the Connection in a different Thread! (so you can still control all the buttons)
                    connectThread = new Thread(new ThreadStart(Connect));
                    connectThread.Start();                    

                    //Start the Update Dialog to wait for the connection!
                    updateDialog = new Thread(new ThreadStart(WaitForConnection)); //Will be started by the ConnectThread
                    

                    //Open Connect Dialog
                    DisplayDialogCancel();
                }
            }
        }

        private void Connect()
        {
            //Get all the info
            string ip = ipaddress.Text;
            int portnr = Convert.ToInt16(port.Text);

            //Start the timer ( to time Timeouts )
            connectStopwatch = new Stopwatch();
            connectStopwatch.Start();

            //Start connecting
            connect = new TCPClient(ip, portnr);
            //Set TCP in Settings_Singleton
            Settings_Singleton.Instance.TCP_Current = connect;
            updateDialog.Start();
        }

        private void WaitForConnection()
        {
            //Make an loop
            bool tryConnect = true;
            int counter = 0;            

            while (tryConnect)
            {
                //Check connect State.
                if (connect.connected)
                {
                    //Succesfull connection
                    //Start Session / Close Dialog / Open Controller
                    Settings_Singleton.Instance.LiveSession = true;
                    dialog.Cancel();
                    LoadController();
                    //Close Loop
                    tryConnect = false;
                    connectStopwatch.Stop();//Stop the Stopwatch
                    connectStopwatch.Reset();//Reset for the next time
                }
                else if (connectStopwatch.ElapsedMilliseconds > 5000)//CheckTimer for possible timeout
                {
                    //Close the Connection It a Timeout!
                    connectThread.Abort(); //Kill the ConnectThread


                    //Display an Error message!
                    RunOnUiThread(() => dialogTxt.Text = "Unable to Connect: IP / Port not reachable");

                    //Stop the loop, Connection not possible
                    tryConnect = false;
                    connectStopwatch.Stop();//Stop the Stopwatch
                    connectStopwatch.Reset();//Reset for the next time
                }
                else
                {
                    //Wait abit more, Still Trying to connect!
                    Thread.Sleep(200);
                    //Update The Dialog to make sure people are waiting
                    //Counter for the Dots
                    int dots = (counter++) % 3;
                    if (dots == 0)
                    {
                        RunOnUiThread(() => dialogTxt.Text = "Connection .");
                    }
                    else if (dots == 1)
                    {
                        RunOnUiThread(() => dialogTxt.Text = "Connection ..");
                    }
                    else
                    {
                        RunOnUiThread(() => dialogTxt.Text = "Connection ...");
                    }
                }//Close Else                
            }//Close While
        }//Close WaitForConnection!

        private void DisplayDialogOkey()
        {
            //Close Keyboard
            InputMethodManager inputMgr = GetSystemService(InputMethodService) as InputMethodManager;
            if (CurrentFocus != null)
                inputMgr.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
            
            //Open Connect Dialog!
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
            

            //set the buttons
            okButton = FindViewById<Button>(Resource.Id.okButton);
            cancelButton = FindViewById<Button>(Resource.Id.cancelButton);

            //Hide Cancel buttom, Only Okey needed            
            cancelButton.Visibility = ViewStates.Invisible;            

            okButton.Click += delegate
            {
                //change the background, close the dialog and set the contentview back to the live / connection screen
                //so we can continue with our activity
                //Draw Click
                okButton.SetBackgroundResource(Resource.Drawable.okbutton_pressed);
                //Close Dialog
                dialog.Cancel();
                this.OnCreate(bundle);
            };//end delegate
        }

        private void DisplayDialogCancel()
        {
            //Close Keyboard
            InputMethodManager inputMgr = GetSystemService(InputMethodService) as InputMethodManager;
            if(CurrentFocus != null)
                inputMgr.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            //Open Connect Dialog!
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


            //set the buttons
            okButton = FindViewById<Button>(Resource.Id.okButton);
            cancelButton = FindViewById<Button>(Resource.Id.cancelButton);

            //Hide Okey buttom, Only Cancel needed            
            okButton.Visibility = ViewStates.Invisible;

            cancelButton.Click += delegate
            {
                //change the background, cancel the dialog and set the contentview back to the live / connection screen
                //so we can continue with our activity
                //Draw Click
                cancelButton.SetBackgroundResource(Resource.Drawable.cancelbutton_pressed);
                //Close Dialog
                dialog.Cancel();
                this.OnCreate(bundle);
            };//end delegate
        }

        private void LoadController()
        {
            Intent i = new Intent();
            i.SetClass(this, typeof(Controller));
            i.AddFlags(ActivityFlags.NewTask);
            StartActivity(i);
        }

    }
}