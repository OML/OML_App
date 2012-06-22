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

namespace OML_App
{
    public class audioControls : View, View.IOnTouchListener
    {
        //initial slider position
        public const float INIT_X = 0;
        public const float INIT_Y = 13;

        //slider position within our view
        public PointF _touchingPoint = new PointF(INIT_X, INIT_Y);

        //value with which we divide our y-axis value, to get a respective value between 0-3.
        public float divider = 124;

        //float to save the coordinates of our last touch on the screen
        private float mLastTouchY;

        private DateTime LastUpdate = DateTime.Now;
        TimeSpan interval = TimeSpan.FromMilliseconds(250);

        public float colorValue;

        public audioControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }//end constructor

        public audioControls(Context context, IAttributeSet attrs, int defStyle) :
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
                    //remember where we started
                    mLastTouchY = events.GetY();
                    break;

                case MotionEventActions.Move:
                    //only get the vertical movement
                    float y = events.GetY();

                    //set touchingpoint
                    _touchingPoint.Y = (int)y;

                    //remember this touch point for the next move event
                    mLastTouchY = y;

                    //invalidate to request a redraw
                    Invalidate();
                    break;
            }//end switch

            //set the touching points to min/max when they go out of bounds
            if (_touchingPoint.Y < 13)
                _touchingPoint.Y = 13;

            if (_touchingPoint.Y > 357)
                _touchingPoint.Y = 357;

            //determine the colorvalue and divide it to get a integral value between 0 - 3
            colorValue = _touchingPoint.Y / divider;
            Math.Round(colorValue);

            //set the power value in our singleton class so we can send it to CARMEN
            //Make sure we dont update too often, so we dont lock the thread
            if (DateTime.Now - LastUpdate > interval)
            {
                LastUpdate = DateTime.Now;
                //Send_Singleton.Instance.
            }//end if
        }//end method Update

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            //draw the y with minus 13 to make it center.
            canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint.X, _touchingPoint.Y - 13, null);
        }
    }//end class audioControls
}//end namespace OML_App