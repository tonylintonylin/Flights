using System;
using System.Collections.Generic;

namespace Flights
{
    // Base viewmodel for paginated pages
    public abstract class PagedModel : BaseModel
    {
        // Default values 
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        public string Sort { get; set; } = "-Id";
        public int Filter { get; set; } = 0;
        public bool AdvancedFilter { get; set; } = false;

        public int TotalRows { get; set; }

        // Shorthand for Entity Framework Skip and Take methods
        public int Skip { get { return Math.Max(FirstRow - 1, 0); } }
        public int Take { get { return PageSize; } }

        // Pagination helpers 
        public int FirstRow { get { return Math.Min(Math.Max(1, ((Page - 1) * PageSize) + 1), TotalRows); } }
        public int LastRow { get { return Math.Min(FirstRow + PageSize - 1, TotalRows); } }
        public string Range { get { return FirstRow.ToString() + " - " + LastRow.ToString(); } }
        public int TotalPages { get { return (int)Math.Ceiling(((double)TotalRows) / PageSize); } }
        public bool HasPrevPage { get { return Page > 1; } }
        public bool HasNextPage { get { return Page < TotalPages; } }
        public string UnsignedSort { get { return Sort[0] == '-' ? Sort.Substring(1) : Sort; } }
        public string Order { get { return Sort[0] == '-' ? "Desc" : "Asc"; } }
        public string OrderBy { get { return UnsignedSort + " " + Order; } }
        public int NextPage { get { return HasNextPage ? Page + 1 : -1; } }
        public int PrevPage { get { return HasPrevPage ? Page - 1 : -1; } }
    }

    // Typed base viewmodel supporting a list of paginated items
    public abstract class PagedModel<T> : PagedModel
    {
        public List<T> Items { get; set; } = new List<T>();
    }
}
