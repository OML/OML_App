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
using Android.Webkit;
using System.Threading;

namespace OML_App
{
    class CamThread
    {
        //webview for our stream
        WebView wView;

        //path to our stream
        string path;

        //bool to check wether our camera is enabled
        bool enabled;

        /// <summary>
        /// CamThread constructor
        /// </summary>
        /// <param name="wView">the webview</param>
        /// <param name="URL">the path to the stream</param>
        public CamThread(WebView wView, string URL)
        {
            //set properties
            this.wView = wView;
            this.path = URL;
        }//end constructor

        /// <summary>
        /// Method to set our webview
        /// </summary>
        public void startCamera()
        {
            if (!enabled)
            {
                //get the settings from webview
                WebSettings wSettings = wView.Settings;

                //enable javascript
                wSettings.JavaScriptEnabled = true;

                //load the url
                wView.LoadUrl(path);
            }//end if

            wView.Reload();

            //the camera is running
            enabled = true;
        }//end method setCamera

        /// <summary>
        /// Method to stop our webview from loading
        /// </summary>
        public void stopCamera()
        {
            //check wether we arent stopped already
            if (enabled)
            {
                wView.StopLoading();
                enabled = false;
            }//end if
        }//end method stopCamera
    }//end class CamThread
}//end namespace OML_App