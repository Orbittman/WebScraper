namespace WebScraper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading;

    public class DownloadManager
    {

        #region Fields
        
        private List<IEnumerable<string>> _enumerations;
        private string _path;
        private string _directory;
        private string formatString;
        private UpdateStatusHandler _updateStatus;
        private int _downloadPause;

        #endregion

        #region Cosntructors
        
        public DownloadManager(string path, string dynamicFormat, string directory, List<IEnumerable<string>> enumerators, int downloadPause, UpdateStatusHandler updateStatus)
        {
            this._path = path;
            this.formatString = dynamicFormat;
            this._directory = directory;
            this._enumerations = enumerators;
            this._downloadPause = downloadPause;
            this._updateStatus = updateStatus;
            this.Enumerate(String.Empty, this.Download, 1);
        }

        #endregion


        private bool Enumerate(string currentStrings, HandleEnumerations handler, int itteration)
        {
            if (this._enumerations.Count >= itteration)
            {
                foreach (string value in this._enumerations[itteration - 1])
                {
                    if (!this.Enumerate(currentStrings + (currentStrings == String.Empty ? String.Empty : ",") + value, handler, itteration + 1))
                    {
                        break;
                    }
                }
            }
            else
            {
                if (!handler(currentStrings.Split(',')))
                {
                    Console.WriteLine(currentStrings);
                    return false;
                }

                if (this._downloadPause > 0)
                {
                    Console.WriteLine("Pausing {0}s", this._downloadPause);
                    Thread.Sleep(this._downloadPause * 1000);
                }
            }

            return true;
        }

        /// <summary>
        /// Starts the download
        /// </summary>
        /// <param name="values">
        /// The values to use to make the url
        /// </param>
        /// <returns>
        /// Whether to continue or move to the next enumeration
        /// </returns>
        private bool Download(string[] values)
        {
            bool returnValue = false;
            string file = String.Format(this.formatString, values);
            string path = this._path + file;
            var request = (HttpWebRequest)WebRequest.Create(path);
            request.Credentials = new NetworkCredential("", "");

                Stream reader = null;
                FileStream fileStream = null;
                try
                {
                    string downloadPath = this._directory + String.Format(this.formatString, values);
                    int startPosition = this.FileLength(downloadPath);

                    request.AddRange(startPosition);
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.PartialContent)
                    {
                        Console.WriteLine(String.Format("Downloading {0} ({1} bytes)", file, response.ContentLength));
                        reader = response.GetResponseStream();

                        fileStream = new FileStream(downloadPath, FileMode.Append, FileAccess.Write, FileShare.None);

                        if (reader != null)
                        {
                            // It will store the current number of bytes we retrieved from the server
                            var bytesSize = 0;

                            // A buffer for storing and writing the data retrieved from the server
                            var downBuffer = new byte[2048];
                            int offset = 0;
                            Console.WriteLine();
                            int totalSize = (int)fileStream.Length + (int)response.ContentLength;

                            // Loop through the buffer until the buffer is empty
                            while ((bytesSize = reader.Read(downBuffer, offset, downBuffer.Length)) > 0)
                            {
                                fileStream.Write(downBuffer, 0, bytesSize);
                                this._updateStatus((int)fileStream.Length, totalSize);
                                offset = 0;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Success: " + path);
                            returnValue = true;
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("File error: " + path);
                    Console.Write(ex.Message);
                    returnValue = true;
                }
                catch (WebException ex)
                {
                    Console.WriteLine(String.Format("Skipping file: {0}", file));
                    returnValue = ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed: " + path);
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

        private int FileLength(string path)
        {
            try
            {
                using(var file = new FileStream(path, FileMode.Open))
                {
                    return (int)file.Length;
                }
            }
            catch(FileNotFoundException ex)
            {
                return 0;
            }
        }
    }
}
