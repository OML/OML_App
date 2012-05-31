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
        float volt0;
        float volt1;
        float amp0;
        float amp1;
        float temp0;
        float temp1;

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

            //get the updated textview values
            volt0 = 11.7f;
            volt1 = 8.3f;
            amp0 = 8.9f;
            amp1 = 4.3f;
            temp0 = 40.1f;
            temp1 = 42.4f;

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
        }
    }
}