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

        public void init()
        {
            Current_ses = new Session("OML Carmen");
            
            //4x motor Voltages            
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_0", "M0V", "V", 0, 50));
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_1", "M1V", "V", 0, 50));
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_2", "M2V", "V", 0, 50));
            Current_ses.AddSensorToArray(new Sensor("Voltage_motor_3", "M3V", "V", 0, 50));

            //4x motor Current
            Current_ses.AddSensorToArray(new Sensor("Current_motor_0", "M0A", "A", 0, 25));
            Current_ses.AddSensorToArray(new Sensor("Current_motor_1", "M1A", "A", 0, 25));
            Current_ses.AddSensorToArray(new Sensor("Current_motor_2", "M2A", "A", 0, 25));
            Current_ses.AddSensorToArray(new Sensor("Current_motor_3", "M3A", "A", 0, 25));

            //4x motor Temperature
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_0", "M0T", "C", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_1", "M1T", "C", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_2", "M2T", "C", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Temperature_motor_3", "M3T", "C", 0, 100));

            //4x motor Throttle
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_0", "M0Th", "%", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_1", "M1Th", "%", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_2", "M2Th", "%", 0, 100));
            Current_ses.AddSensorToArray(new Sensor("Throttle_motor_3", "M3Th", "%", 0, 100));
            
            //Accu values 0
            Current_ses.AddSensorToArray(new Sensor("Voltage_accu_0", "A0V", "V", 0, 50));
            Current_ses.AddSensorToArray(new Sensor("Current_accu_1", "A0A", "A", 0, 25));
            Current_ses.AddSensorToArray(new Sensor("Temperature_accu_2", "A0T", "C", 0, 100));

            //Accu values 1 wont be used at first hand (no support from hardware)
            Current_ses.AddSensorToArray(new Sensor("Voltage_accu_0", "A1V", "V", 0, 50));
            Current_ses.AddSensorToArray(new Sensor("Current_accu_1", "A1A", "A", 0, 25));
            Current_ses.AddSensorToArray(new Sensor("Temperature_accu_2", "A1T", "C", 0, 100));

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
            //Current_ses.EndTime = DateTime.Now;
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
