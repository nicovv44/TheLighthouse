using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RpiRequestor
{
    public partial class FormLightHouse : Form
    {
        private readonly Requestor Requestor;
        private readonly Stopwatch FreshnessStopwatch;
        private readonly object FreshnessStopwatchLock = new object();
        private readonly BackgroundWorker CheckFreshnessWorker;
        public FormLightHouse(string uri)
        {
            // Initialise non GUI
            Requestor = new Requestor(uri, UpdateGuiWithNewResult);
            FreshnessStopwatch = new Stopwatch();
            CheckFreshnessWorker = new BackgroundWorker();
            CheckFreshnessWorker.DoWork += new DoWorkEventHandler(CheckFreshnessWorker_DoWork);

            // Initialize GUI
            InitializeComponent();

            //Start actions
            Requestor.StartRequesting();
            FreshnessStopwatch.Start();
            CheckFreshnessWorker.RunWorkerAsync();
        }

        void UpdateGuiWithNewResult()
        {
            Trace.WriteLine($"Result: {Requestor.Result}");
            string result = Requestor.Result;
            switch (result)
            {
                case "{\"Button\": 0}":
                    panelColourSign.BackColor = Color.Green;
                    RestartFreshnessStopWatch();
                    break;
                case "{\"Button\": 1}":
                    panelColourSign.BackColor = Color.Red;
                    RestartFreshnessStopWatch();
                    break;
                default:
                    panelColourSign.BackColor = Color.Black;
                    RestartFreshnessStopWatch();
                    break;
            }
        }

        private void CheckFreshnessWorker_DoWork(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                long timeElapsedSinceLastResultRefresh;
                lock (FreshnessStopwatchLock)
                {
                    timeElapsedSinceLastResultRefresh = FreshnessStopwatch.ElapsedMilliseconds;
                }

                if (timeElapsedSinceLastResultRefresh > 1000) // If we don't refresh the resut in 1 second, we go transparent
                {
                    panelColourSign.BackColor = Color.Transparent;
                    RestartFreshnessStopWatch();
                }

                Thread.Sleep(100);

                if (worker.CancellationPending)
                {
                    return;
                }
            }
        }

        private void RestartFreshnessStopWatch()
        {
            lock (FreshnessStopwatchLock)
            {
                FreshnessStopwatch.Restart();
            }
        }
    }
}
