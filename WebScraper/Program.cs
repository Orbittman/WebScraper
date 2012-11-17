// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   The delegate for handling enumeration recursion
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System;
    using System.Collections.Generic;

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
        /// <summary>
        /// The main program method
        /// </summary>
        /// <param name="args">
        /// The arguments
        /// </param>
        public static void Main(string[] args)
        {
            var collection =
                new DownloadCollection(
                    string.Empty,
                    new List<IEnumerable<string>>
                        {
                            new List<string> { "A", "B" }, 
                            new List<string> { "D", "E", "F" }, 
                            new List<string> { "G", "H", "I" }
                        },
                        "{0}{1}{2}");
            var collection2 = new DownloadCollection(
                string.Empty,
                new List<IEnumerable<string>>
                    {
                        new List<string> { "1", "2" },
                        new List<string> { "3", "4", "5" },
                        new List<string> { "6", "7", "8" }
                    },
                "{0}{1}{2}") { Pause = 0 };

            var manager = new DownloadManager(new List<DownloadCollection> { collection, collection2 })
            {
                Downloader = new FakeDownloader(),
                LoggingAction = a => Console.WriteLine(a)
            };

            manager.StartDownloading();
            Console.WriteLine("Done");
            Console.ReadKey();
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
    }
}
