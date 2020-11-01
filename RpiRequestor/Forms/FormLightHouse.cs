using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TheLighthouse
{
    /// <summary>
    /// Form of the light house remaining laways on top and indicates info depending on the color of its <see cref="panelColourSign"/>
    /// </summary>
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
            Requestor = new Requestor(uri, ResultReadyCallback);
            FreshnessStopwatch = new Stopwatch();
            CheckFreshnessWorker = new BackgroundWorker();
            CheckFreshnessWorker.DoWork += new DoWorkEventHandler(CheckFreshnessWorker_DoWork);
            
            // Initialize GUI
            InitializeComponent();
            TopMost = true;

            //Start actions
            Requestor.StartRequesting();
            FreshnessStopwatch.Start();
            CheckFreshnessWorker.RunWorkerAsync();
        }

        /// <summary>
        /// To be called when <see cref="Requestor.Result"/> changes
        /// </summary>
        void ResultReadyCallback()
        {
            Trace.WriteLine($"Result: {Requestor.Result}"); 
            RestartFreshnessStopWatch();
            UpdateGuiWithNewResult(Requestor.Result);            
        }

        /// <summary>
        /// Update the GUI components depending on the new result obtained from the Raspberry Pi
        /// </summary>
        /// <param name="result">The strinng of the result of the request sent to the Raspberry Pi</param>
        private void UpdateGuiWithNewResult(string result)
        {
            panelColourSign.BackColor = result switch // TODO: that is the place to edit when the API on the Raspberry Pi changes (to consider ditance for instance)
            {
                "{\"Button\": 0}" => Color.Green,
                "{\"Button\": 1}" => Color.Red,
                _ => Color.Black,
            };
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

                if (timeElapsedSinceLastResultRefresh > Constant.TimeoutFreshnessMs) 
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

        /// <summary>
        /// This function is called when a new result a received so a to reset the stopwatch
        /// </summary>
        private void RestartFreshnessStopWatch()
        {
            lock (FreshnessStopwatchLock)
            {
                FreshnessStopwatch.Restart();
            }
        }
    }
}
