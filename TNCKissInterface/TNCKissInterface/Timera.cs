using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace TNCKissInterface
{
    //
    // Wrapper class for the timers we use in this DLL.
    //
    // Copyright 2008, 2009 - Peter R. Woods (N6PRW)
    //
    public class Timer
    {
        Random timeRandom = new Random();
        System.Timers.Timer timerT;
        String tName;

        public Boolean Enabled
        {
            get { return timerT.Enabled; }
        }

        public Timer(ElapsedEventHandler eHandler, String name)
        {
            timerT = new System.Timers.Timer();
            timerT.AutoReset = true;
            timerT.Enabled = false;
            timerT.Elapsed += timerT_Elapsed;
            timerT.Elapsed += eHandler;
            tName = name;
        }

        void timerT_Elapsed(object sender, ElapsedEventArgs e)
        {
        }
        public void SetTime(Int32 time)
        {
            SetTime(time, false);
        }

        public void SetTime(Int32 time, Boolean variable)
        {
            if (variable)
            {
                //
                // Add up to an attitional 50% to the time value to help avoid channel collisions
                //
                time += timeRandom.Next(time >> 1);
            }
            Support.DbgPrint("Timer " + tName + " interval set to  msec: " + time.ToString());
            timerT.Interval = Convert.ToDouble(time);
        }

        public void Stop()
        {
            timerT.Stop();
            Support.DbgPrint("Timer " + tName + " Stopped");
        }

        public void Start()
        {
            Support.DbgPrint("Timer " + tName + " Started");
            timerT.Start();
        }

        public void Restart()
        {
            Support.DbgPrint("Timer " + tName + " Restarted");
            Stop();
            Start();
        }
    }
}
