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

namespace OML_App.Connection.Viewer
{
    class ViewerData
    {
        public byte[] Message;   //Message text
        public Command OPCommand;  //OP type (login, logout, Send Data)

        //Default constructor
        public ViewerData()
        {
            this.OPCommand = Command.Null;
            this.Message = null;
        }
        //Converts the bytes into an object of type Data
        public ViewerData(byte[] data)
        {
             //The first four bytes are for the Command
             this.OPCommand = (Command)BitConverter.ToInt32(data, 0);
             this.Message = data;          
        }

        //Converts the Data structure into an array of bytes
        public byte[] ToByte()
        {
            List<byte> result = new List<byte>();

            //First four are for the Command           
            result.AddRange(BitConverter.GetBytes((int)OPCommand));            

            //Length of the message
            if (Message != null)
                result.AddRange(Message); 
            return result.ToArray();
        }
    }
}