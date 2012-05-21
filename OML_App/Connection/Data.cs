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
        public SendStructPackage SetPacket(byte Opcode, byte Speed, sbyte left, sbyte right, byte Calibrationmode, sbyte engine, byte sound)
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
            public sbyte engine; //engine int[4] -100 t/m 100
            public byte sound; //sound int //playing sounds array[99] possibility of max 99 sounds SO Sounds[0] gets first sound
        }
        #endregion

        #region Getpackage
        //Converts the bytes into an object of type Data
        /// <summary>
        /// TODO Fix that the data is setup in the singleton classes the data gets stored in our
        /// variables in this class so...
        /// </summary>
        /// <param name="data"></param>
        public void GetPackage(byte[] data)
        {
            opcode = data[0];
            if (opcode == 1)
            {
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
            else
            {
                int list_count = data[1]; //get total numbers of words
                words = new string[list_count];//create the length of an array
                int counter_word = 0; //wich word on what place in an array 
                bool new_word = true; //if there is an new word get the new length
                int length_word = 0;

                for (int i = 2; i < list_count; i++) // total list length
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
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SyncStructPackage
        {
            public byte opcode; //opcode
            public short throttle;
            public ushort current_motor;
            public ushort voltage_motor;
            public short gyro_x;
            public short gyro_y;
            public short gyro_z;
            public short accu_voltage; //millivolts
            public short accu_current; //milliamps
        }

        #endregion
    }
}