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

namespace OML_App
{
    public class SliderControls : View, View.IOnTouchListener
    {
        public static float INIT_X = 0;
	    public static float INIT_Y = 101;
	
	    public PointF _touchingPoint = new PointF(INIT_X, INIT_Y);
	
	    public int _power = 0;

        private float mLastTouchY;
        private int mActivePointerId;
        private int activePointers;

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
            if (_power < -2)
            {
                _power = Convert.ToInt32((Math.Abs(_power / 88)) * 100);
            }
            else if (_power > 2)
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
	    }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (!this.IsInEditMode)
            {
                //draw the y with minus 13 to make it center.
                canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint.X, _touchingPoint.Y - 13, null);
            }
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