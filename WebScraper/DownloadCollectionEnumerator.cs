// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadCollectionEnumerator.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   Defines the DownloadCollectionEnumerator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The download collection enumerator.
    /// </summary>
    internal class DownloadCollectionEnumerator
        : IEnumerator<string>
    {
        /// <summary>
        /// The enumerations.
        /// </summary>
        private readonly IEnumerable<IEnumerator<string>> _enumerators;

        /// <summary>
        /// The format for the output
        /// </summary>
        private readonly string _format;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadCollectionEnumerator"/> class.
        /// </summary>
        /// <param name="enumerations">
        /// The enumerations.
        /// </param>
        /// <param name="format">
        /// The format for the output
        /// </param>
        public DownloadCollectionEnumerator(IEnumerable<IEnumerable<string>> enumerations, string format)
        {
            _enumerators = enumerations.Select(e => e.GetEnumerator()).ToList();
            _format = format;
        }
        
        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <returns>
        /// The element in the collection at the current position of the enumerator.
        /// </returns>
        public string Current { get; private set; }

        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        /// <returns>
        /// The current element in the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public bool MoveNext()
        {
            for (int i = _enumerators.Count() - 1; i >= 0; i--)
            {
                if (_enumerators.ElementAt(i).MoveNext())
                {
                    var currentValues = _enumerators.Select(
                        e =>
                            {
                                if (e.Current == null)
                                {
                                    e.MoveNext();
                                }

                                return e.Current as object;
                            }).ToArray();

                    Current = string.Format(_format, currentValues);
                    return true;
                }

                foreach (var enumerator in _enumerators.Skip(i))
                {
                    enumerator.Reset();
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception><filterpriority>2</filterpriority>
        public void Reset()
        {
            foreach (var enumerator in _enumerators)
            {
                enumerator.Reset();
            }
        }
    }
}
