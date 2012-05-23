﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OML_App.Data
{
    public sealed class Receive_Singleton
    {
        private static volatile Receive_Singleton instance;
        private static object syncRoot = new Object();

        //4x motor Voltages
        private Sensor M0V;
        private Sensor M1V;
        private Sensor M2V;
        private Sensor M3V;

        //4x motor Current
        private Sensor M0A;
        private Sensor M1A;
        private Sensor M2A;
        private Sensor M3A;

        //4x motor Temperature
        private Sensor M0T;
        private Sensor M1T;
        private Sensor M2T;
        private Sensor M3T;

        //4x motor Throttle
        private Sensor M0Th;
        private Sensor M1Th;
        private Sensor M2Th;
        private Sensor M3Th;

        //Accu values 0
        private Sensor A0V;
        private Sensor A0A;
        private Sensor A0T;

        //Accu values 1 wont be used at first hand (no support from hardware)
        private Sensor A1V;
        private Sensor A1A;
        private Sensor A1T;

        //Gyro
        private Sensor G0X;
        private Sensor G0Y;
        private Sensor G0Z;


        private Receive_Singleton() { }

        public void init()
        {
            //4x motor Voltages
            M0V = new Sensor("Voltage_motor_0", "M0V", "V", 0, 50);
            M1V = new Sensor("Voltage_motor_1", "M1V", "V", 0, 50);
            M2V = new Sensor("Voltage_motor_2", "M2V", "V", 0, 50);
            M3V = new Sensor("Voltage_motor_3", "M3V", "V", 0, 50);

            //4x motor Current
            M0A = new Sensor("Current_motor_0", "M0A", "A", 0, 25);
            M1A = new Sensor("Current_motor_1", "M1A", "A", 0, 25);
            M2A = new Sensor("Current_motor_2", "M2A", "A", 0, 25);
            M3A = new Sensor("Current_motor_3", "M3A", "A", 0, 25);

            //4x motor Temperature
            M0T = new Sensor("Temperature_motor_0", "M0T", "C", 0, 100);
            M1T = new Sensor("Temperature_motor_1", "M1T", "C", 0, 100);
            M2T = new Sensor("Temperature_motor_2", "M2T", "C", 0, 100);
            M3T = new Sensor("Temperature_motor_3", "M3T", "C", 0, 100);

            //4x motor Throttle
            M0Th = new Sensor("Throttle_motor_0", "M0Th", "%", 0, 100);
            M1Th = new Sensor("Throttle_motor_1", "M1Th", "%", 0, 100);
            M2Th = new Sensor("Throttle_motor_2", "M2Th", "%", 0, 100);
            M3Th = new Sensor("Throttle_motor_3", "M3Th", "%", 0, 100);

            //Accu values 0
            A0V = new Sensor("Voltage_accu_0", "A0V", "V", 0, 50);
            A0A = new Sensor("Current_accu_1", "A0A", "A", 0, 25);
            A0T = new Sensor("Temperature_accu_2", "A0T", "C", 0, 100);

            //Accu values 1 wont be used at first hand (no support from hardware)
            A1V = new Sensor("Voltage_accu_0", "A1V", "V", 0, 50);
            A1A = new Sensor("Current_accu_1", "A1A", "A", 0, 25);
            A1T = new Sensor("Temperature_accu_2", "A1T", "C", 0, 100);

            //Gyro
            G0X = new Sensor("Gyro_x", "G0X", "G", 0, 100);
            G0Y = new Sensor("Gyro_y", "G0Y", "G", 0, 100);
            G0Z = new Sensor("Gyro_z", "G0Z", "G", 0, 100);

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
