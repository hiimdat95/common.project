using System.Collections.Generic;

namespace Utilities.Common
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> PageData { get; set; }

        public bool HasPreviousPage
        {
            get; set;
        }

        public bool HasNextPage
        {
            get; set;
        }
    }
}