namespace Shared.DTOs.Common
{
    public class PaginationResult<T>
    {
        public PaginationResult(int totalCount, int pageIndex, int pageSize, IEnumerable<T> data)
        {
            TotalCount = totalCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
        }

        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

        public IEnumerable<T> Data { get; set; }
    }
}
