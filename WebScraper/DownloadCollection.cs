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
    using System.Collections.Generic;

    /// <summary>
    /// The download collection class.
    /// </summary>
    public class DownloadCollection
    {
        #region Properties

        /// <summary>
        /// Gets or sets the enumerations.
        /// </summary>
        public IEnumerable<IEnumerable<string>> Enumerations { get; set; }

        #endregion
    }
}
