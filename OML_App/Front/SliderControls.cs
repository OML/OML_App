using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using OML_App.Data;
using OML_App.Setting;

namespace OML_App
{
    public class SliderControls : View, View.IOnTouchListener
    {
        //initial slider position
        public const float INIT_X = 0;
	    public const float INIT_Y = 101;
	    
        //slider position within our view
	    public PointF _touchingPoint = new PointF(INIT_X, INIT_Y);
	
        //float to hold our power value
	    public float _power = 0;

        //float to save the coordinates of our last touch on the screen
        private float mLastTouchY;

        //int to save our active pointer
        private int mActivePointerId;

        //ints to save our textcolor values
        private int redvalue;
        private int greenvalue;


        private DateTime LastUpdate = DateTime.Now;
        TimeSpan interval = TimeSpan.FromMilliseconds(250);

        //textview to show our current power value
        TextView tv;

        public SliderControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }//end constructor

        public SliderControls(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }//end constructor

        private void Initialize()
        {
        }//end method Initialize

        /// <summary>
        /// Method to handle onTouch events
        /// </summary>
        /// <param name="v"></param>
        /// <param name="events"></param>
        /// <returns></returns>
        public bool OnTouch(View v, MotionEvent events) 
	    {
		    update(events);
            this.Invalidate();
		    return true;
	    }//end method OnTouch

        /// <summary>
        /// Method to update our sliders
        /// </summary>
        /// <param name="events"></param>
        public void update(MotionEvent events)
	    {
            switch (events.Action & events.ActionMasked)
            {
                case MotionEventActions.Down:
                    //save the ID of the active pointer
                    mActivePointerId = events.PointerCount - 1;

                    //remember where we started
                    mLastTouchY = events.GetY(mActivePointerId);
                    break;

                case MotionEventActions.Move:
                    //only get the vertical movement
                    float y = events.GetY(mActivePointerId);

                    //set touchingpoint
                    _touchingPoint.Y = (int)y;

                    //remember this touch point for the next move event
                    mLastTouchY = y;

                    //invalidate to request a redraw
                    Invalidate();
                    break;
                    
                case MotionEventActions.Up:
                    //reset our touching point on release
                    _touchingPoint = new PointF(INIT_X, INIT_Y);
                    mLastTouchY = 0;
                    _power = 0;
                    break;
            }//end switch

            //set the touching points to min/max when they go out of bounds
            if (_touchingPoint.Y < 13)
                _touchingPoint.Y = 13;

            if (_touchingPoint.Y > 189)
                _touchingPoint.Y = 189;
		
		    //determine the percentage of power forward/backward
            //-83 < 0 > 83
            _power = Convert.ToInt32((_touchingPoint.Y - 13) - 88);

            //make sure we can return a power value for the engine between 100 and - 100
            //100 being max power forwards, -100 max power reverse.
            if (_power < -1)
            {
                _power = Convert.ToInt32((Math.Abs(_power / 88)) * 100);
            }//end if
            else if (_power > 1)
            {
                _power = Convert.ToInt32((_power / 88) * 100);
                _power *= -1;
            }//end else if
            else
                _power = 0;

            if (_power > 100)
                _power = 100;

            if (_power < -100)
                _power = -100;

            //set the power value in our singleton class so we can send it to CARMEN
            //Make sure we dont update too often, so we dont lock the thread
            if (DateTime.Now - LastUpdate > interval)
            {
                LastUpdate = DateTime.Now;
                if (this.Id == Resource.Id.sliderControls0)
                    Send_Singleton.Instance.left = (int)_power;
                else if (this.Id == Resource.Id.sliderControls1)
                    Send_Singleton.Instance.right = (int)_power;
            }//end if
	    }//end method Update

        /// <summary>
        /// Method to draw our resources on the screen
        /// </summary>
        /// <param name="canvas"></param>
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            //draw the y with minus 13 to make it center.
            canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint.X, _touchingPoint.Y - 13, null);

            //determine the textcolor (green -> red) depending on the power value
            if (Math.Abs(_power) < 50)
            {
                redvalue = (int)(5.1f * Math.Abs(_power));
                greenvalue = 255;
            }//end if
            else
            {
                redvalue = 255;
                greenvalue = 255 - (int)(5.1f * Math.Abs(_power));
            }//end else

            //create the custom text color
            Color custom = Color.Argb(255, redvalue, greenvalue, 0);

            //check which slider we are controlling and act accordingly
            if (this.Id == Resource.Id.sliderControls0)
            {
                tv = ((RelativeLayout)this.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.powerView0);
                tv.Text = _power.ToString();
                tv.SetTextColor(custom);
            }//end if
            else if (this.Id == Resource.Id.sliderControls1)
            {
                tv = ((RelativeLayout)this.Parent.Parent.Parent).FindViewById<TextView>(Resource.Id.powerView1);
                tv.Text = _power.ToString();
                tv.SetTextColor(custom);
            }//end else if
        }//end method OnDraw
    }//end Class SliderControls
}//end namespace OML_App