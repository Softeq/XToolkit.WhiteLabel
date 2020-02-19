// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;

namespace Softeq.XToolkit.WhiteLabel.Model
{
    public class PagingModel<T>
    {
        public IList<T> Data { get; set; } = default!;
        public int Page { get; set; }
        public int TotalNumberOfPages { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public int PageSize { get; set; }
    }
}
