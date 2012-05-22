using System;
using System.Collections.Generic;
using System.Text;

namespace OML_App.Data
{
    public sealed class Singleton
    {
        private static volatile Singleton instance;
        private static object syncRoot = new Object();

        private Singleton() {}


        //incoming
        public int throttle_motor_0 { get; set; }
        public int throttle_motor_1 { get; set; }
        public int throttle_motor_2 { get; set; }
        public int throttle_motor_3 { get; set; }

        public int Current_motor_0 { get; set; }
        public int Current_motor_1 { get; set; }
        public int Current_motor_2 { get; set; }
        public int Current_motor_3 { get; set; }

        public int Voltage_motor_0 { get; set; }
        public int Voltage_motor_1 { get; set; }
        public int Voltage_motor_2 { get; set; }
        public int Voltage_motor_3 { get; set; }

        public  int Gyro


        public int Y { get; set; }
        public int Y { get; set; }
        public int Y { get; set; }
        public int Y { get; set; }
        public int Y { get; set; }
        public int Y { get; set; }
        public int Y { get; set; }
        public int Y { get; set; }



        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Singleton();
                    }
                }

                return instance;
            }
        }
    }
}
