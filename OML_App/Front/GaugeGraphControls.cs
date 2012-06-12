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
using System.Collections;
using OML_App.Front;

namespace OML_App
{
    class GaugeGraphControls : View
    {
        //textviews representing our x and y values on the axis
        TextView minX;
        TextView maxX;
        TextView minY;
        TextView maxY;

        //integers to hold our max and min Y-axis values
        float minimumY = 0;
        float maximumY = 0;

        float pitchval = 0.1f;
        float rollval = -0.1f;

        //starttime & timespan for intervals
        TimeSpan time;
        DateTime start = DateTime.Now;

        //point(0,0) on our graph
        const int originX = 100;
        const int originY = 120;

        //paint to draw with
        Paint paint0 = new Paint();
        Paint paint1 = new Paint();

        ArrayList pitchvalue = new ArrayList();
        ArrayList rollvalue = new ArrayList();

        public GaugeGraphControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }//end constructor

        public GaugeGraphControls(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }//end constructor

        private void Initialize()
        {
        }//end method Initialize

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);

            //set the paint to red or green depending on which line were drawing
            //green for pitch
            //teal for roll
            paint0.SetARGB(255, 0, 255, 0);
            paint1.SetARGB(255, 0, 255, 255);

            //get the textviews so we can set the text in the graph draw
            //minX = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.minX);
            //maxX = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.maxX);
            //minY = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.minY);
            //maxY = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.maxY);

            //set the time
            time = DateTime.Now - start;// Receive_Singleton.Instance.Current_ses.StartTime;

            //set the updated textview values
            if (pitchval != 0)//Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value)
            {
                pitchval += 0.01f;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value;
                pitchvalue.Add(new GraphValue(pitchval, time));

                //get the min and max Y
                minimumY = -10;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Min;
                maximumY = 10;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Max;

                ////show the min and max Y on the graph
                //minY.Text = minimumY.ToString();
                //maxY.Text = maximumY.ToString();

                //if we exceed 100 elements remove the first
                if (pitchvalue.Count > 100)
                    pitchvalue.RemoveAt(0);

                drawGraph(canvas, pitchvalue, minimumY, maximumY, paint0);
            }//end if

            //set the updated textview values
            if (rollval != 0)//Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value)
            {
                rollval -= 0.01f;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value;
                rollvalue.Add(new GraphValue(rollval, time));

                //get the min and max Y
                minimumY = -10;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Min;
                maximumY = 10;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Max;

                ////show the min and max Y on the graph
                //minY.Text = minimumY.ToString();
                //maxY.Text = maximumY.ToString();

                //if we exceed 100 elements remove the first
                if (rollvalue.Count > 100)
                    rollvalue.RemoveAt(0);

                drawGraph(canvas, rollvalue, minimumY, maximumY, paint1);
            }//end if

            Invalidate();
        }//end overrided method OnDraw

        /// <summary>
        /// method to draw the voltage graph for battery 1
        /// </summary>
        /// <param name="canvas"></param>
        private void drawGraph(Canvas canvas, ArrayList list, float minimumY, float maximumY, Paint thepaint)
        {
            //delta-y
            float dY = maximumY - minimumY;

            //check if our list isnt empty
            if (list.Count > 0)
            {
                //loop through all values in our array
                for (int i = 0; i < list.Count - 1; i++)
                {
                    //get our y-axis value from the arraylist
                    GraphValue value0 = (GraphValue)list[i];
                    GraphValue value1 = (GraphValue)list[i + 1];
                    float yValue0 = value0.value;
                    float yValue1 = value1.value;

                    //draw the point on our graph
                    canvas.DrawLine(originX + (i * 5f),
                        originY - (yValue0 * ((float)200 / dY)),
                        originX + ((i + 1) * 5f),
                        originY - (yValue1 * ((float)200 / dY)),
                        thepaint);

                    ////show the min and max x-axis value's
                    //if (i == 0)
                    //    minX.Text = value0.time.ToString();
                    //if (i == list.Count - 2)
                    //    maxX.Text = value1.time.ToString();
                }//end for
            }//end if
        }//end method drawGraph
    }
}