using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace TheLighthouse
{
    /// <summary>
    /// Class used to send queries to the Raspberry Pi, store the result, and inform the instiating class the result has changed
    /// </summary>
    class Requestor
    {
        /// <summary>
        /// The URI of the Raspberry Pi to connect to
        /// </summary>
        private readonly string Uri;
        /// <summary>
        /// The request period used in between two GET requests
        /// </summary>
        private readonly int RequestPeriod;
        /// <summary>
        /// The <see cref="BackgroundWorker"/> used to send the request to the Raspberry Pi and store the result
        /// </summary>
        private readonly BackgroundWorker RequestWorker;
        /// <summary>
        /// The private member (field) containing the result obtained from the Raspberry Pi
        /// </summary>
        private string _result;
        /// <summary>
        /// Lock on the private memeber <see cref="_result"/> to avoid concurent accent when trying to read a result if a result bein retrieved from the network
        /// </summary>
        private readonly object ResultLock = new object();
        /// <summary>
        /// The public member (propety) containing the result obtained from the Raspberry Pi, and accessible from the outside of the class
        /// </summary>
        public string Result 
        { 
            get 
            {
                lock (ResultLock)
                {
                    return _result;
                }
            } 
            private set 
            {
                lock (ResultLock)
                {
                    _result = value;
                }
            } 
        }
        /// <summary>
        /// Delegate of the funnction called wh
        /// </summary>
        public delegate void ResultReady();
        /// <summary>
        /// Function called when a new result is ready (result changed)
        /// </summary>
        private readonly ResultReady ResultReadyCallback;

        /// <summary>
        /// Constructor of the class <see cref="Requestor"/>
        /// </summary>
        /// <param name="uri">URI of the Raspberry Pi to connect to (such as "http://xxx.xxx.xxx.xxx")</param>
        /// <param name="resultReadyCallback">Function of type <see cref="ResultReady"/>, called when a new result is received</param>
        public Requestor(string uri, int requestPeriod, ResultReady resultReadyCallback = null)
        {
            Uri = uri;
            RequestPeriod = requestPeriod;
            Result = default;
            RequestWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            RequestWorker.DoWork += new DoWorkEventHandler(RequestWorker_DoWork);
            ResultReadyCallback = resultReadyCallback;
        }

        /// <summary>
        /// Starts requesting the Raspberry Pi for result by running the background worker
        /// </summary>
        public void StartRequesting()
        {
            if (!RequestWorker.IsBusy)
            {
                RequestWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Stops requesting the Raspberry Pi for result by cancelling the background worker
        /// </summary>
        public void StopRequesting()
        {
            if (RequestWorker.IsBusy)
            {
                RequestWorker.CancelAsync();
                while (RequestWorker.IsBusy) System.Threading.Thread.Sleep(10);
            }
        }

        /// <summary>
        /// The methode handling the <see cref="System.ComponentModel.BackgroundWorker.DoWork"/> event
        /// <see cref="DoWorkEventHandler"/> of the <see cref="BackgroundWorker"/> <see cref="RequestWorker"/> <br/>
        /// It gets a result from the Raspberry Pi and calls the function <see cref="ResultReadyCallback"/> if the result is not empty
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="System.ComponentModel.DoWorkEventArgs"/> that contains the event data.</param>
        private void RequestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            static bool ResultIsValid(bool success, string result) { return success && result.Length>0; }
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                string result = "";
                bool success = false;
                lock (ResultLock)
                {
                    success = Get(Uri, out result);
                    if (ResultIsValid(success, result))
                    {
                        Result = result;
                    }
                }
                if (ResultIsValid(success, result))
                {
                    ResultReadyCallback(); // This cannot be in the lock because it accesses Result via a lock, so it woud be a mutex
                }
                System.Threading.Thread.Sleep(RequestPeriod);
                if (worker.CancellationPending)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Send a GET request to the URI provided in argument output the response as a string
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="responseString">OUTPUT: the string of the response if properly retrieved, an empty string otherwise</param>
        /// <returns><code>true</code> if the response is properly retrieved, <code>false</code> otherwise</returns>
        private bool Get(string uri, out string responseString)
        {
            responseString = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);
                responseString = reader.ReadToEnd();
            }
            catch(Exception exception)
            {
                Trace.WriteLine($"Exception in Requestor.Get(): '{exception.Message}'");
                return false;
            }
            return true;
        }
    }
}
