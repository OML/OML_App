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

namespace OML_App
{
    public class LedControls : View, View.IOnTouchListener
    {
        //float to store our (LED) colorvalue
        public static float colorValue { get; set; }

        //initial slider position
        public const float INIT_X = 0;
        public const float INIT_Y = 13;

        //slider position within our view
        public PointF _touchingPoint = new PointF(INIT_X, INIT_Y);

        //value with which we divide our y-axis value, to get a respective value between 0-7.
        public float divider = 50;

        //bit multiplier to ensure we send 3 bit values (0-2-4-6-8-10-12-14)
        int bitmultiplier = 2;

        //float to save the coordinates of our last touch on the screen
        private float mLastTouchY;

        //time and interval to update 5 times per minute (so we dont lock the singleton class)
        private DateTime LastUpdate = DateTime.Now;
        TimeSpan interval = TimeSpan.FromMilliseconds(Settings_Singleton.Instance.Controller_UpdateRate);

        public LedControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
            this.SetOnTouchListener(this);
        }//end constructor

        public LedControls(Context context, IAttributeSet attrs, int defStyle) :
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
            //if were not in discomode, we can move the slider
            if (!Controller.discoInferno)
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

                //check the bounds of our touching point
                checkBounds();

                //determine the colorvalue and divide it to get a integral value between 0 - 7
                colorValue = (int)Math.Round(_touchingPoint.Y / divider);

                //snap the slider and re-check the bounds
                _touchingPoint.Y = colorValue * divider;
                checkBounds();

                //multiply the color value to get an even value between 0 - 14
                colorValue *= bitmultiplier;
            }//end if

            //set the power value in our singleton class so we can send it to CARMEN
            //Make sure we dont update too often, so we dont lock the thread
            if (DateTime.Now - LastUpdate > interval)
            {
                LastUpdate = DateTime.Now;
                Send_Singleton.Instance.releaseRing = (int)colorValue;
            }//end if
        }//end method Update

        /// <summary>
        /// Method to check the bounds of our touching point on the y-axis
        /// </summary>
        public void checkBounds()
        {
            //set the touching points to min/max when they go out of bounds
            if (_touchingPoint.Y < 25)
                _touchingPoint.Y = 13;

            if (_touchingPoint.Y > 349)
                _touchingPoint.Y = 357;
        }//end method checkBounds

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            //draw the y with minus 13 to make it center.
            canvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.slidersmall), _touchingPoint.X, _touchingPoint.Y - 13, null);
        }//end method OnDraw
    }//end class audioControls
}//end namespace OML_App