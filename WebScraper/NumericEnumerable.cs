using System;
using System.Collections;
using System.Collections.Generic;

namespace WebScraper
{
    public class NumericEnumerable
        : IEnumerable<string>
    {
        #region Fields

        private IEnumerator<string> enumerator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref=" cref="NumericEnumerable"/> class
        /// </summary>
        /// <param name="max">The maximum value for the enumeration</param>
        public NumericEnumerable(int max = 0)
        {
            enumerator = new NumericEnumerator { Max = max };
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<string> GetEnumerator()
        {
            if (enumerator == null)
            {
                enumerator = new NumericEnumerator();
            }

            return enumerator;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
