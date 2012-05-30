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
        public int Thershold;//Thershold Rate TCP
        public int UpdateRate;//Update Rate TCP
        public int IpAdress; //Default Ip , CurrentSession
        public int Port; //Default port, CurrentSession
        public bool Carmen = false; //True if Besturen eigen auto
        public int Camera_Port; //Port from Camera

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