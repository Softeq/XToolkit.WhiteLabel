// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    /// <summary>
    ///    A model for representing data pagination.
    /// </summary>
    /// <typeparam name="T">The type of record.</typeparam>
    public class PagingModel<T>
    {
        /// <summary>
        ///     Gets or sets page data (list of records).
        /// </summary>
        public IList<T> Data { get; set; } = default!;

        /// <summary>
        ///     Gets or sets the index of the current page.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        ///     Gets or sets the length of the current page (records count).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     Gets or sets the info about the total number of pages.
        /// </summary>
        public int TotalNumberOfPages { get; set; }

        /// <summary>
        ///     Gets or sets the info about the total number of records.
        /// </summary>
        public int TotalNumberOfRecords { get; set; }
    }
}
