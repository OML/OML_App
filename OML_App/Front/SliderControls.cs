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

namespace OML_App
{
    public class SliderControls : View, View.IOnTouchListener
    {
        public static float INIT_X = 0;
	    public static float INIT_Y = 101;
	
	    public PointF _touchingPoint = new PointF(INIT_X, INIT_Y);
	
	    public float _power = 0;

        private float mLastTouchY;
        private int mActivePointerId;
        int redvalue;
        int greenvalue;

        TextView tv;

        public SliderControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }

        public SliderControls(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }

        private void Initialize()
        {
        }

        public bool OnTouch(View v, MotionEvent events) 
	    {
		    update(events);
            this.Invalidate();
		    return true;
	    }

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
                    break;
            }

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
            }
            else if (_power > 1)
            {
                _power = Convert.ToInt32((_power / 88) * 100);
                _power *= -1;
            }
            else
                _power = 0;

            if (_power > 100)
                _power = 100;

            if (_power < -100)
                _power = -100;

            if (this.Id == Resource.Id.sliderControls0)
                Send_Singleton.Instance.left = (int)_power;
            else if (this.Id == Resource.Id.sliderControls1)
                Send_Singleton.Instance.right = (int)_power;
	    }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (!this.IsInEditMode)
            {
                //draw the y with minus 13 to make it center.
                canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint.X, _touchingPoint.Y - 13, null);

                //determine the textcolor (green -> red)
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

                Color custom = Color.Argb(255, redvalue, greenvalue, 0);

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
            }//end if
            //for debugging purposes
            else
            {
                Paint innerCirclePaint = new Paint();
                innerCirclePaint.SetARGB(255, 255, 255, 255);
                innerCirclePaint.AntiAlias = true;

                innerCirclePaint.SetStyle(Paint.Style.Fill);
                canvas.DrawLine(21, 0, 21, 205, innerCirclePaint);
            }
        }
    }
}