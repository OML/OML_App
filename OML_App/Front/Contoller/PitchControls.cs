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
    class PitchControls : View
    {
        //value between -100 and 100, default 0 (vertical level)
        private float yDiff = 0;
        private float xOffCenter = -35;
        private float yOffCenter = -35;
        Bitmap original;
        Bitmap bm;
        Matrix m = new Matrix();
        Matrix n = new Matrix();
        private bool init = true;

        //value between -100 and 100, 0 is center (vertical)
        private float increment = 0f;

        public PitchControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }//end constructor

        public PitchControls(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }//end constructor

        private void Initialize()
        {
        }//end method Initialize

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
            //if this is the 1st call of draw
            if (init)
            {
                original = BitmapFactory.DecodeResource(Resources, Resource.Drawable.pitchbg0);
                bm = Bitmap.CreateBitmap(original, 0, 0, original.Width, original.Height, n, true);
                init = false;
            }//end if

            yDiff += 0.1f;

            m.SetTranslate(xOffCenter, yOffCenter + yDiff);

            canvas.DrawBitmap(bm, m, null);
            Invalidate();
        }//end overrided method OnDraw
    }//end class PitchControls
}//end namespace OML_App