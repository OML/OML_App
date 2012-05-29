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

        public static explicit operator Session(net.ukct.reintjan1.Session session)
        {
            Session ses = new Session();
            foreach (net.ukct.reintjan1.Sensor sensor in session.Sensors)
            {
                ses.AddSensorToArray((Sensor)sensor);
            }
            ses.EndTime = session.EndTime;
            ses.StartTime = session.StartTime;
            ses.Vehicle_Name = session.Vehicle_Name;
            return ses;
        }

        public static explicit operator net.ukct.reintjan1.Session(Session session)
        {
            net.ukct.reintjan1.Session ses = new net.ukct.reintjan1.Session();

            ArrayList sensors = new ArrayList();
            foreach (Sensor sensor in session.Sensors)
            {
                sensors.Add((net.ukct.reintjan1.Sensor)sensor);
            }

            ses.Sensors = (net.ukct.reintjan1.Sensor[])sensors.ToArray(typeof(net.ukct.reintjan1.Sensor[]));
            ses.EndTime = session.EndTime;
            ses.StartTime = session.StartTime;
            ses.Vehicle_Name = session.Vehicle_Name;
            return ses;
        }

    }
}