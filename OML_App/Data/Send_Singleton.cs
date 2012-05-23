using System;
using System.Collections.Generic;
using System.Text;

namespace OML_App.Data
{
    public sealed class Send_Singleton
    {
        private static volatile Send_Singleton instance;
        private static object syncRoot = new Object();

        private Send_Singleton() { }

        //Sending
        public int speed { get; set; } //Boolean speed 0 slow and 1 fast driving
        public int left { get; set; }//left int -100 t/m 100 //left wheel values
        public int right { get; set; } //Right int -100 t/m 100 //Right wheel values
        public int Calibration_mode { get; set; } //boolean calibration mode //possibility to move each engine separate
        public int engine0 { get; set; } //engine int[4] -100 t/m 100
        public int engine1 { get; set; } //engine int[4] -100 t/m 100
        public int engine2 { get; set; } //engine int[4] -100 t/m 100
        public int engine3 { get; set; } //engine int[4] -100 t/m 100
        public int sound { get; set; } //sound int //playing sounds array[99] possibility of max 99 sounds SO Sounds[0] gets first sound
        public int throttle { get; set; }
        public int voltage { get; set; }
        public int current { get; set; }
        public int temperature { get; set; }
        public int Gyro { get; set; }


        public static Send_Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Send_Singleton();
                        }

                    }
                }
                return instance;
            }
        }
    }
}
