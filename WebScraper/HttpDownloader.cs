// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpDownloader.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   The http downloader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// The http downloader.
    /// </summary>
    public class HttpDownloader
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
            bool returnValue = false;
            string remoteFile = Path.Combine(collection.RemotePath, file);
            string localFile = Path.Combine(collection.LocalPath, file);
            var request = (HttpWebRequest)WebRequest.Create(remoteFile);

            request.Credentials = new NetworkCredential(collection.UserName, collection.Password);

            Stream reader = null;
            FileStream fileStream = null;
            try
            {
                int startPosition = FileLength(localFile);

                request.AddRange(startPosition);
                var response = await request.GetResponseAsync() as HttpWebResponse;

                if (response != null && (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.PartialContent))
                {
                    Console.WriteLine("Downloading {0} ({1} bytes)", file, response.ContentLength);
                    reader = response.GetResponseStream();

                    fileStream = new FileStream(localFile, FileMode.Append, FileAccess.Write, FileShare.None);

                    if (reader != null)
                    {
                        // It will store the current number of bytes we retrieved from the server
                        int bytesSize;

                        // A buffer for storing and writing the data retrieved from the server
                        var downBuffer = new byte[2048];
                        int offset = 0;
                        Console.WriteLine();
                        int totalSize = (int)fileStream.Length + (int)response.ContentLength;

                        // Loop through the buffer until the buffer is empty
                        while ((bytesSize = reader.Read(downBuffer, offset, downBuffer.Length)) > 0)
                        {
                            fileStream.Write(downBuffer, 0, bytesSize);
                            offset = 0;
                        }

                        Console.WriteLine();
                        Console.WriteLine("Success: {0}", remoteFile);
                        returnValue = true;
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("File error: {0}", remoteFile);
                Console.Write(ex.Message);
                returnValue = true;
            }
            catch (WebException ex)
            {
                Console.WriteLine("Skipping file: {0}", file);
                returnValue = ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed: {0}", remoteFile);
                Console.Write(ex.Message);
                returnValue = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the length of the file at the given path
        /// </summary>
        /// <param name="path">
        /// The path to the file.
        /// </param>
        /// <returns>
        /// The byte length of the file.
        /// </returns>
        private static int FileLength(string path)
        {
            try
            {
                using (var file = new FileStream(path, FileMode.Open))
                {
                    return (int)file.Length;
                }
            }
            catch (FileNotFoundException)
            {
                return 0;
            }
        }
    }
}
