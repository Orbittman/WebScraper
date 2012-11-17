// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDownloader.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   Defines the IDownloader type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System.Threading.Tasks;

    /// <summary>
    /// The Downloader interface.
    /// </summary>
    public interface IDownloader
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
        Task<bool> Download(DownloadCollection collection, string file);
    }
}