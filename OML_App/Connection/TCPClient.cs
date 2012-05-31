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
using System.Diagnostics;

namespace OML_App.Connection
{
    /// <summary>
    /// setting up client en connection to server
    /// </summary>
    public class TCPClient
    {
        #region variable
        //client sock
        Socket m_socClient;
        Data Liefdes_brief = new Data();

        string IP_Adress;
        int Port;
        public bool connected = false;
        public bool connection_impossible = false;

        //data buffer
        private byte[] byteData = new byte[1024];
        //temp checking if changed
        byte[] Temp = new byte[1024];
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipadress">give up ipadress</param>
        /// <param name="port">Port name</param>
        public TCPClient(string ipadress, int port)
        {
            this.IP_Adress = ipadress;//ipadress;
            this.Port = port;//port;
            cmdConnect();
            Thread newThread = new Thread(new ThreadStart(Run));
            newThread.Start();
        }

        /// <summary>
        /// Setup an connection
        /// </summary>
        private void cmdConnect()
        {
            try
            {
                //create a new client socket ...
                m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                String szIPSelected = IP_Adress;
                int alPort = Port;
                System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(szIPSelected);
                System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, alPort);
                m_socClient.Connect(remoteEndPoint);
                byteData = Liefdes_brief.SendPackage(4);
                m_socClient.Send(byteData);
                connected = true;

            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
                connected = false;
            }
        }

        /// <summary>
        /// Sending data
        /// </summary>
        /// <param name="Pack">0==reject 1==ok 2==sync 3==report 4==keepalive</param>
        private void cmdSendData(int Pack)
        {
            try
            {
                byteData = Liefdes_brief.SendPackage(Pack);
                m_socClient.Send(byteData);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
                connected = false;
            }
        }

        /// <summary>
        /// Receive data
        /// </summary>
        private void cmdReceiveData()
        {
            try
            {
                byte[] buffer = new byte[1024];
                
                int irx = m_socClient.Receive(buffer);
                if (irx == 0)
                {
                    Console.WriteLine("no data available");
                }
                else
                {
                    Console.WriteLine("wat is irx?: " + irx);
                    int opcode = Liefdes_brief.GetPackage(buffer);
                    Console.WriteLine(opcode);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(se);
            }
        }

        /// <summary>
        /// Close data connection
        /// </summary>
        private void cmdClose()
        {
            m_socClient.Close();
        }

       /// <summary>
       /// run data connection
       /// </summary>
        public void Run()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int counter = 0;
            
            while (true)
            {

                if (connected)
                {
                    if (stopwatch.ElapsedMilliseconds > 1000) 
                    { 
                        cmdSendData(4);
                        Thread.Sleep(100);
                        cmdReceiveData();
                        stopwatch.Reset();
                        stopwatch.Start(); 
                    }
                    cmdSendData(2);
                    Thread.Sleep(100);
                    cmdReceiveData();
                    
                }
                else 
                {
                    counter++;
                    if (counter <= 25)
                    {
                        cmdClose();
                        Thread.Sleep(250);
                        //AlertDialog AlertaMensagem = new AlertDialog.Builder(this).SetIcon(Resource.Drawable.Icon).SetTitle("Connextion lost!").SetMessage(IP_Adress);
                        cmdConnect();
                    }
                    else
                    {
                        connection_impossible = true;
                    }
                }
            }


        }
    }
    


}