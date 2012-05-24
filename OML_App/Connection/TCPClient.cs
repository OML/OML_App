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
using System.Runtime.InteropServices;
using OML_App.Data;

namespace OML_App.Connection
{
    /// <summary>
    /// setting up client en connection to server
    /// </summary>
    public class TCPClient
    {
        #region Variables

        public Socket clientSocket;
        Data Liefdes_brief = new Data();
        //Data.SendStructPackage packet;
        byte[] packet;
        public bool connected = false;
        string Ip_Adress;
        //data buffer
        private byte[] byteData = new byte[1024];
        //temp checking if changed
        byte[] Temp = new byte[1024];
        //port
        int Port;
        //if first time

        int result_opcode;
        #endregion

        #region Setting_Up_Client
        public TCPClient(string ip_adress, int port)
        {
            this.Ip_Adress = "192.168.1.100"; //"192.168.1.103";
            this.Port = 1337;//port
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
                //Server is listening on port 1337
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Port);
                //Connect to the server

                //clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, null, null);
                //Thread.Sleep(1000);
                //byteData = new byte[1024];
                ////Start listening to the data asynchronously
                //clientSocket.BeginReceive(byteData,
                //                           0,
                //                           byteData.Length,
                //                           SocketFlags.None,
                //                           new AsyncCallback(OnReceive),
                //                           null);


                //Send();
                System.Console.WriteLine(String.Format("Connecting to {0}:{1}", Ip_Adress, Port));
                clientSocket.Connect(ipEndPoint);
                connected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connected = true;

            }
        }
        #region send
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            //catch (ObjectDisposedException)
            //{
            //}
            catch (Exception ex)
            {
                System.Console.WriteLine("@ OnSend:" + ex.Message);
            }
        }

        private void Send()
        {
            try
            {
                clientSocket.BeginSend(packet, 0, packet.Length, SocketFlags.None, null, null);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ Send:" + ex.Message);
            }
        }
        #endregion

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndReceive(ar);
                byteData = new byte[1024];
                clientSocket.BeginReceive(byteData,
                                          0,
                                          byteData.Length,
                                          SocketFlags.None,
                                          new AsyncCallback(OnReceive),
                                          null);

            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
            }
        }

        //private void Receive()
        //{
        //    try
        //    {
        //        byteData = new byte[1024];
        //        clientSocket.BeginReceive(byteData,
        //                                   0,
        //                                   byteData.Length,
        //                                   SocketFlags.None,
        //                                   null,
        //                                   null);
        //        //result_opcode = Liefdes_brief.GetPackage(byteData);
        //        Receive();

        //    }
        //    catch (ObjectDisposedException)
        //    { }
        //    catch (Exception ex)
        //    {
        //        System.Console.WriteLine("@ OnReceive:" + ex.Message);
        //    }
        //}

        #region ischanged
        /// <summary>
        /// Check if the incoming data has changed
        /// </summary>
        /// <returns>boolean if changed returns a true</returns>
        public bool isChanged()
        {
            bool ischanged = false;

            if (Temp == byteData)
            {
                ischanged = false;
            }
            else
            {
                Temp = byteData;
                ischanged = true;
            }
            return ischanged;

        }
        #endregion

        #region keepalive
        public bool keepalive()
        {
            bool alive = false;

            packet = Liefdes_brief.SendPackage(4);
            Send();
            Thread.Sleep(50);

            if (Liefdes_brief.GetPackage(byteData) == 1)
            {
                alive = true;
            }
            else
            {
                keepalive();
            }
            return alive;

        }
        #endregion

        #region report
        public bool Reporting()
        {
            bool report = false;
            packet = Liefdes_brief.SendPackage(3);
            Send();
            Thread.Sleep(50);

            if (Liefdes_brief.GetPackage(byteData) == 3)
            {
                report = true;
            }
            else
            {
                keepalive();
            }
            return report;
        }
        #endregion

        #region syncing
        public bool syncing()
        {
            bool syncing = false;
            packet = Liefdes_brief.SendPackage(3);
            Send();
            Thread.Sleep(50);

            if (Liefdes_brief.GetPackage(byteData) == 2)
            {
                syncing = true;
            }
            else
            {
                keepalive();
            }
            return syncing;
        }
        #endregion

        public void Run()
        {
            packet = Liefdes_brief.SendPackage(4);
            Send();
            Thread.Sleep(1000);

            //int opc = 0;
            //int count = 0;
            //keepalive();
            //Reporting();
            //while (true)
            //{

            //    if (isChanged())
            //    {
            //        opc = Liefdes_brief.GetPackage(byteData);
            //        switch (opc)
            //        {
            //            case 0:
            //                //execute last command
            //                break;
            //            case 1:
            //                //code accepted
            //                break;
            //            case 2:
            //                //sync ok
            //                break;
            //            case 3:
            //                //do nothing save code
            //                break;
            //        }
            //    }

            //    Thread.Sleep(200);
            //    //counter to get once a 800ms a keepalive
            //    count++;
            //    if (count >= 4)
            //    {
            //        keepalive();
            //        count = 0;
            //    }
            //    try
            //    {
            //        packet = Liefdes_brief.SendPackage(2);
            //        Send();
            //    }
            //    catch{} 
            //}
        }
    }           
}