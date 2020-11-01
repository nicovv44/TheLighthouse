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
        public Requestor(string uri, ResultReady resultReadyCallback = null)
        {
            Uri = uri;
            Result = default;
            RequestWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            RequestWorker.DoWork += new DoWorkEventHandler(RequestWorker_DoWork);
            ResultReadyCallback = resultReadyCallback;
        }

        public void StartRequesting()
        {
            if (!RequestWorker.IsBusy)
            {
                RequestWorker.RunWorkerAsync();
            }
        }

        public void StopRequesting()
        {
            if (RequestWorker.IsBusy)
            {
                RequestWorker.CancelAsync();
            }
        }

        private void RequestWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                lock (ResultLock)
                {
                    Result = Get(Uri);
                }
                if(Result.Length>0) ResultReadyCallback();
                if (worker.CancellationPending)
                {
                    return;
                }
            }
        }

        private string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using Stream stream = response.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch(Exception exception)
            {
                Trace.WriteLine($"Exception in Requestor.Get(): '{exception.Message}'");
                return "";
            }
        }
    }
}
