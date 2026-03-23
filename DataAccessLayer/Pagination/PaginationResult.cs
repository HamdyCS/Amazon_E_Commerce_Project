using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Pagination
{
    public class PaginationResult<T> where T : class
    {
        public PaginationResult(IEnumerable<T> data, int totalCount, int pageNumber, int pageSize)
        {
            this.Data = data;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;

            NextPage = HasNextPage ? pageNumber + 1 : null;
            PreviousPage = HasPreviousPage ? pageNumber - 1 : null;
        }

        public IEnumerable<T> Data { get; }

        public int TotalCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int? NextPage {  get; }
        public int? PreviousPage { get; }


        //additional properties for convenience
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasNextPage => PageNumber < TotalPages;

        public bool HasPreviousPage => PageNumber > 1;

    }
}
