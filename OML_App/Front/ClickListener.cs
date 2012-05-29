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

namespace OML_App.Front
{
    public class ClickListener : View, IDialogInterfaceOnClickListener
    {
        public ClickListener(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {

        }

        public ClickListener(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {

        }
    }
}