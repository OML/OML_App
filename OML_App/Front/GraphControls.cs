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
using Android.Util;
using Android.Graphics;
using OML_App.Data;
using OML_App.Setting;
using System.Collections;
using OML_App.Front;

namespace OML_App
{
    public class GraphControls : View
    {
        //6 textviews to show the text on the batterybuttons
        TextView v0;
        TextView v1;
        TextView a0;
        TextView a1;
        TextView t0;
        TextView t1;

        //values for our textviews
        float volt0 = 0;
        float volt1 = 0;
        float amp0 = 0;
        float amp1 = 0;
        float temp0 = 0;
        float temp1 = 0;
        TimeSpan time;

        //two-dimensional arraylists to hold our graph values
        //[value][time]
        ArrayList voltvalue0 = new ArrayList();
        ArrayList voltvalue1 = new ArrayList();
        ArrayList ampvalue0 = new ArrayList();
        ArrayList ampvalue1 = new ArrayList();
        ArrayList tempvalue0 = new ArrayList();
        ArrayList tempvalue1 = new ArrayList();

        public GraphControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }//end constructor

        public GraphControls(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }//end constructor

        private void Initialize()
        {
        }//end method Initialize

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            //set the time
            time = DateTime.Now - Receive_Singleton.Instance.Current_ses.StartTime;

            //set the updated textview values
            if (volt0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value)
            {
                volt0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value;
                voltvalue0.Add(new GraphValue(volt0, time));
            }//end if

            if (volt1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length].Value)
            {
                volt1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length].Value;
                voltvalue1.Add(new GraphValue(volt1, time));
            }//end if

            if (amp0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length].Value)
            {
                amp0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length].Value;
                ampvalue0.Add(new GraphValue(amp0, time));
            }//end if

            if (amp1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length].Value)
            {
                amp1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length].Value;
                ampvalue1.Add(new GraphValue(amp1, time));
            }//end if

            if (temp0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length].Value)
            {
                temp0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length].Value;
                tempvalue0.Add(new GraphValue(temp0, time));
            }//end if

            if (temp1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length].Value)
            {
                temp1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length].Value;
                tempvalue1.Add(new GraphValue(temp1, time));
            }//end if
            

            //set the textviews (didnt work in the initialize method...
            v0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.volttxt0);
            v1 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.volttxt1);
            a0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.amptxt0);
            a1 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.amptxt1);
            t0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.temptxt0);
            t1 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.temptxt1);

            //set the values in the textviews, showing them on the screen
            v0.Text = volt0.ToString();
            v1.Text = volt1.ToString();
            a0.Text = amp0.ToString();
            a1.Text = amp1.ToString();
            t0.Text = temp0.ToString();
            t1.Text = temp1.ToString();

            //invalidate to request a redraw
            Invalidate();
        }

        /// <summary>
        /// method to draw the voltage graph for battery 1
        /// </summary>
        /// <param name="canvas"></param>
        private void drawVolt0Graph(Canvas canvas)
        {

        }

        /// <summary>
        /// method to draw the voltage graph for battery 2
        /// </summary>
        /// <param name="canvas"></param>
        private void drawVolt1Graph(Canvas canvas)
        {

        }

        /// <summary>
        /// method to draw the ampere graph for battery 1
        /// </summary>
        /// <param name="canvas"></param>
        private void drawAmp0Graph(Canvas canvas)
        {

        }

        /// <summary>
        /// method to draw the ampere graph for battery 2
        /// </summary>
        /// <param name="canvas"></param>
        private void drawAmp1Graph(Canvas canvas)
        {

        }

        /// <summary>
        /// method to draw the temperature graph for battery 1
        /// </summary>
        /// <param name="canvas"></param>
        private void drawTemp0Graph(Canvas canvas)
        {

        }

        /// <summary>
        /// method to draw the temperature graph for battery 2
        /// </summary>
        /// <param name="canvas"></param>
        private void drawTemp1Graph(Canvas canvas)
        {

        }
    }
}