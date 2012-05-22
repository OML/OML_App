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

namespace OML_App.Data
{
    class Carmen
    {
        /* @author Daniel de Valk
         * Date 15-5-2012
         * 
         * Specs Packets:
         * Usage Protocol: little Endian
         * Update Frequenty: 5Hz or 10Hz
         * 
         * 
         * Tablet send:
         * Boolean speed // slow and fast driving
         * boolean calibration mode //possibility to move each engine separate
         * left int -100 t/m 100 //left wheel values
         * Right int -100 t/m 100 //Right wheel values
         * 
         * Boolean engine check (calibration)
         * engine int[4] -1000 t/m 1000 
         * 
         * playing sounds array[99] //possibility of max 99 sounds SO Sounds[0] gets first sound
         * 
         * Carmen Send:
         *
         * current int[4] mA 0 t/m 65000 //current from engine 
         * voltage int[4] mV 0 t/m 65000 //Voltage per engine
         * Temperature int[4] .1 degrees //temperature in .1 from four engines
         * gyro Vector(X, Y, Z) //the acceleration from the vehicle
         * throttle int[4] %o //speed from each engine
         * current int[4] mA 0 t/m 65000 //current from accu
         * voltage int[4] mV 0 t/m 65000 //voltage accu 
         *
         *
         *
         */
        //Sensors
        public Sensor Eng1_Temp;
        public Sensor Eng2_Temp;
        public Sensor Eng3_Temp;
        public Sensor Eng4_Temp;
        public Sensor Eng1_Amperage;
        public Sensor Eng2_Amperage;
        public Sensor Eng3_Amperage;
        public Sensor Eng4_Amperage;
        public Sensor Eng1_Voltage;
        public Sensor Eng2_Voltage;
        public Sensor Eng3_Voltage;
        public Sensor Eng4_Voltage;
        public Sensor Eng1_Throttle;
        public Sensor Eng2_Throttle;
        public Sensor Eng3_Throttle;
        public Sensor Eng4_Throttle;

        public Sensor Accu_Temp;
        public Sensor Accu_Voltage;
        public Sensor Accu_Amperage;

        public Sensor Helling;
        public Sensor SteerDirection;

        public Carmen()
        {
            Eng1_Temp = new Sensor("Engine 1 Temp", "Eng1_Temp", "C", 0, 110);
            Eng2_Temp = new Sensor("Engine 2 Temp", "Eng2_Temp", "C", 0, 110);
            Eng3_Temp = new Sensor("Engine 3 Temp", "Eng3_Temp", "C", 0, 110);
            Eng4_Temp = new Sensor("Engine 4 Temp", "Eng4_Temp", "C", 0, 110);
            Eng1_Amperage = new Sensor("Engine 1 Amperage", "Eng1_Amperage", "mA", 0, 65000);
            Eng2_Amperage = new Sensor("Engine 2 Amperage", "Eng2_Amperage", "mA", 0, 65000);
            Eng3_Amperage = new Sensor("Engine 3 Amperage", "Eng3_Amperage", "mA", 0, 65000);
            Eng4_Amperage = new Sensor("Engine 4 Amperage", "Eng4_Amperage", "mA", 0, 65000);
            Eng1_Voltage = new Sensor("Engine 1 Voltage", "Eng1_Voltage", "mV", 0, 65000);
            Eng2_Voltage = new Sensor("Engine 2 Voltage", "Eng2_Voltage", "mV", 0, 65000);
            Eng3_Voltage = new Sensor("Engine 3 Voltage", "Eng3_Voltage", "mV", 0, 65000);
            Eng4_Voltage = new Sensor("Engine 4 Voltage", "Eng4_Voltage", "mV", 0, 65000);
            Eng1_Throttle = new Sensor("Engine 1 Throttle", "Eng1_Throttle", "%o", 0, 1000);
            Eng2_Throttle = new Sensor("Engine 2 Throttle", "Eng2_Throttle", "%o", 0, 1000);
            Eng3_Throttle = new Sensor("Engine 3 Throttle", "Eng3_Throttle", "%o", 0, 1000);
            Eng4_Throttle = new Sensor("Engine 4 Throttle", "Eng4_Throttle", "%o", 0, 1000);

            Accu_Temp = new Sensor("Accu Temp", "Accu_Temp", "C", 0, 110);
            Accu_Voltage = new Sensor("Accu Voltage", "Accu_Voltage", "mV", 0, 65000);
            Accu_Amperage = new Sensor("Accu Amperage", "Accu_Amperage", "mA", 0, 65000);

            Helling = new Sensor("Helling", "Helling", "°", -1800, 1800);
            SteerDirection = new Sensor("Steering Direction", "SteerDirection", "°", 0, 360);
        }
    }
}