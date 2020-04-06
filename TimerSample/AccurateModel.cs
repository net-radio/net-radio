using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace TimerSample
{

    class AccurateModel:LoggerBase, IDisposable
    {
        AccurateTimer mTimer1, mTimer2;
        Stopwatch sw;
        Queue<TimeSpan> queue;


        public AccurateModel()
        {
            sw = new Stopwatch();
            logger.Debug("Starting timers");
            Start();
            Thread.Sleep(3000);
            Stop();
        }

        public void Start()
        {
            sw.Start();
            queue = new Queue<TimeSpan>(2000);
            int delay = 20;   // In milliseconds. 10 = 1/100th second.
            mTimer1 = new AccurateTimer(this, new Action(TimerTick1), delay);
            delay = 100;      // 100 = 1/10th second.
            mTimer2 = new AccurateTimer(this, new Action(TimerTick2), delay);
            //Stop();
        }

        public void Dispose()
        {
            Stop();
            foreach (var item in queue)
            {
                // logger.Debug(sw.Elapsed);
            }
        }

        private void Stop()
        {
            mTimer1.Stop();
            mTimer2.Stop();
        }

        private void TimerTick1()
        {
            logger.Debug(sw.ElapsedMilliseconds);

            queue.Enqueue(sw.Elapsed);
            // Debug.WriteLine(sw.ElapsedMilliseconds);
        }

        private void TimerTick2()
        {
            // Put your second timer code here!
        }


        internal void BeginInvoke(Action mAction)
        {
            mAction();
        }
    }
}
