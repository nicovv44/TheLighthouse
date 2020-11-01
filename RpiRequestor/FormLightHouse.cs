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
        /// <summary>
        /// The requestor sending queries to the raspberry pi
        /// </summary>
        private readonly Requestor Requestor;
        /// <summary>
        /// The stopwatch responsible to count the time alapsed since a new result was received
        /// </summary>
        private readonly Stopwatch FreshnessStopwatch;
        /// <summary>
        /// The lock on the variable <see cref="FreshnessStopwatch"/>
        /// </summary>
        private readonly object FreshnessStopwatchLock = new object();
        /// <summary>
        /// The <see cref="BackgroundWorker"/> responsible to check the value of <see cref="FreshnessStopwatch"/> 
        /// and displays a different color if nothing is recived after waiting some time
        /// </summary>
        private readonly BackgroundWorker CheckFreshnessWorker;

        /// <summary>
        /// Constructor of the class <see cref="FormLightHouse"/>
        /// </summary>
        /// <param name="uri">The Uri of the Raspberry Pi to query</param>
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

        /// <summary>
        /// To be called when <see cref="Requestor.Result"/> changes
        /// </summary>
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

        /// <summary>
        /// <see cref="DoWorkEventHandler"/> of the <see cref="CheckFreshnessWorker"/>
        /// </summary>
        /// <param name="sender">The sender of the event: the worker <see cref="CheckFreshnessWorker"/></param>
        /// <param name="args">Data of the <see cref="DoWorkEventHandler"/></param>
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
