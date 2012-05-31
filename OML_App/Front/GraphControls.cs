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

        //paint to draw with
        Paint paint = new Paint();

        //int representing the active graph within the batteryview
        //0 if batteryview is not active, otherwise 1 - 6
        int activeIndex = 0;

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

            paint.SetARGB(0,0,0,0);

            //set the time
            time = DateTime.Now - Receive_Singleton.Instance.Current_ses.StartTime;

            //set the updated textview values
            if (volt0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value)
            {
                volt0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value;
                voltvalue0.Add(new GraphValue(volt0, time));

                //if we exceed 100 elements remove the first
                if (voltvalue0.Count > 100)
                    voltvalue0.RemoveAt(0);

                //draw the graph volt0 graph is the currently viewed graph
                if (activeIndex == 1)
                    drawGraph(canvas, voltvalue0);
            }//end if

            if (volt1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length].Value)
            {
                volt1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length].Value;
                voltvalue1.Add(new GraphValue(volt1, time));

                if (voltvalue1.Count > 100)
                    voltvalue1.RemoveAt(0);

                if (activeIndex == 2)
                    drawGraph(canvas, voltvalue1);
            }//end if

            if (amp0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length].Value)
            {
                amp0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length].Value;
                ampvalue0.Add(new GraphValue(amp0, time));

                if (ampvalue0.Count > 100)
                    ampvalue0.RemoveAt(0);

                if (activeIndex == 3)
                    drawGraph(canvas, ampvalue0);
            }//end if

            if (amp1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length].Value)
            {
                amp1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length].Value;
                ampvalue1.Add(new GraphValue(amp1, time));

                if (ampvalue1.Count > 100)
                    ampvalue1.RemoveAt(0);

                if (activeIndex == 4)
                    drawGraph(canvas, ampvalue1);
            }//end if

            if (temp0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length].Value)
            {
                temp0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length].Value;
                tempvalue0.Add(new GraphValue(temp0, time));

                if (tempvalue0.Count > 100)
                    tempvalue0.RemoveAt(0);

                if (activeIndex == 5)
                    drawGraph(canvas, tempvalue0);
            }//end if

            if (temp1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length].Value)
            {
                temp1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length].Value;
                tempvalue1.Add(new GraphValue(temp1, time));

                if (tempvalue1.Count > 100)
                    tempvalue1.RemoveAt(0);

                if (activeIndex == 6)
                    drawGraph(canvas, tempvalue1);
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
        }//end overrided method OnDraw

        /// <summary>
        /// method to draw the voltage graph for battery 1
        /// </summary>
        /// <param name="canvas"></param>
        private void drawGraph(Canvas canvas, ArrayList list)
        {
            //loop through all values in our array
            for (int i = 0; i < 99; i++)
            {
                //get our y-axis value from the arraylist
                GraphValue value0 = (GraphValue)list[i];
                GraphValue value1 = (GraphValue)list[i + 1];
                float yValue0 = value0.value;
                float yValue1 = value1.value;

                //draw the point on our graph
                canvas.DrawLine(i * 5.5f, yValue0, (i + 1) * 5.5f, yValue1, paint);
            }//end for
        }//end method drawVolt0Graph
    }//end class GraphControls
}//end namespace OML_App