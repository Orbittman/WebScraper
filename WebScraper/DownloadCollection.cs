// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DownloadCollection.cs" company="Tim Barton">
//   Tim Barton.
// </copyright>
// <summary>
//   The download collection class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WebScraper
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The download collection class.
    /// </summary>
    public class DownloadCollection
        : IEnumerable<string>
    {
        #region Fields

        /// <summary>
        /// The enumerator.
        /// </summary>
        private readonly DownloadCollectionEnumerator _enumerator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadCollection"/> class.
        /// </summary>
        /// <param name="root">
        /// The root of the download path.
        /// </param>
        /// <param name="enumerations">
        /// The enumerations.
        /// </param>
        /// <param name="format">
        /// The string to use to format the output
        /// </param>
        public DownloadCollection(string root, IEnumerable<IEnumerable<string>> enumerations, string format = "")
        {
            _enumerator = new DownloadCollectionEnumerator(enumerations, format);
            this.RemotePath = root;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the local path.
        /// </summary>
        public string LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the collection specific format string.
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets the download root.
        /// </summary>
        public string RemotePath { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the pause between downloads.
        /// </summary>
        public int Pause { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<string> GetEnumerator()
        {
            return _enumerator;
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
