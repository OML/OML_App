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

namespace OML_App.Data
{
    public class ValueData
    {
        public float Value;
        public DateTime Time;

        public ValueData() { }

        public ValueData(float value)
        {
            this.Value = value;
            this.Time = DateTime.Now;
        }

        public ValueData(float value, DateTime time)
        {
            this.Value = value;
            this.Time = time;
        }

        public static explicit operator ValueData(net.ukct.reintjan1.ValueData valueData)
        {
            ValueData vd = new ValueData();
            vd.Value = valueData.Value;
            vd.Time = valueData.Time;
            return vd;
        }

        public static explicit operator net.ukct.reintjan1.ValueData(ValueData valueData)
        {
            net.ukct.reintjan1.ValueData vd = new net.ukct.reintjan1.ValueData();
            vd.Value = valueData.Value;
            vd.Time = valueData.Time;
            return vd;
        }

    }
}