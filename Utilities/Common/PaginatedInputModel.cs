using System.Collections.Generic;

namespace Utilities.Common
{
    public class PaginatedInputModel
    {
        public PaginatedInputModel()
        {
            SortingParams = new HashSet<SortingUtility.SortingParams>();
            FilterParam = new HashSet<FilterUtility.FilterParams>();
        }

        public IEnumerable<SortingUtility.SortingParams> SortingParams { set; get; }
        public IEnumerable<FilterUtility.FilterParams> FilterParam { get; set; }
        public IEnumerable<string> GroupingColumns { get; set; } = null;
        private int pageNumber = 1;
        public int PageNumber { get { return pageNumber; } set { if (value > 1) pageNumber = value; } }

        private int pageSize = 25;
        public int PageSize { get { return pageSize; } set { if (value > 1) pageSize = value; } }
    }
}