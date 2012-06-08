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
using System.Collections;

namespace OML_App.Data
{
    public class Sensor
    {
        public string Name; //Name from sensor
        public string NameShort; //Short Name 3 char max
        public string Unity; //Unity from Values Eg. CM , M , MM, V, A
        public float Min;//Min value Set on start!
        public float Max;//Max value Set on start!

        public ValueData[] Values;

        public Sensor() { }

        public Sensor(string name, string nameshort, string unity, float min, float max)
        {
            this.Name = name; //Assign name
            this.NameShort = nameshort; //Assign short name
            this.Unity = unity; //Set unity
            this.Min = min; //Set min
            this.Max = max; //Set max
            //Add first Value
            Values = new ValueData[1];
            Values[0] = new ValueData((min+max)/ 2);
        }

        public void AddValueDataToArray(ValueData valdata)
        {
            int newLength = 0;
            ValueData[] newList;
            if (Values != null)
            {
                newLength = Values.Length;
                newList = new ValueData[newLength + 1];
                Values.CopyTo(newList, 0);
            }
            else
            {
                newList = new ValueData[1];
            }
            newList[newList.GetUpperBound(0)] = valdata;
            Values = newList;

        }       
        
    }
}