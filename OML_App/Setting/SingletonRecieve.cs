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
    class SingletonRecieve
    {
        private static SingletonRecieve instance;

        private SingletonRecieve() { }

        public static SingletonRecieve Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new SingletonRecieve();
                }
                return instance;
            }
        }
    }
}