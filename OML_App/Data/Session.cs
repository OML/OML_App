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
    public class Session
    {
        public string Vehicle_Name;
        public List<Sensor> Sensors;
        public DateTime StartTime;
        public DateTime EndTime;

        //Create a Live Session!
        public Session(string vehicle_Name, List<Sensor> sensors)
        {
            this.Vehicle_Name = vehicle_Name;
            this.Sensors = sensors;
            this.StartTime = DateTime.Now;
        }

        //Create a Recorded Session
        public Session(string vehicle_Name, List<Sensor> sensors, DateTime start, DateTime end)
        {
            this.Vehicle_Name = vehicle_Name;
            this.Sensors = sensors;
            this.StartTime = start;
            this.EndTime = end;
        }

    }
}