using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;

namespace RpiRequestor
{
    class Requestor
    {
        private readonly string Uri;
        private readonly BackgroundWorker RequestWorker;
        private string _result;
        private readonly object ResultLock = new object();
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
        public delegate void ResultReady();
        /// <summary>
        /// Function called when a new result is ready
        /// </summary>
        private readonly ResultReady ResultReadyCallback;

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
                ResultReadyCallback();
                if (worker.CancellationPending)
                {
                    return;
                }
            }
        }

        private string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
