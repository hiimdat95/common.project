namespace ViewModels.Common
{
    public class PaginationRequestModel
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 9999;
    }
}