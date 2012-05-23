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
        private byte[] byteData = new byte[1024];
        int Port;
        int result_opcode;
        #endregion

        #region Setting_Up_Client
        public TCPClient(string ip_adress, int port)
        {
            bool connect = false;
            this.Ip_Adress = "192.168.1.152"; //"192.168.1.103";
            this.Port = 1337;
            Connect();
            Thread newThread = new Thread(new ThreadStart(Run));
            newThread.Start();
            if (!connect)
            {
                //please an error message
            }
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


                Send();
                clientSocket.Connect(ipEndPoint);
                connected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                connected = true;

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
                //byte[] buffer = new byte[Marshal.SizeOf(packet)];

                ////Console.WriteLine(Marshal.SizeOf(packet));
                //unsafe
                //{
                //    GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                //    Marshal.StructureToPtr(packet, gch.AddrOfPinnedObject(), false);
                //    gch.Free();
                //}
                clientSocket.BeginSend(packet, 0, packet.Length, SocketFlags.None, null, null);
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
                clientSocket.Receive(byteData);
                clientSocket.BeginReceive(byteData,
                                           0,
                                           byteData.Length,
                                           SocketFlags.None,
                                           null,
                                           null);
                result_opcode = Liefdes_brief.GetPackage(byteData);

            }
            catch (ObjectDisposedException)
            { }
            catch (Exception ex)
            {
                System.Console.WriteLine("@ OnReceive:" + ex.Message);
            }
        }

        //i want a loop from uno second
        public void Run()
        {
            while (true)
            {
                packet = Liefdes_brief.SendPackage(4);
                Send();
                //Receive();
                Thread.Sleep(1000);
                //Send();

                //Send();
                //packet = Liefdes_brief.SendPackage(3);
                //Send();
                //packet = Liefdes_brief.SendPackage(2);
                //Send();
                //SetPacket(2, 0, 25, 25, 0, 10, 0);
                //Receive();
            }
        }
    }
}