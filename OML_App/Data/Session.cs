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
    public class Session
    {
        public string Vehicle_Name;
        public Sensor[] Sensors;
        public DateTime StartTime;
        public DateTime EndTime;

        public Session() { }

        //Create a Live Session!
        public Session(string vehicle_Name)
        {
            this.Vehicle_Name = vehicle_Name;
            this.StartTime = DateTime.Now;
        }

        //Create a Live Session!
        public Session(string vehicle_Name, Sensor[] sensors)
        {
            this.Vehicle_Name = vehicle_Name;
            this.Sensors = sensors;
            this.StartTime = DateTime.Now;
        }

        //Create a Recorded Session
        public Session(string vehicle_Name, Sensor[] sensors, DateTime start, DateTime end)
        {
            this.Vehicle_Name = vehicle_Name;
            this.Sensors = sensors;
            this.StartTime = start;
            this.EndTime = end;
        }

        public void AddSensorToArray(Sensor s)
        {
            int newLength = 0;
            Sensor[] newList;
            if (Sensors != null)
            {
                newLength = Sensors.Length;
                newList = new Sensor[newLength + 1];
                Sensors.CopyTo(newList, 0);
            }
            else
            {
                newList = new Sensor[1];
            }
            newList[newList.GetUpperBound(0)] = s;
            Sensors = newList;
        }

        

    }
}