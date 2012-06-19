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
using Android.Graphics.Drawables;
using OML_App.Data;
using OML_App.Setting;

namespace OML_App
{
    class RollControls : View
    {
        //value between 0 and 360;
        private float angle = 0f;
        private float xOffCenter = -35;
        private float yOffCenter = -35;
        Bitmap original;
        Bitmap bm;
        Matrix m = new Matrix();
        Matrix n = new Matrix();
        private bool init = true;

        public RollControls(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }//end constructor

        public RollControls(Context context, IAttributeSet attrs, int defStyle) :
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
                m.SetTranslate(xOffCenter, yOffCenter);
                init = false;
            }//end if

            angle = Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.G0X].Values[Receive_Singleton.Instance.Current_ses.Sensors[Settings_Singleton.Instance.G0X].Values.Length - 1].Value;

            m.PreRotate(angle, 120, 120);

            canvas.DrawBitmap(bm, m, null);
            Invalidate();
        }//end overrided method OnDraw
    }//end class RollControls
}//end namespace OML_App