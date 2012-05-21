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
    class SingletonSend
    {
        private static SingletonSend instance;

        private SingletonSend() { }

        public static SingletonSend Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new SingletonSend();
                }
                return instance;
            }
        }
    }
}