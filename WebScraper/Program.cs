using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace WebScraper
{
    /// <summary>
    /// The delegate for handling enumeration recursion
    /// </summary>
    /// <param name="strings">
    /// The strings to enumerate
    /// </param>
    /// <returns>Value indicating whether the enumeration path has reached the end</returns>
    public delegate bool HandleEnumerations(string[] strings);

    class Program
    {
        private static List<IEnumerable<string>> _enumerations;
        private static string _path = "";
        private static string _formatString = "{0}{1}.wmv";
        private static Thread _downloadManager;
        private static int _downloadPause = 1;

        /// <summary>
        /// The main program method
        /// </summary>
        /// <param name="args">
        /// The arguments
        /// </param>
        public static void Main(string[] args)
        {
            _enumerations = new List<IEnumerable<string>>
                                {
                                    new List<string>(),
                                    new NumericEnumerable()
                                };

            _downloadManager = new Thread(Download);
            _downloadManager.Start();
            Console.Read();
        }
        
        /// <summary>
        /// The thread call back
        /// </summary>
        /// <param name="currentSize">
        /// The current size.
        /// </param>
        /// <param name="totalSize">
        /// The total size.
        /// </param>
        private static void UpdateStatus(int currentSize, int totalSize)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(((100.00 / totalSize) * currentSize).ToString("N") + "% (" + (currentSize / 1048576M).ToString("F") + ")");
        }

        /// <summary>
        /// The download thread method
        /// </summary>
        private static void Download()
        {
            new DownloadManager(_path, _formatString, @"C:/temp/other/",  _enumerations, _downloadPause, UpdateStatus);
        }
    }
}
