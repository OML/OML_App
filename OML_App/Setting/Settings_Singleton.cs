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

namespace OML_App.Setting
{
    class Settings_Singleton
    {
        private static volatile Settings_Singleton instance;
        private static object syncRoot = new Object();


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
    }
}