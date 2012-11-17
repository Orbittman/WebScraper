// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeDownloader.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   Defines the FakeDownloader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// The fake downloader.
    /// </summary>
    internal class FakeDownloader
        : IDownloader
    {
        /// <summary>
        /// Starts the download
        /// </summary>
        /// <param name="collection">
        /// The collection.
        /// </param>
        /// <param name="file">
        /// The name of the file to download.
        /// </param>
        /// <returns>
        /// Whether to continue or move to the next enumeration
        /// </returns>
        public async Task<bool> Download(DownloadCollection collection, string file)
        {
            await Task.Factory.StartNew(() => Thread.Sleep(5000));
            return true;
        }
    }
}
