using System;
using System.Timers;

namespace DateAndTime
{
    class LocalTimer
    {
        public DateTime endTime { get; set; }
        public Timer timer { get; set; }
        private bool _active;
        private int intervals; //counts how many intervals have elapsed
        private int indicatorIntervals; //the number of intervals between indicator dots
        public bool active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                if (value)
                {
                    timer.Elapsed += Timer_Elapsed;
                }
                else
                {
                    timer.Elapsed -= Timer_Elapsed;
                }
            }
        }
        public LocalTimer(DateTime setEndTime, int indicatorIntervalsDelay = 5)
        {
            endTime = setEndTime;
            timer = new Timer
            {
                Interval = 1000 //the timer event will fire each second (1000ms)
            };
            intervals = 0;
            indicatorIntervals = indicatorIntervalsDelay;
            active = true;
            timer.Start();
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            intervals++;
            DateTime currentTime = DateTime.Now;
            if (currentTime.Date == endTime.Date && currentTime.Hour == endTime.Hour && 
                currentTime.Minute == endTime.Minute && currentTime.Second == endTime.Second)
            {
                timer.Stop();
                active = false;
                Console.WriteLine("Time's up!");
                Console.WriteLine("It is now: " + endTime);
                for (int i = 0; i < 10; i++)
                {
                    Console.Beep(1000, 200);
                }
            }
            else
            {
                if (intervals % indicatorIntervals == 0) Console.Write(".");
            }
        }
    }
}
