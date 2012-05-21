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
    public class GameControls : View, View.IOnTouchListener
    {
        public static int INIT_X = 60;
        public static int INIT_Y = 50;
        public static float MAX_RADIUS = 50;
        public Point _touchingPoint = new Point(60, 50);
        private bool _dragging = false;
	
        //pythagoreon theorem
        public float _a = 0;
        public float _b = 0;
        public float _c = 0;
        public float _angle = 0;
        private MotionEvent lastEvent;

        public GameControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }

        public GameControls(Context context, IAttributeSet attrs, int defStyle) :
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
		    Update(events);
		    this.Invalidate();
		    return true;
	    }

        public void Update(MotionEvent events)
	    {
		    if (events == null && lastEvent == null)
			    return;
		
		    else if(events == null && lastEvent != null)
			    events = lastEvent;
		
		    else
			    lastEvent = events;
		
		    //drag drop 
            if (events.Action == MotionEventActions.Down)
			    _dragging = true;

            else if (events.Action == MotionEventActions.Up)
		    {			
			    // Snap back to center when the joystick is released
			    _touchingPoint.X = (int) INIT_X;
			    _touchingPoint.X = (int) INIT_Y;
			
			    _dragging = false;
		    }
		
		    if ( _dragging )
		    {
			    // get the position
			    _touchingPoint.X = (int)events.GetX();
			    _touchingPoint.Y = (int)events.GetY();
			
			    _a = _touchingPoint.X - INIT_X;
			    _b = _touchingPoint.Y - INIT_Y;
			    _c = (float)Math.Sqrt(Math.Pow(_a, 2) + Math.Pow(_b, 2));
			
			    if (_c > MAX_RADIUS)
				    _touchingPoint = new Point((int)(((MAX_RADIUS/_c) * _a + INIT_X)), ((int)((MAX_RADIUS/_c) * _b + INIT_Y)));

			    //get the angle
			    //double angle = Math.atan2(_touchingPoint.y - INIT_Y,_touchingPoint.x - INIT_X)/(Math.PI/180);
		    }
	    }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (!this.IsInEditMode)
            {
                //draw the dragable joystick
                canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.joystick), _touchingPoint.X, _touchingPoint.Y, null);
            }
            //for debugging in edit mode
            else
            {
                Paint innerCirclePaint = new Paint();
                innerCirclePaint.SetARGB(255, 255, 255, 255);
                innerCirclePaint.AntiAlias = true;

                innerCirclePaint.SetStyle(Paint.Style.Fill);
                canvas.DrawLine(0, 0, 60, 50, innerCirclePaint);
            }
        }
    }
}