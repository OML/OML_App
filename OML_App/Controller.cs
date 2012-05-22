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
            Button btnOpenNewActivity1 = FindViewById<Button>(Resource.Id.overviewButton);
            btnOpenNewActivity1.Click += new EventHandler(FlipToOverView);

            Button btnOpenNewActivity = FindViewById<Button>(Resource.Id.batteryButton);
            btnOpenNewActivity.Click += new EventHandler(FlipToBattery);

        }

        /// <summary>
        /// Flips the Current View to First
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToOverView(object sender, EventArgs e)
        {
            flipper.DisplayedChild = 0;
        }

        /// <summary>
        /// Flips the Current View to Second
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FlipToBattery(object sender, EventArgs e)
        {
            flipper.DisplayedChild = 1;
        }

        /// <summary>
        /// Call Update to update The Views
        /// </summary>
        public void Update()
        {
            switch (flipper.CurrentView.Id)
            {
                case 0:
                    UpdateOverView();
                    break;
                case 1:
                    UpdateBattery();
                    break;
            }

        }

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
        }


        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEngineLF()
        {
            float _Eng_Curr = 2.30f; //A
            float _Eng_Volt = 2.3f; //V
            float _Eng_Temp = 34.4f; //C

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngLF1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngLF2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngLF3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeRF()
        {
            float _Eng_Curr = 2.30f; //A
            float _Eng_Volt = 2.3f; //V
            float _Eng_Temp = 34.4f; //C

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngRF1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngRF2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngRF3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeLR()
        {
            float _Eng_Curr = 2.30f; //A
            float _Eng_Volt = 2.3f; //V
            float _Eng_Temp = 34.4f; //C

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngLR1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngLR2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngLR3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateEningeRR()
        {
            float _Eng_Curr = 2.30f; //A
            float _Eng_Volt = 2.3f; //V
            float _Eng_Temp = 34.4f; //C

            TextView Eng_Curr = FindViewById<TextView>(Resource.Id.EngRR1);
            TextView Eng_Volt = FindViewById<TextView>(Resource.Id.EngRR2);
            TextView Eng_Temp = FindViewById<TextView>(Resource.Id.EngRR3);
            RunOnUiThread(() => Eng_Curr.Text = _Eng_Curr + "A");
            RunOnUiThread(() => Eng_Volt.Text = _Eng_Volt + "V");
            RunOnUiThread(() => Eng_Temp.Text = _Eng_Temp + "C");
        }

        /// <summary>
        /// Call to update Battery X on the Overview GUI
        /// </summary>
        public void UpdateBattery1()
        {
            float _Accu_Curr = 2.30f; //A
            float _Accu_Volt = 2.3f; //V
            float _Accu_Temp = 34.4f; //C

            TextView Accu_Curr = FindViewById<TextView>(Resource.Id.Accu1Curr);
            TextView Accu_Volt = FindViewById<TextView>(Resource.Id.Accu1Voltage);
            TextView Accu_Temp = FindViewById<TextView>(Resource.Id.Accu1Temp);
            RunOnUiThread(() => Accu_Curr.Text = _Accu_Curr + "A");
            RunOnUiThread(() => Accu_Volt.Text = _Accu_Volt + "V");
            RunOnUiThread(() => Accu_Temp.Text = _Accu_Temp + "C");
        }

        /// <summary>
        /// Call to update Engine XX on the Overview GUI
        /// </summary>
        public void UpdateBattery2()
        {
            float _Accu_Curr = 2.30f; //A
            float _Accu_Volt = 2.3f; //V
            float _Accu_Temp = 34.4f; //C

            TextView Accu_Curr = FindViewById<TextView>(Resource.Id.Accu2Curr);
            TextView Accu_Volt = FindViewById<TextView>(Resource.Id.Accu2Voltage);
            TextView Accu_Temp = FindViewById<TextView>(Resource.Id.Accu2Temp);
            RunOnUiThread(() => Accu_Curr.Text = _Accu_Curr + "A");
            RunOnUiThread(() => Accu_Volt.Text = _Accu_Volt + "V");
            RunOnUiThread(() => Accu_Temp.Text = _Accu_Temp + "C");
        }

        /// <summary>
        /// Call to update The Battery GUI
        /// </summary>
        public void UpdateBattery()
        {

        }
    }
}