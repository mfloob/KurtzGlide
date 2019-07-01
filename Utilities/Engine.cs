using KurtzGlide.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KurtzGlide
{
    public class Engine
    {
        private List<EngineTask> taskList = new List<EngineTask>();
        private DispatcherTimer timer = new DispatcherTimer();
        private Logger Log { get { return MainWindow.Log; } }
        private Label tickTimeLabel;

        private async Task<T> Run<T>(T x) => await Task.Run(() => x);
        private void EngineTimerTick(object sender, EventArgs e) => this.OnTick();
         
        public int TickDelay
        {
            get { return TickDelay; }
            set
            {
                this.timer.Interval = new TimeSpan(0, 0, 0, 0, value);
                Log.Log($"Tick delay set to {value} ms.");
            }
        }

        public Engine(Label tickTimeLabel, int tickDelay)
        {
            this.tickTimeLabel = tickTimeLabel;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, tickDelay);
            this.timer.Tick += EngineTimerTick;
            this.timer.Start();
        }

        private async void OnTick()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                foreach (EngineTask task in this.taskList)
                {
                    if (await Run(task.Validate()))
                        await Run(task.Execute());
                }
            }
            catch (Exception) { } //Log.Log($"Caught exception due to stale data from mirrored async task. Continuing..."); }
            tickTimeLabel.Content = $"{stopwatch.ElapsedMilliseconds} ms";
        }

        public void Add(params EngineTask[] tasks)
        {
            foreach (EngineTask task in tasks)
            {
                if (!this.taskList.Contains(task))
                {
                    this.taskList.Add(task);
                    Log.Log($"Starting {task.ToString().Substring(11)}.");
                }
            }
        }

        public void Remove(params EngineTask[] tasks)
        {
            foreach (EngineTask task in tasks)
            {
                if (this.taskList.Contains(task))
                {
                    this.taskList.Remove(task);
                    Log.Log($"Removed {task.ToString().Substring(11)}.");
                }
            }
        }
    }
}
