using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Application.DTOs
{
    public class PaginationDto<T>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int TotalItems { get; set; }
        public int? TotalPages => PageSize.HasValue && PageSize > 0
            ? (int)Math.Ceiling((double)TotalItems / PageSize.Value)
            : null;
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
