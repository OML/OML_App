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

namespace OML_App.Front
{
    public class GraphValue
    {
        //list values
        float value;
        TimeSpan time;

        public GraphValue(float value, TimeSpan time)
        {
            this.value = value;
            this.time = time;
        }//end constructor
    }//end class GraphValue
}//end namespace OML_App.Front