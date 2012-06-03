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
using OML_App.Data;
using OML_App.net.ukct.reintjan1;

namespace OML_App.Setting
{
    class Settings_Singleton
    {
        private static volatile Settings_Singleton instance;
        private static object syncRoot = new Object();

        public List<OML_App.Data.Session> _Session = new List<OML_App.Data.Session>();
        public int TCP_Thershold = 2000;//Thershold Rate TCP
        public int TCP_UpdateRate = 5;//Update Rate TCP
        public int Controller_UpdateRate = 250; //SleepTime Controller
        public bool LiveSession = false;
        public string IpAdress = "192.168.1.101"; //Default Ip , CurrentSession
        public string Port = "1337"; //Default port, CurrentSession
        public bool ControllingCarmen = true; //True if Besturen eigen auto
        public int Camera_Port; //Port from Camera

        //** -- Sensor Values
            //4x motor Voltages            
            public int M0V = 0;
            public int M1V = 1;
            public int M2V = 2;
            public int M3V = 3;

            //4x motor Current
            public int M0A = 4;
            public int M1A = 5;
            public int M2A = 6;
            public int M3A = 7;

            //4x motor Temperature
            public int M0T = 8;
            public int M1T = 9;
            public int M2T = 10;
            public int M3T = 11;

            //4x motor Throttle
            public int M0Th = 12;
            public int M1Th = 13;
            public int M2Th = 14;
            public int M3Th = 15;
            
            //Accu values 0
            public int A0V = 16;
            public int A0A = 17;
            public int A0T = 18;

            //Accu values 1 wont be used at first hand (no support from hardware)
            public int A1V = 19;
            public int A1A = 20;
            public int A1T = 21;

            //Gyro
            public int G0X = 22;
            public int G0Y = 23;
            public int G0Z = 24;

        //--** End Sensors

        public WebService Ws = new WebService();

        public static Settings_Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Settings_Singleton();                            
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Main Methode to Close the Current Session!
        /// </summary>
        public void CloseCurrentSession()
        {
            LiveSession = false;
            //End the Recieve Session
            Receive_Singleton.Instance.EndSession();
            //Log the Recieve Session on the webserver !! Can be Laggy !!
            //SessionToWeb(Receive_Singleton.Instance.Current_ses);
        }

        /// <summary>
        /// Sends a Session to the Webservice! It will be saved over there
        /// </summary>
        /// <param name="ses"></param>
        public void SessionToWeb(OML_App.Data.Session ses) 
        {
            Ws.SaveNewSession((OML_App.net.ukct.reintjan1.Session)ses);
        }

        /// <summary>
        /// Get a #sessions in a List of Sessions from the WebService! Keep in mind there will be L@gg!!
        /// </summary>
        /// <param name="number">#numbers</param>
        public void GetList(int number)
        {
            OML_App.net.ukct.reintjan1.Session[] _ses = Ws.GetSessionList(number).ToArray<OML_App.net.ukct.reintjan1.Session>();
            foreach (OML_App.net.ukct.reintjan1.Session ses in _ses)
            {
                _Session.Add((OML_App.Data.Session)ses);
            }
        }

        /// <summary>
        /// Get a List of Sessions of all the Sessions from the WebService! Keep in mind there will be L@gg!!
        /// </summary>
        public void GetAllList()
        {
            OML_App.net.ukct.reintjan1.Session[] _ses = Ws.GetAllSession().ToArray<OML_App.net.ukct.reintjan1.Session>();
            foreach (OML_App.net.ukct.reintjan1.Session ses in _ses)
            {
                _Session.Add((OML_App.Data.Session)ses);
            }
        }
    }
}