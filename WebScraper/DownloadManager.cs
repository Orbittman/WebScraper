// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadManager.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   The download manager class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The download manager class.
    /// </summary>
    public class DownloadManager
    {
        #region Fields

        /// <summary>
        /// The download collections.
        /// </summary>
        private readonly List<DownloadCollection> _collections;

        /// <summary>
        /// The processed item counter.
        /// </summary>
        private int _counter;

        #endregion

        #region Cosntructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadManager"/> class.
        /// </summary>
        /// <param name="collections">
        /// The collections.
        /// </param>
        public DownloadManager(List<DownloadCollection> collections)
        {
            _collections = collections;
            _collections = collections;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the downloader to use for the downloading action.
        /// </summary>
        public IDownloader Downloader { get; set; }

        /// <summary>
        /// Gets or sets the logging action.
        /// </summary>
        public Action<string> LoggingAction { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Start downloading the collections.
        /// </summary>
        public void StartDownloading()
        {
            if (Downloader == null)
                        {
                            throw new NullReferenceException("There was no downloader intitalised");
                        }

            ProcessCollections();
        }

        /// <summary>
        /// The log response invoker.
        /// </summary>
        /// <param name="response">
        /// The response.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        private void InvokeResponse(string response, params object[] parameters)
        {
            if (LoggingAction != null)
            {
                LoggingAction(string.Format(response, parameters));
            }
        }

        /// <summary>
        /// The process collections method.
        /// </summary>
        private void ProcessCollections()
        {
            _counter = _collections.SelectMany(c => c).Count();
            using (var semaphore = new SemaphoreSlim(5))
            {
                foreach (var collection in _collections)
                {
                    ProcessCollection(collection, semaphore);
                }

                while (_counter > 0)
                {
                }
            }
        }

        /// <summary>
        /// Process a <see cref="DownloadCollection"/>  collection.
        /// </summary>
        /// <param name="collection">
        /// The collection to process
        /// </param>
        /// <param name="semaphore">The semaphore to limit the concurrent tasks</param>
        private void ProcessCollection(DownloadCollection collection, SemaphoreSlim semaphore)
        {
            foreach (var downloadItem in collection)
            {
                string item = downloadItem;
                Task.Factory.StartNew(
                    async () =>
                    {
                        semaphore.Wait();
                        Debug.WriteLine("Debugging");
                        InvokeResponse("Downloading {0}", item);
                        await Downloader.Download(collection, item);
                        InvokeResponse("Releasing {0}", item);
                        Interlocked.Decrement(ref this._counter);
                        semaphore.Release();
                    });

                if (collection.Pause > 0)
                {
                    InvokeResponse("Waiting {0} seconds after {1}", collection.Pause, item);
                    Thread.Sleep(collection.Pause * 1000);
                }
            }
        }

        #endregion
    }
}
