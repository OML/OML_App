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

namespace OML_App.Connection
{
    //The commands for interaction between the server and the client
    enum Command
    {
        Login,      //Log into the server
        Logout,     //Logout of the server
        Message,    //Send a text message to all the chat clients
        List,       //Get a list of users in the chat room from the server
        Null        //No command
    }

    public class TCPClient
    {
        Activity1 MainAct;
        public Socket clientSocket;
        public string strName = "Test ReintJan";

        private byte[] byteData = new byte[1024];

        public TCPClient(Activity1 Act)
        {
            this.MainAct = Act;
            Connect("141.252.221.71");
        }

        public void Connect(string ipadress)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse(ipadress);
                //Server is listening on port 1000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 1234);

                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ Connect:" + ex.Message);
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

        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                System.Console.WriteLine("@ OnConnect:" + "Start Timer");
                Thread.Sleep(5000);
                clientSocket.EndConnect(ar);

                //We are connected so we login into the server
                Data msgToSend = new Data();
                msgToSend.cmdCommand = Command.Login;
                msgToSend.strName = strName;
                msgToSend.strMessage = null;

                byte[] b = msgToSend.ToByte();

                //Send the message to the server
                clientSocket.BeginSend(b, 0, b.Length, SocketFlags.None, new AsyncCallback(OnSend), null);

                //Start OnReceive
                byteData = new byte[1024];
                //Start listening to the data asynchronously
                clientSocket.BeginReceive(byteData,
                                           0,
                                           byteData.Length,
                                           SocketFlags.None,
                                           new AsyncCallback(OnReceive),
                                           null);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ OnConnect:" + ex.Message);
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);

                Data msgReceived = new Data(byteData);
                //Accordingly process the message received
                switch (msgReceived.cmdCommand)
                {
                    case Command.Login:
                        //lstChatters.Items.Add(msgReceived.strName);
                        break;

                    case Command.Logout:
                        //lstChatters.Items.Remove(msgReceived.strName);
                        break;

                    case Command.Message:
                        break;

                    case Command.List:
                        //lstChatters.Items.AddRange(msgReceived.strMessage.Split('*'));
                        //lstChatters.Items.RemoveAt(lstChatters.Items.Count - 1);
                        //txtChatBox.Text += "<<<" + strName + " has joined the room>>>\r\n";
                        break;
                }

                if (msgReceived.strMessage != null && msgReceived.cmdCommand != Command.List)
                {
                    //MainAct.LogText += msgReceived.strMessage + "\r\n";
                    //MainAct.OnSetText();

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
                System.Console.WriteLine("@ OnReceive:" + ex.Message);
            }
        }
    }
}