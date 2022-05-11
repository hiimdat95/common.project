using System.Collections.Generic;

namespace ViewModels.Common
{
    public class PaginationResult<T>
    {
        public IReadOnlyList<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
    }
}