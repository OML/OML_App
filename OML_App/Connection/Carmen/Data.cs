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
using System.Runtime.InteropServices;
using OML_App.Data;


namespace OML_App.Connection
{
    /// <summary>
    /// @author Daniel de Valk
    /// Class where we create our data packages and get our data from the packages
    /// </summary>
    public class Data
    {
        #region variable

        public string[] words;
        public byte opcode; //opcode
        //public byte length_list; //length of the list
        //public byte lengthname0; //length of a name
        //public byte name; //name

        #endregion

        #region Sendpackage
        #region switch
        /// <summary>
        /// give the opcode and it executes its request!
        /// </summary>
        /// <param name="ludevedu">0==reject 1==ok 2==sync 3==report 4==keepalive </param>
        /// <returns></returns>
        public byte[] SendPackage(int ludevedu)
        {
            byte[] buffer = null;
            
            switch (ludevedu)
            {
                //Reject
                case 0:
                    Reject();
                    break;
                ////ok
                //case 1:
                //    buffer = new byte[Marshal.SizeOf(ok())];
                //    break;
                //sync
                case 2:
                    SendStructPackage packet = Sync();
                    buffer = new byte[Marshal.SizeOf(packet)];
                    //buffer = Sync();
                    unsafe
                    {
                        GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        Marshal.StructureToPtr(packet, gch.AddrOfPinnedObject(), false);
                        gch.Free();
                    }
                    break;
                //report
                case 3:
                    {
                        ReportStructPackage packet0 = Report();
                        buffer = new byte[Marshal.SizeOf(packet0)];
                        unsafe
                        {
                            GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                            Marshal.StructureToPtr(packet0, gch.AddrOfPinnedObject(), false);
                            gch.Free();
                        }
                        break;
                    }
                //keep
                case 4:
                    keepaliveStructPackage packet1 = keep();
                    buffer = new byte[Marshal.SizeOf(keep())];
                    unsafe
                    {
                        GCHandle gch = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                        Marshal.StructureToPtr(packet1, gch.AddrOfPinnedObject(), false);
                        gch.Free();
                    }
                    break;
                default:
                    break;
            }


            return buffer;
        }
        #endregion

        #region Reject
        public void Reject()
        {

        }
        #endregion

        #region OK
        ///// <summary>
        ///// Standard Accept package is always standard
        ///// </summary>
        ///// <returns>okStructpackage</returns>
        //public okStructPackage ok()
        //{
        //    okStructPackage ok = new okStructPackage();
        //    ok.opcode = 1;
        //    ok.padding = 1;
        //    return ok;
        //}

        ///// <summary>
        ///// ok struct package
        ///// </summary>
        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        //public struct okStructPackage
        //{
        //    public byte opcode;
        //    public byte padding;
        //}
        #endregion

        #region Sync
        /*
         *      request (opcode = sync)
         * 		Type C / C#				Name
         * 		--------------------------------------
         * 		8bit	/ byte			opcode = sync
         * 		8bit	/ byte			Boolean speed 0 slow and 1 fast driving
         * 		8bit signed / sbyte 	left int -100 t/m 100 //left wheel values
         * 		8bit Signed / sbyte		Right int -100 t/m 100 //Right wheel values
         * 		8bit	/ byte			boolean calibration mode //possibility to move 
         *                              each engine separate
         * 		16bit unigned / UInt16	engine int[4] 0..65535 rpm
         * 		8bit	/ byte			sound int //playing sounds array[99] possibility 
         *                              of max 99 sounds SO Sounds[0] gets first sound
         */
        public SendStructPackage Sync()
        {
            SendStructPackage sync = new SendStructPackage();
            sync.opcode = 2;
            sync.speed = Convert.ToByte(Send_Singleton.Instance.speed);
            sync.left =  Convert.ToSByte(Send_Singleton.Instance.left);
            sync.right = Convert.ToSByte(Send_Singleton.Instance.right);
            sync.Calibration_mode = Convert.ToByte(Send_Singleton.Instance.Calibration_mode);
            sync.engine_0 = Convert.ToSByte( Send_Singleton.Instance.engine0);
            sync.engine_1 = Convert.ToSByte(Send_Singleton.Instance.engine1);
            sync.engine_2 = Convert.ToSByte(Send_Singleton.Instance.engine2);
            sync.engine_3 = Convert.ToSByte(Send_Singleton.Instance.engine3);
            //sync.engines = 5;
            sync.sound = Convert.ToByte(Send_Singleton.Instance.sound);
            //put sound to a hold
            if (Send_Singleton.Instance.sound != 0)
            { Send_Singleton.Instance.sound = 0; }
            sync.IOModule = Convert.ToByte(Send_Singleton.Instance.releaseRing);
            return sync;

        }

        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SendStructPackage
        {
            public byte opcode; //opcode
            public byte speed; //Boolean speed 0 slow and 1 fast driving
            public sbyte left; //left int -100 t/m 100 //left wheel values
            public sbyte right; //Right int -100 t/m 100 //Right wheel values
            public byte Calibration_mode; //boolean calibration mode //possibility to move each engine separate
            public short engine_0; //engine int[4] -100 t/m 100
            public short engine_1;
            public short engine_2;
            public short engine_3;
            //public Int16 engines;
            public byte sound; //sound int //playing sounds array[99] possibility of max 99 sounds SO Sounds[0] gets first sound
            public byte IOModule;
        }

        #endregion

        #region report
        /*
         *      request (opcode = report)
         * 		Type C / C#				Name
         * 		--------------------------------------
         * 		8bit / byte				opcode = report
         *   	8bit / byte				padding
         *      16bit / Int16			throttle event treshold (percent)
         *		16bit unsigned / UInt16	voltage event treshold (mV)
         *		16bit unsigned / UInt16	current event treshold (mA)
         *		16bit / Int16			temperature event treshold (tenth degrees)
         *		16bit / Int16			Gyro event treshold (hundreth G)
         */
        public ReportStructPackage Report()
        {
            ReportStructPackage report = new ReportStructPackage();
            try
            {
                
                report.opcode = 3;
                report.padding = 1;
                report.throttle = 10;//Convert.ToInt16(Send_Singleton.Instance.throttle);
                report.voltage = 11;///Convert.ToUInt16(Send_Singleton.Instance.voltage);
                report.current = 11;//Convert.ToUInt16(Send_Singleton.Instance.current);
                report.temperature = 11;//Convert.ToInt16(Send_Singleton.Instance.temperature);
                report.Gyro = 11;///Convert.ToInt16(Send_Singleton.Instance.Gyro);
                Console.WriteLine("succesfully setup Report structpackage");
                
                
            }
            catch {
                Console.WriteLine("Abort structpackage");
            }
            return report;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ReportStructPackage
        {
            public byte opcode;
            public byte padding;
            public Int16 throttle;
            public UInt16 voltage;
            public UInt16 current;
            public Int16 temperature;
            public Int16 Gyro;
        }


        #endregion

        #region Keep Alive
        /// <summary>
        /// Standard keep alive package
        /// </summary>
        public keepaliveStructPackage keep()
        {
            keepaliveStructPackage keep_alive = new keepaliveStructPackage();
            keep_alive.opcode = 4;
            keep_alive.padding = 1;
            return keep_alive;
        }

        /// <summary>
        /// keep alive package must be send in less than 2 seconds.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct keepaliveStructPackage
        {
            public byte opcode;
            public byte padding;
        }
        #endregion
        #endregion

        #region Getpackage

        /*
             * 0    M0V
             * 1    M1V
             * 2    M2V
             * 3    M3V
             * 4    MOA
             * 5    M1A
             * 6    M2A
             * 7    M3A
             * 8    M0T
             * 9    M1T
             * 10   M2T
             * 11   M3T
             * 12   M0TH
             * 13   M1TH
             * 14   M2TH
             * 15   M3TH
             * 16   A0V
             * 17   A0A
             * 18   A0T
             * 19   A1V
             * 20   A1A
             * 21   A1T
             * 22   G0X
             * 23   G0Y
             * 25   G0Z
             */ 

        //Converts the bytes into an object of type Data
        /// <summary>
        /// TODO Fix that the data is setup in the singleton classes the data gets stored in our
        /// variables in this class so...
        ///
        /// opcodes:
        /// reject = 0
        /// ok =1
        /// sync =2
        /// report =3
        /// keepalive =4
        /// 
        ///
        /// </summary>
        /// <param name="data"></param>
        public int GetPackage(byte[] data)
        {
            int return_opcode = 0;
            try
            {
                opcode = data[0];
            }
            catch {
                Console.WriteLine("unable to read opcode");
            }
            if (opcode == 0)
            {
                //data reject
                return_opcode = 0;
            }
            if (opcode == 1)
            {
                //data ok
                return_opcode = 1;
                
            }
            else if (opcode == 2)
            {
                //data sync
                try
                {
                    //
                    SyncStructPackage pack = new SyncStructPackage();
                    unsafe
                    {
                        SyncStructPackage* p;
                        GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        p = (SyncStructPackage*)gch.AddrOfPinnedObject().ToPointer();
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
                return_opcode = 2;
            }
            //get list sounds "REPORT"
            else if (opcode == 3)
            {
                int pad = data[1];
                int protocol = data[2];
                int revision = data[3];

                int list_count = data[4]; //get total numbers of words
                words = new string[list_count];//create the length of an array
                int counter_word = 0; //wich word on what place in an array 
                bool new_word = true; //if there is an new word get the new length
                int length_word = 0;

                for (int i = 5; i < list_count; i++) // total list length
                {

                    if (new_word)//get new length of new word
                    {
                        length_word = data[i];// if correct the length is on this position
                        new_word = false; //stop doing that!
                    }
                    char[] chars = new char[length_word];//we now our length now
                    for (int j = 0; j < length_word; j++)//for each char in the length of word
                    {
                        chars[j] = BitConverter.ToChar(data, i);
                        i++;
                    }
                    words[counter_word] = chars.ToString();
                    counter_word++;
                    new_word = true;
                }
                return_opcode = 3;
            }
            else if (opcode == 4)
            {
                //keep alive
                try
                {
                    //
                    keepaliveStructPackage keepalive = new keepaliveStructPackage();
                    unsafe
                    {
                        keepaliveStructPackage* p;
                        GCHandle gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                        p = (keepaliveStructPackage*)gch.AddrOfPinnedObject().ToPointer();
                        keepalive = *p;
                        gch.Free();
                        Console.WriteLine("Read Keep alive package");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to read keap alive");
                    Console.WriteLine(e);
                }
                return_opcode = 4;
            }
            else { return_opcode = 99; }
            Console.WriteLine("0==reject 1==ok 2==sync 3==report 4==keepalive");
            Console.WriteLine("received kinda package : " + return_opcode);
            return return_opcode;
        }


        /* Carmen Send:
         * response (request opcode = sync, calibrate = 1)
         * 		Type C / C#					    Name
         * 		--------------------------------------
         * 		8bit							opcode = sync
         * 		32bit unsigned / UInt32			timestamp (ms since start server)
         * 		for n = 0 .. 3:
         * 			16bit signed / Int16		throttle motor n [percent]
         * 			16bit unsigned / UInt16		current motor n [milliamps]
         * 			16bit unsigned / UInt16		voltage motor n [millivolts]
         * 			16bit / Int16				temperature motor n [tenth degrees]
         * 
         * 		16bit signed / Int16			gyro X (G / 100)
         * 		16bit signed / Int16			gyro Y (G / 100)
         * 		16bit signed / Int16			gyro Z (G / 100)
         *
         * 		16bit unsigned / UInt16			accu voltage [millivolts]
         * 		16bit unsigned / UInt16			accu current [milliamps]
         *      16bit signed / Int16  			accu temperature accu 1 [tenth degrees]
         *      16bit signed / Int16  			accu temperature accu 2 [tenth degrees]
         *      16bit signed / Int16  			cable temperature 1 [tenth degrees]
         *      16bit signed / Int16  			cable temperature 2 [tenth degrees]
         *      16bit signed / Int16  			cable temperature 3 [tenth degrees]
         *      16bit signed / Int16  			cable temperature 4 [tenth degrees]
         */
        /// <summary>
        /// syncstruct package
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SyncStructPackage
        {
            public byte opcode; //opcode
            public UInt32 timestamp;

            public Int16 throttle_0;
            public UInt16 current_motor_0;
            public UInt16 voltage_motor_0;
            public Int16 Temp_0;

            public Int16 throttle_1;
            public UInt16 current_motor_1;
            public UInt16 voltage_motor_1;
            public Int16 Temp_1;

            public Int16 throttle_2;
            public UInt16 current_motor_2;
            public UInt16 voltage_motor_2;
            public Int16 Temp_2;

            public Int16 throttle_3;
            public UInt16 current_motor_3;
            public UInt16 voltage_motor_3;
            public Int16 Temp_3;

            public Int16 gyro_x;
            public Int16 gyro_y;
            public Int16 gyro_z;

            public UInt16 accu_voltage; //millivolts
            public UInt16 accu_current; //milliamps
            public Int16 accu_temp1;
            public Int16 accu_temp2;

            public Int16 Cable_temp_0;
            public Int16 Cable_temp_1;
            public Int16 Cable_temp_2;
            public Int16 Cable_temp_3;

            public override string ToString()
            {
                return "Package pack: \n " 
                    + "Opcode:     "+ opcode 
                    + "\n time:        "+ timestamp 
                    + "\n throttles:   "+ throttle_0 + " "      +throttle_1 +" "+           throttle_2 + " " +      throttle_3 
                    + "\n Current      "+ current_motor_0 + " " + current_motor_1 +" "+     current_motor_2 +" "+   current_motor_3 
                    + "\n Voltage:     "+ voltage_motor_0 +" "  + voltage_motor_1 +" "+     voltage_motor_2 +" "+   voltage_motor_3
                    + "\n Temprature:  "+ Temp_0 +" "           + Temp_1 +" " +             Temp_2 +" "+            Temp_3 
                    + "\n Gyro values: "+ gyro_x +" "           + gyro_y +" "+              gyro_z 
                    + "\n accu values: "+ accu_current +" "     + accu_temp1+" "+           accu_temp2 +" "+        accu_voltage  ;
            }
        }

        #endregion
    }
}