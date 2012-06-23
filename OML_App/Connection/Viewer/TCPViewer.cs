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
using OML_App.Setting;
using OML_App.Connection;
using OML_App.Data;
using System.Runtime.InteropServices;

namespace OML_App.Connection.Viewer
{
    //The commands for interaction between the server and the client
    enum Command
    {
        Null,       //No command
        Login,      //Log into the server
        Logout,     //Logout of the server
        DataPacket, //Send a text message to all the chat clients
        AppPack     //Send a Message when server        
    }

    class TCPViewer
    {
        public Socket clientSocket;
        private byte[] byteData = new byte[1024];
        private bool FirstConnect = true;
        private bool SetServer = false;

        public TCPViewer()
        {
            Connect(Settings_Singleton.Instance.TCP_View_IP);
        }

        public TCPViewer(bool State)
        {
            //Bool Set server
            Connect(Settings_Singleton.Instance.TCP_View_IP);
            SetServer = State;
                     
        }

        /// <summary>
        /// First Connect To Server
        /// </summary>
        /// <param name="ip">Ip address</param>
        private void Connect(string ip)
        {
            if (clientSocket == null)
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress ipAddress = IPAddress.Parse(ip);
                //Server is listening on port 12000
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Settings_Singleton.Instance.TCP_View_Port);
                Console.WriteLine("Try to connect");
                //Connect to the server
                clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), null);
            }
        }

        /// <summary>
        /// OnConnect : connect to server. Send Connect Message
        /// </summary>
        /// <param name="ar"></param>
        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
                Console.WriteLine("Connected");
                //We are connected so we login into the server
                ViewerData msgToSend = new ViewerData();
                msgToSend.OPCommand = Command.Login;
                msgToSend.Message = null;

                byte[] b = msgToSend.ToByte();
                //Send the message to the server
                Console.WriteLine("Try Send Welcome packet");
                clientSocket.BeginSend(b, 0, b.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// OnSend : Send an TCP packet!
        /// </summary>
        /// <param name="ar"></param>
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
                Console.WriteLine("Packed Send");
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
                    if (SetServer)
                    {
                        SendAppServerPack(true);
                        SetServer = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Handle Recieved TCP Packet
        /// </summary>
        /// <param name="ar"></param>
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
                        //data sync
                        try
                        {
                            //
                            OML_App.Connection.Data.SyncStructPackage pack = new OML_App.Connection.Data.SyncStructPackage();
                            unsafe
                            {
                                OML_App.Connection.Data.SyncStructPackage* p;
                                GCHandle gch = GCHandle.Alloc(msgReceived.Message, GCHandleType.Pinned);
                                p = (OML_App.Connection.Data.SyncStructPackage*)gch.AddrOfPinnedObject().ToPointer();
                                pack = *p;
                                gch.Free();
                            }
                            DateTime this_time = DateTime.Now;
                            Console.WriteLine("succesfully synced");
                            Console.WriteLine(pack.ToString());
                            Receive_Singleton.Instance.Current_ses.Sensors[0].AddValueDataToArray(new ValueData(pack.voltage_motor_0, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[1].AddValueDataToArray(new ValueData(pack.voltage_motor_1, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[2].AddValueDataToArray(new ValueData(pack.voltage_motor_2, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[3].AddValueDataToArray(new ValueData(pack.voltage_motor_3, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[4].AddValueDataToArray(new ValueData(pack.current_motor_0, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[5].AddValueDataToArray(new ValueData(pack.current_motor_1, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[6].AddValueDataToArray(new ValueData(pack.current_motor_2, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[7].AddValueDataToArray(new ValueData(pack.current_motor_3, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[8].AddValueDataToArray(new ValueData(pack.Temp_0, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[9].AddValueDataToArray(new ValueData(pack.Temp_1, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[10].AddValueDataToArray(new ValueData(pack.Temp_2, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[11].AddValueDataToArray(new ValueData(pack.Temp_3, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[12].AddValueDataToArray(new ValueData(pack.throttle_0, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[13].AddValueDataToArray(new ValueData(pack.throttle_1, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[14].AddValueDataToArray(new ValueData(pack.throttle_2, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[15].AddValueDataToArray(new ValueData(pack.throttle_3, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[16].AddValueDataToArray(new ValueData(pack.accu_voltage, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[17].AddValueDataToArray(new ValueData(pack.accu_current, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[18].AddValueDataToArray(new ValueData(pack.accu_temp1, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[19].AddValueDataToArray(new ValueData(pack.accu_voltage, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[20].AddValueDataToArray(new ValueData(pack.accu_current, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[21].AddValueDataToArray(new ValueData(pack.accu_temp1, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[22].AddValueDataToArray(new ValueData(pack.gyro_x, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[23].AddValueDataToArray(new ValueData(pack.gyro_y, this_time));
                            Receive_Singleton.Instance.Current_ses.Sensors[24].AddValueDataToArray(new ValueData(pack.gyro_z, this_time));
                            //Receive_Singleton.Instance.Current_ses.Sensors[25].AddValueDataToArray(new ValueData(pack.voltage_motor_0, this_time));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("het lezen van syncpackage is mislukt");
                        }
                        break;
                    case Command.AppPack:
                        //Set Server State
                        if (BitConverter.ToInt32(msgReceived.Message, 4) == 1)
                            Settings_Singleton.Instance.TCP_View_State = true;
                        else
                            Settings_Singleton.Instance.TCP_View_State = false;
                        
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

        /// <summary>
        /// Close the TCP Connection
        /// </summary>
        public void OnClose()
        {
            try
            {
                //Fill the info for the message to be send
                ViewerData msgToSend = new ViewerData();
                msgToSend.Message = null;
                msgToSend.OPCommand = Command.Logout;

                byte[] byteData = msgToSend.ToByte();

                //Send it to the server
                clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                clientSocket.Close();

                clientSocket = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send a Pack to the TCP Viewer Server to gain server acces
        /// </summary>
        /// <param name="state">True / False to set server</param>
        public void SendAppServerPack(bool state)
        {
            try
            {
                List<byte> result = new List<byte>();
                //First four are for the Command
                result.AddRange(BitConverter.GetBytes((int)Command.AppPack));
                if (!Settings_Singleton.Instance.TCP_View_State)
                {
                    result.AddRange(BitConverter.GetBytes((int)1));
                    Settings_Singleton.Instance.TCP_View_State = true;
                    Settings_Singleton.Instance.TCP_View_IsServer = true;
                }
                else
                {
                    result.AddRange(BitConverter.GetBytes((int)0));
                    Settings_Singleton.Instance.TCP_View_State = false;
                    Settings_Singleton.Instance.TCP_View_IsServer = false;
                }

                byte[] byteData = result.ToArray();

                //Send it to the server
                clientSocket.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to send message to the server. OMLServerTCP: ");
            }
        }

        /// <summary>
        /// Send a (Data) pack to the TCP Viewer
        /// </summary>
        /// <param name="pack">byte[1020] pack</param>
        public void SendDataPack(byte[] pack)
        {
            try
            {
                byte[] nPack = new byte[1020];
                pack.CopyTo(nPack, 4);
                //Fill the info for the message to be send
                ViewerData msgToSend = new ViewerData();
                msgToSend.Message = nPack;
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