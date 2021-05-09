using System;
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
        Object syncLock = new Object();
        System.Timers.Timer timerT;
        String tName;

        public Boolean Enabled
        {
            get { return timerT.Enabled; }
        }

        public String timerName
        {
            get { return tName; }
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
            lock (syncLock)
            {
                if (variable)
                {
                    //
                    // Add up to an attitional 50% to the time value to help avoid channel collisions
                    //
                    time += timeRandom.Next(time >> 1);
                }
                timerT.Interval = Convert.ToDouble(time);
            }
            Support.DbgPrint("Timer " + tName + " interval set to  msec: " + time.ToString());

        }

        public void Stop()
        {
            lock (syncLock)
            {
                timerT.Stop();
            }
            Support.DbgPrint("Timer " + tName + " Stopped");
        }

        public void Start()
        {
            lock (syncLock)
            { 
                timerT.Start();
            }
            Support.DbgPrint("Timer " + tName + " Started");
        }

        public void Restart()
        {
            lock (syncLock)
            {
                Stop();
                Start();
            }
            Support.DbgPrint("Timer " + tName + " Restarted");
        }
    }
}
