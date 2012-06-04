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

namespace OML_App.Connection.Viewer
{
    //The commands for interaction between the server and the client
    enum Command
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        DataPacket,    //Send a text message to all the chat clients
        Null        //No command
    }

    class TCPViewer
    {
        public Socket clientSocket;
        private byte[] byteData = new byte[1024];
        private bool FirstConnect = true;

        public TCPViewer()
        {
            Connect("192.168.0.1");
        }

        private void Connect(string ip)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ipAddress = IPAddress.Parse(ip);
            //Server is listening on port 13000
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 13000);

            //Connect to the server
            clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
        }

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);

                //We are connected so we login into the server
                ViewerData msgToSend = new ViewerData();
                msgToSend.OPCommand = Command.Login;
                msgToSend.Message = null;

                byte[] b = msgToSend.ToByte();

                //Send the message to the server
                clientSocket.BeginSend(b, 0, b.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
                //Connected Start The Recieve now
                if (FirstConnect)
                {
                    FirstConnect = false;
                    //Start listening to the data asynchronously
                    clientSocket.BeginReceive(byteData,
                                               0,
                                               byteData.Length,
                                               SocketFlags.None,
                                               new AsyncCallback(OnReceive),
                                               null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);

                ViewerData msgReceived = new ViewerData(byteData);
                //Accordingly process the message received
                switch (msgReceived.OPCommand)
                {
                    case Command.DataPacket:
                        //TODO Save the Packed somewhere!
                        break;                  
                } 

                byteData = new byte[1024];
                clientSocket.BeginReceive(byteData,
                                          0,
                                          byteData.Length,
                                          SocketFlags.None,
                                          new AsyncCallback(OnReceive),
                                          null);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendDataPack(byte[] pack)
        {
            try
            {
                //Fill the info for the message to be send
                ViewerData msgToSend = new ViewerData();                
                msgToSend.Message = pack;
                msgToSend.OPCommand = Command.DataPacket;

                byte[] byteData = msgToSend.ToByte();

                //Send it to the server
                clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
             }
            catch (Exception)
            {
                Console.WriteLine("Unable to send message to the server. OMLServerTCP: ");
            }
        }
    }
}