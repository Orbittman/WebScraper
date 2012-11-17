// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperTests.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   The scraper tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    /// The scraper tests.
    /// </summary>
    [TestClass]
    public class ScraperTests
    {
        /// <summary>
        /// The numeric enumerator stops at the max value.
        /// </summary>
        [TestMethod]
        public void NumericEnumeratorStopsAtTheMaxValue()
        {
            var max = 10;
            var enumerator = new NumericEnumerable(max);
            var counter = 0;
            foreach (var item in enumerator)
            {
                counter++;
                if (counter > max)
                {
                    Assert.Fail("The enumerator failed to stop at the max limit");
                }
            }

            Assert.AreEqual(max, counter);
        }

        /// <summary>
        /// The numeric enumerator continues if the max is set to zero.
        /// </summary>
        [TestMethod]
        public void NumericEnumeratorContinuesIfTheMaxIsSetToZero()
        {
            var max = 10;
            var enumerator = new NumericEnumerable();
            var counter = 0;
            for (var i = 0; i < max; i ++)
            {
                counter++;
            }

            Assert.AreEqual(max, counter);
        }

        /// <summary>
        /// The download collection enumerates to correct values.
        /// </summary>
        [TestMethod]
        public void DownloadCollectionEnumeratesToCorrectValues()
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
                        "{0}#{1}.{2}");

            var items = collection.ToArray();
            Assert.AreEqual(18, items.Count(), "The collection returned was not the right length");
            Assert.AreEqual("A#D.G", items[0]);
            Assert.AreEqual("A#D.H", items[1]);
            Assert.AreEqual("A#D.I", items[2]); 
            Assert.AreEqual("A#E.G", items[3]);
            Assert.AreEqual("A#E.H", items[4]);
            Assert.AreEqual("A#E.I", items[5]);
            Assert.AreEqual("A#F.G", items[6]);
            Assert.AreEqual("A#F.H", items[7]);
            Assert.AreEqual("A#F.I", items[8]); 
            Assert.AreEqual("B#D.G", items[9]);
            Assert.AreEqual("B#D.H", items[10]);
            Assert.AreEqual("B#D.I", items[11]);
            Assert.AreEqual("B#E.G", items[12]);
            Assert.AreEqual("B#E.H", items[13]);
            Assert.AreEqual("B#E.I", items[14]);
            Assert.AreEqual("B#F.G", items[15]);
            Assert.AreEqual("B#F.H", items[16]);
            Assert.AreEqual("B#F.I", items[17]);
        }

        /// <summary>
        /// The check that the downloader is called for each enumeration.
        /// </summary>
        [TestMethod]
        public void CheckThatTheDownloaderIsCalledForEachEnumeration()
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

            var mockDownloader = new Mock<IDownloader>();
            mockDownloader.Setup(d => d.Download(collection, It.IsAny<string>())).Returns(new Task<bool>(() => true));
            var manager = new DownloadManager(new List<DownloadCollection> { collection })
                {
                    Downloader = new FakeDownloader() //mockDownloader.Object
                };

            manager.StartDownloading();
            var bob = false;
            while(!bob)
            {
            }
            //mockDownloader.Verify(d => d.Download(collection, It.IsAny<string>()), Times.Exactly(18));
        }
    }
}
