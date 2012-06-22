using System;
using System.Collections.Generic;
using System.Text;
using OML_App.Setting;

namespace OML_App.Data
{
    public sealed class Receive_Singleton
    {
        private static volatile Receive_Singleton instance;
        private static object syncRoot = new Object();
       
        public Session Current_ses;        

        private Receive_Singleton() { }


        public string Track1 = "not available";
        public string Track2 = "not available";
        public string Track3 = "not available";
        public string Track4 = "not available";
        public string Track5 = "not available";
        public string Track6 = "not available";
        public string Track7 = "not available";
        public string Track8 = "not available";
        public string Track9 = "not available";
        

        public void init()
        {
            Current_ses = new Session("OML Carmen");
            
            //4x motor Voltages            
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_0", "M0V", "mV", 0, 30000));
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_1", "M1V", "mV", 0, 30000));
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_2", "M2V", "mV", 0, 30000));
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_3", "M3V", "mV", 0, 30000));

            //4x motor Current
            Current_ses.AddSensorToArray(new Sensor("Current_motor_0", "M0A", "mA", 0, 3000));
            Current_ses.AddSensorToArray(new Sensor("Current_motor_1", "M1A", "mA", 0, 3000));
            Current_ses.AddSensorToArray(new Sensor("Current_motor_2", "M2A", "mA", 0, 3000));
            Current_ses.AddSensorToArray(new Sensor("Current_motor_3", "M3A", "mA", 0, 3000));

            //4x motor Temperature
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_0", "M0T", "mC", 0, 10000));
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_1", "M1T", "mC", 0, 10000));
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_2", "M2T", "mC", 0, 10000));
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_3", "M3T", "mC", 0, 10000));

            //4x motor Throttle
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_0", "M0Th", "‰", 0, 1000));
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_1", "M1Th", "‰", 0, 1000));
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_2", "M2Th", "‰", 0, 1000));
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_3", "M3Th", "‰", 0, 1000));
            
            //Accu values 0
            Current_ses.AddSensorToArray(new Sensor("Voltage_accu_0", "A0V", "mV", 0, 30000));
            Current_ses.AddSensorToArray(new Sensor("Current_accu_1", "A0A", "mA", 0, 20000));
            Current_ses.AddSensorToArray(new Sensor("Temperature_accu_2", "A0T", "mC", 0, 10000));

            //Accu values 1 wont be used at first hand (no support from hardware)
            Current_ses.AddSensorToArray(new Sensor("Voltage_accu_0", "A1V", "mV", 0, 30000));
            Current_ses.AddSensorToArray(new Sensor("Current_accu_1", "A1A", "mA", 0, 20000));
            Current_ses.AddSensorToArray(new Sensor("Temperature_accu_2", "A1T", "mC", 0, 10000));

            //Gyro
            Current_ses.AddSensorToArray(new Sensor("Gyro_x", "G0X", "G", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Gyro_y", "G0Y", "G", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Gyro_z", "G0Z", "G", 0, 100));
        }

        /// <summary>
        /// End Session and Close Connection!
        /// </summary>
        public void EndSession()
        {            
            Current_ses.EndTime = DateTime.Now;
        }

        public static Receive_Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Receive_Singleton();
                    }
                }

                return instance;
            }
        }
    }
}
