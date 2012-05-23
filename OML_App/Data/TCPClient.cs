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
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using OML_App.Data;

namespace OML_App.Connection
{
    
  
    /// <summary>
    /// never used
    /// </summary>
    enum Command
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        Message,    //Send a text message to all the chat clients
        List,       //Get a list of users in the chat room from the server
        Null        //No command
    }

    /// <summary>
    /// setting up client en connection to server
    /// </summary>
    public class TCPClient 
    {
        #region Variables
        Activity1 MainAct;
        public Socket clientSocket;
        Data Liefdes_brief = new Data();
        string Ip_Adress;
        private byte[] byteData = new byte[1024];
        int Port;
        #endregion
      
        #region Setting_Up_Client
        public TCPClient(string ip_adress, int port)
        {
            this.Ip_Adress = ip_adress;
            this.Port = port;
            //this.MainAct = Act;
            Connect();
            Thread newThread = new Thread(new ThreadStart(Run));
            newThread.Start(); 
        }

        
        #endregion

        public void Connect()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(Ip_Adress);
                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);
                //Connect to the server
                clientSocket.Connect(ipEndPoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);                
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ OnSend:" + ex.Message);
            }
        }

        private void Send()
        {
            try
            {
                //Singleton.Instance.Y = 4;
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                //bf.Serialize(ms, Liefdes_brief.SetPacket(2, 8, 9, 8, 0, 10, 0));
                byte[] henk = ms.ToArray();
                //clientSocket.EndConnect(ar);
                clientSocket.BeginSend(henk,0 ,henk.Length, SocketFlags.None, null, null );
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ OnConnect:" + ex.Message);
            }
        }

        private void Receive()
        {
            try
            {
                byteData = new byte[1024];

                clientSocket.BeginReceive(byteData,
                                           0,
                                           byteData.Length,
                                           SocketFlags.None,
                                           null,
                                           null);
                Liefdes_brief.GetPackage(byteData);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ OnReceive:" + ex.Message);
            }
        }

        public void Run()
        {
            while (true)
            {
                Thread.Sleep(500);
                Receive();
                Thread.Sleep(500);
                Send();
            }
        }
    }
}