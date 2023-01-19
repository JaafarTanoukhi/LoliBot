using System;
using System.Timers;
using System.Collections.Generic;

namespace LoliBotNew.Modules
{
    public class Worker
    {
        Timer aTimer;
        private static List<ulong> workers = new List<ulong>();
        public ulong Id { get; private set; }

        public Worker(ulong Id)
        {
            this.Id = Id;
            workers.Add(Id);
            SetTimer();
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(30000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            workers.Remove(Id);
            aTimer.Dispose();
        }

        public static bool exists(ulong Id)
        {
            if (workers.Contains(Id)) return true;
            return false;
        }
    }
}
