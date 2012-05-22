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
        public static int INIT_X = 0;
	    public static int INIT_Y = 89;
	
	    public Point _touchingPoint = new Point(INIT_X, INIT_Y);
	
	    private Boolean _dragging = false;
	    private MotionEvent lastEvent;
	
	    public int _power = 0;

        private float mLastTouchX;
        private float mLastTouchY;
        private int mActivePointerId;

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
                    //remember where we started
                    mLastTouchX = events.GetX();
                    mLastTouchY = events.GetY();

                    //save the ID of this pointer
                    mActivePointerId = events.GetPointerId(0);
                    break;

                case MotionEventActions.Move:
                    //find the index of the activepointer and fetch its position
                    int pointerIndex = events.FindPointerIndex(mActivePointerId);

                    float x = events.GetX(pointerIndex);
                    float y = events.GetY(pointerIndex);

                    //calc distance moved
                    float dx = x - mLastTouchX;
                    float dy = y - mLastTouchY;

                    //move object
                    _touchingPoint.X += (int)dx;
                    _touchingPoint.Y += (int)dy;

                    //remember this touch point for the next move event
                    mLastTouchX = x;
                    mLastTouchY = y;

                    //invalidate to request a redraw
                    Invalidate();
                    break;

                case MotionEventActions.Up:
                    //reset our touching point on release
                    _touchingPoint = new Point(0, 89);
                    break;

                case MotionEventActions.Cancel:
                    _touchingPoint = new Point(0, 89);
                    break;
            }
		
		    //determine the percentage of power forward/backward
            //-89.5 > 0 < +89.5
            _power = Convert.ToInt32(_touchingPoint.Y - 89.5f);

            //make sure we can return a power value for the engine between 100 and - 100
            //100 being max power forwards, -100 max power reverse.
            if (_power < 0)
            {
                _power = Convert.ToInt32((Math.Abs(_power / 89.5f)) * 100);
            }
            else if (_power > 0)
            {
                _power = Convert.ToInt32((_power / 89.5f) * 100);
                _power *= -1;
            }
	    }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (!this.IsInEditMode)
            {
                //if (((View)this.GetParent()).getVisibility() == VISIBLE && ((View)this.getParent()).getId() != id.bigSlider)
                //if (((View)this.GetParent()).getVisibility() == VISIBLE && ((View)this.getParent()).getId() != id.bigSlider)
                //{
                //draw the dragable slider(s)
                //canvas.DrawBitmap(BitmapFactory.DecodeResource(Resource.Drawable.slidersmall, _touchingPoint.X, _touchingPoint.Y));            
                canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint.X, _touchingPoint.Y, null);
                //}
                //else
                //{
                //    //draw the dragable slider
                //    canvas.DrawBitmap(BitmapFactory.DecodeResource(Resource.Drawable.sliderbig, _touchingPoint.X, _touchingPoint.Y));
                //}
                //for debugging in edit mode
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