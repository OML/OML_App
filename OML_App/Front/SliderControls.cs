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
        public const float INIT_X = 0;
	    public const float INIT_Y = 101;
	
	    public PointF _touchingPoint0 = new PointF(INIT_X, INIT_Y);
        public PointF _touchingPoint1 = new PointF(INIT_X, INIT_Y);
	
	    public int _power0 = 0;
        public int _power1 = 0;

        private float mLastTouchY0;
        private float mLastTouchY1;
        private int mActivePointerId;
        private int activePointers;
        private RelativeLayout rLayout0;
        private RelativeLayout rLayout1;

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
            rLayout0 = (RelativeLayout)FindViewById<RelativeLayout>(Resource.Id.smallSlider0);
            rLayout1 = (RelativeLayout)FindViewById<RelativeLayout>(Resource.Id.smallSlider1);
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
                    if ((RelativeLayout)this.Parent == rLayout0)
                    {
                        //remember where we started
                        mLastTouchY0 = events.GetY();
                    }//end if
                    else if ((RelativeLayout)this.Parent == rLayout1)
                    {
                        //remember where we started
                        mLastTouchY1 = events.GetY();
                    }//end else if
                    break;

                case MotionEventActions.Move:
                    if ((RelativeLayout)this.Parent == rLayout0)
                    {
                        //only get the vertical movement
                        float y = events.GetY();

                        //set touchingpoint
                        _touchingPoint0.Y = (int)y;

                        //remember this touch point for the next move event
                        mLastTouchY0 = y;

                        //invalidate to request a redraw
                        Invalidate();
                    }//end if
                    else if ((RelativeLayout)this.Parent == rLayout1)
                    {
                        //only get the vertical movement
                        float y = events.GetY();

                        //set touchingpoint
                        _touchingPoint1.Y = (int)y;

                        //remember this touch point for the next move event
                        mLastTouchY1 = y;

                        //invalidate to request a redraw
                        Invalidate();
                    }//end else if
                    break;

                case MotionEventActions.Up:
                    if ((RelativeLayout)this.Parent == rLayout0)
                    {
                        //reset our touching point on release
                        _touchingPoint0 = new PointF(INIT_X, INIT_Y);
                    }//end if
                    else if ((RelativeLayout)this.Parent == rLayout1)
                    {
                        //reset our touching point on release
                        _touchingPoint1 = new PointF(INIT_X, INIT_Y);
                    }//end else if
                    break;
            }//end switch

            if ((RelativeLayout)this.Parent == rLayout0)
            {
                if (_touchingPoint0.Y < 13)
                    _touchingPoint0.Y = 13;

                if (_touchingPoint0.Y > 189)
                    _touchingPoint0.Y = 189;

                //determine the percentage of power forward/backward from slider0
                //-83 < 0 > 83
                _power0 = Convert.ToInt32((_touchingPoint0.Y - 13) - 88);

                //make sure we can return a power value for the engine between 100 and - 100
                //100 being max power forwards, -100 max power reverse. 
                //prevent the slider from getting too sensitive by setting everything between -2 and 2 to 0 power.
                if (_power0 < -2)
                {
                    _power0 = Convert.ToInt32((Math.Abs(_power0 / 88)) * 100);
                }
                else if (_power0 > 2)
                {
                    _power0 = Convert.ToInt32((_power0 / 88) * 100);
                    _power0 *= -1;
                }
                else
                    _power0 = 0;

                if (_power0 > 100)
                    _power0 = 100;

                if (_power0 < -100)
                    _power0 = -100;
            }

            else if((RelativeLayout)this.Parent == rLayout1)
            {
                if (_touchingPoint1.Y < 13)
                    _touchingPoint1.Y = 13;

                if (_touchingPoint1.Y > 189)
                    _touchingPoint1.Y = 189;

                //determine the percentage of power forward/backward from slider1
                //-83 < 0 > 83
                _power1 = Convert.ToInt32((_touchingPoint1.Y - 13) - 88);

                //make sure we can return a power value for the engine between 100 and - 100
                //100 being max power forwards, -100 max power reverse. 
                //prevent the slider from getting too sensitive by setting everything between -2 and 2 to 0 power.
                if (_power1 < -2)
                {
                    _power1 = Convert.ToInt32((Math.Abs(_power1 / 88)) * 100);
                }
                else if (_power1 > 2)
                {
                    _power1 = Convert.ToInt32((_power1 / 88) * 100);
                    _power1 *= -1;
                }
                else
                    _power1 = 0;

                if (_power1 > 100)
                    _power1 = 100;

                if (_power1 < -100)
                    _power1 = -100;
            }
	    }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (!this.IsInEditMode)
            {
                if ((RelativeLayout)this.Parent == rLayout0)
                    //draw the y with minus 13 to make it center.
                    canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint0.X, _touchingPoint0.Y - 13, null);

                else if ((RelativeLayout)this.Parent == rLayout1)
                    canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint1.X, _touchingPoint1.Y - 13, null);
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