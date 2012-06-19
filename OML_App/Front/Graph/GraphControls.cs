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
using Android.Graphics.Drawables;

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

        //textviews representing our x and y values on the axis
        TextView minX;
        TextView maxX;
        TextView minY;
        TextView maxY;

        //integers to hold our max and min Y-axis values
        float minimumY = 0;
        float maximumY = 0;

        //values for our textviews
        float volt0 = 0.1f;
        float volt1 = 0;
        float amp0 = 0;
        float amp1 = 0;
        float temp0 = 0;
        float temp1 = 0;
        TimeSpan time;

        DateTime start = DateTime.Now;

        const int originX = 50;
        const int originY = 290;

        //index for our switch in the draw method
        int switchIndex;

        //paint to draw with
        Paint paint = new Paint();

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

            //set the paint to black
            paint.SetARGB(255,0,0,0);
            
            //get the textviews so we can set the text in the graph draw
            minX = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.minX);
            maxX = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.maxX);
            minY = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.minY);
            maxY = ((RelativeLayout)this.Parent).FindViewById<TextView>(Resource.Id.maxY);

            //set the time
            time = DateTime.Now - start;// Receive_Singleton.Instance.Current_ses.StartTime;

            ////set the updated textview values
            //if (volt0 != 0)//Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value)
            //{
            //    volt0 += 0.01f;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length].Value;
            //    voltvalue0.Add(new GraphValue(volt0, time));

            //    //set the textviews 9didnt work in the initialize method...)
            //    v0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.volttxt0);

            //    //set the values in the textviews, showing them on the screen
            //    v0.Text = volt0.ToString();

            //    //get the min and max Y
            //    minimumY = 0;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Min;
            //    maximumY = 6;// Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Max;

            //    ////show the min and max Y on the graph
            //    //minY.Text = minimumY.ToString();
            //    //maxY.Text = maximumY.ToString();

            //    //if we exceed 100 elements remove the first
            //    if (voltvalue0.Count > 100)
            //        voltvalue0.RemoveAt(0);
            //}//end if

            //set the updated textview values
            if (volt0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length - 1].Value)
            {
                volt0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Values.Length - 1].Value;
                voltvalue0.Add(new GraphValue(volt0, time));

                //set the textviews 9didnt work in the initialize method...)
                v0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.volttxt0);

                //set the values in the textviews, showing them on the screen
                v0.Text = volt0.ToString();

                //get the min and max Y
                minimumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Min;
                maximumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0V].Max;

                //show the min and max Y on the graph
                minY.Text = minimumY.ToString();
                maxY.Text = maximumY.ToString();

                //if we exceed 100 elements remove the first
                if (voltvalue0.Count > 100)
                    voltvalue0.RemoveAt(0);
            }//end if

            if (volt1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length - 1].Value)
            {
                volt1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Values.Length - 1].Value;
                voltvalue1.Add(new GraphValue(volt1, time));

                v1 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.volttxt1);
                v1.Text = volt1.ToString();

                minimumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Min;
                maximumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1V].Max;

                minY.Text = minimumY.ToString();
                maxY.Text = maximumY.ToString();

                if (voltvalue1.Count > 100)
                    voltvalue1.RemoveAt(0);
            }//end if

            if (amp0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length - 1].Value)
            {
                amp0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Values.Length - 1].Value;
                ampvalue0.Add(new GraphValue(amp0, time));

                a0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.amptxt0);
                a0.Text = amp0.ToString();

                minimumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Min;
                maximumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0A].Max;

                minY.Text = minimumY.ToString();
                maxY.Text = maximumY.ToString();

                if (ampvalue0.Count > 100)
                    ampvalue0.RemoveAt(0);
            }//end if

            if (amp1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length - 1].Value)
            {
                amp1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Values.Length - 1].Value;
                ampvalue1.Add(new GraphValue(amp1, time));

                a1 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.amptxt1);
                a1.Text = amp1.ToString();

                minimumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Min;
                maximumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1A].Max;

                minY.Text = minimumY.ToString();
                maxY.Text = maximumY.ToString();

                if (ampvalue1.Count > 100)
                    ampvalue1.RemoveAt(0);
            }//end if

            if (temp0 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length - 1].Value)
            {
                temp0 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Values.Length - 1].Value;
                tempvalue0.Add(new GraphValue(temp0, time));

                t0 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.temptxt0);
                t0.Text = temp0.ToString();

                minimumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Min;
                maximumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A0T].Max;

                minY.Text = minimumY.ToString();
                maxY.Text = maximumY.ToString();

                if (tempvalue0.Count > 100)
                    tempvalue0.RemoveAt(0);
            }//end if

            if (temp1 != Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length - 1].Value)
            {
                temp1 = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Values.Length - 1].Value;
                tempvalue1.Add(new GraphValue(temp1, time));

                t1 = ((RelativeLayout)this.Parent.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.temptxt1);
                t1.Text = temp1.ToString();

                minimumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Min;
                maximumY = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.A1T].Max;

                minY.Text = minimumY.ToString();
                maxY.Text = maximumY.ToString();

                if (tempvalue1.Count > 100)
                    tempvalue1.RemoveAt(0);
            }//end if

            if (Activity1.controller)
                switchIndex = Controller.activeIndex;
            else
                switchIndex = Spectate.activeIndex;

            //switch the activeIndex to see which graph to draw
            switch (switchIndex)
            {
                //draw the voltage graph for battery 1
                case 1:
                    drawGraph(canvas, voltvalue0, minimumY, maximumY);
                    break;
                //draw the voltage graph for battery 2
                case 2:
                    drawGraph(canvas, voltvalue1, minimumY, maximumY);
                    break;
                //draw the ampere graph for battery 1
                case 3:
                    drawGraph(canvas, ampvalue0, minimumY, maximumY);
                    break;
                //draw the ampere graph for battery 2
                case 4:
                    drawGraph(canvas, ampvalue1, minimumY, maximumY);
                    break;
                //draw the temperature graph for battery 1
                case 5:
                    drawGraph(canvas, tempvalue0, minimumY, maximumY);
                    break;
                //draw the temperature graph for battery 2
                case 6:
                    drawGraph(canvas, tempvalue1, minimumY, maximumY);
                    break;
                //if were not drawing anything, "clear" the screen
                default:
                    ((RelativeLayout)this.Parent).SetBackgroundResource(Resource.Drawable.graphview);
                    minX.Text = "";
                    maxX.Text = "";
                    minY.Text = "";
                    maxY.Text = "";
                    break;
            }//end switch

            //invalidate to request a redraw
            Invalidate();
        }//end overrided method OnDraw

        /// <summary>
        /// method to draw the graphs
        /// </summary>
        /// <param name="canvas"></param>
        private void drawGraph(Canvas canvas, ArrayList list, float minimumY, float maximumY)
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
                    canvas.DrawLine(
                        originX + (i * 5.5f), 
                        originY - (yValue0 * ((float)250/dY)), 
                        originX + ((i + 1) * 5.5f), 
                        originY - (yValue1 * ((float)250/dY)), 
                        paint);

                    //show the min and max x-axis value's
                    if (i == 0)
                        minX.Text = value0.time.Seconds.ToString();
                    if (i == list.Count - 2)
                        maxX.Text = value1.time.Seconds.ToString();
                }//end for
            }//end if
        }//end method drawGraph
    }//end class GraphControls
}//end namespace OML_App