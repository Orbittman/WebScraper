using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebScraper.Tests
{
    [TestClass]
    public class UnitTest1
    {
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
    }
}
