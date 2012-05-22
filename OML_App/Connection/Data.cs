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


namespace OML_App.Connection
{   
    /// <summary>
    /// @author Daniel de Valk
    /// Class where we create our data packages and get our data from the packages
    /// </summary>
    class Data
    {
        #region variable

        //our incoming package
        SyncStructPackage pack = new SyncStructPackage();

        public string[] words;
        public byte opcode; //opcode
        //public byte length_list; //length of the list
        //public byte lengthname0; //length of a name
        //public byte name; //name

        #endregion

        #region Sendpackage
        /// <summary>
        /// Converts the Data structure into an array of bytes
        /// </summary>
        /// <param name="Opcode"></param>
        /// <param name="Speed"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="Calibrationmode"></param>
        /// <param name="engine"></param>
        /// <param name="sound"></param>
        /// <returns></returns>
        public SendStructPackage SetPacket(byte Opcode, byte Speed, sbyte left, sbyte right, byte Calibrationmode, ushort engine, byte sound)
        {
            SendStructPackage pack = new SendStructPackage();
            pack.opcode = Opcode;
            pack.speed = Speed;
            pack.left = left;
            pack.right = right;
            pack.Calibration_mode = Calibrationmode;
            pack.engine = engine;
            pack.sound = sound;

            return pack;
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
            public ushort engine; //engine int[4] -100 t/m 100
            public byte sound; //sound int //playing sounds array[99] possibility of max 99 sounds SO Sounds[0] gets first sound
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct okStructPackage
        {
            public byte opcode;
            public byte padding;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct whatStructPackage
        {
            byte opcode;
            byte padding;
            short throttle;
            ushort voltage;
            ushort current;
            short temperature;
            short gyro;
                
        }



        #endregion

        #region Getpackage
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
        public void GetPackage(byte[] data)
        {
            opcode = data[0];
            if (opcode == 0)
            {
                //data reject
            }
            if (opcode == 1)
            {
                //data ok
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
                        GCHandle gch = GCHandle.Alloc(data);
                        p = (SyncStructPackage*)gch.AddrOfPinnedObject().ToPointer();
                        pack = *p;
                        gch.Free();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            //get list sounds "REPORT"
            else if(opcode == 3)
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
            }
            else if (opcode == 4)
            {
                //keep alive
                try
                {
                    //
                    SyncStructPackage keepalive = new SyncStructPackage();
                    unsafe
                    {
                        SyncStructPackage* p;
                        GCHandle gch = GCHandle.Alloc(data);
                        p = (SyncStructPackage*)gch.AddrOfPinnedObject().ToPointer();
                        keepalive = *p;
                        gch.Free();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

        }

        /// <summary>
        /// keep alive
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct keepaliveStructPackage
        {
            byte opcode;
            byte padding;
        }

        /// <summary>
        /// syncstruct package
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SyncStructPackage
        {
            public byte opcode; //opcode
            public UInt32 timestamp;

            public short throttle_0;
            public ushort current_motor_0;
            public ushort voltage_motor_0;
            public short Temp_0;

            public short throttle_1;
            public ushort current_motor_1;
            public ushort voltage_motor_1;
            public short Temp_1;

            public short throttle_2;
            public ushort current_motor_2;
            public ushort voltage_motor_2;
            public short Temp_2;

            public short throttle_3;
            public ushort current_motor_3;
            public ushort voltage_motor_3;
            public short Temp_3;

            public short gyro_x;
            public short gyro_y;
            public short gyro_z;

            public short accu_voltage; //millivolts
            public short accu_current; //milliamps
            public short accu_temp;
        }

        #endregion
    }
}